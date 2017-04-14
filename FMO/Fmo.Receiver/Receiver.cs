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
        private IHttpHandler httpHandler = default(IHttpHandler);
        private IConfigurationHelper configurationHelper = default(IConfigurationHelper);

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
            kernal.Bind<IHttpHandler>().To<HttpHandler>().InSingletonScope();
            kernal.Bind<IConfigurationHelper>().To<ConfigurationHelper>().InSingletonScope();
            msgUSR = kernel.Get<IMessageBroker<AddressLocationUSRDTO>>();
            msgPAF = kernel.Get<IMessageBroker<PostalAddressDTO>>();
            httpHandler = kernel.Get<IHttpHandler>();
            configurationHelper = kernal.Get<IConfigurationHelper>();

            this.PAFWebApiurl = configurationHelper.ReadAppSettingsConfigurationValues("PAFWebApiurl").ToString();
            this.PAFWebApiName = configurationHelper.ReadAppSettingsConfigurationValues("PAFWebApiName").ToString();
            this.USRWebApiurl = configurationHelper.ReadAppSettingsConfigurationValues("USRWebApiurl").ToString();
            this.USRWebApiName = configurationHelper.ReadAppSettingsConfigurationValues("USRWebApiName").ToString();
        }

        protected override void OnStart(string[] args)
        {
            double interval = 15000.0;
            m_mainTimer = new System.Timers.Timer(interval);

            // Hook up the event handler for the Elapsed event.
            m_mainTimer.Elapsed += new ElapsedEventHandler(m_mainTimer_Elapsed);

            // Only raise the event the first time Interval elapses.
            m_mainTimer.AutoReset = true;
            m_mainTimer.Enabled = true;
            // Start();
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
            AddressLocationUSRDTO addressLocationUSRDTO = e.MessageBody;

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
            }

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
        private async Task SaveUSRDetails(AddressLocationUSRDTO addressLocationUSRDTO)
        {
            try
            {
                httpHandler.SetBaseAddress(new Uri(USRWebApiurl));
                var addressLocationUSRPOSTDTO = GenericMapper.Map<AddressLocationUSRDTO, AddressLocationUSRPOSTDTO>(addressLocationUSRDTO);
                await httpHandler.PostAsJsonAsync(USRWebApiName, addressLocationUSRPOSTDTO);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public void PAFMessageReceived()
        {
            List<PostalAddressDTO> lst = new List<PostalAddressDTO>();

            while (msgPAF.HasMessage(Constants.QUEUE_PAF, Constants.QUEUE_PATH))
            {
                PostalAddressDTO b = msgPAF.ReceiveMessage(Constants.QUEUE_PAF, Constants.QUEUE_PATH);
                if (b != null)
                {
                    lst.Add(b);
                }
            }
            SavePAFDetails(lst);

        }

        void m_mainTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                // do some work
                PAFMessageReceived();
            }
            catch (Exception ex)
            {
            }
            
        }

        protected override void OnStop()
        {           
        }

        public void OnDebug()
        {
            OnStart(null);
        }
    }
}
