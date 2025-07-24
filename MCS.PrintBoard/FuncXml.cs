using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;

namespace MCS.PrintBoard;

public class FuncXml
{
	public enum XmlFileType
	{
		LoginInfo,
		ServerInfo,
		RegularExpressonInfo
	}

	private readonly string LoginInfoFilePath = Environment.CurrentDirectory + "\\MCS.PrintBoard.LoginInformation.xml";

	private readonly string ServerInfoFilePath = Environment.CurrentDirectory + "\\MCS.PrintBoard.ServerInfo.xml";

	private static FuncXml instance = null;

	public static FuncXml Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new FuncXml();
			}
			return instance;
		}
	}

	private FuncXml()
	{
	}

	public string ReadXml(string sKey, string sName, string sDefalut)
	{
		return ReadXml(sKey, sName, sDefalut, XmlFileType.LoginInfo, isDecrypt: false, displayErrorMsg: true);
	}

	public string ReadXml(string sKey, string sName, string sDefalut, XmlFileType xmlFileType, bool isDecrypt, bool displayErrorMsg)
	{
		string sValue = sDefalut;
		bool bDecrypt = isDecrypt;
		string strFilePath = string.Empty;
		try
		{
			strFilePath = xmlFileType switch
			{
				XmlFileType.LoginInfo => LoginInfoFilePath, 
				XmlFileType.ServerInfo => ServerInfoFilePath, 
				_ => LoginInfoFilePath, 
			};
			if (!File.Exists(strFilePath))
			{
				if (displayErrorMsg)
				{
					MessageBox.Show(strFilePath + " File is not exists.");
					Environment.Exit(0);
				}
				else
				{
					FuncLog.Instance.LogSave(Environment.NewLine);
					FuncLog.Instance.LogSave(strFilePath + " File is not exists.");
				}
			}
			else
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(strFilePath);
				XmlNode root = doc.DocumentElement;
				if (root.HasChildNodes)
				{
					IEnumerator ienum = root.GetEnumerator();
					XmlNode node2nd = root.SelectSingleNode(sKey);
					if (node2nd != null && node2nd.HasChildNodes && node2nd.SelectSingleNode(sName) != null)
					{
						if (bDecrypt)
						{
							sValue = FuncEtc.Instance.DecryptString(node2nd.SelectSingleNode(sName).InnerText.Trim(), useHashing: true);
						}
						else
						{
							sValue = node2nd.SelectSingleNode(sName).InnerText.Trim();
							if (string.IsNullOrWhiteSpace(sValue))
							{
								sValue = sDefalut;
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			if (displayErrorMsg)
			{
				FuncLog.Instance.LogSave(Environment.NewLine);
				FuncLog.Instance.LogSave("FuncXml.Instance.ReadXml() : Not found name [" + sName + "] of key [" + sKey + "] in " + strFilePath);
				FuncDialog.DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
				return sDefalut;
			}
			FuncLog.Instance.LogSave(Environment.NewLine);
			FuncLog.Instance.LogSave("Message : " + ex.Message);
			FuncLog.Instance.LogSave("Exception : " + ex.ToString());
			FuncLog.Instance.LogSave("StackTrace : " + ex.StackTrace);
			FuncLog.Instance.LogSave("Data : " + ex.Data.ToString());
			FuncLog.Instance.LogSave("TargetSite : " + ex.TargetSite.ToString());
			throw ex;
		}
		return sValue;
	}

	public string[] ReadMultiXml(string sKey, string sName, XmlFileType xmlFileType, bool isDecrypt)
	{
		string[] sValue = null;
		bool bDecrypt = isDecrypt;
		try
		{
			string strFilePath = xmlFileType switch
			{
				XmlFileType.LoginInfo => LoginInfoFilePath, 
				XmlFileType.ServerInfo => ServerInfoFilePath, 
				_ => LoginInfoFilePath, 
			};
			if (!File.Exists(strFilePath))
			{
				Environment.Exit(0);
			}
			else
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(strFilePath);
				XmlNode root = doc.DocumentElement;
				if (root.HasChildNodes)
				{
					IEnumerator ienum = root.GetEnumerator();
					XmlNode node2nd = root.SelectSingleNode(sKey);
					if (node2nd.HasChildNodes)
					{
						sValue = new string[node2nd.SelectNodes(sName).Count];
						for (int i = 0; i < node2nd.SelectNodes(sName).Count; i++)
						{
							if (bDecrypt)
							{
								sValue[i] = FuncEtc.Instance.DecryptString(node2nd.SelectNodes(sName)[i].InnerText.Trim(), useHashing: true);
							}
							else
							{
								sValue[i] = node2nd.SelectNodes(sName)[i].InnerText;
							}
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			FuncLog.Instance.LogSave("FuncEtc.ReadMultiXml()", ex);
			return null;
		}
		return sValue;
	}

	public void WriteXml(string sKey, string sName, string sData)
	{
		WriteXml(sKey, sName, sData, XmlFileType.LoginInfo);
	}

	public void WriteXml(string sKey, string sName, string sData, XmlFileType xmlFileType)
	{
		try
		{
			string strFilePath = xmlFileType switch
			{
				XmlFileType.LoginInfo => LoginInfoFilePath, 
				XmlFileType.ServerInfo => ServerInfoFilePath, 
				_ => LoginInfoFilePath, 
			};
			if (!File.Exists(strFilePath))
			{
				return;
			}
			XmlDocument doc = new XmlDocument();
			doc.Load(strFilePath);
			XmlNode root = doc.DocumentElement;
			if (root == null)
			{
				XmlElement elementRoot = doc.CreateElement("ROOT");
				doc.AppendChild(elementRoot);
			}
			if (!root.HasChildNodes || root.SelectSingleNode(sKey) == null)
			{
				XmlElement element2nd = doc.CreateElement(sKey);
				root.AppendChild(element2nd);
			}
			if (root.HasChildNodes)
			{
				XmlNode node2nd = root.SelectSingleNode(sKey);
				if (node2nd == null || node2nd.SelectSingleNode(sName) == null)
				{
					XmlElement element3rd = doc.CreateElement(sName);
					element3rd.InnerText = sData;
					node2nd.AppendChild(element3rd);
				}
				else if (node2nd.HasChildNodes)
				{
					XmlNode node3rd = node2nd.SelectSingleNode(sName);
					node3rd.InnerText = sData;
				}
			}
			doc.Save(strFilePath);
		}
		catch (Exception ex)
		{
			FuncLog.Instance.LogSave("FuncEtc.WriteXml()", ex);
			sData = ex.Message;
		}
	}
}
