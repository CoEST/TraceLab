<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>c6d4d6f6-c326-4ed0-85f5-50c9d249f3a8</Id>
    <Name>Standard VSM</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
    <Description>This is standard vector space model experiment.</Description>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="654.965" Y="118.9">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="1018.815" Y="574.9">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="017869a3-4a2f-452c-98fc-8a7491e9a46c">
    <SerializedVertexData Version="1" X="1019" Y="215.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="AnswerMatrixImporter" ComponentMetadataDefinitionID="e95dbb36-1db8-11e0-8f08-2ee7dfd72085">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>answerMatrix</ImportAs>
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
                  <Relative>Data\challenge1_HIPPA\AnswerMatrix.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>Separator</Name>
              <DisplayName>Separator</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>,</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="fc5b7c4d-5555-4b91-ac7f-7ae47ce33969">
    <SerializedVertexData Version="1" X="767" Y="214">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Source Artifacts Preprocessor" ComponentMetadataDefinitionID="16534c79-8e9c-4c8e-9d6f-f3b27681b2a3">
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
  <node id="9464b3fd-f186-4539-a08c-2ad95539c72e">
    <SerializedVertexData Version="1" X="531.525" Y="348.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="TFIDF Dictionary Index Builder" ComponentMetadataDefinitionID="1c30b7b5-3e04-433d-817f-b0be187b154f">
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
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Tracer Component" ComponentMetadataDefinitionID="ad26cc51-5234-4b1a-abfa-b631bc0f2382">
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
  <node id="915976dc-d949-4839-844a-be56f4e8b88e">
    <SerializedVertexData Version="1" X="1018.215" Y="428.04">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Results Metric Computation" ComponentMetadataDefinitionID="7cba9f2d-730b-43c3-8ba8-ed9fe53650fb">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>answerMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>similarityMatrix</MappedTo>
            </InputItem>
          </Input>
          <Output>
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
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Threshold</Name>
              <DisplayName>Threshold</DisplayName>
              <ValueType>System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <double>0.01</double>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="1dfca095-8704-4421-aab0-b22bd1329c83">
    <SerializedVertexData Version="1" X="1018" Y="506">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Results Charts (Close on load for performance profiling)" ComponentMetadataDefinitionID="63d1c531-0343-4bcb-bcc0-1168e28a2efd">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <MappedTo>precisionData</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>similarityMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="averagePrecisionData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <MappedTo>averagePrecisionData</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="recallData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <MappedTo>recallData</MappedTo>
            </InputItem>
          </Input>
          <Output />
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Title</Name>
              <DisplayName>Title</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Results</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SeriesTitle</Name>
              <DisplayName>SeriesTitle</DisplayName>
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
  <node id="fe86816a-934f-403c-b81f-a1dab47d6ba1">
    <SerializedVertexData Version="1" X="530" Y="216">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Target Artifacts Preprocessor" ComponentMetadataDefinitionID="16534c79-8e9c-4c8e-9d6f-f3b27681b2a3">
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
  <edge id="c778657e-ae47-4ad3-985a-4778956ea4b0" source="Start" target="017869a3-4a2f-452c-98fc-8a7491e9a46c" />
  <edge id="8d19ca83-77ae-4631-97b5-bd6c3d79f0e7" source="Start" target="fe86816a-934f-403c-b81f-a1dab47d6ba1" />
  <edge id="f3fb6968-1490-46b9-958e-a1e64a9c3e35" source="Start" target="fc5b7c4d-5555-4b91-ac7f-7ae47ce33969" />
  <edge id="d3a30ac5-bcce-4354-8f2b-9e7307c59d0d" source="017869a3-4a2f-452c-98fc-8a7491e9a46c" target="915976dc-d949-4839-844a-be56f4e8b88e" />
  <edge id="260e2ea6-3055-4bbe-b5bd-9e897b838bef" source="fc5b7c4d-5555-4b91-ac7f-7ae47ce33969" target="69ec93fa-addb-4b42-95be-87e571ff561a" />
  <edge id="f4baf7da-d11d-43c4-8a5e-9be67d9b3f96" source="9464b3fd-f186-4539-a08c-2ad95539c72e" target="69ec93fa-addb-4b42-95be-87e571ff561a" />
  <edge id="b6845856-e64f-4741-8e61-12e9c47b9e60" source="69ec93fa-addb-4b42-95be-87e571ff561a" target="915976dc-d949-4839-844a-be56f4e8b88e" />
  <edge id="6cf1b02d-e409-4973-9627-adcb000e5f3e" source="915976dc-d949-4839-844a-be56f4e8b88e" target="1dfca095-8704-4421-aab0-b22bd1329c83" />
  <edge id="99ee2d79-1603-4360-8e52-2a1a544124d2" source="1dfca095-8704-4421-aab0-b22bd1329c83" target="End" />
  <edge id="18852fcd-642d-42e0-b2fe-21069cd5ce60" source="fe86816a-934f-403c-b81f-a1dab47d6ba1" target="9464b3fd-f186-4539-a08c-2ad95539c72e" />
</graph>