using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FarPoint.Win;
using FarPoint.Win.Spread;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;

namespace MCS.PrintBoard.PrintBoard;

public class frmDecantMakeCart : frmBase
{
	public class TxnUnit
	{
		public static readonly string SHEET = "SHEET";

		public static readonly string CARRIER = "CARRIER";
	}

	public class InOutType
	{
		public static readonly string MANUAL_IN = "MANUAL_IN";

		public static readonly string MANUAL_OUT = "MANUAL_OUT";
	}

	public class MakeRemove
	{
		public static readonly string MAKE = "MAKE";

		public static readonly string REMOVE = "REMOVE";

		public static readonly string UPDATE = "UPDATE";
	}

	public class ReturnMSG
	{
		public static readonly string RTN_CODE = "RTN_CODE";

		public static readonly string RTN_VALUE = "RTN_VALUE";

		public static readonly string RESULT_MSG = "RESULT_MSG";
	}

	public class MessageResult
	{
		public static readonly string OK = "OK";

		public static readonly string NG = "NG";
	}

	public enum MessageNoticeType
	{
		None,
		MainPanel,
		Dialog,
		Both
	}

	private DataTable MessageList = null;

	private string _orgID = string.Empty;

	private string _userID = string.Empty;

	private string _langID = string.Empty;

	private string _toLocator = string.Empty;

	private string _toLineCode = string.Empty;

	private System.Windows.Forms.Timer _tmrReqAGVInfo;

	private System.Windows.Forms.Timer _tmrMsgClear;

	private readonly int C_LOCATOR_TIMER_INTERVAL = 5000;

	private readonly int C_MSG_CLEAR_TIMER_INTERVAL = 5000;

	private readonly string C_TXN_FROM = "UI";

	private readonly string C_SUPPLY_TYPE = "UI";

	private readonly string C_START_END = "MANUAL";

	private readonly string C_DECANT_USER_ID = "DECANT";

	private string[] gRecvBuf = new string[10];

	private string[] gRecvData = new string[10];

	private const string STX = "\u0002";

	private const string ETX = "\u0003";

	private const string EOT = "\u0004";

	private const string ENQ = "\u0005";

	private const string ACK = "\u0006";

	private const string LF = "\n";

	private const string CR = "\r";

	private const string NAK = "\u0015";

	private IContainer components = null;

	private PanelOnly panelOnly1;

	private Label label8;

	private Panel panel2;

	private Panel panel3;

	private MCS.Common.ComboBox cboLocator;

	private Label label3;

	private MCS.Common.ComboBox cboPartNo;

	private MCS.Common.FpSpread fpPlanInfo;

	private MCS.Common.FpSpread fpCartLoadInfo;

	private TextBox txtCarrierID;

	private System.Windows.Forms.Button btnNumCh;

	private System.Windows.Forms.Button btnComplete;

	private System.Windows.Forms.Button btnEmptyAll;

	private System.Windows.Forms.Button btnAdd;

	private System.Windows.Forms.Button btnDel;

	private NumericUpDown numLoadQty;

	private System.Windows.Forms.Button btnLoadQty;

	private Panel panel5;

	private TextBox txtMsg;

	private System.Windows.Forms.Button btnPlan;

	private Label label2;

	private Panel panel1;

	private TextBox txtAGVOut;

	private TextBox txtAGVIn;

	private Label label4;

	private Label label1;

	private Label label6;

	private Label label5;

	private Label label7;

	private NumericUpDown numExceptQty;

	private System.Windows.Forms.Button btnSearch;

	public frmDecantMakeCart()
	{
		InitializeComponent();
	}

	private void frmDecantMakeCart_Load(object sender, EventArgs e)
	{
		try
		{
			_userID = C_DECANT_USER_ID;
			_orgID = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "ORG_ID", "302550");
			_langID = Thread.CurrentThread.CurrentCulture.ToString();
			GetMessageInfo(_langID);
			InitControls();
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void frmDecantMakeCart_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			_tmrReqAGVInfo.Stop();
			_tmrReqAGVInfo.Tick -= tmrReqAGVInfo_Tick;
			_tmrMsgClear.Stop();
			_tmrMsgClear.Tick -= tmrMsgClear_Tick;
			cboLocator.SelectedValueChanged -= cboLocator_SelectedValueChanged;
			cboPartNo.SelectedValueChanged -= cboPartNo_SelectedValueChanged;
			if (Variables.scannerPort.IsPortOpen)
			{
				Variables.scannerPort.PortClose(SerialReceiveEvent);
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void tmrReqAGVInfo_Tick(object sender, EventArgs e)
	{
		try
		{
			_tmrReqAGVInfo.Stop();
			if (cboLocator.SelectedValue != null && !(cboLocator.SelectedValue.ToString() == string.Empty))
			{
				GetRequestInfo(cboLocator.SelectedValue.ToString());
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
		finally
		{
			_tmrReqAGVInfo.Start();
		}
	}

	private void tmrMsgClear_Tick(object sender, EventArgs e)
	{
		try
		{
			_tmrMsgClear.Stop();
			txtMsg.Text = string.Empty;
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
		finally
		{
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(txtCarrierID.Text))
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Cart"));
			}
			else if (cboLocator.SelectedValue == null || cboLocator.SelectedValue.ToString() == string.Empty)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Locator"));
			}
			else if (cboPartNo.SelectedValue == null || cboPartNo.SelectedValue.ToString() == string.Empty)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Part No"));
			}
			else
			{
				AddPartNoToCart(cboLocator.SelectedValue.ToString(), txtCarrierID.Text);
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void btnDel_Click(object sender, EventArgs e)
	{
		try
		{
			RemovePartNoToCart();
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void btnEmptyAll_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrWhiteSpace(txtCarrierID.Text))
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Cart"));
			}
			else if (cboLocator.SelectedValue == null || cboLocator.SelectedValue.ToString() == string.Empty)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Locator"));
			}
			else
			{
				RemoveAll(cboLocator.SelectedValue.ToString(), txtCarrierID.Text);
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void cboLocator_SelectedValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (cboLocator.SelectedValue != null && !(cboLocator.SelectedValue.ToString() == string.Empty))
			{
				SetDecantToLocator(cboLocator.SelectedValue.ToString());
				FuncXml.Instance.WriteXml(Variables.LoginXmlItemType.scannerNodeName, "LOCATOR", cboLocator.SelectedValue.ToString());
				SetComboBoxPartNo(cboLocator.SelectedValue.ToString());
				txtCarrierID.Text = string.Empty;
				fpCartLoadInfo.ActiveSheet.Rows.Clear();
				fpCartLoadInfo.Refresh();
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void cboPartNo_SelectedValueChanged(object sender, EventArgs e)
	{
		try
		{
			DataBindPlanInfo(isMSG: true);
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void btnNumCh_Click(object sender, EventArgs e)
	{
		try
		{
			Process.Start("C:\\Windows\\System32\\osk.exe");
			numLoadQty.Focus();
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void btnLoadQty_Click(object sender, EventArgs e)
	{
		try
		{
			CalcLoadQty();
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void txtCarrierID_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (e.KeyChar == '\r')
			{
				if (string.IsNullOrWhiteSpace(txtCarrierID.Text))
				{
					ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Cart"));
				}
				else
				{
					GetScanInfo(txtCarrierID.Text, string.Empty, string.Empty, string.Empty);
				}
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void btnComplete_Click(object sender, EventArgs e)
	{
		try
		{
			if (string.IsNullOrEmpty(txtCarrierID.Text))
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Cart"));
			}
			else if (cboLocator.SelectedValue == null || cboLocator.SelectedValue.ToString() == string.Empty)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Locator"));
			}
			else if (cboPartNo.SelectedValue == null || cboPartNo.SelectedValue.ToString() == string.Empty)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "PartNo"));
			}
			else
			{
				ProcessComplete(cboLocator.SelectedValue.ToString(), txtCarrierID.Text);
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void SerialReceiveEvent(object sender, SerialDataReceivedEventArgs e)
	{
		int SckIndex = 0;
		string sRecvData = "";
		string sFirstScanData = "";
		int STX_Pos = 0;
		int ETX_Pos = 0;
		int DataType = 0;
		int bytelength = 0;
		try
		{
			string sCheckstring = string.Empty;
			bytelength = Variables.scannerPort.SerialPort.BytesToRead;
			byte[] bytes = new byte[bytelength];
			Variables.scannerPort.SerialPort.Read(bytes, 0, bytelength);
			sCheckstring += Encoding.Default.GetString(bytes);
			Invoke((MethodInvoker)delegate
			{
				FuncLog.Instance.LogSave("-----------Scan-----------");
				FuncLog.Instance.LogSave(sCheckstring);
				FuncLog.Instance.LogSave("-----------Scan-----------");
			});
			sRecvData = sCheckstring;
			SckIndex = 0;
			if (sRecvData.Length < 0)
			{
				return;
			}
			STX_Pos = sRecvData.IndexOf("\u0002");
			ETX_Pos = sRecvData.IndexOf("\u0003");
			if (STX_Pos > -1 && ETX_Pos == -1)
			{
				DataType = 1;
			}
			if (STX_Pos == -1 && ETX_Pos > -1)
			{
				DataType = 2;
			}
			if (STX_Pos > -1 && ETX_Pos > -1)
			{
				DataType = 3;
			}
			switch (DataType)
			{
			case 0:
				gRecvBuf[SckIndex] += sRecvData;
				break;
			case 1:
				gRecvBuf[SckIndex] = sRecvData;
				break;
			case 2:
				gRecvData[SckIndex] = gRecvBuf[SckIndex] + sRecvData;
				break;
			case 3:
				if (sRecvData.Substring(0, 1) == "\u0002" && sRecvData.Substring(sRecvData.Length - 1, 1) == "\u0003")
				{
					gRecvData[SckIndex] = sRecvData;
					break;
				}
				gRecvData[SckIndex] = gRecvBuf[SckIndex] + sRecvData.Substring(0, ETX_Pos + 1);
				gRecvBuf[SckIndex] = sRecvData.Substring(STX_Pos + 1);
				break;
			}
			if (gRecvData[SckIndex] == string.Empty || gRecvData[SckIndex] == null)
			{
				return;
			}
			while (gRecvData[SckIndex].Substring(0, 1) == "\u0002" && gRecvData[SckIndex].Substring(gRecvData[SckIndex].Length - 1, 1) == "\u0003")
			{
				STX_Pos = gRecvData[SckIndex].IndexOf("\u0002");
				ETX_Pos = gRecvData[SckIndex].IndexOf("\u0003");
				sFirstScanData = "";
				sFirstScanData = gRecvData[SckIndex].Substring(STX_Pos, ETX_Pos - STX_Pos + 1);
				gRecvData[SckIndex] = gRecvData[SckIndex].Substring(ETX_Pos + 1);
				sFirstScanData = sFirstScanData.Substring(1, sFirstScanData.Length - 2);
				Invoke((MethodInvoker)delegate
				{
					GetScanInfo(sFirstScanData, string.Empty, string.Empty, string.Empty);
				});
				if (gRecvData[SckIndex].IndexOf("\u0003") > -1)
				{
					continue;
				}
				break;
			}
		}
		catch (Exception)
		{
			gRecvData[SckIndex] = "";
			gRecvBuf[SckIndex] = "";
			sFirstScanData = "";
			sRecvData = "";
		}
	}

	private void btnPlan_Click(object sender, EventArgs e)
	{
		frmDecantPlan popup = null;
		try
		{
			if (cboLocator.SelectedValue == null || cboLocator.SelectedValue.ToString() == string.Empty)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91094", _langID, "Locator"));
				return;
			}
			popup = new frmDecantPlan();
			popup.StartPosition = FormStartPosition.CenterScreen;
			popup._orgID = _orgID;
			popup._toLineCode = _toLineCode;
			popup.ShowDialog();
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		try
		{
			DataBindPlanInfo(isMSG: true);
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void fp1_CellClick(object sender, CellClickEventArgs e)
	{
		DataTable dt = null;
		dt = fpPlanInfo.DataSource as DataTable;
		for (int i = 0; i < dt.Rows.Count; i++)
		{
			if (dt.Rows[i].RowState == DataRowState.Added && e.Row == i && e.Column == 13)
			{
				dt.Rows[i].Delete();
			}
		}
	}

	private void InitControls()
	{
		try
		{
			setSheetColumnPlanInfo();
			setSheetColumnCartLoadInfo();
			SetComboBoxLocator();
			cboLocator.SelectedValueChanged += cboLocator_SelectedValueChanged;
			cboPartNo.SelectedValueChanged += cboPartNo_SelectedValueChanged;
			if (FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "LOCATOR", string.Empty) != string.Empty)
			{
				cboLocator.SelectedValue = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "LOCATOR", string.Empty);
			}
			if (!Variables.scannerPort.PortOpen(Variables.ScanTypeItem.Serial, SerialReceiveEvent))
			{
				ProcMsgResult(MessageNoticeType.MainPanel, Variables.scannerPort.LastErrorMessage);
			}
			_tmrReqAGVInfo = new System.Windows.Forms.Timer();
			_tmrReqAGVInfo.Tick += tmrReqAGVInfo_Tick;
			_tmrReqAGVInfo.Interval = C_LOCATOR_TIMER_INTERVAL;
			_tmrReqAGVInfo.Start();
			_tmrMsgClear = new System.Windows.Forms.Timer();
			_tmrMsgClear.Tick += tmrMsgClear_Tick;
			_tmrMsgClear.Interval = C_MSG_CLEAR_TIMER_INTERVAL;
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void setSheetColumnPlanInfo()
	{
		try
		{
			MCS.Common.SheetView svPlanInfo = new MCS.Common.SheetView(fpPlanInfo, "Search", OperationMode.Normal, bRowHeaderVisible: true, "BackColor White");
			svPlanInfo.AddColumnCheckBox("", "CHK", 50, CellHorizontalAlignment.Center, bLocked: false, bVisible: true, "", "", "", bThreeState: false);
			svPlanInfo.AddColumnDateTime("Plan Date", "PLAN_YYYYMMDD", 80, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
			svPlanInfo.AddColumnText("Model", "MODEL_SUFFIX", 180, CellHorizontalAlignment.Center, bLocked: false, bVisible: true, 500);
			svPlanInfo.AddColumnText("Workorder", "WO_NAME", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 300);
			svPlanInfo.AddColumnText("WOID", "WOID", 1, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 300);
			svPlanInfo.AddColumnText("Total Qty", "TOTAL_PLAN_QTY", 1, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 1);
			svPlanInfo.AddColumnText("Daily Qty", "DAILY_PLAN_QTY", 75, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPlanInfo.AddColumnText("Daily/Total", "DAILY_TOTA_QTY", 80, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 300);
			svPlanInfo.AddColumnText("Item Code", "ITEM_CODE", 1, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 300);
			svPlanInfo.AddColumnText("Required", "REQUIRED_QTY", 75, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPlanInfo.AddColumnText("Result Qty", "RESULT_QTY", 75, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPlanInfo.AddColumnText("Decant", "DECANT_QTY", 75, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPlanInfo.AddColumnText("PRE Decant", "PRE_DECANT_QTY", 1, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 1);
			svPlanInfo.AddColumnText("Plan Seq", "PLAN_SEQ", 75, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPlanInfo.RowHeader.Visible = true;
			svPlanInfo.Rows.Default.Height = 45f;
			svPlanInfo.ColumnHeader.Rows[0].Height = 45f;
			svPlanInfo.ColumnHeader.Rows[0].Font = new Font(new FontFamily("Arial"), 10f, FontStyle.Bold);
			svPlanInfo.Rows.Default.Font = new Font(new FontFamily("Arial"), 10f);
			fpPlanInfo.AutoSizeColumnWidth = false;
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void setSheetColumnCartLoadInfo()
	{
		try
		{
			MCS.Common.SheetView svCartLoadInfo = new MCS.Common.SheetView(fpCartLoadInfo, "Search", OperationMode.Normal, bRowHeaderVisible: true, "BackColor White");
			svCartLoadInfo.AddColumnText("Item Code", "ITEM_CODE", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 150);
			svCartLoadInfo.AddColumnText("Cart ID", "CARRIER_ID", 75, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 100);
			svCartLoadInfo.AddColumnText("Decant", "TO_WO_QTY", 65, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 120);
			svCartLoadInfo.AddColumnDateTime("Last Time", "UPDATED_DATE", 130, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " hh:mm:ss");
			svCartLoadInfo.RowHeader.Visible = true;
			svCartLoadInfo.Rows.Default.Height = 45f;
			svCartLoadInfo.ColumnHeader.Rows[0].Height = 50f;
			svCartLoadInfo.ColumnHeader.Rows[0].Font = new Font(new FontFamily("Arial"), 10f, FontStyle.Bold);
			svCartLoadInfo.Rows.Default.Font = new Font(new FontFamily("Arial"), 10f);
			fpCartLoadInfo.AutoSizeColumnWidth = false;
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void CalcLoadQty()
	{
		DataTable dt = null;
		decimal loadQty = default(decimal);
		decimal decantQty = default(decimal);
		decimal inputQty = default(decimal);
		decimal resultQty = default(decimal);
		decimal requiredQty = default(decimal);
		try
		{
			MCS.Common.SheetView svPlanInfo = fpPlanInfo.Sheets[0] as MCS.Common.SheetView;
			dt = fpPlanInfo.DataSource as DataTable;
			loadQty = numLoadQty.Value;
			if (!(loadQty != 0m))
			{
				return;
			}
			if (dt.Rows.Count == 0)
			{
				fpPlanInfo.ActiveSheet.AddRows(0, 1);
				svPlanInfo.Cells[0, 11].BackColor = Color.Yellow;
				fpPlanInfo.ActiveSheet.SetText(0, "CHK", "TRUE");
				fpPlanInfo.ActiveSheet.SetText(0, "PLAN_YYYYMMDD", DateTime.Now.ToString());
				fpPlanInfo.ActiveSheet.SetText(0, "MODEL_SUFFIX", "Manual Created");
				fpPlanInfo.ActiveSheet.SetText(0, "WO_NAME", "-");
				fpPlanInfo.ActiveSheet.SetText(0, "TOTAL_PLAN_QTY", "-");
				fpPlanInfo.ActiveSheet.SetText(0, "DAILY_PLAN_QTY", "-");
				fpPlanInfo.ActiveSheet.SetText(0, "DAILY_TOTA_QTY", "-");
				fpPlanInfo.ActiveSheet.SetText(0, "ITEM_CODE", cboPartNo.SelectedValue.ToString());
				fpPlanInfo.ActiveSheet.SetText(0, "REQUIRED_QTY", "-");
				fpPlanInfo.ActiveSheet.SetText(0, "RESULT_QTY", "-");
				fpPlanInfo.ActiveSheet.SetText(0, "DECANT_QTY", loadQty.ToString());
				fpPlanInfo.ActiveSheet.SetText(0, "PLAN_SEQ", "-");
				fpPlanInfo.ActiveSheet.SetText(0, "PRE_DECANT_QTY", 0.ToString());
			}
			else
			{
				for (int k = 0; k < dt.Rows.Count; k++)
				{
					if (dt.Rows[k].RowState == DataRowState.Added)
					{
						dt.Rows[k].Delete();
					}
				}
				for (int j = 0; j < dt.Rows.Count; j++)
				{
					dt.Rows[j]["DECANT_QTY"] = dt.Rows[j]["PRE_DECANT_QTY"];
					svPlanInfo.Cells[j, 11].BackColor = Color.White;
				}
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Convert.ToBoolean(dt.Rows[i]["CHK"]))
					{
						requiredQty = Convert.ToDecimal(dt.Rows[i]["REQUIRED_QTY"]);
						resultQty = Convert.ToDecimal(dt.Rows[i]["RESULT_QTY"]);
						decantQty = Convert.ToDecimal(dt.Rows[i]["DECANT_QTY"]);
						inputQty = requiredQty - resultQty - decantQty;
						if (!(loadQty - inputQty > 0m))
						{
							dt.Rows[i]["DECANT_QTY"] = decantQty + loadQty;
							svPlanInfo.Cells[i, 11].BackColor = Color.Yellow;
							break;
						}
						dt.Rows[i]["DECANT_QTY"] = decantQty + inputQty;
						loadQty -= inputQty;
						svPlanInfo.Cells[i, 11].BackColor = Color.Green;
						if (i == dt.Rows.Count - 1)
						{
							fpPlanInfo.ActiveSheet.AddRows(i + 1, 1);
							svPlanInfo.Cells[i + 1, 11].BackColor = Color.Red;
							fpPlanInfo.ActiveSheet.SetText(i + 1, "CHK", "TRUE");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "PLAN_YYYYMMDD", DateTime.Now.ToString());
							fpPlanInfo.ActiveSheet.SetText(i + 1, "WOID", "");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "MODEL_SUFFIX", "Manual Created");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "WO_NAME", "-");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "TOTAL_PLAN_QTY", "-");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "DAILY_PLAN_QTY", "-");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "DAILY_TOTA_QTY", "-");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "ITEM_CODE", dt.Rows[0]["ITEM_CODE"].ToString());
							fpPlanInfo.ActiveSheet.SetText(i + 1, "REQUIRED_QTY", "-");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "RESULT_QTY", "-");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "DECANT_QTY", loadQty.ToString());
							fpPlanInfo.ActiveSheet.SetText(i + 1, "PLAN_SEQ", "-");
							fpPlanInfo.ActiveSheet.SetText(i + 1, "PRE_DECANT_QTY", 0.ToString());
							break;
						}
					}
				}
			}
			fpPlanInfo.Refresh();
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void SetComboBoxLocator()
	{
		DataSet dsResult = null;
		try
		{
			dsResult = GetLocatorInfoBIZ(_orgID);
			if (dsResult != null && dsResult.Tables["OUT_DATA"] != null)
			{
				cboLocator.SetItemList(dsResult.Tables["OUT_DATA"], "LOCATOR", "LOCATOR_NAME", AllFlag: false);
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void SetComboBoxPartNo(string locator)
	{
		DataSet dsResult = null;
		try
		{
			dsResult = GetPartNoInfoBIZ(_orgID, locator);
			if (dsResult != null && dsResult.Tables["OUT_DATA"] != null)
			{
				cboPartNo.SetItemList(dsResult.Tables["OUT_DATA"], "ITEM_CODE", "ITEM_CODE_DESD", AllFlag: false);
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void DataBindCartLoadInfo(string carrierID, bool isMSG)
	{
		DataSet dsResult = null;
		try
		{
			if (string.IsNullOrEmpty(carrierID))
			{
				return;
			}
			dsResult = GetCartLoadInfoBIZ(_orgID, carrierID);
			if (dsResult != null)
			{
				fpCartLoadInfo.DataSource = dsResult.Tables["OUT_DATA"];
				fpCartLoadInfo.Refresh();
				if (isMSG)
				{
					ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("10006", _langID, fpCartLoadInfo.ActiveSheet.RowCount.ToString()));
				}
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void DataBindPlanInfo(bool isMSG)
	{
		DataSet dsResult = null;
		DataSet dsResult2 = null;
		try
		{
			if (cboPartNo.SelectedValue == null || cboPartNo.SelectedValue.ToString() == string.Empty)
			{
				return;
			}
			dsResult = GetPlanInfoBIZ(_orgID, _toLocator, cboPartNo.SelectedValue.ToString(), _toLineCode, numExceptQty.Value.ToString());
			if (dsResult != null)
			{
				fpPlanInfo.DataSource = dsResult.Tables["OUT_DATA"];
				fpPlanInfo.Refresh();
				if (isMSG)
				{
					ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("10006", _langID, fpPlanInfo.ActiveSheet.RowCount.ToString()));
				}
			}
			if (cboPartNo.SelectedValue != null && cboPartNo.SelectedValue.ToString() != string.Empty)
			{
				dsResult2 = GetItemMcsInfo(_orgID, cboPartNo.SelectedValue.ToString());
				if (dsResult2.Tables["OUT_DATA"] != null && dsResult2.Tables["OUT_DATA"].Rows.Count > 0)
				{
					numLoadQty.Value = ((dsResult2.Tables["OUT_DATA"].Rows[0]["PACK_UNIT_QTY"] == null) ? numLoadQty.Value : Convert.ToDecimal(dsResult2.Tables["OUT_DATA"].Rows[0]["PACK_UNIT_QTY"].ToString()));
				}
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	public int GetIndexFromSelectedValue(System.Windows.Forms.ComboBox combobox, object value)
	{
		if (combobox.Items.Count > 0)
		{
			for (int i = 0; i < combobox.Items.Count; i++)
			{
				object item = combobox.Items[i];
				object thisValue = (item as DataRowView)[combobox.ValueMember];
				if (thisValue != null && thisValue.Equals(value))
				{
					combobox.SelectedIndex = i;
					return i;
				}
			}
		}
		return -1;
	}

	private void AddPartNoToCart(string pLocatorID, string pCarrierID)
	{
		string orderFlag = "N";
		DataTable dtPlanInfo = null;
		DataTable dtCartInfo = null;
		DataSet dsResult = null;
		Dictionary<string, string> param = null;
		List<Dictionary<string, string>> parameters = null;
		try
		{
			dtPlanInfo = fpPlanInfo.DataSource as DataTable;
			dtCartInfo = fpCartLoadInfo.DataSource as DataTable;
			if (dtPlanInfo.Rows.Count == 0)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("60402", _langID));
				return;
			}
			if (dtCartInfo != null)
			{
				for (int j = 0; j < dtCartInfo.Rows.Count; j++)
				{
					if (cboPartNo.SelectedValue.ToString() == dtCartInfo.Rows[j]["ITEM_CODE"].ToString())
					{
						ProcMsgResult(MessageNoticeType.MainPanel, "PartNo data already exist.");
						return;
					}
				}
			}
			parameters = new List<Dictionary<string, string>>();
			param = new Dictionary<string, string>();
			for (int i = 0; i < dtPlanInfo.Rows.Count; i++)
			{
				if (Convert.ToBoolean(dtPlanInfo.Rows[i]["CHK"]) && Convert.ToDecimal(dtPlanInfo.Rows[i]["DECANT_QTY"]) != 0m && Convert.ToDecimal(dtPlanInfo.Rows[i]["DECANT_QTY"]) != Convert.ToDecimal(dtPlanInfo.Rows[i]["PRE_DECANT_QTY"]))
				{
					param = new Dictionary<string, string>();
					param.Add("ORG_ID", _orgID);
					param.Add("WOID", dtPlanInfo.Rows[i]["WOID"].ToString());
					param.Add("ITEM_CODE", dtPlanInfo.Rows[i]["ITEM_CODE"].ToString());
					param.Add("QTY", dtPlanInfo.Rows[i]["DECANT_QTY"].ToString());
					param.Add("ITEM_SN", string.Empty);
					param.Add("CARRIER_ID", pCarrierID);
					param.Add("LOCATOR", pLocatorID);
					param.Add("MAKE_REMOVE", MakeRemove.MAKE);
					param.Add("USER_ID", _userID);
					param.Add("LANGID", _langID);
					param.Add("ORDER_FLAG", orderFlag);
					param.Add("EQSG_ID", string.Empty);
					parameters.Add(param);
				}
			}
			if (parameters.Count() == 0)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, "There are no data to add.");
				return;
			}
			dsResult = SetCarrierManualCsMultiBIZ(parameters);
			if (dsResult != null && dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_CODE"].ToString() == "OK")
			{
				DataBindCartLoadInfo(pCarrierID, isMSG: false);
				DataBindPlanInfo(isMSG: false);
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91018", _langID));
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void RemoveAll(string pLocatorID, string pCarrierID)
	{
		DataTable dt = null;
		DataSet dsResult = null;
		Dictionary<string, string> param = null;
		List<Dictionary<string, string>> parameters = null;
		try
		{
			if (!(fpCartLoadInfo.DataSource is DataTable dt2) || dt2.Rows.Count == 0)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("20105", _langID));
				return;
			}
			parameters = new List<Dictionary<string, string>>();
			param = new Dictionary<string, string>();
			param.Add("I_ORG_ID", _orgID);
			param.Add("I_START_END", C_START_END);
			param.Add("I_CARRIER_ID", pCarrierID);
			param.Add("I_SHEET_ID", string.Empty);
			param.Add("I_LOCATOR", pLocatorID);
			param.Add("I_GUBUN", string.Empty);
			param.Add("I_POSITION", string.Empty);
			param.Add("I_ITEM_CODE", string.Empty);
			param.Add("I_ITEM_SN", string.Empty);
			param.Add("I_QTY", "0");
			param.Add("I_USER_ID", _userID);
			param.Add("I_TXNDATE", string.Empty);
			param.Add("I_TXN_FROM", C_TXN_FROM);
			param.Add("I_WO_NAME", string.Empty);
			param.Add("I_TO_WO_NAME", string.Empty);
			param.Add("I_COMMENTS", string.Empty);
			param.Add("I_INOUT_TYPE", InOutType.MANUAL_OUT);
			param.Add("I_TXN_UNIT", TxnUnit.CARRIER);
			param.Add("I_TXN_ID", pCarrierID);
			parameters.Add(param);
			DataBindCartLoadInfo(txtCarrierID.Text, isMSG: false);
			DataBindPlanInfo(isMSG: false);
			ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91018", _langID));
			dsResult = SetCarrierUnMapBIZ(parameters);
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void SetDecantToLocator(string locator)
	{
		try
		{
			DataSet dsResult = GetDecantToLocatorBIZ(_orgID, locator);
			if (dsResult != null && dsResult.Tables["OUT_DATA"] != null)
			{
				_toLocator = dsResult.Tables["OUT_DATA"].Rows[0]["LOCATOR"].ToString();
				_toLineCode = dsResult.Tables["OUT_DATA"].Rows[0]["LINE_CODE"].ToString();
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	public void GetScanInfo(string scanData, string SheetInfoFromDB, string carrierInfoFromDB, string locatorInfoFromDB)
	{
		DataSet ds = null;
		string scanType = string.Empty;
		string scanStr = string.Empty;
		try
		{
			ds = getChkScanTypeBIZ(_orgID, _langID, scanData, SheetInfoFromDB, carrierInfoFromDB, locatorInfoFromDB);
			if (ds == null || ds.Tables["OUT_DATA"].Rows.Count == 0)
			{
				return;
			}
			scanType = ds.Tables["OUT_DATA"].Rows[0]["SCAN_TYPE"].ToString();
			scanStr = ds.Tables["OUT_DATA"].Rows[0]["SCAN_STR"].ToString();
			switch (scanType)
			{
			case "LOCATOR":
				cboLocator.SelectedIndex = GetIndexFromSelectedValue(cboLocator, scanStr);
				break;
			case "CARRIER_ID":
			{
				txtCarrierID.Text = scanStr;
				DataSet dsResult = GetManualCarrierMakeBIZ(_orgID, cboLocator.SelectedValue.ToString(), scanStr);
				if (dsResult != null)
				{
					DataBindCartLoadInfo(scanStr, isMSG: true);
				}
				break;
			}
			case "ITEM_CODE":
				cboPartNo.SelectedIndex = GetIndexFromSelectedValue(cboPartNo, scanStr);
				break;
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void ProcessComplete(string locator, string carrierID)
	{
		DataSet dsResult = null;
		try
		{
			dsResult = ProcCarrierCallBIZ(_orgID, locator, carrierID, _userID, C_SUPPLY_TYPE);
			if (dsResult != null)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91018", _langID));
				txtCarrierID.Text = string.Empty;
				fpCartLoadInfo.ActiveSheet.Rows.Clear();
				fpCartLoadInfo.Refresh();
				DataBindPlanInfo(isMSG: false);
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void RemovePartNoToCart()
	{
		DataTable carrierMap = null;
		DataSet dsResult = null;
		Dictionary<string, string> param = null;
		List<Dictionary<string, string>> parameters = null;
		try
		{
			parameters = new List<Dictionary<string, string>>();
			param = new Dictionary<string, string>();
			if (fpCartLoadInfo.ActiveSheet.Rows.Count == 0 || fpCartLoadInfo.ActiveSheet.ActiveRowIndex == -1)
			{
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("31435", _langID));
				return;
			}
			carrierMap = GetCurrentCarrierMapBIZ(_orgID, fpCartLoadInfo.ActiveSheet.GetValue("CARRIER_ID").ToString(), fpCartLoadInfo.ActiveSheet.GetValue("ITEM_CODE").ToString()).Tables["OUT_DATA"];
			if (carrierMap != null)
			{
				param.Add("I_ORG_ID", _orgID);
				param.Add("I_START_END", C_START_END);
				param.Add("I_CARRIER_ID", carrierMap.Rows[0]["CARRIER_ID"].ToString());
				param.Add("I_SHEET_ID", carrierMap.Rows[0]["SHEET_ID"].ToString());
				param.Add("I_LOCATOR", carrierMap.Rows[0]["CARRIER_LOCATOR"].ToString());
				param.Add("I_GUBUN", string.Empty);
				param.Add("I_POSITION", string.Empty);
				param.Add("I_ITEM_CODE", carrierMap.Rows[0]["ITEM_CODE"].ToString());
				param.Add("I_ITEM_SN", string.Empty);
				param.Add("I_QTY", "0");
				param.Add("I_USER_ID", _userID);
				param.Add("I_TXNDATE", string.Empty);
				param.Add("I_TXN_FROM", C_TXN_FROM);
				param.Add("I_WO_NAME", string.Empty);
				param.Add("I_TO_WO_NAME", string.Empty);
				param.Add("I_COMMENTS", string.Empty);
				param.Add("I_INOUT_TYPE", InOutType.MANUAL_OUT);
				param.Add("I_TXN_UNIT", TxnUnit.SHEET);
				param.Add("I_TXN_ID", carrierMap.Rows[0]["SHEET_ID"].ToString());
				parameters.Add(param);
				ProcMsgResult(MessageNoticeType.MainPanel, GetMessage("91018", _langID));
				dsResult = SetCarrierUnMapBIZ(parameters);
				DataBindCartLoadInfo(fpCartLoadInfo.ActiveSheet.GetValue("CARRIER_ID").ToString(), isMSG: false);
				DataBindPlanInfo(isMSG: false);
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private void ProcMsgResult(MessageNoticeType noticetype, Exception ex)
	{
		Dictionary<string, object> exResult = null;
		Dictionary<string, string> resultMSG = null;
		try
		{
			exResult = exceptionResult(ex, isLogHist: false);
			resultMSG = exResult["RESULT_MSG"] as Dictionary<string, string>;
			ProcMsgResult(noticetype, resultMSG["RTN_VALUE"].ToString());
		}
		catch (Exception ex2)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex2.Message);
		}
	}

	private void ProcMsgResult(MessageNoticeType noticetype, string msg)
	{
		try
		{
			switch (noticetype)
			{
			case MessageNoticeType.MainPanel:
				txtMsg.Text = msg;
				break;
			case MessageNoticeType.Dialog:
				MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				break;
			case MessageNoticeType.Both:
				txtMsg.Text = msg;
				MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				break;
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
		finally
		{
			if (_tmrMsgClear == null)
			{
			}
		}
	}

	public void GetRequestInfo(string locator)
	{
		DataSet ds = null;
		string scanType = string.Empty;
		string scanStr = string.Empty;
		try
		{
			ds = GetRequestByLocatorBIZ(_orgID, locator);
			if (ds.Tables["OUT_DATA"].Rows.Count == 1)
			{
				txtAGVIn.Text = ds.Tables["OUT_DATA"].Rows[0]["MESSAGE"].ToString();
			}
			else if (ds.Tables["OUT_DATA"].Rows.Count == 2)
			{
				txtAGVIn.Text = ds.Tables["OUT_DATA"].Rows[0]["MESSAGE"].ToString();
				txtAGVOut.Text = ds.Tables["OUT_DATA"].Rows[1]["MESSAGE"].ToString();
			}
			else if (ds.Tables["OUT_DATA"] == null || ds.Tables["OUT_DATA"].Rows.Count == 0)
			{
				txtAGVIn.Text = string.Empty;
				txtAGVOut.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	public void GetMessageInfo(string langID)
	{
		DataSet ds = null;
		try
		{
			ds = GetMessageBIZ(langID);
			if (ds != null)
			{
				MessageList = ds.Tables["OUT_DATA"];
			}
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex.Message);
		}
	}

	private Dictionary<string, object> exceptionResult(Exception ex, bool isLogHist)
	{
		Dictionary<string, object> resultView = null;
		Dictionary<string, string> result = null;
		DataSet ds = null;
		try
		{
			resultView = new Dictionary<string, object>();
			result = new Dictionary<string, string>();
			if (isLogHist && ex.Message.Contains("I_ERROR_MSG"))
			{
				ds = setErrorLogHist(ex.Message);
				if (string.IsNullOrEmpty(GetMessage(ds.Tables["OUT_DATA"].Rows[0]["I_ERROR_MSG"].ToString(), _langID)))
				{
					result.Add(ReturnMSG.RTN_VALUE, ds.Tables["OUT_DATA"].Rows[0]["I_ERROR_MSG"].ToString());
				}
				else
				{
					result.Add(ReturnMSG.RTN_VALUE, GetMessage(ds.Tables["OUT_DATA"].Rows[0]["I_ERROR_MSG"].ToString(), _langID));
				}
			}
			else if (!isLogHist && ex.Message.Contains("I_ERROR_MSG"))
			{
				result.Add(ReturnMSG.RTN_VALUE, GetMessage("90001", _langID));
			}
			else
			{
				result.Add(ReturnMSG.RTN_VALUE, GetMessage(ex, _langID));
			}
			result.Add(ReturnMSG.RTN_CODE, MessageResult.NG);
		}
		catch (Exception)
		{
			result.Add(ReturnMSG.RTN_CODE, MessageResult.NG);
			result.Add(ReturnMSG.RTN_VALUE, GetMessage("90001", _langID));
		}
		finally
		{
			resultView.Add(ReturnMSG.RESULT_MSG, result);
		}
		return resultView;
	}

	public string GetMessage(string langKey, string langID)
	{
		if (MessageList.Rows.Count > 0)
		{
			string filter = " ( LANG_ID= '" + langID + "' OR  LANG_ID = 'en-US'  )  AND  LANG_KEY = '" + langKey + "'";
			DataRow[] row = MessageList.Select(filter);
			if (row.Count() == 1)
			{
				return row[0]["LANG_DESC"].ToString();
			}
			if (row.Count() > 0)
			{
				return (row.Where((DataRow item) => item["LANG_ID"].ToString().Equals(langID)).ToList()[0]["LANG_DESC"] ?? "").ToString().Trim();
			}
			return string.Empty;
		}
		return string.Empty;
	}

	public string GetMessage(string langKey, string langID, params object[] values)
	{
		if (MessageList.Rows.Count > 0)
		{
			string filter = " ( LANG_ID= '" + langID + "' OR  LANG_ID = 'en-US'  )  AND  LANG_KEY = '" + langKey + "'";
			DataRow[] row = MessageList.Select(filter);
			if (row.Count() == 1)
			{
				string str = row[0]["LANG_DESC"].ToString();
				for (int num = 0; num < values.Count(); num++)
				{
					str = str.Replace("%" + (num + 1), values[num].ToString());
				}
				return str;
			}
			if (row.Count() > 0)
			{
				string str = (row.Where((DataRow item) => item["LANG_ID"].ToString().Equals(langID)).ToList()[0]["LANG_DESC"] ?? "").ToString().Trim();
				for (int num = 0; num < values.Count(); num++)
				{
					str = str.Replace("%" + (num + 1), values[num].ToString());
				}
				return str;
			}
			return string.Empty;
		}
		return string.Empty;
	}

	public string GetMessage(Exception ex, string langID)
	{
		try
		{
			string ExBizRuleID = (ex.Data.Contains("BIZ") ? ex.Data["BIZ"].ToString() : null);
			string ExCode = (ex.Data.Contains("CODE") ? ex.Data["CODE"].ToString() : null);
			string ExData = ex.Message;
			string[] ExPara = (ex.Data.Contains("PARA") ? (ex.Data["PARA"] ?? "").ToString().Split(':') : null);
			if (MessageList.Rows.Count > 0)
			{
				IEnumerable<DataRow> Query = from DataRow c in MessageList.Rows
					where c["LANG_KEY"].ToString() == ExCode
					select c;
				string filter = " LANG_KEY = '" + ExCode + "'";
				DataRow[] msgResult = MessageList.Select(filter);
				if (msgResult.Count() == 1)
				{
					string returnMsg = msgResult[0]["LANG_DESC"].ToString();
					for (int i = 0; i < ExPara.Count(); i++)
					{
						returnMsg = returnMsg.Replace("%" + (i + 1), ExPara[i]);
					}
					return returnMsg;
				}
				if (msgResult.Count() > 1)
				{
					IEnumerable<DataRow> msg = msgResult.Where((DataRow item) => item["LANG_ID"].ToString().Equals(langID));
					string returnMsg2 = string.Empty;
					returnMsg2 = ((msg.Count() != 0) ? (msgResult.Where((DataRow item) => item["LANG_ID"].ToString().Equals(langID)).ToList()[0]["LANG_DESC"] ?? "").ToString().Trim() : (msgResult.Where((DataRow item) => item["LANG_ID"].ToString().Equals("en-US")).ToList()[0]["LANG_DESC"] ?? "").ToString().Trim());
					for (int j = 0; j < ExPara.Count(); j++)
					{
						returnMsg2 = returnMsg2.Replace("%" + (j + 1), ExPara[j]);
					}
					return returnMsg2;
				}
				if (Debugger.IsAttached)
				{
					return ExData;
				}
				if (ExCode == null)
				{
					return ExData;
				}
				return ExData;
			}
			if (ExCode == null)
			{
				return ExData;
			}
			return "[" + ExCode + "] : " + ExData;
		}
		catch
		{
			return string.Empty;
		}
	}

	private DataSet GetCartLoadInfoBIZ(string orgID, string carrierID)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CARRIER_ID", carrierID);
			dsResult = new BizService().ExecBizRule("GMCS_GET_DECANT_LOAD_INFO_BY_CARRIER_ID", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet GetPartNoInfoBIZ(string orgID, string locator)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LOCATOR", locator);
			dsResult = new BizService().ExecBizRule("GMCS_GET_ITEM_LOCATOR_INFO", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet GetLocatorInfoBIZ(string orgID)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "TN_DECANT_ONLY", "Y");
			dsResult = new BizService().ExecBizRule("GMCS_GET_LOCATOR_INFO", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet GetPlanInfoBIZ(string orgID, string toLocator, string partNo, string toLineCode, string minDefQty)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LOCATOR", toLocator);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ITEM_CODE", partNo);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LINE_CODE", toLineCode);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "MIN_DEF_QTY", minDefQty);
			dsResult = new BizService().ExecBizRule("GMCS_GET_DECANT_SUPPLY_INFO_BY_PARTNO", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet GetItemMcsInfo(string orgID, string ItemCode)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ITEM_CODE", ItemCode);
			dsResult = new BizService().ExecBizRule("GMCS_GET_ITEM_INFO", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet GetDecantToLocatorBIZ(string orgID, string locator)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LOCATOR", locator);
			dsResult = new BizService().ExecBizRule("GMCS_GET_DECANT_TO_LOCATOR", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	public DataSet getChkScanTypeBIZ(string orgID, string langID, string scanData, string SheetInfoFromDB, string carrierInfoFromDB, string locatorInfoFromDB)
	{
		DataSet dsInData = null;
		DataSet dsResult = null;
		try
		{
			dsInData = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "LANGID", langID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "SCAN_STR", scanData);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "SHEET_INFO_FROM_DB", SheetInfoFromDB);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "CARRIER_INFO_FROM_DB", carrierInfoFromDB);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "LOCATOR_INFO_FROM_DB", locatorInfoFromDB);
			dsResult = new BizService().ExecBizRule("GMCS_GET_SCAN_INFO", dsInData, "IN_DATA", "OUT_DATA,LOCATOR_INFO,CURRENT_CARRIER_MAP,ALL_SHEET_INFO,ITEM_INFO,LOCATOR_DETAIL,LOCATOR_CARRIER_INFO,ITEM_SN_INFO");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		finally
		{
		}
		return dsResult;
	}

	public DataSet ProcCarrierCallBIZ(string orgID, string locator, string carrierID, string userID, string supplyType)
	{
		DataSet dsInData = null;
		DataSet dsResult = null;
		try
		{
			dsInData = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_LOCATOR", locator);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_CARRIER_ID", carrierID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_USER_ID", userID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_SUPPLY_TYPE", supplyType);
			dsResult = new BizService().ExecBizRule("GMCS_SET_CREATE_REQ_CARRY_MULTI", dsInData, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		finally
		{
		}
		return dsResult;
	}

	public DataSet SetCarrierProdBIZ(List<Dictionary<string, string>> argCarrierProd)
	{
		DataSet InData = null;
		DataSet dsResult = null;
		try
		{
			InData = new DataSet();
			foreach (Dictionary<string, string> item in argCarrierProd)
			{
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_ORG_ID", item["I_ORG_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_START_END", item["I_START_END"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_CARRIER_ID", item["I_CARRIER_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_SHEET_ID", item["I_SHEET_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_LOCATOR", item["I_LOCATOR"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_GUBUN", item["I_GUBUN"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_POSITION", item["I_POSITION"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_ITEM_CODE", item["I_ITEM_CODE"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_ITEM_SN", item["I_ITEM_SN"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_QTY", item["I_QTY"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_USER_ID", item["I_USER_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_TXNDATE", item["I_TXNDATE"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_TXN_FROM", item["I_TXN_FROM"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_WO_NAME", item["I_WO_NAME"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_TO_WO_NAME", item["I_TO_WO_NAME"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "I_COMMENTS", item["I_COMMENTS"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "LANGID", item["LANGID"]);
			}
			dsResult = new BizService().ExecBizRule("GMCS_SET_CARRIER_UNMAP", InData, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		finally
		{
		}
		return dsResult;
	}

	private DataSet GetCurrentCarrierMapBIZ(string orgID, string carrierID, string partNo)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CARRIER_ID", carrierID);
			if (!string.IsNullOrEmpty(partNo))
			{
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ITEM_CODE", partNo);
			}
			dsResult = new BizService().ExecBizRule("GMCS_GET_CURRENT_CARRIER_MAP", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet SetCarrierUnMapBIZ(List<Dictionary<string, string>> argCarrierProd)
	{
		DataSet dsInData = null;
		DataSet dsResult = null;
		try
		{
			dsInData = new DataSet();
			foreach (Dictionary<string, string> item in argCarrierProd)
			{
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_ORG_ID", item["I_ORG_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_START_END", item["I_START_END"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_CARRIER_ID", item["I_CARRIER_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_SHEET_ID", item["I_SHEET_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_LOCATOR", item["I_LOCATOR"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_GUBUN", item["I_GUBUN"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_POSITION", item["I_POSITION"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_ITEM_CODE", item["I_ITEM_CODE"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_ITEM_SN", item["I_ITEM_SN"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_QTY", item["I_QTY"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_USER_ID", item["I_USER_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_TXNDATE", item["I_TXNDATE"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_TXN_FROM", item["I_TXN_FROM"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_WO_NAME", item["I_WO_NAME"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_TO_WO_NAME", item["I_TO_WO_NAME"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_COMMENTS", item["I_COMMENTS"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_INOUT_TYPE", item["I_INOUT_TYPE"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_TXN_UNIT", item["I_TXN_UNIT"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_TXN_ID", item["I_TXN_ID"]);
			}
			dsResult = new BizService().ExecBizRule("GMCS_SET_CARRIER_UNMAP", dsInData, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet SetCarrierManualCsMultiBIZ(List<Dictionary<string, string>> argCarrierProd)
	{
		DataSet InData = null;
		DataSet dsResult = null;
		try
		{
			InData = new DataSet();
			foreach (Dictionary<string, string> item in argCarrierProd)
			{
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_WO_DATA", "ORG_ID", item["ORG_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_WO_DATA", "WOID", item["WOID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_WO_DATA", "ITEM_CODE", item["ITEM_CODE"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_WO_DATA", "QTY", item["QTY"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_WO_DATA", "ITEM_SN", item["ITEM_SN"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "ORG_ID", item["ORG_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "CARRIER_ID", item["CARRIER_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "LOCATOR", item["LOCATOR"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "MAKE_REMOVE", item["MAKE_REMOVE"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "ITEM_CODE", item["ITEM_CODE"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "QTY", item["QTY"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "USER_ID", item["USER_ID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "LANGID", item["LANGID"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "ORDER_FLAG", item["ORDER_FLAG"]);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "EQSG_ID", item["EQSG_ID"]);
			}
			dsResult = new BizService().ExecBizRule("GMCS_SET_CARRIER_PROD_DECANT_MULTI", InData, "IN_DATA,IN_WO_DATA", "OUT_DATA,OUT_SHEET_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet GetRequestByLocatorBIZ(string orgID, string locator)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LOCATOR", locator);
			dsResult = new BizService().ExecBizRule("GMCS_GET_REQUEST_BY_LOCATOR", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private DataSet GetMessageBIZ(string langID)
	{
		DataSet InData = null;
		DataSet dsResult = null;
		try
		{
			InData = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "LANG_ID", langID);
			dsResult = new BizService().ExecBizRule("GMCS_GET_LANG_DICT", InData, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	public DataSet setErrorLogHist(string rtnMsg)
	{
		DataSet dsInData = null;
		DataSet dsResult = null;
		try
		{
			dsInData = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "I_RTN_MSG", rtnMsg);
			dsResult = new BizService().ExecBizRule("GMCS_SET_ERROR_LOG_HIST", dsInData, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	public DataSet GetManualCarrierMakeBIZ(string orgID, string locator, string carrierID)
	{
		DataSet InData = null;
		DataSet dsResult = null;
		try
		{
			InData = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "LOCATOR", locator);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref InData, "IN_DATA", "CARRIER_ID", carrierID);
			dsResult = new BizService().ExecBizRule("GMCS_GET_MANUAL_CARRIER_MAKE", InData, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ProcMsgResult(MessageNoticeType.MainPanel, ex);
		}
		return dsResult;
	}

	private void cboPartNo_SelectionChangeCommitted(object sender, EventArgs e)
	{
	}

	private void cboPartNo_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmDecantMakeCart));
		FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer2 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer3 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer4 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		this.panelOnly1 = new MCS.Common.PanelOnly();
		this.label8 = new System.Windows.Forms.Label();
		this.btnPlan = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.btnComplete = new System.Windows.Forms.Button();
		this.btnDel = new System.Windows.Forms.Button();
		this.btnEmptyAll = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.numLoadQty = new System.Windows.Forms.NumericUpDown();
		this.fpCartLoadInfo = new MCS.Common.FpSpread();
		this.txtCarrierID = new System.Windows.Forms.TextBox();
		this.btnNumCh = new System.Windows.Forms.Button();
		this.btnLoadQty = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnSearch = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.numExceptQty = new System.Windows.Forms.NumericUpDown();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.fpPlanInfo = new MCS.Common.FpSpread();
		this.cboPartNo = new MCS.Common.ComboBox();
		this.cboLocator = new MCS.Common.ComboBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.txtMsg = new System.Windows.Forms.TextBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAGVOut = new System.Windows.Forms.TextBox();
		this.txtAGVIn = new System.Windows.Forms.TextBox();
		this.panelOnly1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numLoadQty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.fpCartLoadInfo).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.numExceptQty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.fpPlanInfo).BeginInit();
		this.panel5.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.panelOnly1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelOnly1.BackColor = System.Drawing.SystemColors.Control;
		this.panelOnly1.BackgroundImage = (System.Drawing.Image)resources.GetObject("panelOnly1.BackgroundImage");
		this.panelOnly1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.panelOnly1.Controls.Add(this.label8);
		this.panelOnly1.Controls.Add(this.btnPlan);
		this.panelOnly1.Location = new System.Drawing.Point(2, 3);
		this.panelOnly1.Name = "panelOnly1";
		this.panelOnly1.Padding = new System.Windows.Forms.Padding(8);
		this.panelOnly1.Size = new System.Drawing.Size(1247, 52);
		this.panelOnly1.TabIndex = 81;
		this.label8.BackColor = System.Drawing.SystemColors.Control;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(9, 7);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(219, 38);
		this.label8.TabIndex = 79;
		this.label8.Text = "Load cart (Decant)";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPlan.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnPlan.BackColor = System.Drawing.Color.Black;
		this.btnPlan.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.btnPlan.ForeColor = System.Drawing.Color.FromArgb(86, 136, 186);
		this.btnPlan.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPlan.Location = new System.Drawing.Point(1093, 2);
		this.btnPlan.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnPlan.Name = "btnPlan";
		this.btnPlan.Size = new System.Drawing.Size(149, 47);
		this.btnPlan.TabIndex = 116;
		this.btnPlan.Text = "Prod Status";
		this.btnPlan.UseVisualStyleBackColor = false;
		this.btnPlan.Click += new System.EventHandler(btnPlan_Click);
		this.panel2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.panel2.BackColor = System.Drawing.SystemColors.Control;
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.btnComplete);
		this.panel2.Controls.Add(this.btnDel);
		this.panel2.Controls.Add(this.btnEmptyAll);
		this.panel2.Controls.Add(this.btnAdd);
		this.panel2.Controls.Add(this.numLoadQty);
		this.panel2.Controls.Add(this.fpCartLoadInfo);
		this.panel2.Controls.Add(this.txtCarrierID);
		this.panel2.Controls.Add(this.btnNumCh);
		this.panel2.Location = new System.Drawing.Point(824, 56);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(425, 516);
		this.panel2.TabIndex = 82;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.FromArgb(238, 238, 238);
		this.label6.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(9, 81);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(165, 40);
		this.label6.TabIndex = 119;
		this.label6.Text = "Load Qty";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.FromArgb(238, 238, 238);
		this.label5.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(45, 15);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(129, 40);
		this.label5.TabIndex = 118;
		this.label5.Text = "Cart ID";
		this.btnComplete.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnComplete.BackColor = System.Drawing.Color.DarkRed;
		this.btnComplete.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.btnComplete.ForeColor = System.Drawing.Color.White;
		this.btnComplete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnComplete.Location = new System.Drawing.Point(232, 402);
		this.btnComplete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnComplete.Name = "btnComplete";
		this.btnComplete.Size = new System.Drawing.Size(160, 104);
		this.btnComplete.TabIndex = 87;
		this.btnComplete.Text = "AGV\r\nCall";
		this.btnComplete.UseVisualStyleBackColor = false;
		this.btnComplete.Click += new System.EventHandler(btnComplete_Click);
		this.btnDel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnDel.BackColor = System.Drawing.Color.DarkRed;
		this.btnDel.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.btnDel.ForeColor = System.Drawing.Color.White;
		this.btnDel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnDel.Location = new System.Drawing.Point(232, 137);
		this.btnDel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnDel.Name = "btnDel";
		this.btnDel.Size = new System.Drawing.Size(160, 104);
		this.btnDel.TabIndex = 115;
		this.btnDel.Text = "Cart\r\nDel(-)";
		this.btnDel.UseVisualStyleBackColor = false;
		this.btnDel.Click += new System.EventHandler(btnDel_Click);
		this.btnEmptyAll.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
		this.btnEmptyAll.BackColor = System.Drawing.Color.DarkRed;
		this.btnEmptyAll.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.btnEmptyAll.ForeColor = System.Drawing.Color.White;
		this.btnEmptyAll.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnEmptyAll.Location = new System.Drawing.Point(40, 402);
		this.btnEmptyAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnEmptyAll.Name = "btnEmptyAll";
		this.btnEmptyAll.Size = new System.Drawing.Size(160, 104);
		this.btnEmptyAll.TabIndex = 86;
		this.btnEmptyAll.Text = "Empty\r\nAll";
		this.btnEmptyAll.UseVisualStyleBackColor = false;
		this.btnEmptyAll.Click += new System.EventHandler(btnEmptyAll_Click);
		this.btnAdd.BackColor = System.Drawing.Color.DarkRed;
		this.btnAdd.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnAdd.Location = new System.Drawing.Point(40, 136);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(165, 104);
		this.btnAdd.TabIndex = 89;
		this.btnAdd.Text = "Cart\r\nLoad(+)";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.numLoadQty.Font = new System.Drawing.Font("Arial", 30f, System.Drawing.FontStyle.Bold);
		this.numLoadQty.Location = new System.Drawing.Point(178, 75);
		this.numLoadQty.Maximum = new decimal(new int[4] { 10000, 0, 0, 0 });
		this.numLoadQty.Name = "numLoadQty";
		this.numLoadQty.Size = new System.Drawing.Size(110, 53);
		this.numLoadQty.TabIndex = 116;
		this.numLoadQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.numLoadQty.Value = new decimal(new int[4] { 100, 0, 0, 0 });
		this.fpCartLoadInfo.AccessibleDescription = "";
		this.fpCartLoadInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.fpCartLoadInfo.AutoSizeColumnWidth = true;
		this.fpCartLoadInfo.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpCartLoadInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpCartLoadInfo.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpCartLoadInfo.EnableSort = false;
		this.fpCartLoadInfo.FocusRenderer = defaultFocusIndicatorRenderer2;
		this.fpCartLoadInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpCartLoadInfo.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpCartLoadInfo.HorizontalScrollBar.Name = "";
		this.fpCartLoadInfo.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
		this.fpCartLoadInfo.HorizontalScrollBar.TabIndex = 0;
		this.fpCartLoadInfo.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpCartLoadInfo.Location = new System.Drawing.Point(4, 248);
		this.fpCartLoadInfo.Name = "fpCartLoadInfo";
		this.fpCartLoadInfo.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpCartLoadInfo.Size = new System.Drawing.Size(417, 146);
		this.fpCartLoadInfo.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpCartLoadInfo.TabIndex = 106;
		this.fpCartLoadInfo.TextTipDelay = 1000;
		this.fpCartLoadInfo.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpCartLoadInfo.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpCartLoadInfo.VerticalScrollBar.Name = "";
		this.fpCartLoadInfo.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
		this.fpCartLoadInfo.VerticalScrollBar.TabIndex = 0;
		this.fpCartLoadInfo.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpCartLoadInfo.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.txtCarrierID.BackColor = System.Drawing.Color.Black;
		this.txtCarrierID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtCarrierID.Font = new System.Drawing.Font("Arial", 30f, System.Drawing.FontStyle.Bold);
		this.txtCarrierID.ForeColor = System.Drawing.Color.FromArgb(235, 222, 0);
		this.txtCarrierID.Location = new System.Drawing.Point(176, 9);
		this.txtCarrierID.Name = "txtCarrierID";
		this.txtCarrierID.Size = new System.Drawing.Size(243, 53);
		this.txtCarrierID.TabIndex = 88;
		this.txtCarrierID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.txtCarrierID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCarrierID_KeyPress);
		this.btnNumCh.BackColor = System.Drawing.Color.Black;
		this.btnNumCh.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.btnNumCh.ForeColor = System.Drawing.Color.White;
		this.btnNumCh.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnNumCh.Location = new System.Drawing.Point(292, 69);
		this.btnNumCh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnNumCh.Name = "btnNumCh";
		this.btnNumCh.Size = new System.Drawing.Size(127, 64);
		this.btnNumCh.TabIndex = 86;
		this.btnNumCh.Text = "Num";
		this.btnNumCh.UseVisualStyleBackColor = false;
		this.btnNumCh.Click += new System.EventHandler(btnNumCh_Click);
		this.btnLoadQty.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnLoadQty.BackColor = System.Drawing.Color.DarkRed;
		this.btnLoadQty.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.btnLoadQty.ForeColor = System.Drawing.Color.White;
		this.btnLoadQty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnLoadQty.Location = new System.Drawing.Point(655, 69);
		this.btnLoadQty.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnLoadQty.Name = "btnLoadQty";
		this.btnLoadQty.Size = new System.Drawing.Size(161, 64);
		this.btnLoadQty.TabIndex = 117;
		this.btnLoadQty.Text = "Allocate";
		this.btnLoadQty.UseVisualStyleBackColor = false;
		this.btnLoadQty.Click += new System.EventHandler(btnLoadQty_Click);
		this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel3.BackColor = System.Drawing.SystemColors.Control;
		this.panel3.Controls.Add(this.btnSearch);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.numExceptQty);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.btnLoadQty);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.fpPlanInfo);
		this.panel3.Controls.Add(this.cboPartNo);
		this.panel3.Controls.Add(this.cboLocator);
		this.panel3.Font = new System.Drawing.Font("Arial", 9f);
		this.panel3.Location = new System.Drawing.Point(2, 56);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(821, 516);
		this.panel3.TabIndex = 114;
		this.btnSearch.BackColor = System.Drawing.Color.DarkRed;
		this.btnSearch.Font = new System.Drawing.Font("Arial", 11f, System.Drawing.FontStyle.Bold);
		this.btnSearch.ForeColor = System.Drawing.Color.White;
		this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSearch.Location = new System.Drawing.Point(294, 137);
		this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(99, 38);
		this.btnSearch.TabIndex = 122;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = false;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.FromArgb(238, 238, 238);
		this.label7.Font = new System.Drawing.Font("Arial", 13f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(23, 146);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(143, 21);
		this.label7.TabIndex = 121;
		this.label7.Text = "Except Min Qty";
		this.numExceptQty.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.numExceptQty.Location = new System.Drawing.Point(175, 141);
		this.numExceptQty.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		this.numExceptQty.Name = "numExceptQty";
		this.numExceptQty.Size = new System.Drawing.Size(110, 30);
		this.numExceptQty.TabIndex = 120;
		this.numExceptQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.numExceptQty.Value = new decimal(new int[4] { 3, 0, 0, 0 });
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.FromArgb(238, 238, 238);
		this.label2.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(24, 81);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(139, 40);
		this.label2.TabIndex = 102;
		this.label2.Text = "Part No";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.FromArgb(238, 238, 238);
		this.label3.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(20, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(143, 40);
		this.label3.TabIndex = 102;
		this.label3.Text = "Locator";
		this.fpPlanInfo.AccessibleDescription = "";
		this.fpPlanInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.fpPlanInfo.AutoSizeColumnWidth = true;
		this.fpPlanInfo.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpPlanInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpPlanInfo.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPlanInfo.EnableSort = false;
		this.fpPlanInfo.FocusRenderer = defaultFocusIndicatorRenderer2;
		this.fpPlanInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpPlanInfo.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPlanInfo.HorizontalScrollBar.Name = "";
		this.fpPlanInfo.HorizontalScrollBar.Renderer = defaultScrollBarRenderer3;
		this.fpPlanInfo.HorizontalScrollBar.TabIndex = 0;
		this.fpPlanInfo.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPlanInfo.Location = new System.Drawing.Point(7, 178);
		this.fpPlanInfo.Name = "fpPlanInfo";
		this.fpPlanInfo.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPlanInfo.Size = new System.Drawing.Size(809, 335);
		this.fpPlanInfo.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpPlanInfo.TabIndex = 105;
		this.fpPlanInfo.TextTipDelay = 1000;
		this.fpPlanInfo.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpPlanInfo.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPlanInfo.VerticalScrollBar.Name = "";
		this.fpPlanInfo.VerticalScrollBar.Renderer = defaultScrollBarRenderer4;
		this.fpPlanInfo.VerticalScrollBar.TabIndex = 0;
		this.fpPlanInfo.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPlanInfo.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.cboPartNo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.cboPartNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboPartNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboPartNo.BackColor = System.Drawing.Color.White;
		this.cboPartNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboPartNo.Font = new System.Drawing.Font("Arial", 30f);
		this.cboPartNo.ForeColor = System.Drawing.Color.Black;
		this.cboPartNo.FormattingEnabled = true;
		this.cboPartNo.Location = new System.Drawing.Point(169, 75);
		this.cboPartNo.Name = "cboPartNo";
		this.cboPartNo.Size = new System.Drawing.Size(482, 53);
		this.cboPartNo.TabIndex = 104;
		this.cboLocator.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.cboLocator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboLocator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboLocator.BackColor = System.Drawing.Color.White;
		this.cboLocator.DropDownHeight = 200;
		this.cboLocator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboLocator.Font = new System.Drawing.Font("Arial", 30f);
		this.cboLocator.ForeColor = System.Drawing.Color.Black;
		this.cboLocator.FormattingEnabled = true;
		this.cboLocator.IntegralHeight = false;
		this.cboLocator.ItemHeight = 45;
		this.cboLocator.Location = new System.Drawing.Point(169, 9);
		this.cboLocator.MaxDropDownItems = 5;
		this.cboLocator.Name = "cboLocator";
		this.cboLocator.Size = new System.Drawing.Size(647, 53);
		this.cboLocator.TabIndex = 103;
		this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel5.BackColor = System.Drawing.SystemColors.Control;
		this.panel5.Controls.Add(this.txtMsg);
		this.panel5.Location = new System.Drawing.Point(1, 682);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1248, 78);
		this.panel5.TabIndex = 117;
		this.txtMsg.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtMsg.BackColor = System.Drawing.SystemColors.InfoText;
		this.txtMsg.Font = new System.Drawing.Font("Arial", 35f, System.Drawing.FontStyle.Bold);
		this.txtMsg.ForeColor = System.Drawing.Color.FromArgb(235, 222, 0);
		this.txtMsg.Location = new System.Drawing.Point(4, 8);
		this.txtMsg.Name = "txtMsg";
		this.txtMsg.Size = new System.Drawing.Size(1239, 61);
		this.txtMsg.TabIndex = 89;
		this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel1.BackColor = System.Drawing.SystemColors.Control;
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.txtAGVOut);
		this.panel1.Controls.Add(this.txtAGVIn);
		this.panel1.Location = new System.Drawing.Point(1, 573);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1248, 107);
		this.panel1.TabIndex = 109;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.FromArgb(238, 238, 238);
		this.label4.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(16, 60);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(170, 40);
		this.label4.TabIndex = 104;
		this.label4.Text = "AGV OUT";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.FromArgb(238, 238, 238);
		this.label1.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(16, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(132, 40);
		this.label1.TabIndex = 103;
		this.label1.Text = "AGV IN";
		this.txtAGVOut.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtAGVOut.BackColor = System.Drawing.SystemColors.InfoText;
		this.txtAGVOut.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.txtAGVOut.ForeColor = System.Drawing.Color.FromArgb(235, 222, 0);
		this.txtAGVOut.Location = new System.Drawing.Point(221, 57);
		this.txtAGVOut.Name = "txtAGVOut";
		this.txtAGVOut.Size = new System.Drawing.Size(1022, 46);
		this.txtAGVOut.TabIndex = 91;
		this.txtAGVIn.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtAGVIn.BackColor = System.Drawing.SystemColors.InfoText;
		this.txtAGVIn.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.txtAGVIn.ForeColor = System.Drawing.Color.FromArgb(235, 222, 0);
		this.txtAGVIn.Location = new System.Drawing.Point(221, 7);
		this.txtAGVIn.Name = "txtAGVIn";
		this.txtAGVIn.Size = new System.Drawing.Size(1022, 46);
		this.txtAGVIn.TabIndex = 90;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(1251, 761);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panelOnly1);
		this.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.DoubleBuffered = true;
		this.Font = new System.Drawing.Font("Arial", 9f);
		base.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
		base.Name = "frmDecantMakeCart";
		this.Text = "MCS Load Cart";
		base.Title = "MCS Decant Make Cart";
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmDecantMakeCart_FormClosed);
		base.Load += new System.EventHandler(frmDecantMakeCart_Load);
		this.panelOnly1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numLoadQty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.fpCartLoadInfo).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.numExceptQty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.fpPlanInfo).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
