using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Nancy;
using Newtonsoft.Json;
using System.Configuration;
using common.Data;
using System.Dynamic;
using Newtonsoft.Json.Linq;
#pragma warning disable 1998

namespace psmonSearch.controller
{
    public class BaseController : NancyModule
    {
        protected string parentSpace = "";

        public BaseController()
        {
            After += async (ctx, ct) =>
            {
                // Modify ctx.Response
                if (ctx.Response.ContentType == "text/html")
                    ctx.Response.ContentType = "text/html; charset=utf-8";
            };

            Get["", true] = async (parameters, ct) => {                
                dynamic dataModel = new DynamicDic();
                dataModel.title = "WellCome PsmonSearch";                
                return View["public/index.html", dataModel];
            };

        }

        public string GetBodyString()
        {
            var body = this.Request.Body;
            int length = (int)body.Length; // this is a dynamic variable
            byte[] data = new byte[length];
            body.Read(data, 0, length);
            string dataStr = System.Text.Encoding.UTF8.GetString(data);
            return dataStr;
        }

        public Dictionary<string, object> GetPayLoad()
        {
            JObject jsonObj = JObject.Parse(GetBodyString());
            
            Dictionary<string, object> dictObj = jsonObj.ToObject<Dictionary<string, object>>();

            return dictObj;
        }

        public string GetUserIP()
        {
            return Request.UserHostAddress;
        }



    }
}
