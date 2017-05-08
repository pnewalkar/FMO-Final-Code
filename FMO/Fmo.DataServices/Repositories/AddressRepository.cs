using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fmo.Common.Constants;
using Fmo.Common.Enums;
using Fmo.Common.Interface;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.DTO.UIDropdowns;
using Fmo.Entities;
using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    /// <summary>
    /// Repository to interact with postal address entity
    /// </summary>
    public class AddressRepository : RepositoryBase<PostalAddress, FMODBContext>, IAddressRepository
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IFileProcessingLogRepository fileProcessingLog = default(IFileProcessingLogRepository);
        private IPostCodeRepository postcodeRepository = default(IPostCodeRepository);
        private IReferenceDataCategoryRepository refDataRepository = default(IReferenceDataCategoryRepository);

        public AddressRepository(IDatabaseFactory<FMODBContext> databaseFactory, ILoggingHelper loggingHelper, IFileProcessingLogRepository fileProcessingLog, IPostCodeRepository postCodeRepository, IReferenceDataCategoryRepository refDataRepository)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
            this.fileProcessingLog = fileProcessingLog;
            this.postcodeRepository = postCodeRepository;
            this.refDataRepository = refDataRepository;
        }

        /// <summary>
        /// Delete postal Address records do not have an associated Delivery Point
        /// </summary>
        /// <param name="lstUDPRN">List of UDPRN</param>
        /// <param name="addressType">NYB</param>
        /// <returns>true or false</returns>
        public bool DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType)
        {
            bool isPostalAddressDeleted = false;
            string nybDeleteMsg = Constants.NYBErrorMessageForDelete;
            if (lstUDPRN != null && lstUDPRN.Count() > 0)
            {
                var lstAddress = DataContext.PostalAddresses.Include(m => m.DeliveryPoints).Where(n => !lstUDPRN.Contains(n.UDPRN.Value) && n.AddressType_GUID == addressType).ToList();
                if (lstAddress != null && lstAddress.Count > 0)
                {
                    lstAddress.ForEach(postalAddressEntity =>
                    {
                        if (postalAddressEntity.DeliveryPoints != null && postalAddressEntity.DeliveryPoints.Count > 0)
                        {
                            isPostalAddressDeleted = false;
                            this.loggingHelper.LogInfo(string.Format(nybDeleteMsg, postalAddressEntity.UDPRN));
                        }
                        else
                        {
                            DataContext.PostalAddresses.Remove(postalAddressEntity);
                        }
                    });
                    DataContext.SaveChanges();
                    isPostalAddressDeleted = true;
                }
            }

            return isPostalAddressDeleted;
        }

        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        public bool SaveAddress(PostalAddressDTO objPostalAddress, string strFileName)
        {
            bool isPostalAddressInserted = false;
            PostalAddress entity = new PostalAddress();
            if (objPostalAddress != null)
            {
                var objAddress = DataContext.PostalAddresses.Where(n => n.UDPRN == objPostalAddress.UDPRN).SingleOrDefault();
                try
                {
                    objPostalAddress.PostCodeGUID = this.postcodeRepository.GetPostCodeID(objPostalAddress.Postcode);
                    if (objAddress != null)
                    {
                        objAddress.Postcode = objPostalAddress.Postcode;
                        objAddress.PostTown = objPostalAddress.PostTown;
                        objAddress.DependentLocality = objPostalAddress.DependentLocality;
                        objAddress.DoubleDependentLocality = objPostalAddress.DoubleDependentLocality;
                        objAddress.Thoroughfare = objPostalAddress.DoubleDependentLocality;
                        objAddress.DependentThoroughfare = objPostalAddress.DependentThoroughfare;
                        objAddress.BuildingNumber = objPostalAddress.BuildingNumber;
                        objAddress.BuildingName = objPostalAddress.BuildingName;
                        objAddress.SubBuildingName = objPostalAddress.SubBuildingName;
                        objAddress.POBoxNumber = objPostalAddress.POBoxNumber;
                        objAddress.DepartmentName = objPostalAddress.DepartmentName;
                        objAddress.OrganisationName = objPostalAddress.OrganisationName;
                        objAddress.UDPRN = objPostalAddress.UDPRN;
                        objAddress.PostcodeType = objPostalAddress.PostcodeType;
                        objAddress.SmallUserOrganisationIndicator = objPostalAddress.SmallUserOrganisationIndicator;
                        objAddress.DeliveryPointSuffix = objPostalAddress.DeliveryPointSuffix;
                        objAddress.PostCodeGUID = objPostalAddress.PostCodeGUID;
                    }
                    else
                    {
                        objPostalAddress.ID = Guid.NewGuid();
                        entity = GenericMapper.Map<PostalAddressDTO, PostalAddress>(objPostalAddress);
                        DataContext.PostalAddresses.Add(entity);
                    }

                    DataContext.SaveChanges();
                    isPostalAddressInserted = true;
                }
                catch (Exception ex)
                {
                    if (objAddress != null)
                    {
                        DataContext.Entry(objAddress).State = EntityState.Unchanged;
                    }
                    else
                    {
                        DataContext.Entry(entity).State = EntityState.Unchanged;
                    }

                    LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Nyb.ToString(), ex.ToString());
                }
            }

            return isPostalAddressInserted;
        }

        /// <summary>
        /// Insert PAF details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">PAF details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        public bool InsertAddress(PostalAddressDTO objPostalAddress, string strFileName)
        {
            bool isPostalAddressInserted = false;
            PostalAddress objAddress = new PostalAddress();
            if (objPostalAddress != null)
            {
                try
                {
                    objPostalAddress.PostCodeGUID = this.postcodeRepository.GetPostCodeID(objPostalAddress.Postcode);
                    objAddress = GenericMapper.Map<PostalAddressDTO, PostalAddress>(objPostalAddress);
                    DataContext.PostalAddresses.Add(objAddress);

                    DataContext.SaveChanges();
                    isPostalAddressInserted = true;
                }
                catch (Exception ex)
                {
                    DataContext.Entry(objAddress).State = EntityState.Unchanged;
                    LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), ex.ToString());
                }
            }

            return isPostalAddressInserted;
        }

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        public PostalAddressDTO GetPostalAddress(int? uDPRN)
        {
            try
            {
                var postalAddress = DataContext.PostalAddresses.Where(n => n.UDPRN == uDPRN).SingleOrDefault();

                return GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// Get Postal address details depending on the address fields such as BuildingName and etc
        /// </summary>
        /// <param name="objPostalAddress">Postal address</param>
        /// <returns>returns PostalAddress object</returns>
        public PostalAddressDTO GetPostalAddress(PostalAddressDTO objPostalAddress)
        {
            try
            {
                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Include(m => m.DeliveryPoints)
                                .Where(
                                n => n.Postcode == objPostalAddress.Postcode
                                && n.BuildingName == (!string.IsNullOrEmpty(objPostalAddress.BuildingName) ? objPostalAddress.BuildingName : null)
                                && n.BuildingNumber == (objPostalAddress.BuildingNumber != null ? objPostalAddress.BuildingNumber : null)
                                && n.SubBuildingName == (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName) ? objPostalAddress.SubBuildingName : null)
                                && n.OrganisationName == (!string.IsNullOrEmpty(objPostalAddress.OrganisationName) ? objPostalAddress.OrganisationName : null)
                                && n.DepartmentName == (!string.IsNullOrEmpty(objPostalAddress.DepartmentName) ? objPostalAddress.DepartmentName : null)
                                && n.Thoroughfare == (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare) ? objPostalAddress.Thoroughfare : null)
                                && n.DependentThoroughfare == (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare) ? objPostalAddress.DependentThoroughfare : null)).FirstOrDefault();
                return GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// Checking for duplicatesthat already exists in FMO as a NYB record
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <returns>boolean</returns>
        public bool CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress)
        {
            try
            {
                bool isduplicate = false;
                Guid nybAddressID = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Nyb.ToString());

                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Include(m => m.DeliveryPoints)
                                .Where(n => n.AddressType_GUID == nybAddressID && n.BuildingName == (!string.IsNullOrEmpty(objPostalAddress.BuildingName) ? objPostalAddress.BuildingName : null)
                               && n.BuildingNumber == (objPostalAddress.BuildingNumber != null ? objPostalAddress.BuildingNumber : null)
                               && n.SubBuildingName == (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName) ? objPostalAddress.SubBuildingName : null)
                               && n.OrganisationName == (!string.IsNullOrEmpty(objPostalAddress.OrganisationName) ? objPostalAddress.OrganisationName : null)
                               && n.DepartmentName == (!string.IsNullOrEmpty(objPostalAddress.DepartmentName) ? objPostalAddress.DepartmentName : null)
                               && n.Thoroughfare == (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare) ? objPostalAddress.Thoroughfare : null)
                               && n.DependentThoroughfare == (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare) ? objPostalAddress.DependentThoroughfare : null)).SingleOrDefault();

                if (postalAddress != null && postalAddress.Postcode == objPostalAddress.Postcode)
                {
                    isduplicate = true;
                }
                else if (postalAddress != null && postalAddress.Postcode != objPostalAddress.Postcode)
                {
                    isduplicate = false;
                }

                return isduplicate;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Update PAF details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">PAF details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <param name="pafUpdateType">Passing 'USR' and 'NYB' to update</param>
        /// <returns>true or false</returns>
        public bool UpdateAddress(PostalAddressDTO objPostalAddress, string strFileName, string pafUpdateType)
        {
            bool isPostalAddressUpdated = false;
            if (objPostalAddress != null)
            {
                var objAddress = DataContext.PostalAddresses.Include(m => m.DeliveryPoints).Where(n => n.ID == objPostalAddress.ID).SingleOrDefault();
                try
                {
                    objPostalAddress.PostCodeGUID = this.postcodeRepository.GetPostCodeID(objPostalAddress.Postcode);
                    if (objAddress != null)
                    {
                        objAddress.Postcode = objPostalAddress.Postcode;
                        objAddress.PostTown = objPostalAddress.PostTown;
                        objAddress.DependentLocality = objPostalAddress.DependentLocality;
                        objAddress.DoubleDependentLocality = objPostalAddress.DoubleDependentLocality;
                        objAddress.Thoroughfare = objPostalAddress.Thoroughfare;
                        objAddress.DependentThoroughfare = objPostalAddress.DependentThoroughfare;
                        objAddress.BuildingNumber = objPostalAddress.BuildingNumber;
                        objAddress.BuildingName = objPostalAddress.BuildingName;
                        objAddress.SubBuildingName = objPostalAddress.SubBuildingName;
                        objAddress.POBoxNumber = objPostalAddress.POBoxNumber;
                        objAddress.DepartmentName = objPostalAddress.DepartmentName;
                        objAddress.OrganisationName = objPostalAddress.OrganisationName;
                        objAddress.UDPRN = objPostalAddress.UDPRN;
                        objAddress.PostcodeType = objPostalAddress.PostcodeType;
                        objAddress.SmallUserOrganisationIndicator = objPostalAddress.SmallUserOrganisationIndicator;
                        objAddress.DeliveryPointSuffix = objPostalAddress.DeliveryPointSuffix;
                        objAddress.PostCodeGUID = objPostalAddress.PostCodeGUID;
                        objAddress.AddressType_GUID = objPostalAddress.AddressType_GUID;
                        if (objAddress.DeliveryPoints != null && objAddress.DeliveryPoints.Count > 0)
                        {
                            foreach (var objDelPoint in objAddress.DeliveryPoints)
                            {
                                if (objAddress.OrganisationName.Length > 0)
                                {
                                    objDelPoint.DeliveryPointUseIndicator = Constants.DeliveryPointUseIndicatorPAF;
                                }

                                objDelPoint.UDPRN = objPostalAddress.UDPRN;
                            }
                        }
                    }

                    DataContext.SaveChanges();
                    isPostalAddressUpdated = true;
                }
                catch (Exception ex)
                {
                    if (objAddress != null)
                    {
                        DataContext.Entry(objAddress).State = EntityState.Unchanged;
                    }

                    LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), ex.ToString());
                }
            }

            return isPostalAddressUpdated;
        }

        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postcodes</returns>
        public async Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid)
        {
            List<string> searchdetails = new List<string>();
            List<Guid> addresstypeIDs = new List<Guid>()
            {
                refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Paf.ToString()),
                refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Nyb.ToString())
            };

            var searchresults = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                       join pc in DataContext.Postcodes.AsNoTracking() on pa.PostCodeGUID equals pc.ID
                                       join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
                                       where addresstypeIDs.Contains(pa.AddressType_GUID) &&
                                       (pa.Thoroughfare.Contains(searchText) || pa.DependentThoroughfare.Contains(searchText) || pa.Postcode.Contains(searchText)) &&
                                       ul.Unit_GUID == unitGuid
                                       select new { pa.Thoroughfare, pa.Postcode }).Distinct().ToListAsync();

            if (searchresults != null && searchresults.Count > 0)
            {
                for (var i = 0; i < searchresults.Count; i++)
                {
                    string searchitem = searchresults[i].Thoroughfare + ", " + searchresults[i].Postcode;
                    string formattedResult = Regex.Replace(searchitem, ",+", ",").Trim(',');
                    searchdetails.Add(formattedResult);
                }
            }

            searchdetails.Sort();
            return searchdetails;
        }

        /// <summary>
        /// Filter PostalAddress based on post code
        /// </summary>
        /// <param name="postCode">postCode</param>
        /// <returns>List of Postal Address</returns>
        public async Task<List<PostalAddressDTO>> GetPostalAddressDetails(string postCode)
        {
            List<string> lstPocodes = new List<string>();
            List<PostalAddressDTO> postalAddressDTO = new List<PostalAddressDTO>();

            var postalAddress = await DataContext.PostalAddresses.AsNoTracking().Include(p => p.Postcode1.DeliveryRoutePostcodes).Where(n => n.Postcode == postCode).ToListAsync();

            postalAddressDTO = GenericMapper.MapList<PostalAddress, PostalAddressDTO>(postalAddress);

            postalAddress.ForEach(p => p.Postcode1.DeliveryRoutePostcodes.ToList().ForEach(d =>
            {
                if (d.IsPrimaryRoute)
                {
                    postalAddressDTO.Where(pa => pa.Postcode == d.Postcode.PostcodeUnit).ToList().ForEach(paDTO =>
                    {
                        if (paDTO.RouteDetails == null)
                        {
                            paDTO.RouteDetails = new List<BindingEntity>();
                        }

                        if (!paDTO.RouteDetails.Where(b => b.DisplayText == Constants.PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim()).Any())
                        {
                            paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = Constants.PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.ID });
                        }
                    });
                }
                else
                {
                    postalAddressDTO.Where(pa => pa.Postcode == d.Postcode.PostcodeUnit).ToList().ForEach(paDTO =>
                    {
                        if (paDTO.RouteDetails == null)
                        {
                            paDTO.RouteDetails = new List<BindingEntity>();
                        }

                        if (!paDTO.RouteDetails.Where(b => b.DisplayText == Constants.SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim()).Any())
                        {
                            paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = Constants.SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.ID });
                        }
                    });
                }
            }));

            return postalAddressDTO;
        }

        /// <summary>
        /// Filter PostalAddress based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDTO GetPostalAddressDetails(Guid id)
        {
            try
            {
                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Where(n => n.ID == id).FirstOrDefault();
                return GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);
            }
            catch (Exception ex)
            {
                this.loggingHelper.LogError(ex);
                return null;
            }
        }

        /// <summary>
        /// Log exception into DB if error occurs while inserting NYB,PAF,USR records in DB
        /// </summary>
        /// <param name="uDPRN">UDPRN</param>
        /// <param name="strFileName">FileName</param>
        /// <param name="fileType">Filetype</param>
        /// <param name="strException">Exception</param>
        private void LogFileException(int uDPRN, string strFileName, string fileType, string strException)
        {
            FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO()
            {
                FileID = Guid.NewGuid(),
                UDPRN = uDPRN,
                AmendmentType = Constants.INSERT,
                FileName = strFileName,
                FileProcessing_TimeStamp = DateTime.UtcNow,
                FileType = fileType,
                NatureOfError = strException
            };
            fileProcessingLog.LogFileException(objFileProcessingLog);
        }
    }
}