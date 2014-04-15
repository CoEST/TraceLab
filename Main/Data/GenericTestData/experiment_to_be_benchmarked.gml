<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>7f487a5a-49b3-4abe-a5bf-092872ff87fc</Id>
    <Name>Standard VSM</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
    <Description>This is standard vector space model experiment.</Description>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="654.965" Y="118.9">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="767.815" Y="440.9">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="fc5b7c4d-5555-4b91-ac7f-7ae47ce33969">
    <SerializedVertexData Version="1" X="767" Y="214">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="Source Artifacts Preprocessor" ComponentMetadataDefinitionID="16534c79-8e9c-4c8e-9d6f-f3b27681b2a3">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>sourceArtifacts</OutputAs>
            </OutputItem>
          </Output>
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="originalListOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>originalSourceArtifacts</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>33a2b2e1-eace-47ab-8e00-8394668ca3e9:Path</Name>
              <DisplayName>Artifacts Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=2c83cea59a8bb151</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Hippa\11HIPAA_Goal_Model.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>1ef84c31-abc8-4f9e-aafb-a75a2bf87c50:Path</Name>
              <DisplayName>StopwordsImporter Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=2c83cea59a8bb151</ValueType>
              <Visible>False</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Hippa\stopwords.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="9464b3fd-f186-4539-a08c-2ad95539c72e">
    <SerializedVertexData Version="1" X="531.525" Y="348.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="TFIDF Dictionary Index Builder" ComponentMetadataDefinitionID="1c30b7b5-3e04-433d-817f-b0be187b154f">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>targetArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="dictionaryIndex" Type="TraceLabSDK.Types.TLDictionaryIndex" />
              <OutputAs>dictionaryIndex</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="69ec93fa-addb-4b42-95be-87e571ff561a">
    <SerializedVertexData Version="1" X="766.525" Y="352.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="Tracer Component" ComponentMetadataDefinitionID="ad26cc51-5234-4b1a-abfa-b631bc0f2382">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="targetArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>targetArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="dictionaryIndex" Type="TraceLabSDK.Types.TLDictionaryIndex" />
              <MappedTo>dictionaryIndex</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <OutputAs>similarityMatrix</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>SimilarityMetric</Name>
              <DisplayName>SimilarityMetric</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Cosine</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="fe86816a-934f-403c-b81f-a1dab47d6ba1">
    <SerializedVertexData Version="1" X="530" Y="216">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="Target Artifacts Preprocessor" ComponentMetadataDefinitionID="16534c79-8e9c-4c8e-9d6f-f3b27681b2a3">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>targetArtifacts</OutputAs>
            </OutputItem>
          </Output>
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="originalListOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>originalTargetArtifacts</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>33a2b2e1-eace-47ab-8e00-8394668ca3e9:Path</Name>
              <DisplayName>Artifacts Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=2c83cea59a8bb151</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Hippa\2CCHIT.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>1ef84c31-abc8-4f9e-aafb-a75a2bf87c50:Path</Name>
              <DisplayName>StopwordsImporter Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=2c83cea59a8bb151</ValueType>
              <Visible>False</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Hippa\stopwords.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <edge id="8d19ca83-77ae-4631-97b5-bd6c3d79f0e7" source="Start" target="fe86816a-934f-403c-b81f-a1dab47d6ba1" />
  <edge id="f3fb6968-1490-46b9-958e-a1e64a9c3e35" source="Start" target="fc5b7c4d-5555-4b91-ac7f-7ae47ce33969" />
  <edge id="260e2ea6-3055-4bbe-b5bd-9e897b838bef" source="fc5b7c4d-5555-4b91-ac7f-7ae47ce33969" target="69ec93fa-addb-4b42-95be-87e571ff561a" />
  <edge id="f4baf7da-d11d-43c4-8a5e-9be67d9b3f96" source="9464b3fd-f186-4539-a08c-2ad95539c72e" target="69ec93fa-addb-4b42-95be-87e571ff561a" />
  <edge id="4ea84a68-302b-4a51-af36-bcaeb34088ab" source="69ec93fa-addb-4b42-95be-87e571ff561a" target="End" />
  <edge id="18852fcd-642d-42e0-b2fe-21069cd5ce60" source="fe86816a-934f-403c-b81f-a1dab47d6ba1" target="9464b3fd-f186-4539-a08c-2ad95539c72e" />
</graph>