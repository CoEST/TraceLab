<?xml version="1.0" encoding="utf-8"?>
<graph>
  <WorkflowInfo xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <Name>New Experiment</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
  </WorkflowInfo>
  <node id="Start">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="820.358" Y="22.88">
      <Metadata xsi:type="StartNodeMetadata" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="824.778" Y="889.88">
      <Metadata xsi:type="EndNodeMetadata" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="c150194e-52d9-46f4-b817-6a4e121c5aef">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1023.058" Y="182.04">
      <Metadata xsi:type="ComponentMetadata" Label="Promise Importer" ComponentMetadataDefinitionID="b6a84f2d-d21b-4c74-813e-e9f28f0db0fa">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>traceMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="traceMatrix">
                  <InputItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLAnswerMatrix" MustExist="false" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>targetArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="targetArtifacts">
                  <InputItemDefinition Name="targetArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="false" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>sourceArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="false" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>traceMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>traceMatrix</OutputAs>
                  <OutputItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLAnswerMatrix" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>targetArtifacts</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>targetArtifacts</OutputAs>
                  <OutputItemDefinition Name="targetArtifacts" Type="TraceLabSDK.Types.TLArtifactList" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>sourceArtifacts</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>sourceArtifacts</OutputAs>
                  <OutputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactList" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>true</IsJava>
          <ConfigValues>
            <item>
              <key>
                <string>M_SourceFolder</string>
              </key>
              <value>
                <PropertyObject name="M_SourceFolder" type="System.String">
                  <Value xsi:type="xsd:string">C:\Users\mgibiec\Marek\TEFSE2011\Challenge\CM1-Subset1\high</Value>
                </PropertyObject>
              </value>
            </item>
            <item>
              <key>
                <string>M_TargetFolder</string>
              </key>
              <value>
                <PropertyObject name="M_TargetFolder" type="System.String">
                  <Value xsi:type="xsd:string">C:\Users\mgibiec\Marek\TEFSE2011\Challenge\CM1-Subset1\low</Value>
                </PropertyObject>
              </value>
            </item>
            <item>
              <key>
                <string>M_TraceMatrixFile</string>
              </key>
              <value>
                <PropertyObject name="M_TraceMatrixFile" type="System.String">
                  <Value xsi:type="xsd:string">C:\Users\mgibiec\Marek\TEFSE2011\Challenge\CM1-Subset1\traceability_matrix.txt</Value>
                </PropertyObject>
              </value>
            </item>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="460b7ef1-4752-49b1-ba01-ce00dfc35784">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="897.95" Y="572.04">
      <Metadata xsi:type="ComponentMetadata" Label="Rocchio" ComponentMetadataDefinitionID="924df0df-add0-4fd2-b357-9a862f308e48">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>dictionary</string>
              </key>
              <value>
                <InputItem MappedTo="dictionaryIndex">
                  <InputItemDefinition Name="dictionary" Type="TraceLabSDK.Types.TLDictionaryIndex" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>listOfDocuments</string>
              </key>
              <value>
                <InputItem MappedTo="targetArtifacts">
                  <InputItemDefinition Name="listOfDocuments" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>listOfQueries</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="listOfQueries" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>answerMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="traceMatrix">
                  <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLAnswerMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>iteration3SimMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>iteration3SimMatrix</OutputAs>
                  <OutputItemDefinition Name="iteration3SimMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>iteration2SimMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>iteration2SimMatrix</OutputAs>
                  <OutputItemDefinition Name="iteration2SimMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>iteration1SimMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>iteration1SimMatrix</OutputAs>
                  <OutputItemDefinition Name="iteration1SimMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>basicSimilarityMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>basicSimilarityMatrix</OutputAs>
                  <OutputItemDefinition Name="basicSimilarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="12d85581-00ce-4bf4-aa50-17d00756abbb">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1052.272" Y="113.04">
      <Metadata xsi:type="ComponentMetadata" Label="StopwordsImporter" ComponentMetadataDefinitionID="b450dc72-1db6-11e0-bb91-fbe4dfd72085">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>stopwords</string>
              </key>
              <value>
                <InputItem MappedTo="stopwords">
                  <InputItemDefinition Name="stopwords" Type="TraceLabSDK.Types.TLStopwords" MustExist="false" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>stopwords</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>stopwords</OutputAs>
                  <OutputItemDefinition Name="stopwords" Type="TraceLabSDK.Types.TLStopwords" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues>
            <item>
              <key>
                <string>Path</string>
              </key>
              <value>
                <PropertyObject name="Path" type="System.String">
                  <Value xsi:type="xsd:string">C:\Users\mgibiec\Marek\RocchioMarek\Rocchio\stopwords.txt</Value>
                </PropertyObject>
              </value>
            </item>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="dd36afdb-a6c4-4ce9-876c-2ed98a2836bf">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="973.508" Y="276.04">
      <Metadata xsi:type="ComponentMetadata" Label="Cleanup Preprocessor" ComponentMetadataDefinitionID="85cb9977-f2a6-426f-b3df-bb0cca26b276">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>sourceArtifacts</OutputAs>
                  <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="0dfa0c06-8cc5-4750-8516-c462238da30e">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="950.272" Y="351.04">
      <Metadata xsi:type="ComponentMetadata" Label="Stopwords Remover" ComponentMetadataDefinitionID="449f5e1f-b66e-4db1-ac70-ba0a0a54a3ba">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>stopwords</string>
              </key>
              <value>
                <InputItem MappedTo="stopwords">
                  <InputItemDefinition Name="stopwords" Type="TraceLabSDK.Types.TLStopwords" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>sourceArtifacts</OutputAs>
                  <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="3c0ddd9c-0172-49bf-8229-c40b64738b1f">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="952.968" Y="422.04">
      <Metadata xsi:type="ComponentMetadata" Label="Java Porter Stemmer" ComponentMetadataDefinitionID="ee6d7c84-2019-11e0-a814-7e80dfd72085">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>sourceArtifacts</OutputAs>
                  <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="1756e725-11d0-45b1-8058-324f85634157">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1314.83" Y="487.04">
      <Metadata xsi:type="ComponentMetadata" Label="TFIDF Dictionary Index Builder" ComponentMetadataDefinitionID="1c30b7b5-3e04-433d-817f-b0be187b154f">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="targetArtifacts">
                  <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>dictionaryIndex</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>dictionaryIndex</OutputAs>
                  <OutputItemDefinition Name="dictionaryIndex" Type="TraceLabSDK.Types.TLDictionaryIndex" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="d5323a44-af0e-43dc-8320-b881a22c47b1">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1202.508" Y="277.04">
      <Metadata xsi:type="ComponentMetadata" Label="Cleanup Preprocessor" ComponentMetadataDefinitionID="85cb9977-f2a6-426f-b3df-bb0cca26b276">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="targetArtifacts">
                  <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>targetArtifacts</OutputAs>
                  <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="332f639d-70ef-4e24-8686-7d659255c09a">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1182.272" Y="352.04">
      <Metadata xsi:type="ComponentMetadata" Label="Stopwords Remover" ComponentMetadataDefinitionID="449f5e1f-b66e-4db1-ac70-ba0a0a54a3ba">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>stopwords</string>
              </key>
              <value>
                <InputItem MappedTo="stopwords">
                  <InputItemDefinition Name="stopwords" Type="TraceLabSDK.Types.TLStopwords" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="targetArtifacts">
                  <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>targetArtifacts</OutputAs>
                  <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="d6b8bcf0-d6a3-46e4-831b-7f9f0fe1f71b">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1229.108" Y="419.04">
      <Metadata xsi:type="ComponentMetadata" Label="English Porter Stemmer" ComponentMetadataDefinitionID="420775e4-1afc-4142-9145-f32a7d1ed8c4">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="targetArtifacts">
                  <InputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>listOfArtifacts</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>targetArtifacts</OutputAs>
                  <OutputItemDefinition Name="listOfArtifacts" Type="TraceLabSDK.Types.TLArtifactList" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="e19ca071-66d5-4237-8ed7-68498d27c25c">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="846.658" Y="676.04">
      <Metadata xsi:type="ComponentMetadata" Label="Results Metric Computation" ComponentMetadataDefinitionID="7cba9f2d-730b-43c3-8ba8-ed9fe53650fb">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>answerMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="traceMatrix">
                  <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLAnswerMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>similarityMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="basicSimilarityMatrix">
                  <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>sourceArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>recallData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>recallData_base</OutputAs>
                  <OutputItemDefinition Name="recallData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>precisionData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>precisionData_base</OutputAs>
                  <OutputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>averagePrecisionData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>averagePrecisionData_base</OutputAs>
                  <OutputItemDefinition Name="averagePrecisionData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>similarityMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>similarityMatrix_base</OutputAs>
                  <OutputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="ed769c64-7786-403c-9000-41f49e946f10">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1056.658" Y="676.04">
      <Metadata xsi:type="ComponentMetadata" Label="Results Metric Computation" ComponentMetadataDefinitionID="7cba9f2d-730b-43c3-8ba8-ed9fe53650fb">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>answerMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="traceMatrix">
                  <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLAnswerMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>similarityMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="iteration1SimMatrix">
                  <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>sourceArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>recallData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>recallData_it1</OutputAs>
                  <OutputItemDefinition Name="recallData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>precisionData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>precisionData_it1</OutputAs>
                  <OutputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>averagePrecisionData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>averagePrecisionData_it1</OutputAs>
                  <OutputItemDefinition Name="averagePrecisionData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>similarityMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>similarityMatrix_it1</OutputAs>
                  <OutputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="eabbd263-059c-46fc-9d12-7b9bdbdc53b7">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1257.658" Y="676.04">
      <Metadata xsi:type="ComponentMetadata" Label="Results Metric Computation" ComponentMetadataDefinitionID="7cba9f2d-730b-43c3-8ba8-ed9fe53650fb">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>answerMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="traceMatrix">
                  <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLAnswerMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>similarityMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="iteration2SimMatrix">
                  <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>sourceArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>recallData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>recallData_it2</OutputAs>
                  <OutputItemDefinition Name="recallData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>precisionData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>precisionData_it2</OutputAs>
                  <OutputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>averagePrecisionData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>averagePrecisionData_it2</OutputAs>
                  <OutputItemDefinition Name="averagePrecisionData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>similarityMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>similarityMatrix_it2</OutputAs>
                  <OutputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="7396a743-0911-4d15-8ed7-a92449b95080">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1461.658" Y="674.04">
      <Metadata xsi:type="ComponentMetadata" Label="Results Metric Computation" ComponentMetadataDefinitionID="7cba9f2d-730b-43c3-8ba8-ed9fe53650fb">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>answerMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="traceMatrix">
                  <InputItemDefinition Name="answerMatrix" Type="TraceLabSDK.Types.TLAnswerMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>similarityMatrix</string>
              </key>
              <value>
                <InputItem MappedTo="iteration3SimMatrix">
                  <InputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>sourceArtifacts</string>
              </key>
              <value>
                <InputItem MappedTo="sourceArtifacts">
                  <InputItemDefinition Name="sourceArtifacts" Type="TraceLabSDK.Types.TLArtifactList" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output>
            <item>
              <key>
                <string>recallData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>recallData_it3</OutputAs>
                  <OutputItemDefinition Name="recallData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>precisionData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>precisionData_it3</OutputAs>
                  <OutputItemDefinition Name="precisionData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>averagePrecisionData</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>averagePrecisionData_it3</OutputAs>
                  <OutputItemDefinition Name="averagePrecisionData" Type="TraceLabSDK.Types.TLMetricContainer" />
                </OutputItem>
              </value>
            </item>
            <item>
              <key>
                <string>similarityMatrix</string>
              </key>
              <value>
                <OutputItem>
                  <OutputAs>similarityMatrix_it3</OutputAs>
                  <OutputItemDefinition Name="similarityMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
                </OutputItem>
              </value>
            </item>
          </Output>
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="dd51f689-2f37-48de-a414-a0e086239dda">
    <SerializedVertexData xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" X="1183.784" Y="818.04">
      <Metadata xsi:type="ComponentMetadata" Label="Rocchio Results Analyzer" ComponentMetadataDefinitionID="01e8c2d0-5f76-42e1-bf00-d93b032c4edf">
        <IOSpec>
          <Input>
            <item>
              <key>
                <string>AvgPrec2</string>
              </key>
              <value>
                <InputItem MappedTo="averagePrecisionData_it2">
                  <InputItemDefinition Name="AvgPrec2" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>Prec2</string>
              </key>
              <value>
                <InputItem MappedTo="precisionData_it2">
                  <InputItemDefinition Name="Prec2" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>Recall2</string>
              </key>
              <value>
                <InputItem MappedTo="recallData_it2">
                  <InputItemDefinition Name="Recall2" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>Recall3</string>
              </key>
              <value>
                <InputItem MappedTo="recallData_it3">
                  <InputItemDefinition Name="Recall3" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>AvgPrec1</string>
              </key>
              <value>
                <InputItem MappedTo="averagePrecisionData_it1">
                  <InputItemDefinition Name="AvgPrec1" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>AvgPrec3</string>
              </key>
              <value>
                <InputItem MappedTo="averagePrecisionData_it3">
                  <InputItemDefinition Name="AvgPrec3" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>Prec3</string>
              </key>
              <value>
                <InputItem MappedTo="precisionData_it3">
                  <InputItemDefinition Name="Prec3" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>AvgPrec0</string>
              </key>
              <value>
                <InputItem MappedTo="averagePrecisionData_base">
                  <InputItemDefinition Name="AvgPrec0" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>Prec0</string>
              </key>
              <value>
                <InputItem MappedTo="precisionData_base">
                  <InputItemDefinition Name="Prec0" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>Recall0</string>
              </key>
              <value>
                <InputItem MappedTo="recallData_base">
                  <InputItemDefinition Name="Recall0" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>Prec1</string>
              </key>
              <value>
                <InputItem MappedTo="precisionData_it1">
                  <InputItemDefinition Name="Prec1" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
            <item>
              <key>
                <string>Recall1</string>
              </key>
              <value>
                <InputItem MappedTo="recallData_it1">
                  <InputItemDefinition Name="Recall1" Type="TraceLabSDK.Types.TLMetricContainer" MustExist="true" />
                </InputItem>
              </value>
            </item>
          </Input>
          <Output />
        </IOSpec>
        <ConfigWrapper>
          <IsJava>false</IsJava>
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <edge id="02f4f51c-e7df-4dd8-b218-d9b95c523b89" source="Start" target="12d85581-00ce-4bf4-aa50-17d00756abbb" />
  <edge id="074081e2-c968-43c5-9314-b92a4e86cf20" source="c150194e-52d9-46f4-b817-6a4e121c5aef" target="dd36afdb-a6c4-4ce9-876c-2ed98a2836bf" />
  <edge id="6485a63d-59f3-44b4-9682-a36b3aa857e6" source="460b7ef1-4752-49b1-ba01-ce00dfc35784" target="e19ca071-66d5-4237-8ed7-68498d27c25c" />
  <edge id="71de331a-068a-46e5-bd89-0eb7a47d764f" source="460b7ef1-4752-49b1-ba01-ce00dfc35784" target="ed769c64-7786-403c-9000-41f49e946f10" />
  <edge id="e67c8164-e3bc-40bd-a626-6e920d74fa87" source="460b7ef1-4752-49b1-ba01-ce00dfc35784" target="eabbd263-059c-46fc-9d12-7b9bdbdc53b7" />
  <edge id="84f02fe1-0b21-4bbc-bb6c-08a1bcbac623" source="460b7ef1-4752-49b1-ba01-ce00dfc35784" target="7396a743-0911-4d15-8ed7-a92449b95080" />
  <edge id="e830f0d5-7259-4624-bd5b-e75162b4441b" source="12d85581-00ce-4bf4-aa50-17d00756abbb" target="c150194e-52d9-46f4-b817-6a4e121c5aef" />
  <edge id="82d1cb44-b232-41e0-a02f-7d9992b73784" source="dd36afdb-a6c4-4ce9-876c-2ed98a2836bf" target="0dfa0c06-8cc5-4750-8516-c462238da30e" />
  <edge id="6feeac50-c599-4979-88c0-480556556437" source="0dfa0c06-8cc5-4750-8516-c462238da30e" target="3c0ddd9c-0172-49bf-8229-c40b64738b1f" />
  <edge id="b592b275-443c-4054-85a9-30db6606c23d" source="3c0ddd9c-0172-49bf-8229-c40b64738b1f" target="d5323a44-af0e-43dc-8320-b881a22c47b1" />
  <edge id="e6f3af3c-6b0d-4764-8ed8-288922acfc29" source="1756e725-11d0-45b1-8058-324f85634157" target="460b7ef1-4752-49b1-ba01-ce00dfc35784" />
  <edge id="3a470dcd-d7bf-47f7-a162-c0c3d6c89235" source="d5323a44-af0e-43dc-8320-b881a22c47b1" target="332f639d-70ef-4e24-8686-7d659255c09a" />
  <edge id="19ee6900-b6aa-4f3c-a897-3471e1e541e0" source="332f639d-70ef-4e24-8686-7d659255c09a" target="d6b8bcf0-d6a3-46e4-831b-7f9f0fe1f71b" />
  <edge id="a48c9f45-cadc-4548-9acb-84e0c87f3a9c" source="d6b8bcf0-d6a3-46e4-831b-7f9f0fe1f71b" target="1756e725-11d0-45b1-8058-324f85634157" />
  <edge id="3df6f07d-1c2f-48da-b9c2-6a934193e4ea" source="e19ca071-66d5-4237-8ed7-68498d27c25c" target="dd51f689-2f37-48de-a414-a0e086239dda" />
  <edge id="c24f6689-f742-43b7-9981-ffa2b7ba6324" source="ed769c64-7786-403c-9000-41f49e946f10" target="dd51f689-2f37-48de-a414-a0e086239dda" />
  <edge id="15ede6dc-2d50-4ade-be71-488ce905af4f" source="eabbd263-059c-46fc-9d12-7b9bdbdc53b7" target="dd51f689-2f37-48de-a414-a0e086239dda" />
  <edge id="0da41492-7c54-49dd-bf7e-91bf03d70d0b" source="7396a743-0911-4d15-8ed7-a92449b95080" target="dd51f689-2f37-48de-a414-a0e086239dda" />
  <edge id="ed12c340-7a5a-4dbb-9ca0-41e6f5eafc03" source="dd51f689-2f37-48de-a414-a0e086239dda" target="End" />
</graph>