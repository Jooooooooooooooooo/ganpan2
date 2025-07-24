using System.IO.Ports;

namespace MCS.PrintBoard;

public class SerialArg
{
	private string sPort = "COM1";

	private int iBaudRate = 9600;

	private Parity parity = Parity.None;

	private int iDataBits = 8;

	private StopBits stopBits = StopBits.One;

	private Handshake handshake = Handshake.None;

	private int iReadInterval = 100;

	public string Port
	{
		get
		{
			return sPort;
		}
		set
		{
			sPort = value;
		}
	}

	public int BaudRate
	{
		get
		{
			return iBaudRate;
		}
		set
		{
			iBaudRate = value;
		}
	}

	public Parity Parity
	{
		get
		{
			return parity;
		}
		set
		{
			parity = value;
		}
	}

	public int DataBits
	{
		get
		{
			return iDataBits;
		}
		set
		{
			iDataBits = value;
		}
	}

	public StopBits Stopbits
	{
		get
		{
			return stopBits;
		}
		set
		{
			stopBits = value;
		}
	}

	public Handshake HandShake
	{
		get
		{
			return handshake;
		}
		set
		{
			handshake = value;
		}
	}

	public int ReadInterval
	{
		get
		{
			return iReadInterval;
		}
		set
		{
			iReadInterval = value;
		}
	}

	public SerialArg()
	{
	}

	public SerialArg(string asPort)
	{
		sPort = asPort;
	}

	public SerialArg(string asPort, int aiBaudRate, Parity aParity, int aiDataBit, StopBits aStopBits)
	{
		sPort = asPort;
		iBaudRate = aiBaudRate;
		parity = aParity;
		iDataBits = aiDataBit;
		stopBits = aStopBits;
	}
}
