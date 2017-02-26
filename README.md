# 피스몬 서치 초안
![초안](http://psmon.x-y.net/doc/img/basiccomponent.PNG)


## 피스몬 서치 사용 플래폼 요악

사용 플래폼 요약

주사용 플래폼: .net45

검색엔진:루씬.net

형태소분석기:Java 라이브러리

주요 통신모델:Akka Remote

이기종 통신: .net<->.java ( Atmosphere)

## JavaWordParserService ( 형태소 분석기 )

역할: .net에 쓸만한 형태소 분석기가 없어서 Java 버젼으로 분석기 서비스 작성,net에는 웹소켓으로 서비스제공

프레임워크: PlayFrameWork ( [Play다운로드](https://downloads.typesafe.com/play/2.2.4/play-2.2.4.zip) https://downloads.typesafe.com/play/2.2.4/play-2.2.4.zip )


  //Java 웹소켓 핸들러.
  
  @ManagedService(path = "/wsapi",
          broadcasterCache=DefaultBroadcasterCache.class
          ,atmosphereConfig={
          "org.atmosphere.cpr.broadcaster.shareableThreadPool=true",
          "org.atmosphere.cpr.broadcaster.maxAsyncWriteThreads=10",
          "org.atmosphere.cpr.broadcaster.maxProcessingThreads=10",
          "org.atmosphere.cpr.broadcaster.broadcasterLifeCyclePolicy=IDLE",
          "org.atmosphere.cpr.websocket.maxIdleTime=600000",
          "org.atmosphere.cpr.CometSupport.maxInactiveActivity=600000"
  }
  )
  public class WebSockHandler {
  
      public enum e_operation {
          SUBSCRIBE,
          UNSUBSCRIBE,
          AUTHENTICATE,
          LOGOUT,
          CHANGELANGUAGE,
          PING
      }
  
      private final Logger.ALogger logger = play.Logger.of("application");
      private final ObjectMapper mapper = new ObjectMapper();
      //private final TopicEventBus curEventBus = LobbyProxy.getInstance().getTopicEventBus();
  
      public WebSockHandler(){
  
      }
  
      public String toNormalUrl(String url){
          String newUrl ="";
          int idx_removeLast = url.indexOf("?");
          if(idx_removeLast>2){
              newUrl = url.substring(0,idx_removeLast);
          }else{
              newUrl = url;
          }
  
          newUrl = newUrl.replace("http://","");
          newUrl = newUrl.replace("https://","");
          return newUrl;
      }
  
  
      @Ready
      public void onReady(final AtmosphereResource r) {
          logger.debug("Browser {} connected.", r.uuid());
          System.out.println("Browser conneted");
          try{
              //curEventBus.createActor(r);
          }
          catch (Exception e){
              logger.error("Create Actor Failed" + " " + e.toString());
          }
      }
  
  
      @Disconnect
      public void onDisconnect(AtmosphereResourceEvent event) {
          try {
              logger.debug("transport:{}-onDisconnect", event.getResource().transport().toString(), event.getResource().uuid());
  
              if (event.isCancelled()) {
                  logger.debug("Browser {} unexpectedly disconnected", event.getResource().uuid());
                  //curEventBus.onDisconnected(event.getResource().uuid(),99);
              } else if (event.isClosedByClient()) {
                  logger.debug("Browser {} closed the connection by Client", event.getResource().uuid());
                  //curEventBus.onDisconnected(event.getResource().uuid(),1);
              }
              else  {
                  logger.warn("Browser {} unexpectedly disconnected -will not fire event", event.getResource().uuid());
                  //curEventBus.onDisconnected(event.getResource().uuid(),99);
              }
          }
          catch (Exception e){
              logger.debug("Disconnect" + " " + e.toString());
          }
      }
  
  
  
      @Post
      public void onPost(AtmosphereResource r) {
          try{
              logger.debug("{} onPost: requestURL:{}", r.uuid(), r.getRequest().requestURL() );
              logger.debug("{} onPost: getRemoteAddr:{} toNormalUrl:{} ", r.uuid(), r.getRequest().getRemoteAddr(), toNormalUrl(r.getRequest().requestURL()) );
              String endPoint = toNormalUrl(r.getRequest().requestURL());
              String[] parts = endPoint.split("/");
              String content = r.getRequest().body().asString();
  
              if(content.length() == 0)
                  return;
              JsonNode node = mapper.readTree(content);
              logger.debug("data {} ", node.toString()  );
          }
          catch (IOException e){
              logger.debug( "onPost" + " " + e.toString());
          }
          catch (Exception e){
              logger.debug( "onPost" + " " + e.toString());
          }
      }
  
      @org.atmosphere.config.service.Message(encoders = {JacksonEncoder.class}, decoders = {JacksonDecoder.class})
      public data.Message onMessage(data.Message message) throws IOException {
          logger.info("{} just send {}", message.getAuthor(), message.getMessage());
          return message;
      }
  
  }

## SearchAPI ( 검색 API 제공)

## 수집기  (  )

## TestClient( 검색 API를 호출하는 클라이언트)


  



 







