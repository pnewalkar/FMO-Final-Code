using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity = Fmo.Entities;
using Dto = Fmo.DTO;

namespace Fmo.MappingConfiguration
{
    public class DeliveryPointMapper
    {
        public static List<Dto.DeliveryPoint> SetupMappings(List<Entity.DeliveryPoint> source)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Entity.DeliveryPoint, Dto.DeliveryPoint>());

            Mapper.Configuration.CreateMapper();
            List<Dto.DeliveryPoint> dto = Mapper.Map<List<Entity.DeliveryPoint>, List<Dto.DeliveryPoint>>(source);
            
            return dto;
        }
    }
}
