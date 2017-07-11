using System.Collections.Generic;

namespace RM.Common.ActionManager.WebAPI.DTO
{
    public class ResponseDTO
    {
        public string Access_Token { get; set; }

        public int Expires_In { get; set; }

        public List<RoleAccessDataDTO> RoleActions { get; set; }

        public string UserName { get; set; }
    }
}