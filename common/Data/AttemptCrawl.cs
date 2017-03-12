using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Data
{
    public class AttemptWebCrawl
    {
        public AttemptWebCrawl(string rawStr)
        {
            RawStr = rawStr;
        }

        public string RawStr { get; private set; }
    }

    public class BadWebCrawlAttempt
    {
        public BadWebCrawlAttempt(string rawStr, string message)
        {
            Message = message;
            RawStr = rawStr;
        }

        public string RawStr { get; private set; }

        public string Message { get; private set; }
    }
}
