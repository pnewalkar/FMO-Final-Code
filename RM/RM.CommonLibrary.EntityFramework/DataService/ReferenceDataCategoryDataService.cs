using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;

using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.EntityFramework.Entities;
using RM.CommonLibrary.DataMiddleware;
using RM.CommonLibrary.ResourceFile;

namespace RM.CommonLibrary.EntityFramework.DataService
{
    /// <summary>
    /// To interact with reference data entity
    /// </summary>
    public class ReferenceDataCategoryDataService : DataServiceBase<ReferenceDataCategory, RMDBContext>, IReferenceDataCategoryDataService
    {
        #region Constructor

        public ReferenceDataCategoryDataService(IDatabaseFactory<RMDBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion Constructor

        public Guid GetReferenceDataId(string strCategoryname, string strRefDataName)
        {
            try
            {
                Guid statusId = Guid.Empty;
                var result = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).Where(n => n.CategoryName.Trim().Equals(strCategoryname, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (result?.ReferenceDatas != null && result.ReferenceDatas.Count > 0)
                {
                    var referenceData = result.ReferenceDatas.Where(n => n.ReferenceDataValue.Trim().Equals(strRefDataName, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                    if (referenceData != null)
                    {
                        statusId = referenceData.ID;
                    }
                }

                return statusId;
            }
            catch (InvalidOperationException ex)
            {
                ex.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new SystemException(ErrorMessageIds.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
        }

        /// <summary>
        ///  Retreive GUIDs for specified categories
        /// </summary>
        /// <param name="strCategoryname">categoryname</param>
        /// <param name="lstRefDataName">Reference data Names</param>
        /// <returns>List<Guid></returns>
        public List<Guid> GetReferenceDataIds(string strCategoryname, List<string> lstRefDataName)
        {
            try
            {
                List<Guid> statusIds = new List<Guid>();
                var result = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).Where(n => n.CategoryName.Trim().Equals(strCategoryname, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (result?.ReferenceDatas != null && result.ReferenceDatas.Count > 0)
                {
                    var referenceData = result.ReferenceDatas.Where(n => lstRefDataName.Contains(n.DataDescription.Trim().ToUpper())).ToList();
                    referenceData.ForEach(r => statusIds.Add(r.ID));
                }

                return statusIds;
            }
            catch (InvalidOperationException ex)
            {
                ex.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new SystemException(ErrorMessageIds.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
        }

        /// <summary>
        /// Fetch the Route log status.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> RouteLogStatus()
        {
            try
            {
                List<ReferenceDataDTO> lstReferenceDt = new List<ReferenceDataDTO>();
                string categoryname = "Delivery Point Operational Status";
                var query = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).Where(n => n.CategoryName.Trim().Equals(categoryname, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
                if (query?.ReferenceDatas != null && query.ReferenceDatas.Count > 0)
                {
                    lstReferenceDt = GenericMapper.MapList<ReferenceData, ReferenceDataDTO>(query.ReferenceDatas.ToList());
                }

                return lstReferenceDt;
            }
            catch (InvalidOperationException ex)
            {
                ex.Data.Add("userFriendlyMessage", ErrorMessageIds.Err_Default);
                throw new SystemException(ErrorMessageIds.Err_InvalidOperationExceptionForSingleorDefault, ex);
            }
        }

        /// <summary>
        /// Fetch the Route log selection type.
        /// </summary>
        /// <returns>List</returns>
        public List<ReferenceDataDTO> RouteLogSelectionType()
        {
            List<ReferenceDataDTO> lstReferenceDt = null;
            var query = DataContext.ReferenceDataCategories.Include(m => m.ReferenceDatas)
                .AsNoTracking().SingleOrDefault(n => n.CategoryName.Equals(
                    "UI_RouteLogSearch_SelectionType",
                    StringComparison.OrdinalIgnoreCase));
            if (query?.ReferenceDatas != null && query.ReferenceDatas.Count > 0)
            {
                lstReferenceDt = GenericMapper.MapList<ReferenceData, ReferenceDataDTO>(query.ReferenceDatas.ToList());
            }

            return lstReferenceDt;
        }

        /// <summary>
        /// Gets all reference category list along with associated reference data.
        /// </summary>
        /// <returns>List<ReferenceDataCategoryDTO></returns>
        public List<ReferenceDataCategoryDTO> GetAllReferenceCategoryList()
        {
            Guid statusId = Guid.Empty;
            var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).ToList();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ReferenceDataCategory, ReferenceDataCategoryDTO>();
                cfg.CreateMap<ReferenceData, ReferenceDataDTO>();
            });

            Mapper.Configuration.CreateMapper();
            List<ReferenceDataCategoryDTO> referenceDataCategoryListDto = Mapper.Map<List<ReferenceDataCategory>, List<ReferenceDataCategoryDTO>>(referenceDataCategories);
            return referenceDataCategoryListDto;
        }

        /// <summary>
        /// Gets the name of the reference data categories by category.
        /// </summary>
        /// <param name="categoryNames">The category names.</param>
        /// <returns>List ReferenceDataCategoryDTO</returns>
        public List<ReferenceDataCategoryDTO> GetReferenceDataCategoriesByCategoryNames(List<string> categoryNames)
        {
            var referenceDataCategories = DataContext.ReferenceDataCategories.AsNoTracking().Include(m => m.ReferenceDatas).Where(m => categoryNames.Contains(m.CategoryName.Trim())).ToList();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<ReferenceDataCategory, ReferenceDataCategoryDTO>();
                cfg.CreateMap<ReferenceData, ReferenceDataDTO>();
            });

            Mapper.Configuration.CreateMapper();
            List<ReferenceDataCategoryDTO> referenceDataCategoryListDto = Mapper.Map<List<ReferenceDataCategory>, List<ReferenceDataCategoryDTO>>(referenceDataCategories);
            return referenceDataCategoryListDto;
        }

    }
}