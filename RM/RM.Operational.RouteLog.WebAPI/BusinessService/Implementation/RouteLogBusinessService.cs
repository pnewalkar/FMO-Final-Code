using Fonet;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.RouteLog.WebAPI.DTO;
using RM.Operational.RouteLog.WebAPI.IntegrationService;
using RM.Operational.RouteLog.WebAPI.Utils;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using static RM.Operational.RouteLog.WebAPI.BusinessService.RouteSummaryGroup;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    public class RouteLogBusinessService : IRouteLogBusinessService
    {
        private string xsltFilepath = string.Empty;
        private IRouteLogIntegrationService routeLogIntegrationService;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int priority = LoggerTraceConstants.RouteLogAPIPriority;
        private int entryEventId = LoggerTraceConstants.RouteLogBusinessServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.RouteLogBusinessServiceMethodExitEventId;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService" /> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public RouteLogBusinessService(IRouteLogIntegrationService routeLogIntegrationService, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.routeLogIntegrationService = routeLogIntegrationService;
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(RouteLogConstants.XSLTFilePath).ToString() : string.Empty;
            this.loggingHelper = loggingHelper;
        }

        /// <summary>
        /// Method to retrieve sequenced delivery point details
        /// </summary>
        /// <param name="deliveryRoute">deliveryRoute</param>
        /// <returns>deliveryRoute</returns>
        public async Task<string> GenerateRouteLog(RouteDTO deliveryRoute)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.GenerateRouteLog"))
            {
                string methodName = typeof(RouteLogBusinessService) + "." + nameof(GenerateRouteLog);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string pdfFilename = string.Empty;
                var routeLogSummary = await routeLogIntegrationService.GenerateRouteLog(deliveryRoute);
                if (routeLogSummary != null)
                {
                    routeLogSummary.RouteLogSequencedPoints = GetRouteSummary(routeLogSummary.RouteLogSequencedPoints);
                    pdfFilename = await routeLogIntegrationService.GenerateRouteLogSummaryReport(RouteSummaryXMLSerialization(routeLogSummary), xsltFilepath);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return pdfFilename;
            }
        }

        /// <summary>
        /// Attempts to add the specified address to the group
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>True if the address could be added to the group, otherwise false</returns>
        private bool Add(RouteLogSequencedPointsDTO address, RouteSummaryGroup grp)
        {
            // Determine whether the address should be part of the current group
            //
            // Assume that the address is not part of the current group
            bool canAdd = false;

            // If the group type is unknown
            if (grp.CurrentGroupType == GroupType.Unknown)
            {
                // Identify the group type from the address
                grp.CurrentGroupType = IdentifyGroupType(address, grp);

                // The address can be added if the group type is known
                canAdd = grp.CurrentGroupType != GroupType.Unknown;
            }
            else
            {
                // The address can be added if it continues the sequence predicted by the group type
                canAdd = ContinuesGroup(address, grp);
            }

            // If the address can be added
            if (canAdd)
            {
                // Add the address to the group
                grp.AddressList.Add(address);

                // Update the last address and last building number
                grp.LastAddress = address;
                grp.LastBuildingNumber = address.BuildingNumber;

                // Update the delivery point count
                grp.DeliveryPointCount++;

                // Update the multiple occupancy count if the address has multiple occupancy
                grp.MultipleOccupancy = grp.MultipleOccupancy + address.MultipleOccupancy;
            }

            // Return whether the address can be added to the group
            return canAdd;
        }

        /// <summary>
        /// This method is used to generate Route log summary Pdf
        /// </summary>
        /// <param name="xml">xml as string</param>
        /// <returns>byte[]</returns>
        private byte[] GenerateRouteLogSummaryPdf(string xml)
        {
            MemoryStream stream = new MemoryStream();
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            FonetDriver driver = FonetDriver.Make();
            driver.Render(xmlDocument, stream);
            return stream.ToArray();
        }

        /// <summary>
        /// Serialze object to xml
        /// </summary>
        /// <typeparam name="T">Class </typeparam>
        /// <param name="type">object</param>
        /// <returns>xml as string</returns>
        /// <summary>
        /// Serialze object to xml
        /// </summary>
        /// <typeparam name="T">Class </typeparam>
        /// <param name="type">object</param>
        /// <returns>xml as string</returns>
        /// <summary>
        /// Serialze object to xml
        /// </summary>
        /// <typeparam name="T">Class </typeparam>
        /// <param name="type">object</param>
        /// <returns>xml as string</returns>
        private string RouteSummaryXMLSerialization(RouteLogSummaryDTO routeLogSummary)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement report = doc.CreateElement(RouteLogConstants.Report);
            XmlElement pageHeader = doc.CreateElement(RouteLogConstants.PageHeader);
            XmlElement pageFooter = doc.CreateElement(RouteLogConstants.PageFooter);
            XmlElement content = doc.CreateElement(RouteLogConstants.Content);
            XmlElement heading1 = doc.CreateElement(RouteLogConstants.Heading1);
            XmlElement heading2 = doc.CreateElement(RouteLogConstants.Heading2);
            XmlElement section = null;
            XmlElement sectionColumn = null;
            XmlElement table = null;
            XmlElement paragraph = null;
            Dictionary<string, string> data;

            report.SetAttribute(RouteLogConstants.PdfOutPut, RouteLogConstants.A4Portrait);
            pageHeader.SetAttribute(RouteLogConstants.Caption, string.Empty);
            pageFooter.SetAttribute(RouteLogConstants.Caption, string.Empty);
            pageFooter.SetAttribute(RouteLogConstants.PageNumber, "true");
            report.AppendChild(pageHeader);
            report.AppendChild(pageFooter);
            report.AppendChild(content);

            // Section 1 Header
            section = doc.CreateElement(RouteLogConstants.Section);

            // Section 1 Header 1
            sectionColumn = doc.CreateElement(RouteLogConstants.SectionColumn);
            sectionColumn.SetAttribute(RouteLogConstants.Width, "1");
            heading1.InnerText = RouteLogConstants.RouteSummaryHeader;
            sectionColumn.AppendChild(heading1);
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            // Section 2
            section = doc.CreateElement(RouteLogConstants.Section);

            // Section 2 columns 1 i.e Table 1
            sectionColumn = doc.CreateElement(RouteLogConstants.SectionColumn);
            sectionColumn.SetAttribute(RouteLogConstants.Width, "1");
            data = GetSectionColumnData(1, routeLogSummary);
            table = CreateTableWithFixedRowsColumns(2, data, doc, true);
            sectionColumn.AppendChild(table);
            section.AppendChild(sectionColumn);

            // Section 2 columns 2 i.e Table 2
            sectionColumn = doc.CreateElement(RouteLogConstants.SectionColumn);
            sectionColumn.SetAttribute(RouteLogConstants.Width, "1");
            data = GetSectionColumnData(2, routeLogSummary);
            table = CreateTableWithFixedRowsColumns(2, data, doc, true);
            sectionColumn.AppendChild(table);
            section.AppendChild(sectionColumn);

            // Section 2 columns 3 i.e Table 3
            sectionColumn = doc.CreateElement(RouteLogConstants.SectionColumn);
            paragraph = doc.CreateElement(RouteLogConstants.Paragraph);
            paragraph.InnerText = RouteLogConstants.RouteSummaryAlias;
            sectionColumn.SetAttribute(RouteLogConstants.Width, "1");
            data = GetSectionColumnData(3, routeLogSummary);
            table = CreateTableWithFixedRowsColumns(2, data, doc, true);
            sectionColumn.AppendChild(table);
            sectionColumn.AppendChild(paragraph);
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            // Section 3
            section = doc.CreateElement(RouteLogConstants.Section);

            // Section 1 Header 1
            sectionColumn = doc.CreateElement(RouteLogConstants.SectionColumn);
            sectionColumn.SetAttribute(RouteLogConstants.Width, "1");
            heading2.InnerText = RouteLogConstants.RouteSummarySequencedPoints;
            table = CreateTableWithDynamicRowsColumns(GetSectionColumnData(routeLogSummary.RouteLogSequencedPoints), doc);
            sectionColumn.AppendChild(heading2);
            sectionColumn.AppendChild(table);
            section.AppendChild(sectionColumn);

            content.AppendChild(section);
            doc.AppendChild(report);
            return doc.InnerXml;
        }

        /// <summary>
        /// This method is used to get section column data
        /// </summary>
        /// <param name="sectionNumber">sectionNumber as int</param>
        /// <param name="routeLogSummary">routeLogSummary as routeLogSummaryDTO</param>
        /// <returns>Dictionary of section column data</returns>
        private Dictionary<string, string> GetSectionColumnData(int sectionNumber, RouteLogSummaryDTO routeLogSummary)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (sectionNumber == 1)
            {
                data.Add(RouteLogConstants.RouteSummaryName, routeLogSummary.Route.RouteName);
                data.Add(RouteLogConstants.Number, routeLogSummary.Route.RouteNumber);
                data.Add(RouteLogConstants.RouteMethod, routeLogSummary.Route.Method);
                data.Add(RouteLogConstants.DeliveryOffice, routeLogSummary.Route.DeliveryOffice);
                data.Add(RouteLogConstants.Aliases, routeLogSummary.Route.Aliases.ToString());
                data.Add(RouteLogConstants.Block, routeLogSummary.Route.Blocks.ToString());
                data.Add(RouteLogConstants.Scenario, routeLogSummary.Route.ScenarioName);
            }
            else if (sectionNumber == 2)
            {
                data.Add(RouteLogConstants.CollectionPoint, "0");
                data.Add(RouteLogConstants.DeliveryPoint, routeLogSummary.Route.DPs.ToString());
                data.Add(RouteLogConstants.BusinessDeliveryPoint, routeLogSummary.Route.BusinessDPs.ToString());
                data.Add(RouteLogConstants.ResidentialDeliveryPoint, routeLogSummary.Route.ResidentialDPs.ToString());
                data.Add(RouteLogConstants.AccelerationIn, routeLogSummary.Route.AccelarationIn);
                data.Add(RouteLogConstants.AccelerationOut, routeLogSummary.Route.AccelarationOut);
                data.Add(RouteLogConstants.PairedRoute, routeLogSummary.Route.PairedRoute);
            }
            else
            {
                data.Add(RouteLogConstants.NoD2D, "0");
                data.Add(RouteLogConstants.DPExemptions, "0");
            }

            return data;
        }

        /// <summary>
        /// This method is used to get List of Columns for section.
        /// </summary>
        /// <param name="routeLogSequencedPoints">routeLogSequencedPoints as List</param>
        /// <returns>List of string</returns>
        private List<List<string>> GetSectionColumnData(List<RouteLogSequencedPointsDTO> routeLogSequencedPoints)
        {
            List<List<string>> data = new List<List<string>>();
            data.Add(new List<string> { RouteLogConstants.Street, RouteLogConstants.Number, RouteLogConstants.DeliveryPoint, RouteLogConstants.MultipleOccupancy, RouteLogConstants.SpecialInstructions, RouteLogConstants.AreaHazards });
            foreach (var routeLogSequencedPoint in routeLogSequencedPoints)
            {
                data.Add(new List<string>
                            {
                                routeLogSequencedPoint.StreetName, routeLogSequencedPoint.Description, routeLogSequencedPoint.DeliveryPointCount.ToString(),
                                routeLogSequencedPoint.MultipleOccupancy != null ? routeLogSequencedPoint.MultipleOccupancy.ToString() : "0", string.Empty, string.Empty
                            });
            }

            return data;
        }

        /// <summary>
        /// This method is used to create table with fixed rows and columns
        /// </summary>
        /// <param name="columnsCount">columnsCount as int</param>
        /// <param name="data">dictionary of string</param>
        /// <param name="doc">XmlDocument</param>
        /// <param name="setWidth">boolean</param>
        /// <returns>XmlElement</returns>
        private XmlElement CreateTableWithFixedRowsColumns(int columnsCount, Dictionary<string, string> data, XmlDocument doc, bool setWidth = false)
        {
            XmlElement table = doc.CreateElement(RouteLogConstants.Table);
            XmlElement columns = doc.CreateElement(RouteLogConstants.Columns);
            XmlElement column = null;
            XmlElement row = null;
            XmlElement cell = null;

            if (setWidth)
            {
                table.SetAttribute(RouteLogConstants.Width, "100%");
            }

            table.SetAttribute(RouteLogConstants.Borders, "false");
            table.SetAttribute(RouteLogConstants.UseShading, "true");

            for (int i = 0; i < columnsCount; i++)
            {
                column = doc.CreateElement(RouteLogConstants.Column);
                column.SetAttribute(RouteLogConstants.Width, "1");
                columns.AppendChild(column);
            }

            table.AppendChild(columns);

            for (int i = 0; i < data.Count; i++)
            {
                var item = data.ElementAt(i);
                row = doc.CreateElement(RouteLogConstants.Row);

                if (i % 2 != 0)
                {
                    row.SetAttribute(RouteLogConstants.Shade, "true");
                }

                cell = doc.CreateElement(RouteLogConstants.Cell);
                cell.InnerText = item.Key;
                row.AppendChild(cell);

                cell = doc.CreateElement(RouteLogConstants.Cell);
                cell.InnerText = item.Value;
                row.AppendChild(cell);
                table.AppendChild(row);
            }

            return table;
        }

        /// <summary>
        /// This method is used to create table with dynamic rows for pdf.
        /// </summary>
        /// <param name="data">List of string</param>
        /// <param name="doc">Xml document</param>
        /// <returns>XmlElement</returns>
        private XmlElement CreateTableWithDynamicRowsColumns(List<List<string>> data, XmlDocument doc)
        {
            XmlElement table = doc.CreateElement(RouteLogConstants.Table);
            XmlElement columns = doc.CreateElement(RouteLogConstants.Columns);
            XmlElement column = null;
            XmlElement row = null;
            XmlElement cell = null;
            var columnsCount = data[0].Count;

            table.SetAttribute(RouteLogConstants.UseShading, "true");

            for (int i = 0; i < columnsCount; i++)
            {
                column = doc.CreateElement(RouteLogConstants.Column);
                column.SetAttribute(RouteLogConstants.Width, "1");
                columns.AppendChild(column);
            }

            table.AppendChild(columns);

            for (int i = 0; i < data.Count; i++)
            {
                var item = data.ElementAt(i);

                if (i == 0)
                {
                    row = doc.CreateElement(RouteLogConstants.Header);
                }
                else
                {
                    row = doc.CreateElement(RouteLogConstants.Row);
                }

                if (i != 0 && i % 2 == 0)
                {
                    row.SetAttribute(RouteLogConstants.Shade, "true");
                }

                for (int j = 0; j < item.Count; j++)
                {
                    cell = doc.CreateElement(RouteLogConstants.Cell);
                    if (i != 0)
                    {
                        cell.SetAttribute("align", "center");
                    }

                    cell.InnerText = item[j];
                    row.AppendChild(cell);
                }

                table.AppendChild(row);
            }

            return table;
        }

        /// <summary>
        /// This method is used to get route summary.
        /// </summary>
        /// <param name="addressList">addressList as List of route sequenced Points</param>
        /// <returns>routeSummary as List<RouteLogSequencedPointsDTO></returns>
        private List<RouteLogSequencedPointsDTO> GetRouteSummary(List<RouteLogSequencedPointsDTO> addressList)
        {
            // Initialize the route summary
            List<RouteLogSequencedPointsDTO> routeSummary = new List<RouteLogSequencedPointsDTO>();
            if (addressList != null && addressList.Count > 0)
            {
                // Route summary group
                // The route summary group keeps track of the current group while the route summary
                // is being generated
                RouteSummaryGroup group = null;

                // Step through the addresses
                foreach (RouteLogSequencedPointsDTO address in addressList)
                {
                    // If the route summary group is not set
                    if (group == null)
                    {
                        // The current address is the start of a new route summary group
                        //
                        // Initialize a new route summary group from the current address
                        group = new RouteSummaryGroup(address);
                    }
                    else
                    {
                        // If the current address can be added to the current route summary group
                        if (Add(address, group))
                        {
                            // The address belongs to the current route summary group
                            //
                            // Do nothing
                        }
                        else
                        {
                            // The address belongs to a new route summary group
                            //
                            // Create a summary row from the current route summary group
                            RouteLogSequencedPointsDTO row = new RouteLogSequencedPointsDTO(group.StreetName, group.Description, group.DeliveryPointCount, group.MultipleOccupancy, group.SubBuildingName, group.BuildingName);
                            routeSummary.Add(row);

                            // Initialize a new route summary group from the current address
                            group = new RouteSummaryGroup(address);
                        }
                    }
                }

                // If the route summary group is set
                if (group != null)
                {
                    // Process the final route summary group
                    //
                    // Create a summary row from the current route summary group
                    RouteLogSequencedPointsDTO row = new RouteLogSequencedPointsDTO(group.StreetName, group.Description, group.DeliveryPointCount, group.MultipleOccupancy, group.SubBuildingName, group.BuildingName);
                    routeSummary.Add(row);
                }
            }

            // Return the route summary
            return routeSummary;
        }

        /// <summary>
        /// Determines whether the specified address continues the current group
        /// To continue the current group the address must:
        ///   Have the same thoroughfare
        /// AND
        ///   Continue the sequence predicted by the current group type
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>True if the address continues the current group, otherwise false</returns>
        private bool ContinuesGroup(RouteLogSequencedPointsDTO address, RouteSummaryGroup grp)
        {
            // Assume that the address does not continue the current group
            bool doesContinueGroup = false;

            // If the thoroughfare is the same as in the last address
            if (address.StreetName == grp.LastAddress.StreetName)
            {
                // Determine whether the group type for the address is the same as the current group type
                GroupType groupTypeForAddress = IdentifyGroupType(address, grp);
                if (groupTypeForAddress == grp.CurrentGroupType)
                {
                    doesContinueGroup = true;
                }
            }

            // Return whether the address continues the current group
            return doesContinueGroup;
        }

        /// <summary>
        /// Gets the group type description for a specified group type
        /// </summary>
        /// <param name="groupType"></param>
        /// <returns>The group type description</returns>
        private string GetGroupTypeDescription(GroupType groupType)
        {
            // Get the description for the group type
            string description = string.Empty;
            switch (groupType)
            {
                case GroupType.EvensAscending:
                    description = "Evens Ascending";
                    break;

                case GroupType.EvensDescending:
                    description = "Evens Descending";
                    break;

                case GroupType.OddsAscending:
                    description = "Odds Ascending";
                    break;

                case GroupType.OddsDescending:
                    description = "Odds Descending";
                    break;

                case GroupType.SequentialAscending:
                    description = "Sequential Ascending";
                    break;

                case GroupType.SequentialDescending:
                    description = "Sequential Descending";
                    break;

                case GroupType.Unknown:
                    description = "Unknown";
                    break;
            }

            // Return the description
            return description;
        }

        /// <summary>
        /// Gets the increment (absolute difference) between two numbers
        /// </summary>
        /// <param name="number1">Number 1</param>
        /// <param name="number2">Number 2</param>
        /// <returns>The increment</returns>
        private short GetIncrement(short number1, short number2)
        {
            // The increment is the absolute difference between the two numbers
            return System.Math.Abs((short)(number1 - number2));
        }

        /// <summary>
        /// Determines whether a specified address has a standard building number
        /// A standard building number is a simple non-zero, positive integer building number
        ///   without a building name or sub name
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>True if the address has a standard building number, otherwise false</returns>
        private bool HasStandardBuildingNumber(RouteLogSequencedPointsDTO address)
        {
            // The building number is standard if it is a positive non-zero value
            bool isStandard = false;
            if (address.BuildingNumber.HasValue)
            {
                if (address.BuildingNumber.Value > 0)
                {
                    isStandard = true;
                }
            }

            // If the building number is standard
            if (isStandard)
            {
                // If the building also has a name or a sub name
                if (!string.IsNullOrWhiteSpace(address.SubBuildingName) || !string.IsNullOrWhiteSpace(address.BuildingName))
                {
                    // The building number is not standard
                    isStandard = false;
                }
            }

            // Return whether the address has a standard building number
            return isStandard;
        }

        /// <summary>
        /// Identifies the group type using the address and the last address
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>The group type</returns>
        private GroupType IdentifyGroupType(RouteLogSequencedPointsDTO address, RouteSummaryGroup grp)
        {
            // Assume that the group type is unknown
            GroupType groupType = GroupType.Unknown;

            // Determine the group type
            //
            // If the last address and address has a standard building number
            if (HasStandardBuildingNumber(grp.LastAddress) && HasStandardBuildingNumber(address))
            {
                // Determine the group type from the building numbers
                groupType = IdentifyGroupType(grp.LastAddress.BuildingNumber.Value, address.BuildingNumber.Value);
            }
            else
            {
                // Only addresses with standard building numbers can be matched to a group type
                groupType = GroupType.Unknown;
            }

            // Return the group type
            return groupType;
        }

        /// <summary>
        /// Identifies the group type for two building numbers
        /// </summary>
        /// <param name="buildingNumber1">Building number 1</param>
        /// <param name="buildingNumber2">Building number 2</param>
        /// <returns>The group type</returns>
        private GroupType IdentifyGroupType(short buildingNumber1, short buildingNumber2)
        {
            // Assume that the group type is unknown
            GroupType groupType = GroupType.Unknown;

            // Get the metadata for the building numbers
            bool isAscending = buildingNumber1 < buildingNumber2;
            short increment = GetIncrement(buildingNumber1, buildingNumber2);
            bool areEven = IsEven(buildingNumber1) && IsEven(buildingNumber2);
            bool areOdd = !IsEven(buildingNumber1) && !IsEven(buildingNumber2);

            // Identify the group type based on the building number metadata
            //
            // If the building numbers are in ascending order
            if (isAscending)
            {
                // If the building numbers are sequential integers
                if (increment == 1)
                {
                    groupType = GroupType.SequentialAscending;
                }
                else
                {
                    // If the building numbers are both even and sequential
                    if (areEven && increment == 2)
                    {
                        groupType = GroupType.EvensAscending;
                    }

                    // If the building numbers are both odd and sequential
                    if (areOdd && increment == 2)
                    {
                        groupType = GroupType.OddsAscending;
                    }
                }
            }
            else
            {
                // The numbers are in descending order
                //
                // If the building numbers are sequential integers
                if (increment == 1)
                {
                    groupType = GroupType.SequentialDescending;
                }
                else
                {
                    // If the building numbers are both even and sequential
                    if (areEven && increment == 2)
                    {
                        groupType = GroupType.EvensDescending;
                    }

                    // If the building numbers are both odd and sequential
                    if (areOdd && increment == 2)
                    {
                        groupType = GroupType.OddsDescending;
                    }
                }
            }

            // Return the group type
            return groupType;
        }

        /// <summary>
        /// Determines whether the specified number is even
        /// </summary>
        /// <param name="number">The number</param>
        /// <returns>True if the number is even, otherwise false</returns>
        private bool IsEven(short number)
        {
            // The number is even if remainder when dividing by two is zero
            return number % 2 == 0;
        }
    }
}