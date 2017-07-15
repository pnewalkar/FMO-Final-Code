using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.AccessLink.WebAPI.DataDTOs
{
    public class NetworkLinkDataDTO
    {
        /// <summary>
        /// This class represents data transfer object for NetworkLink entity
        /// </summary>
        public Guid ID { get; set; }
    
        public string TOID { get; set; }
 
        public DbGeometry LinkGeometry { get; set; }

        public decimal LinkLength { get; set; }

        public int? LinkGradientType { get; set; }

        public Guid? NetworkLinkTypeGUID { get; set; }

        public Guid? DataProviderGUID { get; set; }

        public Guid? RoadNameGUID { get; set; }

        public Guid? StreetNameGUID { get; set; }

        public Guid? StartNodeID { get; set; }

        public Guid? EndNodeID { get; set; }

        public string LinkName { get; set; }

        public DateTime RowCreateDateTime { get; set; }
    }
}
