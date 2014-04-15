<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>6c64c1e4-4da9-4afe-8320-9351d73977cc</Id>
    <Name>Promise to CoEST XML</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="200" Y="100">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="263" Y="323">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="328a9888-1763-4681-89db-252025d734d5">
    <SerializedVertexData Version="1" X="201" Y="155">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Promise Importer" ComponentMetadataDefinitionID="b6a84f2d-d21b-4c74-813e-e9f28f0db0fa">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>traceMatrix</ImportAs>
            </ImportItem>
            <ImportItem>
              <ImportItemDefinition Name="targetArtifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>targetArtifacts</ImportAs>
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
              <DisplayName>SourceFolder</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\UpdatedCM1Subset1\high\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>TargetFolder</Name>
              <DisplayName>TargetFolder</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\UpdatedCM1Subset1\low\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>TraceMatrixFile</Name>
              <DisplayName>TraceMatrixFile</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\UpdatedCM1Subset1\result.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="248bd0d0-e1fa-4ebc-af09-be714e12e51c">
    <SerializedVertexData Version="1" X="267" Y="248">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Source Artifacts Export" ComponentMetadataDefinitionID="c55e7015-f7d4-41b8-a19c-5385a1fa16c6">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="artifactsCollection" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>sourceArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output />
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>CollectionId</Name>
              <DisplayName>1. Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>cm1-high</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>CollectionName</Name>
              <DisplayName>2. Collection Name</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CM 1 Source Artifacts</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>CollectionVersion</Name>
              <DisplayName>3. Collection Version</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>1</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>CollectionDescription</Name>
              <DisplayName>4. Collection Description</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Collection of Source Artifacts for the CM1 dataset (high)</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>Path</Name>
              <DisplayName>5. Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\UpdatedCM1Subset1\CM1-sourceArtifacts.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="026e22d1-4559-4497-8c8f-c26196810331">
    <SerializedVertexData Version="1" X="92" Y="249">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Target Artifacts Export" ComponentMetadataDefinitionID="c55e7015-f7d4-41b8-a19c-5385a1fa16c6">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="artifactsCollection" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <MappedTo>targetArtifacts</MappedTo>
            </InputItem>
          </Input>
          <Output />
          <Import />
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>CollectionId</Name>
              <DisplayName>1. Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>cm1-low</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>CollectionName</Name>
              <DisplayName>2. Collection Name</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CM 1 Target Artifacts </string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>CollectionVersion</Name>
              <DisplayName>3. Collection Version</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>1</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>CollectionDescription</Name>
              <DisplayName>4. Collection Description</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Collection of Target Artifacts for the CM1 dataset (low)</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>Path</Name>
              <DisplayName>5. Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\UpdatedCM1Subset1\CM1-targetArtifacts.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="3f0f4872-f83b-4cdc-9070-075a01cdb221">
    <SerializedVertexData Version="1" X="433" Y="247">
      <Metadata type="TraceLab.Core.Components.ComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Answer Set Exporter" ComponentMetadataDefinitionID="56b9771d-1a42-4890-9451-f7cab3fdd22e">
        <IOSpec Version="1">
          <Input>
            <InputItem>
              <InputItemDefinition Name="answerSet" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
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
              <Name>SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>cm1-high</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>cm1-low</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>Path</Name>
              <DisplayName>3. Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\UpdatedCM1Subset1\CM1-answerSet.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <edge id="701403f6-1cf1-474e-b644-0e882914de2e" source="Start" target="328a9888-1763-4681-89db-252025d734d5" />
  <edge id="9bcd5439-741e-4f56-8f72-3035f6d8183e" source="328a9888-1763-4681-89db-252025d734d5" target="248bd0d0-e1fa-4ebc-af09-be714e12e51c" />
  <edge id="1d0f37bf-2905-4f08-93ac-a4e34d6d54a4" source="328a9888-1763-4681-89db-252025d734d5" target="026e22d1-4559-4497-8c8f-c26196810331" />
  <edge id="95d319c2-500e-452a-8d67-5e61c0f9d03e" source="328a9888-1763-4681-89db-252025d734d5" target="3f0f4872-f83b-4cdc-9070-075a01cdb221" />
  <edge id="f1601685-b072-45e0-882b-96c8b222d9f7" source="248bd0d0-e1fa-4ebc-af09-be714e12e51c" target="End" />
  <edge id="d7554346-f2b4-4cd0-b851-eadb77cf5338" source="026e22d1-4559-4497-8c8f-c26196810331" target="End" />
  <edge id="b6b606c0-141a-42e4-8e26-2ad2d922ec23" source="3f0f4872-f83b-4cdc-9070-075a01cdb221" target="End" />
</graph>