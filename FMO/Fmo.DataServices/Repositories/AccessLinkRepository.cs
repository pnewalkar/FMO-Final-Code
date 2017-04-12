namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;
    using System.IO;
    
 
    using System.Data.SqlTypes;


    public class AccessLinkRepository : RepositoryBase<AccessLink, FMODBContext>, IAccessLinkRepository
    {
        public AccessLinkRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<AccessLinkDTO> SearchAccessLink()
        {
            try
            {
                var result = DataContext.AccessLinks.ToList();
                return GenericMapper.MapList<AccessLink, AccessLinkDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<AccessLink> GetData(string coordinates)
        {
            System.Data.Entity.Spatial.DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(coordinates.ToString(), 27700);

            //return DataContext.AccessLinks.Take(100);
            return DataContext.AccessLinks.Where(dp => dp.AccessLinkLine.Intersects(extent)).ToList();
        }

        public List<AccessLinkDTO> GetAccessLinks(string coordinates)
        {
            List<AccessLink> result = GetData(coordinates).ToList();
            var accessLink = GenericMapper.MapList<AccessLink, AccessLinkDTO>(result);
            return accessLink;
        }
    }
}
