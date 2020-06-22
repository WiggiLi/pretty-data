<?xml version="1.0" encoding="windows-1251"?>
<xsl:stylesheet version="3.0"
xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
xmlns:xs="http://www.w3.org/2001/XMLSchema"
exclude-result-prefixes="#all">
<xsl:output method="text" omit-xml-declaration="yes" indent="no" encoding="windows-1251"/>

<xsl:variable name="count" select="3" />
<xsl:param name="sort" />



<xsl:template match="//*">
	<xsl:copy> 
		
		<xsl:if test="local-name() != 'tt'">
			<xsl:variable name="tag" select="local-name()" />
			<xsl:value-of select="concat('&#xA;','&#xA;', 'Header ', local-name(), ' (id=', if (@*[lower-case(local-name()) = concat($tag,'_id')]) then (@*[lower-case(local-name()) = concat($tag,'_id')]) else @*[1], ')','&#xA;')"/>
		</xsl:if> 
		
		<xsl:choose>
		<xsl:when  test="$sort = 1">
			<xsl:apply-templates mode="toText" select="@*">
					   <xsl:sort select="name()"/>
			</xsl:apply-templates>
			<xsl:apply-templates/>
		</xsl:when>
		<xsl:otherwise>
			<xsl:apply-templates mode="toText" select="@*"/>
			<xsl:apply-templates/>
		</xsl:otherwise>
		</xsl:choose>
		
	</xsl:copy> 
</xsl:template>


<xsl:template match="@*" mode="toText">
	
		<xsl:value-of select="concat(local-name(),'=', ., string-join((1 to (36-string-length(concat(local-name(.),.))))!' '))" />
		<xsl:if test="position() mod $count = 0">
		     <xsl:value-of select="'&#xA;'"/>
		</xsl:if>
	
		
</xsl:template>
</xsl:stylesheet>
