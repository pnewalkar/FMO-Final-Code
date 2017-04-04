﻿using System;
using System.Collections.Generic;
using System.Linq;
using Fmo.DataServices.DBContext;
using Fmo.DataServices.Infrastructure;
using Fmo.DataServices.Repositories.Interfaces;
using Fmo.DTO;
using Fmo.Entities;

using Fmo.MappingConfiguration;

namespace Fmo.DataServices.Repositories
{
    public class SearchRepository : ISearchRepository
    {
        IDeliveryPointsRepository deliveryPointsRepository = default(IDeliveryPointsRepository);
        IDeliveryRouteRepository deliveryRouteRepository = default(IDeliveryRouteRepository);
        IPostCodeRepository postCodeRepository = default(IPostCodeRepository);
        IStreetNetworkRepository streetNetworkRepository = default(IStreetNetworkRepository);

        public SearchRepository(IDeliveryPointsRepository deliveryPointsRepository, IDeliveryRouteRepository deliveryRouteRepository, IPostCodeRepository postCodeRepository, IStreetNetworkRepository streetNetworkRepository)
        {
        }

        public AdvanceSearchDTO FetchAdvanceSearchDetails(string searchtext)
        {
            try
            {
                // List<Entities.Po> deliveryPoint = new List<Entities.DeliveryPoint>();
                AdvanceSearchDTO objAdvDTO = new AdvanceSearchDTO();

                // var addrDetails = DataContext.PostalAddresses.Where(l=>l.OrganisationName == Searchtext || l.BuildingName == Searchtext || l.SubBuildingName== Searchtext|| l.BuildingNumber == Convert.ToInt16(Searchtext)|| l.Thoroughfare == Searchtext || l.DependentLocality == Searchtext).ToList();
                // var result = GenericMapper.MapList<PostalAddress, PostalAddressDTO>(addrDetails);
                // objAdvDTO.PostalAddress = result;

                // var postCodeDetails = DataContext.Postcodes.Where(l => l.PostcodeUnit == Searchtext).ToList();
                // var postCodeResult = GenericMapper.MapList<Postcode, PostCodeDTO>(postCodeDetails);
                // objAdvDTO.PostCode = postCodeResult;
                // var deliveryRouteDetails = DataContext.DeliveryRoutes.Where(l => l.RouteName == Searchtext || l.RouteNumber == Searchtext).ToList();
                // var deliveryRouteResult = GenericMapper.MapList<DeliveryRoute, DeliveryRouteDTO>(deliveryRouteDetails);
                // objAdvDTO.DeliveryRoute = deliveryRouteResult;

                // var streetNameDetails = DataContext.StreetNames.Where(l => l.NationalRoadCode == Searchtext || l.DesignatedName == Searchtext || l.Descriptor == Searchtext).ToList();
                // var streetNameResult = GenericMapper.MapList<StreetName, StreetNameDTO>(streetNameDetails);
                // objAdvDTO.StreetName = streetNameResult;
                return objAdvDTO;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}