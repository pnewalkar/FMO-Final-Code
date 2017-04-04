using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Entity = Fmo.Entities;
using Dto = Fmo.DTO;
using System.Reflection;

namespace Fmo.MappingConfiguration
{
    public class GenericMapper
    {
       
        public static TDestination Map<TSource, TDestination>(TSource source)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDestination>().IgnoreAllUnmapped());
            

            Mapper.Configuration.CreateMapper();

            TDestination result = Mapper.Map<TSource, TDestination>(source);
            return result;
        }

        public static List<TDestination> MapList<TSource, TDestination>(List<TSource> source)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDestination>().IgnoreAllUnmapped());

            Mapper.Configuration.CreateMapper();

            List<TDestination> result = Mapper.Map<List<TSource>, List<TDestination>>(source);
       
            return result;
        }

        public static void Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<TSource, TDestination>().IgnoreAllUnmapped());

            Mapper.Configuration.CreateMapper();

            Mapper.Map<TSource, TDestination>(source,destination);
        }

    }

    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllUnmapped<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}
