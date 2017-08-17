using RM.CommonLibrary.Reporting.Pdf;
using RM.Operational.RouteLog.WebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    /// <summary>
    /// Factory class for generating the Route Log Summary report XML document
    /// </summary>
    internal static class RouteLogSummaryReportFactory
    {
        /// <summary>
        /// Adds the centre table in the route log summary section
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="tableSource">Reference to the table source data</param>
        /// <param name="options">The report options</param>
        private static void AddRouteLogSummaryCenterTable(XmlElement parent, XmlDocument root, List<Tuple<string, string>> tableSource, RouteLogSummaryConfiguration options)
        {
            // If there are any rows in the table source
            if (tableSource.Count > 0)
            {
                // Add the table element
                int? width = null;
                bool useBorders = options.UseTableBorders;
                bool useShading = options.UseTableShading;
                XmlElement tableNode = ReportFactoryHelper.AddTableElement(parent, root, width, useBorders, useShading);

                // Add the columns
                int?[] columnProportionalWidths = { 1, 1 };
                ReportFactoryHelper.AddTableColumnsElement(tableNode, root, columnProportionalWidths);

                // The table does not have a header 

                // Add the rows
                bool shaded = false;
                foreach (var row in tableSource)
                {
                    // Add the current row
                    XmlElement rowNode = ReportFactoryHelper.AddTableRowElement(tableNode, root, useShading, shaded);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Item1, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Item2, ReportFactoryHelper.CellAlignOption.Center);

                    // Toggle the shaded flag
                    shaded = !shaded;
                }

                // The table does not have a footer
            }
        }



        /// <summary>
        /// Adds the left table in the route log summary section
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="tableSource">Reference to the table source data</param>
        /// <param name="options">The report options</param>
        private static void AddRouteLogSummaryLeftTable(XmlElement parent, XmlDocument root, List<Tuple<string, string>> tableSource, RouteLogSummaryConfiguration options)
        {
            // If there are any rows in the table source
            if (tableSource.Count > 0)
            {
                // Add the table element
                int? width = null;
                bool useBorders = options.UseTableBorders;
                bool useShading = options.UseTableShading;
                XmlElement tableNode = ReportFactoryHelper.AddTableElement(parent, root, width, useBorders, useShading);

                // Add the columns
                int?[] columnProportionalWidths = { 1, 1 };
                ReportFactoryHelper.AddTableColumnsElement(tableNode, root, columnProportionalWidths);

                // The table does not have a header 

                // Add the rows
                bool shaded = false;
                foreach (var row in tableSource)
                {
                    // Add the current row
                    XmlElement rowNode = ReportFactoryHelper.AddTableRowElement(tableNode, root, useShading, shaded);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Item1, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Item2, ReportFactoryHelper.CellAlignOption.Center);

                    // Toggle the shaded flag
                    shaded = !shaded;
                }

                // The table does not have a footer
            }
        }



        /// <summary>
        /// Adds the right table in the route log summary section
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="tableSource">Reference to the table source data</param>
        /// <param name="options">The report options</param>
        private static void AddRouteLogSummaryRightTable(XmlElement parent, XmlDocument root, List<Tuple<string, string>> tableSource, RouteLogSummaryConfiguration options)
        {
            // If there are any rows in the table source
            if (tableSource.Count > 0)
            {
                // Add the table element
                int? width = null;
                bool useBorders = options.UseTableBorders;
                bool useShading = options.UseTableShading;
                XmlElement tableNode = ReportFactoryHelper.AddTableElement(parent, root, width, useBorders, useShading);

                // Add the columns
                int?[] columnProportionalWidths = { 1, 1 };
                ReportFactoryHelper.AddTableColumnsElement(tableNode, root, columnProportionalWidths);

                // The table does not have a header 

                // Add the rows
                bool shaded = false;
                foreach (var row in tableSource)
                {
                    // Add the current row
                    XmlElement rowNode = ReportFactoryHelper.AddTableRowElement(tableNode, root, useShading, shaded);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Item1, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Item2, ReportFactoryHelper.CellAlignOption.Center);

                    // Toggle the shaded flag
                    shaded = !shaded;
                }

                // The table does not have a footer
            }
        }



        /// <summary>
        /// Adds the route log summary section
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="leftTableSource">Reference to the table source data for the left table</param>
        /// <param name="centerTableSource">Reference to the table source data for the center table</param>
        /// <param name="rightTableSource">Reference to the table source data for the right table</param>
        /// <param name="options">The report options</param>
        private static void AddRouteLogSummarySection(XmlElement parent, XmlDocument root, List<Tuple<string, string>> leftTableSource, List<Tuple<string, string>> centerTableSource, List<Tuple<string, string>> rightTableSource, RouteLogSummaryConfiguration options)
        {
            // Add the section element
            XmlElement sectionNode = ReportFactoryHelper.AddSectionElement(parent, root);

            // Add three equally-sized section columns
            XmlElement sectionColumnLeftNode = ReportFactoryHelper.AddSectionColumnElement(sectionNode, root, 1);
            XmlElement sectionColumnCenterNode = ReportFactoryHelper.AddSectionColumnElement(sectionNode, root, 1);
            XmlElement sectionColumnRightNode = ReportFactoryHelper.AddSectionColumnElement(sectionNode, root, 1);

            // The left section column contains a single table holding the contents of summaryLeftTable
            //
            // Add the table
            AddRouteLogSummaryLeftTable(sectionColumnLeftNode, root, leftTableSource, options);


            // The center section column contains a single table holding the contents of summaryCenterTable
            //
            // Add the table
            AddRouteLogSummaryCenterTable(sectionColumnCenterNode, root, centerTableSource, options);


            // The right section column contains a single table holding the contents of summaryRightTable followed by a paragraph of text
            //
            // Add the table
            AddRouteLogSummaryRightTable(sectionColumnRightNode, root, rightTableSource, options);

            // Add the paragraph
            string paragraph = "* All Alias, Hazards/Area Hazards and Special Instructions Information is shown on the detailed route log and hazard card."; // TODO load from resource file
            ReportFactoryHelper.AddParagraphElement(sectionColumnRightNode, root, paragraph);
        }



        /// <summary>
        /// Adds the sequenced points section
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="tableSource">Reference to the table source data</param>
        /// <param name="options">The report options</param>
        private static void AddSequencedPointsSection(XmlElement parent, XmlDocument root, List<RouteLogSummaryPoint> tableSource, RouteLogSummaryConfiguration options)
        {
            // If there are any sequenced points to add
            if (tableSource.Count > 0)
            {
                // Add the section element
                XmlElement sectionNode = ReportFactoryHelper.AddSectionElement(parent, root);

                // Add a single section column
                XmlElement sectionColumnNode = ReportFactoryHelper.AddSectionColumnElement(sectionNode, root, 1);

                // Add the section heading
                string heading = "Sequenced Points"; // TODO load from resource file
                ReportFactoryHelper.AddHeading2Element(sectionColumnNode, root, heading);

                // Add the sequenced points table
                AddSequencedPointsTable(sectionColumnNode, root, tableSource, options);
            }
        }



        /// <summary>
        /// Adds the sequenced points table to the sequenced points section
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="tableSource">Reference to the table source data</param>
        /// <param name="options">The report options</param>
        private static void AddSequencedPointsTable(XmlElement parent, XmlDocument root, List<RouteLogSummaryPoint> sequencedPoints, RouteLogSummaryConfiguration options)
        {
            // If there are any sequenced points to add
            if (sequencedPoints.Count > 0)
            {
                // Add the table element
                int? width = null;
                bool useBorders = options.UseTableBorders;
                bool useShading = options.UseTableShading;
                XmlElement tableNode = ReportFactoryHelper.AddTableElement(parent, root, width, useBorders, useShading);

                // Add the columns
                int?[] columnProportionalWidths = { 1, 1, 1, 1, 1, 1 };
                ReportFactoryHelper.AddTableColumnsElement(tableNode, root, columnProportionalWidths);

                // Add the header row
                XmlElement headerNode = ReportFactoryHelper.AddTableHeaderElement(tableNode, root);
                string caption = "Street"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "Number"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "DPs"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "Multiple Occupancy"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "Special Instructions*"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "Hazards/Area Hazards*"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);

                // Add the rows
                bool shaded = false;
                foreach (var row in sequencedPoints)
                {
                    // Add the current row
                    XmlElement rowNode = ReportFactoryHelper.AddTableRowElement(tableNode, root, useShading, shaded);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Street, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Number, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.DPs, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.MultipleOccupancy.ToString(), ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.SpecialInstructions, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.HazardsAndAreaHazards, ReportFactoryHelper.CellAlignOption.Center);

                    // Toggle the shaded flag
                    shaded = !shaded;
                }

                // The table does not have a footer
            }
        }



        /// <summary>
        /// Adds the unsequenced points section
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="tableSource">Reference to the table source data</param>
        /// <param name="options">The report options</param>
        private static void AddUnsequencedPointsSection(XmlElement parent, XmlDocument root, List<RouteLogSummaryPoint> unsequencedPoints, RouteLogSummaryConfiguration options)
        {
            // If there are any unsequenced points to add
            if (unsequencedPoints.Count > 0)
            {
                // Add the section element
                XmlElement sectionNode = ReportFactoryHelper.AddSectionElement(parent, root);

                // Add a single section column
                XmlElement sectionColumnNode = ReportFactoryHelper.AddSectionColumnElement(sectionNode, root, 1);

                // Add the section heading
                string heading = "Unsequenced Points"; // TODO load from resource file
                ReportFactoryHelper.AddHeading2Element(sectionColumnNode, root, heading);

                // Add the sequenced points table
                AddUnsequencedPointsTable(sectionColumnNode, root, unsequencedPoints, options);
            }
        }



        /// <summary>
        /// Adds the unsequenced points table to the unsequenced points section
        /// </summary>
        /// <param name="parent">Reference to the parent element</param>
        /// <param name="root">Reference to the root XML document</param>
        /// <param name="tableSource">Reference to the table source data</param>
        /// <param name="options">The report options</param>
        private static void AddUnsequencedPointsTable(XmlElement parent, XmlDocument root, List<RouteLogSummaryPoint> unsequencedPoints, RouteLogSummaryConfiguration options)
        {
            // If there are any unsequenced points to add
            if (unsequencedPoints.Count > 0)
            {
                // Add the table element
                int? width = null;
                bool useBorders = options.UseTableBorders;
                bool useShading = options.UseTableShading;
                XmlElement tableNode = ReportFactoryHelper.AddTableElement(parent, root, width, useBorders, useShading);

                // Add the columns
                int?[] columnProportionalWidths = { 1, 1, 1, 1, 1, 1 };
                ReportFactoryHelper.AddTableColumnsElement(tableNode, root, columnProportionalWidths);

                // Add the header row
                XmlElement headerNode = ReportFactoryHelper.AddTableHeaderElement(tableNode, root);
                string caption = "Street"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "Number"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "DPs"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "Multiple Occupancy"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "Special Instructions*"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);
                caption = "Hazards/Area Hazards*"; // TODO load from resource file
                ReportFactoryHelper.AddTableCellElement(headerNode, root, caption, ReportFactoryHelper.CellAlignOption.Center);

                // Add the rows
                bool shaded = false;
                foreach (var row in unsequencedPoints)
                {
                    // Add the current row
                    XmlElement rowNode = ReportFactoryHelper.AddTableRowElement(tableNode, root, useShading, shaded);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Street, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.Number, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.DPs, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.MultipleOccupancy, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.SpecialInstructions, ReportFactoryHelper.CellAlignOption.Center);
                    ReportFactoryHelper.AddTableCellElement(rowNode, root, row.HazardsAndAreaHazards, ReportFactoryHelper.CellAlignOption.Center);

                    // Toggle the shaded flag
                    shaded = !shaded;
                }

                // The table does not have a footer
            }
        }



        /// <summary>
        /// Gets the Route Log Summary XML Document for the specified data
        /// </summary>
        /// <param name="leftTable">The data for the left table in the route log summary</param>
        /// <param name="centerTable">The data for the center table in the route log summary</param>
        /// <param name="rightTable">The data for the right table in the route log summary</param>
        /// <param name="sequencedPoints">The data for the sequenced points table in the route log summary</param>
        /// <param name="unsequencedPoints">The data for the unsequenced points table in the route log summary</param>
        /// <param name="options">The report options</param>
        /// <returns>An XML Document containing the XML ready for transformation using FMO_PDFReport_Generic.xslt</returns>
        public static XmlDocument GetRouteLogSummary(List<Tuple<string, string>> leftTable, List<Tuple<string, string>> centerTable, List<Tuple<string, string>> rightTable, List<RouteLogSummaryPoint> sequencedPoints, List<RouteLogSummaryPoint> unsequencedPoints, RouteLogSummaryConfiguration options)
        {
            // Validate the arguments
            if (leftTable == null) { throw new ArgumentNullException(nameof(leftTable)); }
            if (centerTable == null) { throw new ArgumentNullException(nameof(centerTable)); }
            if (rightTable == null) { throw new ArgumentNullException(nameof(rightTable)); }
            if (sequencedPoints == null) { throw new ArgumentNullException(nameof(sequencedPoints)); }
            if (unsequencedPoints == null) { throw new ArgumentNullException(nameof(unsequencedPoints)); }
            if (options == null) { throw new ArgumentNullException(nameof(options)); }


            // Create the XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);

            // Create the report element
            XmlElement reportNode = ReportFactoryHelper.AddReportElement(xmlDoc, options.OutputTo, true);

            // The report element can contain pageHeader, pageFooter and content (required) elements
            string pageHeaderCaption = string.Empty; // TODO load from resource file
            string pageFooterCaption = string.Empty; // TODO load from resource file
            ReportFactoryHelper.AddPageHeaderElement(reportNode, xmlDoc, pageHeaderCaption, options.PageHeaderShowPageNumbers);
            ReportFactoryHelper.AddPageFooterElement(reportNode, xmlDoc, pageFooterCaption, options.PageFooterShowPageNumbers);
            XmlElement contentNode = ReportFactoryHelper.AddContentElement(reportNode, xmlDoc);

            // The content element contains the main heading, route log summary, sequenced points and unsequenced points
            //
            // Add the main heading
            string heading = "Route Log Summary"; // TODO load from resource file
            ReportFactoryHelper.AddFullWidthSectionWithMainHeading(contentNode, xmlDoc, heading);

            // Add the route log summary, sequenced points and unsequenced points
            AddRouteLogSummarySection(contentNode, xmlDoc, leftTable, centerTable, rightTable, options);
            AddSequencedPointsSection(contentNode, xmlDoc, sequencedPoints, options);
            AddUnsequencedPointsSection(contentNode, xmlDoc, unsequencedPoints, options);

            // Return the XML document containing the report
            return xmlDoc;
        }
    }
}
