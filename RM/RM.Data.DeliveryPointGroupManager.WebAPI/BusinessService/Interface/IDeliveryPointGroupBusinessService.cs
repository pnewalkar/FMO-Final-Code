using System;

namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.BusinessService
{
    public interface IDeliveryPointGroupBusinessService
    {
        string GetDeliveryPointGroups(string boundaryBox, Guid unitGuid);
    }
}