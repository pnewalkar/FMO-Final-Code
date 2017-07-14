using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Moq;
using NUnit.Framework;
using RM.Common.ActionManager.WebAPI.DataDTO;
using RM.Common.ActionManager.WebAPI.DataService;
using RM.Common.ActionManager.WebAPI.Entity;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;

namespace RM.Common.ActionManager.WebAPI.Test.DataService
{
    [TestFixture]
    public class ActionManagerDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<ActionDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<ActionDBContext>> mockDatabaseFactory;
        private IActionManagerDataService testCandidate;
        private Mock<ILoggingHelper> mockLoggingHelper;
        private List<UserUnitInfoDataDTO> userUnitInfoDataDTOList;

        [Test]
        public void Test_GetRoleBasedAccessFunctions_ManagerRole()
        {
            var result = testCandidate.GetRoleBasedAccessFunctions(userUnitInfoDataDTOList[0]);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.Count, 2);
            Assert.AreEqual(result.Result[0].UserName, "user1");
            Assert.AreEqual(result.Result[1].UserName, "user1");
        }

        [Test]
        public void Test_GetRoleBasedAccessFunctions_DeliveryRole()
        {
            var result = testCandidate.GetRoleBasedAccessFunctions(userUnitInfoDataDTOList[1]);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.Count, 1);
            Assert.AreEqual(result.Result[0].UserName, "user2");
        }

        [Test]
        public void Test_GetUserUnitInfo_FirstRequest_With_EmptyLocationId()
        {
            var result = testCandidate.GetUserUnitInfo("deliveryuser1", Guid.Empty);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.UnitName, "Unit1");
            Assert.AreEqual(result.Result.UnitType, "Type1");
            Assert.AreEqual(result.Result.LocationId, new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"));
        }

        [Test]
        public void Test_GetUserUnitInfo_Not_FirstRequest_With_LocationId()
        {
            var result = testCandidate.GetUserUnitInfo("user1", new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"));
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.UnitName, "Unit1");
            Assert.AreEqual(result.Result.UnitType, "Type1");
            Assert.AreEqual(result.Result.LocationId, new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"));
        }

        [Test]
        public void Test_GetUserUnitInfoFromReferenceData_FirstRequest_With_EmptyLocationId()
        {
            var result = testCandidate.GetUserUnitInfoFromReferenceData("user1", Guid.Empty);
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.UnitName, "Type1");
            Assert.AreEqual(result.Result.UnitType, "Type1");
            Assert.AreEqual(result.Result.LocationId, new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"));
        }

        [Test]
        public void Test_GetUserUnitInfoFromReferenceData_Not_FirstRequest_With_LocationId()
        {
            var result = testCandidate.GetUserUnitInfoFromReferenceData("user1", new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"));
            Assert.IsNotNull(result.Result);
            Assert.AreEqual(result.Result.UnitName, "Type1");
            Assert.AreEqual(result.Result.UnitType, "Type1");
            Assert.AreEqual(result.Result.LocationId, new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"));
        }

        protected override void OnSetup()
        {
            mockLoggingHelper = CreateMock<ILoggingHelper>();

            // Data Setup
            userUnitInfoDataDTOList = new List<UserUnitInfoDataDTO>()
            {
                new UserUnitInfoDataDTO()
                {
                    UserName = "user1",
                    LocationId = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A")
                },

                new UserUnitInfoDataDTO()
                {
                    UserName = "user2",
                    LocationId = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A")
                }
            };

            List<AccessFunction> accessFunctionList = new List<AccessFunction>()
            {
                new AccessFunction()
                {
                    RoleName = "manager",
                    LocationID = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                    UserName = "user1",
                    FunctionName = "PrintMap",
                    ActionName = "Print",
                    UserId = new Guid("5B9C7207-3D20-E711-9F8C-28D244AEF9ED"),
                },

                new AccessFunction()
                {
                    RoleName = "collection hub",
                    LocationID = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                    UserName = "user1",
                    FunctionName = "accesslink",
                    ActionName = "create",
                    UserId = new Guid("5B9C7207-3D20-E711-9F8C-28D244AEF9ED"),
                },

                new AccessFunction()
                {
                    RoleName = "delivery unit",
                    LocationID = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                    UserName = "user2",
                    FunctionName = "delivery point",
                    ActionName = "create",
                    UserId = new Guid("6B9C7207-3D20-E711-9F8C-28D244AEF9ED"),
                }
            };

            UserRoleLocation userRoleLocation = new UserRoleLocation()
            {
                LocationID = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                UserID = new Guid("5B9C7207-3D20-E711-9F8C-28D244AEF9ED")
            };

            User user = new User()
            {
                ID = new Guid("5B9C7207-3D20-E711-9F8C-28D244AEF9ED"),
                UserName = "user1"
            };

            LocationReferenceData locationReferenceData = new LocationReferenceData()
            {
                ReferenceDataID = new Guid("AFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                LocationID = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A")
            };

            ReferenceData referenceData = new ReferenceData()
            {
                ID = new Guid("AFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                ReferenceDataValue = "Type1"
            };

            PostalAddressIdentifier postalAddressIdentifier = new PostalAddressIdentifier()
            {
                ID = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                IdentifierTypeGUID = new Guid("AFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                Name = "Unit1"
            };

            //Set up for UserRoleLocation
            mockRMDBContext = CreateMock<ActionDBContext>();
            var mockAsynEnumerable1 = new DbAsyncEnumerable<UserRoleLocation>(new List<UserRoleLocation>() { userRoleLocation });
            var mockUserRoleLocation = MockDbSet(new List<UserRoleLocation>() { userRoleLocation });
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockUserRoleLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UserRoleLocation>)mockAsynEnumerable1).GetAsyncEnumerator());

            mockUserRoleLocation.Setup(x => x.AsNoTracking()).Returns(mockUserRoleLocation.Object);

            mockRMDBContext.Setup(x => x.Set<UserRoleLocation>()).Returns(mockUserRoleLocation.Object);
            mockRMDBContext.Setup(x => x.UserRoleLocations).Returns(mockUserRoleLocation.Object);
            mockUserRoleLocation.Setup(x => x.Include(It.IsAny<string>())).Returns(mockUserRoleLocation.Object);

            //Set up for User
            var mockAsynEnumerable2 = new DbAsyncEnumerable<User>(new List<User>() { user });
            var mockUser = MockDbSet(new List<User>() { user });
            mockUser.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable2.AsQueryable().Provider);
            mockUser.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable2.AsQueryable().Expression);
            mockUser.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable2.AsQueryable().ElementType);
            mockUser.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<User>)mockAsynEnumerable2).GetAsyncEnumerator());

            mockRMDBContext.Setup(x => x.Set<User>()).Returns(mockUser.Object);
            mockRMDBContext.Setup(x => x.Users).Returns(mockUser.Object);
            mockUser.Setup(x => x.Include(It.IsAny<string>())).Returns(mockUser.Object);

            //Set up for LocationReferenceData
            var mockAsynEnumerable3 = new DbAsyncEnumerable<LocationReferenceData>(new List<LocationReferenceData>() { locationReferenceData });
            var mockLocationReferenceData = MockDbSet(new List<LocationReferenceData>() { locationReferenceData });
            mockLocationReferenceData.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable3.AsQueryable().Provider);
            mockLocationReferenceData.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable3.AsQueryable().Expression);
            mockLocationReferenceData.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable3.AsQueryable().ElementType);
            mockLocationReferenceData.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<LocationReferenceData>)mockAsynEnumerable3).GetAsyncEnumerator());

            mockRMDBContext.Setup(x => x.Set<LocationReferenceData>()).Returns(mockLocationReferenceData.Object);
            mockRMDBContext.Setup(x => x.LocationReferenceDatas).Returns(mockLocationReferenceData.Object);
            mockLocationReferenceData.Setup(x => x.Include(It.IsAny<string>())).Returns(mockLocationReferenceData.Object);

            //Set up for LocationReferenceData
            var mockAsynEnumerable4 = new DbAsyncEnumerable<ReferenceData>(new List<ReferenceData>() { referenceData });
            var mockReferenceData = MockDbSet(new List<ReferenceData>() { referenceData });
            mockReferenceData.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable4.AsQueryable().Provider);
            mockReferenceData.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable4.AsQueryable().Expression);
            mockReferenceData.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable4.AsQueryable().ElementType);
            mockReferenceData.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<ReferenceData>)mockAsynEnumerable4).GetAsyncEnumerator());

            mockRMDBContext.Setup(x => x.Set<ReferenceData>()).Returns(mockReferenceData.Object);
            mockRMDBContext.Setup(x => x.ReferenceDatas).Returns(mockReferenceData.Object);
            mockReferenceData.Setup(x => x.Include(It.IsAny<string>())).Returns(mockReferenceData.Object);

            //Set up for PostalAddressIdentifier
            var mockAsynEnumerable5 = new DbAsyncEnumerable<PostalAddressIdentifier>(new List<PostalAddressIdentifier>() { postalAddressIdentifier });
            var mockPostalAddressIdentifier = MockDbSet(new List<PostalAddressIdentifier>() { postalAddressIdentifier });
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable5.AsQueryable().Provider);
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable5.AsQueryable().Expression);
            mockPostalAddressIdentifier.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable5.AsQueryable().ElementType);
            mockPostalAddressIdentifier.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<PostalAddressIdentifier>)mockAsynEnumerable5).GetAsyncEnumerator());

            mockRMDBContext.Setup(x => x.Set<PostalAddressIdentifier>()).Returns(mockPostalAddressIdentifier.Object);
            mockRMDBContext.Setup(x => x.PostalAddressIdentifiers).Returns(mockPostalAddressIdentifier.Object);
            mockPostalAddressIdentifier.Setup(x => x.Include(It.IsAny<string>())).Returns(mockPostalAddressIdentifier.Object);

            //Set up for AccessFunction
            var mockAsynEnumerable6 = new DbAsyncEnumerable<AccessFunction>(accessFunctionList);
            var mockAccessFunction = MockDbSet(accessFunctionList);
            mockAccessFunction.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable6.AsQueryable().Provider);
            mockAccessFunction.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable6.AsQueryable().Expression);
            mockAccessFunction.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable6.AsQueryable().ElementType);
            mockAccessFunction.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<AccessFunction>)mockAsynEnumerable6).GetAsyncEnumerator());

            mockAccessFunction.Setup(x => x.AsNoTracking()).Returns(mockAccessFunction.Object);

            mockRMDBContext.Setup(x => x.Set<AccessFunction>()).Returns(mockAccessFunction.Object);
            mockRMDBContext.Setup(x => x.AccessFunctions).Returns(mockAccessFunction.Object);
            mockAccessFunction.Setup(x => x.Include(It.IsAny<string>())).Returns(mockAccessFunction.Object);

            //Set up for Logging
            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<ActionDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new ActionManagerDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);
        }
    }
}