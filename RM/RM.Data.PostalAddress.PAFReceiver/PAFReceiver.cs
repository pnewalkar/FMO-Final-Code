namespace RM.Data.PostalAddress.PAFReceiver
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.Reflection;
    using System.ServiceProcess;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using CommonLibrary.Utilities.HelperMiddleware;
    using RM.CommonLibrary.ConfigurationMiddleware;
    using RM.CommonLibrary.EntityFramework.DTO;
    using RM.CommonLibrary.HelperMiddleware;
    using RM.CommonLibrary.Interfaces;
    using RM.CommonLibrary.LoggingMiddleware;
    using RM.CommonLibrary.MessageBrokerMiddleware;

    /// <summary>
    /// Receiver class run as a windows service to listen to the queue.
    /// </summary>
    public partial class PAFReceiver : ServiceBase
    {
        private const string REQUESTLOG = "udprn: {0} xCoordinate: {1} yCoordinate:{2} latitude:{3} longitude:{4} changeType:{5}";
        private const string PAFWEBAPINAME = "PAFWebApiName";
        private const string QUEUEPAF = "QUEUE_PAF";
        private const string QUEUEPATH = @".\Private$\";
        private const string TimerIntervalInSeconds = "TimerIntervalInSeconds";
        private const string BatchServiceName = "ServiceName";

        #region private member declaration

        private string PAFWebApiName = string.Empty;
        private double timerIntervalInSeconds = default(double);

        private System.Timers.Timer m_mainTimer;
        private IMessageBroker<PostalAddressDTO> msgPAF = default(IMessageBroker<PostalAddressDTO>);
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);
        private int count = 0;

        #endregion private member declaration

        #region Constructor

        /// <summary>
        /// Constructor for the receiver class.
        /// </summary>
        public PAFReceiver(IMessageBroker<PostalAddressDTO> msgPAF, IHttpHandler httpHandler, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper)
        {
            this.msgPAF = msgPAF;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.httpHandler = httpHandler;

            this.PAFWebApiName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(PAFWEBAPINAME).ToString() : string.Empty;
            this.ServiceName = configurationHelper.ReadAppSettingsConfigurationValues(BatchServiceName);
            this.timerIntervalInSeconds = configurationHelper != null ? Convert.ToDouble(configurationHelper.ReadAppSettingsConfigurationValues(TimerIntervalInSeconds)) : default(double);
        }

        #endregion Constructor

        #region OnStart

        /// <summary>
        /// On Start windows service method.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;

            //instantiate timer
            Thread t = new Thread(new ThreadStart(this.InitTimer));
            t.Start();
        }

        #endregion OnStart

        #region Start timer

        /// <summary>
        /// InitTimer() to initialise the timer and register the handlers.
        /// </summary>
        private void InitTimer()
        {
            m_mainTimer = new System.Timers.Timer();

            //wire up the timer event
            m_mainTimer.Elapsed += new ElapsedEventHandler(m_mainTimer_Elapsed);

            //set timer interval
            //var timeInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["TimerIntervalInSeconds"]);
            double timeInSeconds = timerIntervalInSeconds;
            m_mainTimer.Interval = (timeInSeconds * 1000);

            // timer.Interval is in milliseconds, so times above by 1000
            m_mainTimer.Enabled = true;
        }

        /// <summary>
        /// Timer Elapsed event handler to handle the Elapsed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_mainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Service.m_mainTimer_Elapsed"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                try
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFReceiverMethodEntryEventId, LoggerTraceConstants.Title);
                    PAFMessageReceived();
                }
                catch (Exception ex)
                {
                    loggingHelper.Log(ex, TraceEventType.Error);
                }
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFReceiverMethodExitEventId, LoggerTraceConstants.Title);
            }
        }

        #endregion Start timer

        #region Process PAF from Queue

        /// <summary>
        /// PAFMessageReceived to process the PAF messages popped out of the queue.
        /// </summary>
        public void PAFMessageReceived()
        {
            using (loggingHelper.RMTraceManager.StartTrace("Service.PAFMessageReceived"))
            {
                string methodName = MethodBase.GetCurrentMethod().Name;
                try
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFReceiverMethodEntryEventId, LoggerTraceConstants.Title);

                    List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();

                    //The Message queue is checked whether there are pending messages in the queue. This loop runs till all the messages are popped out of the message queue.
                    while (msgPAF.HasMessage(QUEUEPAF, QUEUEPATH))
                    {
                        //Receive Message picks up one message from queue for processing
                        //Message broker internally deserializes the message to the POCO type.
                        PostalAddressDTO objPostalAddress = msgPAF.ReceiveMessage(QUEUEPAF, QUEUEPATH);
                        if (objPostalAddress != null)
                        {
                            lstPostalAddress.Add(objPostalAddress);
                        }
                    }
                    if (lstPostalAddress != null && lstPostalAddress.Count > 0)
                    {
                        m_mainTimer.Elapsed -= new ElapsedEventHandler(m_mainTimer_Elapsed);
                        var result = SavePAFDetails(lstPostalAddress).ContinueWith(p => {
                            m_mainTimer.Elapsed += new ElapsedEventHandler(m_mainTimer_Elapsed);
                        });
                    }
                }
                catch (Exception ex)
                {
                    this.loggingHelper.Log(ex, TraceEventType.Error);
                }
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFReceiverMethodExitEventId, LoggerTraceConstants.Title);
            }
        }

        /// <summary>
        /// SavePAFDetails to save the PAF data by calling the WebApi services.
        /// </summary>
        /// <param name="postalAddress"></param>
        /// <returns></returns>
        private async Task<bool> SavePAFDetails(List<PostalAddressDTO> postalAddress)
        {
            using (loggingHelper.RMTraceManager.StartTrace("Service.SavePAFDetails"))
            {
                string methodName = MethodHelper.GetActualAsyncMethodName();
                bool isPAFDetailsInserted = false;
                try
                {
                    loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFReceiverMethodEntryEventId, LoggerTraceConstants.Title);
                    if (postalAddress != null && postalAddress.Count > 0)
                    {
                        postalAddress.ForEach(objPostalAddress =>
                        {
                            LogMethodInfoBlock(
                                                methodName,
                                                string.Format(
                                                REQUESTLOG,
                                                objPostalAddress.UDPRN == null ? string.Empty : objPostalAddress.UDPRN.ToString(),
                                                objPostalAddress.Postcode == null ? string.Empty : objPostalAddress.Postcode.ToString(),
                                                objPostalAddress.AmendmentType == null ? string.Empty : objPostalAddress.AmendmentType.ToString(),
                                                objPostalAddress.PostTown == null ? string.Empty : objPostalAddress.PostTown.ToString(),
                                                objPostalAddress.SmallUserOrganisationIndicator == null ? string.Empty : objPostalAddress.SmallUserOrganisationIndicator.ToString(),
                                                objPostalAddress.DeliveryPointSuffix == null ? string.Empty : objPostalAddress.DeliveryPointSuffix
                                                ));
                        });

                        var result = await httpHandler.PostAsJsonAsync(PAFWebApiName, postalAddress, true);
                        if (!result.IsSuccessStatusCode)
                        {
                            //LOG ERROR WITH Statuscode
                            var responseContent = result.ReasonPhrase;
                            this.loggingHelper.Log(responseContent, TraceEventType.Error);
                            isPAFDetailsInserted = false;
                        }
                        isPAFDetailsInserted = true;
                    }
                }
                catch (AggregateException ae)
                {
                    foreach (var exception in ae.InnerExceptions)
                    {
                        loggingHelper.Log(exception, TraceEventType.Error);
                    }

                    var realExceptions = ae.Flatten().InnerException;
                    throw realExceptions;
                }
                loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionCompleted, TraceEventType.Verbose, null, LoggerTraceConstants.Category, LoggerTraceConstants.PAFPriority, LoggerTraceConstants.PAFReceiverMethodExitEventId, LoggerTraceConstants.Title);

                return isPAFDetailsInserted;
            }
        }

        #endregion Process PAF from Queue

        #region OnStop

        /// <summary>
        /// OnStop method of the web service to disable the timer.
        /// </summary>
        protected override void OnStop()
        {
            m_mainTimer.Enabled = false;
        }

        #endregion OnStop

        #region On Debug test method

        /// <summary>
        /// Test method. Helps during running the code in debug mode.
        /// </summary>
        public void OnDebug()
        {
            OnStart(null);
        }

        #endregion On Debug test method

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        private void LogMethodInfoBlock(string methodName, string logMessage)
        {
            loggingHelper.Log(methodName + LoggerTraceConstants.COLON + LoggerTraceConstants.MethodExecutionStarted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.GetPostalAddressDetailsPriority, LoggerTraceConstants.GetPostalAddressDetailsBusinessMethodEntryEventId, LoggerTraceConstants.Title);
        }
    }
}