using System;
using Akka;
using Akka.Actor;
using Akka.Event;
using Akka.Configuration;

using Akka.Serialization;

using common.Actors;
using WebSocketSharp;

namespace psmonSearch
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            TestLib.test1();

            ConsoleKeyInfo cki;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);

            var config = ConfigurationFactory.ParseString(@"
            akka {
                actor {
                    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""                    
                }
                remote {
                    helios.tcp {
                        port = 8000
                        hostname = localhost
                    }
                }
            }
            ");

            using (ActorSystem system = ActorSystem.Create("SearchMain", config))
            {                
                var actor = system.ActorOf<SimpleActor>("greeter");
                
                var remoteActor = system.ActorSelection("akka.tcp://WordParserSystem@192.168.0.5:9005/user/parserActor");

                var ws = new WebSocket("ws://192.168.0.5:9001/wsapi");
               

                ws.Connect();
                

                //var actorRef = remoteActor.ResolveOne(TimeSpan.FromSeconds(5)).Result;

                //string id = Serialization.SerializedActorPath(actorRef);

                // Then just serialize the identifier however you like

                // Deserialize
                // (beneath fromBinary)

                //IActorRef deserializedActorRef = system.Provider.ResolveActorRef(id);


                while (true)
                {                   
                    cki = Console.ReadKey(true);                    
                    Console.WriteLine("  Key pressed: {0}\n", cki.Key);

                    byte[] test = new byte[6];
                    test[0] = 1;

                    //remoteActor.Tell(test);
                    ws.Send(@" {""test"":1} ");
                    

                    if (cki.Key == ConsoleKey.X) break;
                }
            }



            Console.WriteLine("Hello World!");
		}

        protected static void myHandler(object sender, ConsoleCancelEventArgs args)
        {
            Console.WriteLine("\nThe read operation has been interrupted.");

            Console.WriteLine("  Key pressed: {0}", args.SpecialKey);

            Console.WriteLine("  Cancel property: {0}", args.Cancel);

            // Set the Cancel property to true to prevent the process from terminating.
            Console.WriteLine("Setting the Cancel property to true...");
            args.Cancel = true;

            // Announce the new value of the Cancel property.
            Console.WriteLine("  Cancel property: {0}", args.Cancel);
            Console.WriteLine("The read operation will resume...\n");
        }
    }
}
