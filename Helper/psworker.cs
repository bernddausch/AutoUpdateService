using System.Management.Automation.Runspaces;

namespace Helper
{
    public class psworker
    {
        public string RunScriptBlock(string Command)
        {
            Runspace MyRunSpace = RunspaceFactory.CreateRunspace();
            MyRunSpace.Open();
            string script = $"Import-Module -Name SWPatchDay;Start-Sleep -seconds 10;{Command}";
            Pipeline pipeline = MyRunSpace.CreatePipeline();
            pipeline.Commands.AddScript(script);
            var result = pipeline.Invoke();
            var pserrors = pipeline.Error.ReadToEnd();
            MyRunSpace.Close();
            return "";
        }

    
    }
}
