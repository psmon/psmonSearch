using System;
using Akka.Configuration;
using System.Configuration;

using common.Actors;

namespace smartCrawler
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            
            ConsoleKeyInfo cki;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);

            var config = ConfigurationFactory.ParseString(@"
            akka {
                actor {
                    provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                    serializers {
                          wire = ""Akka.Serialization.HyperionSerializer, Akka.Serialization.Hyperion""
                    }
                    serialization-bindings {
                    ""System.Object"" = wire
                    }
                }
                remote {
                    helios.tcp {
                        port = 8001
                        hostname = localhost
                    }
                }
            }
            ");

            AppService.GetAkkaCtr().StartAkkaSystem("smartCrawler", config);
            AppService.GetAkkaCtr().CreateActor<SimpleActor>("SearchMain", "test");

            while (true)
            {
                cki = Console.ReadKey(true);
                Console.WriteLine("  Key pressed: {0}\n", cki.Key);

                byte[] test = new byte[6];
                test[0] = 1;
                
                if (cki.Key == ConsoleKey.X) break;
            }
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
