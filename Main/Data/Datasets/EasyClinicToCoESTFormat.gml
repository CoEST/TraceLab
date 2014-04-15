<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>17ec26ba-111b-4090-8160-26eec050435b</Id>
    <Name>Complete Easy Clinic Transformation</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="215" Y="96">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="251" Y="483">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="346fe6f3-d0e0-4d70-a006-94df428ebef4">
    <SerializedVertexData Version="1" X="121" Y="175">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="CC Code Classes" ComponentMetadataDefinitionID="b678a5ef-65d8-4599-bbc7-4890c162eaa2">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="artifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>code_classes</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>e3515c08-d297-4a63-a40e-27faed315ab3:SourceFolder</Name>
              <DisplayName>Easy Clinic SourceFolder</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\EasyClinic_ENG\code classes\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionId</Name>
              <DisplayName>1. Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionName</Name>
              <DisplayName>2. Collection Name</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionVersion</Name>
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
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionDescription</Name>
              <DisplayName>4. Collection Description</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Code classes</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:Path</Name>
              <DisplayName>5. Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\code_classes.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="2cb0b0db-db31-4c2c-87c7-f8c42e44a2b0">
    <SerializedVertexData Version="1" X="366" Y="177">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="CC_CC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>CC_CC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\CC_CC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\CC_CC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="bfd3be9a-0fe1-4bd0-85bc-336f1ea62af6">
    <SerializedVertexData Version="1" X="527" Y="177">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="CC_TC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>CC_TC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\CC_TC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>TC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\CC_TC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="958bb880-040d-4491-9c2e-103db57cfb7e">
    <SerializedVertexData Version="1" X="368" Y="240">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="ID_CC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>ID_CC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\ID_CC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>ID</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\ID_CC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="14e7c031-456f-41f4-a781-694cc38c08d6">
    <SerializedVertexData Version="1" X="527" Y="239">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="ID_ID Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>ID_ID</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\ID_ID.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>ID</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>ID</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\ID_ID.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="a2923a07-e9cb-4253-8155-27e2ec7d9bed">
    <SerializedVertexData Version="1" X="693" Y="238">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="ID_TC  Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>ID_TC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\ID_TC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>ID</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>TC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\ID_TC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="c1a1e9c9-dd84-4963-ab31-656b6119ae46">
    <SerializedVertexData Version="1" X="852" Y="236">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="ID_UC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>ID_UC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\ID_UC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>ID</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>UC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\ID_UC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="bfa1f60d-8c6f-4e73-9a89-bf603ba983b5">
    <SerializedVertexData Version="1" X="369" Y="311">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="TC_CC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>TC_CC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\TC_CC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>TC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\TC_CC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="d74bfb26-26a2-4440-a3c3-b3709855aeb6">
    <SerializedVertexData Version="1" X="522" Y="314">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="TC_TC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>TC_TC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\TC_TC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>TC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>TC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\TC_TC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="903410d4-c642-45e3-92d5-406dfcce375a">
    <SerializedVertexData Version="1" X="377" Y="378">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="UC_CC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>UC_CC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\UC_CC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>UC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>CC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\UC_CC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="44947f9f-d5d8-4179-a306-2b9bc6dacc1e">
    <SerializedVertexData Version="1" X="536" Y="380">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="UC_ID Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>UC_ID</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\UC_ID.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>UC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>ID</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\UC_ID.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="f9adf394-fb7f-45fc-b1d5-04c49ecf3237">
    <SerializedVertexData Version="1" X="688" Y="382">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="UC_TC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>UC_TC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\UC_TC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>UC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>TC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\UC_TC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="7c5c8ba6-aeda-4855-88f3-dcb36fdbebbe">
    <SerializedVertexData Version="1" X="863" Y="380">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="UC_UC Answer Set" ComponentMetadataDefinitionID="89089b68-6c02-4cb9-91aa-a00a472827b3">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="traceMatrix" Type="TraceLabSDK.Types.TLSimilarityMatrix" />
              <ImportAs>UC_UC</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>b5470573-8b37-435f-bd56-94643fa0009b:TraceMatrixFile</Name>
              <DisplayName>EasyClinic Answer Set input file</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG\oracle\UC_UC.txt</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:SourceArtifactsCollectionId</Name>
              <DisplayName>1. Source Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>UC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:TargetArtifactsCollectionId</Name>
              <DisplayName>2. Target Artifacts Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>UC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>3719e77d-c229-40b0-abd2-33a223f6ab39:Path</Name>
              <DisplayName>3. Coest XML Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\trace matrices\UC_UC.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="d8bfa6a2-51a7-453d-ba45-de2cf93c3a62">
    <SerializedVertexData Version="1" X="123" Y="243">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="ID Interaction Diagrams" ComponentMetadataDefinitionID="b678a5ef-65d8-4599-bbc7-4890c162eaa2">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="artifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>interaction_diagrams</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>e3515c08-d297-4a63-a40e-27faed315ab3:SourceFolder</Name>
              <DisplayName>Easy Clinic SourceFolder</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\EasyClinic_ENG\interaction diagrams\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionId</Name>
              <DisplayName>1. Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>ID</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionName</Name>
              <DisplayName>2. Collection Name</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>ID</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionVersion</Name>
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
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionDescription</Name>
              <DisplayName>4. Collection Description</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Interaction Diagrams</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:Path</Name>
              <DisplayName>5. Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\interaction_diagrams.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="39baa3ed-208c-4d09-a785-fd3fe013c761">
    <SerializedVertexData Version="1" X="121" Y="308">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="TC Test Cases" ComponentMetadataDefinitionID="b678a5ef-65d8-4599-bbc7-4890c162eaa2">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="artifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>test_cases</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>e3515c08-d297-4a63-a40e-27faed315ab3:SourceFolder</Name>
              <DisplayName>Easy Clinic SourceFolder</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\EasyClinic_ENG\test cases\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionId</Name>
              <DisplayName>1. Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>TC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionName</Name>
              <DisplayName>2. Collection Name</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>TC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionVersion</Name>
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
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionDescription</Name>
              <DisplayName>4. Collection Description</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Easy Clinic Test Cases</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:Path</Name>
              <DisplayName>5. Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\test_cases.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <node id="c287a0c5-ab6f-400f-975f-b2dfb4877138">
    <SerializedVertexData Version="1" X="122" Y="374">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null" Label="UC Use Cases" ComponentMetadataDefinitionID="b678a5ef-65d8-4599-bbc7-4890c162eaa2">
        <IOSpec Version="1">
          <Input />
          <Output />
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="artifacts" Type="TraceLabSDK.Types.TLArtifactsCollection" />
              <ImportAs>use_cases</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>e3515c08-d297-4a63-a40e-27faed315ab3:SourceFolder</Name>
              <DisplayName>Easy Clinic SourceFolder</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>c:\p4root\RELab\branches\aczauderna\Data\Datasets\EasyClinic_ENG\use cases\</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionId</Name>
              <DisplayName>1. Collection Id</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>UC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionName</Name>
              <DisplayName>2. Collection Name</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>UC</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionVersion</Name>
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
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:CollectionDescription</Name>
              <DisplayName>4. Collection Description</DisplayName>
              <ValueType>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <string>Use cases</string>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>07ff1e90-d318-4897-97f7-86adf21528aa:Path</Name>
              <DisplayName>5. Output filepath</DisplayName>
              <ValueType>TraceLabSDK.Component.Config.FilePath, TraceLabSDK, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <FilePath>
                  <Version>1</Version>
                  <Relative>Data\Datasets\EasyClinic_ENG New Format\use_cases.xml</Relative>
                </FilePath>
              </Value>
            </PropertyObject>
          </ConfigValues>
        </ConfigWrapper>
      </Metadata>
    </SerializedVertexData>
  </node>
  <edge id="693a580a-3652-4952-9407-23c7e2fcbc83" source="Start" target="346fe6f3-d0e0-4d70-a006-94df428ebef4" />
  <edge id="31a0f04f-e179-4bd5-a1a7-371b8e552619" source="Start" target="2cb0b0db-db31-4c2c-87c7-f8c42e44a2b0" />
  <edge id="e6ac6d0c-4bec-4709-9871-9f4df5793430" source="Start" target="bfd3be9a-0fe1-4bd0-85bc-336f1ea62af6" />
  <edge id="a2c195cd-edce-48ca-baa5-f79b16e07d6e" source="Start" target="958bb880-040d-4491-9c2e-103db57cfb7e" />
  <edge id="a9107b43-0ef0-4829-9878-fb17302c5924" source="Start" target="14e7c031-456f-41f4-a781-694cc38c08d6" />
  <edge id="0c66ea26-5c4c-4188-9a4c-76acf629641d" source="Start" target="a2923a07-e9cb-4253-8155-27e2ec7d9bed" />
  <edge id="490f7dc0-bd38-4def-9114-a7d6e74c2835" source="Start" target="c1a1e9c9-dd84-4963-ab31-656b6119ae46" />
  <edge id="3f73f21a-b9e2-4a76-adfb-cf45f8dc8077" source="Start" target="bfa1f60d-8c6f-4e73-9a89-bf603ba983b5" />
  <edge id="d533faf5-c2a1-4630-a547-136ed83b3c7c" source="Start" target="d74bfb26-26a2-4440-a3c3-b3709855aeb6" />
  <edge id="03d90f2e-4684-4183-81c7-0d6b8d58210d" source="Start" target="903410d4-c642-45e3-92d5-406dfcce375a" />
  <edge id="548c926a-2b10-4349-8794-730932245e4b" source="Start" target="44947f9f-d5d8-4179-a306-2b9bc6dacc1e" />
  <edge id="707b7721-90ef-4850-b7aa-dde8bb4af348" source="Start" target="f9adf394-fb7f-45fc-b1d5-04c49ecf3237" />
  <edge id="f126a037-a41a-42b1-82eb-639a9d2a626f" source="Start" target="7c5c8ba6-aeda-4855-88f3-dcb36fdbebbe" />
  <edge id="c2016f0a-fce7-470f-bd0e-803ae625ab0f" source="Start" target="d8bfa6a2-51a7-453d-ba45-de2cf93c3a62" />
  <edge id="3e507442-47cc-4f5f-8c8f-7b5ceb2068e6" source="Start" target="39baa3ed-208c-4d09-a785-fd3fe013c761" />
  <edge id="4d592296-0e76-4e76-8278-f16170ba894d" source="Start" target="c287a0c5-ab6f-400f-975f-b2dfb4877138" />
  <edge id="73e28158-db39-4e0b-b335-166e4359ec50" source="346fe6f3-d0e0-4d70-a006-94df428ebef4" target="End" />
  <edge id="2550db62-3f8f-4482-8a58-2bdcd0542091" source="2cb0b0db-db31-4c2c-87c7-f8c42e44a2b0" target="End" />
  <edge id="55eeaa58-f7af-41d7-93cd-3ea458109ef6" source="bfd3be9a-0fe1-4bd0-85bc-336f1ea62af6" target="End" />
  <edge id="2895e2e2-00e3-4128-9c54-83bbc18256f0" source="958bb880-040d-4491-9c2e-103db57cfb7e" target="End" />
  <edge id="a7c3dc39-0047-45ef-a47c-05a938661aae" source="14e7c031-456f-41f4-a781-694cc38c08d6" target="End" />
  <edge id="73507d4b-f4aa-4c99-80c0-cdc31ab8c7df" source="a2923a07-e9cb-4253-8155-27e2ec7d9bed" target="End" />
  <edge id="c4828261-a142-4279-b90c-b1cd527ebf8a" source="c1a1e9c9-dd84-4963-ab31-656b6119ae46" target="End" />
  <edge id="70754b04-2753-4223-b15e-65dd6445bdc9" source="bfa1f60d-8c6f-4e73-9a89-bf603ba983b5" target="End" />
  <edge id="42e95545-bbf6-4962-afb2-935e4c441c4e" source="d74bfb26-26a2-4440-a3c3-b3709855aeb6" target="End" />
  <edge id="4217b834-1fcb-4f55-a93d-0f1af91f83ff" source="903410d4-c642-45e3-92d5-406dfcce375a" target="End" />
  <edge id="f8f7ee24-1db0-4639-a0bf-e98a35bfefc6" source="44947f9f-d5d8-4179-a306-2b9bc6dacc1e" target="End" />
  <edge id="4c9860fb-e142-492a-91ab-85b1ede04a61" source="f9adf394-fb7f-45fc-b1d5-04c49ecf3237" target="End" />
  <edge id="c9638464-58b7-444d-9b61-f0f544064462" source="7c5c8ba6-aeda-4855-88f3-dcb36fdbebbe" target="End" />
  <edge id="af1a0589-4e92-4267-a9d5-7b1aa0af3d8e" source="d8bfa6a2-51a7-453d-ba45-de2cf93c3a62" target="End" />
  <edge id="4df4456a-9df6-4199-8a07-8423e7e9f980" source="39baa3ed-208c-4d09-a785-fd3fe013c761" target="End" />
  <edge id="4f83b718-ea11-4f66-ad06-20b49e8a5e78" source="c287a0c5-ab6f-400f-975f-b2dfb4877138" target="End" />
</graph>