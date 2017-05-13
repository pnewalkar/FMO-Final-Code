namespace Fmo.DataServices.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using Fmo.Common.Constants;
    using Fmo.DataServices.DBContext;
    using Fmo.DataServices.Infrastructure;
    using Fmo.DataServices.Repositories.Interfaces;
    using Fmo.DTO;
    using Fmo.Entities;
    using Fmo.MappingConfiguration;

    /// <summary>
    /// This class contains methods of Access Link Repository for fetching Access Link data.
    /// </summary>
    public class AccessLinkRepository : RepositoryBase<AccessLink, FMODBContext>, IAccessLinkRepository
    {
        public AccessLinkRepository(IDatabaseFactory<FMODBContext> databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>List of Access Link Dto</returns>
        public List<AccessLinkDTO> GetAccessLinks(string boundingBoxCoordinates, Guid unitGuid)
        {
            List<AccessLink> result = GetAccessLinkCoordinatesDataByBoundingBox(boundingBoxCoordinates, unitGuid).ToList();
            var accessLink = GenericMapper.MapList<AccessLink, AccessLinkDTO>(result);
            return accessLink;
        }

        /// <summary>
        /// Creates access link.
        /// </summary>
        /// <param name="accessLinkDto">Access link data object.</param>
        /// <returns>Success.</returns>
        public bool CreateAccessLink(AccessLinkDTO accessLinkDto)
        {
            bool accessLinkCreationSuccess = false;

            AccessLink accessLink = new AccessLink
            {
                ID = Guid.NewGuid(),
                OperationalObjectPoint = accessLinkDto.OperationalObjectPoint,
                NetworkIntersectionPoint = accessLinkDto.NetworkIntersectionPoint,
                AccessLinkLine = accessLinkDto.AccessLinkLine,
                ActualLengthMeter = accessLinkDto.ActualLengthMeter,
                WorkloadLengthMeter = accessLinkDto.WorkloadLengthMeter,
                Approved = accessLinkDto.Approved,
                OperationalObject_GUID = accessLinkDto.OperationalObject_GUID,
                NetworkLink_GUID = accessLinkDto.NetworkLink_GUID
            };

            DataContext.AccessLinks.Add(accessLink);

            accessLinkCreationSuccess = DataContext.SaveChanges() > 0;

            return accessLinkCreationSuccess;
        }

        /// <summary>
        /// This Method is used to Access Link data for defined coordinates.
        /// </summary>
        /// <param name="boundingBoxCoordinates">BoundingBox Coordinates</param>
        /// <param name="unitGuid">unit unique identifier.</param>
        /// <returns>Link of Access Link Entity</returns>
        private IEnumerable<AccessLink> GetAccessLinkCoordinatesDataByBoundingBox(string boundingBoxCoordinates, Guid unitGuid)
        {
            if (!string.IsNullOrEmpty(boundingBoxCoordinates))
            {
                DbGeometry polygon = DataContext.UnitLocations.AsNoTracking().Where(x => x.ID == unitGuid).Select(x => x.UnitBoundryPolygon).SingleOrDefault();

                DbGeometry extent = System.Data.Entity.Spatial.DbGeometry.FromText(boundingBoxCoordinates.ToString(), Constants.BNGCOORDINATESYSTEM);
                return DataContext.AccessLinks.AsNoTracking().Where(x => x.AccessLinkLine.Intersects(extent) && x.AccessLinkLine.Intersects(polygon)).ToList();
            }
            else
            {
                return null;
            }
        }

        /*

        /// <summary>
        /// Create auto access link
        /// </summary>
        /// <param name="operationalObjectId">operational point unique identifier.</param>
        /// <returns>bool</returns>
        public bool CreateAutoAccessLink(Guid operationalObjectId)
        {
            bool isAccessLinkCreated = false;
            if (operationalObjectId != Guid.Empty)
            {
                using (var transaction = DataContext.Database.BeginTransaction())
                {
                    try
                    {
                        // get corresponding dp and postal address data
                        var deliveryPoint = DataContext.DeliveryPoints
                            .Include(l => l.PostalAddress)
                            .Where(x => x.Positioned == true && x.AccessLinkPresent == false && x.ID == operationalObjectId)
                            .SingleOrDefault();

                        // connect with nearest point on road of related street (rule 1)
                        if (deliveryPoint.LocationXY != null)
                        {
                            var operationalObjectPoint = deliveryPoint.LocationXY;
                            var thoroughfare = deliveryPoint.PostalAddress.Thoroughfare;

                            var pathIntersectionCount = 0;
                            var roadIntersectionCount = 0;
                            SqlGeometry accessLinkLine = null;
                            double? actualLengthMeter = null;
                            Guid networkLinkGuid = default(Guid);

                            var streetName = DataContext.StreetNames
                                .Where(m => (m.Descriptor == thoroughfare
                                            || m.DesignatedName == thoroughfare
                                            || m.LocalName == thoroughfare)
                                            && (operationalObjectPoint.Distance(m.Geometry) <= 40))
                                .OrderBy(n => operationalObjectPoint.Distance(n.Geometry))
                                .FirstOrDefault();

                            if (streetName != null)
                            {
                                NetworkLink networkLink = DataContext.NetworkLinks
                                .Where(m => m.StreetName_GUID == streetName.ID)
                                .OrderBy(n => n.LinkGeometry.Distance(operationalObjectPoint))
                                .FirstOrDefault();

                                networkLinkGuid = networkLink.Id;

                                accessLinkLine = operationalObjectPoint.ToSqlGeometry().ShortestLineTo(networkLink.LinkGeometry.ToSqlGeometry());
                                actualLengthMeter = networkLink.LinkGeometry.Distance(operationalObjectPoint);
                            }

                            Guid networkPathLinkType = default(Guid); // referenceDataCategoryRepository.GetReferenceDataId("Network Link Type", "Path Link");
                            Guid networkRoadLinkType = default(Guid); // referenceDataCategoryRepository.GetReferenceDataId("Network Link Type", "Road Link");

                            if (accessLinkLine != null)
                            {
                                roadIntersectionCount = DataContext.NetworkLinks
                                                .Where(m => m.LinkGeometry.Intersects(accessLinkLine.ToDbGeometry()) == true
                                                            && m.LinkGeometry.Distance(accessLinkLine.ToDbGeometry()) <= 40
                                                            && m.NetworkLinkType_GUID == networkRoadLinkType)
                                                .Count();
                                pathIntersectionCount = DataContext.NetworkLinks
                                                .Where(m => m.LinkGeometry.Intersects(accessLinkLine.ToDbGeometry()) == true
                                                            && m.LinkGeometry.Distance(accessLinkLine.ToDbGeometry()) <= 40
                                                            && m.NetworkLinkType_GUID == networkPathLinkType)
                                                .Count();
                            }

                            if (accessLinkLine == null || actualLengthMeter > 40 || pathIntersectionCount != 0 || roadIntersectionCount > 1)
                            {
                                var networkLinkRoad = DataContext.NetworkLinks
                                    .Where(m => m.StreetName_GUID == streetName.ID
                                                && m.NetworkLinkType_GUID == networkRoadLinkType
                                                && m.NetworkLinkType_GUID == networkPathLinkType
                                                && (operationalObjectPoint.Distance(m.LinkGeometry) <= 40))
                                    .OrderBy(n => n.LinkGeometry.Distance(operationalObjectPoint))
                                    .FirstOrDefault();

                                networkLinkGuid = networkLinkRoad.Id;

                                accessLinkLine = operationalObjectPoint.ToSqlGeometry().ShortestLineTo(networkLinkRoad.LinkGeometry.ToSqlGeometry());
                                actualLengthMeter = networkLinkRoad.LinkGeometry.Distance(operationalObjectPoint);
                            }

                            // Insert access link
                            if (accessLinkLine != null && actualLengthMeter <= 40 && actualLengthMeter > 0)
                            {
                                // substract 66 % of Pavement Width(2) and 50 % of House depth(5) from actual length
                                var workloadLengthMeter = actualLengthMeter - 7;
                                var networkIntersectionPoint = accessLinkLine.STEndPoint();

                                AccessLink accessLink = new AccessLink
                                {
                                    ID = Guid.NewGuid(),
                                    OperationalObjectPoint = operationalObjectPoint,
                                    NetworkIntersectionPoint = networkIntersectionPoint.ToDbGeometry(),
                                    AccessLinkLine = accessLinkLine.ToDbGeometry(),
                                    ActualLengthMeter = (decimal)actualLengthMeter,
                                    WorkloadLengthMeter = (decimal)workloadLengthMeter,
                                    Approved = true,
                                    OperationalObject_GUID = operationalObjectId,
                                    NetworkLink_GUID = networkLinkGuid
                                };

                                deliveryPoint.AccessLinkPresent = true;
                                DataContext.AccessLinks.Add(accessLink);

                                DataContext.SaveChanges();
                            }
                            else
                            {
                                string message = "Access link not created, no feature found";
                            }
                        }
                        else
                        {
                            string message = "Operational object details insufficient";
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                    }
                }
            }

            return isAccessLinkCreated;
        }
        */
    }
}