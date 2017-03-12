# 피스몬 서치 초안

![초안1](http://psmon.x-y.net/doc/img/basiccomponent.PNG)

이 프로젝트는 학습용으로 진행중입니다.


## 피스몬 서치 사용 플래폼 요악

사용 플래폼 요약

주사용 플래폼: .net45

검색엔진:루씬.net

형태소분석기:Java 라이브러리

주요 통신모델:Akka Remote

이기종 통신: .net<->.java ( WebSocket)

## 공통 컨셉
* Actor 모델 사용: 언랭에서 최초 고안된 모델로 마이크로 모델을 어디든 배포할수 있고, 클러스터 구성할수 있는 OOP설계에서 고려하기힘든  메시지 전송에 중점을 둔 개발컨셉이며 Java Akka/Akk.net/MS(Orleans) 등의 라이브러리로 활용가능
* 제약조건 : Java/.net에서 동일 Actor Model사용시 이기종간 통신을 기대하였지만, Serializable,Byter Order등의 문제로 불가,해결책으로 WebSocket 인터페이스로 이기종 Actor를 연결시킴


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





