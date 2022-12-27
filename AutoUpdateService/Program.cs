using System.ServiceProcess;

namespace AutoUpdateService
{
    internal static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AutoUpdateService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
