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


namespace LightHouse
{
    class Program
    {
        static void Main(string[] args)
        {
            LighthouseService service = new LighthouseService();
            service.Start();
            //service.StopAsync().Wait();
            Console.ReadLine();
        }
    }
}
