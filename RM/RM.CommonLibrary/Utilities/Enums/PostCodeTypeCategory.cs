using System.ComponentModel;

namespace RM.CommonLibrary.HelperMiddleware
{
    /// <summary>
    /// Postcode types for ReferenceData category "Postcode Type"
    /// </summary>
    public enum PostCodeTypeCategory
    {
        None = 0,

        [Description("PostcodeSector")]
        PostcodeSector,

        [Description("PostcodeArea")]
        PostcodeArea,

        [Description("Postcode")]
        Postcode,

        [Description("PostcodeDistrict")]
        PostcodeDistrict
    }
}
