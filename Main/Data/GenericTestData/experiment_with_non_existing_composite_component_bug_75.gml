<?xml version="1.0" encoding="utf-8"?>
<graph>
  <ExperimentInfo>
    <Version>1</Version>
    <Id>e87d84b4-f3cd-44bf-a6d2-827ada49c48f</Id>
    <Name>New Experiment</Name>
    <LayoutName>EfficientSugiyama</LayoutName>
  </ExperimentInfo>
  <node id="Start">
    <SerializedVertexData Version="1" X="200" Y="100">
      <Metadata type="TraceLab.Core.Components.StartNodeMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="Start" />
    </SerializedVertexData>
  </node>
  <node id="End">
    <SerializedVertexData Version="1" X="202" Y="204">
      <Metadata type="TraceLab.Core.Components.EndNodeMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="End" />
    </SerializedVertexData>
  </node>
  <node id="5d7cabc4-af74-4b80-bd16-20b10e5af06f">
    <SerializedVertexData Version="1" X="202" Y="149">
      <Metadata type="TraceLab.Core.Components.CompositeComponentMetadata, TraceLab.Core, Culture=neutral, PublicKeyToken=2c83cea59a8bb151" Label="Composite Component" ComponentMetadataDefinitionID="4a1d112a-f96e-4624-b2a4-8f7837fcbed7">
        <IOSpec Version="1">
          <Input />
          <Output>
            <OutputItem>
              <OutputItemDefinition Name="test4" Type="System.Int32" />
              <OutputAs>test4</OutputAs>
            </OutputItem>
          </Output>
          <Import>
            <ImportItem>
              <ImportItemDefinition Name="test1" Type="System.Int32" />
              <ImportAs>test1</ImportAs>
            </ImportItem>
          </Import>
        </IOSpec>
        <ConfigWrapper Version="1" IsJava="False">
          <ConfigValues>
            <PropertyObject>
              <Version>2</Version>
              <Name>34184e55-c66a-491e-ba41-8aaa2c11ef50:Value</Name>
              <DisplayName>Test importer Value</DisplayName>
              <ValueType>System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</ValueType>
              <Visible>True</Visible>
              <IsEnum>False</IsEnum>
              <Value IsNull="False">
                <int>0</int>
              </Value>
            </PropertyObject>
            <PropertyObject>
              <Version>2</Version>
              <Name>9dc3255b-0daf-447a-921d-ce0039672278:Value</Name>
              <DisplayName>Test importer Value 2</DisplayName>
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
  <edge id="8f024943-d4a6-490e-b05a-2e5acfef83e6" source="Start" target="5d7cabc4-af74-4b80-bd16-20b10e5af06f" />
  <edge id="4a979e1c-a327-4817-83b3-b6586715a2c6" source="5d7cabc4-af74-4b80-bd16-20b10e5af06f" target="End" />
</graph>