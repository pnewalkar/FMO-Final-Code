using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.Common.Enums;

namespace Fmo.DTO
{
    public class SearchCountDTO
    {
        public SearchBusinessEntityType Type { get; set; }

        public int Count { get; set; }
    }
}
