using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
using RM.CommonLibrary.EntityFramework.DTO;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Implementation;
using RM.DataManagement.UnitManager.WebAPI.BusinessService.Interface;

namespace RM.Data.UnitManager.WebAPI.Test
{
    // TODO: Fix test methods - SP5 Data Model Changes

    //[TestFixture]
    //public class UnitLocationBusinessServiceFixture : TestFixtureBase
    //{
    //    private Mock<IUnitLocationDataService> mockUnitLocationDataService;
    //    private Mock<IPostcodeSectorDataService> mockPostCodeSectorDataService;
    //    private Mock<IPostcodeDataService> mockPostCodeDataService;
    //    private Mock<IScenarioDataService> mockScenarioDataService;
    //    private UnitLocationDTO actualDeliveryUnitResult = null;
    //    private List<UnitLocationDTO> actualDeliveryUnitListForUserResult = null;
    //    private Guid deliveryUnitID = System.Guid.NewGuid();
    //    private Guid userID = System.Guid.NewGuid();
    //    private IUnitLocationBusinessService testCandidate;
    //    private Mock<ILoggingHelper> loggingHelperMock;

    //    [Test]
    //    public void Test_FetchDeliveryUnitForUser()
    //    {
    //        List<UnitLocationDTO> expectedDeliveryUnitListResult = testCandidate.GetDeliveryUnitsForUser(userID);
    //        Assert.NotNull(expectedDeliveryUnitListResult);
    //        Assert.NotNull(expectedDeliveryUnitListResult[0].BoundingBox);
    //        Assert.NotNull(expectedDeliveryUnitListResult[0].BoundingBoxCenter);
    //        Assert.NotNull(expectedDeliveryUnitListResult[0].UnitBoundryPolygon);
    //        Assert.AreEqual(expectedDeliveryUnitListResult, actualDeliveryUnitListForUserResult);
    //    }

    //    [Test]
    //    public void Test_GetPostCodeSectorByUDPRN()
    //    {
    //        var result = testCandidate.GetPostCodeSectorByUDPRN(12345);
    //        Assert.NotNull(result);
    //    }

    //    [Test]
    //    public void Test_FetchPostCodeUnitForBasicSearch()
    //    {
    //        var result = testCandidate.FetchPostCodeUnitForBasicSearch("abc", Guid.NewGuid());
    //        Assert.NotNull(result);
    //    }

    //    [Test]
    //    public void Test_GetPostCodeUnitCount()
    //    {
    //        var result = testCandidate.GetPostCodeUnitCount("abc", Guid.NewGuid());
    //        Assert.NotNull(result);
    //        Assert.AreEqual(result.Result, 5);
    //    }

    //    [Test]
    //    public void Test_FetchPostCodeUnitForAdvanceSearch()
    //    {
    //        var result = testCandidate.FetchPostCodeUnitForAdvanceSearch("abc", Guid.NewGuid());
    //        Assert.NotNull(result);
    //    }

    //    [Test]
    //    public void Test_GetPostCodeID()
    //    {
    //        var result = testCandidate.GetPostCodeID("123");
    //        Assert.NotNull(result);
    //        Assert.AreEqual(result.Result, System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050"));
    //    }

    //    [Test]
    //    public void Test_FetchDeliveryScenario()
    //    {
    //        var result = testCandidate.FetchDeliveryScenario(Guid.NewGuid(), Guid.NewGuid());
    //        Assert.NotNull(result);
    //    }

    //    protected override void OnSetup()
    //    {
    //        mockUnitLocationDataService = CreateMock<IUnitLocationDataService>();
    //        mockPostCodeSectorDataService = CreateMock<IPostcodeSectorDataService>();
    //        mockPostCodeDataService = CreateMock<IPostcodeDataService>();
    //        mockScenarioDataService = CreateMock<IScenarioDataService>();
    //        loggingHelperMock = CreateMock<ILoggingHelper>();

    //        userID = System.Guid.Parse("A867065B-B91E-E711-9F8C-28D244AEF9ED");
    //        deliveryUnitID = System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050");
    //        var unitBoundary = DbGeometry.PolygonFromText("POLYGON ((505058.162109375 100281.69677734375, 518986.84887695312 100281.69677734375, 518986.84887695312 114158.546875, 505058.162109375 114158.546875, 505058.162109375 100281.69677734375))", 27700);
    //        actualDeliveryUnitListForUserResult = new List<UnitLocationDTO>() { new UnitLocationDTO() { ID = Guid.NewGuid(), ExternalId = "DI0001", UnitAddressUDPRN = 1, UnitName = "UnitOne", UnitBoundryPolygon = unitBoundary } };
    //        mockUnitLocationDataService = CreateMock<IUnitLocationDataService>();
    //        mockUnitLocationDataService.Setup(n => n.FetchDeliveryUnitsForUser(userID)).Returns(actualDeliveryUnitListForUserResult);

    //        actualDeliveryUnitResult = new UnitLocationDTO() { ID = Guid.NewGuid(), ExternalId = "DI0001", UnitAddressUDPRN = 1, UnitName = "UnitOne" };
    //        mockUnitLocationDataService.Setup(n => n.FetchDeliveryUnit(deliveryUnitID)).Returns(actualDeliveryUnitResult);

    //        mockPostCodeSectorDataService.Setup(x => x.GetPostCodeSectorByUDPRN(It.IsAny<int>())).ReturnsAsync(new PostCodeSectorDTO() { });

    //        mockPostCodeDataService.Setup(x => x.FetchPostCodeUnitForBasicSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<PostCodeDTO>() { });
    //        mockPostCodeDataService.Setup(x => x.GetPostCodeUnitCount(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(5);
    //        mockPostCodeDataService.Setup(x => x.FetchPostCodeUnitForAdvanceSearch(It.IsAny<string>(), It.IsAny<Guid>())).ReturnsAsync(new List<PostCodeDTO>() { });
    //        mockPostCodeDataService.Setup(x => x.GetPostCodeID(It.IsAny<string>())).ReturnsAsync(System.Guid.Parse("B51AA229-C984-4CA6-9C12-510187B81050"));

    //        mockScenarioDataService.Setup(x => x.FetchScenario(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(new List<ScenarioDTO>());

    //        var rmTraceManagerMock = new Mock<IRMTraceManager>();
    //        rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
    //        loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

    //        testCandidate = new UnitLocationBusinessService(mockUnitLocationDataService.Object, mockPostCodeSectorDataService.Object, mockPostCodeDataService.Object, mockScenarioDataService.Object, loggingHelperMock.Object);

    //        SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);
    //    }
    //}
}