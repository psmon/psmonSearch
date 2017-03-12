using Akka.Actor;
using Akka.Routing;
using common.Commands.WebCrawler.State;

namespace common.Commands.WebCrawler.V1
{
    public class SubscribeToJob : ISubscribeToJobV1
    {
        public SubscribeToJob(CrawlJob job, IActorRef subscriber)
        {
            Subscriber = subscriber;
            Job = job;
        }

        public CrawlJob Job { get; private set; }

        public IActorRef Subscriber { get; private set; }
    }
}
