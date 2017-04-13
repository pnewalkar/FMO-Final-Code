using System.Collections.Generic;

//using System.ServiceModel;

namespace Fmo.Helpers
{
    public class GeoJson
    {
        public string type { get; set; } = "FeatureCollection";

        public List<Feature> features { get; set; }

        //public JObject getJson()
        //{
        //    var obj = new JObject();

        //    obj.Add("type", type);
        //    obj.Add("features", JArray.FromObject(features.Select(feat => feat.getJson())));

        //    return obj;
        //}
    }
}