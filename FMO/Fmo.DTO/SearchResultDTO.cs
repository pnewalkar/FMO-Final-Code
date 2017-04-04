using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fmo.DTO
{
    public class SearchResultDTO
    {
        public ICollection<DeliveryRouteDTO> DeliveryRoute { get; set; }

        public ICollection<DeliveryPointDTO> DeliveryPoint { get; set; }

        public ICollection<StreetNameDTO> StreetName { get; set; }

        public ICollection<PostCodeDTO> PostCode { get; set; }

        public ICollection<PostalAddressDTO> PostalAddress { get; set; }

        public ICollection<NetworkLinkDTO> NetworkLink { get; set; }

        [NotMapped]
        public int TotalCount { get; set; }
    }
}
