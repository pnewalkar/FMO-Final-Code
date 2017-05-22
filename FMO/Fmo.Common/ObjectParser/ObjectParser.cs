using System;
using System.Collections.Generic;


namespace Fmo.Common.ObjectParser
{
    public static class ObjectParser
    {
        private static List<double[]> lstLines = null;
        private static double[] point = null;

        public static string GetGeometry(string geometry, string type)
        {
            if (type == "LINESTRING")
            {
                return ConstructLinestring(geometry);
            }
            else if (type == "POINT")
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
            linestring = linestring.Replace(",", " ");
            linestring = linestring.Replace("] [", ",");
            linestring = linestring.Replace("[", string.Empty);
            linestring = linestring.Replace("]", string.Empty);
            return string.Format(Constants.Constants.Linestring, linestring);
        }

        private static string ConstructPoint(string point)
        {

            point = point.Replace(",", " ");
            point = point.Replace("[", string.Empty);
            point = point.Replace("]", string.Empty);
            return string.Format(Constants.Constants.Point, point);
        }
    }
}
