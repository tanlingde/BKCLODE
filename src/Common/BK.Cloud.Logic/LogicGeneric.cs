using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BK.Cloud.Logic.Interface;
using Fireasy.Data;
using BK.Cloud.Model.Data.Model;
using System.Data;
using BK.Cloud.Tools;
using Fireasy.Common.Serialization;
using System.Globalization;
using Fireasy.Common.Extensions;
using BK.Cloud.Model.Customer;
using Fireasy.Data.Extensions;
using Nest;
using System.Diagnostics;
using System.Web;
using Elasticsearch.Net;
using Fireasy.Common.Logging;
using ServiceStack.Redis;
using ServiceStack.Text;
using JsonSerializer = ServiceStack.Text.JsonSerializer;
using Fireasy.Data.Syntax;
using Newtonsoft.Json;
using ServiceStack;

namespace BK.Cloud.Logic
{
    public class LogicGeneric : BusQuery, ILogicGeneric
    {
        //public static readonly LogicGeneric Instance = new LogicGeneric();

    }
}
