package tracelab.java.component;

import cli.System.Collections.IEnumerator;
import cli.TraceLabSDK.BaseComponent;
import cli.TraceLabSDK.ComponentAttribute;
import cli.TraceLabSDK.ComponentException;
import cli.TraceLabSDK.ComponentLogger;
import cli.TraceLabSDK.IOSpecAttribute;
import cli.TraceLabSDK.IOSpecType;
import cli.TraceLabSDK.TagAttribute;
import cli.TraceLabSDK.Types.TLArtifact;
import cli.TraceLabSDK.Types.TLArtifactsCollection;

@ComponentAttribute.Annotation(
        GuidIDString = "EE6D7C84-2019-11E0-A814-7E80DFD72085",
        Name = "Java Porter Stemmer",
        DefaultLabel = "Java Porter Stemmer",
        Description = "This Pre-Processor stemms the words to their roots.  It uses the Porter stemming algorithm. Implemented in java",
        Author = "DePaul RE Lab Team",
        Version = "1.0")
@IOSpecAttribute.Annotation.__Multiple({
    @IOSpecAttribute.Annotation(IOType = IOSpecType.__Enum.Input, Name = "listOfArtifacts", DataType = cli.TraceLabSDK.Types.TLArtifactsCollection.class),
    @IOSpecAttribute.Annotation(IOType = IOSpecType.__Enum.Output, Name = "listOfArtifacts", DataType = cli.TraceLabSDK.Types.TLArtifactsCollection.class)
})
@TagAttribute.Annotation("Preprocessors")
public class Stemmer extends BaseComponent {

    public Stemmer(ComponentLogger log) {
        super(log);
    }

    /**
     *
     * @throws ComponentException
     */
    public void Compute() throws ComponentException {

        TLArtifactsCollection listOfArtifacts = (TLArtifactsCollection)super.get_Workspace().Load("listOfArtifacts");
        if(listOfArtifacts == null) {
            throw new ComponentException("Input listOfArtifacts is null");
        }
                
        PorterStemmer porter = new PorterStemmer();

        IEnumerator en = listOfArtifacts.get_Keys().GetEnumerator();
        
        while (en.MoveNext()) {
            String artifactId = (String)en.get_Current();
            TLArtifact art = (TLArtifact)listOfArtifacts.get_Item(artifactId);

            porter.add(art.get_Text().toCharArray(), art.get_Text().length());
            porter.stem();
            
            art.set_Text(porter.toString());
        }

        super.get_Workspace().Store("listOfArtifacts", listOfArtifacts);
    }
}
