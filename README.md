# 피스몬 서치 초안
![초안](http://psmon.x-y.net/doc/img/basiccomponent.PNG)

이 프로젝트는 학습용으로 진행중입니다.

## 피스몬 서치 사용 플래폼 요악

사용 플래폼 요약

주사용 플래폼: .net45

검색엔진:루씬.net

형태소분석기:Java 라이브러리

주요 통신모델:Akka Remote

이기종 통신: .net<->.java ( WebSocket)

## Java PlayFrameWork 2.5 설정

의존 라이브러리 다운로드및 설정

* (Activator download)[https://downloads.typesafe.com/typesafe-activator/1.3.12/typesafe-activator-1.3.12.zip]
* JDK 1.8 설치
* IteliJ 설치
* PATH Activator\Bin  설정 (Activator 압축푼 디렉토리/BIN)
* 환경변수 JAVA_HOME 설정 (자바 SDK설치 디렉토리) 
- Project/playWordParser/Activator compile
- IDEA Import SBT Project
  -Download Source,Sources for SBT and plugins 모두 언체크
  -SBT Version 0.13.11  Scala Version 2.11.7
  -Impot root / root-build 모두체크
  -Build Project( 빌드 성공되는지 확인
- IDEA Debug 환경 설정
   -Edit Configuration
   -+ 버튼 , Add SBT Task , Tasks: run 입력
   -실행및 디버깅 확인

* 참고 신규 프로젝트 생성
   -(Play Docu)[https://www.playframework.com/documentation/2.5.x/PlayConsole]

## 루씬.net 기본 사용법

* (기본 인덱싱및 검색)[https://github.com/psmon/psmonSearch/wiki/%EB%A3%A8%EC%94%AC-%ED%85%8C%EC%8A%A4%ED%8A%B8]



