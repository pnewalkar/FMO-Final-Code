namespace Fmo.BusinessServices.Interfaces
{
    public interface IDeliveryPointBusinessService
    {
        object GetDeliveryPoints(string boundarybox);

        object GetDeliveryPointByUDPRN(int udprn);
    }
}