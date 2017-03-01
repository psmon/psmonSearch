package sock

import play.api.libs.json._
import akka.actor._
import play.api.libs.json.JsValue

object MyWebSocketActor {
  def props(out: ActorRef) = Props(new MyWebSocketActor(out))
}

class MyWebSocketActor(out: ActorRef) extends Actor {

  def receive = {
    case msg: String =>
      out ! ("I received your message: " + msg)
    case msg: JsValue =>
      out ! msg
  }

  override def postStop() = {
    //someResource.close()
  }
}


