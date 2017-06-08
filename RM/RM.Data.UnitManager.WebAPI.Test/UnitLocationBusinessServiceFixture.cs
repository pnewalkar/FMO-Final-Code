﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Implementation;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;

namespace RM.Data.UnitManager.WebAPI.Test
{
    [TestFixture]
    public class UnitLocationBusinessServiceFixture : TestFixtureBase
    {
        private Mock<IUnitLocationDataService> mockUnitLocationRepository;
        private Mock<IPostCodeSectorDataService> mockPostCodeSectorDataService;
        private Mock<IPostCodeDataService> mockPostCodeDataService;
        private Mock<IScenarioDataService> mockIScenarioDataService;
        private UnitLocationDTO actualDeliveryUnitResult = null;
        private List<UnitLocationDTO> actualDeliveryUnitListForUserResult = null;
        private Guid deliveryUnitID = System.Guid.NewGuid();
        private Guid userID = System.Guid.NewGuid();
        private IUnitLocationBusinessService testCandidate;

        [Test]
        public void TestFetchDeliveryUnitForUser()
        {
            List<UnitLocationDTO> expectedDeliveryUnitListResult = testCandidate.FetchDeliveryUnitsForUser(userID);
            Assert.NotNull(expectedDeliveryUnitListResult);
            Assert.NotNull(expectedDeliveryUnitListResult[0].BoundingBox);
            Assert.NotNull(expectedDeliveryUnitListResult[0].BoundingBoxCenter);
            Assert.NotNull(expectedDeliveryUnitListResult[0].UnitBoundaryGeoJSONData);
            Assert.AreEqual(expectedDeliveryUnitListResult, actualDeliveryUnitListForUserResult);
        }

        [Test]
        public void TestFetchDeliveryUnit()
        {
            //  UnitLocationDTO expectedDeliveryUnitResult = testCandidate.FetchDeliveryUnit(Guid.NewGuid());
            //   Assert.NotNull(expectedDeliveryUnitResult);
            // Assert.AreEqual(expectedDeliveryUnitResult, actualDeliveryUnitResult);
        }

        protected override void OnSetup()
        {
            mockUnitLocationRepository = CreateMock<IUnitLocationDataService>();
            mockPostCodeSectorDataService = CreateMock<IPostCodeSectorDataService>();
            mockPostCodeDataService = CreateMock<IPostCodeDataService>();
            mockIScenarioDataService = CreateMock<IScenarioDataService>();

            userID = System.Guid.Parse("A867065B-B91E-E711-9F8C-28D244AEF9ED");
            deliveryUnitID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
            var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
            actualDeliveryUnitListForUserResult = new List<UnitLocationDTO>() { new UnitLocationDTO() { ID = Guid.NewGuid(), ExternalId = "DI0001", UnitAddressUDPRN = 1, UnitName = "UnitOne", UnitBoundryPolygon = unitBoundary } };
            mockUnitLocationRepository = CreateMock<IUnitLocationDataService>();
            mockUnitLocationRepository.Setup(n => n.FetchDeliveryUnitsForUser(userID)).Returns(actualDeliveryUnitListForUserResult);

            actualDeliveryUnitResult = new UnitLocationDTO() { ID = Guid.NewGuid(), ExternalId = "DI0001", UnitAddressUDPRN = 1, UnitName = "UnitOne" };
            mockUnitLocationRepository.Setup(n => n.FetchDeliveryUnit(deliveryUnitID)).Returns(actualDeliveryUnitResult);

            testCandidate = new UnitLocationBusinessService(mockUnitLocationRepository.Object, mockPostCodeSectorDataService.Object, mockPostCodeDataService.Object, mockIScenarioDataService.Object);

            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}