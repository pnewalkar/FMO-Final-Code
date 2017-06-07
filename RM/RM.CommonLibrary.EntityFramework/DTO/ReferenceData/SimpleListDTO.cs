using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RM.CommonLibrary.EntityFramework.DTO.ReferenceData
{
    public class SimpleListDTO
    {
        // ReferenceDataCategpry.ID
        public Guid Id { get; set; }

        // ReferenceDataCategpry.CategoryName
        public string ListName { get; set; }

        // ReferenceDataCategory.Maintainable
        public bool Maintainable { get; set; }

        public List<ListItems> ListItems { get; set; }
    }
}
