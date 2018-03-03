using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
namespace LFP_P1
{
	static class Program
	{
		/// <summary>
		/// Application Entry Point.
		/// </summary>
		[System.STAThreadAttribute()]
		[System.Diagnostics.DebuggerNonUserCodeAttribute()]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new WindowsFormsApp1.Main());
		}
	}
}
