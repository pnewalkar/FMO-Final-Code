namespace RM.CommonLibrary.HelperMiddleware
{
    public static class LoggerTraceConstants
    {
        public const string Category = "General";
        public const string BusinessLayer = "Business.";
        public const string DataLayer = "Data.";

        #region Priority

        public const int DeliveryPointAPIPriority = 8;
        public const int AccessLinkAPIPriority = 8;
        public const int SavePostalAddressPriority = 8;
        public const int SavePAFDetailsPriority = 8;
        public const int SaveDeliveryPointProcessPriority = 8;
        public const int GetPostalAddressSearchDetailsPriority = 8;
        public const int GetPostalAddressDetailsPriority = 8;
        public const int GetPostalAddressDetailsByIdPriority = 8;
        public const int GetRoadRoutesPriority = 8;
        public const int FetchBasicSearchDetailsPriority = 8;
        public const int FetchAdvanceSearchDetailsPriority = 8;
        public const int CreateAutomaticAccessLinkPriority = 8;
        public const int CreateManualAccessLinkPriority = 8;
        public const int GetRouteForDeliveryPointPriority = 8;
        public const int GetDPUsePriority = 8;

        #endregion Priority

        #region EventId

        public const int AccessLinkControllerMethodEntryEventId = 8003;
        public const int AccessLinkControllerMethodExitEventId = 8004;
        public const int CreateManualAccessLinkAPIMethodEntryEventId = 8005;
        public const int CreateManualAccessLinkAPIMethodExitEventId = 8006;

        public const int DeliveryPointControllerMethodEntryEventId = 9003;
        public const int DeliveryPointControllerMethodExitEventId = 9004;
        public const int DeliveryPointBusinessServiceMethodEntryEventId = 9013;
        public const int DeliveryPointBusinessServiceMethodExitEventId = 9014;

        public const int UpdateDeliveryPointAPIMethodEntryEventId = 9103;
        public const int UpdateDeliveryPointAPIMethodExitEventId = 9104;
        public const int UpdateDeliveryPointLocationOnUDPRNAPIMethodEntryEventId = 9105;
        public const int UpdateDeliveryPointLocationOnUDPRNAPIMethodExitEventId = 9106;
        public const int UpdateDeliveryPointLocationOnIDAPIMethodEntryEventId = 9107;
        public const int UpdateDeliveryPointLocationOnIDAPIMethodExitEventId = 9108;
        public const int InsertDeliveryPointAPIMethodEntryEventId = 9109;
        public const int InsertDeliveryPointAPIMethodExitEventId = 9110;
        public const int UpdateDeliveryPointAccessLinkCreationStatusAPIMethodEntryEventId = 9111;
        public const int UpdateDeliveryPointAccessLinkCreationStatusAPIMethodExitEventId = 9112;
        public const int UpdateDeliveryPoinBusinessMethodEntryEventId = 9113;
        public const int UpdateDeliveryPoinBusinessMethodExitEventId = 9114;

        public const int SavePostalAddressBusinessMethodEntryEventId = 9117;
        public const int SavePostalAddressBusinessMethodExitEventId = 9118;
        public const int SavePAFDetailsBusinessMethodEntryEventId = 9121;
        public const int SavePAFDetailsBusinessMethodExitEventId = 9122;
        public const int SaveDeliveryPointProcessBusinessMethodEntryEventId = 9125;
        public const int SaveDeliveryPointProcessBusinessMethodExitEventId = 9126;
        public const int GetPostalAddressSearchDetailsBusinessMethodEntryEventId = 9129;
        public const int GetPostalAddressSearchDetailsBusinessMethodExitEventId = 9130;
        public const int GetPostalAddressDetailsBusinessMethodEntryEventId = 9131;
        public const int GetPostalAddressDetailsBusinessMethodExitEventId = 9132;
        public const int GetPostalAddressDetailsByIdBusinessMethodEntryEventId = 9133;
        public const int GetPostalAddressDetailsByIdBusinessMethodExitEventId = 9134;

        public const int GetRoadRoutesBusinessMethodEntryEventId = 9135;
        public const int GetRoadRoutesBusinessMethodExitEventId = 9136;

        public const int FetchBasicSearchDetailsBusinessMethodEntryEventId = 9137;
        public const int FetchBasicSearchDetailsBusinessMethodExitEventId = 9138;

        public const int FetchAdvanceSearchDetailsBusinessMethodEntryEventId = 9137;
        public const int FetchAdvanceSearchDetailsBusinessMethodExitEventId = 9138;

        public const int CreateAutomaticAccessLinkBusinessMethodEntryEventId = 9313;
        public const int CreateAutomaticAccessLinkBusinessMethodExitEventId = 9314;
        public const int CreateManualAccessLinkBusinessMethodEntryEventId = 9315;
        public const int CreateManualAccessLinkBusinessMethodExitEventId = 9316;

        public const int GetRouteForDeliveryPointBusinessMethodEntryEventId = 9317;
        public const int GetRouteForDeliveryPointBusinessMethodExitEventId = 9318;
        public const int GetRouteForDeliveryPointDataMethodEntryEventId = 9319;
        public const int GetRouteForDeliveryPointDataMethodExitEventId = 9320;

        public const int GetDPUseBusinessMethodEntryEventId = 9321;
        public const int GetDPUseBusinessMethodExitEventId = 9322;
        public const int GetDPUseDataMethodEntryEventId = 9323;
        public const int GetDPUseDataMethodExitEventId = 9324;

        #endregion EventId

        #region Title

        public const string Title = "Trace Log";

        #endregion Title
    }
}