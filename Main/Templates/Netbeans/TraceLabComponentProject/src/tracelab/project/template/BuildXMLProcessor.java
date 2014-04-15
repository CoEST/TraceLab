/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package tracelab.project.template;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.logging.Level;
import java.util.logging.Logger;
import java.util.zip.ZipInputStream;
import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import javax.xml.xpath.*;
import org.openide.WizardDescriptor;
import org.openide.filesystems.FileObject;
import org.openide.filesystems.FileUtil;
import org.openide.xml.XMLUtil;
import org.w3c.dom.DOMException;
import org.w3c.dom.Document;
import org.w3c.dom.Element;
import org.w3c.dom.Node;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

public class BuildXMLProcessor 
{
    private static final Logger logger = Logger.getLogger(BuildXMLProcessor.class.getName());
    
    public static void processBuildXML(FileObject fo, ZipInputStream str, WizardDescriptor wiz) throws IOException, ParserConfigurationException, SAXException, XPathExpressionException 
    {        
        ByteArrayOutputStream baos = new ByteArrayOutputStream();
        FileUtil.copy(str, baos);
                
        //construct DOM document
        DocumentBuilderFactory domFactory = DocumentBuilderFactory.newInstance();
        domFactory.setNamespaceAware(true); // never forget this!
        DocumentBuilder builder = domFactory.newDocumentBuilder();
        
        Document doc = builder.parse(new InputSource(new ByteArrayInputStream(baos.toByteArray())));
        
        //prepare XPathFactory
        XPathFactory factory = XPathFactory.newInstance();
        XPath xpath = factory.newXPath();
        
        String value;
        
        value = (String)wiz.getProperty("components.dir");
        SetProperty(xpath, doc, "components.dir", value);
        
        value = (String)wiz.getProperty("ikvm.executable.dir");
        SetProperty(xpath, doc, "ikvm.executable.dir", value);
                
        value = (String)wiz.getProperty("TraceLabSDK.dll");
        SetProperty(xpath, doc, "TraceLabSDK.dll", value);
        
        value = (String)wiz.getProperty("TraceLabSDK.Types.dll");
        SetProperty(xpath, doc, "TraceLabSDK.Types.dll", value);
        
        OutputStream out = fo.getOutputStream();
        try {
            XMLUtil.write(doc, out, "UTF-8");
        } finally {
            out.close();
        }

    }

    private static void SetProperty(XPath xpath, Document doc, String propertyName, String value) throws XPathExpressionException, DOMException {
        XPathExpression expr = xpath.compile("//property[@name='"+propertyName+"']");
        Node node = (Node)expr.evaluate(doc, XPathConstants.NODE);
        if (node != null) 
        {
            Element el = (Element)node;
            el.setAttribute("value", value);
        }
    }
}
