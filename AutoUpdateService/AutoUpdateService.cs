using Helper;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using System.Management.Automation.Runspaces;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Linq;
using System.Management.Automation;

namespace AutoUpdateService
{
    public partial class AutoUpdateService : ServiceBase
    {
        public AutoUpdateService()
        {
            InitializeComponent();            
        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            // Get Job from API
            /*Task<apiresponse> job = apiclient.GetJobFromAPIAsync(dnshelper.LookupServices());
            int CurrentTask = (int)job.Result.Task;
            string ScriptBlock = job.Result.Scriptblock;
            */
            InitialSessionState initial = InitialSessionState.CreateDefault();
            string[] modules = new string[] { "SWPatchday", "SWStandardTools", "PackageManagement" };
            initial.ImportPSModule(modules);


            //string script = $"{ScriptBlock} -Step {CurrentTask}";
            
            var appLog = new EventLog("Application");
            appLog.Source = "MySource";
            //appLog.WriteEntry($"{script}");
            appLog.WriteEntry("test");
            Runspace MyRunSpace = RunspaceFactory.CreateRunspace(initial);
            MyRunSpace.Open();
    
            Pipeline pipeline = MyRunSpace.CreatePipeline();
            pipeline.Commands.AddScript("Install-PackageProvider -name chocolatey -force -confirm:$false");
            pipeline.Commands.AddScript("Register-PackageSource -Name SchuWa-Repo -ProviderName Chocolatey -Location http://repo.servicezone.local/nuget/nuget -Trusted -force -confirm:$false");

            pipeline.Commands.AddScript(@"get-packageSource  | out-file c:\\test2.txt");
            pipeline.Commands.AddScript(@"Start-UpdSoftwareUpdate");

            //pipeline.Commands.AddScript(@"$PSVersionTable | out-file c:\\test2.txt");
            //pipeline.Commands.AddScript(script);

            var result = pipeline.Invoke();

               

            
            
        }

        protected override void OnStart(string[] args)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
