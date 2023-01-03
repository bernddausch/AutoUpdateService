using Helper;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using System.Management.Automation.Runspaces;
using System.Diagnostics;

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
            Task<apiresponse> job = apiclient.GetJobFromAPIAsync(dnshelper.LookupServices());
            int CurrentTask = (int)job.Result.Task;
            string ScriptBlock = job.Result.Scriptblock;
            string script = $"{ScriptBlock} -Step {CurrentTask}";

            /*var appLog = new EventLog("Application");
            appLog.Source = "MySource";
            appLog.WriteEntry($"{script}");
            appLog.WriteEntry("test");*/

            // Initialize Runspace and Load Modules
            InitialSessionState initial = InitialSessionState.CreateDefault();
            string[] modules = new string[] { "SWPatchday", "SWStandardTools", "PackageManagement" };
            initial.ImportPSModule(modules);


            // Create and Open the Runspace
            Runspace MyRunSpace = RunspaceFactory.CreateRunspace(initial);
            MyRunSpace.Open();
    
            // Initial Pipeline add Commands and run it
            Pipeline pipeline = MyRunSpace.CreatePipeline();
            pipeline.Commands.AddScript(script);

            var result = pipeline.Invoke();

            MyRunSpace.Close();
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
