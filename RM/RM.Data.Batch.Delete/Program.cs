using Ninject;
using RM.CommonLibrary.ConfigurationMiddleware;
using RM.CommonLibrary.Interfaces;
using RM.CommonLibrary.LoggingMiddleware;
using RM.DataManagement.Batch.Delete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataManagement.Batch.Delete
{
    public class Program
    {
        static void Main(string[] args)
        {
            IKernel kernel = new StandardKernel(new StartUp());
            IHttpHandler httpHandler = kernel.Get<IHttpHandler>();
            IConfigurationHelper configurationHelper = kernel.Get<IConfigurationHelper>();
            ILoggingHelper loggingHelper = kernel.Get<ILoggingHelper>();
            DataBatchDelete batchDelete = new DataBatchDelete(httpHandler, configurationHelper, loggingHelper);
            batchDelete.BatchDelete();
        }
    }
}
