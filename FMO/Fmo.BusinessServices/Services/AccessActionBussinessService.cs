using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Helpers;
using Fmo.Helpers.Interface;
using Microsoft.SqlServer.Types;
using Newtonsoft.Json;

namespace Fmo.BusinessServices.Services
{
   public class AccessActionBussinessService : IAccessActionBussinessService
    {
        private IAccessActionRepository accessActionRepository = default(IAccessActionRepository);

        public AccessActionBussinessService(IAccessActionRepository accessActionRepository)
        {
            this.accessActionRepository = accessActionRepository;

        }

        public List<AccessActionDTO> FetchAccessActions()
        {
            return accessActionRepository.FetchAccessActions();
        }
    }
}
