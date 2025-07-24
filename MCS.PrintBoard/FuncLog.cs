using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace MCS.PrintBoard;

public class FuncLog
{
	public string fPATH = Application.StartupPath + "\\Log\\";

	private static FuncLog instance = null;

	private static object thisLock = new object();

	public static FuncLog Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new FuncLog();
			}
			return instance;
		}
	}

	private FuncLog()
	{
		if (Application.StartupPath == "c:\\windows\\system32\\inetsrv")
		{
			fPATH = "C:\\LGE\\WISS\\LOG\\";
		}
	}

	private string FileExistCheck()
	{
		string sYYYY = DateTime.Today.Year.ToString();
		string sMM = DateTime.Today.Month.ToString();
		string sDD = DateTime.Today.Day.ToString();
		sMM = sMM.PadLeft(2, '0');
		sDD = sDD.PadLeft(2, '0');
		string sDir = fPATH + "\\";
		sDir += sYYYY;
		sDir += sMM;
		string sFileName = sDir + "\\";
		sFileName += sDD;
		sFileName += ".TXT";
		FileInfo f = new FileInfo(sFileName);
		if (f.Exists)
		{
			return sFileName;
		}
		DirectoryInfo D = new DirectoryInfo(sDir);
		if (!D.Exists)
		{
			D.Create();
		}
		FileStream fs = f.OpenWrite();
		fs.Close();
		return sFileName;
	}

	public void LogSave(string sMsg)
	{
		try
		{
			string sFileName = FileExistCheck();
			lock (thisLock)
			{
				StreamWriter sw = File.AppendText(sFileName);
				string sToday = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
				sw.WriteLine("*" + sToday + " : " + sMsg);
				sw.Close();
			}
		}
		catch (Exception ex)
		{
			Instance.LogSave(Environment.NewLine);
			Instance.LogSave("Message : " + ex.Message);
			Instance.LogSave("Exception : " + ex.ToString());
			Instance.LogSave("StackTrace : " + ex.StackTrace);
			Instance.LogSave("Data : " + ex.Data.ToString());
			Instance.LogSave("TargetSite : " + ex.TargetSite.ToString());
		}
	}

	public void LogSave(string FunctionName, Exception ex)
	{
		try
		{
			string sFileName = FileExistCheck();
			lock (thisLock)
			{
				StreamWriter sw = File.AppendText(sFileName);
				string sToday = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
				if (ex != null)
				{
					sw.WriteLine("*" + sToday + " : " + FunctionName + ":" + ex.Message);
				}
				else
				{
					sw.WriteLine("*" + sToday + " : " + FunctionName);
				}
				sw.Close();
			}
		}
		catch (Exception ex2)
		{
			Instance.LogSave(Environment.NewLine);
			Instance.LogSave("Message : " + ex2.Message);
			Instance.LogSave("Exception : " + ex2.ToString());
			Instance.LogSave("StackTrace : " + ex2.StackTrace);
			Instance.LogSave("Data : " + ex2.Data.ToString());
			Instance.LogSave("TargetSite : " + ex2.TargetSite.ToString());
		}
	}
}
