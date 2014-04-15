<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>b66473ee-65b3-42ab-98a5-6b68960fdbec</Id>
    <Name>Homework #1 Comparison</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
    <Author>Evan Moritz</Author>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="-51" Y="-76">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="110" Y="231">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="8cd92211-281c-4ac4-9bc6-dc300e3a45d8">
    <SerializedVertexData Version="1" X="-204" Y="-9">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Corpus Importer" ComponentMetadataDefinitionID="ca563e24-06f4-4dd0-bc62-544ca1737d4e">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="NumberOfDocuments" Type="System.Int32" />
              <OutputAs>NumberOfDocuments</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="ListOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>Corpus</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Identifiers</Name>
              <DisplayName>Identifiers</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\WM\AdvSoftEng\Experiment\Rhino\RhinoCorpusMapping.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>Documents</Name>
              <DisplayName>Documents</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\WM\AdvSoftEng\Experiment\Rhino\RhinoCorpus.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="3d52aeab-96ef-493c-90f4-fe59c2e93784">
    <SerializedVertexData Version="1" X="112" Y="-9">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Gold Set Importer" ComponentMetadataDefinitionID="6743f551-a058-4b86-b335-764a7d9d4771">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="AnswerMapping" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <OutputAs>Gold Set</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Directory</Name>
              <DisplayName>Directory</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\WM\AdvSoftEng\Experiment\Rhino\RhinoFeaturesToGoldSetMethodsMapping</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="efc5a760-ebbe-4873-85e7-ca6936d47675">
    <SerializedVertexData Version="1" X="-50" Y="-6">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Features Importer" ComponentMetadataDefinitionID="ca563e24-06f4-4dd0-bc62-544ca1737d4e">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="NumberOfDocuments" Type="System.Int32" />
              <OutputAs>NumberOfFeatures</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="ListOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>Features</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Identifiers</Name>
              <DisplayName>Identifiers</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\WM\AdvSoftEng\Experiment\Rhino\RhinoListOfFeatures.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>Documents</Name>
              <DisplayName>Documents</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\WM\AdvSoftEng\Experiment\Rhino\RhinoQueries.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="fcfb8498-da09-4f3a-a198-3688a9f1d1b9">
    <SerializedVertexData Version="1" X="-201" Y="56">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="TraceLab TFIDF" ComponentMetadataDefinitionID="1c30b7b5-3e04-433d-817f-b0be187b154f">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>Corpus</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="dictionaryIndex" Type="TraceLabSDK.Types.TLDictionaryIndex" />
              <OutputAs>CorpusIndex</OutputAs>
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
  <node id="0cde6f5b-d54e-4bae-8172-a662506db276">
    <SerializedVertexData Version="1" X="-50" Y="103">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="TraceLab Tracer" ComponentMetadataDefinitionID="ad26cc51-5234-4b1a-abfa-b631bc0f2382">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="targetArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>Corpus</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="dictionaryIndex" Type="TraceLabSDK.Types.TLDictionaryIndex" />
              <MappedTo>CorpusIndex</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>Features</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <OutputAs>SimilarityMatrix</OutputAs>
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
  <node id="2afcc6b1-e33e-4cdb-88b7-b49aa94e9002">
    <SerializedVertexData Version="1" X="110" Y="154">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Effectiveness Measure" ComponentMetadataDefinitionID="f75f9452-121f-4619-95cd-1de81fc6d625">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="GoldSet" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>Gold Set</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="Queries" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>Features</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="SimilarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>SimilarityMatrix</MappedTo>
            </InputItem>
          </Input>
          <Output />
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>AllMethodsFile</Name>
              <DisplayName>AllMethodsFile</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\WM\AdvSoftEng\Experiment\EffectivenessAllMethods.txt</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>BestMethodsFile</Name>
              <DisplayName>BestMethodsFile</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\WM\AdvSoftEng\Experiment\EffectivenessBestMethods.txt</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <edge id="d3c6bbdb-4a7a-4620-91b4-356aaf3508fc" source="Start" target="8cd92211-281c-4ac4-9bc6-dc300e3a45d8" />
  <edge id="bff18e69-cf4b-42bf-82b7-ed9c37f2f4eb" source="Start" target="3d52aeab-96ef-493c-90f4-fe59c2e93784" />
  <edge id="089e3ffb-0cc8-49a6-9f5c-1288ce8d13b1" source="Start" target="efc5a760-ebbe-4873-85e7-ca6936d47675" />
  <edge id="88c15388-23bf-408c-bcff-003108eec8fd" source="8cd92211-281c-4ac4-9bc6-dc300e3a45d8" target="fcfb8498-da09-4f3a-a198-3688a9f1d1b9" />
  <edge id="1ea1dee2-cbbb-472f-8f36-49b4599ffc9f" source="3d52aeab-96ef-493c-90f4-fe59c2e93784" target="2afcc6b1-e33e-4cdb-88b7-b49aa94e9002" />
  <edge id="6bb88593-3d76-47e5-ab48-3c48b50baf54" source="efc5a760-ebbe-4873-85e7-ca6936d47675" target="0cde6f5b-d54e-4bae-8172-a662506db276" />
  <edge id="fc6fd11b-6709-40e6-9e6b-869f4c01df7f" source="fcfb8498-da09-4f3a-a198-3688a9f1d1b9" target="0cde6f5b-d54e-4bae-8172-a662506db276" />
  <edge id="2d53dcd6-05ba-4c8a-a975-8a5ba87b67de" source="0cde6f5b-d54e-4bae-8172-a662506db276" target="2afcc6b1-e33e-4cdb-88b7-b49aa94e9002" />
  <edge id="c0b9b35f-2181-4d65-8c43-308ad9141b7b" source="2afcc6b1-e33e-4cdb-88b7-b49aa94e9002" target="End" />
</graph>