using System;

namespace RM.Common.ActionManager.WebAPI.DTO
{
    //TODO: tobe marked as internal
    public class UserUnitInfoDataDTO
    {
        public string UserName { get; set; }

        public Guid LocationId { get; set; }

        public string UnitName { get; set; }

        public Guid UnitId { get; set; }

        public string UnitType { get; set; }
    }
}