namespace RM.DataManagement.DeliveryPoint.WebAPI.Utils
{
    public static class DeliveryPointConstants
    {
        internal const string LayerType = "type";
        internal const string Polygon = "POLYGON(({0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}))";
        internal const string DEFAULTGUID = "00000000-0000-0000-0000-000000000000";
        internal const string NETWORKLINKDATAPROVIDER = "Data Provider";
        internal const int BNGCOORDINATESYSTEM = 27700;
        internal const string Comma = ", ";
        internal const string BuildingName = "name";
        internal const string BuildingNumber = "number";
        internal const string Postcode = "postcode";
        internal const string StreetName = "street_name";
        internal const string SubBuildingName = "subBuildingName";
        internal const string OrganisationName = "organisationName";
        internal const string DepartmentName = "departmentName";
        internal const string MailVolume = "mailVolume";
        internal const string MultipleOccupancyCount = "multipleOccupancyCount";
        internal const string Locality = "locality";
        internal const string DeliveryPointId = "deliveryPointId";
        internal const string Street = "Street";
        internal const string Space = " ";
        internal const string RouteName = "ROUTENAME";
        internal const string DpUse = "DPUSE";

        internal const string USRGEOMETRYPOINT = "POINT({0} {1})";
        internal const string DUPLICATEDELIVERYPOINT = "There is a duplicate of this Delivery Point in the system";
        internal const string DUPLICATENYBRECORDS = "This address is in the NYB file under the postcode ";
        internal const string DELIVERYPOINTCREATED = "Delivery Point created successfully";
        internal const string DELIVERYPOINTCREATEDWITHOUTACCESSLINK = "Delivery Point created successfully without access link";
        internal const string DELIVERYPOINTCREATEDWITHOUTLOCATION = "Delivery Point created successfully without location";
        internal const string INTERNAL = "Internal";

        internal const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        internal const string AccessLinkWebAPIName = "AccessLinkWebAPIName";
        internal const string DeliveryRouteManagerWebAPIName = "DeliveryRouteManagerWebAPIName";
        internal const string PostalAddressManagerWebAPIName = "PostalAddressManagerWebAPIName";
        internal const string UnitManagerDataWebAPIName = "UnitManagerDataWebAPIName";

        internal const string DeliveryPointUseIndicator = "DeliveryPoint Use Indicator";
        internal const string OperationalStatusGUIDLive = "Live";
        internal const string OperationalStatusGUIDLivePendingLocation = "Live pending location";
        internal const string NetworkNodeTypeRMGServiceNode = "RMG Service Node";
        internal const string TASKNOTIFICATION = "Notification Type";
        internal const string TASKACTION = "Action required";
        internal const string TASKSOURCE = "SYSTEM";
        internal const string DeliveryPointUseIndicatorPAF = "Organisation";
        internal const string EXTERNAL = "External";
        internal const string ROWVERSION = "RowVersion";
        internal const string UnSequenced = "U";
        internal const string PRIMARYROUTE = "Primary - ";
        internal const string SECONDARYROUTE = "Secondary - ";
        internal const string SearchResultCount = "SearchResultCount";
    }
}