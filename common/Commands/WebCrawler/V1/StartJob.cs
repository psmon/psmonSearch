﻿using Akka.Actor;
using Akka.Routing;
using common.Commands.WebCrawler.State;

namespace common.Commands.WebCrawler.V1
{
    public class StartJob : IStartJobV1
    {
        public StartJob(CrawlJob job, IActorRef requestor)
        {
            Requestor = requestor;
            Job = job;
        }

        public CrawlJob Job { get; private set; }

        public IActorRef Requestor { get; private set; }
        public object ConsistentHashKey { get { return Job.Root.OriginalString; } }
    }

    /// <summary>
    /// Kill a running <see cref="CrawlJob"/>
    /// </summary>
    public class StopJob
    {
        public StopJob(CrawlJob job, IActorRef requestor)
        {
            Requestor = requestor;
            Job = job;
        }

        public CrawlJob Job { get; private set; }

        public IActorRef Requestor { get; private set; }
    }
}
