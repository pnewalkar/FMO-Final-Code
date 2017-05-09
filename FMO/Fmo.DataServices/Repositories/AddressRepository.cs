namespace Fmo.DataServices.Repositories
{
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
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

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
                //var postalAddress = DataContext.PostalAddresses.AsNoTracking().Include(m => m.DeliveryPoints)
                //                .Where(
                //                n => n.Postcode == objPostalAddress.Postcode
                //                && (!string.IsNullOrEmpty(n.BuildingName) ? n.BuildingName : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.BuildingName) ? objPostalAddress.BuildingName : string.Empty)
                //                && (n.BuildingNumber != null ? n.BuildingNumber : null) == (objPostalAddress.BuildingNumber != null ? objPostalAddress.BuildingNumber : null)
                //                && (!string.IsNullOrEmpty(n.SubBuildingName) ? n.SubBuildingName : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName) ? objPostalAddress.SubBuildingName : string.Empty)
                //                && (!string.IsNullOrEmpty(n.OrganisationName) ? n.OrganisationName : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.OrganisationName) ? objPostalAddress.OrganisationName : string.Empty)
                //                && (!string.IsNullOrEmpty(n.DepartmentName) ? n.DepartmentName : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.DepartmentName) ? objPostalAddress.DepartmentName : string.Empty)
                //                && (!string.IsNullOrEmpty(n.Thoroughfare) ? n.Thoroughfare : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare) ? objPostalAddress.Thoroughfare : string.Empty)
                //                && (!string.IsNullOrEmpty(n.DependentThoroughfare) ? n.DependentThoroughfare : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare) ? objPostalAddress.DependentThoroughfare : string.Empty)).FirstOrDefault();

                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Include(m => m.DeliveryPoints)
                                .Where(
                                n => n.Postcode == objPostalAddress.Postcode
                                && ((n.BuildingName == (!string.IsNullOrEmpty(objPostalAddress.BuildingName) ? objPostalAddress.BuildingName : null))
                                    ||
                                    ((!string.IsNullOrEmpty(n.BuildingName) ? n.BuildingName : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.BuildingName) ? objPostalAddress.BuildingName : string.Empty)))

                                && ((n.BuildingNumber == (objPostalAddress.BuildingNumber != null ? objPostalAddress.BuildingNumber : null))
                                ||
                                 ((n.BuildingNumber.HasValue ? n.BuildingNumber.Value : 0) == (objPostalAddress.BuildingNumber.HasValue ? objPostalAddress.BuildingNumber.Value : 0)))

                                && ((n.SubBuildingName == (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName) ? objPostalAddress.SubBuildingName : null))
                                    ||
                                    ((!string.IsNullOrEmpty(n.SubBuildingName) ? n.SubBuildingName : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName) ? objPostalAddress.SubBuildingName : string.Empty)))
                                && ((n.OrganisationName == (!string.IsNullOrEmpty(objPostalAddress.OrganisationName) ? objPostalAddress.OrganisationName : null))
                                    ||
                                    ((!string.IsNullOrEmpty(n.OrganisationName) ? n.OrganisationName : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.OrganisationName) ? objPostalAddress.OrganisationName : string.Empty)))
                                && ((n.DepartmentName == (!string.IsNullOrEmpty(objPostalAddress.DepartmentName) ? objPostalAddress.DepartmentName : null))
                                    ||
                                    ((!string.IsNullOrEmpty(n.DepartmentName) ? n.DepartmentName : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.DepartmentName) ? objPostalAddress.DepartmentName : string.Empty)))
                                && ((n.Thoroughfare == (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare) ? objPostalAddress.Thoroughfare : null))
                                    ||
                                    ((!string.IsNullOrEmpty(n.Thoroughfare) ? n.Thoroughfare : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare) ? objPostalAddress.Thoroughfare : string.Empty)))
                                && ((n.DependentThoroughfare == (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare) ? objPostalAddress.DependentThoroughfare : null))
                                    ||
                                    ((!string.IsNullOrEmpty(n.DependentThoroughfare) ? n.DependentThoroughfare : string.Empty) == (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare) ? objPostalAddress.DependentThoroughfare : string.Empty)))).FirstOrDefault();

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
        /// <returns>string</returns>
        public string CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress)
        {
            try
            {
                string postCode = string.Empty;
                Guid nybAddressID = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Nyb.ToString());

                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Include(m => m.DeliveryPoints)
                                .Where(n => n.AddressType_GUID == nybAddressID);

                if (!string.IsNullOrEmpty(objPostalAddress.BuildingName))
                {
                    postalAddress = postalAddress.Where(n => n.BuildingName.Equals(objPostalAddress.BuildingName, StringComparison.OrdinalIgnoreCase));
                }

                if (objPostalAddress.BuildingNumber != null)
                {
                    postalAddress = postalAddress.Where(n => n.BuildingNumber == objPostalAddress.BuildingNumber);
                }

                if (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName))
                {
                    postalAddress = postalAddress.Where(n => n.SubBuildingName.Equals(objPostalAddress.SubBuildingName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.OrganisationName))
                {
                    postalAddress = postalAddress.Where(n => n.OrganisationName.Equals(objPostalAddress.OrganisationName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.DepartmentName))
                {
                    postalAddress = postalAddress.Where(n => n.DepartmentName.Equals(objPostalAddress.DepartmentName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare))
                {
                    postalAddress = postalAddress.Where(n => n.Thoroughfare.Equals(objPostalAddress.Thoroughfare, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare))
                {
                    postalAddress = postalAddress.Where(n => n.DependentThoroughfare.Equals(objPostalAddress.DependentThoroughfare, StringComparison.OrdinalIgnoreCase));
                }

                var address = postalAddress.SingleOrDefault();
                if (address != null && address.Postcode != address.Postcode)
                {
                    postCode = address.Postcode;
                }

                return postCode;
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

                        Guid deliveryPointUseIndicator = refDataRepository.GetReferenceDataId(Constants.DeliveryPointUseIndicator, Constants.DeliveryPointUseIndicatorPAF);

                        if (objAddress.DeliveryPoints != null && objAddress.DeliveryPoints.Count > 0)
                        {
                            foreach (var objDelPoint in objAddress.DeliveryPoints)
                            {
                                if (objAddress.OrganisationName.Length > 0)
                                {
                                    objDelPoint.DeliveryPointUseIndicator_GUID = deliveryPointUseIndicator;
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
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postal Address</returns>
        public async Task<List<PostalAddressDTO>> GetPostalAddressDetails(string postCode, Guid unitGuid)
        {
            List<string> lstPocodes = new List<string>();
            List<PostalAddressDTO> postalAddressDTO = new List<PostalAddressDTO>();

            var postalAddress = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                       join pc in DataContext.Postcodes.AsNoTracking()
                                       on pa.PostCodeGUID equals pc.ID
                                       where pc.PostcodeUnit == postCode
                                       select pa).ToListAsync();

            postalAddressDTO = GenericMapper.MapList<PostalAddress, PostalAddressDTO>(postalAddress);

            postalAddress.ForEach(p => p.Postcode1.DeliveryRoutePostcodes.ToList().ForEach(d =>
            {
                if (d.IsPrimaryRoute)
                {
                    postalAddressDTO.Where(pa => pa.Postcode == d.Postcode.PostcodeUnit).Select(pa => pa).ToList().ForEach(paDTO =>
                    {
                        if (paDTO.RouteDetails == null)
                        {
                            paDTO.RouteDetails = new List<BindingEntity>();
                            paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = Constants.SELECT, Value = new Guid(Constants.DEFAULTGUID) });
                        }

                        if (!paDTO.RouteDetails.Where(b => b.DisplayText == Constants.PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim()).Any())
                        {
                            paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = Constants.PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.ID });
                        }
                    });
                }
                else
                {
                    postalAddressDTO.Where(pa => pa.Postcode == d.Postcode.PostcodeUnit).Select(pa => pa).ToList().ForEach(paDTO =>
                    {
                        if (paDTO.RouteDetails == null)
                        {
                            paDTO.RouteDetails = new List<BindingEntity>();
                            paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = Constants.SELECT, Value = new Guid(Constants.DEFAULTGUID) });
                        }

                        if (!paDTO.RouteDetails.Where(b => b.DisplayText == Constants.SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim()).Any())
                        {
                            paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = Constants.SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.ID });
                        }
                    });
                }
            }));

            var postCodes = await DataContext.UnitLocationPostcodes.AsNoTracking().Where(p => p.Unit_GUID == unitGuid).Select(s => s.PoscodeUnit_GUID).Distinct().ToListAsync();
            if (postalAddressDTO != null && postalAddressDTO.Count > 0 && (postalAddressDTO[0].RouteDetails == null || postalAddressDTO[0].RouteDetails.Count == 0))
            {
                List<BindingEntity> routeDetails = new List<BindingEntity>();
                var routes = await DataContext.DeliveryRoutePostcodes.AsNoTracking().Where(dr => postCodes.Contains(dr.Postcode_GUID)).ToListAsync();
                routes.ForEach(r =>
                {
                    if (!routeDetails.Where(rd => rd.Value == r.DeliveryRoute.ID).Any())
                    {
                        routeDetails.Add(new BindingEntity() { DisplayText = r.DeliveryRoute.RouteName, Value = r.DeliveryRoute.ID });
                    }
                });
                postalAddressDTO[0].RouteDetails = new List<BindingEntity>(routeDetails.Distinct());
            }

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
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public bool CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            bool isPostalAddressInserted = false;
            if (addDeliveryPointDTO.PostalAddressDTO != null && addDeliveryPointDTO.DeliveryPointDTO != null)
            {
                var objPostalAddress = DataContext.PostalAddresses.Where(n => n.ID == addDeliveryPointDTO.PostalAddressDTO.ID).SingleOrDefault();
                DeliveryPoint objDeliveryPoint = DeliveryPointAlaisMapping(addDeliveryPointDTO.DeliveryPointDTO);
                try
                {
                    objPostalAddress.PostCodeGUID = this.postcodeRepository.GetPostCodeID(addDeliveryPointDTO.PostalAddressDTO.Postcode);
                    objPostalAddress.AddressType_GUID = refDataRepository.GetReferenceDataId(Constants.PostalAddressType, FileType.Usr.ToString());
                    if (objPostalAddress != null)
                    {
                        objPostalAddress.Postcode = addDeliveryPointDTO.PostalAddressDTO.Postcode;
                        objPostalAddress.PostTown = addDeliveryPointDTO.PostalAddressDTO.PostTown;
                        objPostalAddress.DependentLocality = addDeliveryPointDTO.PostalAddressDTO.DependentLocality;
                        objPostalAddress.DoubleDependentLocality = addDeliveryPointDTO.PostalAddressDTO.DoubleDependentLocality;
                        objPostalAddress.Thoroughfare = addDeliveryPointDTO.PostalAddressDTO.DoubleDependentLocality;
                        objPostalAddress.DependentThoroughfare = addDeliveryPointDTO.PostalAddressDTO.DependentThoroughfare;
                        objPostalAddress.BuildingNumber = addDeliveryPointDTO.PostalAddressDTO.BuildingNumber;
                        objPostalAddress.BuildingName = addDeliveryPointDTO.PostalAddressDTO.BuildingName;
                        objPostalAddress.SubBuildingName = addDeliveryPointDTO.PostalAddressDTO.SubBuildingName;
                        objPostalAddress.POBoxNumber = addDeliveryPointDTO.PostalAddressDTO.POBoxNumber;
                        objPostalAddress.DepartmentName = addDeliveryPointDTO.PostalAddressDTO.DepartmentName;
                        objPostalAddress.OrganisationName = addDeliveryPointDTO.PostalAddressDTO.OrganisationName;
                        objPostalAddress.UDPRN = addDeliveryPointDTO.PostalAddressDTO.UDPRN;
                        objPostalAddress.PostcodeType = addDeliveryPointDTO.PostalAddressDTO.PostcodeType;
                        objPostalAddress.SmallUserOrganisationIndicator = addDeliveryPointDTO.PostalAddressDTO.SmallUserOrganisationIndicator;
                        objPostalAddress.DeliveryPointSuffix = addDeliveryPointDTO.PostalAddressDTO.DeliveryPointSuffix;
                        objPostalAddress.PostCodeGUID = addDeliveryPointDTO.PostalAddressDTO.PostCodeGUID;
                        objPostalAddress.DeliveryPoints.Add(objDeliveryPoint);
                    }
                    else
                    {
                        objPostalAddress.ID = Guid.NewGuid();
                        var entity = GenericMapper.Map<PostalAddressDTO, PostalAddress>(addDeliveryPointDTO.PostalAddressDTO);
                        entity.DeliveryPoints.Add(objDeliveryPoint);
                        DataContext.PostalAddresses.Add(entity);
                    }

                    //DataContext.SaveChanges();
                    isPostalAddressInserted = true;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return isPostalAddressInserted;
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

        private DeliveryPoint DeliveryPointAlaisMapping(DeliveryPointDTO deliveryPointDTO)
        {
            Guid deliveryPointID = Guid.NewGuid();
            return new DeliveryPoint()
            {
                ID = deliveryPointID,
                DeliveryPointUseIndicator_GUID = deliveryPointDTO.DeliveryPointUseIndicator_GUID,
                MultipleOccupancyCount = deliveryPointDTO.MultipleOccupancyCount,
                MailVolume = deliveryPointDTO.MailVolume,
                DeliveryPointAlias = deliveryPointDTO.DeliveryPointAliasDTO.Select(n => new DeliveryPointAlias
                {
                    ID = Guid.NewGuid(),
                    DeliveryPoint_GUID = deliveryPointID,
                    DPAlias = n.DPAlias,
                    Preferred = n.Preferred
                }).ToList()
            };
        }
    }
}