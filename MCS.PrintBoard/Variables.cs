namespace MCS.PrintBoard;

public class Variables
{
	public enum ScanTypeItem
	{
		None,
		Serial,
		Serial2,
		Socket
	}

	public class LoginXmlItemType
	{
		public static readonly string PrinterNodeName = "Printer_CONFIG";

		public static readonly string scannerNodeName = "Scanner_CONFIG";

		public static readonly string ServerNodeName = "Server_CONFIG";

		public static readonly string ServerAgentNodeName = "SERVER_AGENT";
	}

	public static readonly string[] BuadrateItem = new string[7] { "4800", "9600", "14400", "19200", "38400", "57600", "115200" };

	public static readonly string[] DatabitItem = new string[4] { "5", "6", "7", "8" };

	public static string gPrintStatus = string.Empty;

	public static FuncScanner scannerPort = new FuncScanner();
}
