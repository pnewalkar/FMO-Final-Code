using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Types;

namespace RM.CommonLibrary.HelperMiddleware
{
    public static class SqlGeometryExtension
    {
        public static DbGeometry ToDbGeometry(this SqlGeometry sqlGeometry)
        {
            return DbGeometry.FromBinary(sqlGeometry.STAsBinary().Buffer, (int)sqlGeometry.STSrid);
        }

        public static List<DbGeometry> ToDbGeometry(this List<SqlGeometry> sqlGeometryList)
        {
            List<DbGeometry> dbGeometryList = new List<DbGeometry>();
            sqlGeometryList.ForEach(x => dbGeometryList.Add(x.ToDbGeometry()));
            return dbGeometryList;
        }

        public static SqlGeometry ToSqlGeometry(this DbGeometry dbGeometry)
        {
            return SqlGeometry.STGeomFromWKB(new SqlBytes(dbGeometry.AsBinary()), dbGeometry.CoordinateSystemId);
        }

        public static List<SqlGeometry> ToSqlGeometry(this List<DbGeometry> dbGeometryList)
        {
            List<SqlGeometry> sqlGeometryList = new List<SqlGeometry>();
            dbGeometryList.ForEach(x => sqlGeometryList.Add(x.ToSqlGeometry()));
            return sqlGeometryList;
        }
    }
}