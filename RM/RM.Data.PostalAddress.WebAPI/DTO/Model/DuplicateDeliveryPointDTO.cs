using System.Collections.Generic;
using RM.DataManagement.PostalAddress.WebAPI.DTO;

namespace RM.Data.PostalAddress.WebAPI.DTO.Model
{
    public class DuplicateDeliveryPointDTO
    {
        public List<PostalAddressDTO> PostalAddressDTO { get; set; }

        public bool IsDuplicate { get; set; }
    }
}