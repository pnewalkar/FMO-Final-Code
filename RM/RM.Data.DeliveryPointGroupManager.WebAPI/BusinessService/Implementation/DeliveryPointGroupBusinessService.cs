using System;
using System.Threading.Tasks;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.DeliveryPointGroupManager.WebAPI.DataDTO;
using RM.Data.DeliveryPointGroupManager.WebAPI.DTO;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.DataService;
using RM.DataManagement.DeliveryPointGroupManager.WebAPI.Integration;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.BusinessService
{
    public class DeliveryPointGroupBusinessService : IDeliveryPointGroupBusinessService
    {
        #region Member Variables

        private const string Comma = ", ";
        private const string Polygon = "POLYGON(({0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}))";

        private IDeliveryPointGroupDataService deliveryPointGroupDataService = default(IDeliveryPointGroupDataService);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IDeliveryPointGroupIntegrationService deliveryPointGroupIntegrationService = default(IDeliveryPointGroupIntegrationService);

        private int priority = LoggerTraceConstants.DeliveryPointGroupManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.DeliveryPointGroupBusinessServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.DeliveryPointGroupBusinessServiceMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DeliveryPointGroupBusinessService"/> class.
        /// </summary>
        public DeliveryPointGroupBusinessService(
            IDeliveryPointGroupDataService deliveryPointGroupDataService,
            ILoggingHelper loggingHelper,
            IDeliveryPointGroupIntegrationService deliveryPointGroupIntegrationService)
        {
            this.deliveryPointGroupDataService = deliveryPointGroupDataService;
            this.loggingHelper = loggingHelper;
            this.deliveryPointGroupIntegrationService = deliveryPointGroupIntegrationService;
        }

        #endregion Constructors

        public string GetDeliveryPointGroups(string boundaryBox, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(GetDeliveryPointGroups)}"))
            {
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(GetDeliveryPointGroups);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                string deliveryPointGroupJsonData = null;

                if (!string.IsNullOrEmpty(boundaryBox))
                {
                    var deliveryGroupCoordinates = GetGroupCoordinatesDataByBoundingBox(boundaryBox.Split(Comma[0]));
                    //var accessLinkDataDto = deliveryPointGroupDataService.GetDeliveryPointGroups(deliveryGroupCoordinates, unitGuid);
                    //var accessLink = GenericMapper.MapList<DeliveryPointGroupDataDTO, DeliveryPointGroupDTO>(accessLinkDataDto);
                    // deliveryPointGroupJsonData = GetAccessLinkJsonData(accessLinkDataDto);
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return deliveryPointGroupJsonData;
            }
        }

        public DeliveryPointGroupDTO UpdateDeliveryGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace($"Business.{nameof(UpdateDeliveryGroup)}"))
            {
            }

            return deliveryPointGroupDto;
        }

        private static string GetGroupCoordinatesDataByBoundingBox(params object[] deliveryGroupParameters)
        {
            string coordinates = string.Empty;

            if (deliveryGroupParameters != null && deliveryGroupParameters.Length == 4)
            {
                coordinates = string.Format(
                              Polygon,
                              Convert.ToString(deliveryGroupParameters[0]),
                              Convert.ToString(deliveryGroupParameters[1]),
                              Convert.ToString(deliveryGroupParameters[0]),
                              Convert.ToString(deliveryGroupParameters[3]),
                              Convert.ToString(deliveryGroupParameters[2]),
                              Convert.ToString(deliveryGroupParameters[3]),
                              Convert.ToString(deliveryGroupParameters[2]),
                              Convert.ToString(deliveryGroupParameters[1]),
                              Convert.ToString(deliveryGroupParameters[0]),
                              Convert.ToString(deliveryGroupParameters[1]));
            }

            return coordinates;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deliveryPointGroupDto">UI Dto to create Delivery group</param>
        /// <returns></returns>
        public Task<DeliveryPointGroupDTO> CreateDeliveryPointGroup(DeliveryPointGroupDTO deliveryPointGroupDto)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Business.CreateDeliveryPointGroup"))
            {
                string methodName = typeof(DeliveryPointGroupBusinessService) + "." + nameof(CreateDeliveryPointGroup);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                if (deliveryPointGroupDto == null)
                {
                    throw new ArgumentNullException(nameof(deliveryPointGroupDto), string.Format(ErrorConstants.Err_ArgumentmentNullException, deliveryPointGroupDto));
                }

                deliveryPointGroupDataService.CreateDeliveryGroup();
                throw new NotImplementedException();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return null;
            }            
        }
    }
}