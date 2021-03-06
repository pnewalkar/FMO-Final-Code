﻿using System.Collections.Generic;
using AutoMapper;

namespace RM.DataManagement.AccessLink.WebAPI.DataService.MappingConfiguration
{
    public static class GenericMapper
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

        public static void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDestination>());

            Mapper.Configuration.CreateMapper();

            Mapper.Map<TSource, TDestination>(source, destination);
        }
    }
}