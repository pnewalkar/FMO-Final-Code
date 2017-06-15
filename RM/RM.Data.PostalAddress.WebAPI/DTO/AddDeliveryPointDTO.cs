using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.DataManagement.PostalAddress.WebAPI.DTO
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
    }
}
