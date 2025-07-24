using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Win32;
using Oracle.ManagedDataAccess.Client;

namespace MCS.PrintBoard;

public class DBConn
{
	public static string ConnectGMES = string.Empty;

	private static OracleConnection connection = null;

	private static OracleCommand command = null;

	private static OracleDataAdapter dataAdapter = null;

	public static DataSet dataSet = null;

	public static string strConnection = null;

	private static string GetConnectionString()
	{
		string strCon = null;
		try
		{
			strCon = strConnection;
		}
		catch (Exception e)
		{
			throw e;
		}
		finally
		{
		}
		return strCon;
	}

	private static void GetConnection()
	{
		try
		{
			connection = new OracleConnection(GetConnectionString());
			connection.Open();
		}
		catch (Exception e)
		{
			throw e;
		}
		finally
		{
		}
	}

	private static void GetCommand()
	{
		try
		{
			GetConnection();
			command = connection.CreateCommand();
		}
		catch (Exception e)
		{
			throw e;
		}
		finally
		{
		}
	}

	private static void DBClose()
	{
		try
		{
			if (connection != null)
			{
				connection.Close();
				connection.Dispose();
				connection = null;
			}
			if (command != null)
			{
				command.Dispose();
				command = null;
			}
			if (dataAdapter != null)
			{
				dataAdapter.Dispose();
				dataAdapter = null;
			}
		}
		catch (Exception e)
		{
			MessageBox.Show(e.Message);
		}
		finally
		{
		}
	}

	public static DataSet ExecuteDBQuery(string sConnectString, string sSql)
	{
		try
		{
			strConnection = sConnectString;
			GetCommand();
			command.CommandTimeout = 0;
			command.CommandType = CommandType.Text;
			command.CommandText = sSql;
			dataAdapter = new OracleDataAdapter();
			dataSet = new DataSet();
			dataAdapter.SelectCommand = command;
			dataAdapter.Fill(dataSet);
			return dataSet;
		}
		catch
		{
			return dataSet;
		}
		finally
		{
			DBClose();
		}
	}

	public static void ExecuteNonDBQuery(string sConnectString, string sSql)
	{
		DataSet ds = null;
		strConnection = sConnectString;
		OracleConnection oracleConnect = new OracleConnection(strConnection);
		try
		{
			oracleConnect.Open();
		}
		catch (Exception Ex)
		{
			MessageBox.Show(Ex.ToString());
			return;
		}
		try
		{
			OracleCommand command = oracleConnect.CreateCommand();
			command.CommandTimeout = 0;
			command.CommandText = sSql;
			OracleDataAdapter adapter = new OracleDataAdapter();
			ds = new DataSet();
			adapter.SelectCommand = command;
			oracleConnect.Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	public static void SetOraNLS_LANG()
	{
		try
		{
			RegistryKey OurKey = Registry.LocalMachine;
			OurKey = OurKey.OpenSubKey("SOFTWARE", writable: true);
			OurKey = OurKey.OpenSubKey("ORACLE", writable: true);
			if (OurKey == null)
			{
				RegistryKey CreateKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\ORACLE");
				CreateKey.SetValue("NLS_LANG", "KOREAN_KOREA.KO16MSWIN949");
			}
			else if (!((string)OurKey.GetValue("NLS_LANG") == "KOREAN_KOREA.KO16MSWIN949"))
			{
				OurKey.SetValue("NLS_LANG", "KOREAN_KOREA.KO16MSWIN949");
			}
		}
		catch (SystemException ex)
		{
			throw ex;
		}
	}
}
