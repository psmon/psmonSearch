using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Helios.Util.TimedOps;

namespace smartCrawler.State
{
    public class CrawlStatus
    {
        public bool IsComplete { get; private set; }

        public Deadline Timeout { get; private set; }

        public bool CanProcess
        {
            get { return !IsComplete && (Timeout == null || Timeout.IsOverdue); }
        }

        public IActorRef Owner { get; private set; }

        public static CrawlStatus StartCrawl(IActorRef owner, TimeSpan crawlTime)
        {
            var crawl = new CrawlStatus();
            crawl.TryClaim(owner, crawlTime);
            return crawl;
        }

        public CrawlStatus MarkAsComplete()
        {
            IsComplete = true;
            Timeout = null; //let it get GC'ed
            Owner = null; //doesn't matter
            return this;
        }

        public bool TryClaim(IActorRef newOwner, TimeSpan crawlTime)
        {
            if (CanProcess)
            {
                Timeout = Deadline.Now + crawlTime;
                Owner = newOwner;
                return true;
            }
            return false;
        }
    }
}
