package models

/**
  * Created by psmon_qapgr0w on 2017-03-04.
  */

case class SentenceModel( info:String )

case class SentenceSubListModel(name: String , sentens:Seq[SentenceModel] )

case class SentenceMainListModel( dataList:Seq[SentenceSubListModel] )








