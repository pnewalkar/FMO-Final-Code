using System.ServiceProcess;

namespace Fmo.Receiver
{
    /// <summary>
    /// Entry point for receiver service
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] servicesToRun = new ServiceBase[] { new Receiver() };
            ServiceBase.Run(servicesToRun);
        }
    }
}
