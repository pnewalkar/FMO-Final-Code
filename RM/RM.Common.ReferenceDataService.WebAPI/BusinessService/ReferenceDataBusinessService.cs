namespace RM.Common.ReferenceData.WebAPI.BusinessService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using CommonLibrary.ConfigurationMiddleware;
    using CommonLibrary.HelperMiddleware;
    using Microsoft.Extensions.FileProviders;
    using RM.Common.DataService.Interface;
    using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
    using Utils;

    public class ReferenceDataBusinessService : IReferenceDataBusinessService
    {
        #region Member Variables
        private static ReferenceDataMapping referenceDataMapping;
        private readonly IFileProvider fileProvider;
        private IConfigurationHelper configHelper = default(IConfigurationHelper);
        private string refDataXMLFileName = string.Empty;
        private IReferenceDataDataService referenceDataDataService; 
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDataBusinessService"/> class and
        /// other classes.
        /// </summary>
        /// <param name="referenceDataCategoryDataService">IReferenceDataCategoryDataService reference</param>
        public ReferenceDataBusinessService(IFileProvider fileProvider, IReferenceDataDataService referenceDataDataService, IConfigurationHelper configHelper)
        {
            this.fileProvider = fileProvider;
            this.referenceDataDataService = referenceDataDataService;
            IDirectoryContents contents = fileProvider.GetDirectoryContents(string.Empty); // the applicationRoot contents
            refDataXMLFileName = configHelper.ReadAppSettingsConfigurationValues(Constants.RefDataXMLFileName);
            IFileInfo fileInfo = fileProvider.GetFileInfo(refDataXMLFileName); // a file under applicationRoot

            XmlSerializer serializer = new XmlSerializer(typeof(ReferenceDataMapping));

            using (StreamReader reader = new StreamReader(fileInfo.CreateReadStream()))
            {
                referenceDataMapping = (ReferenceDataMapping)serializer.Deserialize(reader);
                reader.Close();
            }
        }

        #endregion Constructor

        #region Methods

        /// <summary> Gets all reference data. </summary> <param name="group">group is recorded as
        /// the category name</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public NameValuePair GetReferenceDataByNameValuePairs(string appGroupName, string appItemName)
        {
            var itemName = default(string);
            var categoryName = referenceDataMapping.CategoryNames
                                .Where(a => a.AppCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase)
                                           || a.DBCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase))
                                .Select(s => s.DBCategoryName).FirstOrDefault();

            if (categoryName != null)
            {
                itemName = referenceDataMapping.CategoryNames
                                    .Where(a => (a.AppCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase)
                                        || a.DBCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase))
                                        && (a.ReferenceDataNames.Any(b => b.AppReferenceDataName.Equals(appItemName, StringComparison.InvariantCultureIgnoreCase)
                                                                       || b.DBReferenceDataName.Equals(appItemName, StringComparison.InvariantCultureIgnoreCase))))
                                    .Select(s => s.ReferenceDataNames.Select(t => t.DBReferenceDataName).FirstOrDefault()).FirstOrDefault();
            }

            return referenceDataDataService.GetNameValueReferenceData(categoryName, itemName);
        }

        /// <summary> Gets all reference data. </summary> <param name="group">group is recorded as
        /// the category name</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public List<NameValuePair> GetReferenceDataByNameValuePairs(string appGroupName)
        {
            var categoryName = referenceDataMapping.CategoryNames
                                    .Where(a => a.AppCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase)
                                             || a.DBCategoryName.Equals(appGroupName, StringComparison.InvariantCultureIgnoreCase))
                                    .Select(s => s.DBCategoryName).FirstOrDefault();

            return referenceDataDataService.GetNameValueReferenceData(categoryName);
        }

        /// <summary> Gets all reference data. </summary> <param name="group">group is recorded as
        /// the category name</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public SimpleListDTO GetSimpleListsReferenceData(string listName)
        {
            var categoryName = referenceDataMapping.CategoryNames
                                    .Where(a => a.AppCategoryName.Equals(listName, StringComparison.InvariantCultureIgnoreCase)
                                             || a.DBCategoryName.Equals(listName, StringComparison.InvariantCultureIgnoreCase))
                                    .Select(s => s.DBCategoryName).FirstOrDefault();

            return referenceDataDataService.GetSimpleListReferenceData(categoryName);
        }

        /// <summary> Gets all reference data. </summary> <param name="group">group is recorded as
        /// the category name</param> <returns>List of <see cref="ReferenceDataCategoryDTO"></returns>
        public List<SimpleListDTO> GetSimpleListsReferenceData(List<string> listNames)
        {
            List<SimpleListDTO> simpleDtoList = new List<SimpleListDTO>();

            var categoryNames = referenceDataMapping.CategoryNames
                                    .Where(a => listNames.Any(x => x.Equals(a.AppCategoryName, StringComparison.InvariantCultureIgnoreCase)
                                                                || x.Equals(a.DBCategoryName, StringComparison.InvariantCultureIgnoreCase)))
                                    .Select(s => s.DBCategoryName);

            foreach (var categoryName in categoryNames)
            {
                var simpleListObject = referenceDataDataService.GetSimpleListReferenceData(categoryName);
                if (simpleListObject != null)
                {
                    simpleDtoList.Add(simpleListObject);
                }
            }

            return simpleDtoList;
        }

        #endregion Reference Data Manager methods
    }
}