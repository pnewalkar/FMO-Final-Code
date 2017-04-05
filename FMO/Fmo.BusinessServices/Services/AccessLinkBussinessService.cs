using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using System.IO;

namespace Fmo.BusinessServices.Services
{
    public class AccessLinkBussinessService : IAccessLinkBussinessService
    {
        private IAccessLinkRepository accessLinkRepository = default(IAccessLinkRepository);

        public AccessLinkBussinessService(IAccessLinkRepository searchAccessLinkRepository)
        {
            this.accessLinkRepository = searchAccessLinkRepository;
        }

        public List<AccessLinkDTO> SearchAccessLink()
        {
            return accessLinkRepository.SearchAccessLink();
        }

        public MemoryStream GetAccessLinks(string bbox)
        {
            string[] bboxArr = bbox.Split(',');
            return accessLinkRepository.GetAccessLinks(bboxArr);
        }
    }
}
