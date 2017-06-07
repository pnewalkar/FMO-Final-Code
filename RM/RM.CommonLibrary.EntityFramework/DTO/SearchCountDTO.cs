using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    public class SearchCountDTO
    {
        public SearchBusinessEntityType Type { get; set; }

        public int? Count { get; set; }
    }
}