﻿namespace RM.CommonLibrary.EntityFramework.DataService
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlTypes;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using CommonLibrary.Utilities.HelperMiddleware;
    using DTO.Model;
    using DTO.UIDropdowns;
    using ExceptionMiddleware;
    using LoggingMiddleware;
    using Microsoft.SqlServer.Types;
    using RM.CommonLibrary.DataMiddleware;
    using RM.CommonLibrary.EntityFramework.DataService.Interfaces;
    using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
    using RM.CommonLibrary.EntityFramework.DTO;
    using RM.CommonLibrary.EntityFramework.Entities;
    using RM.CommonLibrary.HelperMiddleware;

    /// <summary>
    /// DataService to interact with postal address entity
    /// </summary>
    public class PostalAddressDataService : DataServiceBase<PostalAddress, RMDBContext>, IPostalAddressDataService
    {
        private const string NYBErrorMessageForDelete = "Load NYB Error Message : AddressType is NYB and have an associated Delivery Point for UDPRN: {0}";
        private const string PRIMARYROUTE = "Primary - ";
        private const string SECONDARYROUTE = "Secondary - ";
        private const string DEFAULTGUID = "00000000-0000-0000-0000-000000000000";
        private const string INSERT = "I";
        private const int BNGCOORDINATESYSTEM = 27700;

        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IFileProcessingLogDataService fileProcessingLog = default(IFileProcessingLogDataService);

        public PostalAddressDataService(IDatabaseFactory<RMDBContext> databaseFactory, ILoggingHelper loggingHelper, IFileProcessingLogDataService fileProcessingLog)
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
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.DeleteNYBPostalAddress"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    bool isPostalAddressDeleted = false;
                    string nybDeleteMsg = NYBErrorMessageForDelete;
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
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                    return isPostalAddressDeleted;
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlDeleteException, string.Concat("PostalAdresses with UPPRN:", string.Join(",", lstUDPRN))));
            }
        }

        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        public async Task<bool> SaveAddress(PostalAddressDTO objPostalAddress, string strFileName)
        {
            bool isPostalAddressInserted = false;
            PostalAddress objAddress = default(PostalAddress);
            PostalAddress entity = new PostalAddress();
            if (objPostalAddress != null)
            {
                try
                {
                    using (loggingHelper.RMTraceManager.StartTrace("DataService.SaveAddress"))
                    {
                        string methodName = MethodHelper.GetActualAsyncMethodName();
                        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                        objAddress = await DataContext.PostalAddresses.Where(n => n.UDPRN == objPostalAddress.UDPRN).SingleOrDefaultAsync();
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
                        }
                        else
                        {
                            objPostalAddress.ID = Guid.NewGuid();
                            entity = GenericMapper.Map<PostalAddressDTO, PostalAddress>(objPostalAddress);
                            DataContext.PostalAddresses.Add(entity);
                        }

                        await DataContext.SaveChangesAsync();
                        isPostalAddressInserted = true;
                        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    }
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

                    if (objPostalAddress.UDPRN != null)
                    {
                        LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Nyb.ToString(), ex.ToString());
                    }
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
        public async Task<bool> InsertAddress(PostalAddressDTO objPostalAddress, string strFileName)
        {
            bool isPostalAddressInserted = false;
            PostalAddress objAddress = new PostalAddress();
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.InsertAddress"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    if (objPostalAddress != null)
                    {
                        objAddress = GenericMapper.Map<PostalAddressDTO, PostalAddress>(objPostalAddress);
                        DataContext.PostalAddresses.Add(objAddress);

                        await DataContext.SaveChangesAsync();
                        isPostalAddressInserted = true;
                    }
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                DataContext.Entry(objAddress).State = EntityState.Unchanged;
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), dbUpdateException.ToString());
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("PostalAddress PAF for UDPRN:", objAddress.UDPRN)));
            }
            catch (NotSupportedException notSupportedException)
            {
                DataContext.Entry(objAddress).State = EntityState.Unchanged;
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), notSupportedException.ToString());
                notSupportedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                DataContext.Entry(objAddress).State = EntityState.Unchanged;
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), disposedException.ToString());
                disposedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
            }

            return isPostalAddressInserted;
        }

        /// <summary>
        /// Get Postal address details depending on the UDPRN
        /// </summary>
        /// <param name="uDPRN">UDPRN id</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDTO> GetPostalAddress(int? uDPRN)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddress(udprn)"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var postalAddress = await DataContext.PostalAddresses.Where(n => n.UDPRN == uDPRN).SingleOrDefaultAsync();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                return GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);
            }
        }

        /// <summary>
        /// Get Postal address details depending on the address fields such as BuildingName and etc
        /// </summary>
        /// <param name="objPostalAddress">Postal address</param>
        /// <returns>returns PostalAddress object</returns>
        public async Task<PostalAddressDTO> GetPostalAddress(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddress(objPostaladdress)"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var postalAddress = await DataContext.PostalAddresses.AsNoTracking().Include(m => m.DeliveryPoints)
                .FirstOrDefaultAsync(n => n.Postcode == objPostalAddress.Postcode
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
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                return GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);
            }
        }

        /// <summary>
        /// Checking for duplicatesthat already exists in FMO as a NYB record
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <param name="addressTypeNYBGuid">Reference data Guid of NYB</param>
        /// <returns>string</returns>
        public string CheckForDuplicateNybRecords(PostalAddressDTO objPostalAddress, Guid addressTypeNYBGuid)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.CheckForDuplicateNybRecords"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                string postCode = string.Empty;

                var postalAddress = DataContext.PostalAddresses.AsNoTracking()
                    .Where(n => n.AddressType_GUID == addressTypeNYBGuid);

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

                var addressDetails = postalAddress.ToList();
                if (addressDetails != null && addressDetails.Count > 0)
                {
                    foreach (var address in addressDetails)
                    {
                        if (address != null && address.Postcode != objPostalAddress.Postcode)
                        {
                            postCode = address.Postcode;
                            break;
                        }
                    }
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postCode;
            }
        }

        /// <summary>
        /// Check For Duplicate Address With DeliveryPoints
        /// </summary>
        /// <param name="objPostalAddress">objPostalAddress</param>
        /// <returns>bool</returns>
        public bool CheckForDuplicateAddressWithDeliveryPoints(PostalAddressDTO objPostalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.CheckForDuplicateAddressWithDeliveryPoints"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                bool isDuplicate = false;

                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Include(m => m.DeliveryPoints)
                    .Where(n => n.Postcode == objPostalAddress.Postcode);

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
                    postalAddress = postalAddress.Where(n => n.Thoroughfare.Equals(objPostalAddress.Thoroughfare,
                        StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(objPostalAddress.DependentThoroughfare))
                {
                    postalAddress =
                        postalAddress.Where(n => n.DependentThoroughfare.Equals(objPostalAddress.DependentThoroughfare,
                            StringComparison.OrdinalIgnoreCase));
                }

                var addressDetails = postalAddress.ToList();
                if (addressDetails != null && addressDetails.Count > 0)
                {
                    foreach (var address in addressDetails)
                    {
                        if (address != null && address.DeliveryPoints != null && address.DeliveryPoints.Count > 0)
                        {
                            isDuplicate = true;
                            break;
                        }
                    }
                }

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return isDuplicate;
            }
        }

        /// <summary>
        /// Update PAF details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">PAF details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        public async Task<bool> UpdateAddress(PostalAddressDTO objPostalAddress, string strFileName, Guid deliveryPointUseIndicatorPAF)
        {
            bool isPostalAddressUpdated = false;
            PostalAddress objAddress = new PostalAddress();
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.UpdateAddress"))
                {
                    string methodName = MethodHelper.GetActualAsyncMethodName();
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    if (objPostalAddress != null)
                    {
                        objAddress = DataContext.PostalAddresses.Include(m => m.DeliveryPoints).Where(n => n.ID == objPostalAddress.ID).SingleOrDefault();

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
                                        objDelPoint.DeliveryPointUseIndicator_GUID = deliveryPointUseIndicatorPAF;
                                    }

                                    objDelPoint.UDPRN = objPostalAddress.UDPRN;
                                }
                            }
                        }

                        await DataContext.SaveChangesAsync();
                        isPostalAddressUpdated = true;
                        loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                    }
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (objAddress != null)
                {
                    DataContext.Entry(objAddress).State = EntityState.Unchanged;
                }

                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), dbUpdateException.ToString());
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("PostalAddress PAF for UDPRN:", objAddress.UDPRN)));
            }
            catch (NotSupportedException notSupportedException)
            {
                if (objAddress != null)
                {
                    DataContext.Entry(objAddress).State = EntityState.Unchanged;
                }

                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), notSupportedException.ToString());
                notSupportedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                throw new InfrastructureException(notSupportedException, ErrorConstants.Err_NotSupportedException);
            }
            catch (ObjectDisposedException disposedException)
            {
                if (objAddress != null)
                {
                    DataContext.Entry(objAddress).State = EntityState.Unchanged;
                }

                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), disposedException.ToString());
                disposedException.Data.Add(ErrorConstants.UserFriendlyErrorMessage, ErrorConstants.Err_Default);
                throw new ServiceException(disposedException, ErrorConstants.Err_ObjectDisposedException);
            }

            return isPostalAddressUpdated;
        }

        /// <summary>
        /// Filter PostalAddress based on the search text
        /// </summary>
        /// <param name="searchText">searchText</param>
        /// <param name="unitGuid">unitGuid</param>
        /// <returns>List of Postcodes</returns>
        public async Task<List<string>> GetPostalAddressSearchDetails(string searchText, Guid unitGuid, List<Guid> addresstypeIDs)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressSearchDetails"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                List<string> searchdetails = new List<string>();
                var searchresults = await (from pa in DataContext.PostalAddresses.AsNoTracking()
                                           join pc in DataContext.Postcodes.AsNoTracking() on pa.PostCodeGUID equals pc.ID
                                           join ul in DataContext.UnitLocationPostcodes.AsNoTracking() on pc.ID equals ul.PoscodeUnit_GUID
                                           where addresstypeIDs.Contains(pa.AddressType_GUID) &&
                                           (pa.Thoroughfare.Contains(searchText) || pa.Postcode.Contains(searchText)) &&
                                           ul.Unit_GUID == unitGuid
                                           select new { SearchResult = string.IsNullOrEmpty(pa.Thoroughfare) ? pa.Postcode : pa.Thoroughfare + "," + pa.Postcode }).Distinct().OrderBy(x => x.SearchResult).ToListAsync();

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                return searchresults.Select(n => n.SearchResult).ToList();
            }
        }

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

        /// <summary>
        /// Filter PostalAddress based on postal address id.
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Postal Address DTO</returns>
        public PostalAddressDTO GetPostalAddressDetails(Guid id)
        {
            using (loggingHelper.RMTraceManager.StartTrace("DataService.GetPostalAddressDetail(Guid)"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                var postalAddress = DataContext.PostalAddresses.AsNoTracking().Where(n => n.ID == id).FirstOrDefault();
                PostalAddressDTO postalAddressDto = GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);

                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);
                return postalAddressDto;
            }
        }

        /// <summary>
        /// Create delivery point for PAF and NYB details
        /// </summary>
        /// <param name="addDeliveryPointDTO">addDeliveryPointDTO</param>
        /// <returns>bool</returns>
        public CreateDeliveryPointModelDTO CreateAddressAndDeliveryPoint(AddDeliveryPointDTO addDeliveryPointDTO)
        {
            try
            {
                using (loggingHelper.RMTraceManager.StartTrace("DataService.CreateAddressAndDeliveryPoint"))
                {
                    string methodName = MethodBase.GetCurrentMethod().Name;
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodEntryEventId, LoggerTraceConstants.Title);

                    bool isAddressLocationAvailable = false;
                    Guid returnGuid = new Guid(DEFAULTGUID);
                    double? addLocationXCoOrdinate = 0;
                    double? addLocationYCoOrdinate = 0;
                    if (addDeliveryPointDTO.PostalAddressDTO != null && addDeliveryPointDTO.DeliveryPointDTO != null)
                    {
                        var objPostalAddress = DataContext.PostalAddresses.SingleOrDefault(n => n.ID == addDeliveryPointDTO.PostalAddressDTO.ID);
                        var objAddressLocation = DataContext.AddressLocations.SingleOrDefault(n => n.UDPRN == addDeliveryPointDTO.PostalAddressDTO.UDPRN);

                        DeliveryPoint objDeliveryPoint = new DeliveryPoint()
                        {
                            ID = Guid.NewGuid(),
                            UDPRN = addDeliveryPointDTO.PostalAddressDTO.UDPRN,
                            DeliveryPointUseIndicator_GUID = addDeliveryPointDTO.DeliveryPointDTO.DeliveryPointUseIndicator_GUID,
                            MultipleOccupancyCount = addDeliveryPointDTO.DeliveryPointDTO.MultipleOccupancyCount,
                            MailVolume = addDeliveryPointDTO.DeliveryPointDTO.MailVolume
                        };

                        if (objAddressLocation != null)
                        {
                            SqlGeometry deliveryPointSqlGeometry = SqlGeometry.STGeomFromWKB(new SqlBytes(objAddressLocation.LocationXY.AsBinary()), BNGCOORDINATESYSTEM);
                            objDeliveryPoint.LocationXY = objAddressLocation.LocationXY;
                            objDeliveryPoint.Latitude = objAddressLocation.Lattitude;
                            objDeliveryPoint.Longitude = objAddressLocation.Longitude;
                            objDeliveryPoint.Positioned = true;
                            isAddressLocationAvailable = true;
                            addLocationXCoOrdinate = deliveryPointSqlGeometry.STX.Value;
                            addLocationYCoOrdinate = deliveryPointSqlGeometry.STY.Value;
                        }

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
                            objPostalAddress.PostCodeGUID = addDeliveryPointDTO.PostalAddressDTO.PostCodeGUID;
                            objPostalAddress.AddressType_GUID = addDeliveryPointDTO.PostalAddressDTO.AddressType_GUID;
                            objPostalAddress.AddressStatus_GUID = addDeliveryPointDTO.PostalAddressDTO.AddressStatus_GUID;
                            objPostalAddress.DeliveryPoints.Add(objDeliveryPoint);
                        }
                        else
                        {
                            addDeliveryPointDTO.PostalAddressDTO.ID = Guid.NewGuid();
                            var entity = GenericMapper.Map<PostalAddressDTO, PostalAddress>(addDeliveryPointDTO.PostalAddressDTO);
                            entity.DeliveryPoints.Add(objDeliveryPoint);
                            DataContext.PostalAddresses.Add(entity);
                        }

                        DataContext.SaveChanges();
                        returnGuid = objDeliveryPoint.ID;
                    }
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PostalAddressAPIPriority, LoggerTraceConstants.PostalAddressDataServiceMethodExitEventId, LoggerTraceConstants.Title);

                    return new CreateDeliveryPointModelDTO { ID = returnGuid, IsAddressLocationAvailable = isAddressLocationAvailable, XCoordinate = addLocationXCoOrdinate, YCoordinate = addLocationYCoOrdinate };
                }
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new DataAccessException(dbUpdateException, string.Format(ErrorConstants.Err_SqlAddException, string.Concat("Delivery Point for UDPRN:", addDeliveryPointDTO.PostalAddressDTO.UDPRN)));
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
                AmendmentType = INSERT,
                FileName = strFileName,
                FileProcessing_TimeStamp = DateTime.UtcNow,
                FileType = fileType,
                ErrorMessage = strException,
                SuccessFlag = false
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