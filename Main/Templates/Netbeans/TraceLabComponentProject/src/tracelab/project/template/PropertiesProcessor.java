/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package tracelab.project.template;

import java.io.*;
import java.util.Properties;
import java.util.zip.ZipInputStream;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.xpath.XPath;
import javax.xml.xpath.XPathExpressionException;
import javax.xml.xpath.XPathFactory;
import org.openide.WizardDescriptor;
import org.openide.filesystems.FileObject;
import org.openide.filesystems.FileUtil;
import org.openide.xml.XMLUtil;
import org.w3c.dom.Document;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

/**
 *
 * @author aczauder
 */
public class PropertiesProcessor {
    
    public static void processProjectProperties(FileObject fo, ZipInputStream str, WizardDescriptor wiz, String projectName) throws IOException, ParserConfigurationException, SAXException, XPathExpressionException 
    {        
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        FileUtil.copy(str, baos);
                
        Properties properties = new Properties();  
        properties.load(new ByteArrayInputStream(baos.toByteArray()));
        
        properties.setProperty("application.title", projectName);
        properties.setProperty("application.vendor", "anonymous");
        properties.setProperty("dist.jar", "${dist.dir}/"+projectName+".jar");
        
        String value;
        
        value = (String)wiz.getProperty("TraceLabSDK.jar");
        properties.setProperty("file.reference.TraceLabSDK.jar", value);
        
        value = (String)wiz.getProperty("TraceLabSDK.Types.jar");
        properties.setProperty("file.reference.TraceLabSDK.Types.jar", value);
        
        value = (String)wiz.getProperty("mscorlib.jar");
        properties.setProperty("file.reference.mscorlib.jar-1", value);
        
        OutputStream out = fo.getOutputStream();
        try
        {
            properties.store(out, null);
        } finally {
            out.close();
        }

    }
    
}
