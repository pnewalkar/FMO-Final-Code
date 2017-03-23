using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Fmo.MappingConfiguration.Interface;
using Entity = Fmo.Entities;
using Dto = Fmo.DTO;

namespace Fmo.MappingConfiguration
{
    public class DeliveryPointMapper : IAutoMapper<Entity.DeliveryPoint, Dto.DeliveryPoint>
    {
        public Dto.DeliveryPoint SetupMappings(Entity.DeliveryPoint source)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Entity.DeliveryPoint, Dto.DeliveryPoint>());

            Mapper.Configuration.CreateMapper();
            Dto.DeliveryPoint dto = Mapper.Map<Entity.DeliveryPoint, Dto.DeliveryPoint>(source);
            return dto;
        }
    }
}
