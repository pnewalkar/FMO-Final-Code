namespace RM.CommonLibrary.HelperMiddleware
{
    public static class ObjectParser
    {
        public static string GetGeometry(string geometry, string type)
        {
            if (type == Constants.LinestringObject)
            {
                return ConstructLinestring(geometry);
            }
            else if (type == Constants.PointObject)
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
            linestring = linestring.Replace(Constants.CommaWithoutSpace, Constants.SingleSpace);
            linestring = linestring.Replace(Constants.CloseSquareBracket + Constants.SingleSpace + Constants.OpenSquareBracket, Constants.CommaWithoutSpace);
            linestring = linestring.Replace(Constants.OpenSquareBracket, string.Empty);
            linestring = linestring.Replace(Constants.CloseSquareBracket, string.Empty);
            return string.Format(Constants.Linestring, linestring);
        }

        private static string ConstructPoint(string point)
        {
            point = point.Replace(Constants.CommaWithoutSpace, Constants.SingleSpace);
            point = point.Replace(Constants.OpenSquareBracket, string.Empty);
            point = point.Replace(Constants.CloseSquareBracket, string.Empty);
            return string.Format(Constants.Point, point);
        }
    }
}