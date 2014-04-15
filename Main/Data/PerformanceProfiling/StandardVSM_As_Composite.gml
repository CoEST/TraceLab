<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>d8c02313-6925-4767-b997-fe2c275c44d9</Id>
    <Name>Standard VSM as Composite</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
    <Description>Experiment designed to do Performance Profiling. 2 levels of composite components.</Description>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="200" Y="100">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="203" Y="207">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="e85c290c-4f54-4132-884b-a01ecff73857">
    <SerializedVertexData Version="1" X="201" Y="153">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Standard VSM Composite" ComponentMetadataDefinitionID="529694a2-dfd1-4342-9ffe-46d5068dd75d">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="targetArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>targetArtifacts</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>sourceArtifacts</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="recallData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <OutputAs>recallData</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <OutputAs>precisionData</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="averagePrecisionData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <OutputAs>averagePrecisionData</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="dictionaryIndex" Type="TraceLabSDK.Types.TLDictionaryIndex" />
              <OutputAs>dictionaryIndex</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <OutputAs>similarityMatrix</OutputAs>
            </OutputItem>
          </Output>
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>answerMatrix</ImportAs>
            </ImportItem>
            <ImportItem>
              <ImportItemDefinition Name="originalTargetArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>originalTargetArtifacts</ImportAs>
            </ImportItem>
            <ImportItem>
              <ImportItemDefinition Name="originalSourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>originalSourceArtifacts</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>017869a3-4a2f-452c-98fc-8a7491e9a46c:Path</Name>
              <DisplayName>AnswerMatrixImporter Path</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\challenge1_HIPPA\AnswerMatrix.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>017869a3-4a2f-452c-98fc-8a7491e9a46c:Separator</Name>
              <DisplayName>AnswerMatrixImporter Separator</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>,</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>fe86816a-934f-403c-b81f-a1dab47d6ba1:33a2b2e1-eace-47ab-8e00-8394668ca3e9:Path</Name>
              <DisplayName>Target Artifacts Preprocessor Artifacts Path</DisplayName>
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
            <PropertyObject>
              <Version>2</Version>
              <Name>fe86816a-934f-403c-b81f-a1dab47d6ba1:1ef84c31-abc8-4f9e-aafb-a75a2bf87c50:Path</Name>
              <DisplayName>Target Artifacts Preprocessor StopwordsImporter Path</DisplayName>
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
            <PropertyObject>
              <Version>2</Version>
              <Name>fc5b7c4d-5555-4b91-ac7f-7ae47ce33969:33a2b2e1-eace-47ab-8e00-8394668ca3e9:Path</Name>
              <DisplayName>Source Artifacts Preprocessor Artifacts Path</DisplayName>
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
            <PropertyObject>
              <Version>2</Version>
              <Name>fc5b7c4d-5555-4b91-ac7f-7ae47ce33969:1ef84c31-abc8-4f9e-aafb-a75a2bf87c50:Path</Name>
              <DisplayName>Source Artifacts Preprocessor StopwordsImporter Path</DisplayName>
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
            <PropertyObject>
              <Version>2</Version>
              <Name>915976dc-d949-4839-844a-be56f4e8b88e:Threshold</Name>
              <DisplayName>Results Metric Computation Threshold</DisplayName>
              <ValueType>System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <double>0.01</double>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>69ec93fa-addb-4b42-95be-87e571ff561a:SimilarityMetric</Name>
              <DisplayName>Tracer Component SimilarityMetric</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Cosine</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>1dfca095-8704-4421-aab0-b22bd1329c83:Title</Name>
              <DisplayName>Results Charts (Close on load for performance profiling) Title</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Results</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>1dfca095-8704-4421-aab0-b22bd1329c83:SeriesTitle</Name>
              <DisplayName>Results Charts (Close on load for performance profiling) SeriesTitle</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CCHIT</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <edge id="1483a229-6668-49fc-9fb6-2746ed3e7fc4" source="Start" target="e85c290c-4f54-4132-884b-a01ecff73857" />
  <edge id="15169b47-90fc-4c26-95dd-687741b6feba" source="e85c290c-4f54-4132-884b-a01ecff73857" target="End" />
</graph>