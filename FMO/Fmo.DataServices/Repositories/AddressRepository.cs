namespace Fmo.DataServices.Repositories
{
    using Common.Enums;
    using Common.Interface;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using MappingConfiguration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AddressRepository : RepositoryBase<PostalAddress, FMODBContext>, IAddressRepository
    {
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private IFileProcessingLogRepository fileProcessingLog = default(IFileProcessingLogRepository);
        private IPostCodeRepository postCodeRepository = default(IPostCodeRepository);

        public AddressRepository(IDatabaseFactory<FMODBContext> databaseFactory, ILoggingHelper loggingHelper, IFileProcessingLogRepository fileProcessingLog, IPostCodeRepository postCodeRepository)
            : base(databaseFactory)
        {
            this.loggingHelper = loggingHelper;
            this.fileProcessingLog = fileProcessingLog;
            this.postCodeRepository = postCodeRepository;
        }

        /// <summary>
        /// Delete postal Address records do not have an associated Delivery Point
        /// </summary>
        /// <param name="lstUDPRN">List of UDPRN</param>
        /// <param name="addressType">NYB</param>
        /// <returns>true or false</returns>
        public bool DeleteNYBPostalAddress(List<int> lstUDPRN, Guid addressType)
        {
            bool deleteFlag = false;
            try
            {
                if (lstUDPRN != null && lstUDPRN.Count() > 0)
                {
                    var lstAddress = DataContext.PostalAddresses.Include("DeliveryPoints").Where(n => !lstUDPRN.Contains(n.UDPRN.Value) && n.AddressType_GUID == addressType).ToList();
                    if (lstAddress != null && lstAddress.Count > 0)
                    {
                        lstAddress.ForEach(postalAddressEntity =>
                        {
                            if (postalAddressEntity.DeliveryPoints != null && postalAddressEntity.DeliveryPoints.Count > 0)
                            {
                                deleteFlag = false;
                                this.loggingHelper.LogInfo("Load NYB Error Message : AddressType is NYB and have an associated Delivery Point for UDPRN: " + string.Join(",", lstUDPRN));
                            }
                            else
                            {
                                DataContext.PostalAddresses.Remove(postalAddressEntity);
                                DataContext.SaveChanges();
                                deleteFlag = true;
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return deleteFlag;
        }

        /// <summary>
        /// Create or update NYB details depending on the UDPRN
        /// </summary>
        /// <param name="objPostalAddress">NYB details DTO</param>
        /// <param name="strFileName">CSV Filename</param>
        /// <returns>true or false</returns>
        public bool SaveAddress(PostalAddressDTO objPostalAddress, string strFileName)
        {
            bool saveFlag = false;
            try
            {
                if (objPostalAddress != null)
                {
                    var objAddress = DataContext.PostalAddresses.Where(n => n.UDPRN == objPostalAddress.UDPRN).SingleOrDefault();
                    objPostalAddress.PostCodeGUID = this.postCodeRepository.GetPostCodeID(objPostalAddress.Postcode);
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
                        var entity = GenericMapper.Map<PostalAddressDTO, PostalAddress>(objPostalAddress);
                        DataContext.PostalAddresses.Add(entity);
                    }

                    DataContext.SaveChanges();
                    saveFlag = true;
                }
            }
            catch (Exception ex)
            {
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Nyb.ToString(), ex.ToString());
            }

            return saveFlag;
        }

        public bool InsertAddress(PostalAddressDTO objPostalAddress, string strFileName)
        {
            bool saveFlag = false;
            try
            {
                if (objPostalAddress != null)
                {
                    objPostalAddress.PostCodeGUID = this.postCodeRepository.GetPostCodeID(objPostalAddress.Postcode);
                    var objAddress = GenericMapper.Map<PostalAddressDTO, PostalAddress>(objPostalAddress);
                    DataContext.PostalAddresses.Add(objAddress);

                    DataContext.SaveChanges();
                    saveFlag = true;
                }
            }
            catch (Exception ex)
            {
                LogFileException(objPostalAddress.UDPRN.Value, strFileName, FileType.Paf.ToString(), ex.ToString());
                throw ex;
            }

            return saveFlag;
        }

        public PostalAddressDTO GetPostalAddress(int? uDPRN)
        {
            try
            {
                var postalAddress = DataContext.PostalAddresses.Where(n => n.UDPRN == uDPRN).SingleOrDefault();

                return GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PostalAddressDTO GetPostalAddress(PostalAddressDTO objPostalAddress)
        {
            try
            {
                var postalAddress = DataContext.PostalAddresses
                               .Where(
                                n => n.Postcode == objPostalAddress.Postcode &&
                                      n.BuildingName == objPostalAddress.BuildingName &&
                                      n.BuildingNumber == objPostalAddress.BuildingNumber &&
                                      n.SubBuildingName == objPostalAddress.SubBuildingName &&
                                      n.OrganisationName == objPostalAddress.OrganisationName &&
                                      n.DepartmentName == objPostalAddress.DepartmentName &&
                                      n.Thoroughfare == objPostalAddress.Thoroughfare &&
                                      n.DependentThoroughfare == objPostalAddress.DependentThoroughfare).FirstOrDefault();


                return GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /*public bool UpdateAddress(PostalAddressDTO objPostalAddress, string strFileName)
        {
            bool saveFlag = false;
            try
            {
                if (objPostalAddress != null)
                {
                    // .Include("DeliveryPoints")
                    var objAddress = DataContext.PostalAddresses.Where(n => n.ID == objPostalAddress.ID).SingleOrDefault();
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
                        objAddress.PostcodeType = objPostalAddress.PostcodeType;
                        objAddress.SmallUserOrganisationIndicator = objPostalAddress.SmallUserOrganisationIndicator;
                        objAddress.DeliveryPointSuffix = objPostalAddress.DeliveryPointSuffix;
                        objAddress.UDPRN = objPostalAddress.UDPRN;

                        // if (objAddress.DeliveryPoints != null && objAddress.DeliveryPoints.Count > 0)
                        // {
                        //    foreach (var objDelPoint in objAddress.DeliveryPoints)
                        //    {
                        //        objDelPoint.UDPRN = objPostalAddress.UDPRN;
                        //    }
                        // }
                        // else
                        // {
                        //    //To DO log error
                        // }
                    }
                    else
                    {
                        // Error Log entry
                    }

                    DataContext.SaveChanges();
                    saveFlag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return saveFlag;
        }*/

        /// <summary>
        /// Log exception into DB if error occurs while inserting NYB,PAF,USR records in DB
        /// </summary>
        /// <param name="uDPRN"></param>
        /// <param name="strFileName"></param>
        /// <param name="fileType"></param>
        /// <param name="strException"></param>
        /// <returns></returns>
        private bool LogFileException(int uDPRN, string strFileName, string fileType, string strException)
        {
            try
            {
                FileProcessingLogDTO objFileProcessingLog = new FileProcessingLogDTO()
                {
                    FileID = Guid.NewGuid(),
                    UDPRN = uDPRN,
                    AmendmentType = "I",
                    FileName = strFileName,
                    FileProcessing_TimeStamp = DateTime.Now,
                    FileType = fileType,
                    NatureOfError = strException
                };

                return fileProcessingLog.LogFileException(objFileProcessingLog);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}