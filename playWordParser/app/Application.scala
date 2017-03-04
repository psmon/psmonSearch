/**
  * Created by psmon_qapgr0w on 2017-03-04.
  */
import play.api.mvc._
import akka.actor._
import javax.inject._
import play.api.Logger

//import actors.HelloActor
import actors.WordParserAcotor

@Singleton
class Application  @Inject()(system: ActorSystem) extends Controller {
  Logger.info("Create ParserActor");
  val wordParserActor = system.actorOf( WordParserAcotor.props , "wordParserActor")

}
