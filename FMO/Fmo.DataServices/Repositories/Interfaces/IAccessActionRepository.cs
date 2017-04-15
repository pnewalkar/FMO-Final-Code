using Fmo.DTO;
using System.Collections.Generic;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface IAccessActionRepository
    {
        List<AccessActionDTO> FetchAccessActions();
    }
}