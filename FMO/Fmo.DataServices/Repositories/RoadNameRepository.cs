﻿
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

    public class RoadNameRepository : RepositoryBase<RoadName, FMODBContext>, IRoadNameRepository
    {

        public RoadNameRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public List<RoadNameDTO> FetchRoadName()
        {
            try
            {
                var result = DataContext.RoadNames.ToList();
                return GenericMapper.MapList<RoadName, RoadNameDTO>(result.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
