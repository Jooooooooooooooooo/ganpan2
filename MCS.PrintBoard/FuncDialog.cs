using System;
using System.Reflection;
using System.Windows.Forms;

namespace MCS.PrintBoard;

public class FuncDialog
{
	public static DialogResult GetMessageGlobal(string sMsgCode, string sTitle, int iBtnType, int iBtnIcon)
	{
		return GetMessageGlobal(sMsgCode, sTitle, iBtnType, iBtnIcon, "", "");
	}

	public static DialogResult GetMessageGlobal(string sMsgCode, string sTitle, int iBtnType, int iBtnIcon, string sParam)
	{
		return GetMessageGlobal(sMsgCode, sTitle, iBtnType, iBtnIcon, sParam, "");
	}

	public static DialogResult GetMessageGlobal(string sMsgCode, string sTitle, int iBtnType, int iBtnIcon, string sParam, string defaultButton)
	{
		string strTitle = null;
		string setMessage = null;
		DialogResult dResult = DialogResult.None;
		try
		{
			setMessage = string.Empty;
			strTitle = sTitle + " [" + sMsgCode + "] ";
			MessageBoxButtons msgBtn = MessageBoxButtons.OK;
			msgBtn = iBtnType switch
			{
				1 => MessageBoxButtons.OK, 
				2 => MessageBoxButtons.OKCancel, 
				3 => MessageBoxButtons.AbortRetryIgnore, 
				4 => MessageBoxButtons.YesNoCancel, 
				5 => MessageBoxButtons.YesNo, 
				6 => MessageBoxButtons.RetryCancel, 
				_ => MessageBoxButtons.OK, 
			};
			MessageBoxIcon msgIcon = MessageBoxIcon.None;
			msgIcon = iBtnIcon switch
			{
				1 => MessageBoxIcon.None, 
				2 => MessageBoxIcon.Hand, 
				3 => MessageBoxIcon.Question, 
				4 => MessageBoxIcon.Exclamation, 
				5 => MessageBoxIcon.Asterisk, 
				6 => MessageBoxIcon.Hand, 
				7 => MessageBoxIcon.Hand, 
				8 => MessageBoxIcon.Exclamation, 
				9 => MessageBoxIcon.Asterisk, 
				_ => MessageBoxIcon.Asterisk, 
			};
			MessageBoxDefaultButton msgButton = MessageBoxDefaultButton.Button1;
			msgButton = defaultButton switch
			{
				"1" => MessageBoxDefaultButton.Button1, 
				"2" => MessageBoxDefaultButton.Button2, 
				"3" => MessageBoxDefaultButton.Button3, 
				_ => MessageBoxDefaultButton.Button1, 
			};
			if (sParam != null && !(sParam == ""))
			{
				string[] strParam = sParam.Split('|');
				if (setMessage == string.Empty)
				{
					setMessage = strParam[0].ToString();
				}
			}
			dResult = MessageBox.Show(setMessage.Replace("\\n", "\r\n"), strTitle, msgBtn, msgIcon, msgButton);
			return dResult;
		}
		catch (Exception ex)
		{
			DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
			return dResult;
		}
	}

	public static void DspErrMsg(Exception ex, string sMethodName)
	{
		try
		{
			int idx = ex.Message.IndexOf(']');
			if (idx == -1)
			{
				string strTitle = ex.TargetSite.ToString();
				string strMessage = ex.Message;
			}
			else
			{
				string strTitle = ex.Message.Substring(1, idx - 1);
				string strMessage = ex.Message.Substring(idx + 1);
			}
			FuncLog.Instance.LogSave(Environment.NewLine);
			FuncLog.Instance.LogSave("MethodName : " + sMethodName);
			FuncLog.Instance.LogSave("Message : " + ex.Message);
			FuncLog.Instance.LogSave("Exception : " + ex.ToString());
			FuncLog.Instance.LogSave("StackTrace : " + ex.StackTrace);
			FuncLog.Instance.LogSave("Data : " + ex.Data.ToString());
			FuncLog.Instance.LogSave("TargetSite : " + ex.TargetSite.ToString());
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message);
		}
	}
}
