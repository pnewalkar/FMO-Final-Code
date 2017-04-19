using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Threading.Tasks;
using Fmo.MessageBrokerCore.Messaging;
using Fmo.DTO;
using Fmo.DTO.FileProcessing;
using Ninject;
using System.Threading;
using Fmo.Common.Constants;
using Fmo.NYBLoader.Interfaces;
using Fmo.NYBLoader.Common;
using Fmo.Common.Interface;
using Fmo.Common.ConfigurationManagement;
using Fmo.MappingConfiguration;
using System.Timers;
using Fmo.Common.LoggingManagement;

namespace Fmo.Receiver
{
    /// <summary>
    /// Receiver class run as a windows service to listen to the queue.
    /// </summary>
    public partial class Receiver : ServiceBase
    {
        private string PAFWebApiurl = string.Empty;
        private string PAFWebApiName = string.Empty;
        private string USRWebApiurl = string.Empty;
        private string USRWebApiName = string.Empty;

        private System.Timers.Timer m_mainTimer;

        private readonly IKernel kernal;
        private IMessageBroker<AddressLocationUSRDTO> msgUSR = default(IMessageBroker<AddressLocationUSRDTO>);
        private IMessageBroker<PostalAddressDTO> msgPAF = default(IMessageBroker<PostalAddressDTO>);
        private IHttpHandler httpHandler;
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
            kernel.Bind<IMessageBroker<AddressLocationUSRDTO>>().To<MessageBroker<AddressLocationUSRDTO>>().InSingletonScope();
            kernel.Bind<IMessageBroker<PostalAddressDTO>>().To<MessageBroker<PostalAddressDTO>>().InSingletonScope();
            kernal.Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
            kernal.Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope();
            msgUSR = kernel.Get<IMessageBroker<AddressLocationUSRDTO>>();
            msgPAF = kernel.Get<IMessageBroker<PostalAddressDTO>>();
            configurationHelper = kernal.Get<IConfigurationHelper>();
            loggingHelper = kernal.Get<ILoggingHelper>();

            this.PAFWebApiurl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.PAFWEBAPIURL).ToString();
            this.PAFWebApiName = configurationHelper.ReadAppSettingsConfigurationValues(Constants.PAFWEBAPINAME).ToString();
            this.USRWebApiurl = configurationHelper.ReadAppSettingsConfigurationValues(Constants.USRWEBAPIURL).ToString();
            this.USRWebApiName = configurationHelper.ReadAppSettingsConfigurationValues(Constants.USRWEBAPINAME).ToString();
        }

        /// <summary>
        /// On Start windows service method.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            //instantiate timer
            Thread t = new Thread(new ThreadStart(this.InitTimer));
            t.Start();
            // Start();
        }

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
            double timeInSeconds = 3.0;
            m_mainTimer.Interval = (timeInSeconds * 1000);
            // timer.Interval is in milliseconds, so times above by 1000 
            m_mainTimer.Enabled = true;
        }

        /// <summary>
        /// SavePAFDetails to save the PAF data by calling the WebApi services.
        /// </summary>
        /// <param name="postalAddress"></param>
        /// <returns></returns>
        private async Task SavePAFDetails(List<PostalAddressDTO> postalAddress)
        {
            bool saveFlag = false;
            try
            {
                if (postalAddress != null && postalAddress.Count > 0)
                {
                    httpHandler = new HttpHandler();
                    await httpHandler.PostAsJsonAsync(PAFWebApiName, postalAddress);
                    saveFlag = true;
                }
            }
            catch (Exception)
            {
                saveFlag = false;
                throw;
            }

        }

        /// <summary>
        /// SaveUSRDetails to save the USR data by calling the WebApi services.
        /// </summary>
        /// <param name="addressLocationUSRDTO"></param>
        /// <returns></returns>
        private async Task SaveUSRDetails(List<AddressLocationUSRDTO> addressLocationUSRDTO)
        {
            try
            {
                httpHandler = new HttpHandler();
                var addressLocationUSRPOSTDTO = GenericMapper.MapList<AddressLocationUSRDTO, AddressLocationUSRPOSTDTO>(addressLocationUSRDTO);
                await httpHandler.PostAsJsonAsync(USRWebApiName, addressLocationUSRPOSTDTO);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// PAFMessageReceived to process the PAF messages popped out of the queue.
        /// </summary>
        public void PAFMessageReceived()
        {
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
        }

        /// <summary>
        /// USRMessageReceived to process the USR messages popped out of the queue.
        /// </summary>
        public void USRMessageReceived()
        {
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

                if(lstAddressLocationUSR != null && lstAddressLocationUSR.Count > 0)
                    SaveUSRDetails(lstAddressLocationUSR).Wait();

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Timer Elapsed event handler to handle the Elapsed event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_mainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                USRMessageReceived();
                PAFMessageReceived();
            }
            catch (Exception ex)
            {
                loggingHelper.LogInfo(ex.Message);

                if(ex.InnerException != null)
                    loggingHelper.LogInfo(ex.InnerException.Message);

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
    }
}
