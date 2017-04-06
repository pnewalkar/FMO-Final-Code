using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fmo.DTO
{
    /// <summary>
    /// Used for transfer data for basic/advance search
    /// </summary>
    public class SearchResultDTO
    {
        public SearchResultDTO()
        {
            this.SearchCounts = new List<SearchCountDTO>();
            this.SearchResultItems = new List<SearchResultItemDTO>();
        }

        /// <summary>
        /// Gets or sets a collection of search count for entity types searched.
        /// </summary>
        public ICollection<SearchCountDTO> SearchCounts { get; set; }

        /// <summary>
        /// Gets or sets a collection of search items that includes the entity type
        /// </summary>
        public ICollection<SearchResultItemDTO> SearchResultItems { get; set; }

        //public ICollection<DeliveryRouteDTO> DeliveryRoute { get; set; }

        //public ICollection<DeliveryPointDTO> DeliveryPoint { get; set; }

        //public ICollection<StreetNameDTO> StreetName { get; set; }

        //public ICollection<PostCodeDTO> PostCode { get; set; }

        //public ICollection<PostalAddressDTO> PostalAddress { get; set; }

        //public ICollection<NetworkLinkDTO> NetworkLink { get; set; }

        //[NotMapped]
        //public int TotalCount { get; set; }
    }
}
