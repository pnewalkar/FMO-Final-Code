using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using RM.Data.DeliveryPoint.WebAPI.DTO;
using RM.Data.DeliveryPoint.WebAPI.DTO.Model;

namespace RM.DataManagement.DeliveryPoint.WebAPI.Integration
{
    public interface IDeliveryPointIntegrationService
    {
        /// <summary>
        ///  Retreive reference data details from
        /// </summary>
        /// <param name="categoryName">categoryname</param>
        /// <param name="itemName">Reference data Name</param>
        /// <returns>GUID</returns>
        Guid GetReferenceDataGuId(string categoryName, string itemName);

        /// <summary>
        /// Create automatic access link creation after delivery point creation.
        /// </summary>
        /// <param name="operationalObjectId">Operational Object unique identifier.</param>
        /// <param name="operationObjectTypeId">Operational Object type unique identifier.</param>
        /// <returns>bool</returns>
        bool CreateAccessLink(Guid operationalObjectId, Guid operationObjectTypeId);

        /// <summary>
        /// Create address for a delivery point with PAF/NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        Task<CreateDeliveryPointModelDTO> CreateAddressForDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO);

        /// <summary>
        /// This method is used to check Duplicate NYB records
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress as input</param>
        /// <returns>string</returns>
        Task<string> CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// This method is used to check for Duplicate Address with Delivery Points.
        /// </summary>
        /// <param name="objPostalAddress">Postal Addess Dto as input</param>
        /// <returns>bool</returns>
        Task<bool> CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress);

        /// <summary>
        /// Method to map a route for delivery point
        /// </summary>
        /// <param name="deliveryRouteId">deliveryRouteId</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        /// <returns>bool</returns>
        Task<bool> MapRouteForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId);

        Task<List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames);

        /// <summary>
        /// This method is used to get route for delivery point.
        /// </summary>
        /// <param name="deliveryPointId">deliveryPointId as input</param>
        /// <returns>The Route details for the provided delivery point.</returns>
        Task<RouteDTO> GetRouteForDeliveryPoint(Guid deliveryPointId);

        /// <summary>
        /// Gets approx location based on the potal code.
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns>The approx location/</returns>
        Task<DbGeometry> GetApproxLocation(string postcode);

        /// <summary>
        /// Delete delivery point reference from route activity table.
        /// </summary>
        /// <param name="deliveryPointId">Delivery point unique id</param>
        /// <returns>boolean</returns>
        Task<bool> DeleteDeliveryPointRouteMapping(Guid deliveryPointId);

        /// <summary>
        /// Delete access link
        /// </summary>
        /// <param name="deliveryPointId">Delivery point unique id</param>
        /// <returns>boolean</returns>
        Task<bool> DeleteAccesslink(Guid deliveryPointId);
    }
}