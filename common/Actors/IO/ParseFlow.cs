using System;
using System.Collections.Generic;
using System.Linq;
using Akka;
using Akka.Actor;
using Akka.Streams.Dsl;
using HtmlAgilityPack;
using common.Commands.WebCrawler.State;
using common.Data;

namespace common.Actors.IO
{
    //네이버 블로그 ParserFlow
    public static class ParseFlow
    {
        public static IActorRef publisher;

        public static Flow<DownloadHtmlResult, CheckDocuments, NotUsed> GetParseFlow(CrawlJob jobRoot)
        {
            return Flow.Create<DownloadHtmlResult>().Async()
                .Select(downloadHtmlResult =>
                {
                    var requestedUrls = new List<CrawlDocument>();
                    var htmlString = downloadHtmlResult.Content;
                    var doc = new HtmlDocument();
                    doc.LoadHtml(htmlString);

                    //find all of the IMG tags via XPATH
                    var imgs = doc.DocumentNode.SelectNodes("//img[@src]");

                    //find all of the A...HREF tags via XPATH
                    var links = doc.DocumentNode.SelectNodes("//a[@href]");
                    var frameNode = doc.DocumentNode.SelectNodes("//frame[@id='mainFrame']");                                        
                    var titleNode = doc.DocumentNode.SelectNodes("//meta[@property='og:title']");
                    var descripttionNode = doc.DocumentNode.SelectNodes("//meta[@property='og:description']");
                    var postListNode = doc.DocumentNode.SelectNodes("//div[@id='postListBody']//p");
                    var postListNode2 = doc.DocumentNode.SelectNodes("//div[@id='postListBody']//span");

                    if (titleNode != null)
                    {
                        string title = null;
                        string descripttion;
                        string Content1="";
                        string Content2="";
                        foreach(HtmlNode node in titleNode)
                        {
                            title = node.Attributes["content"].Value;
                        }
                                                
                        if(descripttionNode != null)
                        {
                            foreach(HtmlNode node in descripttionNode)
                            {
                                descripttion = node.Attributes["content"].Value;
                            }                            
                        }

                        if (postListNode != null)
                        {
                            foreach(HtmlNode node in postListNode)
                            {
                                Content1 += node.InnerText;
                            }
                            
                        }

                        if (postListNode2 != null)
                        {
                            foreach(HtmlNode node in postListNode2)
                            {
                                Content2 += node.InnerText;
                            }                            
                        }

                        if (title != null && (Content1.Length>5 || Content2.Length>5) )
                        {
                            //Todo : 수집 컨텐츠. 스토어하기
                            BlogDocuments blogdoc = new BlogDocuments(title,Content1+Content2);
                            publisher.Tell(blogdoc, ActorRefs.NoSender);
                        }
                    }

                    if (frameNode !=null)
                    {
                        Console.WriteLine("find frame..");
                        var validLinkUris =
                            frameNode.Select(x => x.Attributes["src"].Value)
                                .Where(x => CanMakeAbsoluteUri(jobRoot, x))
                                .Select(x => ToAsboluteUri(jobRoot, x))
                                .Where(x => AbsoluteUriIsInDomain(jobRoot, x))
                                .Select(y => new CrawlDocument(y, false));

                        requestedUrls = requestedUrls.Concat(validLinkUris).ToList();
                    }
                    

                    /* PROCESS ALL IMAGES */
                    if (imgs != null)
                    {
                        /*
                        var validImgUris =
                            imgs.Select(x => x.Attributes["src"].Value)
                                .Where(x => CanMakeAbsoluteUri(jobRoot, x))
                                .Select(x => ToAsboluteUri(jobRoot, x))
                                .Where(x => AbsoluteUriIsInDomain(jobRoot, x))
                                .Select(y => new CrawlDocument(y, true));

                        requestedUrls = requestedUrls.Concat(validImgUris).ToList();*/
                    }


                    /* PROCESS ALL LINKS */
                    links = null;   //대량의 다운로드 방지를 위해, 관련 링크 다운로드방지,대량 수집을 하려면 코멘트 하세요..
                    if (links != null)
                    {
                        var validLinkUris =
                            links.Select(x => x.Attributes["href"].Value)
                                .Where(x => CanMakeAbsoluteUri(jobRoot, x))
                                .Select(x => ToAsboluteUri(jobRoot, x))
                                .Where(x => AbsoluteUriIsInDomain(jobRoot, x))
                                .Select(y => new CrawlDocument(y, false));

                        requestedUrls = requestedUrls.Concat(validLinkUris).ToList();
                    }

                    return new CheckDocuments(requestedUrls, ActorRefs.NoSender, TimeSpan.FromMilliseconds(requestedUrls.Count * 5000));
                });
        }

        public static bool CanMakeAbsoluteUri(CrawlJob jobRoot, string rawUri)
        {
            if (Uri.IsWellFormedUriString(rawUri, UriKind.Absolute))
                return true;
            try
            {
                var absUri = new Uri(jobRoot.Root, rawUri);
                var returnVal = absUri.Scheme.Equals(Uri.UriSchemeHttp) || absUri.Scheme.Equals(Uri.UriSchemeHttps);
                return returnVal;
            }
            catch
            {
                return false;
            }
        }

        public static bool AbsoluteUriIsInDomain(CrawlJob jobRoot, Uri otherUri)
        {
            return jobRoot.Domain == otherUri.Host;
        }

        public static Uri ToAsboluteUri(CrawlJob jobRoot, string rawUri)
        {
            return Uri.IsWellFormedUriString(rawUri, UriKind.Absolute) ? new Uri(rawUri, UriKind.Absolute) : new Uri(jobRoot.Root, rawUri);
        }
    }
}
