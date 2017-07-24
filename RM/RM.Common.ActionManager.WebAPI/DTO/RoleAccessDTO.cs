using System;

namespace RM.Common.ActionManager.WebAPI.DTO
{
    public class RoleAccessDTO
    {
        public string UserName { get; set; }

        public string RoleName { get; set; }

        public Guid Unit_GUID { get; set; }

        public string FunctionName { get; set; }

        public string ActionName { get; set; }

        public Guid UserId { get; set; }

        public string UnitType { get; set; }

        public string UnitName { get; set; }
    }
}