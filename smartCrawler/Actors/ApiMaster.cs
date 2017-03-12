using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Akka.Routing;
using common.Data;
using common.Actors.IO;
using common.Commands.WebCrawler.V1;
using common.Commands.WebCrawler.State;
using smartCrawler.Actors;
using smartCrawler.Actors.Tracking;


namespace smartCrawler.Actors
{
    public class ApiMaster : ReceiveActor, IWithUnboundedStash
    {
        public const string MasterBroadcastName = "broadcaster";

        #region Messages

        public class FindRunningJob
        {
            public FindRunningJob(CrawlJob key)
            {
                Key = key;
            }

            public CrawlJob Key { get; private set; }
        }

        public class JobFound
        {
            public JobFound(CrawlJob key, IActorRef crawlMaster)
            {
                CrawlMaster = crawlMaster;
                Key = key;
            }

            public CrawlJob Key { get; private set; }

            public IActorRef CrawlMaster { get; private set; }
        }

        public class JobNotFound
        {
            public JobNotFound(CrawlJob key)
            {
                Key = key;
            }

            public CrawlJob Key { get; private set; }
        }

        #endregion

        protected IActorRef ApiBroadcaster;
        protected IStartJobV1 JobToStart;
        protected int OutstandingAcknowledgements;

        public ApiMaster()
        {
            Ready();
        }

        protected override void PreStart()
        {
            ApiBroadcaster = Context.Child(MasterBroadcastName).Equals(ActorRefs.Nobody)
                ? Context.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), MasterBroadcastName)
                : Context.Child(MasterBroadcastName);
        }

        protected override void PreRestart(Exception reason, object message)
        {
            /* Don't kill the children */
            PostStop();
        }

        public IStash Stash { get; set; }

        private void Ready()
        {
            Receive<IStartJobV1>(start =>
            {
                JobToStart = start;

                // contact all of our peers and see if this job is already running
                ApiBroadcaster.Tell(new FindRunningJob(start.Job));

                // and see if we've done this job before
                Context.ActorSelection("/user/downloads").Tell(new DownloadsMaster.RequestDownloadTrackerFor(start.Job, Self));

                Become(SearchingForJob);
                var members = ApiBroadcaster.Ask<Routees>(new GetRoutees()).Result.Members.ToList();
                OutstandingAcknowledgements = members.Count();
                Context.SetReceiveTimeout(TimeSpan.FromSeconds(3.0));
            });

            Receive<FindRunningJob>(job => HandleFindRunningJob(job));
        }

        private void SearchingForJob()
        {
            //not able to start more jobs right now
            Receive<IStartJobV1>(s => Stash.Stash());

            Receive<FindRunningJob>(job => HandleFindRunningJob(job));

            Receive<JobFound>(jobFound =>
            {
                if (jobFound.Key.Equals(JobToStart.Job))
                {
                    jobFound.CrawlMaster.Tell(new SubscribeToJob(JobToStart.Job, JobToStart.Requestor));
                    BecomeReady();
                }
            });

            //treat as a "not found" message
            Receive<ReceiveTimeout>(timeout => ApiBroadcaster.Tell(new JobNotFound(JobToStart.Job)));

            Receive<JobNotFound>(notFound =>
            {
                if (notFound.Key.Equals(JobToStart.Job))
                {
                    OutstandingAcknowledgements--;
                    if (OutstandingAcknowledgements <= 0)
                    {
                        //need to create the job now
                        var crawlMaster = Context.ActorOf(Props.Create(() => new CrawlMaster(JobToStart.Job)),
                            JobToStart.Job.Root.ToActorName());
                        crawlMaster.Tell(JobToStart);
                        ApiBroadcaster.Tell(new JobFound(JobToStart.Job, crawlMaster));
                    }
                }
            });
        }

        private void BecomeReady()
        {
            Become(Ready);
            Context.SetReceiveTimeout(null);
            Stash.UnstashAll();
        }

        private void HandleFindRunningJob(FindRunningJob job)
        {
            var haveChild = Context.Child(job.Key.Root.ToActorName());

            //found a running job already
            if (!haveChild.Equals(ActorRefs.Nobody))
            {
                ApiBroadcaster.Tell(new JobFound(job.Key, haveChild));
                return;
            }

            ApiBroadcaster.Tell(new JobNotFound(job.Key));
        }
    }
}
