namespace RM.CommonLibrary.EntityFramework.DTO
{
    public class Properties
    {
        public string type { get; set; }
        public int? groupId { get; set; }
        public string groupType { get; set; }
        public string groupName { get; set; }

        public byte[] rowversion { get; set; }
    }
}