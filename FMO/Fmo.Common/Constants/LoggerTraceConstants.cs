namespace Fmo.Common.Constants
{
    public static class LoggerTraceConstants
    {
        public const string Category = "General";

        #region Priority

        public const int CreateDeliveryPointPriority = 8;
        public const int UpdateDeliveryPointPriority = 8;

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

        #endregion EventId

        #region Title

        public const string Title = "Trace Log";

        #endregion Title
    }
}