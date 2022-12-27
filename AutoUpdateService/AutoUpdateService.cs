using Helper;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace AutoUpdateService
{
    public partial class AutoUpdateService : ServiceBase
    {
        public AutoUpdateService()
        {
            InitializeComponent();
            while (true)
            {


                // Get Job from API
                Task<apiresponse> job = apiclient.GetJobFromAPIAsync(dnshelper.LookupServices());
                try
                {
                    if (job.Result != null)
                    {
                        // Convert Step to Int, API needs an Integer when send the SuccessStep
                        int CurrentTaks = (int)job.Result.Task;

                        // Wait some Time, API needs it bevor Sending the CurrentStep
                        Thread.Sleep(2000);

                        // Send Scriptblock to Powershell Worker
                        psworker worker = new psworker();
                        worker.Connect();
                        worker.RunScriptBlock(job.Result.Scriptblock);
                        worker.Close();

                        // Send Success Step to API
                        apiclient.SendStepToAPI(dnshelper.LookupServices(), CurrentTaks);
                    }

                }
                catch { }

                Thread.Sleep(60000);
            }
            
        }

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }
    }
}
