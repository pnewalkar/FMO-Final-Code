using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class OsRoadLinkDTO
    {

        public int NetworkLink_Id { get; set; }

        public string TOID { get; set; }

        public DbGeometry CentreLineGeometry { get; set; }

        public bool? Ficticious { get; set; }

        public string RoadClassificaton { get; set; }

        public string RouteHierarchy { get; set; }

        public string FormOfWay { get; set; }

        public byte[] TrunkRoad { get; set; }

        public byte[] PrimaryRoute { get; set; }

        public string RoadClassificationNumber { get; set; }

        public string RoadName { get; set; }

        public string AlternateName { get; set; }

        public string Directionality { get; set; }
        public decimal LengthInMeters { get; set; }

        public string StartNodeTOID { get; set; }

        public string EndNodeTOID { get; set; }

        public byte StartGradeSeparation { get; set; }

        public byte EndGradeSeparation { get; set; }

        public string OperationalState { get; set; }

        public string type { get; set; }

        public object features { get; set; }

        //public virtual NetworkLink NetworkLink { get; set; }
    }
}
