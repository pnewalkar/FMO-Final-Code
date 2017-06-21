using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// This class contains methods fetching for Road Links data.
    /// </summary>
    public class RoadNameDataService : DataServiceBase<RoadName, RMDBContext>, IRoadNameDataService
    {
        public RoadNameDataService(IDatabaseFactory<RMDBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This method is used to fetch Road routes data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of NetworkLinkDTO</returns>
        public List<NetworkLinkDTO> GetRoadRoutes(string boundingBoxCoordinates, Guid unitGuid)
        {
            List<NetworkLink> result = GetRoadNameCoordinatesDatabyBoundingbox(boundingBoxCoordinates, unitGuid).ToList();
            var networkLink = GenericMapper.MapList<NetworkLink, NetworkLinkDTO>(result);
            return networkLink;
        }

        /// <summary>
        /// This method is used to get Road Link data as per coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Road Link entity</returns>
        private IEnumerable<NetworkLink> GetRoadNameCoordinatesDatabyBoundingbox(string boundingBoxCoordinates, Guid unitGuid)
        {
            try
            {
                IEnumerable<NetworkLink> networkLink = null;

                if (!string.IsNullOrEmpty(boundingBoxCoordinates))
                {
                    DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                    var roadLinkTypeId = DataContext.ReferenceDatas.AsNoTracking().Where(x => x.ReferenceDataValue == ReferenceDataValues.NetworkLinkRoadLink).Select(x => x.ID).SingleOrDefault();

                    DbGeometry extent = DbGeometry.FromText(boundingBoxCoordinates, Constants.BNGCOORDINATESYSTEM);

                    networkLink = DataContext.NetworkLinks.AsNoTracking().Where(x => (x.LinkGeometry != null && x.LinkGeometry.Intersects(extent) && x.LinkGeometry.Intersects(polygon)) && x.NetworkLinkType_GUID == roadLinkTypeId).ToList();
                }

                return networkLink;
            }
            catch (InvalidOperationException ex)
            {
                ex.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new SystemException(ErrorConstants.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
        }
    }
}