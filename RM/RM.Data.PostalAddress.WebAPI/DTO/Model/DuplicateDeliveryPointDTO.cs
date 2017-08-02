using RM.DataManagement.PostalAddress.WebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.Data.PostalAddress.WebAPI.DTO.Model
{
    public class DuplicateDeliveryPointDTO
    {
        public List<PostalAddressDTO> PostalAddressDTO { get; set; }

        public bool IsDuplicate { get; set; }
    }
}
