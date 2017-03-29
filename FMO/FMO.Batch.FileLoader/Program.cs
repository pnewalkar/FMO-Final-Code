﻿namespace FMO.Batch.FileLoader
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
            FileLoader myService = new FileLoader();
            myService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else

                //In Release this section is used. This is the "normal" way.
               ServiceBase[] ServicesToRun;
                        ServicesToRun = new ServiceBase[]
                        {
                            new FileLoaderService()
                        };
                        ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}