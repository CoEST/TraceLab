package tracelab.java.component;

import cli.TraceLabSDK.Types.TLArtifact;
import cli.TraceLabSDK.Types.TLArtifactsCollection;
import org.junit.After;
import org.junit.AfterClass;
import org.junit.Assert;
import org.junit.Before;
import org.junit.BeforeClass;
import org.junit.Test;
import static org.junit.Assert.*;

public class SentenceSplitterTest {
    
    public SentenceSplitterTest() {
    }
    
    @BeforeClass
    public static void setUpClass() {
    }
    
    @AfterClass
    public static void tearDownClass() {
    }
    
    @Before
    public void setUp() {
    }
    
    @After
    public void tearDown() {
    }
   
    @Test
    public void SentenceSplitterRunTest() 
    {   
        String textToProcess = "ThisShouldGetSplittedToSeperateWords thisshouldnot thisShouldBe ";
        String expectedResult = "This Should Get Splitted To Seperate Words thisshouldnot this Should Be ";
        
        String result = SentenceSplitter.process(textToProcess);
        
        Assert.assertEquals(expectedResult, result);   
    }
}
