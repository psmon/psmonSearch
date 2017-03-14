using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using Lucene.Net;
using Lucene.Net.Search;
using Lucene.Net.Documents;

using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

using Lucene.Net.QueryParsers;

using Lucene.Net.Store;
using Lucene.Net.Support;


using Lucene.Net.Index;
using FSDirectory = Lucene.Net.Store.FSDirectory;
using Version = Lucene.Net.Util.Version;

namespace psmonSearch
{
    public static class TestLib
    {
        private class AnonymousClassCollector : Collector
        {
            private Scorer scorer;
            private int docBase;

            // simply print docId and score of every matching document
            public override void Collect(int doc)
            {
                Console.Out.WriteLine("doc=" + doc + docBase + " score=" + scorer.Score());
            }

            public override bool AcceptsDocsOutOfOrder
            {
                get { return true; }
            }

            public override void SetNextReader(IndexReader reader, int docBase)
            {
                this.docBase = docBase;
            }

            public override void SetScorer(Scorer scorer)
            {
                this.scorer = scorer;
            }
        }

        private class OneNormsReader : FilterIndexReader
        {
            private readonly String field;

            public OneNormsReader(IndexReader in_Renamed, String field) : base(in_Renamed)
            {
                this.field = field;
            }

            public override byte[] Norms(String field)
            {
                return in_Renamed.Norms(this.field);
            }
        }




        public static Document CreateTourBlogDocument(IndexWriter writer, string title,string htmlcontent,DateTime lastWriteTime)
        {
            // title => 나라명,도시명
            // content ==> 유적, 건물, 맛집
            Document doc = new Document();            
            doc.Add(new Field("modified", DateTools.TimeToString(lastWriteTime.Millisecond, DateTools.Resolution.MINUTE), Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("title", title, Field.Store.YES, Field.Index.NOT_ANALYZED));
            doc.Add(new Field("contents", title + " " + htmlcontent, Field.Store.YES, Field.Index.ANALYZED) );
            writer.AddDocument(doc);
            return doc;            
        }

        public static Document CreateTourBlogDocument2(string title, string htmlcontent, DateTime lastWriteTime , string extData)
        {
            // title => 나라명,도시명
            // content ==> 유적, 건물, 맛집
            Document doc = new Document();
            doc.Add(new Field("modified", DateTools.TimeToString(lastWriteTime.Millisecond, DateTools.Resolution.MINUTE), Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field("title", title, Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field("extData", title, Field.Store.YES, Field.Index.NOT_ANALYZED));

            doc.Add(new Field("contents", title + " " + htmlcontent, Field.Store.YES, Field.Index.ANALYZED));

            writer.AddDocument(doc);
            return doc;
        }

        public static String Search(string keyword)
        {
            string result="";
            Searcher searcher = new IndexSearcher(indexReader);
            Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);
            var parser = new QueryParser(Version.LUCENE_30, "contents", analyzer);
            Query query = parser.Parse( keyword );
            Console.WriteLine(string.Format("Query:{0}", query));
            TopDocs topdocs = searcher.Search(query, null, 100);
            Collector streamingHitCollector = new AnonymousClassCollector();
            searcher.Search(query, streamingHitCollector);

            foreach (ScoreDoc scoreDoc in topdocs.ScoreDocs)
            {
                if (scoreDoc.Score > 0.1f)
                {
                    Document doc = searcher.Doc(scoreDoc.Doc);
                    Console.WriteLine(string.Format("DocTitle:{0} DocContent:{1}  HitCount:{2}", doc.Get("title"), doc.Get("contents"), scoreDoc.Score));
                }
            }
            return result;
        }



        public static void DoStreamingSearch(Searcher searcher, Query query)
        {
            Collector streamingHitCollector = new AnonymousClassCollector();
            searcher.Search(query, streamingHitCollector);
        }


        public static IndexWriter writer;
        public static IndexReader indexReader;

        public static void test1()
        {
            try
            {
                string idexDB = @"d:\Temp\TestIndexDB";
                DirectoryInfo INDEX_DIR = new DirectoryInfo(idexDB);

                SimpleFSDirectory fsDir = new SimpleFSDirectory(INDEX_DIR, null);

                bool isNewCreate = false;
                writer = new IndexWriter(fsDir, new StandardAnalyzer(Version.LUCENE_30), isNewCreate, IndexWriter.MaxFieldLength.LIMITED);

                Console.Out.WriteLine("Indexing to directory '" + INDEX_DIR + "'...");
                CreateTourBlogDocument(writer, "파리 여행", "에펠탑 ", DateTime.Now);
                CreateTourBlogDocument(writer, "서울 여행", "남대문 시장 한국", DateTime.Now);
                CreateTourBlogDocument(writer, "서울 여행2", "남대문 시장 한국", DateTime.Now);
                CreateTourBlogDocument(writer, "유럽 여행", "파리 에펠탑", DateTime.Now);
                CreateTourBlogDocument(writer, "유럽 여행", "스페인 에서 파리 에펠탑 투어", DateTime.Now);
                writer.Optimize();
                writer.Commit();

                indexReader = null;
                indexReader = IndexReader.Open(fsDir, true); // only searching, so read-only=true
                Searcher searcher = new IndexSearcher(indexReader);
                Analyzer analyzer = new StandardAnalyzer(Version.LUCENE_30);
                var parser = new QueryParser(Version.LUCENE_30, "contents", analyzer);

                Query query = parser.Parse("한국");
                Console.WriteLine(string.Format("Query:{0}", query));
                TopDocs topdocs = searcher.Search(query, null, 100);

                Collector streamingHitCollector = new AnonymousClassCollector();
                searcher.Search(query, streamingHitCollector);

                foreach (ScoreDoc scoreDoc in topdocs.ScoreDocs)
                {
                    if (scoreDoc.Score > 0.1f)
                    {
                        Document doc = searcher.Doc(scoreDoc.Doc);
                        Console.WriteLine(string.Format("DocTitle:{0} DocContent:{1}  HitCount:{2}", doc.Get("title"), doc.Get("contents"), scoreDoc.Score));
                    }
                }

                if (indexReader != null)
                {
                    //indexReader.Dispose();
                    //fsDir.Dispose();
                }

            }
            catch(IOException e)
            {
                Console.Out.WriteLine(" caught a " + e.GetType() + "\n with message: " + e.Message);
            }            
        }
        
    }
}
