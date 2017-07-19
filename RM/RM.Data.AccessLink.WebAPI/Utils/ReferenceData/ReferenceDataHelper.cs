using System.Collections.Generic;
using System.Linq;

using RM.CommonLibrary.EntityFramework.DTO.ReferenceData;
using RM.DataManagement.AccessLink.WebAPI.DTOs;

namespace RM.CommonLibrary.EntityFramework.Utilities.ReferenceData
{
    public class ReferenceDataHelper
    {
        public static List<ReferenceDataCategoryDTO> MapDTO(List<NameValuePair> nameValuePairs)
        {
            List<ReferenceDataCategoryDTO> referenceDataCategoryDTOList = new List<ReferenceDataCategoryDTO>();
            foreach (var nameValuePair in nameValuePairs.GroupBy(x => x.Group))
            {
                ReferenceDataCategoryDTO referenceDataCategoryDTO = new ReferenceDataCategoryDTO();
                referenceDataCategoryDTO.CategoryName = nameValuePair.Key;
                referenceDataCategoryDTO.ReferenceDatas = new List<ReferenceDataDTO>();
                foreach (var item in nameValuePair)
                {
                    ReferenceDataDTO referenceDataDTO = new ReferenceDataDTO();
                    referenceDataDTO.DisplayText = item.DisplayText;
                    referenceDataDTO.DataDescription = item.Description;
                    referenceDataDTO.ID = item.Id;
                    referenceDataDTO.ReferenceDataName = item.Name;
                    referenceDataDTO.ReferenceDataValue = item.Value;

                    referenceDataCategoryDTO.ReferenceDatas.Add(referenceDataDTO);
                }

                referenceDataCategoryDTOList.Add(referenceDataCategoryDTO);
            }

            return referenceDataCategoryDTOList;
        }

        public static ReferenceDataCategoryDTO MapDTO(SimpleListDTO simpleList)
        {
            ReferenceDataCategoryDTO referenceDataCategoryDTO = new ReferenceDataCategoryDTO();
            referenceDataCategoryDTO.Id = simpleList.Id;
            referenceDataCategoryDTO.CategoryName = simpleList.ListName;
            referenceDataCategoryDTO.ReferenceDatas = new List<ReferenceDataDTO>();
            foreach (var item in simpleList.ListItems)
            {
                ReferenceDataDTO referenceDataDTO = new ReferenceDataDTO();
                referenceDataDTO.DisplayText = item.DisplayText;
                referenceDataDTO.DataDescription = item.Description;
                referenceDataDTO.ID = item.Id;
                referenceDataDTO.ReferenceDataName = item.Name;
                referenceDataDTO.ReferenceDataValue = item.Value;

                referenceDataCategoryDTO.ReferenceDatas.Add(referenceDataDTO);
            }

            return referenceDataCategoryDTO;
        }

        public static List<ReferenceDataCategoryDTO> MapDTO(List<SimpleListDTO> simpleLists)
        {
            List<ReferenceDataCategoryDTO> referenceDataCategoryDTOList = new List<ReferenceDataCategoryDTO>();
            foreach (var simpleList in simpleLists)
            {
                ReferenceDataCategoryDTO referenceDataCategoryDTO = new ReferenceDataCategoryDTO();
                referenceDataCategoryDTO.CategoryName = simpleList.ListName;
                referenceDataCategoryDTO.ReferenceDatas = new List<ReferenceDataDTO>();
                foreach (var item in simpleList.ListItems)
                {
                    ReferenceDataDTO referenceDataDTO = new ReferenceDataDTO();
                    referenceDataDTO.DisplayText = item.DisplayText;
                    referenceDataDTO.DataDescription = item.Description;
                    referenceDataDTO.ID = item.Id;
                    referenceDataDTO.ReferenceDataName = item.Name;
                    referenceDataDTO.ReferenceDataValue = item.Value;

                    referenceDataCategoryDTO.ReferenceDatas.Add(referenceDataDTO);
                }

                referenceDataCategoryDTOList.Add(referenceDataCategoryDTO);
            }

            return referenceDataCategoryDTOList;
        }
    }
}