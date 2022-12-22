using DnsClient;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;


namespace Helper
{
    public class dnshelper
    {
        public static string GetDnsSuffix()
        {
            string domain = Registry.GetValue("HKEY_LOCAL_MACHINE\\System\\CurrentControlSet\\Services\\Tcpip\\Parameters", "Domain", -1).ToString();

            if (domain == "sw750.local")
            {
                NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
                List<string> dnssuffix = new List<string>();
                foreach (NetworkInterface adapter in adapters)
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    dnssuffix.Add(properties.DnsSuffix.ToString());
                }
                return dnssuffix.First();
            }
            else
            {
                return domain;
            }
        }

        public static string LookupServices()
        {
            string SrvAddress = "swdvupdate._tcp." + GetDnsSuffix();
            var lookup = new LookupClient();

            try
            {
                var result = lookup.Query(SrvAddress, QueryType.SRV).Answers.SrvRecords().ToList();
                string hostname = result[0].Target;
                string Uri = hostname.Remove(hostname.Length-1) + ':' + result[0].Port;
                return Uri;
            }
            catch
            {
                return null;
            }
        }
    }
}
