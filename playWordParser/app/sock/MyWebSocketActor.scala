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
      val json:JsValue = Json.parse(msg)
      val pid = (json \ "pid").asOpt[String]

      pid match {
        case Some("WordParserInfo")  =>
          val text:String = (json \ "text").as[String]
          val reqID:Int = (json \ "reqID").as[Int]
          system.actorSelection("/user/wordParserActor") ! AskParser(text,1,reqID)
        case None =>
          out ! ("None")
      }

    case msg: JsValue =>
      out ! msg.toString()
    case msg: SentenceMainListModel =>
      out ! "ttt"
  }

  override def postStop() = {
    //someResource.close()
  }
}


