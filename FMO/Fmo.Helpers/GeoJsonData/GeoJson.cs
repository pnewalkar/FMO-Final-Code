using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
//using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fmo.Helpers
{
    public class GeoJson
    {
        public string type { get; set; } = "FeatureCollection";
        public List<Feature> features { get; set; }

        public JObject getJson()
        {
            var obj = new JObject();

            obj.Add("type", type);
            obj.Add("features", JArray.FromObject(features.Select(feat => feat.getJson())));

            return obj;
        }
    }
}
