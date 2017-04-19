namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Threading.Tasks;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    public class RoadNameRepository : RepositoryBase<RoadName, FMODBContext>, IRoadNameRepository
    {
        public RoadNameRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This method is used to Fetch Road Name details.
        /// </summary>
        /// <returns> Task List of Road Name DTO</returns>
        public async Task<List<RoadNameDTO>> FetchRoadName()
        {
            try
            {
                var result = await DataContext.RoadNames.ToListAsync();
                return GenericMapper.MapList<RoadName, RoadNameDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This method is used to get Road Link data as per coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>List of Road Link entity</returns>
        public IEnumerable<OSRoadLink> GetData(string coordinates)
        {
            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(coordinates.ToString(), 27700);

            return DataContext.OSRoadLinks.Where(dp => dp.CentreLineGeometry != null && dp.CentreLineGeometry.Intersects(extent)).ToList();
        }

        /// <summary>
        /// This method is used to fetch Road routes data.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>List of OsRoadLinkDTO</returns>
        public List<OsRoadLinkDTO> GetRoadRoutes(string coordinates)
        {
            List<OSRoadLink> result = GetData(coordinates).ToList();
            var oSRoadLink = GenericMapper.MapList<OSRoadLink, OsRoadLinkDTO>(result);
            return oSRoadLink;
        }
    }
}
