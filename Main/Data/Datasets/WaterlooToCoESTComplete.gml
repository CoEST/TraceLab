<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>75a29f93-1d61-4dbd-adf0-f838e4a1cff4</Id>
    <Name>Waterloo</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="73" Y="142">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="-317" Y="692">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="7e823879-030a-4bd4-aafe-67f5c8529ad7">
    <SerializedVertexData Version="1" X="33" Y="293">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Start Counter" ComponentMetadataDefinitionID="f09ee85f-b499-4c7b-b071-6b21aaa9ec0f">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="integer" Type="System.Int32" />
              <OutputAs>counter</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>OutputInteger</Name>
              <DisplayName>Output integer</DisplayName>
              <ValueType>System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <int>0</int>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="639bba11-b76c-4e6a-8317-f671f8f7813f">
    <SerializedVertexData Version="1" X="-171" Y="627">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Incrementer Counter by 1" ComponentMetadataDefinitionID="e83abfc7-a495-4458-99e0-1d756cf8d626">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="integer" Type="System.Int32" />
              <MappedTo>counter</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="integer" Type="System.Int32" />
              <OutputAs>counter</OutputAs>
            </OutputItem>
          </Output>
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>IncrementBy</Name>
              <DisplayName>Increment by value</DisplayName>
              <ValueType>System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <int>1</int>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="c0929663-0ccd-451b-a5dc-9a958bdb6d61">
    <SerializedVertexData Version="1" X="505" Y="363">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Get next answer file" ComponentMetadataDefinitionID="f5f6ee84-18a2-4ec3-96a9-4c6410e384e2">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="listOfStrings" Type="TraceLabSDK.Types.Generics.Collections.StringList" />
              <MappedTo>answerSetfiles</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="index" Type="System.Int32" />
              <MappedTo>counter</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="selectedString" Type="System.String" />
              <OutputAs>answerSetfile</OutputAs>
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
  <node id="089eb9a1-57a4-438f-8064-e857ea3c986b">
    <SerializedVertexData Version="1" X="501" Y="426">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Import Answer Set" ComponentMetadataDefinitionID="bfc8daee-ba81-46b1-a2ec-3997d7316285">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="filePath" Type="System.String" />
              <MappedTo>answerSetfile</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="answerSet" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <OutputAs>answerSet</OutputAs>
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
  <node id="8dad596d-d962-4660-80e9-5c201a3b199a">
    <SerializedVertexData Version="1" X="-167" Y="294">
      <Metadata type="TraceLab.Core.Components.DecisionMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Has More files">
        <DecisionCode>if(Load("counter")&lt; Load("numberOfFiles")) {
Select("Get next high artifacts file");
} else {
Select("End");
}
</DecisionCode>
        <DecisionMetadataDefinition>
          <ID>7f07cc5b-d8b6-4f5f-99ad-22da91e3a871</ID>
          <Classname>DecisionModule_7f07cc5b_d8b6_4f5f_99ad_22da91e3a871</Classname>
          <Assembly>C:\p4root\RELab\branches\aczauderna\Decisions\7f07cc5b-d8b6-4f5f-99ad-22da91e3a871.dll</Assembly>
        </DecisionMetadataDefinition>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="3c2cf93a-c420-4139-b53e-fa37c3338aa0">
    <SerializedVertexData Version="1" X="250" Y="631">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Answer Set Exporter to XML With Input config" ComponentMetadataDefinitionID="fc7a2699-1654-4368-a370-b7f5225f13aa">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="outputPath" Type="System.String" />
              <MappedTo>answerSetfile</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="sourceArtifactsCollectionId" Type="System.String" />
              <MappedTo>highArtifactsId</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="targetArtifactsCollectionId" Type="System.String" />
              <MappedTo>lowArtifactsId</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="answerSet" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <MappedTo>answerSet</MappedTo>
            </InputItem>
          </Input>
          <Output />
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues />
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="44a7f737-d922-437d-912c-38fa42bdbfe4">
    <SerializedVertexData Version="1" X="226" Y="219">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Answers Sets" ComponentMetadataDefinitionID="3dd86801-e9a2-4286-823d-45521d62a80e">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="numberOfFiles" Type="System.Int32" />
              <OutputAs>numberOfFiles</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="files" Type="TraceLabSDK.Types.Generics.Collections.StringList" />
              <OutputAs>answerSetfiles</OutputAs>
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
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\Waterloo New Format\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SearchPattern</Name>
              <DisplayName>SearchPattern</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>grp*_answers.txt</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SearchOption</Name>
              <DisplayName>SearchOption</DisplayName>
              <ValueType>HelperComponents.DirectoryReaderSearchOption, HelperComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>True</IsEnum>
              <EnumInfo>
                <EnumValueCollection>
                  <SourceEnum>HelperComponents.DirectoryReaderSearchOption, HelperComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</SourceEnum>
                  <Value>TopDirectoryOnly</Value>
                  <PossibleValues>
                    <ArrayOfEnumValue xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                      <EnumValue>
                        <Value>TopDirectoryOnly</Value>
                      </EnumValue>
                      <EnumValue>
                        <Value>AllDirectories</Value>
                      </EnumValue>
                    </ArrayOfEnumValue>
                  </PossibleValues>
                </EnumValueCollection>
              </EnumInfo>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="a16cd855-8f39-4ff5-afbc-3a216b23f8cc">
    <SerializedVertexData Version="1" X="34" Y="475">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Import High Artifacts" ComponentMetadataDefinitionID="bdd4c451-0012-4bcc-b12c-39618c6b5008">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="filePath" Type="System.String" />
              <MappedTo>highArtifactsFile</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="artifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>highArtifacts</OutputAs>
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
  <node id="5f604a39-2e02-4649-9914-b7e03b1e570f">
    <SerializedVertexData Version="1" X="256" Y="426">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Import Low Artifacts" ComponentMetadataDefinitionID="bdd4c451-0012-4bcc-b12c-39618c6b5008">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="filePath" Type="System.String" />
              <MappedTo>lowArtifactFile</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="artifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <OutputAs>lowArtifacts</OutputAs>
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
  <node id="4389af2d-4cae-44fb-a1fa-3950105cae7f">
    <SerializedVertexData Version="1" X="101" Y="221">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Low Artifacts" ComponentMetadataDefinitionID="3dd86801-e9a2-4286-823d-45521d62a80e">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="numberOfFiles" Type="System.Int32" />
              <OutputAs>numberOfFiles</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="files" Type="TraceLabSDK.Types.Generics.Collections.StringList" />
              <OutputAs>lowArtifactsFiles</OutputAs>
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
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\Waterloo New Format\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SearchPattern</Name>
              <DisplayName>SearchPattern</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>grp*_low_level.txt</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SearchOption</Name>
              <DisplayName>SearchOption</DisplayName>
              <ValueType>HelperComponents.DirectoryReaderSearchOption, HelperComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>True</IsEnum>
              <EnumInfo>
                <EnumValueCollection>
                  <SourceEnum>HelperComponents.DirectoryReaderSearchOption, HelperComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</SourceEnum>
                  <Value>TopDirectoryOnly</Value>
                  <PossibleValues>
                    <ArrayOfEnumValue xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                      <EnumValue>
                        <Value>TopDirectoryOnly</Value>
                      </EnumValue>
                      <EnumValue>
                        <Value>AllDirectories</Value>
                      </EnumValue>
                    </ArrayOfEnumValue>
                  </PossibleValues>
                </EnumValueCollection>
              </EnumInfo>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="08a74f65-6057-428e-ad73-85513ae10849">
    <SerializedVertexData Version="1" X="-25" Y="222">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="High Artifacts" ComponentMetadataDefinitionID="3dd86801-e9a2-4286-823d-45521d62a80e">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="numberOfFiles" Type="System.Int32" />
              <OutputAs>numberOfFiles</OutputAs>
            </OutputItem>
            <OutputItem>
              <OutputItemDefinition Name="files" Type="TraceLabSDK.Types.Generics.Collections.StringList" />
              <OutputAs>highArtifactsfiles</OutputAs>
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
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\Waterloo New Format\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SearchPattern</Name>
              <DisplayName>SearchPattern</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>grp*_high_level.txt</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>SearchOption</Name>
              <DisplayName>SearchOption</DisplayName>
              <ValueType>HelperComponents.DirectoryReaderSearchOption, HelperComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>True</IsEnum>
              <EnumInfo>
                <EnumValueCollection>
                  <SourceEnum>HelperComponents.DirectoryReaderSearchOption, HelperComponents, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null</SourceEnum>
                  <Value>TopDirectoryOnly</Value>
                  <PossibleValues>
                    <ArrayOfEnumValue xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                      <EnumValue>
                        <Value>TopDirectoryOnly</Value>
                      </EnumValue>
                      <EnumValue>
                        <Value>AllDirectories</Value>
                      </EnumValue>
                    </ArrayOfEnumValue>
                  </PossibleValues>
                </EnumValueCollection>
              </EnumInfo>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="93450480-f4ae-4ba0-b6e5-da91b4694627">
    <SerializedVertexData Version="1" X="35" Y="364">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Get next high artifacts file" ComponentMetadataDefinitionID="f5f6ee84-18a2-4ec3-96a9-4c6410e384e2">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="listOfStrings" Type="TraceLabSDK.Types.Generics.Collections.StringList" />
              <MappedTo>highArtifactsfiles</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="index" Type="System.Int32" />
              <MappedTo>counter</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="selectedString" Type="System.String" />
              <OutputAs>highArtifactsFile</OutputAs>
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
  <node id="e21a2038-dfbf-4bd6-b1ac-10bf2eb27249">
    <SerializedVertexData Version="1" X="255" Y="363">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Get next low artifacts file" ComponentMetadataDefinitionID="f5f6ee84-18a2-4ec3-96a9-4c6410e384e2">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="listOfStrings" Type="TraceLabSDK.Types.Generics.Collections.StringList" />
              <MappedTo>lowArtifactsFiles</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="index" Type="System.Int32" />
              <MappedTo>counter</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="selectedString" Type="System.String" />
              <OutputAs>lowArtifactFile</OutputAs>
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
  <node id="46677c8b-22ab-40b4-b933-1944f5335f68">
    <SerializedVertexData Version="1" X="36" Y="532">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Export High Artifacts" ComponentMetadataDefinitionID="ffdac867-b8a8-4de5-b251-ca9258be371c">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="artifactsCollection" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>highArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="outputPath" Type="System.String" />
              <MappedTo>highArtifactsFile</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="collectionId" Type="System.String" />
              <OutputAs>highArtifactsId</OutputAs>
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
  <node id="260de187-f815-4ab0-a24f-402a1f3954f3">
    <SerializedVertexData Version="1" X="257" Y="489">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Export Low Artifacts" ComponentMetadataDefinitionID="ffdac867-b8a8-4de5-b251-ca9258be371c">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="artifactsCollection" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>lowArtifacts</MappedTo>
            </InputItem>
            <InputItem>
              <InputItemDefinition Name="outputPath" Type="System.String" />
              <MappedTo>lowArtifactFile</MappedTo>
            </InputItem>
          </Input>
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="collectionId" Type="System.String" />
              <OutputAs>lowArtifactsId</OutputAs>
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
  <edge id="e7e43427-2047-4c74-a0cd-d3e8f587a0f7" source="Start" target="44a7f737-d922-437d-912c-38fa42bdbfe4" />
  <edge id="bd4cea9e-8ffe-46a4-9bf0-12b4fabfe953" source="Start" target="4389af2d-4cae-44fb-a1fa-3950105cae7f" />
  <edge id="f4d0cd5e-cef1-44b1-935e-cfca7ffc2bc1" source="Start" target="08a74f65-6057-428e-ad73-85513ae10849" />
  <edge id="20ad2733-3c0a-4254-9f4d-4766d18c61d6" source="7e823879-030a-4bd4-aafe-67f5c8529ad7" target="93450480-f4ae-4ba0-b6e5-da91b4694627" />
  <edge id="ad7dd7f1-6f6f-4c03-b019-9b2ab8eb035f" source="639bba11-b76c-4e6a-8317-f671f8f7813f" target="8dad596d-d962-4660-80e9-5c201a3b199a" />
  <edge id="faa65442-ea9c-4386-bd78-992d5b8c4a05" source="c0929663-0ccd-451b-a5dc-9a958bdb6d61" target="089eb9a1-57a4-438f-8064-e857ea3c986b" />
  <edge id="8cc5e2f0-2647-4846-8fa9-dcdec5828d35" source="089eb9a1-57a4-438f-8064-e857ea3c986b" target="3c2cf93a-c420-4139-b53e-fa37c3338aa0" />
  <edge id="8c575d5f-ddbf-46d1-852d-1139d0f5a97e" source="8dad596d-d962-4660-80e9-5c201a3b199a" target="End" />
  <edge id="2e3bbb1b-9c01-4ba5-88e9-70e092996607" source="8dad596d-d962-4660-80e9-5c201a3b199a" target="93450480-f4ae-4ba0-b6e5-da91b4694627" />
  <edge id="c7de8b52-5dd3-4c63-a0ae-5b0b2101c0e1" source="3c2cf93a-c420-4139-b53e-fa37c3338aa0" target="639bba11-b76c-4e6a-8317-f671f8f7813f" />
  <edge id="f28ea368-1af5-4846-a690-61c480eafa75" source="44a7f737-d922-437d-912c-38fa42bdbfe4" target="7e823879-030a-4bd4-aafe-67f5c8529ad7" />
  <edge id="0b0e391f-f52d-46b7-a792-28372ba8dca6" source="a16cd855-8f39-4ff5-afbc-3a216b23f8cc" target="46677c8b-22ab-40b4-b933-1944f5335f68" />
  <edge id="408844a7-a17d-44b3-bd63-c695cb7bd905" source="5f604a39-2e02-4649-9914-b7e03b1e570f" target="260de187-f815-4ab0-a24f-402a1f3954f3" />
  <edge id="5925f315-f22d-4350-b488-ed2e748a61dc" source="4389af2d-4cae-44fb-a1fa-3950105cae7f" target="7e823879-030a-4bd4-aafe-67f5c8529ad7" />
  <edge id="6db8c74d-d41e-4e51-92b4-12da3b2bd3a9" source="08a74f65-6057-428e-ad73-85513ae10849" target="7e823879-030a-4bd4-aafe-67f5c8529ad7" />
  <edge id="798227c1-d8a4-4f2f-8a33-ba0c0a37a81a" source="93450480-f4ae-4ba0-b6e5-da91b4694627" target="a16cd855-8f39-4ff5-afbc-3a216b23f8cc" />
  <edge id="4e01a8fa-4f42-47b7-b008-7d6122752a66" source="93450480-f4ae-4ba0-b6e5-da91b4694627" target="e21a2038-dfbf-4bd6-b1ac-10bf2eb27249" />
  <edge id="59ea6617-b41c-4dda-ba47-ac71cfd18638" source="e21a2038-dfbf-4bd6-b1ac-10bf2eb27249" target="5f604a39-2e02-4649-9914-b7e03b1e570f" />
  <edge id="654beae0-341e-41c6-a084-ad3e3d17fdb1" source="e21a2038-dfbf-4bd6-b1ac-10bf2eb27249" target="c0929663-0ccd-451b-a5dc-9a958bdb6d61" />
  <edge id="c4d6a28f-6632-4783-8741-21188475a81f" source="46677c8b-22ab-40b4-b933-1944f5335f68" target="3c2cf93a-c420-4139-b53e-fa37c3338aa0" />
  <edge id="6994b4dc-a31c-4ec3-8c58-a1b1fd239de9" source="260de187-f815-4ab0-a24f-402a1f3954f3" target="3c2cf93a-c420-4139-b53e-fa37c3338aa0" />
</graph>