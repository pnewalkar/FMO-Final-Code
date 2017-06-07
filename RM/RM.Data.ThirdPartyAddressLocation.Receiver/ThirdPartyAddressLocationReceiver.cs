using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Threading;
using System.Timers;
using System.Reflection;
using RM.CommonLibrary.MessageBrokerMiddleware;
using RM.CommonLibrary.EntityFramework.DTO.FileProcessing;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.LoggingMiddleware;
using RM.CommonLibrary.HelperMiddleware;
using RM.CommonLibrary.EntityFramework.DataService.MappingConfiguration;
using System.Diagnostics;

namespace RM.Data.ThirdPartyAddressLocation.Receiver
{
    /// <summary>
    /// Receiver class run as a windows service to listen to the queue.
    /// </summary>
    public partial class ThirdPartyAddressLocationReceiver : ServiceBase
    {
        private string USRWebApiName = string.Empty;

        private System.Timers.Timer m_mainTimer;
        private IMessageBroker<AddressLocationUSRDTO> msgUSR = default(IMessageBroker<AddressLocationUSRDTO>);
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Constructor for the receiver class.
        /// </summary>
        public ThirdPartyAddressLocationReceiver(IMessageBroker<AddressLocationUSRDTO> msgUSR, IConfigurationHelper configurationHelper, ILoggingHelper loggingHelper, IHttpHandler httpHandler)
        {
            this.msgUSR = msgUSR;
            this.configurationHelper = configurationHelper;
            this.loggingHelper = loggingHelper;
            this.httpHandler = httpHandler;

            this.USRWebApiName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.USRWEBAPINAME).ToString() : string.Empty;
            this.ServiceName = configurationHelper.ReadAppSettingsConfigurationValues(Constants.ServiceName);
        }

        /// <summary>
        /// USRMessageReceived to process the USR messages popped out of the queue.
        /// </summary>
        public void USRMessageReceived()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                List<AddressLocationUSRDTO> lstAddressLocationUSR = new List<AddressLocationUSRDTO>();

                //The Message queue is checked whether there are pending messages in the queue. This loop runs till all the messages are popped out of the message queue.
                while (msgUSR.HasMessage(Constants.QUEUETHIRDPARTY, Constants.QUEUEPATH))
                {
                    //Receive Message picks up one message from queue for processing
                    //Message broker internally deserializes the message to the POCO type.
                    AddressLocationUSRDTO objAddressLocationUSR = msgUSR.ReceiveMessage(Constants.QUEUETHIRDPARTY, Constants.QUEUEPATH);
                    if (objAddressLocationUSR != null)
                    {
                        lstAddressLocationUSR.Add(objAddressLocationUSR);
                    }
                }

                if (lstAddressLocationUSR != null && lstAddressLocationUSR.Count > 0)
                    SaveUSRDetails(lstAddressLocationUSR).Wait();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        /// <summary>
        /// Test method. Helps during running the code in debug mode.
        /// </summary>
        public void OnDebug()
        {
            OnStart(null);
        }

        /// <summary>
        /// On Start windows service method.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                //instantiate timer
                Thread t = new Thread(new ThreadStart(this.InitTimer));
                t.Start();
            }
            catch (Exception ex)
            {
                loggingHelper.Log(ex, TraceEventType.Error);
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        protected override void OnStop()
        {
            m_mainTimer.Enabled = false;
        }

        /// <summary>
        /// InitTimer() to initialise the timer and register the handlers.
        /// </summary>
        private void InitTimer()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                m_mainTimer = new System.Timers.Timer();

                //wire up the timer event
                m_mainTimer.Elapsed += new ElapsedEventHandler(m_mainTimer_Elapsed);

                //set timer interval
                //var timeInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["TimerIntervalInSeconds"]);
                double timeInSeconds = 15.0;
                m_mainTimer.Interval = (timeInSeconds * 1000);

                // timer.Interval is in milliseconds, so times above by 1000
                m_mainTimer.Enabled = true;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        /// <summary>
        /// SaveUSRDetails to save the USR data by calling the WebApi services.
        /// </summary>
        /// <param name="addressLocationUSRDTO"></param>
        /// <returns></returns>
        private async Task SaveUSRDetails(List<AddressLocationUSRDTO> addressLocationUSRDTO)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                var addressLocationUSRPOSTDTO = GenericMapper.MapList<AddressLocationUSRDTO, AddressLocationUSRPOSTDTO>(addressLocationUSRDTO);
                addressLocationUSRPOSTDTO.ForEach(addressLocation =>
                {
                    LogMethodInfoBlock(
                                        methodName,
                                        string.Format(
                                        Constants.REQUESTLOG,
                                        addressLocation.UDPRN == null ? string.Empty : addressLocation.UDPRN.ToString(),
                                        addressLocation.XCoordinate == null ? string.Empty : addressLocation.XCoordinate.ToString(),
                                        addressLocation.YCoordinate == null ? string.Empty : addressLocation.YCoordinate.ToString(),
                                        addressLocation.Latitude == null ? string.Empty : addressLocation.Latitude.ToString(),
                                        addressLocation.Longitude == null ? string.Empty : addressLocation.Longitude.ToString(),
                                        addressLocation.ChangeType == null ? string.Empty : addressLocation.ChangeType
                                        ));
                });
                var result = await httpHandler.PostAsJsonAsync(USRWebApiName, addressLocationUSRPOSTDTO, isBatchJob: true);
                if (!result.IsSuccessStatusCode)
                {
                    var responseContent = result.ReasonPhrase;
                    this.loggingHelper.Log(responseContent, TraceEventType.Error);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        /// <summary>
        /// Timer Elapsed event handler to handle the Elapsed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_mainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                USRMessageReceived();
            }
            catch (Exception ex)
            {
                loggingHelper.Log(ex, TraceEventType.Error);
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        /// <summary>
        /// OnStop method of the web service to disable the timer.
        /// </summary>
        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        private void LogMethodInfoBlock(string methodName, string logMessage)
        {
            loggingHelper.Log(methodName + Constants.COLON + Constants.MethodExecutionCompleted, TraceEventType.Information, null, LoggerTraceConstants.Category, LoggerTraceConstants.SavePostalAddressPriority, LoggerTraceConstants.SavePostalAddressBusinessMethodExitEventId, LoggerTraceConstants.Title);
        }
    }
}