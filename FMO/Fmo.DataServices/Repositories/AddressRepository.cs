namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.Entities;

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
                        lstAddress.ForEach(x =>
                        {
                            if (x.DeliveryPoints != null && x.DeliveryPoints.Count > 0)
                            {
                                // TO DO log error
                            }
                            else
                            {
                                DataContext.Entry(lstAddress).State = System.Data.Entity.EntityState.Deleted;
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

        public bool SaveAddress(PostalAddress objPostalAddress)
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
                        DataContext.Entry(objAddress).State = System.Data.Entity.EntityState.Modified;
                    }
                    else
                    {
                        DataContext.PostalAddresses.Add(objPostalAddress);
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

        public int GetPostalAddress(int? uDPRN)
        {
            int statusId = 0;
            try
            {
                statusId = DataContext.PostalAddresses.Where(n => n.UDPRN == uDPRN).SingleOrDefault().UDPRN ?? 0;
            }
            catch (Exception)
            {
                // TO DO implement logging
                throw;
            }

            return statusId;
        }

        public int GetPostalAddress(PostalAddress objPostalAddress)
        {
            int addressId = 0;
            try
            {
                addressId = DataContext.PostalAddresses
                               .Where
                                (n => n.Postcode == objPostalAddress.Postcode &&
                                      n.BuildingName == objPostalAddress.BuildingName &&
                                      n.BuildingNumber == objPostalAddress.BuildingNumber &&
                                      n.SubBuildingName == objPostalAddress.SubBuildingName &&
                                      n.OrganisationName == objPostalAddress.OrganisationName &&
                                      n.DepartmentName == objPostalAddress.DepartmentName &&
                                      n.Thoroughfare == objPostalAddress.Thoroughfare &&
                                      n.DependentThoroughfare == objPostalAddress.DependentThoroughfare
                                      ).SingleOrDefault().Address_Id;

            }
            catch (Exception)
            {
                // TO DO implement logging
                throw;
            }

            return addressId;
        }

        public bool UpdateAddress(PostalAddress objPostalAddress, int addressType)
        {
            bool saveFlag = false;
            try
            {
                if (objPostalAddress != null)
                {
                    var objAddress = DataContext.PostalAddresses.Where(n => n.Address_Id == objPostalAddress.Address_Id && n.AddressType_Id == addressType).SingleOrDefault();
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