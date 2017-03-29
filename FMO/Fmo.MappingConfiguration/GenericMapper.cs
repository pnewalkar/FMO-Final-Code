﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity = Fmo.Entities;
using Dto = Fmo.DTO;

namespace Fmo.MappingConfiguration
{
    public class GenericMapper
    {
       
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDestination>());

            Mapper.Configuration.CreateMapper();

            TDestination result = Mapper.Map<TSource, TDestination>(source);
            return result;
        }

        public static List<TDestination> MapList<TSource, TDestination>(List<TSource> source)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDestination>());

            Mapper.Configuration.CreateMapper();
            List<TDestination> result = Mapper.Map<List<TSource>, List<TDestination>>(source);
            return result;
        }

    }
}