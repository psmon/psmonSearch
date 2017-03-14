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

namespace common.AkkaUtils
{
    // Akka System을 제어하는 유틸을 집합화합니다.
    public class AkkaControler
    {
        //public static string LastUsedName;
        public static ActorSystem LastActorSystem;

        Dictionary<string, ActorSystem> actorSystemList = new Dictionary<string, ActorSystem>();

        public void SystemDown()
        {            
        }

        public ActorSystem StartAkkaSystem(string name, Config config)
        {
            ActorSystem result = null;            
            if (actorSystemList.ContainsKey(name) == false)
            {
                
                ActorSystem system = ActorSystem.Create(name, config);
                LastActorSystem = system;
                actorSystemList[name] = system;
                result =  system;
            }
            return result;
        }

        public ActorSystem GetSystem(string name)
        {
            return actorSystemList[name];
        }
        
        public IActorRef CreateActor<TActor>(string systemName, string actorName) where TActor : ActorBase, new()
        {
            ActorSystem system = actorSystemList[systemName];
            var actor = system.ActorOf<TActor>(actorName);
            return actor;
        }
    }
}
