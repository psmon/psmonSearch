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

namespace psmonSearch.controller
{
    public class SearchController : BaseController
    {
        public SearchController()
        {
            Post["api/cmd", true] = async (parameters, ct) =>
            {
                
                var payload = GetPayLoad();
                object command;

                string resultValue ="bad request";
                if(payload.TryGetValue("command", out command))
                    resultValue = TestLib.TestCommand(command as string);
                
                await Task.Delay(0);
                return resultValue;
            };
        }
    }
}
