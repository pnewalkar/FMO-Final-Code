﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.Data.DeliveryPoint.WebAPI.DTO
{
    public class AddDeliveryPointDTO
    {
        /// <summary>
        /// Gets or sets a Postal Address for entity types searched while adding delivery point.
        /// </summary>
        public PostalAddressDTO PostalAddressDTO { get; set; }

        /// <summary>
        /// Gets or sets a delivery point for entity types searched while adding delivery point.
        /// </summary>
        public DeliveryPointDTO DeliveryPointDTO { get; set; }

        /// <summary>
        /// Gets or sets a address location for entity types searched while adding delivery point.
        /// </summary>
        public AddressLocationDTO AddressLocationDTO { get; set; }

        /// <summary>
        /// Gets or sets the Type of Delivery Point i.e. Range, Sub-Building Range and/or Building Number in Building Name
        /// </summary>
        public string DeliveryPointType { get; set; }

        /// <summary>
        /// Gets or sets the Type of Range i.e. Even, Odd and/or Consecutive
        /// </summary>
        public string RangeType { get; set; }

        /// <summary>
        /// Gets or sets the From Range value
        /// </summary>
        public int FromRange { get; set; }

        /// <summary>
        /// Gets or sets the To Range value
        /// </summary>
        public int ToRange { get; set; }

        /// <summary>
        /// Gets or sets the To Sub buidling value
        /// </summary>
        public string SubBuildingType { get; set; }

        /// <summary>
        /// Gets or sets collection of address alises for respective Postal Address
        /// </summary>
        public List<PostalAddressAliasDTO> PostalAddressAliasDTOs { get; set; }
    }
}