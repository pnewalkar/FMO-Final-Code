namespace RM.Common.ReferenceData.WebAPI.BusinessService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Serialization;
    using CommonLibrary.ConfigurationMiddleware;
    using CommonLibrary.HelperMiddleware;
    using DTO;
    using Microsoft.Extensions.FileProviders;
    using RM.Common.DataService.Interface;
    using RM.Common.ReferenceData.WebAPI.BusinessService.Interface;
    using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;

    public class ReferenceDataBusinessService : IReferenceDataBusinessService
    {
        private static ReferenceDataMapping referenceDataMapping;
        private readonly IFileProvider fileProvider;
        private IConfigurationHelper configHelper = default(IConfigurationHelper);
        private string RefDataXMLFileName = string.Empty;
        private IReferenceDataDataService referenceDataDataService;

        //private IConfigurationHelper configHelper = default(IConfigurationHelper);

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
            RefDataXMLFileName = configHelper.ReadAppSettingsConfigurationValues(Constants.RefDataXMLFileName);
            IFileInfo fileInfo = fileProvider.GetFileInfo(RefDataXMLFileName); // a file under applicationRoot

            XmlSerializer serializer = new XmlSerializer(typeof(ReferenceDataMapping));

            using (StreamReader reader = new StreamReader(fileInfo.CreateReadStream()))
            {
                referenceDataMapping = (ReferenceDataMapping)serializer.Deserialize(reader);
                reader.Close();
            }
        }

        #endregion Constructor

        #region Reference Data Manager methods

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