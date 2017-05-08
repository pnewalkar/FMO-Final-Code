namespace Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FMO.TempPostalAddress")]
    public partial class TempPostalAddress
    {
        [Key]
        public int Address_Id { get; set; }

        public int? Temp_Address_Id { get; set; }
    }
}
