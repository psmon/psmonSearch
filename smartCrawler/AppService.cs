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

namespace smartCrawler
{
    public static class AppService
    {
        static AkkaControler akkaCtr = new AkkaControler();

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
