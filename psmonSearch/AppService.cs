using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Akka;
using Akka.Actor;
using Akka.Event;
using Akka.Configuration;
using Akka.Serialization;
using Akka.Routing;

using common.Actors;
using common.AkkaUtils;

using WebSocketSharp;

using System.Configuration;
using Microsoft.Owin.Hosting;

using psmonSearch.Actors;

namespace psmonSearch
{
    public class AppService
    {        
        static AkkaControler akkaCtr = new AkkaControler();

        public static void Start(string url)
        {
            var config = ConfigurationFactory.ParseString(@"
				
            ");

            //var url = ConfigurationManager.AppSettings.Get("ServiceURL");
            StartWebService(url);
            var actorSystem = GetAkkaCtr().StartAkkaSystem("webcrawler",null);
            var router = actorSystem.ActorOf(Props.Empty.WithRouter(FromConfig.Instance), "tasker");
            //GetAkkaCtr().CreateActor<CommandWebCrawlProcessor>("SearchMain", "commands");
            actorSystem.ActorOf(Props.Create(() => new CommandWebCrawlProcessor(router)),
                "commands");

        }

        public static void SystemDown()
        {
            akkaCtr.SystemDown();
        }

        public static void StartWebService(string url)
        {
            WebApp.Start<WebStartup>(url);
        }

        public static AkkaControler GetAkkaCtr()
        {
            return akkaCtr;
        }

    }
}
