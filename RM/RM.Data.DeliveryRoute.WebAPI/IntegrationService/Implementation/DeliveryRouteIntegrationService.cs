using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.CommonLibrary.EntityFramework.Utilities.ReferenceData;
using RM.CommonLibrary.ExceptionMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.Interfaces;

namespace RM.DataManagement.DeliveryRoute.WebAPI.IntegrationService
{
    public class DeliveryRouteIntegrationService : IDeliveryRouteIntegrationService
    {
        #region Property Declarations

        private string deliveryRouteWebAPIName = string.Empty;
        private string referenceDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);

        #endregion Property Declarations

        #region Constructor

        public DeliveryRouteIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper)
        {
            this.httpHandler = httpHandler;
            this.deliveryRouteWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.DeliveryRouteManagerWebAPIName).ToString() : string.Empty;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.ReferenceDataWebAPIName).ToString() : string.Empty;
        }

        #endregion Constructor

        #region public methods

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> categoryNames)
        {
            List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "simpleLists", categoryNames);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
            listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));
            return listReferenceCategories;
        }

        /// <summary>
        /// Retreive reference data details from
        /// </summary>
        /// <param name="categoryName">categoryname</param>
        /// <param name="itemName">Reference data Name</param>
        /// <returns>GUID</returns>
        public async Task<Guid> GetReferenceDataGuId(string categoryName, string itemName)
        {
            Guid referenceId = Guid.Empty;
            HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "simpleLists?listName=" + categoryName);
            if (!result.IsSuccessStatusCode)
            {
                // LOG ERROR WITH Statuscode
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            Tuple<string, SimpleListDTO> apiResult = JsonConvert.DeserializeObject<Tuple<string, SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
            if (apiResult != null && apiResult.Item2 != null && apiResult.Item2.ListItems != null && apiResult.Item2.ListItems.Count > 0)
            {
                referenceId = apiResult.Item2.ListItems.Where(n => n.Value == itemName).SingleOrDefault().Id;
            }

            return referenceId;
        }

        #endregion public methods
    }
}