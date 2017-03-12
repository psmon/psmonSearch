using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Routing;
using common.Data;

using common.Commands.WebCrawler.V1;
using common.Commands.WebCrawler.State;

namespace smartCrawler.Actors
{
    public interface ICommandProcessor
    {
        //Todo: 공통 커멘드 정의..        
    }

    public class CommandWebCrawlProcessor : ReceiveActor, ICommandProcessor
    {
        protected readonly IActorRef CommandRouter;

        public CommandWebCrawlProcessor(IActorRef commandRouter)
        {
            CommandRouter = commandRouter;
            Receives();
        }

        private void Receives()
        {
            Receive<AttemptWebCrawl>(attempt =>
            {
                if (Uri.IsWellFormedUriString(attempt.RawStr, UriKind.Absolute))
                {
                    var startJob = new StartJob(new CrawlJob(new Uri(attempt.RawStr, UriKind.Absolute), true), Sender);
                    CommandRouter.Tell(startJob);
                    /*  Todo: 작업상황 모든 라우터에게 실시간으로 알려주기...
                    CommandRouter.Ask<Routees>(new GetRoutees()).ContinueWith(tr =>
                    {
                        var grrr =
                            new SignalRActor.DebugCluster(string.Format("{0} has {1} routees: {2}", CommandRouter,
                                tr.Result.Members.Count(),
                                string.Join(",",
                                    tr.Result.Members.Select(
                                        y => y.ToString()))));

                        return grrr;
                    }).PipeTo(Sender);*/
                    Sender.Tell(startJob);
                }
                else
                {
                    Sender.Tell(new BadWebCrawlAttempt(attempt.RawStr, "Not an absolute URI"));
                }
            });
        }

    }
}
