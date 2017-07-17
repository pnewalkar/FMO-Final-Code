using AutoMapper;

namespace RM.DataManagement.AccessLink.WebAPI.DataService.MappingConfiguration
{
    /// <summary>
    /// Mapping extensions for generic mapper
    /// </summary>
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}