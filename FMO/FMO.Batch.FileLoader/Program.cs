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
#if DEBUG

            // While debugging this section is used.
            using (FileLoader myService = new FileLoader())
            {
                myService.OnDebug();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }

#else

                //In Release this section is used. This is the "normal" way.
               ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new FileLoader()
                        };
                        ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}