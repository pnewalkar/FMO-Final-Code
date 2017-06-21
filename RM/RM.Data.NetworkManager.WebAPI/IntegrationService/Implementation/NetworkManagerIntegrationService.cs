using System;
using System.Collections.Generic;
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

namespace RM.DataManagement.NetworkManager.WebAPI.IntegrationService
{
    public class NetworkManagerIntegrationService : INetworkManagerIntegrationService
    {
        private const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        #region Property Declarations

        private string referenceDataWebAPIName = string.Empty;
        private IHttpHandler httpHandler = default(IHttpHandler);

        #endregion Property Declarations

        public NetworkManagerIntegrationService(IHttpHandler httpHandler, IConfigurationHelper configurationHelper)
        {
            this.httpHandler = httpHandler;
            this.referenceDataWebAPIName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(ReferenceDataWebAPIName).ToString() : string.Empty;
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataNameValuePairs(List<string> categoryNames)
        {
            List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();
            List<NameValuePair> nameValuePairs = new List<NameValuePair>();
            foreach (var category in categoryNames)
            {
                HttpResponseMessage result = await httpHandler.GetAsync(referenceDataWebAPIName + "nameValuePairs?appGroupName=" + category);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    throw new ServiceException(responseContent);
                }

                Tuple<string, List<NameValuePair>> apiResult = JsonConvert.DeserializeObject<Tuple<string, List<NameValuePair>>>(result.Content.ReadAsStringAsync().Result);
                nameValuePairs.AddRange(apiResult.Item2);
            }

            listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(nameValuePairs));
            return listReferenceCategories;
        }

        /// <summary> Gets the name of the reference data categories by category. </summary> <param
        /// name="categoryNames">The category names.</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public async Task<List<ReferenceDataCategoryDTO>> GetReferenceDataSimpleLists(List<string> listNames)
        {
            List<ReferenceDataCategoryDTO> listReferenceCategories = new List<ReferenceDataCategoryDTO>();

            HttpResponseMessage result = await httpHandler.PostAsJsonAsync(referenceDataWebAPIName + "simpleLists", listNames);
            if (!result.IsSuccessStatusCode)
            {
                var responseContent = result.ReasonPhrase;
                throw new ServiceException(responseContent);
            }

            List<SimpleListDTO> apiResult = JsonConvert.DeserializeObject<List<SimpleListDTO>>(result.Content.ReadAsStringAsync().Result);
            listReferenceCategories.AddRange(ReferenceDataHelper.MapDTO(apiResult));
            return listReferenceCategories;
        }
    }
}