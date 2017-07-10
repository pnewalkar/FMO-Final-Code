<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:fo="http://www.w3.org/1999/XSL/Format"
    exclude-result-prefixes="xs" version="2.0">
  <xsl:template match="/">
    <fo:root xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:svg="http://www.w3.org/2000/svg" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:msxml="urn:schemas-microsoft-com:xslt">
      <fo:layout-master-set>
        <fo:simple-page-master master-name="A4" page-width="297mm" page-height="210mm" margin-top="1cm" margin-bottom="1cm" margin-left="0cm"  margin-right="0cm">
          <fo:region-body region-name="xsl-region-body" margin="3cm"/>
          <fo:region-before region-name="xsl-region-before" extent="2cm" display-align="after"/>
          <fo:region-after region-name="xsl-region-after" extent="2cm"/>
          <fo:region-start region-name="xsl-region-start" extent="2cm"/>
          <fo:region-end region-name="xsl-region-end" extent="2cm"/>
        </fo:simple-page-master>
      </fo:layout-master-set>
      <fo:page-sequence master-reference="A4" language="en">
        <fo:static-content flow-name="xsl-region-before">
          <fo:block></fo:block>
        </fo:static-content>
        <fo:static-content flow-name="xsl-region-after">
          <fo:block></fo:block>
        </fo:static-content>
        <fo:static-content flow-name="xsl-region-start">
          <fo:block></fo:block>
        </fo:static-content>
        <fo:static-content flow-name="xsl-region-end">
          <fo:block></fo:block>
        </fo:static-content>
        <fo:flow flow-name="xsl-region-body">
          <fo:block>

            <fo:block>
              <fo:inline font-size="8.5mm">Route Log Summary</fo:inline>
            </fo:block>

          </fo:block>
          <fo:block>
          </fo:block>
          <fo:block></fo:block>
          <fo:block></fo:block>
          <fo:block>
            <fo:table  width="100%">
              <fo:table-column column-width="proportional-column-width(30.27)" column-number="1"/>
              <fo:table-column column-width="proportional-column-width(1.659)" column-number="2"/>
              <fo:table-column column-width="proportional-column-width(34.085)" column-number="3"/>
              <fo:table-column column-width="proportional-column-width(1.808)" column-number="4"/>
              <fo:table-column column-width="proportional-column-width(32.177)" column-number="5"/>
              <fo:table-body>
                <fo:table-row height="44.15mm" overflow="hidden">
                  <fo:table-cell>
                    <fo:block>
                      <fo:table border-collapse="collapse" width="100%" table-layout="fixed">
                        <fo:table-column column-width="proportional-column-width(50)" column-number="1"/>
                        <fo:table-column column-width="proportional-column-width(50)" column-number="2"/>
                        <fo:table-body>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Name</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/RouteName"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Number</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/RouteNumber"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Method</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/Method"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Delivery Office</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/DeliveryOffice"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Aliases*</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/Aliases"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Blocks</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/Blocks"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Scenario</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/ScenarioName"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                        </fo:table-body>
                      </fo:table>
                    </fo:block>
                  </fo:table-cell>
                  <fo:table-cell>
                    <fo:block></fo:block>
                  </fo:table-cell>
                  <fo:table-cell>
                    <fo:block>
                      <fo:table border-collapse="collapse" width="100%" table-layout="fixed">
                        <fo:table-column column-width="proportional-column-width(50)" column-number="1"/>
                        <fo:table-column column-width="proportional-column-width(50)" column-number="2"/>
                        <fo:table-body>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>CPs</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                0
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>DPs</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/DPs"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Business DPs</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/BusinessDPs"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Redential DPs</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/ResidentialDPs"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Acceleration in</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/AccelarationIn"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Acceleration out</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/AccelarationOut"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>Paired Route</fo:block>
                            </fo:table-cell>
                            <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                              <fo:block>
                                <xsl:value-of select="RouteLogSummaryModelDTO/DeliveryRoute/PairedRoute"/>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                        </fo:table-body>
                      </fo:table>
                    </fo:block>
                  </fo:table-cell>
                  <fo:table-cell>
                    <fo:block></fo:block>
                  </fo:table-cell>
                  <fo:table-cell>
                    <fo:block>
                      <fo:table  width="80%">
                        <fo:table-column column-width="proportional-column-width(100)" column-number="1"/>
                        <fo:table-body>
                          <fo:table-row height="17.66mm" overflow="hidden">
                            <fo:table-cell>
                              <fo:block>
                                <fo:table border-collapse="collapse" width="100%" table-layout="fixed">
                                  <fo:table-column column-width="proportional-column-width(50)" column-number="1"/>
                                  <fo:table-column column-width="proportional-column-width(50)" column-number="2"/>
                                  <fo:table-body>
                                    <fo:table-row>
                                      <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                                        <fo:block>No D2D</fo:block>
                                      </fo:table-cell>
                                      <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                                        <fo:block></fo:block>
                                      </fo:table-cell>
                                    </fo:table-row>
                                    <fo:table-row>
                                      <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                                        <fo:block>DP Exemptions</fo:block>
                                      </fo:table-cell>
                                      <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                                        <fo:block></fo:block>
                                      </fo:table-cell>
                                    </fo:table-row>
                                  </fo:table-body>
                                </fo:table>
                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row>
                            <fo:table-cell>
                              <fo:block></fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                          <fo:table-row height="17.04mm" overflow="hidden">
                            <fo:table-cell>
                              <fo:block>

                                <fo:block>*All Alias, Hazards/Area Hazards and Special Instructions Information is shown on the detailed route log and hazard card.</fo:block>

                              </fo:block>
                            </fo:table-cell>
                          </fo:table-row>
                        </fo:table-body>
                      </fo:table>
                    </fo:block>
                  </fo:table-cell>
                </fo:table-row>
              </fo:table-body>
            </fo:table>
          </fo:block>
          <fo:block></fo:block>
          <fo:block></fo:block>
          <fo:block></fo:block>
          <fo:block></fo:block>
          <fo:block>

            <fo:block>
              <fo:inline font-size="4.9mm" text-underline-style="solid" margin-top="1cm" margin-bottom="1cm" margin-left="0cm"  margin-right="0cm">Sequenced Points</fo:inline>
            </fo:block>
            <fo:block></fo:block>

          </fo:block>
          <fo:block></fo:block>
          <fo:block>
            <fo:table border-collapse="collapse" width="100%" table-layout="fixed">
              <fo:table-column column-width="proportional-column-width(16.667)" column-number="1"/>
              <fo:table-column column-width="proportional-column-width(16.667)" column-number="2"/>
              <fo:table-column column-width="proportional-column-width(16.667)" column-number="3"/>
              <fo:table-column column-width="proportional-column-width(16.667)" column-number="4"/>
              <fo:table-column column-width="proportional-column-width(16.667)" column-number="5"/>
              <fo:table-column column-width="proportional-column-width(16.667)" column-number="6"/>
              <fo:table-body>
                <fo:table-row>
                  <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                    <fo:block text-align="center">Street</fo:block>
                  </fo:table-cell>
                  <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                    <fo:block text-align="center">Number</fo:block>
                  </fo:table-cell>
                  <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                    <fo:block text-align="center">DP's</fo:block>
                  </fo:table-cell>
                  <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                    <fo:block text-align="center">Multiple Occupancy</fo:block>
                  </fo:table-cell>
                  <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                    <fo:block text-align="center">Special Instructions*</fo:block>
                  </fo:table-cell>
                  <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                    <fo:block text-align="center">Hazard / Area Hazards*</fo:block>
                  </fo:table-cell>
                </fo:table-row>
                <xsl:for-each select="RouteLogSummaryModelDTO/RouteLogSequencedPoints/RouteLogSequencedPointsDTO">
                  <fo:table-row>
                    <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                      <fo:block>
                        <xsl:value-of select="StreetName"/>
                      </fo:block>
                    </fo:table-cell>
                    <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                      <fo:block>
                        <xsl:value-of select="Description"/>
                      </fo:block>
                    </fo:table-cell>
                    <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                      <fo:block>
                        <xsl:value-of select="DeliveryPointCount"/>
                      </fo:block>
                    </fo:table-cell>
                    <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                      <fo:block>
                        <xsl:value-of select="MultipleOccupancy"/>
                      </fo:block>
                    </fo:table-cell>
                    <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                      <fo:block>

                      </fo:block>
                    </fo:table-cell>
                    <fo:table-cell border="0.36mm solid black" padding="0.71mm">
                      <fo:block>

                      </fo:block>
                    </fo:table-cell>
                  </fo:table-row>
                </xsl:for-each>
              </fo:table-body>
            </fo:table>
          </fo:block>
          <fo:block></fo:block>
          <fo:block></fo:block>
        </fo:flow>
      </fo:page-sequence>
    </fo:root>
  </xsl:template>
</xsl:stylesheet>