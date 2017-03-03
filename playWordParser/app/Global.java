/**
 * Created by psmon_qapgr0w on 2017-03-03.
 */
import play.*;
import java.util.List;
import org.snu.ids.ha.ma.MExpression;
import org.snu.ids.ha.ma.MorphemeAnalyzer;
import org.snu.ids.ha.ma.Sentence;
import org.snu.ids.ha.index.Keyword;
import org.snu.ids.ha.index.KeywordExtractor;
import org.snu.ids.ha.index.KeywordList;


public class Global extends GlobalSettings {

    public  void wordTest(String testStr){

        try{
            // string to analyze
            String string = testStr;

            // init MorphemeAnalyzer
            MorphemeAnalyzer ma = new MorphemeAnalyzer();

            // create logger, null then System.out is set as a default logger
            //ma.createLogger("c:\\log.txt");

            // analyze morpheme without any post processing
            List<MExpression> ret = ma.analyze(string);

            // refine spacing
            ret = ma.postProcess(ret);

            // leave the best analyzed result
            ret = ma.leaveJustBest(ret);

            // divide result to setences
            List<Sentence> stl = ma.divideToSentences(ret);

            // print the result
            for( int i = 0; i < stl.size(); i++ ) {
                Sentence st = stl.get(i);
                System.out.println("===>  " + st.getSentence());
                for( int j = 0; j < st.size(); j++ ) {
                    System.out.println(st.get(j));
                }
            }

            ma.closeLogger();

        }catch (Exception e){
            Logger.error(e.getMessage());
        }


    }

    public void wordTest2(String testStr){
        // string to extract keywords
        String strToExtrtKwrd = testStr;

        // init KeywordExtractor
        KeywordExtractor ke = new KeywordExtractor();

        // extract keywords
        KeywordList kl = ke.extractKeyword(strToExtrtKwrd, true);

        // print result
        for( int i = 0; i < kl.size(); i++ ) {
            Keyword kwrd = kl.get(i);
            System.out.println(kwrd.getString() + "\t" + kwrd.getCnt());
        }
    }

    public  void test(){
        wordTest("나는 개발자이다");
        /* Result
        나는			=> [0/나/NP+1/는/JX]
        개발자이다		=> [3/개발자/NNG+6/이/VCP+7/다/EFN]
         */

        wordTest2("대통령 혼자 힘으로는 나라를 못바꿉니다.");
        /* Result
        대통령	1
        혼자	1
        힘	1
        나라	1
         */

    }


    @Override
    public void onStart(Application application) {
        Logger.info("Play Start...");
        test();
    }

    @Override
    public void onStop(Application application) {

    }
}
