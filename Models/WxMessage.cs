using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace WxWebApi.Models
{
    [Serializable, XmlRoot("xml")]
    public class WxMessage
    {
        [XmlElement]
        public string ToUserName { get; set; }
        [XmlElement]
        public string FromUserName { get; set; }
        [XmlElement]
        public string CreateTime { get; set; }
        [XmlElement]
        public string MsgType { get; set; }
        [XmlElement]
        public string Content { get; set; }
    }
}
