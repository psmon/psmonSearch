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
import org.snu.ids.ha.ma.MExpression
import org.snu.ids.ha.ma.MorphemeAnalyzer
import org.snu.ids.ha.ma.Sentence
import org.snu.ids.ha.index.Keyword
import org.snu.ids.ha.index.KeywordExtractor
import org.snu.ids.ha.index.KeywordList
import scala.collection.JavaConverters._



object WordParserAcotor {
  def props = Props[WordParserAcotor]
  case class Greeting(from: String)
  case object Goodbye
  case class AskParser(words:String,parserType:Int)
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
      case AskParser(words,parserType) =>
        if(parserType==1){
          //val ma = new MorphemeAnalyzer
          var ret = ma.analyze(words)
          // refine spacing
          ret = ma.postProcess(ret)
          // leave the best analyzed result
          ret = ma.leaveJustBest(ret)
          // divide result to setences
          val stl:List[Sentence] = ma.divideToSentences(ret)
          // print the result

          var sentenceSubList:Seq[SentenceSubListModel] = Seq[SentenceSubListModel]()
          log.debug("==StlSize " + stl.size().toString())

          for( stInfo <- stl.asScala ) {
            log.debug(stInfo.getSentence)
            val mainWord: String = stInfo.getSentence
            var sentenceModelList: Seq[SentenceModel] = Seq[SentenceModel]()
            var j = 0
            while (j < stInfo.size()) {
              var item = stInfo.get(j)
              var sentenceModel = SentenceModel(item.toString())
              sentenceModelList = sentenceModelList :+sentenceModel
              log.debug(item.toString())
              j += 1
            }

            var sentencSubListModel = SentenceSubListModel(mainWord, sentenceModelList)
            sentenceSubList = sentenceSubList :+ sentencSubListModel


          }//End For
          val wordResult = SentenceMainListModel(sentenceSubList)
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
