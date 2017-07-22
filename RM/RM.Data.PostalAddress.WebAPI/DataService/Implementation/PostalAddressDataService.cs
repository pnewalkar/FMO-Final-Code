namespace RM.DataManagement.PostalAddress.WebAPI.DataService.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.PostalAddress.WebAPI.Utils;
    using DataDTO;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.ExceptionMiddleware;
    using RM.CommonLibrary.HelperMiddleware;
    using RM.CommonLibrary.LoggingMiddleware;
    using RM.DataManagement.PostalAddress.WebAPI.DataService.Implementation.MappingConfiguration;
    using RM.DataManagement.PostalAddress.WebAPI.DataService.Interfaces;
    using RM.DataManagement.PostalAddress.WebAPI.DTO;
    using RM.DataManagement.PostalAddress.WebAPI.Entities;

    /// <summary>
    /// DataService to interact with postal address entity
    /// </summary>
    public class PostalAddressDataService : DataServiceBase<PostalAddress, PostalAddressDBContext>, IPostalAddressDataService
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IFileProcessingLogDataService fileProcessingLog = default(IFileProcessingLogDataService);
        private int priority = LoggerTraceConstants.PostalAddressAPIPriority;
        private int entryEventId = LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId;
        private int exitEventId = LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId;

        public PostalAddressDataService(IDatabaseFactory<PostalAddressDBContext> databaseFactory, ILoggingHelper loggingHelper, IFileProcessingLogDataService fileProcessingLog)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
            this.fileProcessingLog = fileProcessingLog;
        }

        /// <summary>
        /// Delete postal Address records do not have an associated Delivery Point
        /// </summary>
        /// <param name="lstUDPRN">List of UDPRN</param>
        /// <param name="addressType">NYB</param>
        /// <returns>true or false</returns>
        public async Task<bool> DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.DeleteNYBPostalAddress"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(DeleteNYBPostalAddress);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                try
                {
                    bool isPostalAddressDeleted = false;
                    string nybDeleteMsg = PostalAddressConstants.NYBErrorMessageForDelete;
                    if (lstUDPRN != null && lstUDPRN.Any())
                    {
                        var lstAddress = await DataContext.PostalAddresses.Include(m => m.DeliveryPoints).Where(n => !lstUDPRN.Contains(n.UDPRN.Value) && n.AddressType_GUID == addressType).ToListAsync();
                        if (lstAddress.Count > 0)
                        {
                            lstAddress.ForEach(postalAddressEntity =>
                            {
                                if (postalAddressEntity.DeliveryPoints != null && postalAddressEntity.DeliveryPoints.Count > 0)
                                {
                                    isPostalAddressDeleted = false;
                                    this.loggingHelper.Log(string.Format(nybDeleteMsg, postalAddressEntity.UDPRN), TraceEventType.Information);
                                }
                                else
                                {
                                    DataContext.PostalAddresses.Remove(postalAddressEntity);
                                }
                            });
                            await DataContext.SaveChangesAsync();
                            isPostalAddressDeleted = true;
                        }
                    }

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return isPostalAddressDeleted;
                }
                catch (DbUpdateException dbUpdateException)
                {
                    throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlDeleteException, string.Concat("PostalAdresses with UPPRN:", string.Join(",", lstUDPRN))));
                }
            }
        }

        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>Whether the record has been updated correctly</returns>
        public async Task<bool> SaveAddress(PostalAddressDataDTO objPostalAddressDataDTO, string strFileName)
        {
            bool isPostalAddressInserted = false;
            PostalAddress objPostalAddress = default(PostalAddress);
            PostalAddress entity = new PostalAddress();
            if (objPostalAddress != null)
            {
                try
                {
                    using (loggingHelper.RMTraceManager.StartTrace("DataService.SaveAddress"))
                    {
                        string methodName = typeof(PostalAddressDataService) + "." + nameof(SaveAddress);
                        loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                        objPostalAddress = await DataContext.PostalAddresses.Where(n => n.UDPRN == objPostalAddressDataDTO.UDPRN).SingleOrDefaultAsync();

                        // Update existing Postal Address else Insert new Postal Address
                        if (objPostalAddress != null)
                        {
                            objPostalAddress.Postcode = objPostalAddressDataDTO.Postcode;
                            objPostalAddress.PostTown = objPostalAddressDataDTO.PostTown;
                            objPostalAddress.DependentLocality = objPostalAddressDataDTO.DependentLocality;
                            objPostalAddress.DoubleDependentLocality = objPostalAddressDataDTO.DoubleDependentLocality;
                            objPostalAddress.Thoroughfare = objPostalAddressDataDTO.Thoroughfare;
                            objPostalAddress.DependentThoroughfare = objPostalAddressDataDTO.DependentThoroughfare;
                            objPostalAddress.BuildingNumber = objPostalAddressDataDTO.BuildingNumber;
                            objPostalAddress.BuildingName = objPostalAddressDataDTO.BuildingName;
                            objPostalAddress.SubBuildingName = objPostalAddressDataDTO.SubBuildingName;
                            objPostalAddress.POBoxNumber = objPostalAddressDataDTO.POBoxNumber;
                            objPostalAddress.DepartmentName = objPostalAddressDataDTO.DepartmentName;
                            objPostalAddress.OrganisationName = objPostalAddressDataDTO.OrganisationName;
                            objPostalAddress.UDPRN = objPostalAddressDataDTO.UDPRN;
                            objPostalAddress.PostcodeType = objPostalAddressDataDTO.PostcodeType;
                            objPostalAddress.SmallUserOrganisationIndicator = objPostalAddressDataDTO.SmallUserOrganisationIndicator;
                            objPostalAddress.DeliveryPointSuffix = objPostalAddressDataDTO.DeliveryPointSuffix;
                            objPostalAddress.Postcode = objPostalAddressDataDTO.Postcode;
                        }
                        else
                        {
                            objPostalAddress.ID = Guid.NewGuid();

                            Mapper.Initialize(cfg =>
                            {
                                cfg.CreateMap<PostalAddressDataDTO, PostalAddress>();
                                cfg.CreateMap<PostalAddressStatusDataDTO, PostalAddressStatus>();
                                cfg.CreateMap<DeliveryPointDataDTO, DeliveryPoint>();
                            });
                            Mapper.Configuration.CreateMapper();

                            entity = Mapper.Map<PostalAddressDataDTO, PostalAddress>(objPostalAddressDataDTO);
                            DataContext.PostalAddresses.Add(entity);
                        }

                        await DataContext.SaveChangesAsync();
                        isPostalAddressInserted = true;
                        loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    }
                }
                catch (Exception ex)
                {
                    // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                    if (objPostalAddressDataDTO.UDPRN != null)
                    {
                        LogFileException(objPostalAddressDataDTO.UDPRN.Value, strFileName, FileType.Nyb.ToString(), ex.ToString());
                    }

                    throw ex;
                }
            }

            return isPostalAddressInserted;
        }

        /// <summary>
        /// Insert PAF details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">PAF details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>Whether the record has been inserted correctly</returns>
        public async Task<bool> InsertAddress(PostalAddressDataDTO objPostalAddress, string strFileName)
        {
            if (objPostalAddress == null)
            {
                throw new ArgumentNullException(nameof(objPostalAddress), string.Format(ErrorConstants.Err_ArgumentmentNullException, objPostalAddress));
            }
            bool isPostalAddressInserted = false;
            PostalAddress objAddress = new PostalAddress();
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.InsertAddress"))
                {
                    string methodName = typeof(PostalAddressDataService) + "." + nameof(InsertAddress);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    // Insert new Postal Address
                    if (objPostalAddress != null)
                    {
                        Mapper.Initialize(cfg =>
                        {
                            cfg.CreateMap<PostalAddressDataDTO, PostalAddress>();
                            cfg.CreateMap<PostalAddressStatusDataDTO, PostalAddressStatus>();
                            cfg.CreateMap<DeliveryPointDataDTO, DeliveryPoint>();
                        });
                        Mapper.Configuration.CreateMapper();

                        objAddress = Mapper.Map<PostalAddressDataDTO, PostalAddress>(objPostalAddress);
                        DataContext.PostalAddresses.Add(objAddress);

                        await DataContext.SaveChangesAsync();
                        isPostalAddressInserted = true;
                    }
                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), dbUpdateException.ToString());
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("PostalAddress PAF for UDPRN:", objAddress.UDPRN)));
            }
            catch (NotSupportedException notSupportedException)
            {
                // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), notSupportedException.ToString());
                notSupportedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), disposedException.ToString());
                disposedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
            }

            return isPostalAddressInserted;
        }

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDataDTO> GetPostalAddress(int? uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddress"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(GetPostalAddress);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var postalAddress = await DataContext.PostalAddresses.Where(n => n.UDPRN == uDPRN).SingleOrDefaultAsync();

                ConfigureMapper();

                var dtoPostalAddress = Mapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return dtoPostalAddress;
            }
        }

        /// <summary>
        /// Get Postal address details depending on the address fields such as BuildingName and etc
        /// </summary>
        /// <param name="objPostalAddress">Postal address</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDataDTO> GetPostalAddress(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddress"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(GetPostalAddress);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                // Get first Postal Address satifying the below condition
                PostalAddress postalAddress = await GetPostalAddressEntity(objPostalAddress);

                ConfigureMapper();

                var dtoPostalAddress = Mapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return dtoPostalAddress;
            }
        }

        /// <summary>
        /// Checking for duplicatesthat already exists in FMO as a NYB record
        /// </summary>
        /// <param name="objPostalAddress">Postal address</param>
        /// <param name="addressTypeNYBGuid">Reference data Guid of NYB</param>
        /// <returns>postcode</returns>
        public string CheckForDuplicateNybRecords(PostalAddressDataDTO objPostalAddress, Guid addressTypeNYBGuid)
        {
            string postCode = string.Empty;

            using (loggingHelper.RMTraceManager.StartTrace("DataService.CheckForDuplicateNybRecords"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(CheckForDuplicateNybRecords);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                // Get the Postal Address Entity matching the address fields
                List<PostalAddress> postalAddress = GetPostalAddressEntities(objPostalAddress, addressTypeNYBGuid).ToList();

                if (postalAddress != null && postalAddress.Count > 0)
                {
                    foreach (var address in postalAddress)
                    {
                        if (address != null && address.Postcode != objPostalAddress.Postcode)
                        {
                            postCode = address.Postcode;
                            break;
                        }
                    }
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return postCode;
            }
        }

        /// <summary>
        /// Check For Duplicate Address With DeliveryPoints
        /// </summary>
        /// <param name="objPostalAddress">Postal address</param>
        /// <returns>Whether the record is a duplicate or not</returns>
        public bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDataDTO objPostalAddress)
        {
            bool isDuplicate = false;

            using (loggingHelper.RMTraceManager.StartTrace("DataService.CheckForDuplicateAddressWithDeliveryPoints"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(CheckForDuplicateAddressWithDeliveryPoints);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var postalAddress = GetPostalAddressEntities(objPostalAddress).ToList();

                if (postalAddress != null && postalAddress.Count > 0)
                {
                    foreach (var address in postalAddress)
                    {
                        if (address != null && address.DeliveryPoints != null && address.DeliveryPoints.Count > 0)
                        {
                            isDuplicate = true;
                            break;
                        }
                    }
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return isDuplicate;
            }
        }

        /// <summary>
        /// Update PAF details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">PAF details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>Whether the entity has been updated or not</returns>
        public async Task<bool> UpdateAddress(PostalAddressDataDTO objPostalAddress, string strFileName, Guid deliveryPointUseIndicatorPAF)
        {
            bool isPostalAddressUpdated = false;
            PostalAddress objAddress = new PostalAddress();
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.UpdateAddress"))
                {
                    string methodName = typeof(PostalAddressDataService) + "." + nameof(UpdateAddress);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    if (objPostalAddress != null)
                    {
                        objAddress = DataContext.PostalAddresses.Where(n => n.ID == objPostalAddress.ID).SingleOrDefault();

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
                            objAddress.Postcode = objPostalAddress.Postcode;
                            objAddress.AddressType_GUID = objPostalAddress.AddressType_GUID;
                        }

                        await DataContext.SaveChangesAsync();
                        isPostalAddressUpdated = true;
                        loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    }
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), dbUpdateException.ToString());
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("PostalAddress PAF for UDPRN:", objAddress.UDPRN)));
            }
            catch (NotSupportedException notSupportedException)
            {
                // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), notSupportedException.ToString());
                notSupportedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), disposedException.ToString());
                disposedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
                throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
            }

            return isPostalAddressUpdated;
        }

        /// <summary>
        /// Filter PostalAddress based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDataDTO GetPostalAddressDetails(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressDetails"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(GetPostalAddressDetails);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Where(n => n.ID == id).FirstOrDefault();
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return GenericMapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>Guid address Guid</returns>
        public Guid CreateAddressForDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO, Guid OperationalStatus)
        {
            try
            {
                //bool isAddressLocationAvailable = false;
                //double? addLocationXCoOrdinate = 0;
                //double? addLocationYCoOrdinate = 0;

                Guid returnGuid = Guid.Empty;

                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateAddressForDeliveryPoint"))
                {
                    string methodName = typeof(PostalAddressDataService) + "." + nameof(CreateAddressForDeliveryPoint);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    if (addDeliveryPointDTO.PostalAddressDTO != null && addDeliveryPointDTO.DeliveryPointDTO != null)
                    {
                        var objPostalAddress = DataContext.PostalAddresses.SingleOrDefault(n => n.UDPRN == addDeliveryPointDTO.PostalAddressDTO.UDPRN);

                        if (objPostalAddress != null)
                        {
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
                            objPostalAddress.Postcode = addDeliveryPointDTO.PostalAddressDTO.Postcode;
                            objPostalAddress.AddressType_GUID = addDeliveryPointDTO.PostalAddressDTO.AddressType_GUID;
                        }

                        returnGuid = objPostalAddress.ID;
                        DataContext.SaveChanges();
                    }

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    // return new CreateDeliveryPointModelDTO { ID = returnGuid, IsAddressLocationAvailable = isAddressLocationAvailable, XCoordinate = addLocationXCoOrdinate, YCoordinate = addLocationYCoOrdinate };
                    return returnGuid;
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for UDPRN:", addDeliveryPointDTO.PostalAddressDTO.UDPRN)));
            }
        }

        /// <summary>
        /// Get PostalAddress
        /// </summary>
        /// <param name="addressGuids"></param>
        /// <returns></returns>
        public async Task<List<PostalAddressDataDTO>> GetPostalAddresses(List<Guid> addressGuids)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddresses"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(GetPostalAddresses);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                var addressDetails = await DataContext.PostalAddresses.Where(pa => addressGuids.Contains(pa.ID)).ToListAsync();

                ConfigureMapper();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Mapper.Map<List<PostalAddress>, List<PostalAddressDataDTO>>(addressDetails);
            }
        }

        /// <summary>
        /// Get Postal Address on UDPRN value
        /// </summary>
        /// <param name="udprn">udprn value of PostalAddress</param>
        /// <param name="pafGuid">pafGuid as Address Type Guid</param>
        /// <returns></returns>
        public async Task<PostalAddressDTO> GetPAFAddress(int udprn, Guid pafGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPAFAddress"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(GetPAFAddress);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                PostalAddress postalAddress = await DataContext.PostalAddresses.Where(pa => pa.UDPRN == udprn && pa.AddressType_GUID == pafGuid).SingleOrDefaultAsync();

                ConfigureMapper();

                PostalAddressDataDTO postalAddressDataDTO = Mapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return GenericMapper.Map<PostalAddressDataDTO, PostalAddressDTO>(postalAddressDataDTO);
            }
        }

        /// <summary>
        /// Get the Postal Address Entity based on the Address Fields
        /// </summary>
        /// <param name="objPostalAddress">Postal Address Transfer Object</param>
        /// <param name="addressTypeNYBGuid">Static NYB Address Type Guid</param>
        /// <returns>Posta Address matching the criteria</returns>
        private IQueryable<PostalAddress> GetPostalAddressEntities(PostalAddressDataDTO objPostalAddress, Guid addressTypeNYBGuid = new Guid())
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressEntities"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(GetPostalAddressEntities);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                IQueryable<PostalAddress> postalAddress = null;

                if (addressTypeNYBGuid == Guid.Empty)
                {
                    postalAddress = DataContext.PostalAddresses.AsNoTracking()
                            .Where(n => n.AddressType_GUID == addressTypeNYBGuid);
                }
                else
                {
                    postalAddress = DataContext.PostalAddresses.AsNoTracking().Where(n => n.Postcode == objPostalAddress.Postcode);
                }

                if (!string.IsNullOrEmpty(objPostalAddress.BuildingName))
                {
                    postalAddress = postalAddress.Where(n => n.BuildingName.Equals(objPostalAddress.BuildingName,
                        StringComparison.OrdinalIgnoreCase));
                }

                if (objPostalAddress.BuildingNumber != null)
                {
                    postalAddress = postalAddress.Where(n => n.BuildingNumber == objPostalAddress.BuildingNumber);
                }

                if (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName))
                {
                    postalAddress = postalAddress.Where(n => n.SubBuildingName.Equals(objPostalAddress.SubBuildingName,
                        StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.OrganisationName))
                {
                    postalAddress = postalAddress.Where(n => n.OrganisationName.Equals(objPostalAddress.OrganisationName,
                        StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.DepartmentName))
                {
                    postalAddress = postalAddress.Where(n => n.DepartmentName.Equals(objPostalAddress.DepartmentName,
                        StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare))
                {
                    postalAddress = postalAddress.Where(n => n.Thoroughfare.Equals(
                        objPostalAddress.Thoroughfare,
                        StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare))
                {
                    postalAddress =
                        postalAddress.Where(n => n.DependentThoroughfare.Equals(objPostalAddress.DependentThoroughfare,
                            StringComparison.OrdinalIgnoreCase));
                }

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return postalAddress;
            }
        }

        /// <summary>
        /// Get the Postal Address Entity based on the Address fields.
        /// </summary>
        /// <param name="objPostalAddress">Postal Address Object</param>
        /// <returns>Postal Address Entity</returns>
        private async Task<PostalAddress> GetPostalAddressEntity(PostalAddressDataDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressEntity"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(GetPostalAddressEntity);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                PostalAddress postalAddress = await DataContext.PostalAddresses.AsNoTracking().FirstOrDefaultAsync(n => n.Postcode == objPostalAddress.Postcode
                                                 && ((n.BuildingName ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.BuildingName)
                                                          ? objPostalAddress.BuildingName
                                                          : null))
                                                     ||
                                                     ((!string.IsNullOrEmpty(n.BuildingName) ? n.BuildingName : string.Empty) ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.BuildingName)
                                                          ? objPostalAddress.BuildingName
                                                          : string.Empty)))

                                                 && ((n.BuildingNumber == objPostalAddress.BuildingNumber)
                                                     ||
                                                     ((n.BuildingNumber ?? 0) == (objPostalAddress.BuildingNumber ?? 0)))

                                                 && ((n.SubBuildingName ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName)
                                                          ? objPostalAddress.SubBuildingName
                                                          : null))
                                                     ||
                                                     ((!string.IsNullOrEmpty(n.SubBuildingName)
                                                          ? n.SubBuildingName
                                                          : string.Empty) ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName)
                                                          ? objPostalAddress.SubBuildingName
                                                          : string.Empty)))
                                                 && ((n.OrganisationName ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.OrganisationName)
                                                          ? objPostalAddress.OrganisationName
                                                          : null))
                                                     ||
                                                     ((!string.IsNullOrEmpty(n.OrganisationName)
                                                          ? n.OrganisationName
                                                          : string.Empty) ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.OrganisationName)
                                                          ? objPostalAddress.OrganisationName
                                                          : string.Empty)))
                                                 && ((n.DepartmentName ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.DepartmentName)
                                                          ? objPostalAddress.DepartmentName
                                                          : null))
                                                     ||
                                                     ((!string.IsNullOrEmpty(n.DepartmentName) ? n.DepartmentName : string.Empty) ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.DepartmentName)
                                                          ? objPostalAddress.DepartmentName
                                                          : string.Empty)))
                                                 && ((n.Thoroughfare ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare)
                                                          ? objPostalAddress.Thoroughfare
                                                          : null))
                                                     ||
                                                     ((!string.IsNullOrEmpty(n.Thoroughfare) ? n.Thoroughfare : string.Empty) ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare)
                                                          ? objPostalAddress.Thoroughfare
                                                          : string.Empty)))
                                                 && ((n.DependentThoroughfare ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare)
                                                          ? objPostalAddress.DependentThoroughfare
                                                          : null))
                                                     ||
                                                     ((!string.IsNullOrEmpty(n.DependentThoroughfare)
                                                          ? n.DependentThoroughfare
                                                          : string.Empty) ==
                                                      (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare)
                                                          ? objPostalAddress.DependentThoroughfare
                                                          : string.Empty))));

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                return postalAddress;
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
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressEntity"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(LogFileException);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO()
                {
                    FileID = Guid.NewGuid(),
                    UDPRN = uDPRN,
                    AmendmentType = PostalAddressConstants.INSERT,
                    FileName = strFileName,
                    FileProcessing_TimeStamp = DateTime.UtcNow,
                    FileType = fileType,
                    ErrorMessage = strException,
                    SuccessFlag = false
                };
                fileProcessingLog.LogFileException(objFileProcessingLog);
                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
            }
        }

        /// <summary>
        /// Common steps to configure mapper for PostalAddress
        /// </summary>
        private static void ConfigureMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<PostalAddress, PostalAddressDataDTO>().MaxDepth(1);
                cfg.CreateMap<PostalAddressStatus, PostalAddressStatusDataDTO>().MaxDepth(2);
                cfg.CreateMap<DeliveryPoint, DeliveryPointDataDTO>().MaxDepth(1);
            });
            Mapper.Configuration.CreateMapper();
        }

        /* To be a part of Unit manager

        /// <summary>
        /// Filter PostalAddress based on post code. Also, it fetches the route information based on
        /// the postcode and if there are no matching routes then the routes for the unit is fetched.
        /// </summary>
        /// <param name="selectedItem">selectedItem</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postal Address</returns>
        public async Task<List<PostalAddressDTO>> GetPostalAddressDetails(string selectedItem, Guid unitGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressDetails(string, Guid)"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<string> lstPocodes = new List<string>();
                List<PostalAddressDTO> postalAddressDTO = new List<PostalAddressDTO>();
                string[] selectedItems = selectedItem.Split(',');
                string postCode = string.Empty;
                string streetName = string.Empty;
                List<PostalAddress> postalAddress = null;

                if (selectedItems.Count() == 2)
                {
                    postCode = selectedItems[1].Trim();
                    streetName = selectedItems[0].Trim();
                    postalAddress = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                           join pc in DataContext.Postcodes.AsNoTracking()
                                           on pa.PostCodeGUID equals pc.ID
                                           join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
                                           where pc.PostcodeUnit == postCode && pa.Thoroughfare == streetName && ul.Unit_GUID == unitGuid
                                           select pa).ToListAsync();
                }
                else
                {
                    postCode = selectedItems[0].Trim();
                    postalAddress = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                           join pc in DataContext.Postcodes.AsNoTracking()
                                           on pa.PostCodeGUID equals pc.ID
                                           join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
                                           where pc.PostcodeUnit == postCode && ul.Unit_GUID == unitGuid
                                           select pa).ToListAsync();
                }

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
                            }

                            if (paDTO.RouteDetails.All(b => b.DisplayText != PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim()))
                            {
                                paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.DeliveryRoute.ID });
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
                            }

                            if (paDTO.RouteDetails.All(b => b.DisplayText != SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim()))
                            {
                                paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.DeliveryRoute.ID });
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
                    postalAddressDTO[0].RouteDetails = new List<BindingEntity>(routeDetails.Distinct().OrderBy(n => n.DisplayText));
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postalAddressDTO;
            }
        }
        */

        // Old code begins here //

        ///// <summary>
        ///// Delete postal Address records do not have an associated Delivery Point
        ///// </summary>
        ///// <param name="lstUDPRN">List of UDPRN</param>
        ///// <param name="addressType">NYB</param>
        ///// <returns>true or false</returns>
        //public async Task<bool> DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType)
        //{
        //    try
        //    {
        //        bool isPostalAddressDeleted = false;
        //        string nybDeleteMsg = Constants.NYBErrorMessageForDelete;
        //        if (lstUDPRN != null && lstUDPRN.Any())
        //        {
        //            var lstAddress = await DataContext.PostalAddresses.Include(m => m.DeliveryPoints).Where(n => !lstUDPRN.Contains(n.UDPRN.Value) && n.AddressType_GUID == addressType).ToListAsync();
        //            if (lstAddress.Count > 0)
        //            {
        //                lstAddress.ForEach(postalAddressEntity =>
        //                {
        //                    if (postalAddressEntity.DeliveryPoints != null && postalAddressEntity.DeliveryPoints.Count > 0)
        //                    {
        //                        isPostalAddressDeleted = false;
        //                        this.loggingHelper.Log(string.Format(nybDeleteMsg, postalAddressEntity.UDPRN), TraceEventType.Information);
        //                    }
        //                    else
        //                    {
        //                        DataContext.PostalAddresses.Remove(postalAddressEntity);
        //                    }
        //                });
        //                await DataContext.SaveChangesAsync();
        //                isPostalAddressDeleted = true;
        //            }
        //        }

        //        return isPostalAddressDeleted;
        //    }
        //    catch (DbUpdateException dbUpdateException)
        //    {
        //        throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlDeleteException, string.Concat("PostalAdresses with UPPRN:", string.Join(",", lstUDPRN))));
        //    }
        //}

        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        //public async Task<bool> SaveAddress(PostalAddressDataDTO objPostalAddressDTO, string strFileName)
        //{
        //    bool isPostalAddressInserted = false;
        //    PostalAddress objPostalAddress = default(PostalAddress);
        //    PostalAddress entity = new PostalAddress();
        //    if (objPostalAddress != null)
        //    {
        //        try
        //        {
        //            objPostalAddress = await DataContext.PostalAddresses.Where(n => n.UDPRN == objPostalAddressDTO.UDPRN).SingleOrDefaultAsync();

        //            if (objPostalAddress != null)
        //            {
        //                objPostalAddress.Postcode = objPostalAddressDTO.Postcode;
        //                objPostalAddress.PostTown = objPostalAddressDTO.PostTown;
        //                objPostalAddress.DependentLocality = objPostalAddressDTO.DependentLocality;
        //                objPostalAddress.DoubleDependentLocality = objPostalAddressDTO.DoubleDependentLocality;
        //                objPostalAddress.Thoroughfare = objPostalAddressDTO.Thoroughfare;
        //                objPostalAddress.DependentThoroughfare = objPostalAddressDTO.DependentThoroughfare;
        //                objPostalAddress.BuildingNumber = objPostalAddressDTO.BuildingNumber;
        //                objPostalAddress.BuildingName = objPostalAddressDTO.BuildingName;
        //                objPostalAddress.SubBuildingName = objPostalAddressDTO.SubBuildingName;
        //                objPostalAddress.POBoxNumber = objPostalAddressDTO.POBoxNumber;
        //                objPostalAddress.DepartmentName = objPostalAddressDTO.DepartmentName;
        //                objPostalAddress.OrganisationName = objPostalAddressDTO.OrganisationName;
        //                objPostalAddress.UDPRN = objPostalAddressDTO.UDPRN;
        //                objPostalAddress.PostcodeType = objPostalAddressDTO.PostcodeType;
        //                objPostalAddress.SmallUserOrganisationIndicator = objPostalAddressDTO.SmallUserOrganisationIndicator;
        //                objPostalAddress.DeliveryPointSuffix = objPostalAddressDTO.DeliveryPointSuffix;
        //                objPostalAddress.Postcode = objPostalAddressDTO.Postcode;
        //            }
        //            else
        //            {
        //                objPostalAddress.ID = Guid.NewGuid();
        //                entity = GenericMapper.Map<PostalAddressDataDTO, PostalAddress>(objPostalAddressDTO);
        //                DataContext.PostalAddresses.Add(entity);
        //            }

        //            await DataContext.SaveChangesAsync();
        //            isPostalAddressInserted = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            if (objPostalAddress != null)
        //            {
        //                DataContext.Entry(objPostalAddress).State = EntityState.Unchanged;
        //            }
        //            else
        //            {
        //                DataContext.Entry(entity).State = EntityState.Unchanged;
        //            }

        //            if (objPostalAddressDTO.UDPRN != null)
        //            {
        //                LogFileException(objPostalAddressDTO.UDPRN.Value, strFileName, FileType.Nyb.ToString(), ex.ToString());
        //            }
        //        }
        //    }

        //    return isPostalAddressInserted;
        //}

        /// <summary>
        /// Insert PAF details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">PAF details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        //public async Task<bool> InsertAddress(PostalAddressDataDTO objPostalAddress, string strFileName)
        //{
        //    bool isPostalAddressInserted = false;
        //    PostalAddress objAddress = new PostalAddress();
        //    try
        //    {
        //        using (loggingHelper.RMTraceManager.StartTrace("DataService.InsertAddress"))
        //        {
        //            string methodName = MethodHelper.GetActualAsyncMethodName();
        //            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

        //            if (objPostalAddress != null)
        //            {
        //                Mapper.Initialize(cfg =>
        //                {
        //                    cfg.CreateMap<PostalAddressDataDTO, PostalAddress>();
        //                    cfg.CreateMap<PostalAddressStatusDTO, PostalAddressStatus>();
        //                });
        //                Mapper.Configuration.CreateMapper();

        //                objAddress = Mapper.Map<PostalAddressDataDTO, PostalAddress>(objPostalAddress);
        //                DataContext.PostalAddresses.Add(objAddress);

        //                await DataContext.SaveChangesAsync();
        //                isPostalAddressInserted = true;
        //            }
        //            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
        //        }
        //    }
        //    catch (DbUpdateException dbUpdateException)
        //    {
        //        DataContext.Entry(objAddress).State = EntityState.Unchanged;
        //        LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), dbUpdateException.ToString());
        //        throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("PostalAddress PAF for UDPRN:", objAddress.UDPRN)));
        //    }
        //    catch (NotSupportedException notSupportedException)
        //    {
        //        DataContext.Entry(objAddress).State = EntityState.Unchanged;
        //        LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), notSupportedException.ToString());
        //        notSupportedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
        //        throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
        //    }
        //    catch (ObjectDisposedException disposedException)
        //    {
        //        DataContext.Entry(objAddress).State = EntityState.Unchanged;
        //        LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), disposedException.ToString());
        //        disposedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
        //        throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
        //    }

        //    return isPostalAddressInserted;
        //}

        ///// <summary>
        ///// Get Postal address details depending on the UDPRN
        ///// </summary>
        ///// <param name="uDPRN">UDPRN id</param>
        ///// <returns>returns PostalAddress object</returns>
        //public async Task<PostalAddressDataDTO> GetPostalAddress(int? uDPRN)
        //{
        //    using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddress"))
        //    {
        //        string methodName = MethodHelper.GetActualAsyncMethodName();
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

        //        var postalAddress = await DataContext.PostalAddresses.Include(m => m.PostalAddressStatus).Where(n => n.UDPRN == uDPRN).SingleOrDefaultAsync();

        //        Mapper.Initialize(cfg =>
        //        {
        //            cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
        //            cfg.CreateMap<PostalAddressStatus, PostalAddressStatusDTO>();
        //        });
        //        Mapper.Configuration.CreateMapper();

        //        var dtoPostalAddress = Mapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
        //        return dtoPostalAddress;
        //    }
        //}

        /// <summary>
        /// Get Postal address details depending on the address fields such as BuildingName and etc
        /// </summary>
        /// <param name="objPostalAddress">Postal address</param>
        /// <returns>returns PostalAddress object</returns>
        //public async Task<PostalAddressDataDTO> GetPostalAddress(PostalAddressDataDTO objPostalAddress)
        //{
        //    using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddress"))
        //    {
        //        string methodName = MethodHelper.GetActualAsyncMethodName();
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

        //        var postalAddress = await DataContext.PostalAddresses.AsNoTracking().Include(l => l.PostalAddressStatus)//.Include(m => m.DeliveryPoints)
        //        .FirstOrDefaultAsync(n => n.Postcode == objPostalAddress.Postcode
        //                             && ((n.BuildingName ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.BuildingName)
        //                                      ? objPostalAddress.BuildingName
        //                                      : null))
        //                                 ||
        //                                 ((!string.IsNullOrEmpty(n.BuildingName) ? n.BuildingName : string.Empty) ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.BuildingName)
        //                                      ? objPostalAddress.BuildingName
        //                                      : string.Empty)))

        //                             && ((n.BuildingNumber == objPostalAddress.BuildingNumber)
        //                                 ||
        //                                 ((n.BuildingNumber ?? 0) == (objPostalAddress.BuildingNumber ?? 0)))

        //                             && ((n.SubBuildingName ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName)
        //                                      ? objPostalAddress.SubBuildingName
        //                                      : null))
        //                                 ||
        //                                 ((!string.IsNullOrEmpty(n.SubBuildingName)
        //                                      ? n.SubBuildingName
        //                                      : string.Empty) ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName)
        //                                      ? objPostalAddress.SubBuildingName
        //                                      : string.Empty)))
        //                             && ((n.OrganisationName ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.OrganisationName)
        //                                      ? objPostalAddress.OrganisationName
        //                                      : null))
        //                                 ||
        //                                 ((!string.IsNullOrEmpty(n.OrganisationName)
        //                                      ? n.OrganisationName
        //                                      : string.Empty) ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.OrganisationName)
        //                                      ? objPostalAddress.OrganisationName
        //                                      : string.Empty)))
        //                             && ((n.DepartmentName ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.DepartmentName)
        //                                      ? objPostalAddress.DepartmentName
        //                                      : null))
        //                                 ||
        //                                 ((!string.IsNullOrEmpty(n.DepartmentName) ? n.DepartmentName : string.Empty) ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.DepartmentName)
        //                                      ? objPostalAddress.DepartmentName
        //                                      : string.Empty)))
        //                             && ((n.Thoroughfare ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare)
        //                                      ? objPostalAddress.Thoroughfare
        //                                      : null))
        //                                 ||
        //                                 ((!string.IsNullOrEmpty(n.Thoroughfare) ? n.Thoroughfare : string.Empty) ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare)
        //                                      ? objPostalAddress.Thoroughfare
        //                                      : string.Empty)))
        //                             && ((n.DependentThoroughfare ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare)
        //                                      ? objPostalAddress.DependentThoroughfare
        //                                      : null))
        //                                 ||
        //                                 ((!string.IsNullOrEmpty(n.DependentThoroughfare)
        //                                      ? n.DependentThoroughfare
        //                                      : string.Empty) ==
        //                                  (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare)
        //                                      ? objPostalAddress.DependentThoroughfare
        //                                      : string.Empty))));

        //        Mapper.Initialize(cfg =>
        //        {
        //            cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
        //            cfg.CreateMap<PostalAddressStatus, PostalAddressStatusDTO>();
        //        });
        //        Mapper.Configuration.CreateMapper();

        //        var dtoPostalAddress = Mapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);
        //        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
        //        return dtoPostalAddress;
        //    }
        //}

        /// <summary>
        /// Checking for duplicatesthat already exists in FMO as a NYB record
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <param name="addressTypeNYBGuid">Reference data Guid of NYB</param>
        /// <returns>string</returns>
        //public string CheckForDuplicateNybRecords(PostalAddressDataDTO objPostalAddress, Guid addressTypeNYBGuid)
        //{
        //    string postCode = string.Empty;

        //    var postalAddress = DataContext.PostalAddresses.AsNoTracking()
        //        .Where(n => n.AddressType_GUID == addressTypeNYBGuid);

        //    if (!string.IsNullOrEmpty(objPostalAddress.BuildingName))
        //    {
        //        postalAddress = postalAddress.Where(n => n.BuildingName.Equals(objPostalAddress.BuildingName,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (objPostalAddress.BuildingNumber != null)
        //    {
        //        postalAddress = postalAddress.Where(n => n.BuildingNumber == objPostalAddress.BuildingNumber);
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName))
        //    {
        //        postalAddress = postalAddress.Where(n => n.SubBuildingName.Equals(objPostalAddress.SubBuildingName,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.OrganisationName))
        //    {
        //        postalAddress = postalAddress.Where(n => n.OrganisationName.Equals(objPostalAddress.OrganisationName,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.DepartmentName))
        //    {
        //        postalAddress = postalAddress.Where(n => n.DepartmentName.Equals(objPostalAddress.DepartmentName,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare))
        //    {
        //        postalAddress = postalAddress.Where(n => n.Thoroughfare.Equals(
        //            objPostalAddress.Thoroughfare,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare))
        //    {
        //        postalAddress =
        //            postalAddress.Where(n => n.DependentThoroughfare.Equals(objPostalAddress.DependentThoroughfare,
        //                StringComparison.OrdinalIgnoreCase));
        //    }

        //    var addressDetails = postalAddress.ToList();
        //    if (addressDetails != null && addressDetails.Count > 0)
        //    {
        //        foreach (var address in addressDetails)
        //        {
        //            if (address != null && address.Postcode != objPostalAddress.Postcode)
        //            {
        //                postCode = address.Postcode;
        //                break;
        //            }
        //        }
        //    }

        //    return postCode;
        //}

        ///// <summary>
        ///// Check For Duplicate Address With DeliveryPoints
        ///// </summary>
        ///// <param name="objPostalAddress">objPostalAddress</param>
        ///// <returns>bool</returns>
        //public bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDataDTO objPostalAddress)
        //{
        //    bool isDuplicate = false;

        //    var postalAddress = DataContext.PostalAddresses.AsNoTracking().Include(m => m.DeliveryPoints)
        //        .Where(n => n.Postcode == objPostalAddress.Postcode);

        //    if (!string.IsNullOrEmpty(objPostalAddress.BuildingName))
        //    {
        //        postalAddress = postalAddress.Where(n => n.BuildingName.Equals(objPostalAddress.BuildingName,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (objPostalAddress.BuildingNumber != null)
        //    {
        //        postalAddress = postalAddress.Where(n => n.BuildingNumber == objPostalAddress.BuildingNumber);
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.SubBuildingName))
        //    {
        //        postalAddress = postalAddress.Where(n => n.SubBuildingName.Equals(objPostalAddress.SubBuildingName,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.OrganisationName))
        //    {
        //        postalAddress = postalAddress.Where(n => n.OrganisationName.Equals(objPostalAddress.OrganisationName,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.DepartmentName))
        //    {
        //        postalAddress = postalAddress.Where(n => n.DepartmentName.Equals(objPostalAddress.DepartmentName,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.Thoroughfare))
        //    {
        //        postalAddress = postalAddress.Where(n => n.Thoroughfare.Equals(objPostalAddress.Thoroughfare,
        //            StringComparison.OrdinalIgnoreCase));
        //    }

        //    if (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare))
        //    {
        //        postalAddress =
        //            postalAddress.Where(n => n.DependentThoroughfare.Equals(objPostalAddress.DependentThoroughfare,
        //                StringComparison.OrdinalIgnoreCase));
        //    }

        //    var addressDetails = postalAddress.ToList();
        //    if (addressDetails != null && addressDetails.Count > 0)
        //    {
        //        foreach (var address in addressDetails)
        //        {
        //            if (address != null && address.DeliveryPoints != null && address.DeliveryPoints.Count > 0)
        //            {
        //                isDuplicate = true;
        //                break;
        //            }
        //        }
        //    }

        //    return isDuplicate;
        //}

        /// <summary>
        /// Update PAF details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">PAF details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        //public async Task<bool> UpdateAddress(PostalAddressDataDTO objPostalAddress, string strFileName, Guid deliveryPointUseIndicatorPAF)
        //{
        //    bool isPostalAddressUpdated = false;
        //    PostalAddress objAddress = new PostalAddress();
        //    try
        //    {
        //        using (loggingHelper.RMTraceManager.StartTrace("DataService.UpdateAddress"))
        //        {
        //            string methodName = MethodHelper.GetActualAsyncMethodName();
        //            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

        //            if (objPostalAddress != null)
        //            {
        //                //objAddress = DataContext.PostalAddresses.Include(m => m.DeliveryPoints).Where(n => n.ID == objPostalAddress.ID).SingleOrDefault();
        //                objAddress = DataContext.PostalAddresses.Include(m => m.PostalAddressStatus).Where(n => n.ID == objPostalAddress.ID).SingleOrDefault();

        //                if (objAddress != null)
        //                {
        //                    objAddress.Postcode = objPostalAddress.Postcode;
        //                    objAddress.PostTown = objPostalAddress.PostTown;
        //                    objAddress.DependentLocality = objPostalAddress.DependentLocality;
        //                    objAddress.DoubleDependentLocality = objPostalAddress.DoubleDependentLocality;
        //                    objAddress.Thoroughfare = objPostalAddress.Thoroughfare;
        //                    objAddress.DependentThoroughfare = objPostalAddress.DependentThoroughfare;
        //                    objAddress.BuildingNumber = objPostalAddress.BuildingNumber;
        //                    objAddress.BuildingName = objPostalAddress.BuildingName;
        //                    objAddress.SubBuildingName = objPostalAddress.SubBuildingName;
        //                    objAddress.POBoxNumber = objPostalAddress.POBoxNumber;
        //                    objAddress.DepartmentName = objPostalAddress.DepartmentName;
        //                    objAddress.OrganisationName = objPostalAddress.OrganisationName;
        //                    objAddress.UDPRN = objPostalAddress.UDPRN;
        //                    objAddress.PostcodeType = objPostalAddress.PostcodeType;
        //                    objAddress.SmallUserOrganisationIndicator = objPostalAddress.SmallUserOrganisationIndicator;
        //                    objAddress.DeliveryPointSuffix = objPostalAddress.DeliveryPointSuffix;
        //                    objAddress.Postcode = objPostalAddress.Postcode;
        //                    objAddress.AddressType_GUID = objPostalAddress.AddressType_GUID;

        //                    //if (objAddress.PostalAddressStatus != null && objAddress.PostalAddressStatus.Count > 0)
        //                    //{
        //                    //}

        //                    //if (objAddress.DeliveryPoints != null && objAddress.DeliveryPoints.Count > 0)
        //                    //{
        //                    //    foreach (var objDelPoint in objAddress.DeliveryPoints)
        //                    //    {
        //                    //        if (objAddress.OrganisationName.Length > 0)
        //                    //        {
        //                    //            objDelPoint.DeliveryPointUseIndicator_GUID = deliveryPointUseIndicatorPAF;
        //                    //        }

        //                    //        // objDelPoint.UDPRN = objPostalAddress.UDPRN;
        //                    //    }
        //                    //}
        //                }

        //                await DataContext.SaveChangesAsync();
        //                isPostalAddressUpdated = true;
        //                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
        //            }
        //        }
        //    }
        //    catch (DbUpdateException dbUpdateException)
        //    {
        //        if (objAddress != null)
        //        {
        //            DataContext.Entry(objAddress).State = EntityState.Unchanged;
        //        }

        //        LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), dbUpdateException.ToString());
        //        throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("PostalAddress PAF for UDPRN:", objAddress.UDPRN)));
        //    }
        //    catch (NotSupportedException notSupportedException)
        //    {
        //        if (objAddress != null)
        //        {
        //            DataContext.Entry(objAddress).State = EntityState.Unchanged;
        //        }

        //        LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), notSupportedException.ToString());
        //        notSupportedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
        //        throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
        //    }
        //    catch (ObjectDisposedException disposedException)
        //    {
        //        if (objAddress != null)
        //        {
        //            DataContext.Entry(objAddress).State = EntityState.Unchanged;
        //        }

        //        LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), disposedException.ToString());
        //        disposedException.Data.Add("userFriendlyMessage", ErrorConstants.Err_Default);
        //        throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
        //    }

        //    return isPostalAddressUpdated;
        //}

        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postcodes</returns>
        //public async Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid, List<Guid> addresstypeIDs, List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> postCodeDTOs)
        //{
        //    try
        //    {
        //        List<string> searchdetails = new List<string>();
        //        List<Guid> postCodeGuids = new List<Guid>();
        //        postCodeDTOs.ForEach(pc => postCodeGuids.Add(pc.ID));

        //        var searchresults = await (from pa in DataContext.PostalAddresses.AsNoTracking()
        //                                       //join pc in DataContext.Postcodes.AsNoTracking() on pa.PostCodeGUID equals pc.ID
        //                                       //join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
        //                                       // TODO: Fix the entity field
        //                                   where postCodeGuids.Contains(pa.PostCodeGUID) && addresstypeIDs.Contains(pa.AddressType_GUID)
        //                                   select new { SearchResult = string.IsNullOrEmpty(pa.Thoroughfare) ? pa.Postcode : pa.Thoroughfare + "," + pa.Postcode }).Distinct().OrderBy(x => x.SearchResult).ToListAsync();

        //        return searchresults.Select(n => n.SearchResult).ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        this.loggingHelper.Log(ex, TraceEventType.Error);
        //        throw;
        //    }
        //}

        //public async Task<List<Guid>> GetPostcodeGuids(string searchText)
        //{
        //    try
        //    {
        //        var postcodeGuids = await (from pa in DataContext.PostalAddresses.AsNoTracking()
        //                                   where (pa.Thoroughfare.Contains(searchText) || pa.Postcode.Contains(searchText))
        //                                   // TODO: Fix the entity field
        //                                   select pa.PostCodeGUID).ToListAsync();

        //        return postcodeGuids;
        //    }
        //    catch (Exception ex)
        //    {
        //        this.loggingHelper.Log(ex, TraceEventType.Error);
        //        throw;
        //    }
        //}

        /* To be moved to Unit Manager
    /// <summary>
    /// Filter PostalAddress based on post code. Also, it fetches the route information based on
    /// the postcode and if there are no matching routes then the routes for the unit is fetched.
    /// </summary>
    /// <param name="selectedItem">selectedItem</param>
    /// <param name="unitGuid">unitGuid</param>
    /// <returns>List of Postal Address</returns>
    public async Task<List<PostalAddressDataDTO>> GetPostalAddressDetails(string selectedItem, Guid unitGuid, List<CommonLibrary.EntityFramework.DTO.PostCodeDTO> postcodeDTOs)
    {
        List<string> lstPocodes = new List<string>();
        List<PostalAddressDataDTO> PostalAddressDataDTO = new List<PostalAddressDataDTO>();
        string[] selectedItems = selectedItem.Split(',');
        string postCode = string.Empty;
        string streetName = string.Empty;
        List<PostalAddress> postalAddress = null;
        List<Guid> postcodeGuids = new List<Guid>();

        postcodeDTOs.ForEach(pc => postcodeGuids.Add(pc.ID));

        postalAddress = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                   // TODO: Fix the entity field
                               where postcodeGuids.Contains(pa.PostCodeGUID)
                               select pa).ToListAsync();

        if (selectedItems.Count() == 2)
        {
            postCode = selectedItems[1].Trim();
            streetName = selectedItems[0].Trim();
            postalAddress = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                       //join pc in DataContext.Postcodes.AsNoTracking()
                                       //on pa.PostCodeGUID equals pc.ID
                                       //join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
                                       //where pa.PostcodeUnit == postCode && pa.Thoroughfare == streetName && ul.Unit_GUID == unitGuid
                                   where pa.PostCodeGUID == postcodeDTO.ID
                                   select pa).ToListAsync();
        }
        else
        {
            postCode = selectedItems[0].Trim();
            postalAddress = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                   join pc in DataContext.Postcodes.AsNoTracking()
                                   on pa.PostCodeGUID equals pc.ID
                                   join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
                                   where pc.PostcodeUnit == postCode && ul.Unit_GUID == unitGuid
                                   select pa).ToListAsync();
        }

        Mapper.Initialize(cfg =>
        {
            cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
            cfg.CreateMap<PostalAddressStatus, PostalAddressStatusDTO>();
        });
        Mapper.Configuration.CreateMapper();

        return Mapper.Map<List<PostalAddress>, List<PostalAddressDataDTO>>(postalAddress);

        //postalAddress.ForEach(p => p.Postcode1.DeliveryRoutePostcodes.ToList().ForEach(d =>
        //{
        //    if (d.IsPrimaryRoute)
        //    {
        //        PostalAddressDataDTO.Where(pa => pa.Postcode == d.Postcode.PostcodeUnit).Select(pa => pa).ToList().ForEach(paDTO =>
        //        {
        //            if (paDTO.RouteDetails == null)
        //            {
        //                paDTO.RouteDetails = new List<BindingEntity>();
        //            }

        //            if (paDTO.RouteDetails.All(b => b.DisplayText != Constants.PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim()))
        //            {
        //                paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = Constants.PRIMARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.DeliveryRoute.ID });
        //            }
        //        });
        //    }
        //    else
        //    {
        //        PostalAddressDataDTO.Where(pa => pa.Postcode == d.Postcode.PostcodeUnit).Select(pa => pa).ToList().ForEach(paDTO =>
        //        {
        //            if (paDTO.RouteDetails == null)
        //            {
        //                paDTO.RouteDetails = new List<BindingEntity>();
        //            }

        //            if (paDTO.RouteDetails.All(b => b.DisplayText != Constants.SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim()))
        //            {
        //                paDTO.RouteDetails.Add(new BindingEntity() { DisplayText = Constants.SECONDARYROUTE + d.DeliveryRoute.RouteName.Trim(), Value = d.DeliveryRoute.ID });
        //            }
        //        });
        //    }
        //}));

        //var postCodes = await DataContext.UnitLocationPostcodes.AsNoTracking().Where(p => p.Unit_GUID == unitGuid).Select(s => s.PoscodeUnit_GUID).Distinct().ToListAsync();
        //if (PostalAddressDataDTO != null && PostalAddressDataDTO.Count > 0 && (PostalAddressDataDTO[0].RouteDetails == null || PostalAddressDataDTO[0].RouteDetails.Count == 0))
        //{
        //    List<BindingEntity> routeDetails = new List<BindingEntity>();
        //    var routes = await DataContext.DeliveryRoutePostcodes.AsNoTracking().Where(dr => postCodes.Contains(dr.Postcode_GUID)).ToListAsync();
        //    routes.ForEach(r =>
        //    {
        //        if (!routeDetails.Where(rd => rd.Value == r.DeliveryRoute.ID).Any())
        //        {
        //            routeDetails.Add(new BindingEntity() { DisplayText = r.DeliveryRoute.RouteName, Value = r.DeliveryRoute.ID });
        //        }
        //    });
        //    PostalAddressDataDTO[0].RouteDetails = new List<BindingEntity>(routeDetails.Distinct().OrderBy(n => n.DisplayText));
        //}
    }*/

        /*public async Task<List<Guid>> GetSelectedPostcode(string selectedItem)
        {
            string[] selectedItems = selectedItem.Split(',');
            string postCode = string.Empty;
            string streetName = string.Empty;
            List<Guid> postcodeGuids = new List<Guid>();

            if (selectedItems.Count() == 2)
            {
                postCode = selectedItems[1].Trim();
                streetName = selectedItems[0].Trim();
                postcodeGuids = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                           //join pc in DataContext.Postcodes.AsNoTracking()
                                           //on pa.PostCodeGUID equals pc.ID
                                           //join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
                                       where pa.Postcode == postCode && pa.Thoroughfare == streetName
                                       // TODO: Fix the entity field
                                       select pa.PostCodeGUID).ToListAsync();
            }
            else
            {
                postCode = selectedItems[0].Trim();
                postcodeGuids = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                           //join pc in DataContext.Postcodes.AsNoTracking()
                                           //on pa.PostCodeGUID equals pc.ID
                                           //join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
                                       where pa.Postcode == postCode
                                       select pa.PostCodeGUID).ToListAsync();
            }

            return postcodeGuids;
        }*/

        /// <summary>
        /// Filter PostalAddress based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        //public PostalAddressDataDTO GetPostalAddressDetails(Guid id)
        //{
        //    try
        //    {
        //        var postalAddress = DataContext.PostalAddresses.AsNoTracking().Where(n => n.ID == id).FirstOrDefault();
        //        return GenericMapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);
        //    }
        //    catch (Exception ex)
        //    {
        //        this.loggingHelper.Log(ex, TraceEventType.Error);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        //public CreateDeliveryPointModelDTO CreateAddressForDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO, Guid OperationalStatus)
        //{
        //    try
        //    {
        //        bool isAddressLocationAvailable = false;
        //        Guid returnGuid = Guid.Empty;
        //        double? addLocationXCoOrdinate = 0;
        //        double? addLocationYCoOrdinate = 0;
        //        if (addDeliveryPointDTO.PostalAddressDTO != null && addDeliveryPointDTO.DeliveryPointDTO != null)
        //        {
        //            var objPostalAddress = DataContext.PostalAddresses.SingleOrDefault(n => n.UDPRN == addDeliveryPointDTO.PostalAddressDTO.UDPRN);
        //            //var objAddressLocation = DataContext.AddressLocations.SingleOrDefault(n => n.UDPRN == addDeliveryPointDTO.PostalAddressDTO.UDPRN);

        //            //DeliveryPoint objDeliveryPoint = new DeliveryPoint()
        //            //{
        //            //    // ID = Guid.NewGuid(),
        //            //    // UDPRN = addDeliveryPointDTO.PostalAddressDataDTO.UDPRN,
        //            //    DeliveryPointUseIndicator_GUID = addDeliveryPointDTO.DeliveryPointDTO.DeliveryPointUseIndicator_GUID,
        //            //    MultipleOccupancyCount = addDeliveryPointDTO.DeliveryPointDTO.MultipleOccupancyCount,
        //            //    MailVolume = addDeliveryPointDTO.DeliveryPointDTO.MailVolume
        //            //};

        //            //if (objAddressLocation != null)
        //            //{
        //            //    SqlGeometry deliveryPointSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(objAddressLocation.LocationXY.AsBinary()), Constants.BNGCOORDINATESYSTEM);
        //            //    //objDeliveryPoint.LocationXY = objAddressLocation.LocationXY;
        //            //    //objDeliveryPoint.Latitude = objAddressLocation.Lattitude;
        //            //    //objDeliveryPoint.Longitude = objAddressLocation.Longitude;
        //            //    //objDeliveryPoint.Positioned = true;
        //            //    isAddressLocationAvailable = true;
        //            //    addLocationXCoOrdinate = deliveryPointSqlGeometry.STX.Value;
        //            //    addLocationYCoOrdinate = deliveryPointSqlGeometry.STY.Value;
        //            //}

        //            if (objPostalAddress != null)
        //            {
        //                objPostalAddress.BuildingNumber = addDeliveryPointDTO.PostalAddressDTO.BuildingNumber;
        //                objPostalAddress.BuildingName = addDeliveryPointDTO.PostalAddressDTO.BuildingName;
        //                objPostalAddress.SubBuildingName = addDeliveryPointDTO.PostalAddressDTO.SubBuildingName;
        //                objPostalAddress.POBoxNumber = addDeliveryPointDTO.PostalAddressDTO.POBoxNumber;
        //                objPostalAddress.DepartmentName = addDeliveryPointDTO.PostalAddressDTO.DepartmentName;
        //                objPostalAddress.OrganisationName = addDeliveryPointDTO.PostalAddressDTO.OrganisationName;
        //                objPostalAddress.UDPRN = addDeliveryPointDTO.PostalAddressDTO.UDPRN;
        //                objPostalAddress.PostcodeType = addDeliveryPointDTO.PostalAddressDTO.PostcodeType;
        //                objPostalAddress.SmallUserOrganisationIndicator = addDeliveryPointDTO.PostalAddressDTO.SmallUserOrganisationIndicator;
        //                objPostalAddress.DeliveryPointSuffix = addDeliveryPointDTO.PostalAddressDTO.DeliveryPointSuffix;
        //                // TODO: Fix the entity field
        //                objPostalAddress.PostCodeGUID = addDeliveryPointDTO.PostalAddressDTO.PostCodeGUID;
        //                objPostalAddress.AddressType_GUID = addDeliveryPointDTO.PostalAddressDTO.AddressType_GUID;
        //                //objPostalAddress.AddressStatus_GUID = addDeliveryPointDTO.PostalAddressDataDTO.AddressStatus_GUID;
        //                //objPostalAddress.DeliveryPoints.Add(objDeliveryPoint);
        //            }
        //            /*else
        //            {
        //                addDeliveryPointDTO.PostalAddressDTO.ID = Guid.NewGuid();
        //                addDeliveryPointDTO.PostalAddressDTO.PostalAddressStatus = new List<PostalAddressStatusDTO>();
        //                addDeliveryPointDTO.PostalAddressDTO.PostalAddressStatus.Add(new PostalAddressStatusDTO {
        //                    ID = Guid.NewGuid(),
        //                    PostalAddressGUID = addDeliveryPointDTO.PostalAddressDTO.ID,
        //                    StartDateTime = DateTime.UtcNow,
        //                    RowCreateDateTime = DateTime.UtcNow,
        //                    OperationalStatusGUID = OperationalStatus
        //                });

        //                Mapper.Initialize(cfg =>
        //                {
        //                    cfg.CreateMap<PostalAddressDataDTO, PostalAddress>();
        //                    cfg.CreateMap<PostalAddressStatusDTO, PostalAddressStatus>();
        //                });
        //                Mapper.Configuration.CreateMapper();

        //                var objAddress = Mapper.Map<PostalAddressDataDTO, PostalAddress>(addDeliveryPointDTO.PostalAddressDTO);

        //                DataContext.PostalAddresses.Add(objAddress);
        //                returnGuid = objAddress.ID;
        //            }*/

        //            returnGuid = objPostalAddress.ID;
        //            DataContext.SaveChanges();
        //        }

        //        return new CreateDeliveryPointModelDTO { ID = returnGuid, IsAddressLocationAvailable = isAddressLocationAvailable, XCoordinate = addLocationXCoOrdinate, YCoordinate = addLocationYCoOrdinate };
        //    }
        //    catch (DbUpdateException dbUpdateException)
        //    {
        //        throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for UDPRN:", addDeliveryPointDTO.PostalAddressDTO.UDPRN)));
        //    }
        //}

        //public async Task<List<PostalAddressDataDTO>> GetPostalAddresses(List<Guid> addressGuids)
        //{
        //    try
        //    {
        //        var addressDetails = await DataContext.PostalAddresses.Include(pa => pa.PostalAddressStatus).Where(pa => addressGuids.Contains(pa.ID)).ToListAsync();

        //        Mapper.Initialize(cfg =>
        //        {
        //            cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
        //            cfg.CreateMap<PostalAddressStatus, PostalAddressStatusDTO>();
        //            //cfg.CreateMap<DeliveryPointStatus, DeliveryPointStatusDTO>();
        //            //cfg.CreateMap<PostalAddress, PostalAddressDTO>().IgnoreAllUnmapped();
        //        });

        //        Mapper.Configuration.CreateMapper();

        //        return Mapper.Map<List<PostalAddress>, List<PostalAddressDataDTO>>(addressDetails);
        //    }
        //    catch (AggregateException ae)
        //    {
        //        foreach (var exception in ae.InnerExceptions)
        //        {
        //            loggingHelper.Log(exception, TraceEventType.Error);
        //        }

        //        var realExceptions = ae.Flatten().InnerException;
        //        throw realExceptions;
        //    }
        //}

        /// <summary>
        /// Log exception into DB if error occurs while inserting NYB,PAF,USR records in DB
        /// </summary>
        /// <param name="uDPRN">UDPRN</param>
        /// <param name="strFileName">FileName</param>
        /// <param name="fileType">Filetype</param>
        /// <param name="strException">Exception</param>
        //private void LogFileException(int uDPRN, string strFileName, string fileType, string strException)
        //{
        //    FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO()
        //    {
        //        FileID = Guid.NewGuid(),
        //        UDPRN = uDPRN,
        //        AmendmentType = INSERT,
        //        FileName = strFileName,
        //        FileProcessing_TimeStamp = DateTime.UtcNow,
        //        FileType = fileType,
        //        ErrorMessage = strException,
        //        SuccessFlag = false
        //    };
        //    fileProcessingLog.LogFileException(objFileProcessingLog);
        //}

        //public Task<bool> DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid, List<Guid> addresstypeIDs)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<List<PostalAddressDataDTO>> GetPostalAddressDetails(string selectedItem, Guid unitGuid)
        //{
        //    throw new NotImplementedException();
        //}

        //public CreateDeliveryPointModelDTO CreateAddressForDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDataDTO objPostalAddress)
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<PostalAddressDTO> GetPAFAddress(int udprn, Guid pafGuid)
        //{
        //    try
        //    {
        //        PostalAddress postalAddress = await DataContext.PostalAddresses.Where(pa => pa.UDPRN == udprn && pa.AddressType_GUID == pafGuid).SingleOrDefaultAsync();

        //        Mapper.Initialize(cfg =>
        //        {
        //            cfg.CreateMap<PostalAddress, PostalAddressDataDTO>();
        //            cfg.CreateMap<PostalAddressStatus, PostalAddressStatusDTO>();
        //        });
        //        Mapper.Configuration.CreateMapper();

        //        PostalAddressDataDTO PostalAddressDataDTO = Mapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);

        //        return GenericMapper.Map<PostalAddressDataDTO, PostalAddressDTO>(PostalAddressDataDTO);
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        //private DeliveryPoint DeliveryPointAlaisMapping(DeliveryPointDTO deliveryPointDTO)
        //{
        //    Guid deliveryPointID = Guid.NewGuid();
        //    return new DeliveryPoint()
        //    {
        //        ID = deliveryPointID,
        //        DeliveryPointUseIndicator_GUID = deliveryPointDTO.DeliveryPointUseIndicator_GUID,
        //        MultipleOccupancyCount = deliveryPointDTO.MultipleOccupancyCount,
        //        MailVolume = deliveryPointDTO.MailVolume,
        //        DeliveryPointAlias = deliveryPointDTO.DeliveryPointAliasDTO.Select(n => new DeliveryPointAlias
        //        {
        //            ID = Guid.NewGuid(),
        //            DeliveryPoint_GUID = deliveryPointID,
        //            DPAlias = n.DPAlias,
        //            Preferred = n.Preferred
        //        }).ToList()
        //    };
        //}

        // Old code ends here //
    }
}