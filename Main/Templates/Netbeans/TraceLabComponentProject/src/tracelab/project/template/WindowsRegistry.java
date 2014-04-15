package tracelab.project.template;

import java.io.IOException;
import java.io.InputStream;
import java.io.StringWriter;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 * @author Oleg Ryaboy, based on work by Miguel Enriquez 
 */
public class WindowsRegistry {

    /**
     * 
     * @param location path in the registry
     * @param key registry key
     * @return registry value or null if not found
     */
    public static final String readRegistry(String location, String key)
    {
        try {
            // Run reg query, then read output with StreamReader (internal class)
            Process process = Runtime.getRuntime().exec("reg query " + 
                    '"'+ location + "\" /v " + key);

            StreamReader reader = new StreamReader(process.getInputStream());
            reader.start();
            process.waitFor();
            reader.join();
            String output = reader.getResult();

            // Parse out the value
            String[] parsed = output.split("REG_SZ");
            
            String returnValue = null;
            if(parsed.length > 0) 
            {
                returnValue = parsed[parsed.length-1].trim();
            }
            
            return returnValue;
        }
        catch (Exception e) {
            logger.log(Level.WARNING, null, e);
            return null;
        }

    }
    
    private static final Logger logger = Logger.getLogger(WindowsRegistry.class.getName());

    static class StreamReader extends Thread {
        private InputStream is;
        private StringWriter sw= new StringWriter();

        public StreamReader(InputStream is) {
            this.is = is;
        }

        public void run() {
            try {
                int c;
                while ((c = is.read()) != -1)
                    sw.write(c);
            }
            catch (IOException e) { 
                logger.log(Level.WARNING, null, e);
            }
        }

        public String getResult() {
            return sw.toString();
        }
    }
}