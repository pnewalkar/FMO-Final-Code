using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RM.CommonLibrary.HelperMiddleware;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    /// <summary>
    ///
    /// </summary>
    public class SearchResultItemDTO
    {
        /// <summary>
        /// Gets or sets the Id of the underlying entity.
        /// </summary>
        public int? UDPRN { get; set; }

        /// <summary>
        /// Gets or sets the display text to render in the UI
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// The <see cref="SearchBusinessEntityType"/>.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public SearchBusinessEntityType Type { get; set; }
    }
}