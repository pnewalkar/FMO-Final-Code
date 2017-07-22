using System;
using System.Data.Entity.Spatial;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace RM.CommonLibrary.HelperMiddleware
{
    public class DbGeometryConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(DbGeometry).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            JObject location = JObject.Load(reader);
            string geometry = "geometry";
            string wellKnownText = "wellKnownText";
            string coordinateSystemId = "coordinateSystemId";
            if (((JProperty)location.First).Name == "Geometry")
            {
                geometry = "Geometry";
                wellKnownText = "WellKnownText";
                coordinateSystemId = "CoordinateSystemId";
            }
            JToken token = location[geometry][wellKnownText];
            string value = token.ToString();
            JToken sridToken = location[geometry][coordinateSystemId];
            int srid;
            if (sridToken == null || int.TryParse(sridToken.ToString(), out srid) == false || value.Contains("SRID"))
            {
                //Set default coordinate system here.
                srid = 0;
            }

            DbGeometry converted;
            if (srid > 0)
                converted = DbGeometry.FromText(value, srid);
            else
                converted = DbGeometry.FromText(value);

            return converted;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            // Base serialization is fine
            serializer.Serialize(writer, value);
        }
    }
}