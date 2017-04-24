namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using Common.Constants;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    /// <summary>
    /// This class contains methods fetching for Road Links data.
    /// </summary>
    public class RoadNameRepository : RepositoryBase<RoadName, FMODBContext>, IRoadNameRepository
    {
        public RoadNameRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This method is used to fetch Road routes data.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of OsRoadLinkDTO</returns>
        public List<OsRoadLinkDTO> GetRoadRoutes(string boundingBoxCoordinates, Guid unitGuid)
        {
            List<OSRoadLink> result = GetRoadNameCoordinatesDatabyBoundingbox(boundingBoxCoordinates, unitGuid).ToList();
            var oSRoadLink = GenericMapper.MapList<OSRoadLink, OsRoadLinkDTO>(result);
            return oSRoadLink;
        }

        /// <summary>
        /// This method is used to get Road Link data as per coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Road Link entity</returns>
        private IEnumerable<OSRoadLink> GetRoadNameCoordinatesDatabyBoundingbox(string boundingBoxCoordinates, Guid unitGuid)
        {
            if (!string.IsNullOrEmpty(boundingBoxCoordinates))
            {
                DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                DbGeometry extent = DbGeometry.FromText(boundingBoxCoordinates, Constants.BNGCOORDINATESYSTEM);

                return DataContext.OSRoadLinks.AsNoTracking().Where(x => x.CentreLineGeometry != null && x.CentreLineGeometry.Intersects(extent) && x.CentreLineGeometry.Intersects(polygon)).ToList();
            }
            else
            {
                return null;
            }
        }
    }
}