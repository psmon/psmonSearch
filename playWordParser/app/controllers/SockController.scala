package controllers

/**
  * Created by psmon_qapgr0w on 2017-03-01.
  */
import javax.inject._

import play.api._
import play.api.mvc._
import play.api.libs.streams._
import akka.actor._
import akka.stream._
import sock._
import play.api.libs.json._

import scala.concurrent.Future

@Singleton
class SockController @Inject() (implicit system: ActorSystem, materializer: Materializer)  extends play.api.mvc.Controller{

  //implicit val inEventFormat = Json.format[InEvent]
  //implicit val outEventFormat = Json.format[OutEvent]
  //implicit val messageFlowTransformer = MessageFlowTransformer.jsonMessageFlowTransformer[InEvent, OutEvent]

  def socket = WebSocket.accept[String, String] { request =>
    ActorFlow.actorRef(out => MyWebSocketActor.props(out,system))
  }

}
