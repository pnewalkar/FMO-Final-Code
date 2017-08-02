using System;

namespace RM.Common.ActionManager.WebAPI.DTO
{
    public class UserUnitInfoDTO
    {
        public string UserName { get; set; }

        public Guid LocationId { get; set; }

        public string UnitName { get; set; }

        public string UnitType { get; set; }
    }
}