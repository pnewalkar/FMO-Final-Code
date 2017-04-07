using Fmo.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fmo.DataServices.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        NotificationDTO GetNotificationByUDPRN(int uDPRN);

        Task<int> DeleteNotificationbyUDPRN(int uDPRN);
    }
}
