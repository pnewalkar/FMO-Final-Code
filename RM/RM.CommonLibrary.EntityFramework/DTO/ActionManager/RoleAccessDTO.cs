using System;

namespace RM.CommonLibrary.EntityFramework.DTO
{
    public class RoleAccessDTO
    {
        public string UserName { get; set; }

        public string RoleName { get; set; }

        public Guid Unit_GUID { get; set; }

        public string FunctionName { get; set; }

        public string ActionName { get; set; }

        public Guid UserId { get; set; }
    }
}