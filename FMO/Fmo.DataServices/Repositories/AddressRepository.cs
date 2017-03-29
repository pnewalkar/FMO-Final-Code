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

        public bool DeleteNYBPostalAddress(List<int> lstUDPRN)
        {
            bool deleteFlag = false;
            try
            {
                if (lstUDPRN != null && lstUDPRN.Count() > 0)
                {
                    var lstAddress = DataContext.PostalAddresses.Where(n => !lstUDPRN.Contains(n.UDPRN.Value)).ToList();
                    if (lstAddress != null && lstUDPRN.Count > 0)
                    {
                        foreach (var entity in lstAddress)
                        {
                            DataContext.Entry(lstAddress).State = System.Data.Entity.EntityState.Deleted;

                            DataContext.SaveChanges();
                        }
                    }

                    deleteFlag = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            catch (Exception ex)
            {
                throw ex;
            }

            return saveFlag;
        }
    }
}