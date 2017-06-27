namespace RM.CommonLibrary.HelperMiddleware
{
    public static class ObjectParser
    {
        private const string Linestring = "LINESTRING({0})";
        private const string Point = "POINT({0})";
        private const string LinestringObject = "LINESTRING";
        private const string PointObject = "POINT";
        private const string CommaWithoutSpace = ",";
        private const string SingleSpace = " ";
        private const string OpenSquareBracket = "[";
        private const string CloseSquareBracket = "]";

        public static string GetGeometry(string geometry, string type)
        {
            if (type == LinestringObject)
            {
                return ConstructLinestring(geometry);
            }
            else if (type == PointObject)
            {
                return ConstructPoint(geometry);
            }
            else
            {
                return string.Empty;
            }
        }

        private static string ConstructLinestring(string linestring)
        {
            linestring = linestring.Replace(CommaWithoutSpace, SingleSpace);
            linestring = linestring.Replace(CloseSquareBracket + SingleSpace + OpenSquareBracket, CommaWithoutSpace);
            linestring = linestring.Replace(OpenSquareBracket, string.Empty);
            linestring = linestring.Replace(CloseSquareBracket, string.Empty);
            return string.Format(Linestring, linestring);
        }

        private static string ConstructPoint(string point)
        {
            point = point.Replace(CommaWithoutSpace, SingleSpace);
            point = point.Replace(OpenSquareBracket, string.Empty);
            point = point.Replace(CloseSquareBracket, string.Empty);
            return string.Format(Point, point);
        }
    }
}