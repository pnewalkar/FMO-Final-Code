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
            if (type == Constants.Constants.LinestringObject)
            {
                return ConstructLinestring(geometry);
            }
            else if (type == Constants.Constants.PointObject)
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
            linestring = linestring.Replace(Constants.Constants.CommaWithoutSpace, Constants.Constants.SingleSpace);
            linestring = linestring.Replace(Constants.Constants.CloseSquareBracket + Constants.Constants.SingleSpace + Constants.Constants.OpenSquareBracket, Constants.Constants.CommaWithoutSpace);
            linestring = linestring.Replace(Constants.Constants.OpenSquareBracket, string.Empty);
            linestring = linestring.Replace(Constants.Constants.CloseSquareBracket, string.Empty);
            return string.Format(Constants.Constants.Linestring, linestring);
        }

        private static string ConstructPoint(string point)
        {

            point = point.Replace(Constants.Constants.CommaWithoutSpace, Constants.Constants.SingleSpace);
            point = point.Replace(Constants.Constants.OpenSquareBracket, string.Empty);
            point = point.Replace(Constants.Constants.CloseSquareBracket, string.Empty);
            return string.Format(Constants.Constants.Point, point);
        }
    }
}
