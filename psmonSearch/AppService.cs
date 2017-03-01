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
using WebSocketSharp;

using System.Configuration;
using Microsoft.Owin.Hosting;


namespace psmonSearch
{
    public class AppService
    {
        static Dictionary<string, ActorSystem> actorSystemList = new Dictionary<string, ActorSystem>(); 

        static void SystemDown()
        {
        }

        public static void StartWebService(string url)
        {
            WebApp.Start<WebStartup>(url);
        }

        public static void StartAkkaSystem(string name , Config config)
        {           
            if (actorSystemList.ContainsKey(name) == false)
            {
                ActorSystem system = ActorSystem.Create(name, config);
                actorSystemList[name] = system;                
                //var remoteActor = system.ActorSelection("akka.tcp://WordParserSystem@192.168.0.5:9005/user/parserActor");
                //var ws = new WebSocket("ws://192.168.0.5:9001/wsapi");
                //ws.Connect();
                //remoteActor.Tell(test);
                //ws.Send(@" {""test"":1} ");
            }            
        }

        public static void CreateActor<TActor>(string systemName, string actorName) where TActor : ActorBase, new()
        {
            ActorSystem system = actorSystemList[systemName];           
            var actor = system.ActorOf<TActor>(actorName);            
        }        
        
    }
}
