package tracelab.java.component;

import cli.System.Collections.IEnumerator;
import cli.TraceLabSDK.ComponentLogger;
import cli.TraceLabSDK.IOSpecType;
import cli.TraceLabSDK.BaseComponent;
import cli.TraceLabSDK.ComponentAttribute;
import cli.TraceLabSDK.ComponentException;
import cli.TraceLabSDK.IOSpecAttribute;
import cli.TraceLabSDK.TagAttribute;
import cli.TraceLabSDK.Types.TLArtifact;
import cli.TraceLabSDK.Types.TLArtifactsCollection;

@ComponentAttribute.Annotation(
    GuidIDString= "74283A1E-49A8-11E0-82B1-4BCADFD72085",
    Name = "Splitter",
    DefaultLabel = "Splitter",
    Description = "Splitter",
    Author = "DePaul RE Lab Team",
    Version = "0.1")
@IOSpecAttribute.Annotation.__Multiple({
    @IOSpecAttribute.Annotation(IOType = IOSpecType.__Enum.Input, Name = "listOfArtifacts", DataType = TLArtifactsCollection.class, Description="Collection of artifacts that is going to be processed"),
    @IOSpecAttribute.Annotation(IOType = IOSpecType.__Enum.Output, Name = "listOfArtifacts", DataType = TLArtifactsCollection.class, Description="Processed artifacts")
})
@TagAttribute.Annotation("Preprocessors")
public class SplitterComponent  extends BaseComponent {

   public SplitterComponent(ComponentLogger log){
        super(log);
    }

    @Override
    public void Compute() throws ComponentException {

        TLArtifactsCollection listOfArtifacts = (TLArtifactsCollection) super.get_Workspace().Load("listOfArtifacts");
     
        if(listOfArtifacts == null) {
            throw new ComponentException("Received list of artifacts is null!");
        }
        
        //iterate through all artifacts and run sentence splitter on each of the artifact
        IEnumerator en = listOfArtifacts.get_Keys().GetEnumerator();
        while (en.MoveNext()) {
            String artifactId = (String)en.get_Current();
            TLArtifact art = (TLArtifact)listOfArtifacts.get_Item(artifactId);
            String processedText = SentenceSplitter.process(art.get_Text());
            art.set_Text(processedText);
        }

        super.get_Workspace().Store("listOfArtifacts", listOfArtifacts);
    }
}
