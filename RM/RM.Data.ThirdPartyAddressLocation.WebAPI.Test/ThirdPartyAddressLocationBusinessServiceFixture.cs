using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Entities;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DTO.FileProcessing;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.BusinessService;
using RM.DataManagement.ThirdPartyAddressLocation.WebAPI.IntegrationService;
using RM.Data.ThirdPartyAddressLocation.WebAPI.DataService;
using RM.Data.ThirdPartyAddressLocation.WebAPI.Utils;

namespace RM.Data.ThirdPartyAddressLocation.WebAPI.Test
{
    [TestFixture]
    public class ThirdPartyAddressLocationBusinessServiceFixture : TestFixtureBase
    {
        private const string USRACTION = "Check updated DP Location";
        private const string NETWORKLINKDATAPROVIDER = "Data Provider";
        private const string EXTERNAL = "External";
        private const string NOTIFICATIONCLOSED = "Closed";

        private ThirdPartyAddressLocationBusinessService testCandidate;
        private Mock<IAddressLocationDataService> addressLocationDataServiceMock;
        private Mock<IThirdPartyAddressLocationIntegrationService> thirdPartyAddressLocationIntegrationServiceMock;
        private Mock<ILoggingHelper> loggingHelperMock;
        private Mock<IConfigurationHelper> configurationHelperMock;

        
        /// <summary>
        /// Test the method get Address location by Udprn.
        /// </summary>
        [Test]
        public async Task Test_Address_Location_By_Udprn()
        {
            int udprn = 0;

            Exception mockException = It.IsAny<Exception>();
            var result = await testCandidate.GetAddressLocationByUDPRN(udprn);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test the method get Address Location By Udprn Json.
        /// </summary>
        [Test]
        public void Test_Address_Location_By_UDPRN_Json()
        {
            int udprn = 0;
            Exception mockException = It.IsAny<Exception>();
            var result = testCandidate.GetAddressLocationByUDPRNJson(udprn);
            Assert.NotNull(result);
        }

        /// <summary>
        /// Test the method get Address location by Udprn.
        /// </summary>
        [Test]
        public void Test_Save_USR_Details_Valid_Scenario()
        {
            List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryList = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Notification Type", Id = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DB44760F-A5F4-42B8-8C36-24A89559AC8C"), ReferenceDataValue = "Action Required", DataDescription = "Action required", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("4C238DDE-9755-4FF0-9BDF-43328E67EEBA"), ReferenceDataValue = "Informational", DataDescription = "Informational", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("F151040D-3606-42D8-A0A9-705430C9DA7B"), ReferenceDataValue = "Authorization", DataDescription = "Authorization", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Data Provider", Id = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("3742D14F-BCDF-48AE-90D7-1D3E3E3BAA40"), ReferenceDataValue = "Internal", DataDescription = "Internal", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("5ABA4B09-D741-473D-801D-E88602BB2B03"), ReferenceDataValue = "External", DataDescription = "External", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "DeliveryPoint Use Indicator", Id = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Organisation", DataDescription = "Organisation", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Residential", DataDescription = "Residential", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Delivery Point Operational Status", Id = new Guid("87216073-E731-4B8C-9801-877EA4891F7E"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4"), ReferenceDataValue = "Live pending location", DataDescription = "Live pending location", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("9C1E56D7-5397-4984-9CF0-CD9EE7093C88"), ReferenceDataValue = "Live", DataDescription = "Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") },  new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("BEE6048D-79B3-49A4-AD26-E4F5B988B7AB"), ReferenceDataValue = "Not Live", DataDescription = "Not Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Network Node Type", Id = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("FE392EC6-92D6-4585-AA1E-19B60E16298B"), ReferenceDataValue = "Access Link Intersection Node", DataDescription = "Access Link Intersection Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DC58822F-5494-4FAB-A045-6F82F29AB01E"), ReferenceDataValue = "RMG Node", DataDescription = "RMG Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("0279ADDF-A549-4C00-815F-804630E8F1DF"), ReferenceDataValue = "Road Node", DataDescription = "Road Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("6F1C0263-7B4B-E711-A00D-AB551D02912E"), ReferenceDataValue = "RMG Service Node", DataDescription = "RMG Service Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("081AC364-EB64-4A4F-BBF2-C7DCF6865A46"), ReferenceDataValue = "Connecting Node", DataDescription = "Connecting Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") } } });


            string sbLocationXY = string.Format("POINT({0} {1})", "1234", "4567");

            // Convert the location from string type to geometry type
            DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), 27700);

            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO { ID = Guid.Empty, UDPRN = 1234, BuildingName = "Test", BuildingNumber = 10, DeliveryPoints = new List<DeliveryPointDataDTO> { new DeliveryPointDataDTO { ID = Guid.NewGuid(), PostalAddressID = Guid.Empty } } };
            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO { ID = Guid.Empty, UDPRN = 1234, LocationXY = spatialLocationXY, OperationalStatus_GUID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4") };

            List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos = new List<AddressLocationUSRPOSTDTO>() { new AddressLocationUSRPOSTDTO { UDPRN = 1234, XCoordinate = 12, YCoordinate = 10 , Latitude = 100, Longitude=200} };
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), USRACTION)).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryList));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(Task.FromResult(deliveryPointDTO));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            addressLocationDataServiceMock.Setup(x => x.GetPostalAddressData(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDataDTO));
            Exception mockException = It.IsAny<Exception>();

            var result = testCandidate.SaveUSRDetails(addressLocationUsrpostdtos);

            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>()), Times.Once);
            addressLocationDataServiceMock.Verify(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            addressLocationDataServiceMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDataDTO>()), Times.Once);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>()), Times.Once);
        }

        [Test]
        public void Test_Save_USR_Details_Invalid_Scenario()
        {
            List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryList = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Notification Type", Id = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DB44760F-A5F4-42B8-8C36-24A89559AC8C"), ReferenceDataValue = "Action Required", DataDescription = "Action required", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("4C238DDE-9755-4FF0-9BDF-43328E67EEBA"), ReferenceDataValue = "Informational", DataDescription = "Informational", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("F151040D-3606-42D8-A0A9-705430C9DA7B"), ReferenceDataValue = "Authorization", DataDescription = "Authorization", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Data Provider", Id = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("3742D14F-BCDF-48AE-90D7-1D3E3E3BAA40"), ReferenceDataValue = "Internal", DataDescription = "Internal", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("5ABA4B09-D741-473D-801D-E88602BB2B03"), ReferenceDataValue = "External", DataDescription = "External", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "DeliveryPoint Use Indicator", Id = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Organisation", DataDescription = "Organisation", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Residential", DataDescription = "Residential", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Delivery Point Operational Status", Id = new Guid("87216073-E731-4B8C-9801-877EA4891F7E"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4"), ReferenceDataValue = "Live pending location", DataDescription = "Live pending location", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("9C1E56D7-5397-4984-9CF0-CD9EE7093C88"), ReferenceDataValue = "Live", DataDescription = "Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("BEE6048D-79B3-49A4-AD26-E4F5B988B7AB"), ReferenceDataValue = "Not Live", DataDescription = "Not Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Network Node Type", Id = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("FE392EC6-92D6-4585-AA1E-19B60E16298B"), ReferenceDataValue = "Access Link Intersection Node", DataDescription = "Access Link Intersection Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DC58822F-5494-4FAB-A045-6F82F29AB01E"), ReferenceDataValue = "RMG Node", DataDescription = "RMG Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("0279ADDF-A549-4C00-815F-804630E8F1DF"), ReferenceDataValue = "Road Node", DataDescription = "Road Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("6F1C0263-7B4B-E711-A00D-AB551D02912E"), ReferenceDataValue = "RMG Service Node", DataDescription = "RMG Service Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("081AC364-EB64-4A4F-BBF2-C7DCF6865A46"), ReferenceDataValue = "Connecting Node", DataDescription = "Connecting Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") } } });

            List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos = new List<AddressLocationUSRPOSTDTO>() { new AddressLocationUSRPOSTDTO { UDPRN = 1234, XCoordinate = 12, YCoordinate = 10 } };
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            

            Exception mockException = It.IsAny<Exception>();
            var result = testCandidate.SaveUSRDetails(addressLocationUsrpostdtos);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>()), Times.Never);
            addressLocationDataServiceMock.Verify(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            addressLocationDataServiceMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDataDTO>()), Times.Never);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Approx_Location_Notification()
        {
            List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryList = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Notification Type", Id = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DB44760F-A5F4-42B8-8C36-24A89559AC8C"), ReferenceDataValue = "Action Required", DataDescription = "Action required", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("4C238DDE-9755-4FF0-9BDF-43328E67EEBA"), ReferenceDataValue = "Informational", DataDescription = "Informational", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("F151040D-3606-42D8-A0A9-705430C9DA7B"), ReferenceDataValue = "Authorization", DataDescription = "Authorization", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Data Provider", Id = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("3742D14F-BCDF-48AE-90D7-1D3E3E3BAA40"), ReferenceDataValue = "Internal", DataDescription = "Internal", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("5ABA4B09-D741-473D-801D-E88602BB2B03"), ReferenceDataValue = "External", DataDescription = "External", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "DeliveryPoint Use Indicator", Id = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Organisation", DataDescription = "Organisation", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Residential", DataDescription = "Residential", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Delivery Point Operational Status", Id = new Guid("87216073-E731-4B8C-9801-877EA4891F7E"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4"), ReferenceDataValue = "Live pending location", DataDescription = "Live pending location", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("9C1E56D7-5397-4984-9CF0-CD9EE7093C88"), ReferenceDataValue = "Live", DataDescription = "Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("BEE6048D-79B3-49A4-AD26-E4F5B988B7AB"), ReferenceDataValue = "Not Live", DataDescription = "Not Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Network Node Type", Id = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("FE392EC6-92D6-4585-AA1E-19B60E16298B"), ReferenceDataValue = "Access Link Intersection Node", DataDescription = "Access Link Intersection Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DC58822F-5494-4FAB-A045-6F82F29AB01E"), ReferenceDataValue = "RMG Node", DataDescription = "RMG Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("0279ADDF-A549-4C00-815F-804630E8F1DF"), ReferenceDataValue = "Road Node", DataDescription = "Road Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("6F1C0263-7B4B-E711-A00D-AB551D02912E"), ReferenceDataValue = "RMG Service Node", DataDescription = "RMG Service Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("081AC364-EB64-4A4F-BBF2-C7DCF6865A46"), ReferenceDataValue = "Connecting Node", DataDescription = "Connecting Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") } } });


            string sbLocationXY = string.Format("POINT({0} {1})", "1234", "4567");

            // Convert the location from string type to geometry type
            DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), 27700);

            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO { ID = Guid.Empty, UDPRN = 1234, BuildingName = "Test", BuildingNumber = 10, DeliveryPoints = new List<DeliveryPointDataDTO> { new DeliveryPointDataDTO { ID = Guid.NewGuid(), PostalAddressID = Guid.Empty } } };
            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO { ID = Guid.Empty, UDPRN = 1234, LocationXY = spatialLocationXY, OperationalStatus_GUID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4") };

            List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos = new List<AddressLocationUSRPOSTDTO>() { new AddressLocationUSRPOSTDTO { UDPRN = 1234, XCoordinate = 12, YCoordinate = 10, Latitude = 100, Longitude = 200 } };
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), USRACTION)).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryList));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(Task.FromResult(deliveryPointDTO));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            addressLocationDataServiceMock.Setup(x => x.GetPostalAddressData(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDataDTO));
            Exception mockException = It.IsAny<Exception>();

            await testCandidate.SaveUSRDetails(addressLocationUsrpostdtos);

            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>()), Times.Once);
            addressLocationDataServiceMock.Verify(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            addressLocationDataServiceMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDataDTO>()), Times.Once);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public async Task SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Real_Location_Non_Existing_Notification()
        {
            List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryList = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Notification Type", Id = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DB44760F-A5F4-42B8-8C36-24A89559AC8C"), ReferenceDataValue = "Action Required", DataDescription = "Action required", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("4C238DDE-9755-4FF0-9BDF-43328E67EEBA"), ReferenceDataValue = "Informational", DataDescription = "Informational", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("F151040D-3606-42D8-A0A9-705430C9DA7B"), ReferenceDataValue = "Authorization", DataDescription = "Authorization", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Data Provider", Id = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("3742D14F-BCDF-48AE-90D7-1D3E3E3BAA40"), ReferenceDataValue = "Internal", DataDescription = "Internal", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("5ABA4B09-D741-473D-801D-E88602BB2B03"), ReferenceDataValue = "External", DataDescription = "External", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "DeliveryPoint Use Indicator", Id = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Organisation", DataDescription = "Organisation", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Residential", DataDescription = "Residential", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Delivery Point Operational Status", Id = new Guid("87216073-E731-4B8C-9801-877EA4891F7E"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4"), ReferenceDataValue = "Live pending location", DataDescription = "Live pending location", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("9C1E56D7-5397-4984-9CF0-CD9EE7093C88"), ReferenceDataValue = "Live", DataDescription = "Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("BEE6048D-79B3-49A4-AD26-E4F5B988B7AB"), ReferenceDataValue = "Not Live", DataDescription = "Not Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Network Node Type", Id = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("FE392EC6-92D6-4585-AA1E-19B60E16298B"), ReferenceDataValue = "Access Link Intersection Node", DataDescription = "Access Link Intersection Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DC58822F-5494-4FAB-A045-6F82F29AB01E"), ReferenceDataValue = "RMG Node", DataDescription = "RMG Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("0279ADDF-A549-4C00-815F-804630E8F1DF"), ReferenceDataValue = "Road Node", DataDescription = "Road Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("6F1C0263-7B4B-E711-A00D-AB551D02912E"), ReferenceDataValue = "RMG Service Node", DataDescription = "RMG Service Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("081AC364-EB64-4A4F-BBF2-C7DCF6865A46"), ReferenceDataValue = "Connecting Node", DataDescription = "Connecting Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") } } });


            string sbLocationXY = string.Format("POINT({0} {1})", "1234", "4567");

            // Convert the location from string type to geometry type
            DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), 27700);

            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO { ID = Guid.Empty, UDPRN = 1234, BuildingName = "Test", BuildingNumber = 10, DeliveryPoints = new List<DeliveryPointDataDTO> { new DeliveryPointDataDTO { ID = Guid.NewGuid(), PostalAddressID = Guid.Empty } } };
            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO { ID = Guid.Empty, UDPRN = 1234, LocationXY = spatialLocationXY, OperationalStatus_GUID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4") };

            List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos = new List<AddressLocationUSRPOSTDTO>() { new AddressLocationUSRPOSTDTO { UDPRN = 1234, XCoordinate = 12, YCoordinate = 10, Latitude = 100, Longitude = 200 } };
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), USRACTION)).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryList));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(Task.FromResult(deliveryPointDTO));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(false));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            addressLocationDataServiceMock.Setup(x => x.GetPostalAddressData(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDataDTO));
            Exception mockException = It.IsAny<Exception>();

            await testCandidate.SaveUSRDetails(addressLocationUsrpostdtos);

            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>()), Times.Once);
            addressLocationDataServiceMock.Verify(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            addressLocationDataServiceMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDataDTO>()), Times.Once);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public async Task SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Real_Location_Within_Range_Non_Existing_Notification()
        {
            List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryList = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Notification Type", Id = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DB44760F-A5F4-42B8-8C36-24A89559AC8C"), ReferenceDataValue = "Action Required", DataDescription = "Action required", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("4C238DDE-9755-4FF0-9BDF-43328E67EEBA"), ReferenceDataValue = "Informational", DataDescription = "Informational", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("F151040D-3606-42D8-A0A9-705430C9DA7B"), ReferenceDataValue = "Authorization", DataDescription = "Authorization", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Data Provider", Id = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("3742D14F-BCDF-48AE-90D7-1D3E3E3BAA40"), ReferenceDataValue = "Internal", DataDescription = "Internal", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("5ABA4B09-D741-473D-801D-E88602BB2B03"), ReferenceDataValue = "External", DataDescription = "External", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "DeliveryPoint Use Indicator", Id = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Organisation", DataDescription = "Organisation", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Residential", DataDescription = "Residential", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Delivery Point Operational Status", Id = new Guid("87216073-E731-4B8C-9801-877EA4891F7E"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4"), ReferenceDataValue = "Live pending location", DataDescription = "Live pending location", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("9C1E56D7-5397-4984-9CF0-CD9EE7093C88"), ReferenceDataValue = "Live", DataDescription = "Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("BEE6048D-79B3-49A4-AD26-E4F5B988B7AB"), ReferenceDataValue = "Not Live", DataDescription = "Not Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Network Node Type", Id = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("FE392EC6-92D6-4585-AA1E-19B60E16298B"), ReferenceDataValue = "Access Link Intersection Node", DataDescription = "Access Link Intersection Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DC58822F-5494-4FAB-A045-6F82F29AB01E"), ReferenceDataValue = "RMG Node", DataDescription = "RMG Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("0279ADDF-A549-4C00-815F-804630E8F1DF"), ReferenceDataValue = "Road Node", DataDescription = "Road Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("6F1C0263-7B4B-E711-A00D-AB551D02912E"), ReferenceDataValue = "RMG Service Node", DataDescription = "RMG Service Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("081AC364-EB64-4A4F-BBF2-C7DCF6865A46"), ReferenceDataValue = "Connecting Node", DataDescription = "Connecting Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") } } });


            string sbLocationXY = string.Format("POINT({0} {1})", "1234", "4567");

            // Convert the location from string type to geometry type
            DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), 27700);

            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO { ID = Guid.Empty, UDPRN = 1234, BuildingName = "Test", BuildingNumber = 10, DeliveryPoints = new List<DeliveryPointDataDTO> { new DeliveryPointDataDTO { ID = Guid.NewGuid(), PostalAddressID = Guid.Empty } } };
            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO { ID = Guid.Empty, UDPRN = 1234, LocationXY = spatialLocationXY, OperationalStatus_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") };

            List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos = new List<AddressLocationUSRPOSTDTO>() { new AddressLocationUSRPOSTDTO { UDPRN = 1234, XCoordinate = 1234, YCoordinate = 4569, Latitude = 100, Longitude = 200 } };
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), USRACTION)).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryList));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(Task.FromResult(deliveryPointDTO));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            addressLocationDataServiceMock.Setup(x => x.GetPostalAddressData(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDataDTO));
            Exception mockException = It.IsAny<Exception>();

            await testCandidate.SaveUSRDetails(addressLocationUsrpostdtos);

            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>()), Times.Once);
            addressLocationDataServiceMock.Verify(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>()), Times.Once);
            addressLocationDataServiceMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDataDTO>()), Times.Once);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }


        [Test]
        public async Task SaveUSRDetails_Check_New_Address_Location_Existing_DP_With_Real_Location_Outside_Range_Non_Existing_Notification()
        {
            List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO> referenceDataCategoryList = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO>();
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Notification Type", Id = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DB44760F-A5F4-42B8-8C36-24A89559AC8C"), ReferenceDataValue = "Action Required", DataDescription = "Action required", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("4C238DDE-9755-4FF0-9BDF-43328E67EEBA"), ReferenceDataValue = "Informational", DataDescription = "Informational", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("F151040D-3606-42D8-A0A9-705430C9DA7B"), ReferenceDataValue = "Authorization", DataDescription = "Authorization", ReferenceDataCategory_GUID = new Guid("5780A5C0-8C04-4926-9C60-158F02A4E696") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Data Provider", Id = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("3742D14F-BCDF-48AE-90D7-1D3E3E3BAA40"), ReferenceDataValue = "Internal", DataDescription = "Internal", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("5ABA4B09-D741-473D-801D-E88602BB2B03"), ReferenceDataValue = "External", DataDescription = "External", ReferenceDataCategory_GUID = new Guid("6A2662CD-936C-44ED-961B-4448E8AB3EC8") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "DeliveryPoint Use Indicator", Id = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("990B86A2-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Organisation", DataDescription = "Organisation", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("178EDCAD-9431-E711-83EC-28D244AEF9ED"), ReferenceDataValue = "Residential", DataDescription = "Residential", ReferenceDataCategory_GUID = new Guid("5F3D7F7A-9431-E711-83EC-28D244AEF9ED") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Delivery Point Operational Status", Id = new Guid("87216073-E731-4B8C-9801-877EA4891F7E"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("D1E07533-15A0-4CE6-9DF1-181D385EFBB4"), ReferenceDataValue = "Live pending location", DataDescription = "Live pending location", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("9C1E56D7-5397-4984-9CF0-CD9EE7093C88"), ReferenceDataValue = "Live", DataDescription = "Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("BEE6048D-79B3-49A4-AD26-E4F5B988B7AB"), ReferenceDataValue = "Not Live", DataDescription = "Not Live", ReferenceDataCategory_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") } } });
            referenceDataCategoryList.Add(new CommonLibrary.EntityFramework.DTO.ReferenceDataCategoryDTO { CategoryName = "Network Node Type", Id = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9"), ReferenceDatas = new List<CommonLibrary.EntityFramework.DTO.ReferenceDataDTO>() { new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("FE392EC6-92D6-4585-AA1E-19B60E16298B"), ReferenceDataValue = "Access Link Intersection Node", DataDescription = "Access Link Intersection Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("DC58822F-5494-4FAB-A045-6F82F29AB01E"), ReferenceDataValue = "RMG Node", DataDescription = "RMG Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("0279ADDF-A549-4C00-815F-804630E8F1DF"), ReferenceDataValue = "Road Node", DataDescription = "Road Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("6F1C0263-7B4B-E711-A00D-AB551D02912E"), ReferenceDataValue = "RMG Service Node", DataDescription = "RMG Service Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("10A11A94-4B7E-411C-AF24-A0DD3DF631D6"), ReferenceDataValue = "Path Node", DataDescription = "Path Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") }, new CommonLibrary.EntityFramework.DTO.ReferenceDataDTO() { ID = new Guid("081AC364-EB64-4A4F-BBF2-C7DCF6865A46"), ReferenceDataValue = "Connecting Node", DataDescription = "Connecting Node", ReferenceDataCategory_GUID = new Guid("36F1D97F-AB4D-4422-BEA6-1472C392C6E9") } } });


            string sbLocationXY = string.Format("POINT({0} {1})", "1234", "4590");

            // Convert the location from string type to geometry type
            DbGeometry spatialLocationXY = DbGeometry.FromText(sbLocationXY.ToString(), 27700);

            PostalAddressDataDTO postalAddressDataDTO = new PostalAddressDataDTO { ID = Guid.Empty, UDPRN = 1234, BuildingName = "Test", BuildingNumber = 10, DeliveryPoints = new List<DeliveryPointDataDTO> { new DeliveryPointDataDTO { ID = Guid.NewGuid(), PostalAddressID = Guid.Empty } } };
            DeliveryPointDTO deliveryPointDTO = new DeliveryPointDTO { ID = Guid.Empty, UDPRN = 1234, LocationXY = spatialLocationXY, OperationalStatus_GUID = new Guid("87216073-E731-4B8C-9801-877EA4891F7E") };

            List<AddressLocationUSRPOSTDTO> addressLocationUsrpostdtos = new List<AddressLocationUSRPOSTDTO>() { new AddressLocationUSRPOSTDTO { UDPRN = 1234, XCoordinate = 1234, YCoordinate = 4569, Latitude = 100, Longitude = 200 } };
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.DeliveryPointExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), USRACTION)).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetReferenceDataSimpleLists(It.IsAny<List<string>>())).Returns(Task.FromResult(referenceDataCategoryList));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetDeliveryPointByPostalAddress(It.IsAny<Guid>())).Returns(Task.FromResult(deliveryPointDTO));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>())).Returns(Task.FromResult(true));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetPostCodeSectorByUDPRN(It.IsAny<int>())).Returns(Task.FromResult(It.IsAny<PostCodeSectorDTO>()));
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(false));
            addressLocationDataServiceMock.Setup(x => x.GetPostalAddressData(It.IsAny<int>())).Returns(Task.FromResult(postalAddressDataDTO));
            Exception mockException = It.IsAny<Exception>();

            await testCandidate.SaveUSRDetails(addressLocationUsrpostdtos);

            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateDeliveryPointById(It.IsAny<DeliveryPointDTO>()), Times.Never);
            addressLocationDataServiceMock.Verify(x => x.CheckIfNotificationExists(It.IsAny<int>(), It.IsAny<string>()), Times.Never);
            addressLocationDataServiceMock.Verify(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDataDTO>()), Times.Once);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.GetPostCodeSectorByUDPRN(It.IsAny<int>()), Times.Once);
            thirdPartyAddressLocationIntegrationServiceMock.Verify(x => x.AddNewNotification(It.IsAny<NotificationDTO>()), Times.Once);
        }

        protected override void OnSetup()
        {
            // OnSetup to be configured 
            addressLocationDataServiceMock = new Mock<IAddressLocationDataService>();
            thirdPartyAddressLocationIntegrationServiceMock = new Mock<IThirdPartyAddressLocationIntegrationService>();
            loggingHelperMock = new Mock<ILoggingHelper>();

            addressLocationDataServiceMock.Setup(x => x.GetAddressLocationByUDPRN(It.IsAny<int>())).Returns(Task.FromResult(new AddressLocationDataDTO() { }));
            addressLocationDataServiceMock.Setup(x => x.AddressLocationExists(It.IsAny<int>())).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.SaveNewAddressLocation(It.IsAny<AddressLocationDataDTO>())).Returns(Task.FromResult(1));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetDeliveryPointByUDPRNForThirdParty(It.IsAny<int>())).Returns(Task.FromResult(new DeliveryPointDTO() { LocationProvider = "abcd" }));
            var guid = System.Guid.Parse("B13D545D-2DE7-4E62-8DAD-00EC2B7FF8B8");
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.GetReferenceDataId(NETWORKLINKDATAPROVIDER, EXTERNAL)).Returns(Task.FromResult(guid));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateDeliveryPointLocationOnUDPRN(It.IsAny<DeliveryPointDTO>())).Returns(Task.FromResult(1));

            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.UpdateNotificationByUDPRN(It.IsAny<int>(), USRACTION, NOTIFICATIONCLOSED)).Returns(Task.FromResult(true));
            addressLocationDataServiceMock.Setup(x => x.GetPostalAddressData(It.IsAny<int>())).Returns(Task.FromResult(new PostalAddressDataDTO() {BuildingName = "abcd" }));
            thirdPartyAddressLocationIntegrationServiceMock.Setup(x => x.AddNewNotification(It.IsAny<NotificationDTO>())).Returns(Task.FromResult(1));

            var rmTraceManagerMock = new Mock<IRMTraceManager>();
            rmTraceManagerMock.Setup(x => x.StartTrace(It.IsAny<string>(), It.IsAny<Guid>()));
            loggingHelperMock.Setup(x => x.RMTraceManager).Returns(rmTraceManagerMock.Object);

            testCandidate = new ThirdPartyAddressLocationBusinessService(addressLocationDataServiceMock.Object, thirdPartyAddressLocationIntegrationServiceMock.Object, loggingHelperMock.Object);            
        }
    }
}