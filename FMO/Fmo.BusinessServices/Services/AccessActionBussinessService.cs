using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    /// <summary>
    /// This class contains contains methods for fetching Access Action items.
    /// </summary>
    public class AccessActionBussinessService : IAccessActionBusinessService
    {
        private IAccessActionRepository accessActionRepository = default(IAccessActionRepository);

        public AccessActionBussinessService(IAccessActionRepository accessActionRepository)
        {
            this.accessActionRepository = accessActionRepository;
        }

        /// <summary>
        /// This method is used to fetch Access Actions data.
        /// </summary>
        /// <returns>List of Access Action Dto</returns>
        //public List<AccessActionDTO> FetchAccessActions()
        //{
        //    return accessActionRepository.FetchAccessActions();
        //}
    }
}