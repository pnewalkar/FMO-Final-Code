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

        public static SqlGeometry ToSqlGeometry(this DbGeometry dbGeometry)
        {
            return SqlGeometry.STGeomFromWKB(new SqlBytes(dbGeometry.AsBinary()), dbGeometry.CoordinateSystemId);
        }
    }
}