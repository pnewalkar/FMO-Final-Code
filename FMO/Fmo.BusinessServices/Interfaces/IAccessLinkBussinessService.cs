using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.DTO;
using System.IO;

namespace Fmo.BusinessServices.Interfaces
{
  public interface IAccessLinkBussinessService
    {
        List<AccessLinkDTO> SearchAccessLink();

        AccessLinkDTO GetAccessLinks(string bbox);

        string GetData(string query, params object[] parameters);
    }
}
