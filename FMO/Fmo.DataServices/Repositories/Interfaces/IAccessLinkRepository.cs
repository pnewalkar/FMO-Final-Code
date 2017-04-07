using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using System.IO;

namespace Fmo.DataServices.Repositories.Interfaces
{
   public interface IAccessLinkRepository
    {
        List<AccessLinkDTO> SearchAccessLink();

        List<AccessLinkDTO> GetAccessLinks(string coordinates);
    }
}
