using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;

namespace Fmo.Common.SqlGeometryExtension
{
    public static class SqlGeometryExtension
    {
        public static DbGeometry ToDbGeometry(this SqlGeometry sqlGeometry)
        {
            return DbGeometry.FromBinary(sqlGeometry.STAsBinary().Buffer);
        }

        public static SqlGeometry ToSqlGeometry(this DbGeometry dbGeometry)
        {
            return SqlGeometry.STGeomFromWKB(new SqlBytes(dbGeometry.AsBinary()), dbGeometry.CoordinateSystemId);
        }
    }
}
