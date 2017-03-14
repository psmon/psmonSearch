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

using common.Actors;
using common.AkkaUtils;

using Akka.Configuration;
using System.Configuration;

using smartCrawler.Actors;
using smartCrawler.Actors.Tracking;

using common.Commands.WebCrawler.V1;
using common.Commands.WebCrawler.State;

using HtmlAgilityPack;

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
            var config2 = ConfigurationFactory.ParseString(@"
                akka {
                  actor {
                    provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
                    serializers {
                                wire = ""Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion""
                          }
                    serialization-bindings {
                    ""System.Object"" = wire
                    }
                  }
  
                  remote {
                    log-remote-lifecycle-events = DEBUG
                    log-received-messages = on
    
                    helios.tcp {
                      transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
                      applied-adapters = []
                      transport-protocol = tcp
                      #will be populated with a dynamic host-name at runtime if left uncommented
                      #public-hostname = ""POPULATE STATIC IP HERE""
                      hostname = ""127.0.0.1""
                      port = 5000
                      maximum-frame-size = 256000b
                    }
                  }            

                  cluster {
                    #will inject this node as a self-seed node at run-time
                    seed-nodes = [""akka.tcp://webcrawler@127.0.0.1:4053""] #manually populate other seed nodes here, i.e. ""akka.tcp://lighthouse@127.0.0.1:4053"", ""akka.tcp://lighthouse@127.0.0.1:4044""
                    roles = [crawler]
                  }
                }
            ");
            var config = ConfigurationFactory.ParseString(@"
            akka {
	            actor {
		                provider = ""Akka.Cluster.ClusterActorRefProvider, Akka.Cluster""
                        serializers {
                            wire = ""Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion""
                        }
                        serialization-bindings {
                        ""System.Object"" = wire
                        }
		            deployment {                        
			            /api/broadcaster {
				            router = broadcast-group
				            routees.paths = [""/user/api""]
				            cluster {
						            enabled = on
						            allow-local-routees = on
						            use-role = tracker
				            }
			            }
			            /downloads/broadcaster {
				            router = broadcast-group
				            routees.paths = [""/user/downloads""]
				            cluster {
						            enabled = on
						            max-nr-of-instances-per-node = 1
						            allow-local-routees = on
						            use-role = tracker
				            }
			            }
			            ""/api/*/coordinators"" {
				            router = round-robin-pool
				            nr-of-instances = 10
				            cluster {
					            enabled = on
					            max-nr-of-instances-per-node = 2
					            allow-local-routees = off
					            use-role = crawler
				            }
			            }             
		            }
	            }
	            remote {
		            log-remote-lifecycle-events = DEBUG
		            log-received-messages = on
		            helios.tcp {
			            transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
			            applied-adapters = []
			            transport-protocol = tcp
			            #will be populated with a dynamic host-name at runtime if left uncommented
			            #public-hostname = ""POPULATE STATIC IP HERE""
			            hostname = ""127.0.0.1""
			            port = 5001
                        maximum-frame-size = 256000b
		            }
	            }            

	            cluster {
		            #will inject this node as a self-seed node at run-time
		            seed-nodes = [""akka.tcp://webcrawler@127.0.0.1:4053""] #manually populate other seed nodes here, i.e. ""akka.tcp://lighthouse@127.0.0.1:4053"", ""akka.tcp://lighthouse@127.0.0.1:4044""
		            roles = [""tracker""]
	            }
            }
            ");
            
            var actorSystem = GetAkkaCtr().StartAkkaSystem("webcrawler", config);
            ApiMaster = actorSystem.ActorOf(Props.Create(() => new ApiMaster()),
                "api");

            DownloadMaster = actorSystem.ActorOf(Props.Create(() => new DownloadsMaster()),
                "downloads");

            var ghettoConsoleActor = actorSystem.ActorOf(Props.Create(() => new SimpleActor()),
                "test");

            ActorSystem actorSystem2 = ActorSystem.Create("webcrawler", config2);



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
