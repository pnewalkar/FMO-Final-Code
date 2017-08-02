using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.DeliveryPoint.WebAPI.DTO.Model
{
    public class CreateDeliveryPointForRangeModelDTO
    {
        public bool HasDuplicates { get; set; } = false;

        public bool HasAllDuplicates { get; set; } = false;

        public string Message { get; set; }

        public List<CreateDeliveryPointModelDTO> CreateDeliveryPointModelDTOs { get; set; }

        public List<PostalAddressDTO> PostalAddressDTOs { get; set; }
    }
}
