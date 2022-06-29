using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace Injector
{
    class Program
    {
        private readonly static string @namespace = "SFTGTrainer";
        private readonly static string @class = "Loader";
        private readonly static string @method = "Load";

        static void Main(string[] args)
        {
            if (Process.GetProcessesByName("StickFight").Length == 0)
            {
                ProcessStartInfo info = new ProcessStartInfo("steam://rungameid/674940");
                info.UseShellExecute = true;                
                Process.Start(info);

                bool gameRunning = false;
                while (!gameRunning)
                {
                    Thread.Sleep(200);
                    Process[] pname = Process.GetProcessesByName("StickFight");
                    Console.WriteLine("Waiting for stick fight...");
                    if (pname.Length > 0)
                        gameRunning = true;
                }
            }

            Console.WriteLine("Found !");

            Process[] mname = Process.GetProcessesByName("StickFight");
            if (mname.Length <= 0)
            {
                Console.WriteLine("StickFight is not running !");
                return;
            }

            Console.WriteLine("Waiting " + 5 + " seconds...");
            Thread.Sleep(5 * 1000);

            Console.WriteLine("Preparing for injection...");

            try
            {
                byte[] assembly = GetDLLBytes();
                Console.WriteLine("Injecting...");
                SharpMonoInjector.Injector injector = new SharpMonoInjector.Injector("StickFight");
                injector.Inject(assembly, @namespace, @class, @method);
                Console.WriteLine("Injected !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("The Injection Failed: " + ex.Message + ", Stacktrace: " + ex.StackTrace);
            }
        }

        private static byte[] GetDLLBytes()
        {
            if (IsDebugRelease())
            {
                return File.ReadAllBytes("C:\\Users\\thoma\\Nextcloud\\SFTGTrainer\\SFTGTrainer\\bin\\Debug\\SFTGTrainer.dll");
            }
            else
            {
                WebClient wc = new WebClient();
                return wc.DownloadData("https://resources.redboxing.fr/stickFight/SFTGTrainer.dll");
            }
        }

        private static bool IsDebugRelease()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
    }
}
