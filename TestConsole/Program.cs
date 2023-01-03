using Helper;
using System;
using System.IO;
using System.IO.Pipes;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Helper.ErrorHandler MyErrors = new Helper.ErrorHandler("SchuWa", "AutoUpdateAgent");
            MyErrors.LogError("asdfasfd", ErrorType.Warning);
            MyErrors.LogError("ig", ErrorType.Information);
        }
    }
}
