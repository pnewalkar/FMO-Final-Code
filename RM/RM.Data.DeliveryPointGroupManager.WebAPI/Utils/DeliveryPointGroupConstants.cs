namespace RM.DataManagement.DeliveryPointGroupManager.WebAPI.Utils
{
    public static class DeliveryPointGroupConstants
    {
        internal const string ReferenceDataWebAPIName = "ReferenceDataWebAPIName";
        internal const int BNGCOORDINATESYSTEM = 27700;
        internal const string FeatureType = "Feature";
        internal const string LayerType = "type";
        internal const string Polygon = "POLYGON(({0} {1}, {2} {3}, {4} {5}, {6} {7}, {8} {9}))";

        internal const string DeliveryPointGroupDataProviderGUID = "RMG Service Node";
        internal const string PolygonWellKnownText = "POLYGON(({0}))";
        internal const string OperationalStatusGUIDLive = "Live";
        internal const string Residential = "Residential";
        internal const string RelationshipTypeForCentroidToBoundary = "Is Supporting DP For Group Polygon";
        internal const string RelationshipTypeForCentroidToDeliveryPoint = "Is Delivery Point In Group";
    }
}