namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryPointBusinessService
    {
        object GetDeliveryPoints(string boundarybox);       

        string GetDeliveryPointByUDPRN(int udprn);
    }
}