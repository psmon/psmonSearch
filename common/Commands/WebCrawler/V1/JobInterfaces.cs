using System;
using Akka.Actor;
using Akka.Routing;
using common.Commands.WebCrawler.State;

namespace common.Commands.WebCrawler.V1
{
    public interface IStartJobV1 : IConsistentHashable
    {
        CrawlJob Job { get; }
        IActorRef Requestor { get; }
    }

    public interface IStatusUpdateV1
    {
        CrawlJob Job { get; }
        CrawlJobStats Stats { get; set; }
        DateTime StartTime { get; }
        DateTime? EndTime { get; set; }
        TimeSpan Elapsed { get; }
        JobStatus Status { get; set; }
    }

    public interface ISubscribeToJobV1
    {
        CrawlJob Job { get; }
        IActorRef Subscriber { get; }
    }

    public interface IUnsubscribeFromJobV1
    {
        CrawlJob Job { get; }
        IActorRef Subscriber { get; }
    }











}
