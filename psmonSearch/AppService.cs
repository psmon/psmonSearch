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
					        /tasker {
							router = consistent-hashing-group
                            routees.paths = [""/user/api""]
									virtual-nodes-factor = 8
									cluster {
											enabled = on
											max-nr-of-instances-per-node = 2
											allow-local-routees = off
											use-role = tracker
									}
								}                
							}
						}
                        remote {
							log-remote-lifecycle-events = DEBUG
							helios.tcp {
								transport-class = ""Akka.Remote.Transport.Helios.HeliosTcpTransport, Akka.Remote""
								applied-adapters = []
								transport-protocol = tcp
								#will be populated with a dynamic host-name at runtime if left uncommented
								#public-hostname = ""POPULATE STATIC IP HERE""
								hostname = ""127.0.0.1""
								port = 4053
							}
						}            

						cluster {
							#will inject this node as a self-seed node at run-time
							seed-nodes = [""akka.tcp://webcrawler@127.0.0.1:4053""] #manually populate other seed nodes here, i.e. ""akka.tcp://lighthouse@127.0.0.1:4053"", ""akka.tcp://lighthouse@127.0.0.1:4044""
							roles = [web]
						}
					}
            ");

            //var url = ConfigurationManager.AppSettings.Get("ServiceURL");
            StartWebService(url);
            var actorSystem = GetAkkaCtr().StartAkkaSystem("webcrawler", config);
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
