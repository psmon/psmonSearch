import com.typesafe.config.ConfigFactory;
import org.atmosphere.nettosphere.Config;
import org.atmosphere.nettosphere.Nettosphere;
import play.Application;
import play.GlobalSettings;

import play.Logger;
import java.lang.String;
import service.WordParserSystem;
import actors.WebSockHandler;


public class Global extends GlobalSettings {

    private  void StartNetmosphere(){
        try {
            Config.Builder b = new Config.Builder();
            java.net.InetAddress address = java.net.InetAddress.getLocalHost();
            b.resource(WebSockHandler.class)
                    // For *-distrubution
                    //.resource("./webapps")
                    // For mvn exec:java
                    .resource("./public/socket")
                    // For running inside an IDE
//                .resource("./nettosphere-samples/chat/src/main/resources")
//                .configFile("atmosphere.xml")
//                  .port(ConfigFactory.load().getInt("RGS.PORT"))
                    .port( ConfigFactory.load().getInt("SERVICE.WEBSOCKETPORT") )
                    .host( address.getHostAddress() )
//                .initParam("org.atmosphere.cpr.AtmosphereInterceptor.disableDefaults", "false")
                    .build();

            Nettosphere s = new Nettosphere.Builder().config(b.build()).build();
            s.start();
        }
        catch (Exception e)
        {
            Logger.error("StartNetmosphere Error");
        }
    }

    @Override
    public void onStart(Application app) {
        StartNetmosphere();
        String message = "Application has started! LOLADA";
        Logger.info(message);
        System.out.println(message);

        WordParserSystem.startSystem();

    }

    @Override
    public void onStop(Application app) {
        String message = "Application shutdown...!!! LOLADA";
        Logger.info(message);
        System.out.println(message);
    }
}