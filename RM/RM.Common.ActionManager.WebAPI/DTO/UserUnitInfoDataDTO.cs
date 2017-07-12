using System;

namespace RM.Common.ActionManager.WebAPI.DTO
{
    public class UserUnitInfoDataDTO
    {
        public string UserName { get; set; }

        public Guid LocationId { get; set; }

        public string UnitName { get; set; }

        public Guid UnitId { get; set; }
    }
}