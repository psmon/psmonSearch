using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka;
using Akka.Actor;
using Akka.Event;

namespace common.Actors
{
    public class SimpleActor : ReceiveActor
    {
        protected ILoggingAdapter Log = Context.GetLogger();

        public SimpleActor()
        {
            Receive<byte[]>(message => {
                Log.Debug("Received String message: {0}", message);
                //Sender.Tell("re:" + message);
            });

            Receive<string[]>(message => {
                Log.Debug("Received String message: {0}", message);
                //Sender.Tell("re:" + message);
            });

        }
    }
}
