using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
   public class ReferenceDataCategory
    {
        public int ReferenceDataCategory_Id { get; set; }
        
        public string CategoryName { get; set; }

        public bool Maintainable { get; set; }

        public int CategoryType { get; set; }
        
        public virtual ICollection<ReferenceData> ReferenceDatas { get; set; }
    }
}
