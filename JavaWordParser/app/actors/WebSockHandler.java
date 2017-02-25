package actors;
import data.*;

import com.fasterxml.jackson.databind.util.JSONPObject;
import play.Play;
import play.Logger;
import com.typesafe.config.Config;
import org.atmosphere.interceptor.*;
import org.atmosphere.config.service.Post;
import org.atmosphere.config.service.Disconnect;
//import org.atmosphere.config.service.Heartbeat;
import org.atmosphere.config.service.ManagedService;
import org.atmosphere.config.service.Ready;
import org.atmosphere.config.service.*;

import org.atmosphere.cache.DefaultBroadcasterCache;
import org.atmosphere.cpr.*;
import java.io.IOException;

import static org.atmosphere.cpr.ApplicationConfig.MAX_INACTIVE;

import java.io.File;
import java.io.IOException;
import java.lang.Boolean;
import java.lang.Exception;
import java.lang.String;
import java.util.Set;
import java.util.HashSet;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import akka.actor.Props;
import akka.actor.ActorRef;
import akka.actor.ActorSystem;
import akka.actor.ActorSelection;
import akka.event.Logging;
import akka.event.LoggingAdapter;
import com.fasterxml.jackson.databind.JsonNode;
import com.fasterxml.jackson.databind.ObjectMapper;
import play.libs.Json;

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

    /*
    @Heartbeat
    public void onHeartbeat(final AtmosphereResourceEvent event) {
        logger.trace("Heartbeat send by {}", event.getResource());
    } */


    /*
    @Get
    public void onGet(final AtmosphereResource r) {
        logger.debug("Browser onGet {} connected.", r.uuid());
        try{
        }
        catch (Exception e){
            logger.debug("onGet" + " " + e.toString());
        }
    } */

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