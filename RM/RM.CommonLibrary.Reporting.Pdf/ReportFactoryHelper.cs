using System;
using System.Runtime.CompilerServices;
using System.Xml;

[assembly: InternalsVisibleTo("RM.CommonLibrary.Reporting.Pdf.Test")]
namespace RM.CommonLibrary.Reporting.Pdf
{
    /// <summary>
    /// Helper methods for report generation using FMO_PDFReport_Generic (xslt and xsd)
    /// 
    /// Use the methods in this class to construct a report XML document that can be rendered as a PDF document
    ///   using one of the overloads of the PDFGenerator.CreatePDF method.
    /// The helper methods in this class must be used in conjunction with the schema FMO_PDFReport_Generic.xsd
    ///   to produce a valid report XML document. The schema controls the overall structure and sequencing of 
    ///   elements; the helper methods in this class provide a standardized mechanism of creating elements and
    ///   attributes that comply with the schema from simple data types. Refer to previous implementations of
    ///   reports that use this class for guidance on how to assemble a report using these helper methods.
    ///   
    /// Note: this class does not incorporate logging functionality by design
    /// </summary>
    public static class ReportFactoryHelper
    {
        /// <summary>
        /// Cell align attribute options
        /// </summary>
        public enum CellAlignOption
        {
            Left,
            Center,
            Right
        }



        /// <summary>
        /// Report output to options
        /// </summary>
        public enum ReportOutputToOption
        {
            Unknown,
            A0Portrait,
            A0Landscape,
            A1Portrait,
            A1Landscape,
            A2Portrait,
            A2Landscape,
            A3Portrait,
            A3Landscape,
            A4Portrait,
            A4Landscape
        }



        /// <summary>
        /// Text align attribute options
        /// </summary>
        public enum TextAlignOption
        {
            Left,
            Center,
            Right
        }





        /// <summary>
        /// Adds an attribute to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="name">The name of the attribute</param>
        /// <param name="value">The value to assign to the attribute</param>
        /// <param name="required">True if the attribute is required by the XSD, otherwise false</param>
        internal static void AddAttribute(XmlElement parent, XmlDocument root, string name, string value, bool required)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException($"{nameof(name)} must not be null or empty."); }
            // name does not require validation
            // required does not require validation


            // If the attribute is required or the attribute has a value
            if (required || !string.IsNullOrWhiteSpace(value))
            {
                // Add the attribute to the parent element
                XmlAttribute attr = null;
                attr = root.CreateAttribute(name);
                attr.Value = value;
                parent.Attributes.Append(attr);
            }
        }



        /// <summary>
        /// Adds a caption element with the specified caption text to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="caption">The caption text</param>
        /// <param name="align">The text alignment</param>
        public static void AddCaptionElement(XmlElement parent, XmlDocument root, string caption, TextAlignOption align)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // caption does not require validation
            // align does not require validation


            // Add the caption element with the specified caption
            const string nodeName = "caption";
            XmlElement node = AddElement(parent, root, nodeName);
            node.InnerText = caption;

            // Add the align attribute
            if (align != TextAlignOption.Left)
            {
                const string alignAttributeName = "align";
                string alignValue = GetTextAlignAttributeValue(align);
                AddAttribute(node, root, alignAttributeName, alignValue, false);
            }
        }



        /// <summary>
        /// Adds a content element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <returns>A reference to the content element</returns>
        public static XmlElement AddContentElement(XmlElement parent, XmlDocument root)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }


            // Add a content element
            const string nodeName = "content";
            return AddElement(parent, root, nodeName);
        }



        /// <summary>
        /// Adds a new element named nodeName to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="nodeName">The name of the element</param>
        /// <returns>A reference to the new element</returns>
        internal static XmlElement AddElement(XmlElement parent, XmlDocument root, string nodeName)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            if (string.IsNullOrWhiteSpace(nodeName)) { throw new ArgumentException($"{nameof(nodeName)} must not be null or empty."); }


            // Add the element
            XmlElement node = root.CreateElement(nodeName);
            parent.AppendChild(node);

            // Return the element
            return node;
        }



        /// <summary>
        /// Adds a full width section containing a heading1 heading to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="heading">The heading text</param>
        public static void AddFullWidthSectionWithMainHeading(XmlElement parent, XmlDocument root, string heading)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // heading does not require validation


            // Create the section element
            XmlElement sectionNode = AddSectionElement(parent, root);

            // Create the sectionColumn element
            XmlElement sectionColumnNode = AddSectionColumnElement(sectionNode, root, 1);

            // Create the heading1 element
            AddHeading1Element(sectionColumnNode, root, heading);
        }



        /// <summary>
        /// Adds a heading1 element with the specified heading text to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="heading">The heading text</param>
        public static void AddHeading1Element(XmlElement parent, XmlDocument root, string heading)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // heading does not require validation


            // Add the heading1 element with the specified heading
            const string nodeName = "heading1";
            XmlElement node = AddElement(parent, root, nodeName);
            node.InnerText = heading;
        }



        /// <summary>
        /// Adds a heading2 element with the specified heading text to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="heading">The heading text</param>
        public static void AddHeading2Element(XmlElement parent, XmlDocument root, string heading)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // heading does not require validation


            // Add the heading2 element with the specified heading
            const string nodeName = "heading2";
            XmlElement node = AddElement(parent, root, nodeName);
            node.InnerText = heading;
        }



        /// <summary>
        /// Adds an image element with the specified source to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="source">The image source</param>
        public static void AddImageElement(XmlElement parent, XmlDocument root, string source)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            if (string.IsNullOrWhiteSpace(source)) { throw new ArgumentException($"{nameof(source)} must not be null or empty, and should contain the file URI for the image source file."); }


            // Add the image element
            const string nodeName = "image";
            XmlElement node = AddElement(parent, root, nodeName);

            // Add the source attribute
            const string sourceAttributeName = "source";
            AddAttribute(node, root, sourceAttributeName, source, false);
        }



        /// <summary>
        /// Adds a legalNotices element with the specified legal notices to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="legalNotices">The legal notices</param>
        /// <param name="align">The text alignment</param>
        internal static void AddLegalNoticesElement(XmlElement parent, XmlDocument root, string[] legalNotices, TextAlignOption align)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // legalNotices does not require validation
            // align does not require validation


            // If there are any legal notices to add
            if (legalNotices != null)
            {
                if (legalNotices.Length > 0)
                {
                    // Add the legalNotices element 
                    const string nodeName = "legalNotices";
                    XmlElement node = AddElement(parent, root, nodeName);

                    // Add the legal notices
                    foreach (string notice in legalNotices)
                    {
                        AddLegalNoticeElement(node, root, notice, align);
                    }
                }
            }
        }



        /// <summary>
        /// Adds a legalNotice element with the specified notice text to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="notice">The notice text</param>
        /// <param name="align">The text alignment</param>
        internal static void AddLegalNoticeElement(XmlElement parent, XmlDocument root, string notice, TextAlignOption align)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // notice does not require validation
            // align does not require validation


            // Add the legalNotice element with the specified notice
            const string nodeName = "legalNotice";
            XmlElement node = AddElement(parent, root, nodeName);
            node.InnerText = notice;

            // Add the align attribute
            if (align != TextAlignOption.Left)
            {
                const string alignAttributeName = "align";
                string alignValue = GetTextAlignAttributeValue(align);
                AddAttribute(node, root, alignAttributeName, alignValue, false);
            }
        }



        /// <summary>
        /// Adds a map element with the specified source to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="source">The map source</param>
        /// <param name="timestamp">The timestamp</param>
        /// <param name="scale">The scale</param>
        internal static void AddMapElement(XmlElement parent, XmlDocument root, string source, string timestamp, string scale)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            if (string.IsNullOrWhiteSpace(source)) { throw new ArgumentException($"{nameof(source)} must not be null or empty, and should contain the file URI for the map source file."); }
            // timestamp does not require validation
            // scale does not require validation


            // Add the map element
            const string nodeName = "map";
            XmlElement node = AddElement(parent, root, nodeName);

            // Add the source attribute
            const string sourceAttributeName = "source";
            AddAttribute(node, root, sourceAttributeName, source, false);

            // If either the timestamp or scale is set
            if (!string.IsNullOrWhiteSpace(timestamp) || !string.IsNullOrWhiteSpace(scale))
            {
                // Add a map footer element
                const string mapFooterNodeName = "mapFooter";
                XmlElement mapFooterNode = AddElement(node, root, mapFooterNodeName);

                // Add the timestamp attribute
                const string timestampAttributeName = "timestamp";
                AddAttribute(mapFooterNode, root, timestampAttributeName, timestamp, false);

                // Add the scale attribute
                const string scaleAttributeName = "scale";
                AddAttribute(mapFooterNode, root, scaleAttributeName, scale, false);
            }
        }



        /// <summary>
        /// Adds a map section to the parent element
        /// The map section uses images that are designed to fit on a single page when combined with the caption
        ///   and the legal notices without a page header or page footer
        /// Note: the images may need to be resized if the length of the legal notices changes
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="caption">The caption</param>
        /// <param name="timestamp">The timestamp</param>
        /// <param name="scale">The scale</param>
        /// <param name="source">The path to the map image</param>
        /// <param name="legalNotices">The legal notices</param>
        internal static void AddMapSection(XmlElement parent, XmlDocument root, string caption, string source, string timestamp, string scale, string[] legalNotices)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // caption does not require validation
            if (string.IsNullOrWhiteSpace(source)) { throw new ArgumentException($"{nameof(source)} must not be null or empty, and should contain the file URI for the map source file."); }
            // timestamp does not require validation
            // scale does not require validation
            // legalNotices does not require validation


            // A map section is a full width section that contains a caption, the map image and a copyright notice
            //   on a single page
            //
            // Create the section element
            XmlElement sectionNode = AddSectionElement(parent, root);

            // Create the sectionColumn element
            XmlElement sectionColumnNode = AddSectionColumnElement(sectionNode, root, 1);

            // Create the caption element for the caption
            AddCaptionElement(sectionColumnNode, root, caption, TextAlignOption.Center);

            // Create the map element for the map
            AddMapElement(sectionColumnNode, root, source, timestamp, scale);

            // Create the legalNotices element for the legal notices
            AddLegalNoticesElement(sectionColumnNode, root, legalNotices, TextAlignOption.Left);
        }



        /// <summary>
        /// Adds a pageBreak element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        public static void AddPageBreakElement(XmlElement parent, XmlDocument root)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }


            // Add the pageBreak element 
            const string nodeName = "pageBreak";
            XmlElement node = AddElement(parent, root, nodeName);
        }



        /// <summary>
        /// Adds a pageFooter element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="caption">Optional caption</param>
        /// <param name="showPageNumbers">True to show page numbers, otherwise false</param>
        public static void AddPageFooterElement(XmlElement parent, XmlDocument root, string caption, bool showPageNumbers)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // caption does not require validation
            // showPageNumbers does not require validation


            // If the page footer has any content to display
            if (!string.IsNullOrWhiteSpace(caption) || showPageNumbers)
            {
                // Add the pageFooter element
                const string nodeName = "pageFooter";
                XmlElement node = AddElement(parent, root, nodeName);

                // Add the caption attribute
                if (!string.IsNullOrWhiteSpace(caption))
                {
                    const string attributeName = "caption";
                    AddAttribute(node, root, attributeName, caption, false);
                }

                // Add the pageNumbers attribute
                if (showPageNumbers)
                {
                    const string attributeName = "pageNumbers";
                    AddAttribute(node, root, attributeName, GetFormattedBoolean(showPageNumbers), false);
                }
            }
        }



        /// <summary>
        /// Adds a pageHeader element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="caption">Optional caption</param>
        /// <param name="showPageNumbers">True to show page numbers, otherwise false</param>
        public static void AddPageHeaderElement(XmlElement parent, XmlDocument root, string caption, bool showPageNumbers)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // caption does not require validation
            // showPageNumbers does not require validation


            // If the page footer has any content to display
            if (!string.IsNullOrWhiteSpace(caption) || showPageNumbers)
            {
                // Add the pageHeader element
                const string nodeName = "pageHeader";
                XmlElement node = AddElement(parent, root, nodeName);

                // Add the caption attribute
                if (!string.IsNullOrWhiteSpace(caption))
                {
                    const string attributeName = "caption";
                    AddAttribute(node, root, attributeName, caption, false);
                }

                // Add the pageNumbers attribute
                if (showPageNumbers)
                {
                    const string attributeName = "pageNumbers";
                    AddAttribute(node, root, attributeName, GetFormattedBoolean(showPageNumbers), false);
                }
            }
        }



        /// <summary>
        /// Adds a paragraph element with the specified paragraph text to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="paragraph">The paragraph text</param>
        public static void AddParagraphElement(XmlElement parent, XmlDocument root, string paragraph)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // paragraph does not require validation


            // Add the paragraph element with the specified paragraph of text
            const string nodeName = "paragraph";
            XmlElement node = AddElement(parent, root, nodeName);
            node.InnerText = paragraph;
        }



        /// <summary>
        /// Adds a report element to the parent element
        /// </summary>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="outputTo">The output to option for the report</param>
        /// <param name="allowSpaceForHeaderAndFooter">True if the report has a header or a footer, otherwise false</param>
        /// <returns>A reference to the report element</returns>
        public static XmlElement AddReportElement(XmlDocument root, ReportOutputToOption outputTo, bool allowSpaceForHeaderAndFooter)
        {
            // Validate the arguments
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            if (outputTo == ReportOutputToOption.Unknown) { throw new ArgumentException($"{nameof(outputTo)} must be set to a specific paper size and orientation. {nameof(ReportOutputToOption.Unknown)} is not supported.", nameof(outputTo)); }
            // allowSpaceForHeaderAndFooter does not require validation


            // Add the report element
            const string nodeName = "report";
            XmlElement node = root.CreateElement(nodeName);
            root.AppendChild(node);

            // Add the outputTo attribute
            const string outputToAttributeName = "outputTo";
            string outputToValue = GetReportOutputToAttributeValue(outputTo, allowSpaceForHeaderAndFooter);
            AddAttribute(node, root, outputToAttributeName, outputToValue, false);

            // Return the report element
            return node;
        }



        /// <summary>
        /// Adds a section column element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="proportionalWidth">Optional proportional width (null or positive integer)</param>
        /// <returns>A reference to the section column element</returns>
        public static XmlElement AddSectionColumnElement(XmlElement parent, XmlDocument root, int? proportionalWidth)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // proportionalWidth does not require validation


            // Add the sectionColumn element
            const string nodeName = "sectionColumn";
            XmlElement node = AddElement(parent, root, nodeName);

            // Add the width attribute
            if (proportionalWidth.HasValue)
            {
                if (proportionalWidth.Value > 0)
                {
                    const string widthAttributeName = "width";
                    AddAttribute(node, root, widthAttributeName, proportionalWidth.Value.ToString(), false);
                }
            }

            // Return the sectionColumn element
            return node;
        }



        /// <summary>
        /// Adds a section element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <returns>A reference to the section element</returns>
        public static XmlElement AddSectionElement(XmlElement parent, XmlDocument root)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }


            // Add the section element
            const string nodeName = "section";
            XmlElement node = AddElement(parent, root, nodeName);

            // Return the section element
            return node;
        }



        /// <summary>
        /// Adds a table cell element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="content">The cell contents</param>
        /// <param name="align">The cell content alignment</param>
        public static void AddTableCellElement(XmlElement parent, XmlDocument root, string content, CellAlignOption align)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // content does not require validation
            // align does not require validation


            // Add the cell element
            const string nodeName = "cell";
            XmlElement node = AddElement(parent, root, nodeName);
            node.InnerText = content;

            // Add the align attribute
            if (align != CellAlignOption.Left)
            {
                const string alignAttributeName = "align";
                string alignValue = GetCellAlignAttributeValue(align);
                AddAttribute(node, root, alignAttributeName, alignValue, false);
            }
        }



        /// <summary>
        /// Adds a table column element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="proportionalWidth">Optional proportional width (null or positive integer)</param>
        internal static void AddTableColumnElement(XmlElement parent, XmlDocument root, int? proportionalWidth)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // proportionalWidth does not require validation


            // Add the column element
            const string nodeName = "column";
            XmlElement node = AddElement(parent, root, nodeName);

            // Add the width attribute
            if (proportionalWidth.HasValue)
            {
                if (proportionalWidth.Value > 0)
                {
                    const string widthAttributeName = "width";
                    AddAttribute(node, root, widthAttributeName, proportionalWidth.Value.ToString(), false);
                }
            }
        }



        /// <summary>
        /// Adds a table columns element to the parent element
        /// The table columns element is populated with table column elements using the proportional widths specified
        ///   in the columnProportionalWidths array. The array should be sized for the number of columns to display
        ///   regardless of whether proportional widths are being specified.
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="columnProportionalWidths">Array of proportional widths (each proportional width must be null or a positive integer); the array size determine the number of table columns to add</param>
        public static void AddTableColumnsElement(XmlElement parent, XmlDocument root, int?[] columnProportionalWidths)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            if (columnProportionalWidths == null) { throw new ArgumentNullException(nameof(columnProportionalWidths)); }
            if (columnProportionalWidths.Length <= 0) { throw new ArgumentException($"{nameof(columnProportionalWidths)} must contain at least one element."); }


            // Add the columns element
            const string nodeName = "columns";
            XmlElement columnsNode = AddElement(parent, root, nodeName);

            // Add the column elements
            foreach (int? proportionalWidth in columnProportionalWidths)
            {
                // Add a column element
                AddTableColumnElement(columnsNode, root, proportionalWidth);
            }
        }



        /// <summary>
        /// Adds a table element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="width">Optional table width (null or positive integer, forced to 0-100) and expressed as a percentage</param>
        /// <param name="useBorders">True if horizontal borders should be used in the table, otherwise false</param>
        /// <param name="useShading">True if shading should be used in the table, otherwise false</param>
        /// <returns>A reference to the table element</returns>
        public static XmlElement AddTableElement(XmlElement parent, XmlDocument root, int? width, bool useBorders, bool useShading)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // width does not require validation
            // useBorders does not require validation
            // useShading does not require validation


            // Add the table element
            const string nodeName = "table";
            XmlElement node = AddElement(parent, root, nodeName);

            // Add the width attribute
            if (width.HasValue)
            {
                // Format the width
                if (width.Value < 0) width = 0;
                if (width.Value > 100) width = 100;
                string formattedWidth = width.Value.ToString() + "%";

                const string attributeName = "width";
                AddAttribute(node, root, attributeName, formattedWidth, false);
            }

            // Add the borders attribute
            if (useBorders)
            {
                const string attributeName = "borders";
                AddAttribute(node, root, attributeName, GetFormattedBoolean(true), false);
            }

            // Add the useShading attribute
            if (useShading)
            {
                const string attributeName = "useShading";
                AddAttribute(node, root, attributeName, GetFormattedBoolean(true), false);
            }

            // Return the table element
            return node;
        }



        /// <summary>
        /// Adds a table footer element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <returns>A reference to the table footer element</returns>
        public static XmlElement AddTableFooterElement(XmlElement parent, XmlDocument root)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }


            // Add the footer element
            const string nodeName = "footer";
            XmlElement node = AddElement(parent, root, nodeName);

            // Return the footer element
            return node;
        }



        /// <summary>
        /// Adds a table header element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <returns>A reference to the table header element</returns>
        public static XmlElement AddTableHeaderElement(XmlElement parent, XmlDocument root)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }


            // Add the header element
            const string nodeName = "header";
            XmlElement node = AddElement(parent, root, nodeName);

            // Return the header element
            return node;
        }



        /// <summary>
        /// Adds a table row element to the parent element
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="useShading">True if shading is to be used on the table, otherwise false</param>
        /// <param name="shaded">True if the current row should be shaded, otherwise false</param>
        /// <returns>A reference to the table row element</returns>
        public static XmlElement AddTableRowElement(XmlElement parent, XmlDocument root, bool useShading, bool shaded)
        {
            // Validate the arguments
            if (parent == null) { throw new ArgumentNullException(nameof(parent)); }
            if (root == null) { throw new ArgumentNullException(nameof(root)); }
            // useShading does not require validation
            // shaded does not require validation


            // Add the row element
            const string nodeName = "row";
            XmlElement node = AddElement(parent, root, nodeName);

            // Add the shade attribute
            if (useShading && shaded)
            {
                const string attributeName = "shade";
                AddAttribute(node, root, attributeName, GetFormattedBoolean(true), false);
            }

            // Return the row element
            return node;
        }



        /// <summary>
        /// Gets the cell align attribute value for a specified align option
        /// </summary>
        /// <param name="align">The align option</param>
        /// <returns>The cell align attribute value</returns>
        public static string GetCellAlignAttributeValue(CellAlignOption align)
        {
            // Validate the arguments
            // align does not require validation


            string formattedValue = null;
            switch (align)
            {
                case CellAlignOption.Left:
                    formattedValue = "left";
                    break;
                case CellAlignOption.Center:
                    formattedValue = "center";
                    break;
                case CellAlignOption.Right:
                    formattedValue = "right";
                    break;
                default:
                    throw new ArgumentException($"Argument {nameof(align)} contains an unsupported value.", nameof(align));
            }

            return formattedValue;
        }



        /// <summary>
        /// Gets the formatted string equivalent of a boolean value
        /// </summary>
        /// <param name="value">The boolean value to format</param>
        /// <returns>The formatted value</returns>
        public static string GetFormattedBoolean(bool value)
        {
            return value ? "true" : "false";
        }



        /// <summary>
        /// Gets the report outputTo attribute value for a specified output to option (using the standard page masters)
        /// </summary>
        /// <param name="outputTo">The output to option</param>
        /// <param name="allowSpaceForHeaderAndFooter">True if the report should allow space for a header or a footer, otherwise false</param>
        /// <returns>The report outputTo attribute value</returns>
        public static string GetReportOutputToAttributeValue(ReportOutputToOption outputTo, bool allowSpaceForHeaderAndFooter)
        {
            // Validate the arguments
            // outputTo does not require validation
            // allowSpaceForHeaderAndFooter does not require validation


            // Get the formatted value for the page size and orientation
            string formattedValue = null;
            switch (outputTo)
            {
                case ReportOutputToOption.A0Portrait:
                    formattedValue = "A0Portrait";
                    break;
                case ReportOutputToOption.A0Landscape:
                    formattedValue = "A0Landscape";
                    break;
                case ReportOutputToOption.A1Portrait:
                    formattedValue = "A1Portrait";
                    break;
                case ReportOutputToOption.A1Landscape:
                    formattedValue = "A1Landscape";
                    break;
                case ReportOutputToOption.A2Portrait:
                    formattedValue = "A2Portrait";
                    break;
                case ReportOutputToOption.A2Landscape:
                    formattedValue = "A2Landscape";
                    break;
                case ReportOutputToOption.A3Portrait:
                    formattedValue = "A3Portrait";
                    break;
                case ReportOutputToOption.A3Landscape:
                    formattedValue = "A3Landscape";
                    break;
                case ReportOutputToOption.A4Portrait:
                    formattedValue = "A4Portrait";
                    break;
                case ReportOutputToOption.A4Landscape:
                    formattedValue = "A4Landscape";
                    break;
                default:
                    if (outputTo != ReportOutputToOption.Unknown)
                    {
                        throw new ArgumentException($"Argument {nameof(outputTo)} contains an unsupported value.", nameof(outputTo));
                    }

                    break;
            }

            // If the paper size and orientation are set and the report should not allow space for a header or a footer
            if (outputTo != ReportOutputToOption.Unknown && !allowSpaceForHeaderAndFooter)
            {
                const string noHeaderOrFooter = "NoHeaderOrFooter";
                formattedValue += noHeaderOrFooter;
            }

            return formattedValue;
        }



        /// <summary>
        /// Gets the text align attribute value for a specified align option
        /// </summary>
        /// <param name="align">The align option</param>
        /// <returns>The text align attribute value</returns>
        public static string GetTextAlignAttributeValue(TextAlignOption align)
        {
            // Validate the arguments
            // align does not require validation


            string formattedValue = null;
            switch (align)
            {
                case TextAlignOption.Left:
                    formattedValue = "left";
                    break;
                case TextAlignOption.Center:
                    formattedValue = "center";
                    break;
                case TextAlignOption.Right:
                    formattedValue = "right";
                    break;
                default:
                    throw new ArgumentException($"Argument {nameof(align)} contains an unsupported value.", nameof(align));
            }

            return formattedValue;
        }
    }
}
