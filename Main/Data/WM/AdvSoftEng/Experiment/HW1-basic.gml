<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>b66473ee-65b3-42ab-98a5-6b68960fdbec</Id>
    <Name>Homework #1 Basic</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
    <Author>Evan Moritz</Author>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="-47" Y="-76">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="111" Y="431">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="8cd92211-281c-4ac4-9bc6-dc300e3a45d8">
    <SerializedVertexData Version="1" X="-210" Y="-9">
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
    <SerializedVertexData Version="1" X="-47" Y="-6">
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
  <node id="f491a95c-e72c-4e9d-9474-90ab69cc2d27">
    <SerializedVertexData Version="1" X="-47" Y="44">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Queries Vectorizer" ComponentMetadataDefinitionID="b4193270-7cb9-4814-b7b4-33a6758b3041">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="ListOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>Features</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="DocumentFrequencies" Type="AdvSoftEng.Types.DocumentVector" />
              <OutputAs>Ignore</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="DocumentVectors" Type="AdvSoftEng.Types.DocumentVectorCollection" />
              <OutputAs>Queries</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Representation</Name>
              <DisplayName>Representation</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Boolean</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="828b21df-8561-4655-8173-57a3d1f416e3">
    <SerializedVertexData Version="1" X="-212" Y="40">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Corpus Vectors" ComponentMetadataDefinitionID="b4193270-7cb9-4814-b7b4-33a6758b3041">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="ListOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>Corpus</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="DocumentFrequencies" Type="AdvSoftEng.Types.DocumentVector" />
              <OutputAs>Corpus_DF</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="DocumentVectors" Type="AdvSoftEng.Types.DocumentVectorCollection" />
              <OutputAs>Corpus_Vectors</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Representation</Name>
              <DisplayName>Representation</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Ordinal</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="736145b5-35df-49d8-b5a0-13de46bc3791">
    <SerializedVertexData Version="1" X="-281" Y="105">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Corpus TF" ComponentMetadataDefinitionID="0c5bb0a2-f0cd-4cb4-9006-a2c90b9dd600">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="DocumentVectors" Type="AdvSoftEng.Types.DocumentVectorCollection" />
              <MappedTo>Corpus_Vectors</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="NormalizedVectors" Type="AdvSoftEng.Types.NormalizedVectorCollection" />
              <OutputAs>Corpus_TF</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="NormalizedVectorLengths" Type="AdvSoftEng.Types.NormalizedVector" />
              <OutputAs>Corpus_Lengths</OutputAs>
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
  <node id="0de980c1-8639-45a0-9349-a6c0f7f6e1a6">
    <SerializedVertexData Version="1" X="-153" Y="103">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Corpus IDF" ComponentMetadataDefinitionID="d3d9e709-2448-4ee8-b124-01c5b4f8f94a">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="NumberOfDocuments" Type="System.Int32" />
              <MappedTo>NumberOfDocuments</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="DocumentFrequencies" Type="AdvSoftEng.Types.DocumentVector" />
              <MappedTo>Corpus_DF</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="InverseDocumentFrequencies" Type="AdvSoftEng.Types.NormalizedVector" />
              <OutputAs>Corpus_IDF</OutputAs>
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
  <node id="2e4d61c7-64fd-4b49-a3a9-191f19ad7d25">
    <SerializedVertexData Version="1" X="-212" Y="169">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Corpus TFIDF" ComponentMetadataDefinitionID="63058b8a-5f0a-4460-adb3-56b139ac1642">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="NormalizedVectors" Type="AdvSoftEng.Types.NormalizedVectorCollection" />
              <MappedTo>Corpus_TF</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="InverseDocumentFrequencies" Type="AdvSoftEng.Types.NormalizedVector" />
              <MappedTo>Corpus_IDF</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="WeightedVectors" Type="AdvSoftEng.Types.NormalizedVectorCollection" />
              <OutputAs>Corpus_TFIDF</OutputAs>
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
  <node id="e5f37e65-e055-4234-a6d6-65409bd87ade">
    <SerializedVertexData Version="1" X="-47" Y="241">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Cosine Similarity" ComponentMetadataDefinitionID="82c70245-5224-4831-9fa3-840489a483de">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="Queries" Type="AdvSoftEng.Types.DocumentVectorCollection" />
              <MappedTo>Queries</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="Documents" Type="AdvSoftEng.Types.NormalizedVectorCollection" />
              <MappedTo>Corpus_TFIDF</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="DocumentLengths" Type="AdvSoftEng.Types.NormalizedVector" />
              <MappedTo>Corpus_Lengths</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="SimilarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <OutputAs>SimilarityMatrix</OutputAs>
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
  <node id="ab2287ad-f47a-4fa8-b3fc-f0ab57ed497e">
    <SerializedVertexData Version="1" X="112" Y="310">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Effectiveness Measures" ComponentMetadataDefinitionID="f75f9452-121f-4619-95cd-1de81fc6d625">
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
  <edge id="5fe01b3f-0666-4d03-b94e-4b357dfa7961" source="8cd92211-281c-4ac4-9bc6-dc300e3a45d8" target="828b21df-8561-4655-8173-57a3d1f416e3" />
  <edge id="a724a305-5e95-43e5-8919-a771fb4b61d0" source="3d52aeab-96ef-493c-90f4-fe59c2e93784" target="ab2287ad-f47a-4fa8-b3fc-f0ab57ed497e" />
  <edge id="33b913f9-d439-4c32-9e2b-d4b7412ffa67" source="efc5a760-ebbe-4873-85e7-ca6936d47675" target="f491a95c-e72c-4e9d-9474-90ab69cc2d27" />
  <edge id="a8282a20-1a8f-472f-96f3-826907aad99e" source="f491a95c-e72c-4e9d-9474-90ab69cc2d27" target="e5f37e65-e055-4234-a6d6-65409bd87ade" />
  <edge id="4132ea81-6223-4327-b4a8-311dd23febf0" source="828b21df-8561-4655-8173-57a3d1f416e3" target="736145b5-35df-49d8-b5a0-13de46bc3791" />
  <edge id="8502539d-2815-42d9-9e9b-8589a4bfa450" source="828b21df-8561-4655-8173-57a3d1f416e3" target="0de980c1-8639-45a0-9349-a6c0f7f6e1a6" />
  <edge id="e042c609-aa9f-4b9d-91a9-56993717b6f9" source="736145b5-35df-49d8-b5a0-13de46bc3791" target="2e4d61c7-64fd-4b49-a3a9-191f19ad7d25" />
  <edge id="2b724141-dedb-4c12-87b6-784774257cdb" source="0de980c1-8639-45a0-9349-a6c0f7f6e1a6" target="2e4d61c7-64fd-4b49-a3a9-191f19ad7d25" />
  <edge id="38e6a29b-e736-46f1-abfb-3446cc9085e4" source="2e4d61c7-64fd-4b49-a3a9-191f19ad7d25" target="e5f37e65-e055-4234-a6d6-65409bd87ade" />
  <edge id="15df1962-4e6b-499b-8320-fbc7d4d95981" source="e5f37e65-e055-4234-a6d6-65409bd87ade" target="ab2287ad-f47a-4fa8-b3fc-f0ab57ed497e" />
  <edge id="2fdcc319-fe0b-4e9a-b7ac-b40ca698261b" source="ab2287ad-f47a-4fa8-b3fc-f0ab57ed497e" target="End" />
</graph>