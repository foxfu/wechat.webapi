using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WxWebApi.Models
{
    public class Conversation
    {
        public string Signature { get; set; }
        public string TimeStamp { get; set; }
        public string Nonce { get; set; }
        public string Echostr { get; set; }
    }
}
