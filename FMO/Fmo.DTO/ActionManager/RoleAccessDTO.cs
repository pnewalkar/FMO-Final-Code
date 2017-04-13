using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DTO
{
    public class RoleAccessDTO
    {
        public string UserName { get; set; }
        public string RoleName { get; set; }
        public Guid Unit_GUID { get; set; }
        public string FunctionName { get; set; }
        public string ActionName { get; set; }
    }
}
