<?xml version="1.0" encoding="windows-1251"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:xs="http://www.w3.org/2001/XMLSchema"
	exclude-result-prefixes="#all"
	version="3.0">
<xsl:output method="xml"  encoding="windows-1251"/>

  <xsl:mode on-no-match="shallow-copy"/>
  
  <xsl:param name="translate" />
  <xsl:param name="hide" />
  
  <xsl:param name="all" select="document('allfld.xml')"/> 
  <xsl:key name="att-ref3" match="tt/row" use="fld" />
 
   
  <xsl:template match="//*[.!='uz' and .!='uz1']/@*[key('att-ref3', name(), $all)]" priority="3">
	  <xsl:param name="this_tag" select="local-name(..)"/> 
	  <xsl:choose>
	  <xsl:when test="$translate=1 ">  
		  <xsl:choose>
		  <xsl:when test="$hide=1 ">
			<xsl:if test="key('att-ref3', name(), $all)[./tag = $this_tag]/flag=1">
				<xsl:choose>
				<xsl:when test="key('att-ref3', name(), $all)[./tag = $this_tag]/comment != ''" >
					<xsl:attribute name="{key('att-ref3', name(), $all)[./tag = $this_tag]/comment}" select="."/>
				</xsl:when> 
				<xsl:otherwise>	
					<xsl:attribute name="{local-name()}" select="."/>
				</xsl:otherwise>
				</xsl:choose>	
			</xsl:if>	
		  </xsl:when> 
		  <xsl:otherwise>
			 <xsl:choose>
				<xsl:when test="key('att-ref3', name(), $all)[./tag = $this_tag]/comment != ''" >
					<xsl:attribute name="{key('att-ref3', name(), $all)[./tag = $this_tag]/comment}" select="."/>
				</xsl:when> 
				<xsl:otherwise>	
					<xsl:attribute name="{local-name()}" select="."/>
				</xsl:otherwise>
				</xsl:choose>
		  </xsl:otherwise>
		  </xsl:choose>	  
	  </xsl:when> 
 
	  <xsl:when test="$translate=0 and $hide=1 and key('att-ref3', name(), $all)[./tag = $this_tag]/flag=1">
		  <xsl:attribute name="{local-name()}" select="."/>
	  </xsl:when>
	  <xsl:when test="$translate=0 and $hide=0">
		  <xsl:attribute name="{local-name()}" select="."/>
	  </xsl:when>
	  </xsl:choose>
	  
  </xsl:template>
  
  
  <xsl:param name="uz" select="document('uzfld.xml')"/> 
  <xsl:key name="att-ref" match="tt/uz" use="replace(replace(fld, 'rv_uz.',''),' ','')" />

  <xsl:template match="//uz/@*[key('att-ref', name(),$uz)]" priority="4">
	  <xsl:param name="type" select="../@*[local-name() = 'typeuz_id']"/>
		
	  <xsl:choose>
	  <xsl:when test="$translate=1 ">  
		  <xsl:choose>
		  <xsl:when test="$hide=1 ">			
			  <xsl:if test="key('att-ref', name(),$uz)[typeuz_id= (if ($type) then $type else 20)]/flag=1">				 
				 <xsl:attribute name="{replace(replace(replace(key('att-ref', name(),$uz)[typeuz_id= (if ($type) then $type else 20 )]/comment,' ','_'),',','.'),'№','номер')}" select="."/>			  
			  </xsl:if>
		 </xsl:when> 
		 <xsl:otherwise>
			<xsl:attribute name="{replace(replace(replace(key('att-ref', name(),$uz)[typeuz_id= (if ($type) then $type else 20 )]/comment,' ','_'),',','.'),'№','номер')}" select="."/>
		 </xsl:otherwise>
		 </xsl:choose>	  
	  </xsl:when> 
	  <xsl:when test="$translate=0 and $hide=1 and key('att-ref', name(),$uz)[typeuz_id= (if ($type) then $type else 20)]/flag=1">
		  <xsl:attribute name="{local-name()}" select="."/>
	  </xsl:when>
	  <xsl:when test="$translate=0 and $hide=0">
		  <xsl:attribute name="{local-name()}" select="."/>
	  </xsl:when>
	  </xsl:choose>	
	 
  </xsl:template>
 
 
 <xsl:param name="uz1" select="document('uz1fld.xml')"/> 
  <xsl:key name="att-ref2" match="tt/uz1" use="replace(replace(fld, 'rv_uz1.',''),' ','')" />
  
  <xsl:template match="//uz1/@*[key('att-ref2', name(),$uz1)]" priority="5">
	  <xsl:param name="type2" select="../@*[local-name() = 'typeuz_id']"/>  
	  
	  <xsl:choose>
	  <xsl:when test="$translate=1 ">  
		  <xsl:choose>
		  <xsl:when test="$hide=1 ">	
		  
			  <xsl:if test="key('att-ref2', name(),$uz1)[typeuz_id=(if ($type2) then $type2 else 20)]/flag=1 ">
				  <xsl:attribute name="{replace(replace(replace(replace(key('att-ref2', name(),$uz1)[typeuz_id=(if ($type2) then $type2 else 20) ]/comment,' ','_'),',','.'),'№','номер'),'\$','доллары')}" select="."/>
			  </xsl:if>
		  </xsl:when> 
		 <xsl:otherwise>
			<xsl:attribute name="{replace(replace(replace(replace(key('att-ref2', name(),$uz1)[typeuz_id=(if ($type2) then $type2 else 20) ]/comment,' ','_'),',','.'),'№','номер'),'\$','доллары')}" select="."/>
		 </xsl:otherwise>
		 </xsl:choose>	  
	  </xsl:when> 
	  <xsl:when test="$translate=0 and $hide=1 and key('att-ref2', name(),$uz1)[typeuz_id=(if ($type2) then $type2 else 20)]/flag=1">
		  <xsl:attribute name="{local-name()}" select="."/>
	  </xsl:when>
	  <xsl:when test="$translate=0 and $hide=0">
		  <xsl:attribute name="{local-name()}" select="."/>
	  </xsl:when>
	  </xsl:choose>	
	  
  </xsl:template>
  
  
  
 </xsl:stylesheet>