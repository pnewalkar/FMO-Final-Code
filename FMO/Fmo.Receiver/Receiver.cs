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

namespace Fmo.Receiver
{
    public partial class Receiver : ServiceBase
    {
        private readonly IKernel kernal;
        private IMessageBroker<AddressLocationUSRDTO> msgUSR = default(IMessageBroker<AddressLocationUSRDTO>);
        public Receiver()
        {
            kernal = new StandardKernel();
            Register(kernal);
            InitializeComponent();
        }
        protected void Register(IKernel kernel)
        {
            kernel.Bind<IMessageBroker<AddressLocationUSRDTO>>().To<MessageBroker<AddressLocationUSRDTO>>().InSingletonScope();
            msgUSR = kernel.Get<IMessageBroker<AddressLocationUSRDTO>>();
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        private void Start()
        {
            Thread threadAddressUSRDTO = new Thread(StartUSR);
            threadAddressUSRDTO.Start();
        }

        public void StartUSR()
        {
            msgUSR.Start(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH, USRMessageReceived);
        }

        public void USRMessageReceived(object sender, MessageEventArgs<AddressLocationUSRDTO> e)
        {
            AddressLocationUSRDTO a = e.MessageBody;

            if (msgUSR.HasMessage(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH))
            {
                AddressLocationUSRDTO b = msgUSR.ReceiveMessage(Constants.QUEUE_THIRD_PARTY, Constants.QUEUE_PATH);
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
