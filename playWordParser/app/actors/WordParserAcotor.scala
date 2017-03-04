package actors

/**
  * Created by psmon_qapgr0w on 2017-03-04.
  */

import play.api.libs.json._
import akka.actor._
import play.api.libs.json.JsValue
import java.util.List

import com.fasterxml.jackson.annotation.JsonValue
import models._
import org.snu.ids.ha.ma.{Eojeol, MExpression, MorphemeAnalyzer, Sentence}
import org.snu.ids.ha.index.Keyword
import org.snu.ids.ha.index.KeywordExtractor
import org.snu.ids.ha.index.KeywordList

import scala.collection.JavaConverters._
import scala.collection.mutable.ListBuffer



object WordParserAcotor {
  def props = Props[WordParserAcotor]
  case class Greeting(from: String)
  case object Goodbye
  case class AskParser(words:String,parserType:Int,seqID: Int)
}

class WordParserAcotor extends Actor with ActorLogging{

  implicit val sentenceModelWrites = new Writes[SentenceModel] {
    def writes(data: SentenceModel) = Json.obj(
      "info" -> data.info
    )
  }

  implicit val sentenceSubListModelWrites = new Writes[SentenceSubListModel] {
    def writes(data: SentenceSubListModel) = Json.obj(
      "name" -> data.name,
      "sentens" -> data.sentens
    )
  }

  implicit val sentenceMainListModel = new Writes[SentenceMainListModel] {
    def writes(data: SentenceMainListModel) = Json.obj(
      "dataList" -> data.dataList
    )
  }

  var ma:MorphemeAnalyzer = null;

  import WordParserAcotor._
    def receive = {
      case Greeting(greeter) => log.info(s"I was greeted by $greeter.")
      case Goodbye           => log.info("Someone said goodbye to me.")
      case AskParser(words,parserType,seqID) =>
        if(parserType==1){
          //val ma = new MorphemeAnalyzer
          var ret = ma.analyze(words)
          // refine spacing
          ret = ma.postProcess(ret)
          // leave the best analyzed result
          ret = ma.leaveJustBest(ret)
          // divide result to setences
          val stl:List[Sentence] = ma.divideToSentences(ret)
          var sentenceSubList:ListBuffer[SentenceSubListModel] = ListBuffer[SentenceSubListModel]()

          log.debug("==StlSize " + stl.size().toString())

          for( stInfo <- stl.asScala ) {
            log.debug(stInfo.getSentence)
            val mainWord: String = stInfo.getSentence
            //var sentenceModelList: Seq[SentenceModel] = Seq[SentenceModel]()
            var sentenceModelList: ListBuffer[SentenceModel] = ListBuffer[SentenceModel]()

            val subInfo = stInfo.listIterator();
            for(subItem <- subInfo.asScala ){
              log.debug("==SubInfo " + subItem )
              var sentenceModel = SentenceModel(subItem.toString())
              //sentenceModelList = sentenceModelList :+sentenceModel
              sentenceModelList+=sentenceModel;
              log.debug(subItem.toString())
            }
            var sentencSubListModel = SentenceSubListModel(mainWord, sentenceModelList)
            sentenceSubList+=sentencSubListModel

          }//End For
          val wordResult = SentenceMainListModel( "WordParserInfo", seqID ,sentenceSubList)
          val jsonData = Json.toJson(wordResult)
          sender ! jsonData

        }//end if
    }

  override def preStart()={
    ma = new MorphemeAnalyzer

  }

  override def postStop()={
    if(ma!=null){
      ma.closeLogger();
    }
  }

}
