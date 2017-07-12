using System.Collections.Generic;

namespace RM.Common.ActionManager.WebAPI.DTO
{
    public class ResponseDTO
    {
        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public List<RoleAccessDataDTO> RoleActions { get; set; }

        public string UserName { get; set; }
    }
}