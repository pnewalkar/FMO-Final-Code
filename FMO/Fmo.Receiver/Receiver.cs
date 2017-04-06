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
using Ninject;
using System.Threading;
using Fmo.Common.Constants;
using Fmo.NYBLoader.Interfaces;
using Fmo.NYBLoader.Common;

namespace Fmo.Receiver
{
    public partial class Receiver : ServiceBase
    {
        private readonly IKernel kernal;
        private IMessageBroker<AddressLocationUSRDTO> msgUSR = default(IMessageBroker<AddressLocationUSRDTO>);
        private IMessageBroker<PostalAddressDTO> msgPAF = default(IMessageBroker<PostalAddressDTO>);
        private IHttpHandler httpHandler = default(IHttpHandler);
        public Receiver(IHttpHandler httpHandler)
        {
            kernal = new StandardKernel();
            Register(kernal);
            this.httpHandler = httpHandler;
            InitializeComponent();
        }
        protected void Register(IKernel kernel)
        {
            kernel.Bind<IMessageBroker<AddressLocationUSRDTO>>().To<MessageBroker<AddressLocationUSRDTO>>().InSingletonScope();
            kernel.Bind<IMessageBroker<PostalAddressDTO>>().To<MessageBroker<PostalAddressDTO>>().InSingletonScope();
            kernal.Bind<IHttpHandler>().To<HttpHandler>().InSingletonScope();
            msgUSR = kernel.Get<IMessageBroker<AddressLocationUSRDTO>>();
            msgPAF = kernel.Get<IMessageBroker<PostalAddressDTO>>();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        private void Start()
        {
            Thread threadAddressUSRDTO = new Thread(StartUSR);
            threadAddressUSRDTO.Start();

            Thread threadAddressPAFDTO = new Thread(StartPAF);
            threadAddressPAFDTO.Start();
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
            AddressLocationUSRDTO a = e.MessageBody;

            if (msgUSR.HasMessage(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH))
            {
                AddressLocationUSRDTO b = msgUSR.ReceiveMessage(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
            }

        }

        public void PAFMessageReceived(object sender, MessageEventArgs<PostalAddressDTO> e)
        {
            PostalAddressDTO a = e.MessageBody;

            if (msgPAF.HasMessage(Constants.QUEUE_PAF, Constants.QUEUE_PATH))
            {
                PostalAddressDTO b = msgPAF.ReceiveMessage(Constants.QUEUE_PAF, Constants.QUEUE_PATH);
                if (b != null)
                {

                }
            }

        }

        public bool SavePAFDetails(PostalAddressDTO postalAddress)
        {

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
