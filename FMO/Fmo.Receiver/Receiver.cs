using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
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
    public partial class Receiver : ServiceBase
    {
        private string PAFWebApiurl = string.Empty;
        private string PAFWebApiName = string.Empty;
        private string USRWebApiurl = string.Empty;
        private string USRWebApiName = string.Empty;

        private System.Timers.Timer m_mainTimer;
        private bool m_timerTaskSuccess;

        private readonly IKernel kernal;
        private IMessageBroker<AddressLocationUSRDTO> msgUSR = default(IMessageBroker<AddressLocationUSRDTO>);
        private IMessageBroker<PostalAddressDTO> msgPAF = default(IMessageBroker<PostalAddressDTO>);
        //private IHttpHandler httpHandler = default(IHttpHandler);
        private IHttpHandler httpHandler;
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);
        private ILoggingHelper loggingHelper = default(ILoggingHelper);

        public Receiver()
        {
            kernal = new StandardKernel();
            Register(kernal);
            //this.httpHandler = httpHandler;
            //this.configurationHelper = configurationHelper;


            InitializeComponent();
        }
        protected void Register(IKernel kernel)
        {
            kernel.Bind<IMessageBroker<AddressLocationUSRDTO>>().To<MessageBroker<AddressLocationUSRDTO>>().InSingletonScope();
            kernel.Bind<IMessageBroker<PostalAddressDTO>>().To<MessageBroker<PostalAddressDTO>>().InSingletonScope();
            //kernal.Bind<IHttpHandler>().To<HttpHandler>().InTransientScope();
            kernal.Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
            kernal.Bind<ILoggingHelper>().To<LoggingHelper>().InSingletonScope();
            msgUSR = kernel.Get<IMessageBroker<AddressLocationUSRDTO>>();
            msgPAF = kernel.Get<IMessageBroker<PostalAddressDTO>>();
            //httpHandler = kernel.Get<IHttpHandler>();
            configurationHelper = kernal.Get<IConfigurationHelper>();
            loggingHelper = kernal.Get<ILoggingHelper>();

            this.PAFWebApiurl = configurationHelper.ReadAppSettingsConfigurationValues("PAFWebApiurl").ToString();
            this.PAFWebApiName = configurationHelper.ReadAppSettingsConfigurationValues("PAFWebApiName").ToString();
            this.USRWebApiurl = configurationHelper.ReadAppSettingsConfigurationValues("USRWebApiurl").ToString();
            this.USRWebApiName = configurationHelper.ReadAppSettingsConfigurationValues("USRWebApiName").ToString();
        }

        protected override void OnStart(string[] args)
        {
            //instantiate timer
            Thread t = new Thread(new ThreadStart(this.InitTimer));
            t.Start();
            // Start();
        }

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

        private void Start()
        {
            Thread threadAddressUSRDTO = new Thread(StartUSR);
            threadAddressUSRDTO.Start();

            msgPAF.Start(Constants.QUEUE_PAF, Constants.QUEUE_PATH, PAFMessageReceived);
            //Thread threadAddressPAFDTO = new Thread(StartPAF);
            //threadAddressPAFDTO.Start();
        }

        public void StartUSR()
        {
            msgUSR.Start(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH, USRMessageReceived);
        }

        public void StartPAF()
        {
            msgPAF.Start(Constants.QUEUE_PAF, Constants.QUEUE_PATH, PAFMessageReceived);
        }

        public void USRMessageReceived(object sender, MessageEventArgs<AddressLocationUSRDTO> e)
        {
            /*AddressLocationUSRDTO addressLocationUSRDTO = e.MessageBody;

            if (addressLocationUSRDTO != null)
            {
                SaveUSRDetails(addressLocationUSRDTO).Wait();
            }

            while (msgUSR.HasMessage(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH))
            {
                AddressLocationUSRDTO addressLocationUSRDTOPending = msgUSR.ReceiveMessage(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                if (addressLocationUSRDTOPending != null)
                {
                    SaveUSRDetails(addressLocationUSRDTOPending).Wait();
                }
            }*/

        }

        public void PAFMessageReceived(object sender, MessageEventArgs<PostalAddressDTO> e)
        {
            //PostalAddressDTO a = e.MessageBody;
            //if (a != null)
            //{
            //    SavePAFDetails(a).Wait();
            //}

            //while (msgPAF.HasMessage(Constants.QUEUE_PAF, Constants.QUEUE_PATH))
            //{
            //    PostalAddressDTO b = msgPAF.ReceiveMessage(Constants.QUEUE_PAF, Constants.QUEUE_PATH);
            //    if (b != null)
            //    {
            //        SavePAFDetails(b).Wait();
            //    }
            //}

        }

        private async Task SavePAFDetails(List<PostalAddressDTO> postalAddress)
        {
            bool saveFlag = false;
            try
            {
                if (postalAddress != null && postalAddress.Count > 0)
                {
                    httpHandler = new HttpHandler();
                    httpHandler.SetBaseAddress(new Uri(PAFWebApiurl));
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
        private async Task SaveUSRDetails(List<AddressLocationUSRDTO> addressLocationUSRDTO)
        {
            try
            {
                httpHandler = new HttpHandler();
                httpHandler.SetBaseAddress(new Uri(USRWebApiurl));
                var addressLocationUSRPOSTDTO = GenericMapper.MapList<AddressLocationUSRDTO, AddressLocationUSRPOSTDTO>(addressLocationUSRDTO);
                await httpHandler.PostAsJsonAsync(USRWebApiName, addressLocationUSRPOSTDTO);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void PAFMessageReceived()
        {
            try
            {                
                List<PostalAddressDTO> lstPostalAddress = new List<PostalAddressDTO>();

                while (msgPAF.HasMessage(Constants.QUEUE_PAF, Constants.QUEUE_PATH))
                {
                    PostalAddressDTO objPostalAddress = msgPAF.ReceiveMessage(Constants.QUEUE_PAF, Constants.QUEUE_PATH);
                    if (objPostalAddress != null)
                    {
                        lstPostalAddress.Add(objPostalAddress);
                    }
                }
                if (lstPostalAddress != null && lstPostalAddress.Count > 0)
                    SavePAFDetails(lstPostalAddress);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void USRMessageReceived()
        {
            try
            {
                List<AddressLocationUSRDTO> lstAddressLocationUSR = new List<AddressLocationUSRDTO>();

                while (msgUSR.HasMessage(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH))
                {
                    AddressLocationUSRDTO objAddressLocationUSR = msgUSR.ReceiveMessage(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
                    if (objAddressLocationUSR != null)
                    {
                        lstAddressLocationUSR.Add(objAddressLocationUSR);
                    }
                }

                if(lstAddressLocationUSR != null && lstAddressLocationUSR.Count > 0)
                    SaveUSRDetails(lstAddressLocationUSR);

            }
            catch (Exception)
            {
                throw;
            }
        }

        void m_mainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                // do some work
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

        protected override void OnStop()
        {
            m_mainTimer.Enabled = false;
        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
