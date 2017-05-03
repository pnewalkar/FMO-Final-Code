using System.ServiceProcess;

namespace Fmo.Batch.FileLoader
{
    /// <summary>
    /// Entry Point for FileLoader service
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase[] servicesToRun = new ServiceBase[] { new FileLoader() };
            ServiceBase.Run(servicesToRun);
        }
    }
}