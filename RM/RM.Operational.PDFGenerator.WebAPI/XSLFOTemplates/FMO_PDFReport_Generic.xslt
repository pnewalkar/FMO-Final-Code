<?xml version="1.0" encoding="iso-8859-1"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">
  <xsl:output method="xml" indent="yes"/>
  <!-- FMO_PDFReport_Generic -->
  <xsl:template match="/report">
    <fo:root>
      <fo:layout-master-set>
        <fo:simple-page-master master-name="A4Portrait" page-width="210mm" page-height="297mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A4Landscape" page-width="297mm" page-height="210mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A3Portrait" page-width="297mm" page-height="420mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A3Landscape" page-width="420mm" page-height="297mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A2Portrait" page-width="420mm" page-height="594mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A2Landscape" page-width="594mm" page-height="420mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A1Portrait" page-width="594mm" page-height="841mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A1Landscape" page-width="841mm" page-height="594mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A0Portrait" page-width="841mm" page-height="1189mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A0Landscape" page-width="1189mm" page-height="841mm" margin-top="1cm" margin-bottom="1cm" margin-left="1cm"  margin-right="1cm">
          <fo:region-body margin-top="1cm" margin-bottom="1cm" margin-left="0cm" margin-right="0cm" column-count="1"/>
          <fo:region-before extent="0.5cm"/>
          <fo:region-after extent="0.5cm"/>
          <fo:region-start extent="0.5cm"/>
          <fo:region-end extent="0.5cm"/>
        </fo:simple-page-master>
        <!-- Define other page formats here and add their names to the OutputToAttribute definition in the XSD -->
      </fo:layout-master-set>
      <fo:page-sequence font-family="Calibri" font-size="10pt">
        <xsl:choose>
          <xsl:when test="@outputTo">
            <xsl:attribute name="master-reference">
              <xsl:value-of select="@outputTo"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="master-reference">A4Landscape</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>

        <fo:static-content flow-name="xsl-region-before">
          <fo:block>
            <xsl:call-template name="PageHeader"/>
          </fo:block>
        </fo:static-content>

        <fo:static-content flow-name="xsl-region-after">
          <fo:block>
            <xsl:call-template name="PageFooter"/>
          </fo:block>
        </fo:static-content>

        <fo:flow flow-name="xsl-region-body">
          <xsl:apply-templates/>
          <fo:block id="EndOfDocument"/>
        </fo:flow>

      </fo:page-sequence>
    </fo:root>
  </xsl:template>

  <xsl:template name="PageHeader">
    <xsl:choose>
      <xsl:when test="pageHeader">
        <fo:block font-size="8pt" font-style="italic">
          <fo:table table-layout="fixed" width="100%">
            <fo:table-column column-width="proportional-column-width(1)" column-number="1"/>
            <fo:table-column column-width="proportional-column-width(1)" column-number="2"/>
            <fo:table-body>
              <fo:table-row>
                <fo:table-cell>
                  <fo:block text-align="left">
                    <xsl:value-of select="pageHeader/@caption"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block text-align="right">
                    <xsl:if test="pageHeader/@pageNumbers='true'">
                      Page <fo:page-number/> of <fo:page-number-citation ref-id="EndOfDocument"/>
                    </xsl:if>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>
            </fo:table-body>
          </fo:table>
        </fo:block>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="PageFooter">
    <xsl:choose>
      <xsl:when test="pageFooter">
        <fo:block font-size="8pt" font-style="italic">
          <fo:table table-layout="fixed" width="100%">
            <fo:table-column column-width="proportional-column-width(1)" column-number="1"/>
            <fo:table-column column-width="proportional-column-width(1)" column-number="2"/>
            <fo:table-body>
              <fo:table-row>
                <fo:table-cell>
                  <fo:block text-align="left">
                    <xsl:value-of select="pageFooter/@caption"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block text-align="right">
                    <xsl:if test="pageFooter/@pageNumbers='true'">
                      Page <fo:page-number/> of <fo:page-number-citation ref-id="EndOfDocument"/>
                    </xsl:if>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>
            </fo:table-body>
          </fo:table>
        </fo:block>
      </xsl:when>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="section">
    <fo:block>
      <fo:table table-layout="fixed" width="100%">
        <xsl:for-each select="sectionColumn">
          <xsl:variable name="width">
            <xsl:choose>
              <xsl:when test="@width">
                <xsl:value-of select="@width"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <xsl:choose>
            <xsl:when test="position()>1">
              <fo:table-column column-width="0.5cm" />
            </xsl:when>
            <xsl:otherwise>
              <fo:table-column column-width="0.0cm" />
            </xsl:otherwise>
          </xsl:choose>
          <fo:table-column column-width="proportional-column-width({$width})" />
          <!-- column-number="{position()}"-->
        </xsl:for-each>
        <fo:table-body>
          <fo:table-row>
            <xsl:apply-templates/>
          </fo:table-row>
        </fo:table-body>
      </fo:table>
    </fo:block>
  </xsl:template>

  <xsl:template match="sectionColumn">
    <fo:table-cell>
      <fo:block>
        <!-- empty cell for layout purposes -->
      </fo:block>
    </fo:table-cell>
    <fo:table-cell>
      <fo:block>
        <xsl:apply-templates/>
      </fo:block>
    </fo:table-cell>
  </xsl:template>

  <xsl:template match="heading1">
    <fo:block font-size="16pt" space-after="0.1cm" keep-with-next.within-page="always">
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="heading1CenterAligned">
    <fo:block font-size="16pt" space-after="0.1cm" text-align="center" keep-with-next.within-page="always">
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="heading2">
    <fo:block font-size="12pt" space-before="0.3cm" space-after="0.1cm" keep-with-next.within-page="always">
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="paragraph">
    <fo:block font-size="10pt" space-after="0.2cm">
      <xsl:apply-templates/>
    </fo:block>
  </xsl:template>

  <xsl:template match="image">
    <fo:block>
      <fo:external-graphic src="url({@source})" background-color="white" />
    </fo:block>
  </xsl:template>

  <xsl:template match="pageBreak">
    <fo:block break-after="page"/>
  </xsl:template>

  <xsl:template match="table">
    <!-- The border needs to be in the enclosing block rather than in the table because the FoNet engine had a defect that draws the background colour over the table borders -->
    <fo:block font-size="10pt" space-before="0.1cm" space-after="0.1cm" border-width="0.05cm" border-style="solid">
      <fo:table table-layout="fixed">
        <xsl:choose>
          <xsl:when test="@width">
            <xsl:attribute name="width">
              <xsl:value-of select="@width"/>
            </xsl:attribute>
          </xsl:when>
          <xsl:otherwise>
            <xsl:attribute name="width">100%</xsl:attribute>
          </xsl:otherwise>
        </xsl:choose>

        <xsl:for-each select="columns/column">
          <xsl:variable name="width">
            <xsl:choose>
              <xsl:when test="@width">
                <xsl:value-of select="@width"/>
              </xsl:when>
              <xsl:otherwise>
                <xsl:value-of select="1"/>
              </xsl:otherwise>
            </xsl:choose>
          </xsl:variable>
          <fo:table-column column-width="proportional-column-width({$width})" column-number="{position()}" />
        </xsl:for-each>

        <!-- Process the header rows -->
        <xsl:if test="header">
          <fo:table-header font-weight="bold">
            <xsl:if test="@useShading='true'">
              <xsl:attribute name="background-color">rgb(150,150,150)</xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="header" />
          </fo:table-header>
        </xsl:if>
        <xsl:if test="footer">
          <fo:table-footer>
            <xsl:if test="@useShading='true'">
              <xsl:attribute name="background-color">rgb(190,190,190)</xsl:attribute>
            </xsl:if>
            <xsl:apply-templates select="footer" />
          </fo:table-footer>
        </xsl:if>
        <xsl:if test="row">
          <fo:table-body>
            <xsl:apply-templates select="row" />
          </fo:table-body>
        </xsl:if>

      </fo:table>
    </fo:block>
  </xsl:template>

  <xsl:template match="header">
    <fo:table-row text-align="center" border-bottom-width="0.01cm" border-bottom-style="solid">
      <xsl:apply-templates />
    </fo:table-row>
  </xsl:template>

  <xsl:template match="footer">
    <fo:table-row border-top-width="0.01cm" border-top-style="solid">
      <xsl:apply-templates />
    </fo:table-row>
  </xsl:template>

  <xsl:template match="row">
    <fo:table-row>
      <xsl:if test="@shade='true'">
        <xsl:if test="ancestor::table[1]/@useShading='true'">
          <xsl:attribute name="background-color">
            <xsl:value-of select="'rgb(230,230,230)'"/>
          </xsl:attribute>
        </xsl:if>
      </xsl:if>
      <xsl:apply-templates/>
    </fo:table-row>
  </xsl:template>

  <xsl:template match="cell">
    <fo:table-cell padding="0.5mm">
      <xsl:choose>
        <xsl:when test="ancestor::table[1]/@borders='true'">
          <!-- Show all borders -->
          <xsl:attribute name="border-width">0.01cm</xsl:attribute>
          <xsl:attribute name="border-style">solid</xsl:attribute>
        </xsl:when>
        <xsl:otherwise>
          <!-- Show the column borders only -->
          <xsl:attribute name="border-left-width">0.01cm</xsl:attribute>
          <xsl:attribute name="border-left-style">solid</xsl:attribute>
          <xsl:attribute name="border-right-width">0.01cm</xsl:attribute>
          <xsl:attribute name="border-right-style">solid</xsl:attribute>
        </xsl:otherwise>
      </xsl:choose>
      <fo:block start-indent="0.1cm" end-indent="0.1cm"  space-before="0.1cm" space-after="0.1cm">
        <xsl:if test="@align">
          <xsl:attribute name="text-align">
            <xsl:value-of select="@align"/>
          </xsl:attribute>
        </xsl:if>
        <xsl:value-of select="."/>
      </fo:block>
    </fo:table-cell>
  </xsl:template>

</xsl:stylesheet>
