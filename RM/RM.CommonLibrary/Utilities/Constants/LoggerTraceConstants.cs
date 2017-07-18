namespace RM.CommonLibrary.HelperMiddleware
{
    public static class LoggerTraceConstants
    {
        #region default

        internal const string DefaultLoggingCategory = "General";
        internal const int DefaultLoggingPriority = 0;
        internal const int DefaultLoggingEventId = 0;
        internal const string DefaultLoggingTitle = "";

        #endregion default

        public const string Category = "General";
        public const string BusinessLayer = "Business.";
        public const string DataLayer = "Data.";

        public const string MethodExecutionStarted = " Method execution started";
        public const string MethodExecutionCompleted = " Method execution completed";
        public const string COLON = " : ";

        #region Priority

        public const int DeliveryPointAPIPriority = 8;
        public const int AccessLinkAPIPriority = 8;
        public const int DeliveryRouteAPIPriority = 8;
        public const int PostalAddressAPIPriority = 8;
        public const int FileProcessingLogPriority = 8;
        public const int ThirdPartyAddressLocationAPIPriority = 8;
        public const int NotificationAPIPriority = 8;
        public const int SearchManagerAPIPriority = 8;
        public const int NYBPriority = 8;
        public const int PAFPriority = 8;
        public const int ThirdPartyPriority = 8;
        public const int ReferenceDataAPIPriority = 8;
        public const int RouteLogAPIPriority = 8;
        public const int PDFGeneratorAPIPriority = 8;
        public const int MapManagerAPIPriority = 8;
        public const int UnitManagerAPIPriority = 8;
        public const int NetworkManagerAPIPriority = 8;
        public const int OSRoadLinkPriority = 8;
        public const int PostCodePriority = 8;
        public const int ActionManagerAPIPriority = 8;

        public const int SavePostalAddressPriority = 8;
        public const int GetPostalAddressSearchDetailsPriority = 8;
        public const int GetPostalAddressDetailsPriority = 8;
        public const int GetPostalAddressDetailsByIdPriority = 8;
        public const int GetDPUsePriority = 8;
        public const int SavePAFDetailsPriority = 8;
        public const int SaveDeliveryPointProcessPriority = 8;
        public const int TokenProviderMiddlewarePriority = 8;
        public const int UpdateDeliveryPointPriority = 8;

        #endregion Priority

        #region EventId

        public const int ActionManagerDataServiceMethodEntryEventId = 8003;
        public const int ActionManagerDataServiceMethodExitEventId = 8004;
        public const int ActionManagerBusinessServiceEntryEventId = 8013;
        public const int ActionManagerBusinessServiceExitEventId = 8014;
        public const int TokenProviderMiddlewareEntryEventId = 8015;
        public const int TokenProviderMiddlewareExitEventId = 8016;

        public const int UserRoleUnitDataServiceMethodEntryEventId = 8013;
        public const int UserRoleUnitDataServiceMethodExitEventId = 8014;

        public const int SearchManagerControllerMethodEntryEventId = 8103;
        public const int SearchManagerControllerMethodExitEventId = 8104;
        public const int SearchManagerBusinessServiceMethodEntryEventId = 8113;
        public const int SearchManagerBusinessServiceMethodExitEventId = 8114;
        public const int SearchManagerIntegrationServiceMethodEntryEventId = 8123;
        public const int SearchManagerIntegrationServiceMethodExitEventId = 8124;

        public const int ReferenceDataControllerMethodEntryEventId = 8203;
        public const int ReferenceDataControllerMethodExitEventId = 8204;
        public const int ReferenceDataBusinessServiceMethodEntryEventId = 8213;
        public const int ReferenceDataBusinessServiceMethodExitEventId = 8214;
        public const int ReferenceDataDataServiceMethodEntryEventId = 8223;
        public const int ReferenceDataDataServiceMethodExitEventId = 8224;
        public const int ReferenceDataCategoryDataServiceMethodEntryEventId = 8233;
        public const int ReferenceDataCategoryDataServiceMethodExitEventId = 8234;

        public const int RouteLogControllerMethodEntryEventId = 8303;
        public const int RouteLogControllerMethodExitEventId = 8304;
        public const int RouteLogBusinessServiceMethodEntryEventId = 8313;
        public const int RouteLogBusinessServiceMethodExitEventId = 8314;
        public const int RouteLogIntegrationServiceMethodEntryEventId = 8323;
        public const int RouteLogIntegrationServiceMethodExitEventId = 8324;

        public const int PDFGeneratorControllerMethodEntryEventId = 8403;
        public const int PDFGeneratorControllerMethodExitEventId = 8404;
        public const int PDFGeneratorBusinessServiceMethodEntryEventId = 8413;
        public const int PDFGeneratorBusinessServiceMethodExitEventId = 8414;

        public const int MapManagerControllerMethodEntryEventId = 8503;
        public const int MapManagerBusinessServiceMethodEntryEventId = 8496;
        public const int MapManagerBusinessServiceMethodExitEventId = 8497;
        public const int MapManagerControllerMethodExitEventId = 8504;
        public const int MapManagerIntegrationServiceMethodEntryEventId = 8486;
        public const int MapManagerIntegrationServiceMethodExitEventId = 8487;

        public const int UnitManagerControllerMethodEntryEventId = 8603;
        public const int UnitManagerControllerMethodExitEventId = 8604;
        public const int UnitManagerBusinessServiceMethodEntryEventId = 8613;
        public const int UnitManagerBusinessServiceMethodExitEventId = 8614;
        public const int ScenarioDataServiceMethodEntryEventId = 8623;
        public const int ScenarioDataServiceMethodExitEventId = 8624;
        public const int UnitLocationDataServiceMethodEntryEventId = 8633;
        public const int UnitLocationDataServiceMethodExitEventId = 8634;

        public const int NetworkManagerControllerMethodEntryEventId = 8703;
        public const int NetworkManagerControllerMethodExitEventId = 8704;
        public const int NetworkManagerBusinessServiceMethodEntryEventId = 8713;
        public const int NetworkManagerBusinessServiceMethodExitEventId = 8714;
        public const int NetworkManagerIntegrationServiceMethodEntryEventId = 8723;
        public const int NetworkManagerIntegrationServiceMethodExitEventId = 8724;
        public const int RoadNameDataServiceMethodEntryEventId = 8733;
        public const int RoadNameDataServiceMethodExitEventId = 8734;
        public const int StreetNetworkDataServiceMethodEntryEventId = 8743;
        public const int StreetNetworkDataServiceMethodExitEventId = 8744;

        public const int OSRoadLinkDataServiceMethodEntryEventId = 8803;
        public const int OSRoadLinkDataServiceMethodExitEventId = 8804;
        public const int PostCodeDataServiceMethodEntryEventId = 8813;
        public const int PostCodeDataServiceMethodExitEventId = 8814;
        public const int PostCodeSectorDataServiceMethodEntryEventId = 8823;
        public const int PostCodeSectorDataServiceMethodExitEventId = 8824;

        public const int DeliveryPointControllerMethodEntryEventId = 9003;
        public const int DeliveryPointControllerMethodExitEventId = 9004;
        public const int DeliveryPointBusinessServiceMethodEntryEventId = 9013;
        public const int DeliveryPointBusinessServiceMethodExitEventId = 9014;
        public const int DeliveryPointDataServiceMethodEntryEventId = 9023;
        public const int DeliveryPointDataServiceMethodExitEventId = 9024;
        public const int DeliveryPointIntegrationServiceMethodEntryEventId = 9033;
        public const int DeliveryPointIntegrationServiceMethodExitEventId = 9034;

        public const int AccessLinkControllerMethodEntryEventId = 9103;
        public const int AccessLinkControllerMethodExitEventId = 9104;
        public const int AccessLinkBusinessMethodEntryEventId = 9113;
        public const int AccessLinkBusinessMethodExitEventId = 9114;
        public const int AccessLinkDataServiceMethodEntryEventId = 9123;
        public const int AccessLinkDataServiceMethodExitEventId = 9124;
        public const int AccessLinkIntegrationMethodEntryEventId = 9133;
        public const int AccessLinkIntegrationMethodExitEventId = 9134;

        public const int DeliveryRouteControllerMethodEntryEventId = 9203;
        public const int DeliveryRouteControllerMethodExitEventId = 9204;
        public const int DeliveryRouteBusinessServiceMethodEntryEventId = 9213;
        public const int DeliveryRouteBusinessServiceMethodExitEventId = 9214;
        public const int DeliveryRouteDataServiceMethodEntryEventId = 9223;
        public const int DeliveryRouteDataServiceMethodExitEventId = 9224;
        public const int DeliveryRouteIntegrationServiceMethodEntryEventId = 9233;
        public const int DeliveryRouteIntegrationServiceMethodExitEventId = 9234;

        public const int PostalAddressControllerMethodEntryEventId = 9303;
        public const int PostalAddressControllerMethodExitEventId = 9304;
        public const int PostalAddressBusinessServiceMethodEntryEventId = 9313;
        public const int PostalAddressBusinessServiceMethodExitEventId = 9314;
        public const int PostalAddressDataServiceMethodEntryEventId = 9323;
        public const int PostalAddressDataServiceMethodExitEventId = 9324;
        public const int PostalAddressIntegrationServiceMethodEntryEventId = 9333;
        public const int PostalAddressIntegrationServiceMethodExitEventId = 9334;
        public const int FileProcessingLogDataServiceMethodEntryEventId = 9343;
        public const int FileProcessingLogDataServiceMethodExitEventId = 9344;

        public const int ThirdPartyAddressLocationControllerMethodEntryEventId = 9403;
        public const int ThirdPartyAddressLocationControllerMethodExitEventId = 9404;
        public const int ThirdPartyAddressLocationBusinessServiceMethodEntryEventId = 9413;
        public const int ThirdPartyAddressLocationBusinessServiceMethodExitEventId = 9414;
        public const int ThirdPartyAddressLocationIntegrationServiceMethodEntryEventId = 9423;
        public const int ThirdPartyAddressLocationIntegrationServiceMethodExitEventId = 9424;
        public const int ThirdPartyAddressLocationDataServiceMethodEntryEventId = 9433;
        public const int ThirdPartyAddressLocationDataServiceMethodExitEventId = 9434;

        public const int NotificationControllerMethodEntryEventId = 9503;
        public const int NotificationControllerMethodExitEventId = 9504;
        public const int NotificationBusinessServiceMethodEntryEventId = 9513;
        public const int NotificationBusinessServiceMethodExitEventId = 9514;
        public const int NotificationDataServiceMethodEntryEventId = 9523;
        public const int NotificationDataServiceMethodExitEventId = 9524;

        public const int NYBLoaderMethodEntryEventId = 9603;
        public const int NYBLoaderMethodExitEventId = 9604;

        public const int PAFLoaderMethodEntryEventId = 9703;
        public const int PAFLoaderMethodExitEventId = 9704;
        public const int PAFReceiverMethodEntryEventId = 9713;
        public const int PAFReceiverMethodExitEventId = 9714;

        public const int ThirdPartyLoaderMethodEntryEventId = 9803;
        public const int ThirdPartyLoaderMethodExitEventId = 9804;

        public const int GetPostalAddressSearchDetailsBusinessMethodEntryEventId = 9129;
        public const int GetPostalAddressSearchDetailsBusinessMethodExitEventId = 9130;
        public const int GetPostalAddressDetailsBusinessMethodEntryEventId = 9131;
        public const int GetPostalAddressDetailsBusinessMethodExitEventId = 9132;
        public const int GetPostalAddressDetailsByIdBusinessMethodEntryEventId = 9133;
        public const int GetPostalAddressDetailsByIdBusinessMethodExitEventId = 9134;

        public const int GetDPUseBusinessMethodEntryEventId = 9321;
        public const int GetDPUseBusinessMethodExitEventId = 9322;
        public const int GetDPUseDataMethodEntryEventId = 9323;
        public const int GetDPUseDataMethodExitEventId = 9324;

        public const int SavePostalAddressBusinessMethodEntryEventId = 9117;
        public const int SavePostalAddressBusinessMethodExitEventId = 9118;
        public const int SavePAFDetailsBusinessMethodEntryEventId = 9121;
        public const int SavePAFDetailsBusinessMethodExitEventId = 9122;
        public const int SaveDeliveryPointProcessBusinessMethodEntryEventId = 9125;
        public const int SaveDeliveryPointProcessBusinessMethodExitEventId = 9126;

        public const int BlockSequenceDataServiceMethodEntryEventId = 9865;
        public const int BlockSequenceDataServiceMethodExitEventId = 9866;

        #endregion EventId

        #region Title

        public const string Title = "Trace Log";

        #endregion Title
    }
}