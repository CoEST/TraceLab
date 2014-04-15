<?xml version="1.0" encoding="utf-8"?>
<graph>
  <WorkflowInfo>
    <Version>1</Version>
    <Id>9a78da5b-4aa6-4506-bc18-d04dbcf92c2c</Id>
    <Name>eTour</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
  </WorkflowInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="774.502" Y="216.72">
      <Metadata type="TraceLab.Core.Models.StartNodeMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="1127.482" Y="749.72">
      <Metadata type="TraceLab.Core.Models.EndNodeMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="45a3ff82-8ab7-49cc-923e-25e236618b5f">
    <SerializedVertexData Version="1" X="1032" Y="369.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Stopwords Remover" ComponentMetadataDefinitionID="449f5e1f-b66e-4db1-ac70-ba0a0a54a3ba">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="stopwords" Type="TraceLabSDK.Types.TLStopwords" />
              <MappedTo>stopwords</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>targetArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>targetArtifacts</OutputAs>
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
  <node id="c1f3cd70-9772-4f91-8483-a108706d22e4">
    <SerializedVertexData Version="1" X="1353" Y="321.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Cleanup Preprocessor" ComponentMetadataDefinitionID="85cb9977-f2a6-426f-b3df-bb0cca26b276">
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
  <node id="49e3908e-326b-405d-b464-2b5335b2f511">
    <SerializedVertexData Version="1" X="1035" Y="321.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Cleanup Preprocessor" ComponentMetadataDefinitionID="85cb9977-f2a6-426f-b3df-bb0cca26b276">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>targetArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>targetArtifacts</OutputAs>
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
  <node id="a76a2104-bc00-4e01-bfc4-dc51fa2316bc">
    <SerializedVertexData Version="1" X="1312" Y="247.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Splitter" ComponentMetadataDefinitionID="74283a1e-49a8-11e0-82b1-4bcadfd72085">
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
  <node id="acf64625-f66b-4346-b042-c3bc309b9e23">
    <SerializedVertexData Version="1" X="1352" Y="367.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Stopwords Remover" ComponentMetadataDefinitionID="449f5e1f-b66e-4db1-ac70-ba0a0a54a3ba">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="stopwords" Type="TraceLabSDK.Types.TLStopwords" />
              <MappedTo>stopwords</MappedTo>
            </InputItem>
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
  <node id="f8428671-2c76-4dac-a198-8b7f6c9ac947">
    <SerializedVertexData Version="1" X="1193" Y="316.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="StopwordsImporter" ComponentMetadataDefinitionID="b450dc72-1db6-11e0-bb91-fbe4dfd72085">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="stopwords" Type="TraceLabSDK.Types.TLStopwords" />
              <ImportAs>stopwords</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Path</Name>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Value>
                <FilePath xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                  <Value>C:\p4root\RELab\branches\aczauderna\Data\challenge1_HIPPA\HIPPA\stopwords.txt</Value>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="c67125c5-afa1-4f66-adf7-66afb0ffa30c">
    <SerializedVertexData Version="1" X="784" Y="601.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Results Metric Computation" ComponentMetadataDefinitionID="7cba9f2d-730b-43c3-8ba8-ed9fe53650fb">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>traceMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>ANCsimilarityMatrix</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="recallData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <OutputAs>ANCrecallData</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <OutputAs>ANCprecisionData</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="averagePrecisionData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <OutputAs>ANCaveragePrecisionData</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Threshold</Name>
              <ValueType>System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <double>1</double>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="9719fc48-5226-4fb2-a6b1-4c14e04735de">
    <SerializedVertexData Version="1" X="876.671" Y="692.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Results Charts" ComponentMetadataDefinitionID="ed3ee047-cb6c-4b47-a40d-c36d7dd8cec5">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>ANCsimilarityMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <MappedTo>precisionData</MappedTo>
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
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>ANC Graphs</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SeriesTitle</Name>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>ETour</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="1d5b5d08-65e5-4c75-a40a-c7ba6966e781">
    <SerializedVertexData Version="1" X="1179.27" Y="537.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="TFIDF Dictionary Index Builder" ComponentMetadataDefinitionID="1c30b7b5-3e04-433d-817f-b0be187b154f">
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
  <node id="b8b90a3e-41ca-4063-9b78-32745a7d4329">
    <SerializedVertexData Version="1" X="1481.47" Y="508.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Tracer Component" ComponentMetadataDefinitionID="ad26cc51-5234-4b1a-abfa-b631bc0f2382">
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
              <OutputAs>Standard_TFIDF_similarityMatrix</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>SimilarityMetric</Name>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>Cosine</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b">
    <SerializedVertexData Version="1" X="838.398" Y="507.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="ANC Tracer Component" ComponentMetadataDefinitionID="ab30ead2-46ac-11e0-8ad8-3a8edfd72085">
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
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>Cosine</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="179277ef-9c53-4e88-b4f2-0e0398e39ef2">
    <SerializedVertexData Version="1" X="1027" Y="223.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Easy ClinicImporter" ComponentMetadataDefinitionID="96c70bef-0b4f-41e4-99cc-c24b15725d61">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="targetArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>targetArtifacts</ImportAs>
            </ImportItem>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>traceMatrix</ImportAs>
            </ImportItem>
            <ImportItem>
              <ImportItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>sourceArtifacts</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="True">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>SourceFolder</Name>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>c:\p4root\RELab\branches\aczauderna\Data\ANC-Data\eTour\docs\UC\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>TargetFolder</Name>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>c:\p4root\RELab\branches\aczauderna\Data\ANC-Data\eTour\docs\CC\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>TraceMatrixFile</Name>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Value>
                <FilePath xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                  <Value>C:\p4root\RELab\branches\aczauderna\Data\ANC-Data\eTour\traceability matrix.txt</Value>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="a217af1d-c6db-46c7-9d08-4fe5fc173175">
    <SerializedVertexData Version="1" X="757" Y="299.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="ANC Importer" ComponentMetadataDefinitionID="4010e6c9-7b42-413d-aaf4-3e072b36457b">
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
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Value>
                <FilePath xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                  <Value>C:\p4root\RELab\branches\aczauderna\Data\ANC-Data\ANC_stemmed.csv</Value>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="42876d9e-a88c-405d-8997-55434b47642f">
    <SerializedVertexData Version="1" X="1455.955" Y="694.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Results Charts" ComponentMetadataDefinitionID="ed3ee047-cb6c-4b47-a40d-c36d7dd8cec5">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>Standard_TFIDF_similarityMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricsContainer" />
              <MappedTo>precisionData</MappedTo>
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
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>Standard TF-IDF Graph</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SeriesTitle</Name>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>Etour</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="ead4e4e1-6e97-48e3-bd78-9dc77c022059">
    <SerializedVertexData Version="1" X="1355" Y="416.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="English Porter Stemmer" ComponentMetadataDefinitionID="420775e4-1afc-4142-9145-f32a7d1ed8c4">
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
  <node id="7d31deaa-cef1-4334-9208-5c1f9703a274">
    <SerializedVertexData Version="1" X="1025" Y="414.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="English Porter Stemmer" ComponentMetadataDefinitionID="420775e4-1afc-4142-9145-f32a7d1ed8c4">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>targetArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>targetArtifacts</OutputAs>
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
  <node id="a86a843d-41e6-4ffa-aa20-1307223103e9">
    <SerializedVertexData Version="1" X="1508" Y="599.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Results Metric Computation" ComponentMetadataDefinitionID="7cba9f2d-730b-43c3-8ba8-ed9fe53650fb">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>traceMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>Standard_TFIDF_similarityMatrix</MappedTo>
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
              <ValueType>System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <double>1</double>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="5f60a7e0-0126-4ac9-b498-175d9941f413">
    <SerializedVertexData Version="1" X="1009" Y="648.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Average average precision Metric Computation" ComponentMetadataDefinitionID="6d869f0e-46b8-11e0-9506-4c9adfd72085">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>traceMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>ANCsimilarityMatrix</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="averageAveragePrecision" Type="System.Double" />
              <OutputAs>ANCaverageAveragePrecision</OutputAs>
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
  <node id="589cc9b0-d623-4741-ab78-eeb00f723a52">
    <SerializedVertexData Version="1" X="1316" Y="647.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="Average average precision Metric Computation" ComponentMetadataDefinitionID="6d869f0e-46b8-11e0-9506-4c9adfd72085">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>traceMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>Standard_TFIDF_similarityMatrix</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="averageAveragePrecision" Type="System.Double" />
              <OutputAs>averageAveragePrecision</OutputAs>
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
  <node id="24969f09-b37c-41c3-b098-09554ffaa026">
    <SerializedVertexData Version="1" X="783" Y="744.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="CSV Similarity Matrix Exporter" ComponentMetadataDefinitionID="7c742474-49d2-11e0-8b6e-11f7dfd72085">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>ANCsimilarityMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>traceMatrix</MappedTo>
            </InputItem>
          </Input>
          <Output />
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Path</Name>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>c:\p4root\RELab\branches\aczauderna\Data\ANC-Data\eTour\eTour-ANC</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="1f62c56d-8f9d-4ea3-baff-ced827b9bb27">
    <SerializedVertexData Version="1" X="1540" Y="739.04">
      <Metadata type="TraceLab.Core.Models.ComponentMetadata, TraceLab.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" Label="CSV Similarity Matrix Exporter" ComponentMetadataDefinitionID="7c742474-49d2-11e0-8b6e-11f7dfd72085">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>Standard_TFIDF_similarityMatrix</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>traceMatrix</MappedTo>
            </InputItem>
          </Input>
          <Output />
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>Path</Name>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Value>
                <string>c:\p4root\RELab\branches\aczauderna\Data\ANC-Data\eTour\eTour-TFIDF</string>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <edge id="44d4a4c5-36b5-444a-95c6-ba5e0fcd44dc" source="Start" target="a217af1d-c6db-46c7-9d08-4fe5fc173175" />
  <edge id="dc8f7c2e-1a41-4b5e-b364-faf5bc3fe690" source="Start" target="179277ef-9c53-4e88-b4f2-0e0398e39ef2" />
  <edge id="37f3b263-db78-4e5d-ab02-542705d379f7" source="45a3ff82-8ab7-49cc-923e-25e236618b5f" target="7d31deaa-cef1-4334-9208-5c1f9703a274" />
  <edge id="1b559bfd-c274-40c4-ae5a-b857514fa484" source="c1f3cd70-9772-4f91-8483-a108706d22e4" target="acf64625-f66b-4346-b042-c3bc309b9e23" />
  <edge id="1bce0075-6135-4384-8991-997acb61a8e8" source="49e3908e-326b-405d-b464-2b5335b2f511" target="45a3ff82-8ab7-49cc-923e-25e236618b5f" />
  <edge id="f19f20fc-22c4-4ff6-9955-26d635ed8077" source="a76a2104-bc00-4e01-bfc4-dc51fa2316bc" target="c1f3cd70-9772-4f91-8483-a108706d22e4" />
  <edge id="affa7084-f395-4051-973a-0ac6818bf307" source="acf64625-f66b-4346-b042-c3bc309b9e23" target="ead4e4e1-6e97-48e3-bd78-9dc77c022059" />
  <edge id="de2b8375-6736-4bc0-baf9-880fd29748d2" source="f8428671-2c76-4dac-a198-8b7f6c9ac947" target="acf64625-f66b-4346-b042-c3bc309b9e23" />
  <edge id="7f8e5c8a-caed-4ba3-9067-2754241833e8" source="f8428671-2c76-4dac-a198-8b7f6c9ac947" target="45a3ff82-8ab7-49cc-923e-25e236618b5f" />
  <edge id="fc47ff5b-e042-48ce-9340-4a0874885924" source="c67125c5-afa1-4f66-adf7-66afb0ffa30c" target="24969f09-b37c-41c3-b098-09554ffaa026" />
  <edge id="f984a138-7a06-4ea8-948e-c2fd5e267111" source="9719fc48-5226-4fb2-a6b1-4c14e04735de" target="End" />
  <edge id="2e82d7ee-2f83-4806-b298-f5d8479fe808" source="1d5b5d08-65e5-4c75-a40a-c7ba6966e781" target="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" />
  <edge id="1bad4a70-7b75-4c4f-ae58-ffcae00ffced" source="1d5b5d08-65e5-4c75-a40a-c7ba6966e781" target="b8b90a3e-41ca-4063-9b78-32745a7d4329" />
  <edge id="1c87796b-839c-44c0-97e6-aa8a7b0a3e18" source="b8b90a3e-41ca-4063-9b78-32745a7d4329" target="a86a843d-41e6-4ffa-aa20-1307223103e9" />
  <edge id="631009cc-4690-41f5-b07d-c5f74a8d955e" source="b8b90a3e-41ca-4063-9b78-32745a7d4329" target="589cc9b0-d623-4741-ab78-eeb00f723a52" />
  <edge id="6967b4e4-7074-46a5-8cc0-2a4246bab9f1" source="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" target="c67125c5-afa1-4f66-adf7-66afb0ffa30c" />
  <edge id="6660724d-96b5-47de-9166-01d1e96a2c73" source="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" target="5f60a7e0-0126-4ac9-b498-175d9941f413" />
  <edge id="eb2f8348-08e4-4819-ad9e-446a5562858f" source="179277ef-9c53-4e88-b4f2-0e0398e39ef2" target="49e3908e-326b-405d-b464-2b5335b2f511" />
  <edge id="0ca27dab-e3c0-4930-971e-b5634bb738ed" source="179277ef-9c53-4e88-b4f2-0e0398e39ef2" target="f8428671-2c76-4dac-a198-8b7f6c9ac947" />
  <edge id="4f5e488a-ed20-4870-b00d-7b6469d81a4a" source="179277ef-9c53-4e88-b4f2-0e0398e39ef2" target="a76a2104-bc00-4e01-bfc4-dc51fa2316bc" />
  <edge id="acff96ee-27f2-498e-abe6-1eb05b8c6677" source="a217af1d-c6db-46c7-9d08-4fe5fc173175" target="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" />
  <edge id="d4b0d9e3-a6c0-4e8a-954d-d20bd27df4a3" source="42876d9e-a88c-405d-8997-55434b47642f" target="End" />
  <edge id="7d09f5b0-f66b-47f6-8fd6-fff73ca49b59" source="ead4e4e1-6e97-48e3-bd78-9dc77c022059" target="b028ed2c-a9a0-4feb-b1e4-32cca6251f4b" />
  <edge id="0987f9b3-67b4-45b0-90e7-c10f3ba2fdab" source="ead4e4e1-6e97-48e3-bd78-9dc77c022059" target="b8b90a3e-41ca-4063-9b78-32745a7d4329" />
  <edge id="ef558f15-9094-494e-8510-7dde852e2418" source="7d31deaa-cef1-4334-9208-5c1f9703a274" target="1d5b5d08-65e5-4c75-a40a-c7ba6966e781" />
  <edge id="c3bc0f9d-c905-42b1-983c-59886d26ad5f" source="a86a843d-41e6-4ffa-aa20-1307223103e9" target="1f62c56d-8f9d-4ea3-baff-ced827b9bb27" />
  <edge id="6d6b172f-b979-425a-b1a3-ba4ce793f70f" source="5f60a7e0-0126-4ac9-b498-175d9941f413" target="End" />
  <edge id="f5e32463-5f2f-40eb-9d99-cc779c3b2802" source="589cc9b0-d623-4741-ab78-eeb00f723a52" target="End" />
  <edge id="156d96a9-e9a6-4d3a-8ecf-6d945e718475" source="24969f09-b37c-41c3-b098-09554ffaa026" target="End" />
  <edge id="90210fc9-6709-44ad-a9b2-6dace6bfa1f6" source="1f62c56d-8f9d-4ea3-baff-ced827b9bb27" target="End" />
</graph>