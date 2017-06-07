using System;

namespace RM.CommonLibrary.EntityFramework.DTO
{
   public class DeliveryPointAliasDTO
    {
        public Guid ID { get; set; }

        public Guid DeliveryPoint_GUID { get; set; }
        
        public string DPAlias { get; set; }

        public bool Preferred { get; set; }
    }
}
