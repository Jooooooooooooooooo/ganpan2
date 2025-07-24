using System;
using System.Windows.Forms;
using MCS.PrintBoard.PrintBoard;

namespace MCS.PrintBoard;

internal static class Program
{
	[STAThread]
	private static void Main()
	{
		Application.EnableVisualStyles();
		Application.SetCompatibleTextRenderingDefault(defaultValue: false);
		Application.Run(new frmMain());
	}
}
