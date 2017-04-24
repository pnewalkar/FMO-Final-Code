namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using Common.Constants;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;
    using Interfaces;

    /// <summary>
    /// This class contains methods of Access Link Repository for fetching Access Link data.
    /// </summary>
    public class AccessLinkRepository : RepositoryBase<AccessLink, FMODBContext>, IAccessLinkRepository
    {
        public AccessLinkRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>Link of Access Link Entity</returns>
        private IEnumerable<AccessLink> GetAccessLinkCoordinatesDataByBoundingBox(string boundingBoxCoordinates, Guid unitGuid)
        {
            if (!string.IsNullOrEmpty(boundingBoxCoordinates))
            {
                DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), Constants.BNGCOORDINATESYSTEM);
                return DataContext.AccessLinks.Where(x => x.AccessLinkLine.Intersects(extent) && x.AccessLinkLine.Intersects(polygon)).ToList();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Access Link Dto</returns>
        public List<AccessLinkDTO> GetAccessLinks(string boundingBoxCoordinates, Guid unitGuid)
        {
            List<AccessLink> result = GetAccessLinkCoordinatesDataByBoundingBox(boundingBoxCoordinates, unitGuid).ToList();
            var accessLink = GenericMapper.MapList<AccessLink, AccessLinkDTO>(result);
            return accessLink;
        }
    }
}