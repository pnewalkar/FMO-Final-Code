using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using RM.CommonLibrary.HelperMiddleware;
using RM.Common.ActionManager.WebAPI.DataDTO;
using RM.Common.ActionManager.WebAPI.Entity;
using Moq;
using RM.Common.ActionManager.WebAPI.Interfaces;
using RM.Common.ActionManager.WebAPI.DataService;
using RM.CommonLibrary.LoggingMiddleware;
using System.Data.Entity.Infrastructure;

namespace RM.Common.ActionManager.WebAPI.Test.DataService
{
    [TestFixture]
    public class ActionManagerDataServiceFixture : RepositoryFixtureBase
    {
        private Mock<ActionDBContext> mockRMDBContext;
        private Mock<IDatabaseFactory<ActionDBContext>> mockDatabaseFactory;
        private IActionManagerDataService testCandidate;
        private Mock<ILoggingHelper> mockLoggingHelper;

        [Test]
        public void Test_GetUserUnitInfo()
        {
            var result = testCandidate.GetUserUnitInfo("deliveryuser1", Guid.Empty);
            Assert.IsNotNull(result.Result);
        }
        protected override void OnSetup()
        {
            mockLoggingHelper = CreateMock<ILoggingHelper>();

            UserRoleLocation userRoleLocation = new UserRoleLocation()
            {
                LocationID = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                UserID = new Guid("5B9C7207-3D20-E711-9F8C-28D244AEF9ED")
            };

            User user = new User()
            {
                ID = new Guid("5B9C7207-3D20-E711-9F8C-28D244AEF9ED"),
                UserName = "deliveryuser1"
            };

            LocationReferenceData locationReferenceData = new LocationReferenceData()
            { };

            ReferenceData referenceData = new ReferenceData()
            {
                //ID = 
            };

            PostalAddressIdentifier postalAddressIdentifier = new PostalAddressIdentifier()
            {
                ID = new Guid("FFD741D9-5BBE-4D7F-9C3B-79D3588DC98A"),
                IdentifierTypeGUID = new Guid("FFD741D0-5BBE-4D7F-9C3B-79D3588DC98A")
            };

            //Set up for UserRoleLocation
            mockRMDBContext = CreateMock<ActionDBContext>();
            var mockAsynEnumerable1 = new DbAsyncEnumerable<UserRoleLocation>(new List<UserRoleLocation>() { userRoleLocation });
            var mockUserRoleLocation = MockDbSet(new List<UserRoleLocation>() { userRoleLocation });
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.Provider).Returns(mockAsynEnumerable1.AsQueryable().Provider);
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.Expression).Returns(mockAsynEnumerable1.AsQueryable().Expression);
            mockUserRoleLocation.As<IQueryable>().Setup(mock => mock.ElementType).Returns(mockAsynEnumerable1.AsQueryable().ElementType);
            mockUserRoleLocation.As<IDbAsyncEnumerable>().Setup(mock => mock.GetAsyncEnumerator()).Returns(((IDbAsyncEnumerable<UserRoleLocation>)mockAsynEnumerable1).GetAsyncEnumerator());
            
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


            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            mockLoggingHelper.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            mockDatabaseFactory = CreateMock<IDatabaseFactory<ActionDBContext>>();
            mockDatabaseFactory.Setup(x => x.Get()).Returns(mockRMDBContext.Object);
            testCandidate = new ActionManagerDataService(mockDatabaseFactory.Object, mockLoggingHelper.Object);


        }
    }
}
