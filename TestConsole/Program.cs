using Helper;
using System;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Task<apiresponse> job = apiclient.GetJobFromAPIAsync(dnshelper.LookupServices());
            Console.WriteLine(job.Result.Task);
            Console.WriteLine(job.Result.Scriptblock);
            Console.ReadLine();
        }
    }
}
