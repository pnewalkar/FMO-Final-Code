using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Operational.RouteLog.WebAPI.DTO;
using RM.Operational.RouteLog.WebAPI.IntegrationService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System;

namespace RM.Operational.RouteLog.WebAPI.BusinessService
{
    /// <summary>
    /// Route Log Business Service
    /// </summary>
    public class RouteLogBusinessService : IRouteLogBusinessService
    {
        /// <summary>
        /// Reference to the Route Log integration service
        /// </summary>
        private IRouteLogIntegrationService routeLogIntegrationService;



        /// <summary>
        /// Reference to the logging helper
        /// </summary>
        private ILoggingHelper loggingHelper = default(ILoggingHelper);





        /// <summary>
        /// Initializes a new instance of the <see cref="RouteLogBusinessService" /> class
        /// </summary>
        /// <param name="routeLogIntegrationService">Route log integration service</param>
        /// <param name="loggingHelper">Logging helper</param>
        public RouteLogBusinessService(IRouteLogIntegrationService routeLogIntegrationService, ILoggingHelper loggingHelper)
        {
            // Validate the arguments
            if (routeLogIntegrationService == null) throw new ArgumentNullException(nameof(routeLogIntegrationService));
            if (loggingHelper == null) throw new ArgumentNullException(nameof(loggingHelper));


            // Store the injected dependencies
            this.routeLogIntegrationService = routeLogIntegrationService;
            this.loggingHelper = loggingHelper;
        }



        /// <summary>
        /// Generates a route log summary report for the specified delivery route and returns the file name
        ///   of the generated PDF document
        /// </summary>
        /// <param name="deliveryRoute">The delivery route</param>
        /// <returns>The PDF document file name</returns>
        public async Task<string> GenerateRouteLog(RouteDTO deliveryRoute)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(GenerateRouteLog)}"))
            {
                string methodName = typeof(RouteLogBusinessService) + "." + nameof(GenerateRouteLog);
                loggingHelper.LogMethodEntry(methodName, LoggerTraceConstants.RouteLogAPIPriority, LoggerTraceConstants.RouteLogBusinessServiceMethodEntryEventId);


                // Initialize the PDF document file name
                string pdfFileName = string.Empty;


                // Retrieve the route log summary for the delivery route
                var routeLogSummary = await routeLogIntegrationService.GetRouteLog(deliveryRoute);
                if (routeLogSummary != null)
                {
                    // Build the left hand table content
                    string RouteSummaryName = "Name"; // TODO load from resource file
                    string Number = "Number"; // TODO load from resource file
                    string RouteMethod = "Method"; // TODO load from resource file
                    string DeliveryOffice = "Delivery Office"; // TODO load from resource file
                    string Aliases = "Aliases*"; // TODO load from resource file
                    string Block = "Blocks"; // TODO load from resource file
                    string Scenario = "Scenario"; // TODO load from resource file
                    List<Tuple<string, string>> leftTable = new List<Tuple<string, string>>();
                    leftTable.Add(Tuple.Create<string, string>(RouteSummaryName, routeLogSummary.Route.RouteName));
                    leftTable.Add(Tuple.Create<string, string>(Number, routeLogSummary.Route.RouteNumber));
                    leftTable.Add(Tuple.Create<string, string>(RouteMethod, routeLogSummary.Route.Method));
                    leftTable.Add(Tuple.Create<string, string>(DeliveryOffice, routeLogSummary.Route.DeliveryOffice));
                    leftTable.Add(Tuple.Create<string, string>(Aliases, routeLogSummary.Route.Aliases.ToString()));
                    leftTable.Add(Tuple.Create<string, string>(Block, routeLogSummary.Route.Blocks.ToString()));
                    leftTable.Add(Tuple.Create<string, string>(Scenario, routeLogSummary.Route.ScenarioName));

                    // Build the centre hand table content
                    string CollectionPoint = "CPs"; // TODO load from resource file
                    string DeliveryPoint = "DPs"; // TODO load from resource file
                    string BusinessDeliveryPoint = "Business DPs"; // TODO load from resource file
                    string ResidentialDeliveryPoint = "Residential DPs"; // TODO load from resource file
                    string AccelerationIn = "Acceleration In"; // TODO load from resource file
                    string AccelerationOut = "Acceleration Out"; // TODO load from resource file
                    string PairedRoute = "Paired Route"; // TODO load from resource file
                    List<Tuple<string, string>> centerTable = new List<Tuple<string, string>>();
                    centerTable.Add(Tuple.Create<string, string>(CollectionPoint, "0")); // TODO add collection point value here
                    centerTable.Add(Tuple.Create<string, string>(DeliveryPoint, routeLogSummary.Route.DPs.ToString()));
                    centerTable.Add(Tuple.Create<string, string>(BusinessDeliveryPoint, routeLogSummary.Route.BusinessDPs.ToString()));
                    centerTable.Add(Tuple.Create<string, string>(ResidentialDeliveryPoint, routeLogSummary.Route.ResidentialDPs.ToString()));
                    centerTable.Add(Tuple.Create<string, string>(AccelerationIn, routeLogSummary.Route.AccelarationIn));
                    centerTable.Add(Tuple.Create<string, string>(AccelerationOut, routeLogSummary.Route.AccelarationOut));
                    centerTable.Add(Tuple.Create<string, string>(PairedRoute, routeLogSummary.Route.PairedRoute));

                    // Build the right hand table content
                    string NoD2D = "No D2D"; // TODO load from resource file
                    string DPExemptions = "DP Exemptions"; // TODO load from resource file
                    List<Tuple<string, string>> rightTable = new List<Tuple<string, string>>();
                    rightTable.Add(Tuple.Create<string, string>(NoD2D, "0")); // TODO Add no D2D value here
                    rightTable.Add(Tuple.Create<string, string>(DPExemptions, "0")); // TODO Add DP exemptions value here

                    // Get the sequenced points
                    List<RouteLogSummaryPoint> sequencedPoints = RouteLogSummaryFactory.GetSequencedPointSummary(routeLogSummary.RouteLogSequencedPoints);

                    // Get the unsequenced points
                    List<RouteLogSummaryPoint> unsequencedPoints = new List<RouteLogSummaryPoint>(); // TODO populate unsequenced points here


                    // Construct the route log summary report
                    RouteLogSummaryConfiguration config = new RouteLogSummaryConfiguration();
                    XmlDocument report = RouteLogSummaryReportFactory.GetRouteLogSummary(leftTable, centerTable, rightTable, sequencedPoints, unsequencedPoints, config);


                    // Generate the route log summary report PDF file
                    pdfFileName = await routeLogIntegrationService.GeneratePdfDocument(report.InnerXml);
                }

                loggingHelper.LogMethodExit(methodName, LoggerTraceConstants.RouteLogAPIPriority, LoggerTraceConstants.RouteLogBusinessServiceMethodExitEventId);
                return pdfFileName;
            }
        }
    }
}