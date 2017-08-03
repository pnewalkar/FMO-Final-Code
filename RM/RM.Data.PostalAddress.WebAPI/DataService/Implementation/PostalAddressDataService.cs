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
        /// Common steps to configure mapper for PostalAddress
        /// </summary>

        /// <summary>
        /// Delete postal Address records do not have an associated Delivery Point
        /// </summary>
        /// <param name="lstUDPRN">List of UDPRN</param>
        /// <param name="addressType">NYB</param>
        /// <returns>true or false</returns>
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
                        var lstAddress = await DataContext.PostalAddresses.Include(m => m.DeliveryPoints).Include(n => n.PostalAddressStatus).Where(n => !lstUDPRN.Contains(n.UDPRN.Value) && n.AddressType_GUID == addressType).ToListAsync();
                        if (lstAddress != null && lstAddress.Count > 0)
                        {
                            foreach (var postalAddressEntity in lstAddress)
                            {
                                if (postalAddressEntity.DeliveryPoints != null && postalAddressEntity.DeliveryPoints.Count > 0)
                                {
                                    isPostalAddressDeleted = false;
                                    this.loggingHelper.Log(string.Format(nybDeleteMsg, postalAddressEntity.UDPRN), TraceEventType.Information);
                                }
                                else
                                {
                                    DataContext.PostalAddressStatus.RemoveRange(postalAddressEntity.PostalAddressStatus);
                                    DataContext.PostalAddresses.Remove(postalAddressEntity);
                                }
                            }

                            await DataContext.SaveChangesAsync();
                            isPostalAddressDeleted = true;
                        }
                    }

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                    return isPostalAddressDeleted;
                }
                catch (DbUpdateException dbUpdateException)
                {
                    throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlDeleteException, string.Concat("PostalAdresses with UPPRN:", string.Join(",", lstUDPRN))));
                }
            }
        }

        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>Whether the record has been updated correctly</returns>
        public async Task<bool> SaveAddress(PostalAddressDataDTO objPostalAddressDataDTO, string strFileName, Guid operationalStatusGUID)
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
                             Mapper.Initialize(cfg =>
                            {
                                cfg.CreateMap<PostalAddressDataDTO, PostalAddress>();
                                cfg.CreateMap<PostalAddressStatusDataDTO, PostalAddressStatus>();
                                cfg.CreateMap<DeliveryPointDataDTO, DeliveryPoint>();
                            });
                            Mapper.Configuration.CreateMapper();

                            entity = Mapper.Map<PostalAddressDataDTO, PostalAddress>(objPostalAddressDataDTO);

                            entity.ID = Guid.NewGuid();
                            PostalAddressStatus postalAddressStatus = new PostalAddressStatus
                            {
                                ID = Guid.NewGuid(),
                                OperationalStatusGUID = operationalStatusGUID,
                                StartDateTime = DateTime.UtcNow,
                                RowCreateDateTime = DateTime.UtcNow
                            };
                            entity.PostalAddressStatus.Add(postalAddressStatus);
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), dbUpdateConcurrencyException.ToString());
                throw new DataAccessException(dbUpdateConcurrencyException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("PostalAddress PAF for UDPRN:", objAddress.UDPRN)));
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
        public async Task<bool> CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDataDTO objPostalAddress)
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
        public async Task<bool> UpdateAddress(PostalAddressDataDTO objPostalAddress, string strFileName)
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
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                // Logging exception to database as mentioned in JIRA RFMO-258, RFMO-259 and RFMO-260
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), dbUpdateConcurrencyException.ToString());
                throw new DataAccessException(dbUpdateConcurrencyException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("PostalAddress PAF for UDPRN:", objAddress.UDPRN)));
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
        /// <param name="postalAddressId">PostalAddress Unique Identifier</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDataDTO GetPostalAddressDetails(Guid postalAddressId)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressDetails"))
            {
                string methodName = typeof(PostalAddressDataService) + "." + nameof(GetPostalAddressDetails);
                loggingHelper.LogMethodEntry(methodName, priority, entryEventId);
                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Where(n => n.ID == postalAddressId).FirstOrDefault();

                ConfigureMapper();

                loggingHelper.LogMethodExit(methodName, priority, exitEventId);
                return Mapper.Map<PostalAddress, PostalAddressDataDTO>(postalAddress);
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="postalAddressDataDTO">Postal address data DTO.</param>
        /// <returns>Guid address Guid</returns>
        public Guid CreateAddressForDeliveryPoint(PostalAddressDataDTO postalAddressDataDTO)
        {
            try
            {
                Guid returnGuid = Guid.Empty;

                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateAddressForDeliveryPoint"))
                {
                    string methodName = typeof(PostalAddressDataService) + "." + nameof(CreateAddressForDeliveryPoint);
                    loggingHelper.LogMethodEntry(methodName, priority, entryEventId);

                    if (postalAddressDataDTO != null)
                    {
                        var objPostalAddress = DataContext.PostalAddresses.Include(x => x.PostalAddressStatus).SingleOrDefault(n => n.ID == postalAddressDataDTO.ID );

                        if (objPostalAddress != null)
                        {
                            objPostalAddress.BuildingNumber = postalAddressDataDTO.BuildingNumber;
                            objPostalAddress.BuildingName = postalAddressDataDTO.BuildingName;
                            objPostalAddress.SubBuildingName = postalAddressDataDTO.SubBuildingName;
                            objPostalAddress.POBoxNumber = postalAddressDataDTO.POBoxNumber;
                            objPostalAddress.DepartmentName = postalAddressDataDTO.DepartmentName;
                            objPostalAddress.OrganisationName = postalAddressDataDTO.OrganisationName;
                            objPostalAddress.UDPRN = postalAddressDataDTO.UDPRN;
                            objPostalAddress.PostcodeType = postalAddressDataDTO.PostcodeType;
                            objPostalAddress.SmallUserOrganisationIndicator = postalAddressDataDTO.SmallUserOrganisationIndicator;
                            objPostalAddress.DeliveryPointSuffix = postalAddressDataDTO.DeliveryPointSuffix;
                            objPostalAddress.Postcode = postalAddressDataDTO.Postcode;
                            objPostalAddress.AddressType_GUID = postalAddressDataDTO.AddressType_GUID;
                        }
                        else
                        {
                            Mapper.Initialize(cfg =>
                            {
                                cfg.CreateMap<PostalAddressDataDTO, PostalAddress>();
                                cfg.CreateMap<PostalAddressStatusDataDTO, PostalAddressStatus>();
                                cfg.CreateMap<PostalAddressAliasDataDTO, PostalAddressAlias>();
                                cfg.CreateMap<DeliveryPointDataDTO, DeliveryPoint>();
                            });

                            Mapper.Configuration.CreateMapper();

                            objPostalAddress = Mapper.Map<PostalAddressDataDTO, PostalAddress>(postalAddressDataDTO);

                            objPostalAddress.RowCreateDateTime = DateTime.UtcNow;

                          
                            foreach (var status in objPostalAddress.PostalAddressStatus)
                            {
                                status.RowCreateDateTime = DateTime.UtcNow;
                                status.StartDateTime = DateTime.UtcNow;
                            }

                            foreach (var alias in objPostalAddress.PostalAddressAlias)
                            {
                                alias.RowCreateDateTime = DateTime.UtcNow;
                                alias.StartDateTime = DateTime.UtcNow;
                            }



                            // add new address
                            DataContext.PostalAddresses.Add(objPostalAddress);
                        }

                        returnGuid = objPostalAddress.ID;
                        DataContext.SaveChanges();
                    }

                    loggingHelper.LogMethodExit(methodName, priority, exitEventId);

                    return returnGuid;
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for UDPRN:", postalAddressDataDTO.UDPRN)));
            }
        }

        /// <summary>
        /// Get PostalAddress on list of PostalAddress Guid
        /// </summary>
        /// <param name="addressGuids">List of PostalAddress Guid</param>
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

                if (addressTypeNYBGuid != Guid.Empty)
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
                    postalAddress =
                        postalAddress.Where(n => n.DependentThoroughfare.Equals(objPostalAddress.DependentThoroughfare, StringComparison.OrdinalIgnoreCase));
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
    }
}