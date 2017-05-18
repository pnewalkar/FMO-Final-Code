using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO.Model
{
    public class AccessLinkManualCreateModelDTO
    {
        public string OperationalObjectPoint { get; set; }

        public string NetworkIntersectionPoint { get; set; }

        public string AccessLinkLine { get; set; }

        public Guid NetworkLink_GUID { get; set; }
    }
}
