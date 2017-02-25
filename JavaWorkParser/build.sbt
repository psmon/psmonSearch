name := "JavaWordParser"

version := "1.0-SNAPSHOT"

scalaVersion := "2.10.4"

lazy val root = (project in file(".")).enablePlugins(PlayJava)
//play.Project.playScalaSettings
//resolvers += "public repo.mal.internal" at "https://repo.mal.internal/content/groups/public"

libraryDependencies ++= Seq(
  // Select Play modules
  //jdbc,
  filters,   // A set of built-in filters
  javaCore,  // The core Java API
  javaWs,
  ws,
  cache,
  // WebJars pull in client-side web libraries
  "org.webjars" %% "webjars-play" % "2.2.0",
  "org.webjars" % "bootstrap" % "2.3.1",
  "com.typesafe.akka" %% "akka-remote"  % "2.3.1",
  "org.atmosphere" % "nettosphere" % "2.2.1",
  "org.atmosphere" % "atmosphere-runtime" % "2.2.6",
  "org.codehaus.jackson" % "jackson-mapper-asl" % "1.5.0",
  "org.scala-tools.sbinary" % "sbinary_2.10" % "0.4.2"
)     

//play.Project.playScalaSettings
