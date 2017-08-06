using System.Collections.Generic;

namespace RM.Data.DeliveryPoint.WebAPI.DTO.Model
{
    public class DuplicateDeliveryPointDTO
    {
        public List<PostalAddressDTO> PostalAddressDTO { get; set; }

        public bool IsDuplicate { get; set; }
    }
}
