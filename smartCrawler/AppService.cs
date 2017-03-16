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
using Akka.Configuration;

using common.Actors;
using common.AkkaUtils;

using System.Configuration;

using smartCrawler.Actors;
using smartCrawler.Actors.Tracking;

using common.Commands.WebCrawler.V1;
using common.Commands.WebCrawler.State;

using HtmlAgilityPack;

using System.Configuration;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using ConfigurationException = Akka.Configuration.ConfigurationException;



namespace smartCrawler
{
    public static class AppService
    {
        static AkkaControler akkaCtr = new AkkaControler();
        static IActorRef ApiMaster;
        static IActorRef DownloadMaster;

        public static void Test()
        {
            var htmlString = "<meta property='og:title' content='test'>";
            var doc = new HtmlDocument();
            doc.LoadHtml(htmlString);
            var titleNode = doc.DocumentNode.SelectNodes("//meta[@property='og:title']");
            var descripttionNode = doc.DocumentNode.SelectNodes("//meta[@property='og:description']");
            var postListNode = doc.DocumentNode.SelectNodes("//div[@id='postListBody']//p");
            var postListNode2 = doc.DocumentNode.SelectNodes("//div[@id='postListBody']//span");

            if (titleNode != null)
            {
                
                foreach(HtmlNode html in titleNode)
                {
                    string str = html.Attributes["content"].Value;
                    int a = 99;

                }

            }
        }

        public static void Start()
        {
            ActorSystem actorSystem2 = ActorSystem.Create("webcrawler");

            var section = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka2");
            var clusterConfig = section.AkkaConfig;
            var actorSystem = GetAkkaCtr().StartAkkaSystem("webcrawler", clusterConfig);
            ApiMaster = actorSystem.ActorOf(Props.Create(() => new ApiMaster()),
                "api");

            DownloadMaster = actorSystem.ActorOf(Props.Create(() => new DownloadsMaster()),
                "downloads");
            
            //ApiMaster.Tell(new StartJob(new CrawlJob(new Uri("http://www.rottentomatoes.com/", UriKind.Absolute), true), ghettoConsoleActor));
        }

        public static void SystemDown()
        {
            akkaCtr.SystemDown();
        }
        
        public static AkkaControler GetAkkaCtr()
        {
            return akkaCtr;
        }
    }
}
