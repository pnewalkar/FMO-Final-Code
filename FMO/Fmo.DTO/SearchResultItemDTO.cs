using Fmo.Common.Enums;

namespace Fmo.DTO
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchResultItemDTO
    {
        /// <summary>
        /// Gets or sets the Id of the underlying entity.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the display text to render in the UI
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// The <see cref="SearchBusinessEntityType"/>.
        /// </summary>
        public SearchBusinessEntityType Type { get; set; }
    }
}