<?xml version='1.0' encoding='utf-8'?>
<xsl:stylesheet version='1.0' xmlns:xsl='http://www.w3.org/1999/XSL/Transform'
    xmlns:msxsl='urn:schemas-microsoft-com:xslt' exclude-result-prefixes='msxsl'
>
  <xsl:output method='xml' indent='yes' omit-xml-declaration='yes' />

  <xsl:template match='DocumentPane'>
    <xsl:text disable-output-escaping='yes'>&lt;</xsl:text>DocumentPane IsMain='<xsl:value-of select='@IsMain'/>' ResizeWidth='<xsl:value-of select='@ResizeWidth'/>' ResizeHeight='<xsl:value-of select='@ResizeHeight'/>' EffectiveSize='<xsl:value-of select='@EffectiveSize'/>' ShowHeader='False' /<xsl:text disable-output-escaping='yes'>&gt;</xsl:text>
  </xsl:template>

  <xsl:template match='@* | node()'>
    <xsl:copy>
      <xsl:apply-templates select='@* | node()'/>
    </xsl:copy>
  </xsl:template>
</xsl:stylesheet>
