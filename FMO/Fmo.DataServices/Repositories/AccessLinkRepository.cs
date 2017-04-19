namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Linq;
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
        /// This Method is used to fetch Access Link Data.
        /// </summary>
        /// <returns>List of Access Link DTO</returns>
        public List<AccessLinkDTO> SearchAccessLink()
        {
            try
            {
                var result = DataContext.AccessLinks.ToList();
                return GenericMapper.MapList<AccessLink, AccessLinkDTO>(result.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// This Method is used to Accesss Link data for defined coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>Link of Access Link Entity</returns>
        public IEnumerable<AccessLink> GetData(string coordinates)
        {
            DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(coordinates.ToString(), 27700);
            return DataContext.AccessLinks.Where(dp => dp.AccessLinkLine.Intersects(extent)).ToList();
        }

        /// <summary>
        /// This Method is used to Accesss Link data for defined coordinates.
        /// </summary>
        /// <param name="coordinates">coordinates as string</param>
        /// <returns>List of Access Link Dto</returns>
        public List<AccessLinkDTO> GetAccessLinks(string coordinates)
        {
            List<AccessLink> result = GetData(coordinates).ToList();
            var accessLink = GenericMapper.MapList<AccessLink, AccessLinkDTO>(result);
            return accessLink;
        }
    }
}
