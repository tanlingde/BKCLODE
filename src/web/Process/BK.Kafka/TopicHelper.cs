using Fireasy.Common.Extensions;
using System;
using System.Linq;


namespace BK.Kafka
{
    /// <summary>
    /// 用于标识主题类型的名称。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TopicAttribute : Attribute
    {
        /// <summary>
        /// 初始化 <see cref="TopicAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="name">主题名称。</param>
        public TopicAttribute(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 获取或设置主题名称。
        /// </summary>
        public string Name { get; set; }
    }

    public static class TopicHelper
    {
        /// <summary>
        /// 获取主题类型的主题名称。
        /// </summary>
        /// <param name="subjectType">主题类型。</param>
        /// <returns></returns>
        public static string GetTopicName(Type subjectType)
        {
            if (subjectType.IsDefined<TopicAttribute>())
            {
                var attr = subjectType.GetCustomAttributes<TopicAttribute>().FirstOrDefault();
                return attr?.Name;
            }

            return subjectType.FullName;
        }

        /// <summary>
        /// 尝试通过 <see cref="ITopicNameNormalizer"/> 来标准化主题名称。
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="topicName"></param>
        /// <returns></returns>
        public static string GetTopicName(this IServiceProvider serviceProvider, string topicName)
        {
            var normalizer = serviceProvider.TryGetService<ITopicNameNormalizer>();
            return normalizer != null ? normalizer.NormalizeName(topicName) : topicName;
        }

        /// <summary>
        /// 尝试从 <see cref="IServiceProvider"/> 实例中获取指定的服务，如果没有则使用 <paramref name="creator"/> 函数创建一个服务实例。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceProvier"></param>
        /// <param name="creator"></param>
        /// <returns></returns>
        public static T TryGetService<T>(this IServiceProvider serviceProvier, Func<T> creator = null)
        {
            if (serviceProvier != null)
            {
                try
                {
                    var service = serviceProvier.GetService(typeof(T));
                    if (service != null)
                    {
                        return (T)service;
                    }
                }
                catch (ObjectDisposedException)
                {
                    //Tracer.Error($"{serviceProvier} is disposed");
                }
            }

            return creator == null ? default : creator();
        }
    }
}
