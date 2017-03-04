package sock

import actors.WordParserAcotor.AskParser
import play.api.libs.json._
import akka.actor._
import play.api.libs.json.JsValue
import models._


object MyWebSocketActor {
  def props(out: ActorRef, system:ActorSystem ) = Props(new MyWebSocketActor(out,system))
}

class MyWebSocketActor(out: ActorRef, system:ActorSystem) extends Actor {
  def receive = {
    case msg: String =>
      system.actorSelection("/user/wordParserActor") ! AskParser(msg,1)
      out ! ("I received your message: " + msg)
    case msg: JsValue =>
      out ! msg.toString()
    case msg: SentenceMainListModel =>
      out ! "ttt"
  }

  override def postStop() = {
    //someResource.close()
  }
}


