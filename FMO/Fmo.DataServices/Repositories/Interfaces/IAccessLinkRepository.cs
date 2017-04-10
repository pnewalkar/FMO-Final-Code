namespace Fmo.DataServices.Repositories.Interfaces
{
    using System.Collections.Generic;
    using Fmo.DTO;
    using Fmo.Entities;

    public interface IAccessLinkRepository
    {
        List<AccessLinkDTO> SearchAccessLink();

        List<AccessLinkDTO> GetAccessLinks(string coordinates);

        IEnumerable<AccessLink> GetData(string coordinates);
    }
}
