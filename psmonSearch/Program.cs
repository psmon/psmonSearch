using System;
using Akka.Configuration;
using System.Configuration;

using common.Actors;
using common.Data;

namespace psmonSearch
{
	class MainClass
	{
		public static void Main(string[] args)
		{
            TestLib.test1();

            ConsoleKeyInfo cki;
            Console.CancelKeyPress += new ConsoleCancelEventHandler(myHandler);
            var url = "http://+:80";

            AppService.Start(url);

            while (true)
            {
                string readCommand;
                readCommand = Console.ReadLine();
                
                var system = AppService.GetAkkaCtr().GetSystem("webcrawler");

                string[] conargs = readCommand.Split('^');

                if (conargs.Length > 1)
                {
                    string command = conargs[0];
                    string reqData1 = conargs[1];
                    switch (command)
                    {
                        case "get":
                            if (reqData1.Length > 10)
                            {
                                if (reqData1.Substring(0, 4) == "http")
                                {
                                    system.ActorSelection("user/commands").Tell(new AttemptWebCrawl(reqData1));
                                }
                            }
                            break;
                        case "search":
                            Console.WriteLine(TestLib.Search(reqData1)) ;
                            break;
                    }
                }
                
                
                if (readCommand == "exit")
                    break;

                //if (cki.Key == ConsoleKey.X) break;
            }

            AppService.SystemDown();


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
