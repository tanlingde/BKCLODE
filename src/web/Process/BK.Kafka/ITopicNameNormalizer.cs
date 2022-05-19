using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Kafka
{
    /// <summary>
    /// 提供对主题名称标准化的接口。
    /// </summary>
    public interface ITopicNameNormalizer
    {
        /// <summary>
        /// 标准化主题名称。
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        string NormalizeName(string topicName);
    }
}
