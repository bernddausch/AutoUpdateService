using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Helper
{
    public class psworker
    {
        public static Runspace MyRunSpace = RunspaceFactory.CreateRunspace();

        public void Connect()
        {
            MyRunSpace.Open();
        }

        public string RunScriptBlock(string ScriptBlock)
        {
            Pipeline pipeline = MyRunSpace.CreatePipeline();
            pipeline.Commands.AddScript("Set-ExecutionPolicy unrestricted -force -confirm:$false");
            pipeline.Commands.AddScript("Import-Module -Name SWPatchDay");
            pipeline.Commands.AddScript(ScriptBlock);

                var result = pipeline.Invoke();

                foreach (PSObject Entry in result)
                {
                    if (Entry != null)
                    {
                        return Entry.Properties["Name"].Value.ToString();
                    }
                }
            
            return "Not Found";
        }

        public void Close() { }
    
    }
}
