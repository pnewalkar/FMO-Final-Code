using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.MappingConfiguration.Interface
{
    public interface IAutoMapper<TSource, TDestination>
        where TSource : class
        where TDestination : class
    {
        TDestination SetupMappings(TSource source);
    }
}
