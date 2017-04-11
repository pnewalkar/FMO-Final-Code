using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class ReferenceDataDTO
    {
        public int ReferenceData_Id { get; set; }

        public int ReferenceDataCategory_Id { get; set; }
  
        public string ReferenceDataName { get; set; }
       
        public string ReferenceDataValue { get; set; }
    
        public string DataDescription { get; set; }
    
        public string DisplayText { get; set; }

        public int? DataParentId { get; set; }

        public Guid ID { get; set; }

        public Guid ReferenceDataCategory_GUID { get; set; }

        public Guid? DataParent_GUID { get; set; }
    }
}
