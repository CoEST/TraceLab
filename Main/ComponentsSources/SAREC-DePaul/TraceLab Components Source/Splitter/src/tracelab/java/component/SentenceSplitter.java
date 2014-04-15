package tracelab.java.component;

import java.util.ArrayList;
import java.util.List;
import java.util.StringTokenizer;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class SentenceSplitter {

    private SentenceSplitter() {}
    
    private static final String DELIM = " .,:;/?'\"[]{})(-_=+~!@#$%^&*<>\n\t\r1234567890";
    private static final String SPLIT_REGEX = "[A-Z][a-z]+|[a-z]+|[A-Z]+";

    /**
     * Sentence splitter process input in such a way, that any token with camel cases are
     * split to separate terms. 
     * @param input text to process
     * @return 
     */
    public static String process(String input) {
        StringTokenizer st = new StringTokenizer(input, DELIM);
        StringBuilder result = new StringBuilder();
        String space = " ";

        while (st.hasMoreTokens()) {
            String tok = st.nextToken();
            Pattern p = Pattern.compile(SPLIT_REGEX);
            Matcher m = p.matcher(tok);
            boolean found = m.find();
            while (found) {
                String subStringFound = m.group();
                if (1 < subStringFound.length()) {
                    result.append(subStringFound + space);
                }
                found = m.find();
            }
        }
        return result.toString();
    }
}
