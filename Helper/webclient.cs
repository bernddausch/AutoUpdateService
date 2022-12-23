using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Helper
{
	public class LocalHostName
	{
		public string hostName;

		public static string GetHostName()
		{
			// Get Local FQDN
            string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
            string hostName = Dns.GetHostName();

            domainName = "." + domainName;
            if (!hostName.EndsWith(domainName))  
            {
                // if hostname does not already include domain name, add the domain name Part
                hostName += domainName;   
            }

            return hostName;
        }
    }
	public class apiresponse
	{
		public int Task { get; set; }
		public string Scriptblock { get; set; }
	}
	public class apiclient
	{
		public static async Task<apiresponse> GetJobFromAPIAsync(string url)
		{
			
            var values = new Dictionary<string, string> {
                {"Computername", LocalHostName.GetHostName() }
            };

            string json = JsonSerializer.Serialize(values);
			var data = new StringContent(json, Encoding.UTF8, "application/json");

            // Create and Initialize Client
            HttpClient WebClient = new HttpClient();
            WebClient.BaseAddress = new System.Uri("http://" + url);
            WebClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
				var response = await WebClient.PostAsync("/getjob", data);
				if (response.IsSuccessStatusCode)
				{
					var result = await response.Content.ReadAsStringAsync();
					var jobdata = JsonSerializer.Deserialize<apiresponse>(result);
					return jobdata;
				}
				else
				{
					return null;
				}
			}
            catch {
                return null;
            }


			
        }

		public static async void SendStepToAPI(string url, int step)
		{
            // Create and Initialize Client
            HttpClient WebClient = new HttpClient();
            WebClient.BaseAddress = new System.Uri("http://" + url);
            WebClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            string json = $"{{\"Computername\": \"{LocalHostName.GetHostName()}\", \"SuccessStep\": {step} }}";

            var data = new StringContent(json, Encoding.UTF8, "application/json");
            try
            {
                var response = await WebClient.PostAsync("/jobdone", data);
                if (response.IsSuccessStatusCode)
                {
                }
            }
            catch
            {
            }
        }
	}
}
