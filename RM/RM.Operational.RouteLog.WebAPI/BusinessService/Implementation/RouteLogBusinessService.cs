using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Fonet;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.Model;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Utilities.Enums;
using RM.Operational.RouteLog.WebAPI.IntegrationService;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    public class RouteLogBusinessService : IRouteLogBusinessService
    {
        private const string XSLTFilePath = "XSLTFilePath";
        private const string Report = "report";

        private string xsltFilepath = string.Empty;
        private IRouteLogIntegrationService routeLogIntegrationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryRouteBusinessService" /> class and other classes.
        /// </summary>
        /// <param name="deliveryRouteDataService">IDeliveryRouteRepository reference</param>
        /// <param name="scenarioDataService">IScenarioRepository reference</param>
        /// <param name="referenceDataBusinessService">The reference data business service.</param>
        public RouteLogBusinessService(IRouteLogIntegrationService routeLogIntegrationService, IConfigurationHelper configurationHelper)
        {
            this.routeLogIntegrationService = routeLogIntegrationService;
            this.xsltFilepath = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(XSLTFilePath).ToString() : string.Empty;
        }

        /// <summary>
        /// Method to retrieve sequenced delivery point details
        /// </summary>
        /// <param name="deliveryRouteDto">deliveryRouteDto</param>
        /// <returns>deliveryRouteDto</returns>
        public async Task<string> GenerateRouteLog(DeliveryRouteDTO deliveryRouteDto)
        {
            string pdfFilename = string.Empty;
            var routeLogSummaryModelDTO = await routeLogIntegrationService.GenerateRouteLog(deliveryRouteDto);
            if (routeLogSummaryModelDTO != null)
            {
                routeLogSummaryModelDTO.RouteLogSequencedPoints = GetRouteSummary(routeLogSummaryModelDTO.RouteLogSequencedPoints);
                pdfFilename = await routeLogIntegrationService.GenerateRouteLogSummaryReport(RouteSummaryXMLSerialization(routeLogSummaryModelDTO), xsltFilepath);
            }

            return pdfFilename;
        }

        /// <summary>
        /// Attempts to add the specified address to the group
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>True if the address could be added to the group, otherwise false</returns>
        private bool Add(RouteLogSequencedPointsDTO address, RouteSummaryGroupDTO grp)
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
        private string RouteSummaryXMLSerialization(RouteLogSummaryModelDTO routeLogSummaryModelDTO)
        {
            XmlDocument doc = new XmlDocument();
            XmlElement report = doc.CreateElement(Report);
            XmlElement pageHeader = doc.CreateElement(Constants.PageHeader);
            XmlElement pageFooter = doc.CreateElement(Constants.PageFooter);
            XmlElement content = doc.CreateElement(Constants.Content);
            XmlElement heading1 = doc.CreateElement(Constants.Heading1);
            XmlElement heading2 = doc.CreateElement(Constants.Heading2);
            XmlElement section = null;
            XmlElement sectionColumn = null;
            XmlElement table = null;
            XmlElement paragraph = null;
            Dictionary<string, string> data;

            report.SetAttribute(Constants.PdfOutPut, Constants.A4Portrait);
            pageHeader.SetAttribute(Constants.Caption, string.Empty);
            pageFooter.SetAttribute(Constants.Caption, string.Empty);
            pageFooter.SetAttribute(Constants.PageNumber, "true");
            report.AppendChild(pageHeader);
            report.AppendChild(pageFooter);
            report.AppendChild(content);

            //Section 1 Header
            section = doc.CreateElement(Constants.Section);

            //Section 1 Header 1
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            heading1.InnerText = Constants.RouteSummaryHeader;
            sectionColumn.AppendChild(heading1);
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            //Section 2
            section = doc.CreateElement(Constants.Section);

            //Section 2 columns 1 i.e Table 1
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            data = GetSectionColumnData(1, routeLogSummaryModelDTO);
            table = CreateTableWithFixedRowsColumns(2, data, doc, true);
            sectionColumn.AppendChild(table);
            section.AppendChild(sectionColumn);

            //Section 2 columns 2 i.e Table 2
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            data = GetSectionColumnData(2, routeLogSummaryModelDTO);
            table = CreateTableWithFixedRowsColumns(2, data, doc);
            sectionColumn.AppendChild(table);
            section.AppendChild(sectionColumn);

            //Section 2 columns 3 i.e Table 3
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            paragraph = doc.CreateElement(Constants.Paragraph);
            paragraph.InnerText = Constants.RouteSummaryAlias;
            sectionColumn.SetAttribute(Constants.Width, "1");
            data = GetSectionColumnData(3, routeLogSummaryModelDTO);
            table = CreateTableWithFixedRowsColumns(2, data, doc);
            sectionColumn.AppendChild(table);
            sectionColumn.AppendChild(paragraph);
            section.AppendChild(sectionColumn);
            content.AppendChild(section);

            //Section 3
            section = doc.CreateElement(Constants.Section);

            //Section 1 Header 1
            sectionColumn = doc.CreateElement(Constants.SectionColumn);
            sectionColumn.SetAttribute(Constants.Width, "1");
            heading2.InnerText = Constants.RouteSummarySequencedPoints;
            table = CreateTableWithDynamicRowsColumns(GetSectionColumnData(routeLogSummaryModelDTO.RouteLogSequencedPoints), doc);
            sectionColumn.AppendChild(heading2);
            sectionColumn.AppendChild(table);
            section.AppendChild(sectionColumn);

            content.AppendChild(section);
            doc.AppendChild(report);
            return doc.InnerXml; ;
        }

        private Dictionary<string, string> GetSectionColumnData(int sectionNumber, RouteLogSummaryModelDTO routeLogSummaryModelDTO)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            if (sectionNumber == 1)
            {
                data.Add(Constants.RouteSummaryName, routeLogSummaryModelDTO.DeliveryRoute.RouteName);
                data.Add(Constants.Number, routeLogSummaryModelDTO.DeliveryRoute.RouteNumber);
                data.Add(Constants.RouteMethod, routeLogSummaryModelDTO.DeliveryRoute.Method);
                data.Add(Constants.DeliveryOffice, routeLogSummaryModelDTO.DeliveryRoute.DeliveryOffice);
                data.Add(Constants.Aliases, routeLogSummaryModelDTO.DeliveryRoute.Aliases.ToString());
                data.Add(Constants.Block, routeLogSummaryModelDTO.DeliveryRoute.Blocks.ToString());
                data.Add(Constants.Scenario, routeLogSummaryModelDTO.DeliveryRoute.ScenarioName);
            }
            else if (sectionNumber == 2)
            {
                data.Add(Constants.CollectionPoint, "0");
                data.Add(Constants.DeliveryPoint, routeLogSummaryModelDTO.DeliveryRoute.DPs.ToString());
                data.Add(Constants.BusinessDeliveryPoint, routeLogSummaryModelDTO.DeliveryRoute.BusinessDPs.ToString());
                data.Add(Constants.ResidentialDeliveryPoint, routeLogSummaryModelDTO.DeliveryRoute.ResidentialDPs.ToString());
                data.Add(Constants.AccelerationIn, routeLogSummaryModelDTO.DeliveryRoute.AccelarationIn);
                data.Add(Constants.AccelerationOut, routeLogSummaryModelDTO.DeliveryRoute.AccelarationOut);
                data.Add(Constants.PairedRoute, routeLogSummaryModelDTO.DeliveryRoute.PairedRoute);
            }
            else
            {
                data.Add(Constants.NoD2D, "0");
                data.Add(Constants.DPExemptions, "0");
            }
            return data;
        }

        private List<List<string>> GetSectionColumnData(List<RouteLogSequencedPointsDTO> routeLogSequencedPointsDTOs)
        {
            List<List<string>> data = new List<List<string>>();
            data.Add(new List<string> { Constants.Street, Constants.Number, Constants.DeliveryPoint, Constants.MultipleOccupancy, Constants.SpecialInstructions, Constants.AreaHazards });
            foreach (var routeLogSequencedPointsDTO in routeLogSequencedPointsDTOs)
            {
                data.Add(new List<string> { routeLogSequencedPointsDTO.StreetName, routeLogSequencedPointsDTO.Description,routeLogSequencedPointsDTO.DeliveryPointCount.ToString(),
                    routeLogSequencedPointsDTO.MultipleOccupancy!=null ?routeLogSequencedPointsDTO.MultipleOccupancy.ToString(): "0", string.Empty,string.Empty});
            }
            return data;
        }

        private XmlElement CreateTableWithFixedRowsColumns(int columnsCount, Dictionary<string, string> data, XmlDocument doc, bool setWidth = false)
        {
            XmlElement table = doc.CreateElement(Constants.Table);
            XmlElement columns = doc.CreateElement(Constants.Columns);
            XmlElement column = null;
            XmlElement row = null;
            XmlElement cell = null;

            if (setWidth)
            {
                table.SetAttribute(Constants.Width, "100%");
            }
            table.SetAttribute(Constants.Borders, "false");
            table.SetAttribute(Constants.UseShading, "true");

            for (int i = 0; i < columnsCount; i++)
            {
                column = doc.CreateElement(Constants.Columns);
                column.SetAttribute(Constants.Width, "1");
                columns.AppendChild(column);
            }

            table.AppendChild(columns);

            for (int i = 0; i < data.Count; i++)
            {
                var item = data.ElementAt(i);
                row = doc.CreateElement(Constants.Row);

                if (i % 2 == 0)
                {
                    row.SetAttribute(Constants.Shade, "true");
                }

                cell = doc.CreateElement(Constants.Cell);
                cell.InnerText = item.Key;
                row.AppendChild(cell);

                cell = doc.CreateElement(Constants.Cell);
                cell.InnerText = item.Value;
                row.AppendChild(cell);
                table.AppendChild(row);
            }

            return table;
        }

        private XmlElement CreateTableWithDynamicRowsColumns(List<List<string>> data, XmlDocument doc)
        {
            XmlElement table = doc.CreateElement(Constants.Table);
            XmlElement columns = doc.CreateElement(Constants.Columns);
            XmlElement column = null;
            XmlElement row = null;
            XmlElement cell = null;
            var columnsCount = data[0].Count;

            table.SetAttribute(Constants.UseShading, "true");

            for (int i = 0; i < columnsCount; i++)
            {
                column = doc.CreateElement(Constants.Column);
                column.SetAttribute(Constants.Width, "1");
                columns.AppendChild(column);
            }
            table.AppendChild(columns);

            for (int i = 0; i < data.Count; i++)
            {
                var item = data.ElementAt(i);

                if (i == 0)
                {
                    row = doc.CreateElement(Constants.Header);
                }
                else
                {
                    row = doc.CreateElement(Constants.Row);
                }
                if (i != 0 && i % 2 == 0)
                {
                    row.SetAttribute(Constants.Shade, "true");
                }

                for (int j = 0; j < item.Count; j++)
                {
                    cell = doc.CreateElement(Constants.Cell);
                    if (i != 0)
                        cell.SetAttribute("align", "center");
                    cell.InnerText = item[j];
                    row.AppendChild(cell);
                }

                table.AppendChild(row);
            }

            return table;
        }

        private List<RouteLogSequencedPointsDTO> GetRouteSummary(List<RouteLogSequencedPointsDTO> addressList)
        {
            // Initialize the route summary
            List<RouteLogSequencedPointsDTO> routeSummary = new List<RouteLogSequencedPointsDTO>();
            if (addressList != null && addressList.Count > 0)
            {
                // Route summary group
                // The route summary group keeps track of the current group while the route summary
                // is being generated
                RouteSummaryGroupDTO group = null;

                // Step through the addresses
                foreach (RouteLogSequencedPointsDTO address in addressList)
                {
                    // If the route summary group is not set
                    if (group == null)
                    {
                        // The current address is the start of a new route summary group
                        //
                        // Initialize a new route summary group from the current address
                        group = new RouteSummaryGroupDTO(address);
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
                            RouteLogSequencedPointsDTO row = new RouteLogSequencedPointsDTO(group.StreetName, group.Description, group.DeliveryPointCount, group.MultipleOccupancy);
                            routeSummary.Add(row);

                            // Initialize a new route summary group from the current address
                            group = new RouteSummaryGroupDTO(address);
                        }
                    }
                }

                // If the route summary group is set
                if (group != null)
                {
                    // Process the final route summary group
                    //
                    // Create a summary row from the current route summary group
                    RouteLogSequencedPointsDTO row = new RouteLogSequencedPointsDTO(group.StreetName, group.Description, group.DeliveryPointCount, group.MultipleOccupancy);
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
        private bool ContinuesGroup(RouteLogSequencedPointsDTO address, RouteSummaryGroupDTO grp)
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
        private GroupType IdentifyGroupType(RouteLogSequencedPointsDTO address, RouteSummaryGroupDTO grp)
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