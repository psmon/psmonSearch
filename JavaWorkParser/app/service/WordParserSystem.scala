package service
import akka.actor._
import scala.concurrent.duration._
import akka.pattern.ask
import akka.pattern.after
import akka.util.Timeout
import scala.collection.mutable.ArrayBuffer
import scala.concurrent.{Await, Future}
import akka.pattern.pipe
import com.typesafe.config.ConfigFactory
import java.io.File
import play.api.Logger;
import actors.ParserActor;

/**
  * Created by psmon_qapgr0w on 2017-02-25.
  */
object WordParserSystem {
  val system = ActorSystem("WordParserSystem");


  def getSystem()={
    system
  }

  def startSystem()={
    Logger.info("create actor[parserActor]");
    system.actorOf( Props[ ParserActor ] , name = "parserActor" );

  }

}
