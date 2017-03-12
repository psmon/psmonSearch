using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace common.Commands.WebCrawler.State
{
    public class JobStatusMessage
    {
        public JobStatusMessage(CrawlJob job, string documentTitle, string message)
        {
            Message = message;
            DocumentTitle = documentTitle;
            Job = job;
        }

        public CrawlJob Job { get; private set; }

        public string DocumentTitle { get; private set; }

        public string Message { get; private set; }

        public override string ToString()
        {
            return string.Format("[{0}][{1}][{2}]", Job, DocumentTitle, Message);
        }
    }
}
