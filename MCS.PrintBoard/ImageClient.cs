using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MCS.PrintBoard;

public class ImageClient
{
	public delegate void ImageDataReceivedEventHandler(ImageClient sender, byte[] e, int size);

	public delegate void ImageUploadFormReceivedEventHandler(ImageClient sender, byte[] e, int size);

	public delegate void StringDataReceivedEventHandler(ImageClient sender, byte[] e, int size);

	public delegate void SockErrorEventHandler(ImageClient sender, string e);

	private static ImageClient _instance;

	protected BackgroundWorker _bgWorker = null;

	protected TcpClient _client = null;

	protected NetworkStream _clientStream = null;

	protected StreamWriter _clientWriter = null;

	protected string _errorMsg = string.Empty;

	protected bool _serverMode = false;

	public static ImageClient GetInstance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new ImageClient();
			}
			return _instance;
		}
	}

	public bool Busy => _bgWorker.IsBusy;

	public string ErrorMsg => _errorMsg;

	public bool IsConnected { get; set; }

	public event ImageDataReceivedEventHandler ImageDataReceived = null;

	public event ImageUploadFormReceivedEventHandler ImageUploadFormReceived = null;

	public event StringDataReceivedEventHandler StringDataReceived = null;

	public event SockErrorEventHandler ErrorEventHandler = null;

	private ImageClient()
	{
	}

	public bool Connect(string ipAddress, int port, bool serverMode)
	{
		if (_client != null)
		{
			_client.Close();
		}
		try
		{
			_client = new TcpClient(ipAddress, port);
			_clientStream = _client.GetStream();
			_clientWriter = new StreamWriter(_clientStream);
			_serverMode = serverMode;
		}
		catch (Exception ex)
		{
			if (_client != null)
			{
				_clientWriter.Close();
				_clientStream.Close();
				_client.Close();
			}
			_errorMsg = "Getting client stream : " + ex.Message;
			IsConnected = false;
			return false;
		}
		_bgWorker = new BackgroundWorker();
		_bgWorker.WorkerSupportsCancellation = true;
		_bgWorker.DoWork += BgWorker_DoWork;
		_bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
		StartAndStop();
		IsConnected = true;
		return true;
	}

	public bool Connect(TcpClient client, bool serverMode)
	{
		_client = client;
		_serverMode = serverMode;
		try
		{
			_clientStream = _client.GetStream();
			_clientWriter = new StreamWriter(_clientStream);
		}
		catch (Exception ex)
		{
			_client.Close();
			_errorMsg = "Getting client stream : " + ex.Message;
			IsConnected = false;
			return false;
		}
		if (_clientStream == null)
		{
			_client.Close();
			IsConnected = false;
			return false;
		}
		_bgWorker = new BackgroundWorker();
		_bgWorker.WorkerSupportsCancellation = true;
		_bgWorker.DoWork += BgWorker_DoWork;
		_bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
		StartAndStop();
		IsConnected = true;
		return true;
	}

	protected void BgWorker_DoWork(object sender, DoWorkEventArgs e)
	{
		if (!(sender is BackgroundWorker worker))
		{
			return;
		}
		if (_serverMode)
		{
			e.Result = ReceiveClientToServer(worker);
		}
		else
		{
			e.Result = ReceiveServerToClient(worker);
		}
		if (worker.CancellationPending)
		{
			e.Cancel = true;
		}
		else if (!(bool)e.Result)
		{
			if (this.ErrorEventHandler != null)
			{
				this.ErrorEventHandler(this, _errorMsg);
			}
			throw new ArgumentException(_errorMsg);
		}
	}

	protected void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (e.Error == null && !e.Cancelled && (bool)e.Result && sender is BackgroundWorker worker)
		{
			worker.RunWorkerAsync();
		}
	}

	protected bool ReceiveClientToServer(BackgroundWorker worker)
	{
		byte[] rcvBuf = new byte[512];
		int count = 0;
		try
		{
			count = _clientStream.Read(rcvBuf, 0, rcvBuf.Length);
			this.StringDataReceived(this, rcvBuf, count);
		}
		catch (Exception ex)
		{
			_clientStream.Close();
			_client.Close();
			_errorMsg = "Reading header of the test request data : " + ex.Message;
			return false;
		}
		return true;
	}

	protected bool ReceiveServerToClient(BackgroundWorker worker)
	{
		try
		{
			int count = 0;
			int offset = 0;
			int headerSize = 4;
			byte[] rcvHdrBuf = new byte[headerSize];
			char dataCode = (char)_clientStream.ReadByte();
			count = _clientStream.Read(rcvHdrBuf, 0, headerSize);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(rcvHdrBuf);
			}
			int dataLength = BitConverter.ToInt32(rcvHdrBuf, 0);
			byte[] rcvDataBuffer = new byte[dataLength];
			Thread.Sleep(100);
			do
			{
				try
				{
					count = _clientStream.Read(rcvDataBuffer, offset, dataLength - offset);
				}
				catch (Exception ex)
				{
					_clientStream.Close();
					_client.Close();
					_errorMsg = "Reading test request data : " + ex.Message;
					return false;
				}
				offset += count;
			}
			while (offset < dataLength);
			if (dataCode == 'D')
			{
				string message = Encoding.UTF8.GetString(rcvDataBuffer, 0, rcvDataBuffer.Length);
				string captions = " Data Received ";
				this.StringDataReceived(this, rcvDataBuffer, 0);
			}
			else if (this.ImageUploadFormReceived != null && dataCode == 'I')
			{
				this.ImageUploadFormReceived(this, rcvDataBuffer, 0);
			}
			else if (this.ImageDataReceived != null && dataCode == 'I')
			{
				this.ImageDataReceived(this, rcvDataBuffer, 0);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
		}
		return true;
	}

	public void StringDataSend(char[] command, string strText)
	{
		MemoryStream ms = new MemoryStream();
		BinaryWriter bw = new BinaryWriter(ms);
		bw.Write(command, 0, 1);
		if (!string.IsNullOrEmpty(strText) && command[0] == 'N')
		{
			char[] sendText = strText.ToCharArray();
			byte[] test = BitConverter.GetBytes(sendText.Length);
			Array.Reverse(test);
			bw.Write(test);
			bw.Write(sendText);
			bw.Close();
		}
		byte[] sendBuffer = ms.ToArray();
		ms.Dispose();
		DataSend(sendBuffer, 0, sendBuffer.Length);
	}

	public byte[] ImageToByteArray(Image imageIn)
	{
		using MemoryStream ms = new MemoryStream();
		imageIn.Save(ms, ImageFormat.Png);
		return ms.ToArray();
	}

	public void StringDataSend(char[] command, string strText, string strImagePath)
	{
		MemoryStream ms = new MemoryStream();
		BinaryWriter bw = new BinaryWriter(ms);
		Image img1 = Image.FromFile(strImagePath);
		byte[] rawImage = ImageToByteArray(img1);
		bw.Write(command, 0, 1);
		if (!string.IsNullOrEmpty(strText) && command[0] == 'N')
		{
			char[] sendText = strText.ToCharArray();
			byte[] textLength = BitConverter.GetBytes(sendText.Length);
			byte[] imageLength = BitConverter.GetBytes(rawImage.Length);
			byte[] imageWidth = BitConverter.GetBytes(img1.Width);
			byte[] imageHeight = BitConverter.GetBytes(img1.Height);
			Array.Reverse(textLength);
			Array.Reverse(imageLength);
			Array.Reverse(imageWidth);
			Array.Reverse(imageHeight);
			bw.Write(textLength);
			bw.Write(sendText);
			bw.Write(imageLength);
			bw.Write(imageWidth);
			bw.Write(imageHeight);
			bw.Write(rawImage);
			bw.Close();
		}
		byte[] sendBuffer = ms.ToArray();
		ms.Dispose();
		DataSend(sendBuffer, 0, sendBuffer.Length);
	}

	public void StringDataSend(char[] command, string strText, int ImgWidth, int ImgHeight, byte[] rawImage)
	{
		MemoryStream ms = new MemoryStream();
		BinaryWriter bw = new BinaryWriter(ms);
		bw.Write(command, 0, 1);
		string test = $"TextLength {strText.Length} ImageLength {rawImage.Length} width {ImgWidth} height {ImgHeight}";
		Console.WriteLine(test);
		Console.WriteLine(strText);
		if (!string.IsNullOrEmpty(strText) && command[0] == 'N')
		{
			char[] sendText = strText.ToCharArray();
			byte[] textLength = BitConverter.GetBytes(sendText.Length);
			byte[] imageLength = BitConverter.GetBytes(rawImage.Length);
			byte[] imageWidth = BitConverter.GetBytes(ImgWidth);
			byte[] imageHeight = BitConverter.GetBytes(ImgHeight);
			Array.Reverse(textLength);
			Array.Reverse(imageLength);
			Array.Reverse(imageWidth);
			Array.Reverse(imageHeight);
			bw.Write(textLength);
			bw.Write(sendText);
			bw.Write(imageLength);
			bw.Write(imageWidth);
			bw.Write(imageHeight);
			bw.Write(rawImage);
			bw.Close();
		}
		byte[] sendBuffer = ms.ToArray();
		ms.Dispose();
		DataSend(sendBuffer, 0, sendBuffer.Length);
	}

	public void ImageDataSend(string strImagePath)
	{
		MemoryStream ms = new MemoryStream();
		BinaryWriter bw = new BinaryWriter(ms);
		byte[] imageReadBuffer = File.ReadAllBytes(strImagePath);
		char[] dataType = new char[1] { 'N' };
		bw.Write(dataType);
		bw.Write(imageReadBuffer.Length);
		bw.Write(imageReadBuffer);
		bw.Close();
		byte[] sendBuffer = ms.ToArray();
		ms.Dispose();
		DataSend(sendBuffer, 0, sendBuffer.Length);
	}

	public void ImageDataSend(byte[] rawImage)
	{
		MemoryStream ms = new MemoryStream();
		BinaryWriter bw = new BinaryWriter(ms);
		char[] dataType = new char[4] { '0', '1', '0', '0' };
		bw.Write(dataType);
		bw.Write(rawImage.Length);
		bw.Write(rawImage);
		bw.Close();
		byte[] sendBuffer = ms.ToArray();
		ms.Dispose();
		DataSend(sendBuffer, 0, sendBuffer.Length);
	}

	public void DataSend(byte[] data, int index, int length)
	{
		if (_client != null && _client.Connected)
		{
			_clientStream.BeginWrite(data, 0, length, DataSendCallBack, null);
		}
	}

	public void DataSendCallBack(IAsyncResult ar)
	{
		try
		{
			_clientStream.EndWrite(ar);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"SEND ERROR\n{ex.Message}");
		}
	}

	public void StartAndStop()
	{
		if (_bgWorker.IsBusy)
		{
			_bgWorker.CancelAsync();
		}
		else
		{
			_bgWorker.RunWorkerAsync();
		}
	}
}
