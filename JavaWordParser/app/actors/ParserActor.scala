package actors

import akka.actor.{ActorLogging, Actor}
import play.api.libs.concurrent.Execution.Implicits._
import scala.concurrent.Future
import akka.pattern.pipe
import play.api.libs.json._
import play.api.libs.json.Json
import play.api.Logger

case class Message_JsonString( data:String )  extends Serializable

class ParserActor extends Actor with ActorLogging{
  var me = this
  var playLog =  Logger;

  def receive = {
    case data:Array[Byte] =>
      playLog.debug("ActorLog-" + data)
    case data:String =>
      //ActorMessage...
      playLog.debug("ActorLog-" + data)  //Can Write All Message,but high Load...
    val json:JsValue =Json.toJson( Json.parse(data.toString()) )
      val apiName: Option[String] = (json \ "APINAME" ).asOpt[String]
      apiName match{
        case Some(data_apiname) => {
          val origSender = sender
          Future {
            try{
              val resultString =   "ttt";
              resultString.asInstanceOf[String]
            }
            catch {
              case e:Exception =>  {
                playLog.error("LobbyProxyActor-" + e.getMessage)
                "actorError"
              }
            }
          } pipeTo  origSender
        }
        case None =>
          playLog.error("LobbyProxyActor-Argment Wrong")
          val origSender = sender
          val futureRunAPI = Future {
            "actorError"  //this convert internal error....
          } pipeTo  origSender
      }
    case _ =>
      playLog.error("LobbyProxyActor-Argment must string")
      val origSender = sender
      val futureRunAPI = Future {
        "actorError"  //this convert internal error....
      } pipeTo  origSender
  }

}