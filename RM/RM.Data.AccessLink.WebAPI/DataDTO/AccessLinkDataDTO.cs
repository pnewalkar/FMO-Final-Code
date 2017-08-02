﻿using System;
using System.Collections.Generic;

namespace RM.Data.AccessLink.WebAPI.DataDTOs
{
    public class AccessLinkDataDTO
    {
        public AccessLinkDataDTO()
        {
            this.AccessLinkStatus = new List<AccessLinkStatusDataDTO>();
            this.NetworkLink = new NetworkLinkDataDTO();
        }

        public Guid ID { get; set; }

        public bool? Approved { get; set; }

        public decimal WorkloadLengthMeter { get; set; }

        public Guid? AccessLinkTypeGUID { get; set; }

        public Guid? LinkDirectionGUID { get; set; }

        public DateTime RowCreateDateTime { get; set; }

        public Guid? ConnectedNetworkLinkID { get; set; }

        public NetworkLinkDataDTO NetworkLink { get; set; }

        public NetworkLinkDataDTO NetworkLink1 { get; set; }

        public ICollection<AccessLinkStatusDataDTO> AccessLinkStatus { get; set; }

        //public AccessLinkDataDTO()
        //{
        //    this.AccessLinkStatus = new List<AccessLinkStatusDataDTO>();
        //    this.NetworkLink = new NetworkLinkDataDTO();
        //    this.NetworkNode = new NetworkNodeDataDTO();
        //}

        ///// <summary>
        ///// This class represents data transfer object for AccessLink entity
        ///// </summary>
        //public Guid ID { get; set; }

        //public bool? Approved { get; set; }

        //public decimal WorkloadLengthMeter { get; set; }

        //public Guid? AccessLinkTypeGUID { get; set; }

        //public Guid? LinkDirectionGUID { get; set; }

        //public byte[] RowVersion { get; set; }

        //public NetworkLinkDataDTO NetworkLink { get; set; }

        //public NetworkLinkDataDTO NetworkLink1 { get; set; }

        //public DateTime RowCreateDateTime { get; set; }

        //public Guid? ConnectedNetworkLinkID { get; set; }

        //public ICollection<AccessLinkStatusDataDTO> AccessLinkStatus { get; set; }
        //public NetworkNodeDataDTO NetworkNode { get; set; }
    }
}