using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class ReferenceDataBusinessService : IReferenceDataBusinessService
    {
        private IReferenceDataCategoryRepository referenceDataCategoryRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDataBusinessService"/> class and other classes.
        /// </summary>
        /// <param name="referenceDataCategoryRepository">IReferenceDataCategoryRepository reference</param>
        public ReferenceDataBusinessService(IReferenceDataCategoryRepository referenceDataCategoryRepository)
        {
            this.referenceDataCategoryRepository = referenceDataCategoryRepository;
        }

        /// <summary>
        /// Fetch the Route Log Status.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> FetchRouteLogStatus()
        {
            return referenceDataCategoryRepository.RouteLogStatus();
        }

        /// <summary>
        /// Fetch the Route Log Selection Type.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> FetchRouteLogSelectionType()
        {
            return referenceDataCategoryRepository.RouteLogSelectionType();
        }

        /// <summary>
        /// Gets all reference category list along with associated reference data.
        /// </summary>
        /// <returns>List<ReferenceDataCategoryDTO></returns>
        public List<ReferenceDataCategoryDTO> GetAllReferenceCategoryList()
        {
            return referenceDataCategoryRepository.GetAllReferenceCategoryList();
        }

        /// <summary>
        /// Gets the name of the reference data categories by category.
        /// </summary>
        /// <param name="categoryNames">The category names.</param>
        /// <returns>List of <see cref="ReferenceDataCategoryDTO"/>.</returns>
        public List<ReferenceDataCategoryDTO> GetReferenceDataCategoriesByCategoryNames(List<string> categoryNames)
        {
            return referenceDataCategoryRepository.GetReferenceDataCategoriesByCategoryNames(categoryNames);
        }
    }
}