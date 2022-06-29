using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace SFTGTrainer
{
	public static class AllocConsoleHandler
	{
		[DllImport("Kernel32.dll")]
		private static extern bool AllocConsole();

		public static void Open()
		{
			AllocConsoleHandler.AllocConsole();
			Console.SetOut(new StreamWriter(Console.OpenStandardOutput())
			{
				AutoFlush = true
			});
			Console.SetIn(new StreamReader(Console.OpenStandardInput()));
			Application.logMessageReceivedThreaded += delegate (string condition, string stackTrace, LogType type)
			{
				Console.WriteLine(condition + " " + stackTrace);
			};
		}
	}
}
