using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace SFTGTrainer
{
    class LogUtils
    {
		public static void info(string text)
		{
			Console.WriteLine("[" + DateTime.Now.ToString() + "] [INFO] " + text);
		}

		public static void debug(string text)
		{
			Console.WriteLine("[" + DateTime.Now.ToString() + "] [DEBUG] " + text);
		}

		public static void warn(string text)
		{
			Console.WriteLine("[" + DateTime.Now.ToString() + "] [WARN] " + text);
		}

		public static void error(string text)
		{
			Console.WriteLine("[" + DateTime.Now.ToString() + "] [ERROR] " + text);
		}
	}
}
