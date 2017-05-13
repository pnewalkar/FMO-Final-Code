using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class NetworkLinkDTO
    {
        public int NetworkLink_Id { get; set; }

        public string NetworkLinkType { get; set; }

        public string TOID { get; set; }

        public string DataProvider { get; set; }

        public int? RoadName_Id { get; set; }

        public int? StreetName_Id { get; set; }

        public int StartNode_Id { get; set; }

        public int EndNode_Id { get; set; }

        public string LinkGeometry { get; set; }

        public decimal LinkLength { get; set; }

        public int? LinkGradientType { get; set; }

        public Guid? NetworkLinkType_GUID { get; set; }
    }
}
