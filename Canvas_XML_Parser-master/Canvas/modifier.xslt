<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <xsl:template match="/">
    <html>
      <body style="background-color:powderblue;">
        <h2>Drawing on Canvas</h2>
        <xsl:apply-templates select="CanvasContainer/canvas" />
      </body>
    </html>
  </xsl:template>
  <xsl:template match="canvas">
    <svg viewBox="-10 -10 2200 1200" xmlns="http://www.w3.org/2000/svg">
      <xsl:for-each select="Figure">
        <xsl:choose>
          <xsl:when test="@xsi:type='Circle'">
            <circle cx="{_x}" cy="{_y}" r="{_r}" stroke="green" stroke-width="4" fill="yellow" />
          </xsl:when>
          <xsl:when test="@xsi:type='Line'">
            <line x1="{_x1}" y1="{_y1}" x2="{_x2}" y2="{_y2}" style="stroke:rgb(255,0,0);stroke-width:2" />
          </xsl:when>
          <xsl:when test="@xsi:type='Rectangle'">
            <rect x="{_x1}" y="{_y1}" width="{_x1 + _x2}" height="{_y1 + _y2}" style="fill:blue;stroke:pink;stroke-width:5;fill-opacity:0.1;stroke-opacity:0.9" />
          </xsl:when>
          <xsl:when test="@xsi:type='Point'">
            <circle cx="{_x}" cy="{_y}" r="2" stroke="green" stroke-width="4" fill="red" />
          </xsl:when>
          <xsl:otherwise>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:for-each>
    </svg>
  </xsl:template>
</xsl:stylesheet>