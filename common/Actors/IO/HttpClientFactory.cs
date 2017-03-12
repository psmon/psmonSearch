using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace common.Actors.IO
{
    public static class HttpClientFactory
    {
        public static HttpClient GetClient()
        {
            return new HttpClient();
        }
    }
}
