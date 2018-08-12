using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using WxWebApi.Models;

namespace WxWebApi.Controllers
{
    [Route("api/[controller]")]
    public class AiController : Controller
    {
        private string _token;
        private readonly ILogger _logger;

        public AiController(ILogger<AiController> logger)
        {
            _token = "maxtoken";
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        //[Produces("application/json")]//Restrict to json response no matter the accept header in request
        public IActionResult Get(Conversation con)
        {
            if (CheckSignature(con))
            {
                return Ok(con.Echostr);
            }
            return NotFound("Not found");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        [Produces("text/xml")]
        public IActionResult Post([FromBody]WxMessage message)
        {
            try
            {
                _logger.LogWarning($"Get message data - Type:{message.MsgType}|From:{message.FromUserName}|Content:{message.Content}");
                //<xml>
                //    <ToUserName><![CDATA[gh_3c9f22cb4c1f]]></ToUserName>
                //    <FromUserName><![CDATA[ov96W1HsVW12OoVCouvw92sQNMFc]]></FromUserName>
                //    <CreateTime>1530606235</CreateTime>
                //    <MsgType><![CDATA[text]]></MsgType>
                //    <Content><![CDATA[一]]></Content>
                //    <MsgId>6573903722800677892</MsgId>
                //</xml>
                WxMessage reply = new WxMessage()
                {
                    FromUserName = message.ToUserName,
                    ToUserName = message.FromUserName,
                    CreateTime = message.CreateTime,
                    Content = "你好呀",
                    MsgType = message.MsgType
                };
                
                _logger.LogWarning($"Reply message data - Type:{reply.MsgType}|From:{reply.ToUserName}|Content:{reply.Content}");

                return Ok(reply);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return NotFound();
            }
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        

        private bool CheckSignature(Conversation con)
        {
            //echostring = 6146405371800967166
            //nonce = 2443149446
            //timestamp = 1530548526
            //signature = 5f862912dbcae362c90357bcd9ef2b8721f0c171

            //Test
            //con.TimeStamp = "1530548526";
            //con.Nonce = "2443149446";
            //con.Signature = "5f862912dbcae362c90357bcd9ef2b8721f0c171";

            string[] ArrTmp = { _token, con.TimeStamp, con.Nonce };
            Array.Sort(ArrTmp);   //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            var sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] buffer = Encoding.ASCII.GetBytes(tmpStr);
            byte[] output = sha1.ComputeHash(buffer);
            tmpStr = HexStringFromBytes(output);
            return (tmpStr == con.Signature);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        }
    }
}
