﻿using System;
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

using WebSocketSharp;

using System.Configuration;
using Microsoft.Owin.Hosting;


namespace psmonSearch
{
    public class AppService
    {        
        static AkkaControler akkaCtr = new AkkaControler();

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
