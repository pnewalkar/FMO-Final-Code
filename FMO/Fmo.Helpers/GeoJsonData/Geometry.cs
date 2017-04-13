using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.Helpers
{
    public class Geometry
    {
        public string type { get; set; } = "Point";
        public Coordinates coordinates { get; set; }

        public JObject getJson()
        {
            var obj = new JObject();

            obj.Add("type", type);
            obj.Add("coordinates", coordinates.getJson());

            return obj;
        }
    }
}
