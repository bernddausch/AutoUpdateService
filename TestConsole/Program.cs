using Helper;
using System;
using System.IO.Pipes;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading.Tasks;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Task<string> job = apiclient.GetJobFromAPIAsync(dnshelper.LookupServices());
            Console.WriteLine(job.Result);
            Console.ReadLine();
        }
    }
}
