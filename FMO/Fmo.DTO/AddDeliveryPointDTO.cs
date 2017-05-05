using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class AddDeliveryPointDTO
    {
        public AddDeliveryPointDTO()
        {
            this.postalAddress =new PostalAddressDTO();
            this.deliveryPoint = new DeliveryPointDTO();
            this.addressLocation =new AddressLocationDTO();
        }

        /// <summary>
        /// Gets or sets a Postal Address for entity types searched while adding delivery point.
        /// </summary>
        public PostalAddressDTO postalAddress { get; set; }

        /// <summary>
        /// Gets or sets a delivery point for entity types searched while adding delivery point.
        /// </summary>
        public DeliveryPointDTO deliveryPoint { get; set; }

        /// <summary>
        /// Gets or sets a address location for entity types searched while adding delivery point.
        /// </summary>
        public AddressLocationDTO addressLocation { get; set; }
    }
}
