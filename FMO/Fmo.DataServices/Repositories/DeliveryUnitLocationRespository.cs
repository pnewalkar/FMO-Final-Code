﻿using System.Collections.Generic;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class DeliveryUnitLocationRespository : RepositoryBase<UnitLocation, FMODBContext>, IDeliveryUnitLocationRepository
    {
        public DeliveryUnitLocationRespository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// Fetch the delivery unit.
        /// </summary>
        /// <returns>List</returns>
        public List<DeliveryUnitLocationDTO> FetchDeliveryUnit()
        {
            try
            {
                var result = DataContext.UnitLocations.Where(x => x.UnitBoundryPolygon != null).ToList();
                return GenericMapper.MapList<UnitLocation, DeliveryUnitLocationDTO>(result.ToList());
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
