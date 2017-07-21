using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.Data.UnitManager.WebAPI.DataDTO;
using RM.Data.UnitManager.WebAPI.DTO;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Implementation;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Integration.Interface;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;
using RM.DataManagement.UnitManager.WebAPI.DataDTO;
using RM.DataManagement.UnitManager.WebAPI.DataService.Interfaces;
using RM.DataManagement.UnitManager.WebAPI.DTO;

namespace RM.Data.UnitManager.WebAPI.Test
{
    /// <summary>
    /// This class contains test methods for UnitLocationBusinessService
    /// </summary>
    [TestFixture]
    public class UnitLocationBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IUnitLocationDataService> mockUnitLocationDataService;
        private Mock<IPostcodeSectorDataService> mockPostCodeSectorDataService;
        private Mock<IPostcodeDataService> mockPostCodeDataService;
        private Mock<IScenarioDataService> mockScenarioDataService;
        private Mock<IUnitManagerIntegrationService> mockUnitManagerIntegrationService;
        private Mock<IPostalAddressDataService> mockPostalAddressDataService;
        private Mock<ILoggingHelper> loggingHelperMock;

        private UnitLocationDTO actualDeliveryUnitResult = null;
        private List<UnitLocationDTO> actualDeliveryUnitListForUserResult = null;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid userID = System.Guid.NewGuid();
        private IUnitLocationBusinessService testCandidate;

        /// <summary>
        /// Test for getting delivery unit
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_FetchDeliveryUnitForUser()
        {
            string currentUserUnitType = "Delivery Office";
            var result = await testCandidate.GetUnitsByUser(userID, currentUserUnitType);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Select(x => x.Area).First(), "Polygon");
            Assert.AreEqual(result.Select(x => x.UnitName).First(), "UnitOne");
        }

        /// <summary>
        /// Test for getting postcode sector by passing UDPRN
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_GetPostCodeSectorByUDPRN()
        {
            var result = await testCandidate.GetPostcodeSectorByUdprn(12345);
            Assert.NotNull(result);
            Assert.AreEqual(result.District, null);
            Assert.AreEqual(result.Sector, null);
        }

        /// <summary>
        /// Test for getting PostcodeUnit for basic search
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Test_FetchPostCodeUnitForBasicSearch()
        {
            var result = await testCandidate.GetPostcodeUnitForBasicSearch("abc", Guid.NewGuid());
            Assert.NotNull(result);
            Assert.AreEqual(result.ToList()[0].PostcodeUnit, "Unit1");
        }

        /// <summary>
        ///Test for getting count for Postcodeunit
        /// </summary>
        [Test]
        public void Test_GetPostCodeUnitCount()
        {
            var result = testCandidate.GetPostcodeUnitCount("abc", Guid.NewGuid());
            Assert.NotNull(result.Result);
            Assert.AreEqual(result.Result, 5);
        }

        [Test]
        public void Test_FetchPostCodeUnitForAdvanceSearch()
        {
            var result = testCandidate.GetPostcodeUnitForAdvanceSearch("abc", Guid.NewGuid());
            Assert.NotNull(result.Result);
        }

        [Test]
        public void Test_GetPostCodeID()
        {
            var result = testCandidate.GetPostcodeID("123");
            Assert.NotNull(result.Result);
            Assert.AreEqual(result.Result, new Guid("B51AA229-C984-4CA6-9C12-510187B81050"));
        }

        [Test]
        public void Test_GetRouteScenarios()
        {
            var result = testCandidate.GetRouteScenarios(Guid.NewGuid(), Guid.NewGuid());
            Assert.NotNull(result.Result);
        }

        [Test]
        public void Test_GetPostcodes()
        {
            var result = testCandidate.GetPostcodes(new List<Guid> { Guid.NewGuid() });
            Assert.NotNull(result.Result);
        }

        [Test]
        public void Test_GetApproxLocation()
        {
            var result = testCandidate.GetApproxLocation("123", Guid.NewGuid());
            Assert.NotNull(result.Result);
        }

        [Test]
        public void Test_GetPostalAddressDetails()
        {
            var result = testCandidate.GetPostalAddressDetails("12,MG", Guid.NewGuid());
            Assert.NotNull(result.Result);
            Assert.AreEqual(result.Result.BuildingName, "bldg1");
            Assert.AreEqual(result.Result.BuildingNumber, 1);
            Assert.AreEqual(result.Result.SubBuildingName, "subbldg");
        }

        [Test]
        public void Test_GetPostalAddressSearchDetails()
        {
            var result = testCandidate.GetPostalAddressSearchDetails("12,MG", Guid.NewGuid());
            Assert.NotNull(result.Result);
            Assert.AreEqual(result.Result.Count, 1);
            Assert.AreEqual(result.Result[0], "123");
        }

        protected override void OnSetup()
        {
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            mockUnitLocationDataService = CreateMock<IUnitLocationDataService>();
            mockPostCodeSectorDataService = CreateMock<IPostcodeSectorDataService>();
            mockPostCodeDataService = CreateMock<IPostcodeDataService>();
            mockScenarioDataService = CreateMock<IScenarioDataService>();
            loggingHelperMock = CreateMock<ILoggingHelper>();
            mockUnitLocationDataService = CreateMock<IUnitLocationDataService>();
            mockUnitManagerIntegrationService = CreateMock<IUnitManagerIntegrationService>();
            mockPostalAddressDataService = CreateMock<IPostalAddressDataService>();

            userID = System.Guid.Parse("A867065B-B91E-E711-9F8C-28D244AEF9ED");
            deliveryUnitID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            actualDeliveryUnitListForUserResult = new List<UnitLocationDTO>() { new UnitLocationDTO() { ID = Guid.NewGuid(), UnitName = "UnitOne", UnitBoundryPolygon = unitBoundary } };
            List<UnitLocationDataDTO> unitLocationDataDTOList = new List<UnitLocationDataDTO>()
            {
                new UnitLocationDataDTO()
                {
                    LocationId = Guid.NewGuid(), Name = "UnitOne", Area = "Polygon", Shape = unitBoundary
                }
            };
            CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO referenceDataCategoryDTO = new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO()
            {
                CategoryName = "PostalAddressType",
                ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>()
                {
                    new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                    { ReferenceDataValue = PostCodeTypeCategory.PostcodeSector.GetDescription()
                    },
                    new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO()
                    { ReferenceDataValue = PostCodeTypeCategory.PostcodeDistrict.GetDescription()
                    }
                }
            };
            PostcodeSectorDataDTO postcodeSectorDataDTO = new PostcodeSectorDataDTO() { };
            List<PostcodeDataDTO> postcodeDataDTOList = new List<PostcodeDataDTO>() { new PostcodeDataDTO() { PostcodeUnit = "Unit1", ID = new Guid("B51AA229-C984-4CA6-9C12-510187B81050") } };
            List<ScenarioDataDTO> scenarioDataDTOList = new List<ScenarioDataDTO>() { new ScenarioDataDTO() { } };
            List<PostalAddressDataDTO> PostalAddressDataServiceList = new List<PostalAddressDataDTO>() { new PostalAddressDataDTO() { AddressType_GUID = new Guid("A867065B-B91E-E711-9F8C-28D244AEF9EC"), BuildingName = "bldg1", BuildingNumber = 1, SubBuildingName = "subbldg" } };
            List<DeliveryRouteDTO> deliveryRouteDTOList = new List<DeliveryRouteDTO>() { };

            mockUnitManagerIntegrationService.Setup(x => x.GetReferenceDataGuId(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new Guid("A867065B-B91E-E711-9F8C-28D244AEF9EC"));
            mockUnitLocationDataService.Setup(x => x.GetUnitsByUser(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(unitLocationDataDTOList);
            mockUnitManagerIntegrationService.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<string>())).ReturnsAsync(referenceDataCategoryDTO);
            mockPostCodeSectorDataService.Setup(x => x.GetPostcodeSectorByUdprn(It.IsAny<int>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(postcodeSectorDataDTO);
            mockPostCodeDataService.Setup(x => x.GetPostcodeUnitForBasicSearch(It.IsAny<SearchInputDataDto>())).ReturnsAsync(postcodeDataDTOList);
            mockPostCodeDataService.Setup(x => x.GetPostcodeUnitCount(It.IsAny<SearchInputDataDto>())).ReturnsAsync(5);
            mockPostCodeDataService.Setup(x => x.GetPostcodeUnitForAdvanceSearch(It.IsAny<SearchInputDataDto>())).ReturnsAsync(postcodeDataDTOList);
            mockPostCodeDataService.Setup(x => x.GetPostcodeID(It.IsAny<string>())).ReturnsAsync(postcodeDataDTOList[0]);
            mockScenarioDataService.Setup(x => x.GetScenariosByOperationStateAndDeliveryUnit(It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(scenarioDataDTOList);
            mockUnitLocationDataService.Setup(x => x.GetPostcodes(It.IsAny<List<Guid>>(), It.IsAny<Guid>())).ReturnsAsync(postcodeDataDTOList);
            mockPostCodeDataService.Setup(x => x.GetApproxLocation(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(unitBoundary);
            mockPostalAddressDataService.Setup(x => x.GetPostalAddressDetails(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(PostalAddressDataServiceList);
            mockUnitManagerIntegrationService.Setup(x => x.GetRouteData(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(deliveryRouteDTOList);
            mockPostalAddressDataService.Setup(x => x.GetPostalAddressSearchDetails(It.IsAny<string>(), It.IsAny<Guid>(), It.IsAny<List<Guid>>())).ReturnsAsync(new List<string>() { "123" });

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new UnitLocationBusinessService(mockUnitLocationDataService.Object, mockPostCodeSectorDataService.Object, mockPostCodeDataService.Object, mockScenarioDataService.Object, mockPostalAddressDataService.Object, loggingHelperMock.Object, mockUnitManagerIntegrationService.Object);
        }
    }
}