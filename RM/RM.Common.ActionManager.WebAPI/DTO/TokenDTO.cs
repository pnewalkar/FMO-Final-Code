using System.Collections.Generic;

namespace RM.Common.ActionManager.WebAPI.DTO
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }

        public int ExpiresIn { get; set; }

        public List<RoleAccessDTO> RoleActions { get; set; }

        public string UserName { get; set; }
    }
}