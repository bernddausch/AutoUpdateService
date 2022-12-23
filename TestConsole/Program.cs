using Helper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<apiresponse> job = apiclient.GetJobFromAPIAsync(dnshelper.LookupServices());
            try
            {
                 if (job.Result != null)
                {
                    Console.WriteLine(job.Result.Task);
                    int CurrentTaks = (int)job.Result.Task;
                    Console.WriteLine(job.Result.Scriptblock);
                    Thread.Sleep(2000);
                    apiclient.SendStepToAPI(dnshelper.LookupServices(), CurrentTaks);
                }

            }
            catch { }

            Console.ReadLine();
        }
    }
}
