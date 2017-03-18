# 피스몬 서치 초안

![초안1](http://psmon.x-y.net/doc/img/basiccomponent.PNG)

기반라이브러리 학습용으로, 최소한의 기능을 작동하는 인터페이스만 작동예정입니다.


## 피스몬 서치 사용 플래폼 요악

사용 플래폼 요약

주사용 플래폼: .net45

검색엔진:루씬.net

형태소분석기:Java 라이브러리

주요 통신모델:Akka Remote

이기종 통신: .net<->.java ( WebSocket)

## 공통 컨셉
* Actor 모델 사용: 언랭에서 최초 고안된 모델로 마이크로 모델을 어디든 배포할수 있고, 클러스터 구성할수 있는 OOP설계에서 고려하기힘든  메시지 전송에 중점을 둔 개발컨셉이며 Java Akka/Akk.net/MS(Orleans) 등의 라이브러리로 활용가능
* 제약조건 : Java/.net에서 동일 Actor Model사용시 이기종간 통신을 기대하였지만,Actor는 같은기종 미들웨어 서버간 통신을 위해 고려된 모델이며 Serializable,Byter Order등의 문제로 불가,해결책으로 WebSocket 인터페이스로 이기종 Actor를 연결시킴, 이 방식은 미들웨어-사용자(브라우져)간 통신방식을 확장할때도 사용하는 방식임


## .net 프로젝트에 사용될 컨셉 (VS 2015)

* [루씬 인덱싱/검색](https://github.com/psmon/psmonSearch/blob/master/psmonSearch/TestLib.cs)
* [서버간 PUB/SUB 모델 사용](https://github.com/psmon/TopicEventBus)
* [서비스간 리모트 통신 사용](https://github.com/psmon/AkkaNetTest)
* [수집 크롤 엔진 작성]( https://github.com/psmon/psmonSearch/tree/master/common/Commands )
* [for Micro Service Deploy](https://topshelf.readthedocs.io/en/latest/index.html)
* [Html Parser](https://htmlagilitypack.codeplex.com/)

## java 프로젝트에 사용될 컨셉

* [WebSocket for Play25](https://github.com/psmon/psmonSearch/commit/8a885c1de1820192cb56c32cc34b41e18f03eefc#diff-911a0f2c3e264e6dd7b2e110349983d5)
* [형태소 분석기-꼬꼬마 SocketTestTool](http://socktest.webnori.com/wstest)
  -한글 Data ,1차 분해를 위한 역활로 WebSocket API화하여, 어느 플래폼이건 빠르게 사용 가능하게 해두었습니다. 

## 설계에 참조할 컨셉및 소스

https://github.com/petabridge/akkadotnet-code-samples/tree/master/Cluster.WebCrawler



## playWordParser 설정 (JAVA 1.8 IDEA Community)

C# 라이브러리에, 만족할만한 형태소 분석기 라이브러리를 찾지못해 JAVA진영의 라이브러리(꼬꼬마) 사용하였으며

PlayFramework 는, FullStack-[Spec확인](https://www.playframework.com/documentation/2.5.x/Tutorials) 웹개발이 가능하여

선택하였습니다.  C#<->.net 고성능 이기종간의 통신을위해 웹소켓 베이스 인터페이스로 구현예정입니다. 


### 의존 라이브러리 다운로드및 설정

* [Activator download](https://downloads.typesafe.com/typesafe-activator/1.3.12/typesafe-activator-1.3.12.zip)
* JDK 1.8 설치
* IteliJ 설치
* PATH Activator\Bin  설정 (Activator 압축푼 디렉토리/BIN)
* 환경변수 JAVA_HOME 설정 (자바 SDK설치 디렉토리) 

### Compile
* Project/playWordParser/Activator compile

### Import Porject
* IDEA Import SBT Project
  - Download Source,Sources for SBT and plugins 모두 언체크
  - SBT Version 0.13.11  Scala Version 2.11.7
  - Impot root / root-build 모두체크
  - Build Project( 빌드 성공되는지 확인
* IDEA Debug 환경 설정
   - Edit Configuration
   - + 버튼 , Add SBT Task , Tasks: run 입력
   - 실행및 디버깅 확인
   
### 기본 개념

## 한국어 형태소 분석
한글 형태소의 품사를 '체언, 용언, 관형사, 부사, 감탄사, 조사, 어미, 접사, 어근, 부호, 한글 이외'와 같이 나누고 각 세부 품사를 구분한다.

http://kkma.snu.ac.kr/documents/index.jsp?doc=postag

## Ngram
N-gram이란?
N-gram은 텍스트나 문서에서 추출한 문자 또는 단어의 시퀀스이며, 문자 기반 및 단어 기반이라는 두 그룹으로 분류할 수 있다. 
N-gram은 단어 또는 문자열(이 튜토리얼의 경우)에서 추출한 N개의 연속 문자 세트이다. 이 방법의 배후에는 비슷한 단어가 
N-gram의 높은 비율을 차지할 것이라는 개념이 깔려있다. 가장 일반적으로 사용되는 N값은 2와 3이며, 각각의 경우를 bigram과 trigram이라고 한다. 
예를 들어, TIKA라는 단어에서는 T, TI, IK, KA, A*라는 bigram과 **T, *TI, TIK, IKA, KA, A*라는 trigram이 생성된다. ""는 채우기 공간을 의미한다. 
문자 기반 N-gram은 문자열으 유사성을 측정하는데 사용된다. 문자 기반 N-gram을 사용하는 애플리케이션으로는 맞춤법 검사기 스테밍(strmming), OCR등이 있다.
발췌 :  http://www.ibm.com/developerworks/kr/opensource/tutorials/os-apache-tika/section6.html





