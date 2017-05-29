﻿using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Fmo.Common.ConfigurationManagement;
using Fmo.Common.Constants;
using Fmo.Common.Interface;
using Fmo.Common.LoggingManagement;
using Fmo.DTO;
using Fmo.DTO.FileProcessing;
using Fmo.MappingConfiguration;
using Fmo.MessageBrokerCore.Messaging;
using Fmo.NYBLoader.Common;
using Fmo.NYBLoader.Interfaces;
using Ninject;

namespace Fmo.Receiver
{
    /// <summary>
    /// Receiver class run as a windows service to listen to the queue.
    /// </summary>
    public partial class Receiver : ServiceBase
    {
        private string PAFWebApiName = string.Empty;
        private string USRWebApiName = string.Empty;

        private System.Timers.Timer m_mainTimer;

        private readonly IKernel kernal;
        private IMessageBroker<AddressLocationUSRDTO> msgUSR = default(IMessageBroker<AddressLocationUSRDTO>);
        private IMessageBroker<PostalAddressDTO> msgPAF = default(IMessageBroker<PostalAddressDTO>);
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        /// <summary>
        /// Constructor for the receiver class.
        /// </summary>
        public Receiver()
        {
            kernal = new StandardKernel();
            Register(kernal);

            InitializeComponent();
        }

        /// <summary>
        /// Register the dependencies in the container.
        /// </summary>
        /// <param name="kernel"></param>
        protected void Register(IKernel kernel)
        {
            if (kernel != null)
            {
                kernel.Bind<IMessageBroker<AddressLocationUSRDTO>>().To<MessageBroker<AddressLocationUSRDTO>>().InSingletonScope();
                kernel.Bind<IMessageBroker<PostalAddressDTO>>().To<MessageBroker<PostalAddressDTO>>().InSingletonScope();
                kernal.Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
                kernal.Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope();
                kernel.Bind<IHttpHandler>().To<HttpHandler>();
                msgUSR = kernel.Get<IMessageBroker<AddressLocationUSRDTO>>();
                msgPAF = kernel.Get<IMessageBroker<PostalAddressDTO>>();
                configurationHelper = kernal.Get<IConfigurationHelper>();
                loggingHelper = kernal.Get<ILoggingHelper>();
                httpHandler = kernal.Get<IHttpHandler>();
            }

            this.PAFWebApiName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.PAFWEBAPINAME).ToString() : string.Empty;
            this.USRWebApiName = configurationHelper != null ? configurationHelper.ReadAppSettingsConfigurationValues(Constants.USRWEBAPINAME).ToString() : string.Empty;
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

                // Start();
            }
            catch (Exception ex)
            {
                loggingHelper.LogError(ex);
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
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
                double timeInSeconds = 3.0;
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
        /// SavePAFDetails to save the PAF data by calling the WebApi services.
        /// </summary>
        /// <param name="postalAddress"></param>
        /// <returns></returns>
        private async Task SavePAFDetails(List<PostalAddressDTO> postalAddress)
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                if (postalAddress != null && postalAddress.Count > 0)
                {
                    postalAddress.ForEach(objPostalAddress =>
                    {
                        LogMethodInfoBlock(
                                            methodName,
                                            string.Format(
                                            Constants.REQUESTLOG,
                                            objPostalAddress.UDPRN == null ? string.Empty : objPostalAddress.UDPRN.ToString(),
                                            objPostalAddress.Postcode == null ? string.Empty : objPostalAddress.Postcode.ToString(),
                                            objPostalAddress.AmendmentType == null ? string.Empty : objPostalAddress.AmendmentType.ToString(),
                                            objPostalAddress.PostTown == null ? string.Empty : objPostalAddress.PostTown.ToString(),
                                            objPostalAddress.SmallUserOrganisationIndicator == null ? string.Empty : objPostalAddress.SmallUserOrganisationIndicator.ToString(),
                                            objPostalAddress.DeliveryPointSuffix == null ? string.Empty : objPostalAddress.DeliveryPointSuffix
                                            ));
                    });
                    await httpHandler.PostAsJsonAsync(PAFWebApiName, postalAddress);
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
                await httpHandler.PostAsJsonAsync(USRWebApiName, addressLocationUSRPOSTDTO);
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
        /// PAFMessageReceived to process the PAF messages popped out of the queue.
        /// </summary>
        public void PAFMessageReceived()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();

                while (msgPAF.HasMessage(Constants.QUEUEPAF, Constants.QUEUEPATH))
                {
                    PostalAddressDTO objPostalAddress = msgPAF.ReceiveMessage(Constants.QUEUEPAF, Constants.QUEUEPATH);
                    if (objPostalAddress != null)
                    {
                        lstPostalAddress.Add(objPostalAddress);
                    }
                }
                if (lstPostalAddress != null && lstPostalAddress.Count > 0)
                    SavePAFDetails(lstPostalAddress).Wait();
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
        /// USRMessageReceived to process the USR messages popped out of the queue.
        /// </summary>
        public void USRMessageReceived()
        {
            string methodName = MethodBase.GetCurrentMethod().Name;
            LogMethodInfoBlock(methodName, Constants.MethodExecutionStarted);
            try
            {
                List<AddressLocationUSRDTO> lstAddressLocationUSR = new List<AddressLocationUSRDTO>();

                while (msgUSR.HasMessage(Constants.QUEUETHIRDPARTY, Constants.QUEUEPATH))
                {
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
                PAFMessageReceived();
            }
            catch (Exception ex)
            {
                loggingHelper.LogError(ex);
            }
            finally
            {
                LogMethodInfoBlock(methodName, Constants.MethodExecutionCompleted);
            }
        }

        /// <summary>
        /// OnStop method of the web service to disable the timer.
        /// </summary>

        protected override void OnStop()
        {
            m_mainTimer.Enabled = false;
        }

        /// <summary>
        /// Test method. Helps during running the code in debug mode.
        /// </summary>
        public void OnDebug()
        {
            OnStart(null);
        }

        /// <summary>
        /// Method level entry exit logging.
        /// </summary>
        /// <param name="methodName">Function Name</param>
        /// <param name="logMessage">Message</param>
        private void LogMethodInfoBlock(string methodName, string logMessage)
        {
            this.loggingHelper.LogInfo(string.Concat(methodName, Constants.COLON, logMessage));
        }
    }
}