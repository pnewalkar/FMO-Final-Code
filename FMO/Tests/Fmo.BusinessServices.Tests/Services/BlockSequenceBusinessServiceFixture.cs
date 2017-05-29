using System;
using System.Collections.Generic;
using Fmo.BusinessServices.Interfaces;
using Fmo.BusinessServices.Services;
using Fmo.Common.Constants;
using Fmo.Common.TestSupport;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Moq;
using NUnit.Framework;

namespace Fmo.BusinessServices.Tests.Services
{
    [TestFixture]
    public class BlockSequenceBusinessServiceFixture : TestFixtureBase
    {
        private IBlockSequenceBusinessService testCandidate;
        private Mock<IBlockSequenceRepository> mockBlockSequenceRepository;
        private Mock<IReferenceDataBusinessService> referenceDataCategoryBusinessServiceMock;

        [Test]
        public void TestFetchDeliveryUnitForUser()
        {
            bool result = testCandidate.CreateBlockSequenceForDeliveryPoint(new Guid("A107065B-B91E-E711-9F8C-28D244AEF9ED"), new Guid("A117065B-B91E-E711-9F8C-28D244AEF9ED"));
            Assert.IsNotNull(result);
            Assert.IsTrue(result);
        }

        protected override void OnSetup()
        {
            List<ReferenceDataCategoryDTO> refDataCategotyDTO = new List<ReferenceDataCategoryDTO>()
            {
                new ReferenceDataCategoryDTO()
                {
                CategoryName = ReferenceDataCategoryNames.OperationalObjectType,
                ReferenceDatas = new List<ReferenceDataDTO>()
                    {
                        new ReferenceDataDTO()
                        {
                            ReferenceDataName = ReferenceDataValues.OperationalObjectTypeDP,
                            ReferenceDataValue = ReferenceDataValues.OperationalObjectTypeDP,
                            ID = Guid.Parse("415c9129-0615-457e-98b7-3a60436320c5")
                        }
                    }
                }
            };
            mockBlockSequenceRepository = new Mock<IBlockSequenceRepository>();
            mockBlockSequenceRepository.Setup(x => x.AddBlockSequence(It.IsAny<BlockSequenceDTO>(), It.IsAny<Guid>())).Returns(true);
            referenceDataCategoryBusinessServiceMock = new Mock<IReferenceDataBusinessService>();
            referenceDataCategoryBusinessServiceMock.Setup(x => x.GetReferenceDataCategoriesByCategoryNames(It.IsAny<List<string>>())).Returns(refDataCategotyDTO);
            testCandidate = new BlockSequenceBusinessService(mockBlockSequenceRepository.Object, referenceDataCategoryBusinessServiceMock.Object);
        }
    }
}