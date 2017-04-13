namespace Fmo.Helpers
{
    public class Geometry
    {
        public string type { get; set; } = "Point";

        public object coordinates { get; set; }

        //public JObject getJson()
        //{
        //    var obj = new JObject();

        //    obj.Add("type", type);
        //    obj.Add("coordinates", coordinates);

        //    return obj;
        //}
    }
}