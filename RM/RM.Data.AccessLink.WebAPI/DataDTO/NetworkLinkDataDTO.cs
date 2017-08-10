using System;
using System.Data.Entity.Spatial;

namespace RM.Data.AccessLink.WebAPI.DataDTOs
{
    public class NetworkLinkDataDTO
    {
        /// <summary>
        /// This class represents data transfer object for NetworkLink entity
        /// </summary>
        public NetworkLinkDataDTO()
        {
        }

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

        public NetworkNodeDataDTO NetworkNode { get; set; }

        public NetworkNodeDataDTO NetworkNode1 { get; set; }
    }
}