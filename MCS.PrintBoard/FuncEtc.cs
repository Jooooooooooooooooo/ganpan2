using System;
using System.Configuration;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace MCS.PrintBoard;

public class FuncEtc
{
	private static FuncEtc instance = null;

	private static string SecurityKey = "WID";

	public static FuncEtc Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new FuncEtc();
			}
			return instance;
		}
	}

	private FuncEtc()
	{
	}

	public string EncryptString(string toEncrypt, bool useHashing)
	{
		byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);
		AppSettingsReader settingsReader = new AppSettingsReader();
		string key = SecurityKey;
		byte[] keyArray;
		if (useHashing)
		{
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
			hashmd5.Clear();
		}
		else
		{
			keyArray = Encoding.UTF8.GetBytes(key);
		}
		TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
		tdes.Key = keyArray;
		tdes.Mode = CipherMode.ECB;
		tdes.Padding = PaddingMode.PKCS7;
		ICryptoTransform cTransform = tdes.CreateEncryptor();
		byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
		tdes.Clear();
		return Convert.ToBase64String(resultArray, 0, resultArray.Length);
	}

	public string DecryptString(string cipherString, bool useHashing)
	{
		byte[] toEncryptArray = Convert.FromBase64String(cipherString);
		AppSettingsReader settingsReader = new AppSettingsReader();
		string key = SecurityKey;
		byte[] keyArray;
		if (useHashing)
		{
			MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
			keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
			hashmd5.Clear();
		}
		else
		{
			keyArray = Encoding.UTF8.GetBytes(key);
		}
		TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
		tdes.Key = keyArray;
		tdes.Mode = CipherMode.ECB;
		tdes.Padding = PaddingMode.PKCS7;
		ICryptoTransform cTransform = tdes.CreateDecryptor();
		byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
		tdes.Clear();
		return Encoding.UTF8.GetString(resultArray);
	}

	public static string TableToXmlString(DataTable obj)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("<NewDataSet>" + Environment.NewLine);
		builder.Append(TableToString(obj));
		builder.Append("</NewDataSet>" + Environment.NewLine);
		return builder.ToString();
	}

	public static string DataSetToXmlString(DataSet obj)
	{
		StringBuilder builder = new StringBuilder();
		builder.Append("<NewDataSet>" + Environment.NewLine);
		for (int i = 0; i < obj.Tables.Count; i++)
		{
			builder.Append(TableToString(obj.Tables[i]));
		}
		builder.Append("</NewDataSet>" + Environment.NewLine);
		return builder.ToString();
	}

	private static string TableToString(DataTable obj)
	{
		StringBuilder builder = new StringBuilder();
		for (int i = 0; i < obj.Rows.Count; i++)
		{
			builder.Append("<" + obj.TableName + ">" + Environment.NewLine);
			for (int j = 0; j < obj.Columns.Count; j++)
			{
				builder.Append("\t<" + obj.Columns[j].ColumnName + ">" + obj.Rows[i][j].ToString() + "</" + obj.Columns[j].ColumnName + ">" + Environment.NewLine);
			}
			builder.Append("</" + obj.TableName + ">" + Environment.NewLine);
		}
		return builder.ToString();
	}

	public static void SelectValueChangeIndex(ComboBox combobox, object value)
	{
		if (combobox.Items.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < combobox.Items.Count; i++)
		{
			object item = combobox.Items[i];
			object thisValue = (item as DataRowView)[combobox.ValueMember];
			if (thisValue != null && thisValue.Equals(value))
			{
				combobox.SelectedIndex = i;
				return;
			}
		}
		combobox.SelectedIndex = -1;
	}

	public static void SelectValueChangeIndex1(ComboBox combobox, object value)
	{
		if (combobox.Items.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < combobox.Items.Count; i++)
		{
			object item = combobox.Items[i];
			object thisValue = item.GetType().GetProperty(combobox.ValueMember).GetValue(item);
			if (thisValue != null && thisValue.Equals(value))
			{
				combobox.SelectedIndex = i;
				return;
			}
		}
		combobox.SelectedIndex = -1;
	}
}
