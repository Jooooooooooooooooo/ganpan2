using System;
using System.IO.Ports;
using System.Reflection;
using System.Text;
using System.Threading;

namespace MCS.PrintBoard;

public class FuncPrintCOM
{
	private SerialArg initArg = null;

	private SerialPort serial = null;

	private string sPort = "COM1";

	public SerialArg InitArg
	{
		get
		{
			return initArg;
		}
		set
		{
			initArg = value;
		}
	}

	public bool IsOpen => serial.IsOpen;

	public FuncPrintCOM()
	{
		serial = new SerialPort();
	}

	public FuncPrintCOM(SerialArg aSerialArg)
		: this()
	{
		initArg = aSerialArg;
	}

	public bool Open()
	{
		return Open(isDataReceived: true);
	}

	public bool Open(bool isDataReceived)
	{
		try
		{
			if (isDataReceived)
			{
				serial.DataReceived += OnDataReceivedEvent;
			}
			if (initArg == null)
			{
				if (ComPortSetting())
				{
					return true;
				}
				return false;
			}
			if (ComPortSetting_Arg())
			{
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			FuncDialog.DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
			return false;
		}
	}

	public void Close()
	{
		try
		{
			if (serial != null && serial.IsOpen)
			{
				serial.Close();
			}
		}
		catch (InvalidOperationException ex2)
		{
			FuncLog.Instance.LogSave(ex2.Message);
			FuncLog.Instance.LogSave(ex2.StackTrace);
			FuncLog.Instance.LogSave(ex2.TargetSite.ToString());
			FuncLog.Instance.LogSave(ex2.ToString());
			FuncLog.Instance.LogSave(MethodBase.GetCurrentMethod().Name);
			FuncDialog.DspErrMsg(ex2, MethodBase.GetCurrentMethod().Name);
		}
		catch (Exception ex)
		{
			FuncLog.Instance.LogSave(ex.Message);
			FuncLog.Instance.LogSave(ex.StackTrace);
			FuncLog.Instance.LogSave(ex.TargetSite.ToString());
			FuncLog.Instance.LogSave(ex.ToString());
			FuncLog.Instance.LogSave(MethodBase.GetCurrentMethod().Name);
			FuncDialog.DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
		}
	}

	private bool ComPortSetting()
	{
		try
		{
			if (serial != null)
			{
				serial.Handshake = Handshake.None;
				serial.PortName = sPort;
				serial.BaudRate = 9600;
				serial.Parity = Parity.None;
				serial.DataBits = 8;
				serial.StopBits = StopBits.One;
				serial.Encoding = Encoding.GetEncoding("ks_c_5601-1987");
				serial.Open();
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			FuncDialog.DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
			return false;
		}
	}

	private bool ComPortSetting_Arg()
	{
		try
		{
			if (serial != null)
			{
				serial.PortName = initArg.Port;
				serial.BaudRate = initArg.BaudRate;
				serial.Parity = initArg.Parity;
				serial.DataBits = initArg.DataBits;
				serial.StopBits = initArg.Stopbits;
				serial.Encoding = Encoding.GetEncoding("ks_c_5601-1987");
				serial.Open();
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			FuncDialog.DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
			return false;
		}
	}

	private void OnDataReceivedEvent(object source, SerialDataReceivedEventArgs e)
	{
		string sCheckstring = string.Empty;
		byte[] bytes = null;
		try
		{
			if (serial == null || !serial.IsOpen)
			{
				return;
			}
			do
			{
				int iReadLen = serial.BytesToRead;
				bytes = new byte[iReadLen];
				serial.Read(bytes, 0, iReadLen);
				sCheckstring += Encoding.Default.GetString(bytes);
				int startIndex;
				if (!sCheckstring.StartsWith('\u0002'.ToString()))
				{
					startIndex = sCheckstring.IndexOf('\u0002'.ToString());
					if (startIndex >= 0)
					{
						sCheckstring = sCheckstring.Substring(startIndex);
					}
				}
				startIndex = sCheckstring.IndexOf('\u0002'.ToString());
				int endIndex = sCheckstring.IndexOf('\u0003'.ToString());
				if (startIndex != -1 && endIndex != -1 && endIndex > startIndex && endIndex - startIndex < 10)
				{
					sCheckstring = sCheckstring.Substring(sCheckstring.IndexOf('\u0002', endIndex));
				}
			}
			while (sCheckstring.Length < 82);
			Variables.gPrintStatus = sCheckstring;
			serial.DiscardInBuffer();
		}
		catch (InvalidOperationException ex2)
		{
			FuncDialog.DspErrMsg(ex2, MethodBase.GetCurrentMethod().Name);
		}
		catch (Exception ex)
		{
			FuncDialog.DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
		}
	}

	public bool SendMsg(string sMsg)
	{
		try
		{
			if (serial != null)
			{
				if (serial.IsOpen)
				{
					serial.Write(sMsg);
					return true;
				}
				serial.Open();
				Thread.Sleep(1000);
				serial.Write(sMsg);
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			FuncDialog.DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
			return false;
		}
	}

	public bool SendMsg(byte[] bytes)
	{
		try
		{
			if (serial != null)
			{
				if (serial.IsOpen)
				{
					serial.Write(bytes, 0, bytes.Length);
					return true;
				}
				serial.Open();
				Thread.Sleep(2000);
				serial.Write(bytes, 0, bytes.Length);
				return true;
			}
			return false;
		}
		catch (Exception ex)
		{
			FuncDialog.DspErrMsg(ex, MethodBase.GetCurrentMethod().Name);
			return false;
		}
	}
}
