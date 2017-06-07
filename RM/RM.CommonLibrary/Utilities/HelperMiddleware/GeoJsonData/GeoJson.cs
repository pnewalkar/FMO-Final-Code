using System.Collections.Generic;

namespace RM.CommonLibrary.HelperMiddleware
{
    public class GeoJson
    {
        public string type { get; set; } = "FeatureCollection";

        public List<Feature> features { get; set; }
    }
}