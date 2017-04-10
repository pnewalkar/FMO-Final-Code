namespace Fmo.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("FMO.TempPostalAddress")]
    public partial class TempPostalAddress
    {
        [Key]
        public int Address_Id { get; set; }

        public int? Temp_Address_Id { get; set; }
    }
}