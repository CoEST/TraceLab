<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        xmlns:xs="http://www.w3.org/2001/XMLSchema"
        xmlns:fn="http://www.w3.org/2005/xpath-functions"
   xmlns:w="http://schemas.microsoft.com/wix/2006/wi">

  <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" />

  <xsl:template match="/w:Wix">
  <w:Wix>
  <xsl:text disable-output-escaping="yes">
  <![CDATA[<?include Variables.wxi ?>]]>
  </xsl:text>
  <xsl:copy-of select="*" />
  </w:Wix>
  </xsl:template>

</xsl:stylesheet>