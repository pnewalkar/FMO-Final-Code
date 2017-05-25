namespace Fmo.Common.Constants
{
    public static class LoggerTraceConstants
    {
        public const string Category = "General";

        #region Priority

        public const int CreateDeliveryPointPriority = 8;
        public const int UpdateDeliveryPointPriority = 8;
        public const int CreateAutomaticAccessLinkPriority = 8;
        public const int CreateManualAccessLinkPriority = 8;

        #endregion Priority

        #region EventId

        public const int CreateDeliveryPointAPIMethodEntryEventId = 9003;
        public const int CreateDeliveryPointAPIMethodExitEventId = 9004;
        public const int CreateDeliveryPoinBusinessMethodEntryEventId = 9013;
        public const int CreateDeliveryPoinBusinessMethodExitEventId = 9014;

        public const int UpdateDeliveryPointAPIMethodEntryEventId = 9103;
        public const int UpdateDeliveryPointAPIMethodExitEventId = 9104;
        public const int UpdateDeliveryPoinBusinessMethodEntryEventId = 9113;
        public const int UpdateDeliveryPoinBusinessMethodExitEventId = 9114;

        public const int CreateAutomaticAccessLinkBusinessMethodEntryEventId = 9313;
        public const int CreateAutomaticAccessLinkBusinessMethodExitEventId = 9314;
        public const int CreateManualAccessLinkBusinessMethodEntryEventId = 9315;
        public const int CreateManualAccessLinkBusinessMethodExitEventId = 9316;

        #endregion EventId

        #region Title

        public const string Title = "Trace Log";

        #endregion Title
    }
}