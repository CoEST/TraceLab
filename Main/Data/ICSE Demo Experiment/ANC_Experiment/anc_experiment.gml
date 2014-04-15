<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>22e2bc93-a90e-49cc-8f49-0134746e22e1</Id>
    <Name>ANC Experiment</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
    <Author>Re Lab</Author>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="299.502" Y="86.72">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="603.482" Y="417.72">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="a76a2104-bc00-4e01-bfc4-dc51fa2316bc">
    <SerializedVertexData Version="1" X="413" Y="223.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="Splitter" ComponentMetadataDefinitionID="74283a1e-49a8-11e0-82b1-4bcadfd72085">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>sourceArtifacts</OutputAs>
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
  <node id="705d4d6f-a712-44b9-b33e-375ba6d48f21">
    <SerializedVertexData Version="1" X="215" Y="281">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="Preprocess Target Artifacts" ComponentMetadataDefinitionID="16534c79-8e9c-4c8e-9d6f-f3b27681b2a3">
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
              <ImportAs>targetArtifacts</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>33a2b2e1-eace-47ab-8e00-8394668ca3e9:Path</Name>
              <DisplayName>Artifacts Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\challenge1_HIPPA\HIPPA\2CCHIT.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>1ef84c31-abc8-4f9e-aafb-a75a2bf87c50:Path</Name>
              <DisplayName>StopwordsImporter Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>False</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\challenge1_HIPPA\HIPPA\stopwords.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="1d5b5d08-65e5-4c75-a40a-c7ba6966e781">
    <SerializedVertexData Version="1" X="215.27" Y="355.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="TFIDF Dictionary Index Builder" ComponentMetadataDefinitionID="1c30b7b5-3e04-433d-817f-b0be187b154f">
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
  <node id="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b">
    <SerializedVertexData Version="1" X="603.398" Y="356.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="ANC Tracer Component" ComponentMetadataDefinitionID="ab30ead2-46ac-11e0-8ad8-3a8edfd72085">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="dictionaryIndex" Type="TraceLabSDK.Types.TLDictionaryIndex" />
              <MappedTo>dictionaryIndex</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="ancTermsWeights" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <MappedTo>ANC</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="targetArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>targetArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <OutputAs>ANCsimilarityMatrix</OutputAs>
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
  <node id="12473c3f-ec5b-483a-88a4-9f3615b3b082">
    <SerializedVertexData Version="1" X="217" Y="166">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="Import target artifacts" ComponentMetadataDefinitionID="d98bd1e6-1db5-11e0-bfa9-3ee4dfd72085">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>targetArtifacts</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Path</Name>
              <DisplayName>Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\challenge1_HIPPA\Target-Artifacts-CCHIT.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="a217af1d-c6db-46c7-9d08-4fe5fc173175">
    <SerializedVertexData Version="1" X="602" Y="163.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="ANC Importer" ComponentMetadataDefinitionID="4010e6c9-7b42-413d-aaf4-3e072b36457b">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="ANC" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <ImportAs>ANC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="True">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>PathToAnc</Name>
              <DisplayName>PathToAnc</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\ICSE Demo Experiment\ANC_Experiment\ANC_stemmed.csv</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="da047f2e-d18a-4647-9ee6-c2cc9384b51d">
    <SerializedVertexData Version="1" X="414" Y="281">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="Preprocess Source Artifacts" ComponentMetadataDefinitionID="16534c79-8e9c-4c8e-9d6f-f3b27681b2a3">
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
              <ImportAs>sourceArtifacts</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>33a2b2e1-eace-47ab-8e00-8394668ca3e9:Path</Name>
              <DisplayName>Artifacts Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\challenge1_HIPPA\HIPPA\2CCHIT.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>1ef84c31-abc8-4f9e-aafb-a75a2bf87c50:Path</Name>
              <DisplayName>StopwordsImporter Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>False</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\challenge1_HIPPA\HIPPA\stopwords.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="01eb2c55-a408-40e7-92d5-94b3e7d068df">
    <SerializedVertexData Version="1" X="413" Y="165">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.3.2.0, Culture=neutral, PublicKeyToken=null" Label="Import source artifacts" ComponentMetadataDefinitionID="d98bd1e6-1db5-11e0-bfa9-3ee4dfd72085">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>sourceArtifacts</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Path</Name>
              <DisplayName>Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\challenge1_HIPPA\Source-Artifacts-HIPAA_Goal_Model.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <edge id="44d4a4c5-36b5-444a-95c6-ba5e0fcd44dc" source="Start" target="a217af1d-c6db-46c7-9d08-4fe5fc173175" />
  <edge id="ab1ea357-4c5b-48b3-b7c5-e48c46a87324" source="Start" target="12473c3f-ec5b-483a-88a4-9f3615b3b082" />
  <edge id="aab6fd91-473b-4c68-9f8d-71c303ab1144" source="Start" target="01eb2c55-a408-40e7-92d5-94b3e7d068df" />
  <edge id="8ed6330b-f5fc-4e25-8a99-7a8221c1fa25" source="a76a2104-bc00-4e01-bfc4-dc51fa2316bc" target="da047f2e-d18a-4647-9ee6-c2cc9384b51d" />
  <edge id="b9b9d357-74fe-4562-b3d5-da95d70cb585" source="705d4d6f-a712-44b9-b33e-375ba6d48f21" target="1d5b5d08-65e5-4c75-a40a-c7ba6966e781" />
  <edge id="2e82d7ee-2f83-4806-b298-f5d8479fe808" source="1d5b5d08-65e5-4c75-a40a-c7ba6966e781" target="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" />
  <edge id="d75ea88b-8ea5-422f-b3b9-8913ae41842a" source="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" target="End" />
  <edge id="10f67a7e-df38-455f-80e1-b0775ae67812" source="12473c3f-ec5b-483a-88a4-9f3615b3b082" target="705d4d6f-a712-44b9-b33e-375ba6d48f21" />
  <edge id="acff96ee-27f2-498e-abe6-1eb05b8c6677" source="a217af1d-c6db-46c7-9d08-4fe5fc173175" target="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" />
  <edge id="daa807d0-6f4f-48d7-b982-36323b53362f" source="da047f2e-d18a-4647-9ee6-c2cc9384b51d" target="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" />
  <edge id="e370b872-d0e3-4b7b-8a60-15fd9c149b21" source="01eb2c55-a408-40e7-92d5-94b3e7d068df" target="a76a2104-bc00-4e01-bfc4-dc51fa2316bc" />
</graph>