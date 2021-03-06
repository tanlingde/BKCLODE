using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elasticsearch.Net;
using Fireasy.Common.Extensions;
using Nest;
using BK.Cloud.Tools;

namespace BK.Cloud.Logic
{
    public class ElasticSearchHelper
    {
        public static readonly ElasticSearchHelper Intance = new ElasticSearchHelper();
        private ElasticClient Client;
        private ElasticSearchHelper()
        {
            //var node = new Uri("http://localhost:9200");
            string host = CommonHelper.GetAppConfig("elasticurl");
            host = host ?? "192.168.1.9";
            string[] splits = host.Split(':');
            int port = splits.Length > 1 ? int.Parse(splits[1]) : 9200;
            string hostname = splits[0];
            var settings = new ConnectionSettings(new Uri("http://"+host));
            Client = new ElasticClient(settings);

        }

        public ElasticClient MyClient
        {
            get
            {
                return Client;
            }
        }

        /// <summary>
        /// 数据索引
        /// </summary>       
        /// <param name="indexName">索引名称</param>
        /// <param name="indexType">索引类型</param>
        /// <param name="id">索引文档id，不能重复,如果重复则覆盖原先的</param>
        /// <param name="document">要索引的文档,json格式或对象</param>
        /// <returns></returns>
        public IIndexResponse Index(string indexName, string indexType, string id, object document)
        {
            return Client.Index(document, i => i
              .Index(indexName)
              .Type(indexType)
              .Id(id)
              .Refresh()
              .Ttl("1m")
            );
        }

        /// <summary>
        /// 模糊查找匹配
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="conditions"></param>
        /// <param name="indexName"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="totalcnt"></param>
        /// <param name="ordercol"></param>
        /// <param name="isdesc"></param>
        /// <returns></returns>
        public List<T> SearchResultLike1<T>(Dictionary<string, string> conditions, string indexName, int page, int rows, out int totalcnt,string ordercol,bool isdesc) where T : class
        {
            totalcnt = 0;
            return null;
            //QueryContainer query = new QueryStringQuery() { DefaultOperator = Operator.And };
            //foreach (var condition in conditions.Keys)
            //{
            //    string keyword = condition;
            //    string wholeKeyword = keyword;
            //    keyword = String.Format("*{0}*", keyword);

            //    QueryContainer query1 = new QueryStringQuery()
            //        {
            //            Query = keyword,
            //            Fields = new PropertyPathMarker[] { conditions[condition] },
            //            DefaultOperator = Operator.And
            //        };
            //    if (!String.IsNullOrEmpty(wholeKeyword))
            //    {
            //        QueryContainer wholeWordQuery = new QueryStringQuery()
            //        {
            //            Query = wholeKeyword,
            //            Fields = new PropertyPathMarker[] { conditions[condition] }
            //        };
            //        query1 = query1 || wholeWordQuery;
            //    }
            //    query = query && query1;
            //}

            //var searchResults = Client.Search<T>(s => s
            //    .Index(indexName)
            //    .Query(query)
            //    .Sort(o => isdesc ? o.OnField("ordercol").Descending() : o.OnField("ordercol").Ascending())
            //    .From((page - 1) * rows)
            //    .Size(rows)
            //);
            //totalcnt = searchResults.Hits.Count();
            //return searchResults.Documents.ToList();
        }

        //全文检索，单个字段或者多字段 或关系
        //字段intro 包含词组key中的任意一个单词
        //
        //public personList Search<person>(string indexName, string indexType, string key, int from, int size)
        //{
        //    string cmd = new SearchCommand(indexName, indexType);
        //    string query = new QueryBuilder<person>()
        //        //1 查询
        //        .Query(b =>
        //                    b.Bool(m =>
        //                        //并且关系
        //                        m.Must(t =>

        //                            //分词的最小单位或关系查询
        //                           t.QueryString(t1 => t1.DefaultField("intro").Query(key))
        //                            //.QueryString(t1 => t1.DefaultField("name").Query(key))
        //                            // t .Terms(t2=>t2.Field("intro").Values("研究","方鸿渐"))
        //                            //范围查询
        //                            // .Range(r =>  r.Field("age").From("100").To("200") )  
        //                             )
        //                          )
        //                        )
        //        //分页
        //         .From(from)
        //         .Size(size)
        //        //排序
        //        // .Sort(c => c.Field("age", SortDirection.desc))
        //        //添加高亮
        //          .Highlight(h => h
        //              .PreTags("<b>")
        //              .PostTags("</b>")
        //              .Fields(
        //                     f => f.FieldName("intro").Order(HighlightOrder.score),
        //                     f => f.FieldName("_all")
        //                     )
        //             )
        //            .Build();


        //    string result = Client.Post(cmd, query);
        //    var serializer = new JsonNetSerializer();
        //    var list = serializer.ToSearchResult<Supernova.Webapi.DbHelper.person>(result);
        //    personList datalist = new personList();
        //    datalist.hits = list.hits.total;
        //    datalist.took = list.took;
        //    var personList = list.hits.hits.Select(c => new Supernova.Webapi.DbHelper.person()
        //    {
        //        id = c._source.id,
        //        age = c._source.age,
        //        birthday = c._source.birthday,
        //        intro = string.Join("", c.highlight["intro"]), //高亮显示的内容，一条记录中出现了几次
        //        name = c._source.name,
        //        sex = c._source.sex,

        //    });
        //    datalist.list.AddRange(personList);
        //    return datalist;


        //}

        //全文检索，多字段 并关系
        //字段intro 或者name 包含词组key
        //public personList SearchFullFileds<person>(string indexName, string indexType, string key, int from, int size)
        //{
        //    MustQuery<person> mustNameQueryKeys = new MustQuery<person>();
        //    MustQuery<person> mustIntroQueryKeys = new MustQuery<person>();
        //    var arrKeys = GetIKTokenFromStr(key);
        //    foreach (var item in arrKeys)
        //    {
        //        mustNameQueryKeys = mustNameQueryKeys.Term(t3 => t3.Field("name").Value(item)) as MustQuery<person>;
        //        mustIntroQueryKeys = mustIntroQueryKeys.Term(t3 => t3.Field("intro").Value(item)) as MustQuery<person>;
        //    }

        //    string cmd = new SearchCommand(indexName, indexType);
        //    string query = new QueryBuilder<person>()
        //        //1 查询
        //        .Query(b =>
        //                    b.Bool(m =>
        //                        m.Should(t =>
        //                                 t.Bool(m1 =>
        //                                             m1.Must(
        //                                                     t2 =>
        //                                                         //t2.Term(t3=>t3.Field("name").Value("研究"))
        //                                                         //   .Term(t3=>t3.Field("name").Value("方鸿渐"))  
        //                                                         mustNameQueryKeys
        //                                                     )
        //                                        )
        //                               )
        //                       .Should(t =>
        //                                 t.Bool(m1 =>
        //                                             m1.Must(t2 =>
        //                                                 //t2.Term(t3 => t3.Field("intro").Value("研究"))
        //                                                 //.Term(t3 => t3.Field("intro").Value("方鸿渐"))  
        //                                                             mustIntroQueryKeys
        //                                                    )
        //                                        )
        //                              )
        //                          )
        //                )
        //        //分页
        //         .From(from)
        //         .Size(size)
        //        //排序
        //        // .Sort(c => c.Field("age", SortDirection.desc))
        //        //添加高亮
        //          .Highlight(h => h
        //              .PreTags("<b>")
        //              .PostTags("</b>")
        //              .Fields(
        //                     f => f.FieldName("intro").Order(HighlightOrder.score),
        //                      f => f.FieldName("name").Order(HighlightOrder.score)
        //                     )
        //             )
        //            .Build();


        //    string result = Client.Post(cmd, query);
        //    var serializer = new JsonNetSerializer();
        //    var list = serializer.ToSearchResult<Supernova.Webapi.DbHelper.person>(result);
        //    personList datalist = new personList();
        //    datalist.hits = list.hits.total;
        //    datalist.took = list.took;
        //    var personList = list.hits.hits.Select(c => new Supernova.Webapi.DbHelper.person()
        //    {
        //        id = c._source.id,
        //        age = c._source.age,
        //        birthday = c._source.birthday,
        //        intro = c.highlight == null || !c.highlight.Keys.Contains("intro") ? c._source.intro : string.Join("", c.highlight["intro"]), //高亮显示的内容，一条记录中出现了几次
        //        name = c.highlight == null || !c.highlight.Keys.Contains("name") ? c._source.name : string.Join("", c.highlight["name"]),
        //        sex = c._source.sex

        //    });
        //    datalist.list.AddRange(personList);
        //    return datalist;


        //}

        //全文检索，多字段 并关系
        //搜索age在100到200之间，并且字段intro 或者name 包含词组key
        //public personList SearchFullFiledss<person>(string indexName, string indexType, string key, int from, int size)
        //{
        //    MustQuery<person> mustNameQueryKeys = new MustQuery<person>();
        //    MustQuery<person> mustIntroQueryKeys = new MustQuery<person>();
        //    var arrKeys = GetIKTokenFromStr(key);
        //    foreach (var item in arrKeys)
        //    {
        //        mustNameQueryKeys = mustNameQueryKeys.Term(t3 => t3.Field("name").Value(item)) as MustQuery<person>;
        //        mustIntroQueryKeys = mustIntroQueryKeys.Term(t3 => t3.Field("intro").Value(item)) as MustQuery<person>;
        //    }

        //    string cmd = new SearchCommand(indexName, indexType);
        //    string query = new QueryBuilder<person>()
        //        //1 查询
        //        .Query(b =>
        //                    b.Bool(m =>
        //                        m.Must(t =>
        //                                  t.Range(r => r.Field("age").From("1").To("500"))
        //                                  .Bool(ms =>
        //                                            ms.Should(ts =>
        //                                                     ts.Bool(m1 =>
        //                                                                 m1.Must(
        //                                                                         t2 =>
        //                                                                             //t2.Term(t3=>t3.Field("name").Value("研究"))
        //                                                                             //   .Term(t3=>t3.Field("name").Value("方鸿渐"))  
        //                                                                             //
        //                                                                              mustNameQueryKeys
        //                                                                         )
        //                                                            )
        //                                                   )
        //                                           .Should(ts =>
        //                                                     ts.Bool(m1 =>
        //                                                                 m1.Must(t2 =>
        //                                                                     //t2.Term(t3 => t3.Field("intro").Value("研究"))
        //                                                                     //.Term(t3 => t3.Field("intro").Value("方鸿渐"))  

        //                                                                                //
        //                                                                                mustIntroQueryKeys
        //                                                                        )
        //                                                            )
        //                                                  )
        //                                              )
        //                                                )
        //                                              )
        //               )
        //        //分页
        //         .From(from)
        //         .Size(size)
        //        //排序
        //        // .Sort(c => c.Field("age", SortDirection.desc))
        //        //添加高亮
        //          .Highlight(h => h
        //              .PreTags("<b>")
        //              .PostTags("</b>")
        //              .Fields(
        //                     f => f.FieldName("intro").Order(HighlightOrder.score),
        //                      f => f.FieldName("name").Order(HighlightOrder.score)
        //                     )
        //             )
        //            .Build();


        //    string result = Client.Post(cmd, query);
        //    var serializer = new JsonNetSerializer();
        //    var list = serializer.ToSearchResult<Supernova.Webapi.DbHelper.person>(result);
        //    personList datalist = new personList();
        //    datalist.hits = list.hits.total;
        //    datalist.took = list.took;
        //    var personList = list.hits.hits.Select(c => new Supernova.Webapi.DbHelper.person()
        //    {
        //        id = c._source.id,
        //        age = c._source.age,
        //        birthday = c._source.birthday,
        //        intro = c.highlight == null || !c.highlight.Keys.Contains("intro") ? c._source.intro : string.Join("", c.highlight["intro"]), //高亮显示的内容，一条记录中出现了几次
        //        name = c.highlight == null || !c.highlight.Keys.Contains("name") ? c._source.name : string.Join("", c.highlight["name"]),
        //        sex = c._source.sex

        //    });
        //    datalist.list.AddRange(personList);
        //    return datalist;


        //}
        //分词映射
        //private static string BuildCompanyMapping()
        //{
        //    return new MapBuilder<person>()
        //        .RootObject(typeName: "person",
        //                    map: r => r
        //            .All(a => a.Enabled(false))
        //            .Dynamic(false)
        //            .Properties(pr => pr
        //                .String(person => person.name, f => f.Analyzer(DefaultAnalyzers.standard).Boost(2))
        //                .String(person => person.intro, f => f.Analyzer("ik"))
        //                )
        //      )
        //      .BuildBeautified();
        //}

        //将语句用ik分词，返回分词结果的集合
        //private List<string> GetIKTokenFromStr(string key)
        //{
        //    string s = "/db_test/_analyze?analyzer=ik";
        //    var result = Client.Post(s, "{" + key + "}");
        //    var serializer = new JsonNetSerializer();
        //    var list = serializer.Deserialize(result, typeof(ik)) as ik;
        //    return list.tokens.Select(c => c.token).ToList();
        //}


    }
}
