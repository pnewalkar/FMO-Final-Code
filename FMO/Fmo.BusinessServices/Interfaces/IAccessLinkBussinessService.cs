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

        string GetAccessLinks(string boundaryBox);
        
    }
}
