using System.ServiceProcess;

namespace Fmo.Batch.FileLoader
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            //ServiceBase[] servicesToRun = new ServiceBase[] { new FileLoader() };
            //ServiceBase.Run(servicesToRun);
            using (FileLoader myService = new FileLoader())
            {
                myService.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
        }
    }
}