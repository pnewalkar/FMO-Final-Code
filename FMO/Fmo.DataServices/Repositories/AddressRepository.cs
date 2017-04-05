namespace Fmo.DataServices.Repositories
{
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
        public AddressRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        public bool DeleteNYBPostalAddress(List<int> lstUDPRN, int addressType)
        {
            bool deleteFlag = false;
            try
            {
                if (lstUDPRN != null && lstUDPRN.Count() > 0)
                {
                    var lstAddress = DataContext.PostalAddresses.Include("DeliveryPoints").Where(n => !lstUDPRN.Contains(n.UDPRN.Value) && n.AddressType_Id == addressType).ToList();
                    if (lstAddress != null && lstUDPRN.Count > 0)
                    {
                        lstAddress.ForEach(postalAddressEntity =>
                        {
                            if (postalAddressEntity.DeliveryPoints != null && postalAddressEntity.DeliveryPoints.Count > 0)
                            {
                                // TO DO log error
                            }
                            else
                            {
                                DataContext.PostalAddresses.Remove(postalAddressEntity);
                            }
                        });
                        DataContext.SaveChanges();
                    }

                    deleteFlag = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return deleteFlag;
        }

        public bool SaveAddress(PostalAddressDTO objPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                if (objPostalAddress != null)
                {
                    var objAddress = DataContext.PostalAddresses.Where(n => n.UDPRN == objPostalAddress.UDPRN).SingleOrDefault();
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
                    }
                    else
                    {
                        var entity = GenericMapper.Map<PostalAddressDTO, PostalAddress>(objPostalAddress);
                        DataContext.PostalAddresses.Add(entity);
                    }

                    DataContext.SaveChanges();
                    saveFlag = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return saveFlag;
        }

        public bool InsertAddress(PostalAddressDTO objPostalAddress)
        {
            bool saveFlag = false;
            try
            {
                if (objPostalAddress != null)
                {
                    var objAddress = GenericMapper.Map<PostalAddressDTO, PostalAddress>(objPostalAddress);
                    DataContext.PostalAddresses.Add(objAddress);

                    DataContext.SaveChanges();
                    saveFlag = true;
                }
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                // TO DO implement logging
                throw;
            }
        }

        public PostalAddressDTO GetPostalAddress(DTO.PostalAddressDTO objPostalAddress)
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
                                      n.DependentThoroughfare == objPostalAddress.DependentThoroughfare).SingleOrDefault();


                return GenericMapper.Map<PostalAddress, PostalAddressDTO>(postalAddress);
            }
            catch (Exception)
            {
                // TO DO implement logging
                throw;
            }
        }

        public bool UpdateAddress(PostalAddressDTO objPostalAddress)// , int addressType)&& n.AddressType_Id == addressType
        {
            bool saveFlag = false;
            try
            {
                if (objPostalAddress != null)
                {
                    // .Include("DeliveryPoints")
                    var objAddress = DataContext.PostalAddresses.Where(n => n.Address_Id == objPostalAddress.Address_Id).SingleOrDefault();
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
                        DataContext.Entry(objAddress).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        // Error Log entry
                    }

                    DataContext.SaveChanges();
                    saveFlag = true;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return saveFlag;
        }
    }
}