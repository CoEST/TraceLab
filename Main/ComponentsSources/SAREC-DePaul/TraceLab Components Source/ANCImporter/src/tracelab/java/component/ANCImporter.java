package tracelab.java.component;

import cli.TraceLabSDK.BaseComponent;
import cli.TraceLabSDK.ComponentAttribute;
import cli.TraceLabSDK.ComponentException;
import cli.TraceLabSDK.ComponentLogger;
import cli.TraceLabSDK.IOSpecAttribute;
import cli.TraceLabSDK.IOSpecType;
import cli.TraceLabSDK.TagAttribute;
import cli.TraceLabSDK.Types.TLKeyValuePairsList;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileReader;
import java.io.IOException;

/**
 *
 * @author mgibiec
 */
@ComponentAttribute.Annotation(
    GuidIDString= "4010e6c9-7b42-413d-aaf4-3e072b36457b",
    Name = "American National Corpus Importer",
    DefaultLabel = "ANC Importer",
    Description = "Imports American National Corpus from CSV file. \n ANC data can be found in package 'Importers Sample Files/ANC Stemmed.csv'",
    Author = "DePaul RE Lab Team",
    Version = "0.1",
    ConfigurationType=Config.class)
@IOSpecAttribute.Annotation.__Multiple({
    @IOSpecAttribute.Annotation(IOType=IOSpecType.__Enum.Output, Name = "ANC", DataType = cli.TraceLabSDK.Types.TLKeyValuePairsList.class)
})
@TagAttribute.Annotation("Importers.ANC.From CSV")
public class ANCImporter extends BaseComponent
{
    private Config m_Config;
    TLKeyValuePairsList termsWithWeights;

    public ANCImporter(ComponentLogger log)
    {
        super(log);

        m_Config = new Config();
        super.set_Configuration(m_Config);
    }

    @Override
    public void Compute() throws ComponentException
    {
        String path = m_Config.getPathToAnc().get_Absolute();
        termsWithWeights = readANC(path);
        super.get_Workspace().Store("ANC", termsWithWeights);
    }

    private TLKeyValuePairsList readANC(String path) throws ComponentException
    {
        TLKeyValuePairsList anc = new TLKeyValuePairsList();
        File file = new File(path);
        try
        {
            BufferedReader br = new BufferedReader(new FileReader((file)));
            int count = 0;
            String line;
            while ((line = br.readLine()) != null)
            {
                String[] tokens = line.split("\t");
                String term = tokens[0].replaceAll(",", "");
                anc.Add(term, Double.parseDouble(tokens[2]));
                count++;
                //super.get_Logger().Trace(term + " " + Double.parseDouble(tokens[2]));
            }
            
            super.get_Logger().Trace("Imported " + count + " terms");
        } 
        catch (FileNotFoundException e)
        {
            throw new ComponentException("Specified file has not been found: " + path);
        } 
        catch (IOException e)
        {
            throw new ComponentException("File " + path + " failed to be open. \n IOException" + e.toString());   
        }
        
        return anc;
    }
}
