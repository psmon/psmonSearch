using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka.Actor;

using common.Commands.WebCrawler.State;
using common.Commands.WebCrawler.V1;

namespace common.Data
{
    public class CheckDocuments
    {
        public CheckDocuments(IList<CrawlDocument> documents, IActorRef requestor, TimeSpan? estimatedCrawlTime)
        {
            EstimatedCrawlTime = estimatedCrawlTime;
            Requestor = requestor;
            Documents = documents;
        }

        public IList<CrawlDocument> Documents { get; private set; }

        public int HtmlDocs { get { return Documents.Count(x => !x.IsImage); } }

        public int Images { get { return Documents.Count(x => x.IsImage); } }

        /// <summary>
        /// Reference to the actor who should take on the cleared documents
        /// </summary>
        public IActorRef Requestor { get; private set; }

        /// <summary>
        /// The amount of time we think it'll take to crawl this document
        /// based on current workload.
        /// </summary>
        public TimeSpan? EstimatedCrawlTime { get; private set; }
    }

    public class ProcessDocuments
    {
        public ProcessDocuments(IList<CrawlDocument> documents, IActorRef assigned)
        {
            Assigned = assigned;
            Documents = documents;
        }

        public IList<CrawlDocument> Documents { get; private set; }

        public int HtmlDocs { get { return Documents.Count(x => !x.IsImage); } }

        public int Images { get { return Documents.Count(x => x.IsImage); } }

        /// <summary>
        /// Reference to the actor who should take on the cleared documents
        /// </summary>
        public IActorRef Assigned { get; private set; }
    }    

}
