using System;
using System.Collections.Generic;
using System.Linq;
using Fmo.BusinessServices.Interfaces;
using Fmo.Common.Constants;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;

namespace Fmo.BusinessServices.Services
{
    public class BlockSequenceBusinessService : IBlockSequenceBusinessService
    {
        private IBlockSequenceRepository blockSequenceRepository;
        private IReferenceDataBusinessService referenceDataBusinessService;

        public BlockSequenceBusinessService(IBlockSequenceRepository blockSequenceRepository, IReferenceDataBusinessService referenceDataBusinessService)
        {
            this.blockSequenceRepository = blockSequenceRepository;
            this.referenceDataBusinessService = referenceDataBusinessService;
        }

        /// <summary>
        /// Method to create block sequence for delivery point
        /// </summary>
        /// <param name="deliveryRouteId">Route ID</param>
        /// <param name="deliveryPointId">deliveryPointId</param>
        public void CreateBlockSequenceForDeliveryPoint(Guid deliveryRouteId, Guid deliveryPointId)
        {
            List<string> categoryNames = new List<string> { ReferenceDataCategoryNames.OperationalObjectType };
            var referenceDataCategoryList = referenceDataBusinessService.GetReferenceDataCategoriesByCategoryNames(categoryNames);
            Guid operationalObjectTypeForDp = referenceDataCategoryList
              .Where(x => x.CategoryName == ReferenceDataCategoryNames.OperationalObjectType)
              .SelectMany(x => x.ReferenceDatas)
              .Where(x => x.ReferenceDataValue == ReferenceDataValues.OperationalObjectTypeDP).Select(x => x.ID)
              .SingleOrDefault();
            BlockSequenceDTO blockSequenceDTO = new BlockSequenceDTO { ID = Guid.NewGuid(), OperationalObjectType_GUID = operationalObjectTypeForDp, OperationalObject_GUID = deliveryPointId };
            blockSequenceRepository.AddBlockSequence(blockSequenceDTO, deliveryRouteId);
        }
    }
}