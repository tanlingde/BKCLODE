using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BK.Cloud.Task
{
     public class config
    {
        public string appid { get; set; }
        public string appkey { get; set; }
        public string callUrl { get; set; }
        public string token { get; set; }
        public string tokenTime { get; set; }
        public string platformIP { get; set; }
        public int port { get; set; }
        public string p12cert { get; set; }
        public string certpassword { get; set; }
        public string refreshToken { get; set; }
    }
}
