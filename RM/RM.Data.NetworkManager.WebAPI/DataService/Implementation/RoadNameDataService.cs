using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.NetworkManager.WebAPI.Entities;
using RM.DataManagement.NetworkManager.WebAPI.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.DataManagement.NetworkManager.WebAPI.DataDTO;

namespace RM.DataManagement.NetworkManager.WebAPI.DataService.Implementation
{
    /// <summary>
    /// This class contains methods fetching for Road Links data.
    /// </summary>
    public class RoadNameDataService : DataServiceBase<RoadName, NetworkDBContext>, IRoadNameDataService
    {
        #region Member Variables

        private const int BNGCOORDINATESYSTEM = 27700;
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        private int priority = LoggerTraceConstants.NetworkManagerAPIPriority;
        private int entryEventId = LoggerTraceConstants.RoadNameDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.RoadNameDataServiceMethodExitEventId;

        #endregion Member Variables

        #region Constructors

        public RoadNameDataService(IDatabaseFactory<NetworkDBContext> databaseFactory, ILoggingHelper loggingHelper)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// This method is used to fetch Road routes data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="locationID">location unique identifier.</param>
        /// <returns>List of NetworkLinkDTO</returns>
        public List<NetworkLinkDataDTO> GetRoadRoutes(string boundingBoxCoordinates, Guid locationID, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetRoadRoutes"))
            {
                string methodName = typeof(RoadNameDataService) + "." + nameof(GetRoadRoutes);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                List<NetworkLink> result = GetRoadNameCoordinatesDatabyBoundingbox(boundingBoxCoordinates, locationID, referenceDataCategoryList).ToList();
                var networkLink = GenericMapper.MapList<NetworkLink, NetworkLinkDataDTO>(result);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return networkLink;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// This method is used to get Road Link data as per coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="locationID">Location unique identifier.</param>
        /// <returns>List of Road Link entity</returns>
        private IEnumerable<NetworkLink> GetRoadNameCoordinatesDatabyBoundingbox(string boundingBoxCoordinates, Guid locationID, List<ReferenceDataCategoryDTO> referenceDataCategoryList)
        {
            try
            {
                IEnumerable<NetworkLink> networkLink = null;

                if (!string.IsNullOrEmpty(boundingBoxCoordinates))
                {
                    DbGeometry polygon = DataContext.Locations.AsNoTracking().Where(x => x.ID == locationID).Select(x => x.Shape).SingleOrDefault();

                    var roadLinkTypeId = referenceDataCategoryList.Where(x => x.CategoryName.Replace(" ", string.Empty) == ReferenceDataCategoryNames.NetworkLinkType)
                                                                        .SelectMany(x => x.ReferenceDatas).Select(x => x.ID).SingleOrDefault();

                    DbGeometry extent = DbGeometry.FromText(boundingBoxCoordinates, BNGCOORDINATESYSTEM);

                    networkLink = DataContext.NetworkLinks.AsNoTracking().Where(x => (x.LinkGeometry != null && x.LinkGeometry.Intersects(extent) && x.LinkGeometry.Intersects(polygon)) && x.NetworkLinkTypeGUID == roadLinkTypeId).ToList();
                }

                return networkLink;
            }
            catch (InvalidOperationException ex)
            {
                ex.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
        }

        #endregion Private Methods
    }
}