using System;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace MCS.PrintBoard;

public class FuncScanner
{
	private DelDataReceived dataReceived;

	public SerialArg initArg = null;

	private SerialPort _serialPort = null;

	private string dataBuffer = string.Empty;

	private int readTimeout = 500;

	private byte end_of_transmition = 4;

	private byte end_of_text = 3;

	private byte end_of_start = 2;

	private byte line_feed = 10;

	private byte carriage_return = 13;

	private char[] eof = null;

	public SerialPort SerialPort => _serialPort;

	public bool IsPortOpen => _serialPort.IsOpen;

	public string PortName
	{
		get
		{
			return _serialPort.PortName;
		}
		set
		{
			if ((value ?? "").ToString().ToUpper().Contains("COM"))
			{
				_serialPort.PortName = value;
			}
		}
	}

	public string LastErrorMessage { get; set; }

	[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	internal static extern SafeFileHandle CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr securityAttrs, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

	public FuncScanner()
	{
		_serialPort = new SerialPort();
		eof = new char[4]
		{
			(char)end_of_transmition,
			(char)end_of_text,
			(char)line_feed,
			(char)carriage_return
		};
	}

	public FuncScanner(SerialArg aSerialArg)
		: this()
	{
		initArg = aSerialArg;
	}

	public bool Ready()
	{
		bool ret = true;
		if (!SerialPort.GetPortNames().Any((string x) => string.Compare(x, initArg.Port, ignoreCase: true) == 0))
		{
			LastErrorMessage = $"{initArg.Port} port was not found.";
			ret = false;
		}
		else
		{
			SafeFileHandle hFile = CreateFile("\\\\.\\" + initArg.Port, -1073741824, 0, IntPtr.Zero, 3, 1073741824, IntPtr.Zero);
			if (hFile.IsInvalid)
			{
				LastErrorMessage = $"{initArg.Port} port is already open.";
				ret = false;
			}
			hFile.Close();
		}
		return ret;
	}

	public bool PortOpen(Variables.ScanTypeItem serialX)
	{
		try
		{
			if (_serialPort.IsNullOrEmpty())
			{
				_serialPort = null;
				dataBuffer = string.Empty;
				_serialPort = new SerialPort();
			}
			else if (_serialPort.IsOpen)
			{
				_serialPort.Close();
				_serialPort = null;
				dataBuffer = string.Empty;
				_serialPort = new SerialPort();
			}
			if (serialX == Variables.ScanTypeItem.Serial)
			{
				_serialPort.PortName = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Port", "COM1");
				_serialPort.BaudRate = Convert.ToInt32(FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Baud", "9600"));
				_serialPort.DataBits = Convert.ToInt32(FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Databits", "8"));
				_serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Stopbits", "None"));
				_serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Parity", "None"));
				_serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Handshake", "None"));
			}
			if (serialX == Variables.ScanTypeItem.Serial2)
			{
				_serialPort.PortName = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Port_2", "COM1");
				_serialPort.BaudRate = Convert.ToInt32(FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Baud_2", "9600"));
				_serialPort.DataBits = Convert.ToInt32(FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Databits_2", "8"));
				_serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Stopbits_2", "None"));
				_serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Parity_2", "None"));
				_serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Handshake_2", "None"));
			}
			_serialPort.ReadTimeout = readTimeout;
			_serialPort.Open();
			return true;
		}
		catch (Exception ex)
		{
			LastErrorMessage = ex.Message;
			return false;
		}
	}

	public bool PortOpen(Variables.ScanTypeItem serialX, DelDataReceived del)
	{
		try
		{
			if (_serialPort.IsNullOrEmpty())
			{
				_serialPort = null;
				dataBuffer = string.Empty;
				_serialPort = new SerialPort();
			}
			else if (_serialPort.IsOpen)
			{
				_serialPort.Close();
				_serialPort = null;
				dataBuffer = string.Empty;
				_serialPort = new SerialPort();
			}
			dataReceived = del;
			if (serialX == Variables.ScanTypeItem.Serial)
			{
				_serialPort.PortName = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Port", "COM1");
				_serialPort.BaudRate = Convert.ToInt32(FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Baud", "9600"));
				_serialPort.DataBits = Convert.ToInt32(FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Databits", "8"));
				_serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Stopbits", "One"));
				_serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Parity", "None"));
				_serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Handshake", "None"));
			}
			if (serialX == Variables.ScanTypeItem.Serial2)
			{
				_serialPort.PortName = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Port_2", "COM1");
				_serialPort.BaudRate = Convert.ToInt32(FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Baud_2", "9600"));
				_serialPort.DataBits = Convert.ToInt32(FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Databits_2", "8"));
				_serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Stopbits_2", "None"));
				_serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Parity_2", "None"));
				_serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Handshake_2", "None"));
			}
			_serialPort.ReadTimeout = readTimeout;
			_serialPort.Open();
			_serialPort.DataReceived -= dataReceived.Invoke;
			_serialPort.DataReceived += dataReceived.Invoke;
			return true;
		}
		catch (Exception ex)
		{
			LastErrorMessage = ex.Message;
			return false;
		}
	}

	public void PortClose(DelDataReceived del)
	{
		try
		{
			if (_serialPort != null)
			{
				_serialPort.DataReceived -= del.Invoke;
				_serialPort.Close();
				_serialPort = null;
				dataBuffer = string.Empty;
			}
		}
		catch (Exception ex)
		{
			FuncLog.Instance.LogSave("PortClose()" + ex.Message);
		}
	}

	public void DataBufferClear()
	{
		dataBuffer = string.Empty;
	}
}
