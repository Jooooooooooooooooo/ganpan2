using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FarPoint.Win;
using FarPoint.Win.Spread;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;
using MCS.PrintBoard.Properties;

namespace MCS.PrintBoard.PrintBoard;

public class frmOutsideIssueList : frmBase
{
	private static DataTable dtGroupBy = new DataTable();

	private static DataTable dtTemp = new DataTable();

	private string _fromDate = string.Empty;

	private string _toDate = string.Empty;

	private string _printYN = string.Empty;

	private string __IssueID = string.Empty;

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

	private string _orgID = string.Empty;

	private string _userID = string.Empty;

	private string _langID = string.Empty;

	private int back_row = 0;

	private string NowDate = string.Empty;

	private IContainer components = null;

	private SearchPanel searchPanel1;

	public System.Windows.Forms.Timer tmRefresh;

	private DataGridView dgvBuffer;

	private System.Windows.Forms.Timer tmDemo;

	private BackPanel2 backPanel21;

	private SplitContainer splitContainerMain;

	private SplitContainer splitContainer1;

	private SplitContainer splitContainer2;

	private SplitContainer splitContainer3;

	private Panel panel2;

	private Panel panel1;

	private Label label9;

	private DateTimePicker dtpToDate;

	private Label label11;

	private DateTimePicker dtpFromDate;

	private Label label10;

	private Panel panel3;

	private Panel panel4;

	private MCS.Common.ComboBox cboPrintYN;

	private System.Windows.Forms.Button btnNew;

	private System.Windows.Forms.Button btnPrint;

	private Label label1;

	private System.Windows.Forms.Button btnSave;

	private System.Windows.Forms.Button btnDelete;

	private System.Windows.Forms.Button btnSearch;

	private TextBox txtCarrierID;

	private System.Windows.Forms.Button btnNew2;

	private Label label2;

	private MCS.Common.FpSpread fpSearchMain;

	private Label label3;

	private MCS.Common.FpSpread fpSearchDetail;

	private void frmMain_Load(object sender, EventArgs e)
	{
		try
		{
			dtpFromDate.Value = DateTime.Now.AddDays(-1.0);
			dtpToDate.Value = DateTime.Now;
			Dictionary<string, string> combo = new Dictionary<string, string>();
			combo.Add("ALL", null);
			combo.Add("Y", "Y");
			combo.Add("N", "N");
			cboPrintYN.DataSource = new BindingSource(combo, null);
			cboPrintYN.DisplayMember = "Key";
			cboPrintYN.ValueMember = "Value";
			procMakeSheetColumn();
			GetdtTemp();
			btnPrint.Enabled = false;
			btnNew2.Enabled = false;
			btnDelete.Enabled = false;
			btnSave.Enabled = false;
			InitControls();
			_orgID = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "ORG_ID", "202351");
			_langID = Thread.CurrentThread.CurrentCulture.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InitControls()
	{
		try
		{
			if (FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "LOCATOR", string.Empty) != string.Empty)
			{
			}
			if (!Variables.scannerPort.PortOpen(Variables.ScanTypeItem.Serial, SerialReceiveEvent))
			{
				MessageBox.Show(" Connect the scanner. ");
			}
		}
		catch (Exception)
		{
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

	public void GetScanInfo(string scanData, string SheetInfoFromDB, string carrierInfoFromDB, string locatorInfoFromDB)
	{
		DataSet ds = null;
		string scanType = string.Empty;
		string scanStr = string.Empty;
		string IssueID = string.Empty;
		string _orgID = "202351";
		try
		{
			ds = getChkScanTypeBIZ(_orgID, _langID, scanData);
			if (ds != null && ds.Tables["OUT_DATA"].Rows.Count != 0)
			{
				for (int i = 0; fpSearchDetail.ActiveSheet.Rows.Count > i; i++)
				{
					string A = ds.Tables["OUT_DATA"].Rows[0]["SHEET_ID"].ToString();
					if (A == fpSearchDetail.ActiveSheet.GetText(i, fpSearchDetail.ActiveSheet.GetColumnIndex("SHEET_ID")))
					{
						throw new Exception("Duplicate SHEET ID.");
					}
				}
				fpSearchDetail.ActiveSheet.AddRows(0, 1);
				fpSearchDetail.ActiveSheet.SetText(0, "ORG_ID", ds.Tables["OUT_DATA"].Rows[0]["ORG_ID"].ToString());
				fpSearchDetail.ActiveSheet.SetText(0, "ISSUELIST_ID", __IssueID);
				fpSearchDetail.ActiveSheet.SetText(0, "SHEET_ID", ds.Tables["OUT_DATA"].Rows[0]["SHEET_ID"].ToString());
				fpSearchDetail.ActiveSheet.SetText(0, "ITEM_CODE", ds.Tables["OUT_DATA"].Rows[0]["ITEM_CODE"].ToString());
				fpSearchDetail.ActiveSheet.SetText(0, "ITEM_DESCRIPTION", ds.Tables["OUT_DATA"].Rows[0]["ITEM_DESC"].ToString());
				fpSearchDetail.ActiveSheet.SetText(0, "ACTUAL_QTY", ds.Tables["OUT_DATA"].Rows[0]["TOTAL_QTY"].ToString());
				fpSearchDetail.ActiveSheet.SetText(0, "UNIT", ds.Tables["OUT_DATA"].Rows[0]["ITEM_UNIT"].ToString());
				fpSearchDetail.ActiveSheet.SetText(0, "STATUS", "MER");
				return;
			}
			throw new Exception("No Sheet Data!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public DataSet getChkScanTypeBIZ(string orgID, string langID, string scanData)
	{
		DataSet dsInData = null;
		DataSet dsResult = null;
		try
		{
			dsInData = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "SCAN_STR", scanData);
			string aaa = FuncEtc.DataSetToXmlString(dsInData);
			dsResult = new BizService().ExecBizRule("GMCS_GET_SCAN_SHEET_MA", dsInData, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
		}
		return dsResult;
	}

	private void procMakeSheetColumn()
	{
		try
		{
			MCS.Common.SheetView svSearchMain = new MCS.Common.SheetView(fpSearchMain, "Search", OperationMode.Normal, bRowHeaderVisible: true);
			MCS.Common.SheetView svSearchDetail = new MCS.Common.SheetView(fpSearchDetail, "Detail", OperationMode.Normal, bRowHeaderVisible: true);
			svSearchMain.AddColumnText("", "CHK", 40, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 500);
			svSearchMain.AddColumnText("Org", "ORG_ID", 300, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 500);
			svSearchMain.AddColumnText("Issue List ID", "ISSUELIST_ID", 400, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 200);
			svSearchMain.AddColumnText("Issue Locator", "START_LOCATOR", 200, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 30);
			svSearchMain.AddColumnText("Arrive Locator", "ARRIVE_LOCATOR", 200, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 50);
			svSearchMain.AddColumnText("PRINT", "PRINT_YN", 200, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 30);
			svSearchMain.AddColumnText("Created Date", "CREATED_DATE", 350, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 120);
			svSearchMain.AddColumnText("Updated Date", "UPDATED_DATE", 350, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 500);
			svSearchMain.AddColumnText("Status", "STATUS", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 500);
			svSearchDetail.AddColumnCheckBox("", "CHK", 40, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, "", "", "", bThreeState: false);
			svSearchDetail.AddColumnText("Sheet ID", "SHEET_ID", 300, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 200);
			svSearchDetail.AddColumnText("Part Number", "ITEM_CODE", 180, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 30);
			svSearchDetail.AddColumnText("Item Description", "ITEM_DESCRIPTION", 300, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 50);
			svSearchDetail.AddColumnText("Required Qty", "REQUIRED_QTY", 150, CellHorizontalAlignment.Right, bLocked: true, bVisible: true, 50);
			svSearchDetail.AddColumnText("Unit", "UNIT", 150, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 30);
			svSearchDetail.AddColumnText("Actual Qty", "ACTUAL_QTY", 150, CellHorizontalAlignment.Right, bLocked: true, bVisible: true, 30);
			svSearchDetail.AddColumnText("Remark", "REMARK", 400, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 120);
			svSearchDetail.AddColumnText("Updaed Date", "UPDATED_DATE", 300, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 120);
			svSearchDetail.AddColumnText("Creaed Date", "CREATED_DATE", 80, CellHorizontalAlignment.Right, bLocked: true, bVisible: false, 120);
			svSearchDetail.AddColumnText("Org", "ORG_ID", 140, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 200);
			svSearchDetail.AddColumnText("Issue List ID", "ISSUELIST_ID", 140, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 200);
			svSearchDetail.AddColumnText("Status", "STATUS", 140, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 10);
			svSearchMain.RowHeader.Visible = true;
			svSearchMain.Rows.Default.Height = 35f;
			svSearchMain.ColumnHeader.Rows[0].Height = 35f;
			svSearchDetail.Rows.Default.Height = 35f;
			svSearchDetail.ColumnHeader.Rows[0].Height = 35f;
			svSearchMain.Columns["ISSUELIST_ID"].Locked = true;
			fpSearchDetail.DataSource = fpSearchDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public frmOutsideIssueList()
	{
		InitializeComponent();
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		for (int i = 0; fpSearchDetail.ActiveSheet.Rows.Count > i; i++)
		{
			if (fpSearchDetail.ActiveSheet.GetText(i, fpSearchDetail.ActiveSheet.GetColumnIndex("STATUS")) == "MER")
			{
				MessageBox.Show("Please Save first.");
				return;
			}
		}
		dtGroupBy = (DataTable)fpSearchDetail.ActiveSheet.DataSource;
		SetdtTemp();
		SetPrint();
		try
		{
			frmOutsideIssuePreview frm = new frmOutsideIssuePreview();
			frm.dtGroupBy = dtTemp;
			dtGroupBy.DefaultView.Sort = "ITEM_CODE";
			frm.ShowDialog();
		}
		catch
		{
		}
	}

	private void btnSearch_Click(object sender, EventArgs e)
	{
		back_row = 0;
		btnPrint.Enabled = false;
		btnNew2.Enabled = false;
		btnDelete.Enabled = false;
		btnSave.Enabled = false;
		GetIssuemaster();
		btnNew.Enabled = true;
	}

	private void GetIssuemaster()
	{
		string _fromDate = dtpFromDate.Value.ToString("yyyyMMdd");
		string _toDate = dtpToDate.Value.ToString("yyyyMMdd");
		if (cboPrintYN.SelectedValue.ToString() == "")
		{
			_printYN = null;
		}
		else
		{
			_printYN = cboPrintYN.SelectedValue.ToString();
		}
		fpSearchDetail.ActiveSheet.Rows.Clear();
		GetOutsideIssueBIZ(_fromDate, _toDate, _printYN);
	}

	private DataSet GetOutsideIssueBIZ(string _fromDate, string _toDate, string _prinYN)
	{
		DataSet dsResult = null;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", "202351");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "FROM_DATE", _fromDate);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "TO_DATE", _toDate);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "PRINT_YN", _prinYN);
			dsResult = new BizService().ExecBizRule("SEL_OUTSIDE_ISSUE_LIST", ds, "IN_DATA", "OUT_DATA");
			fpSearchMain.DataSource = dsResult.Tables["OUT_DATA"];
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return dsResult;
	}

	private DataSet GetOutsideIssueDetailBIZ(string _fromDate, string _toDate, string _IssueID)
	{
		DataSet dsResult = null;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "FROM_DATE", _fromDate);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "TO_DATE", _toDate);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ISSUELIST_ID", _IssueID);
			dsResult = new BizService().ExecBizRule("SEL_OUTSIDE_ISSUE_LIST_DETAIL", ds, "IN_DATA", "OUT_DATA");
			fpSearchDetail.DataSource = dsResult.Tables["OUT_DATA"];
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return dsResult;
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		btnNew.Enabled = false;
		if (fpSearchMain.ActiveSheet.Rows.Count > 0)
		{
			fpSearchMain.ActiveSheet.SetText(back_row, "CHK", "");
			fpSearchMain.ActiveSheet.Rows[back_row].BackColor = Color.White;
			fpSearchMain.ActiveSheet.Rows[back_row].ForeColor = Color.Black;
		}
		NowDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").ToString();
		fpSearchDetail.ActiveSheet.Rows.Clear();
		fpSearchMain.ActiveSheet.Rows.Add(0, 1);
		fpSearchMain.ActiveSheet.SetText(0, "CHK", "●");
		fpSearchMain.ActiveSheet.Rows[0].BackColor = Color.Green;
		fpSearchMain.ActiveSheet.Rows[0].ForeColor = Color.White;
		fpSearchMain.ActiveSheet.SetText(0, "CREATED_DATE", NowDate);
		fpSearchMain.ActiveSheet.SetText(0, "UPDATED_DATE", NowDate);
		fpSearchMain.ActiveSheet.SetText(0, "PRINT_YN", "N");
		fpSearchMain.ActiveSheet.SetText(0, "STATUS", "NEW");
		GetNewIssueNo("202351");
		back_row = 0;
		btnNew2.Enabled = true;
		btnDelete.Enabled = true;
		btnPrint.Enabled = false;
	}

	private DataSet GetNewIssueNo(string _orgID)
	{
		DataSet dsResult = null;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", _orgID);
			dsResult = new BizService().ExecBizRule("GMCS_GET_ISSUELIST_DOCNUM_MA", ds, "IN_DATA", "OUT_DATA");
			string NewIssueID = dsResult.Tables["OUT_DATA"].Rows[0]["ISSUELIST_DOCNUM"].ToString();
			fpSearchMain.ActiveSheet.SetText(0, "ISSUELIST_ID", NewIssueID);
			__IssueID = NewIssueID;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		return dsResult;
	}

	private void GetdtTemp()
	{
		dtTemp.Clear();
		dtTemp.Reset();
		dtTemp.Columns.Add("GROUPID");
		dtTemp.Columns.Add("ORG_ID");
		dtTemp.Columns.Add("SYSTIME");
		dtTemp.Columns.Add("ISSUELIST_ID");
		dtTemp.Columns.Add("SHEET_ID");
		dtTemp.Columns.Add("LP1");
		dtTemp.Columns.Add("ITEM_CODE1");
		dtTemp.Columns.Add("ITEM_DESCRIPTION1");
		dtTemp.Columns.Add("REQUIRED_QTY1");
		dtTemp.Columns.Add("UNIT1");
		dtTemp.Columns.Add("ACTUAL_QTY1");
		dtTemp.Columns.Add("REMARK1");
		dtTemp.Columns.Add("LP2");
		dtTemp.Columns.Add("ITEM_CODE2");
		dtTemp.Columns.Add("ITEM_DESCRIPTION2");
		dtTemp.Columns.Add("REQUIRED_QTY2");
		dtTemp.Columns.Add("UNIT2");
		dtTemp.Columns.Add("ACTUAL_QTY2");
		dtTemp.Columns.Add("REMARK2");
		dtTemp.Columns.Add("LP3");
		dtTemp.Columns.Add("ITEM_CODE3");
		dtTemp.Columns.Add("ITEM_DESCRIPTION3");
		dtTemp.Columns.Add("REQUIRED_QTY3");
		dtTemp.Columns.Add("UNIT3");
		dtTemp.Columns.Add("ACTUAL_QTY3");
		dtTemp.Columns.Add("REMARK3");
		dtTemp.Columns.Add("LP4");
		dtTemp.Columns.Add("ITEM_CODE4");
		dtTemp.Columns.Add("ITEM_DESCRIPTION4");
		dtTemp.Columns.Add("REQUIRED_QTY4");
		dtTemp.Columns.Add("UNIT4");
		dtTemp.Columns.Add("ACTUAL_QTY4");
		dtTemp.Columns.Add("REMARK4");
		dtTemp.Columns.Add("LP5");
		dtTemp.Columns.Add("ITEM_CODE5");
		dtTemp.Columns.Add("ITEM_DESCRIPTION5");
		dtTemp.Columns.Add("REQUIRED_QTY5");
		dtTemp.Columns.Add("UNIT5");
		dtTemp.Columns.Add("ACTUAL_QTY5");
		dtTemp.Columns.Add("REMARK5");
		dtTemp.Columns.Add("LP6");
		dtTemp.Columns.Add("ITEM_CODE6");
		dtTemp.Columns.Add("ITEM_DESCRIPTION6");
		dtTemp.Columns.Add("REQUIRED_QTY6");
		dtTemp.Columns.Add("UNIT6");
		dtTemp.Columns.Add("ACTUAL_QTY6");
		dtTemp.Columns.Add("REMARK6");
		dtTemp.Columns.Add("LP7");
		dtTemp.Columns.Add("ITEM_CODE7");
		dtTemp.Columns.Add("ITEM_DESCRIPTION7");
		dtTemp.Columns.Add("REQUIRED_QTY7");
		dtTemp.Columns.Add("UNIT7");
		dtTemp.Columns.Add("ACTUAL_QTY7");
		dtTemp.Columns.Add("REMARK7");
		dtTemp.Columns.Add("LP8");
		dtTemp.Columns.Add("ITEM_CODE8");
		dtTemp.Columns.Add("ITEM_DESCRIPTION8");
		dtTemp.Columns.Add("REQUIRED_QTY8");
		dtTemp.Columns.Add("UNIT8");
		dtTemp.Columns.Add("ACTUAL_QTY8");
		dtTemp.Columns.Add("REMARK8");
		dtTemp.Columns.Add("LP9");
		dtTemp.Columns.Add("ITEM_CODE9");
		dtTemp.Columns.Add("ITEM_DESCRIPTION9");
		dtTemp.Columns.Add("REQUIRED_QTY9");
		dtTemp.Columns.Add("UNIT9");
		dtTemp.Columns.Add("ACTUAL_QTY9");
		dtTemp.Columns.Add("REMARK9");
		dtTemp.Columns.Add("LP10");
		dtTemp.Columns.Add("ITEM_CODE10");
		dtTemp.Columns.Add("ITEM_DESCRIPTION10");
		dtTemp.Columns.Add("REQUIRED_QTY10");
		dtTemp.Columns.Add("UNIT10");
		dtTemp.Columns.Add("ACTUAL_QTY10");
		dtTemp.Columns.Add("REMARK10");
		dtTemp.Columns.Add("LP11");
		dtTemp.Columns.Add("ITEM_CODE11");
		dtTemp.Columns.Add("ITEM_DESCRIPTION11");
		dtTemp.Columns.Add("REQUIRED_QTY11");
		dtTemp.Columns.Add("UNIT11");
		dtTemp.Columns.Add("ACTUAL_QTY11");
		dtTemp.Columns.Add("REMARK11");
		dtTemp.Columns.Add("LP12");
		dtTemp.Columns.Add("ITEM_CODE12");
		dtTemp.Columns.Add("ITEM_DESCRIPTION12");
		dtTemp.Columns.Add("REQUIRED_QTY12");
		dtTemp.Columns.Add("UNIT12");
		dtTemp.Columns.Add("ACTUAL_QTY12");
		dtTemp.Columns.Add("REMARK12");
	}

	private void SetdtTemp()
	{
		int sREQUIRED_QTY = 0;
		int sACTUAL_QTY = 0;
		string sREMARK = string.Empty;
		dtTemp.Clear();
		DataRow dr = dtTemp.NewRow();
		DataView view = dtGroupBy.DefaultView;
		view.Sort = "ITEM_CODE,ITEM_DESCRIPTION,UNIT";
		DataTable dtSort = view.ToTable();
		dr["ISSUELIST_ID"] = dtSort.Rows[0]["ISSUELIST_ID"].ToString();
		dr["SYSTIME"] = DateTime.Now.ToString("yyyy.MM.dd.HH:mm:ss").ToString();
		dr["SHEET_ID"] = "";
		dtTemp.Rows.Add(dr);
		for (int i = 0; 1 > i; i++)
		{
			int cnt = 1;
			for (int j = 0; dtSort.Rows.Count > j; j++)
			{
				if (12 <= cnt)
				{
					break;
				}
				int sbREQUIRED_QTY = sREQUIRED_QTY;
				sREQUIRED_QTY = dtSort.Rows[j]["REQUIRED_QTY"].SafeToInt32();
				int sbACTUAL_QTY = sACTUAL_QTY;
				sACTUAL_QTY = int.Parse(dtSort.Rows[j]["ACTUAL_QTY"].ToString());
				string sbREMARK = sREMARK;
				sREMARK = dtSort.Rows[j]["REMARK"].ToString();
				if (j >= 1 && dtSort.Rows[j - 1]["ITEM_CODE"].ToString() != dtSort.Rows[j]["ITEM_CODE"].ToString())
				{
					sREQUIRED_QTY = dtSort.Rows[j]["REQUIRED_QTY"].SafeToInt32();
					sACTUAL_QTY = int.Parse(dtSort.Rows[j]["ACTUAL_QTY"].ToString());
					sREMARK = dtSort.Rows[j]["REMARK"].ToString();
					cnt++;
				}
				else if (j != 0)
				{
					sREQUIRED_QTY = dtSort.Rows[j]["REQUIRED_QTY"].SafeToInt32() + sbREQUIRED_QTY;
					sACTUAL_QTY = int.Parse(dtSort.Rows[j]["ACTUAL_QTY"].ToString()) + sbACTUAL_QTY;
					sREMARK = sbREMARK + "  " + dtSort.Rows[j]["REMARK"].ToString();
				}
				string sITEM_CODE = dtSort.Rows[j]["ITEM_CODE"].ToString();
				string sITEM_DESCRIPTION = dtSort.Rows[j]["ITEM_DESCRIPTION"].ToString();
				string sUNIT = dtSort.Rows[j]["UNIT"].ToString();
				string sLP = cnt.ToString();
				string LP_Name = "LP" + cnt;
				string ITEM_CODE_Name = "ITEM_CODE" + cnt;
				string ITEM_DESCRIPTION_Name = "ITEM_DESCRIPTION" + cnt;
				string REQUIRED_QTY_Name = "REQUIRED_QTY" + cnt;
				string UNIT_Name = "UNIT" + cnt;
				string ACTUAL_QTY_Name = "ACTUAL_QTY" + cnt;
				string REMARK_Name = "REMARK" + cnt;
				dtTemp.Rows[i][LP_Name] = sLP;
				dtTemp.Rows[i][ITEM_CODE_Name] = sITEM_CODE;
				dtTemp.Rows[i][ITEM_DESCRIPTION_Name] = sITEM_DESCRIPTION;
				dtTemp.Rows[i][REQUIRED_QTY_Name] = sREQUIRED_QTY;
				dtTemp.Rows[i][UNIT_Name] = sUNIT;
				dtTemp.Rows[i][ACTUAL_QTY_Name] = sACTUAL_QTY;
				dtTemp.Rows[i][REMARK_Name] = sREMARK;
			}
		}
	}

	private string SetPrint()
	{
		string sTxnID = string.Empty;
		string OrgID = string.Empty;
		string IssueID = string.Empty;
		string SheetID = string.Empty;
		string ItemID = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			OrgID = fpSearchDetail.ActiveSheet.GetText(0, fpSearchDetail.ActiveSheet.GetColumnIndex("ORG_ID"));
			IssueID = fpSearchDetail.ActiveSheet.GetText(0, fpSearchDetail.ActiveSheet.GetColumnIndex("ISSUELIST_ID"));
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", OrgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ISSUELIST_ID", IssueID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "PRINT_YN", "Y");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "UPDATED_BY", "CS");
			DataSet dsResult = bizServer.ExecBizRule("UPD_SHEET_PRINT_YN", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			sTxnID = "";
		}
		return sTxnID;
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		int chkCount = 0;
		try
		{
			for (int i = 0; fpSearchDetail.ActiveSheet.Rows.Count > i; i++)
			{
				if (fpSearchDetail.ActiveSheet.GetText(i, "CHK") == "True")
				{
					chkCount++;
				}
			}
			if (chkCount == 0)
			{
				throw new Exception("Please select a target to delete");
			}
			DelIssue();
			if (fpSearchDetail.ActiveSheet.Rows.Count == 0)
			{
				btnPrint.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void txtCarrierID_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (e.KeyChar == '\r' && !string.IsNullOrWhiteSpace(txtCarrierID.Text))
			{
				GetScanInfo(txtCarrierID.Text, string.Empty, string.Empty, string.Empty);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private string DelIssue()
	{
		string sTxnID = string.Empty;
		string OrgID = string.Empty;
		string IssueID = string.Empty;
		string SheetID = string.Empty;
		string ItemID = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			for (int i = fpSearchDetail.ActiveSheet.RowCount - 1; i > -1; i--)
			{
				if (fpSearchDetail.ActiveSheet.GetText(i, fpSearchDetail.ActiveSheet.GetColumnIndex("CHK")) == "True")
				{
					if (fpSearchDetail.ActiveSheet.GetText(i, "STATUS") == "MER" && fpSearchDetail.ActiveSheet.GetText(i, "SHEET_ID") == "")
					{
						fpSearchDetail.ActiveSheet.RemoveRows(i, 1);
					}
					else
					{
						OrgID = fpSearchDetail.ActiveSheet.GetText(i, fpSearchDetail.ActiveSheet.GetColumnIndex("ORG_ID"));
						IssueID = fpSearchDetail.ActiveSheet.GetText(i, fpSearchDetail.ActiveSheet.GetColumnIndex("ISSUELIST_ID"));
						SheetID = fpSearchDetail.ActiveSheet.GetText(i, fpSearchDetail.ActiveSheet.GetColumnIndex("SHEET_ID"));
						ItemID = fpSearchDetail.ActiveSheet.GetText(i, fpSearchDetail.ActiveSheet.GetColumnIndex("ITEM_CODE"));
						LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DIV", "DEL");
						LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", OrgID);
						LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ISSUELIST_ID", IssueID);
						LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_ID", SheetID);
						LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ITEM_CODE", ItemID);
						fpSearchDetail.ActiveSheet.RemoveRows(i, 1);
					}
				}
			}
			if (ds.Tables.Count > 0)
			{
				DataSet dsResult = bizServer.ExecBizRule("GMCS_SET_OUTSIDE_ISSUE_LIST", ds, "IN_DATA", "OUT_DATA");
				if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
				{
					sTxnID = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_MSG"].ToString();
					MessageBox.Show("Delete Complete");
				}
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			sTxnID = "";
		}
		return sTxnID;
	}

	private void fpSearchDetail_EditChange(object sender, EditorNotifyEventArgs e)
	{
		fpSearchDetail.ActiveSheet.SetText(e.Row, "STATUS", "MER");
		btnDelete.Enabled = true;
		btnSave.Enabled = true;
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		int MERcnt = 0;
		int NGcnt = 0;
		string NGSheetID = string.Empty;
		try
		{
			if (fpSearchDetail.ActiveSheet.Rows.Count == 0)
			{
				MessageBox.Show("Insert New Data");
				return;
			}
			for (int j = 0; j < fpSearchDetail.ActiveSheet.Rows.Count; j++)
			{
				for (int k = j + 1; k < fpSearchDetail.ActiveSheet.Rows.Count && k != fpSearchDetail.ActiveSheet.Rows.Count; k++)
				{
					if (fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("SHEET_ID")) == fpSearchDetail.ActiveSheet.GetText(k, fpSearchDetail.ActiveSheet.GetColumnIndex("SHEET_ID")))
					{
						MessageBox.Show("Duplicate Sheet ID!   SheetID : " + fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("SHEET_ID")));
						return;
					}
				}
				if (fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("STATUS")) == "MER")
				{
					if (fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("SHEET_ID")) == "")
					{
						MessageBox.Show("Insert Sheet ID!   row : " + (j + 1));
						return;
					}
					if (fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("ITEM_CODE")) == "")
					{
						MessageBox.Show("Insert Part Number!   row : " + (j + 1));
						return;
					}
					if (fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("ACTUAL_QTY")) == "")
					{
						MessageBox.Show(" Insert Actual Qty!   row : " + (j + 1));
						return;
					}
					MERcnt++;
				}
			}
			if (MERcnt == 0)
			{
				MessageBox.Show("No data changed.");
				return;
			}
			DataSet dsOQC = getChkOQCBlockBIZ();
			for (int i = 0; i < dsOQC.Tables["OUT_DATA"].Rows.Count; i++)
			{
				if (dsOQC.Tables["OUT_DATA"].Rows[0]["OQC_CHECK_RESULT"].ToString() == "NG")
				{
					fpSearchDetail.ActiveSheet.Rows[i].BackColor = Color.Red;
					NGSheetID = NGSheetID + fpSearchDetail.ActiveSheet.GetText(i, fpSearchDetail.ActiveSheet.GetColumnIndex("SHEET_ID")) + "   ";
					NGcnt++;
				}
			}
			if (NGcnt > 0)
			{
				throw new Exception("This sheet is blocked because Quality Issue. \r\n Sheet ID = " + NGSheetID);
			}
			setIssue();
			btnNew2.Enabled = true;
			btnNew.Enabled = true;
			btnDelete.Enabled = true;
			btnPrint.Enabled = true;
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private string setIssue()
	{
		string sTxnID = string.Empty;
		string OrgID = string.Empty;
		string IssueID = string.Empty;
		string SheetID = string.Empty;
		string ItemID = string.Empty;
		string ItemDesc = string.Empty;
		string RequiredQty = string.Empty;
		string IUnit = string.Empty;
		string Remark = string.Empty;
		string ActualQty = string.Empty;
		string StartLocator = string.Empty;
		string ArriveLocator = string.Empty;
		string Status = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			for (int j = 0; j < fpSearchDetail.ActiveSheet.RowCount; j++)
			{
				if (fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("STATUS")) == "MER")
				{
					OrgID = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("ORG_ID"));
					IssueID = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("ISSUELIST_ID"));
					SheetID = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("SHEET_ID"));
					ItemID = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("ITEM_CODE"));
					ItemDesc = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("ITEM_DESCRIPTION"));
					RequiredQty = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("REQUIRED_QTY"));
					IUnit = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("UNIT"));
					ActualQty = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("ACTUAL_QTY"));
					Remark = fpSearchDetail.ActiveSheet.GetText(j, fpSearchDetail.ActiveSheet.GetColumnIndex("REMARK"));
					StartLocator = fpSearchMain.ActiveSheet.GetText(back_row, fpSearchMain.ActiveSheet.GetColumnIndex("START_LOCATOR"));
					ArriveLocator = fpSearchMain.ActiveSheet.GetText(back_row, fpSearchMain.ActiveSheet.GetColumnIndex("ARRIVE_LOCATOR"));
					Status = fpSearchMain.ActiveSheet.GetText(back_row, fpSearchMain.ActiveSheet.GetColumnIndex("STATUS"));
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DIV", "MER");
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", OrgID);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ISSUELIST_ID", IssueID);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_ID", SheetID);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ITEM_CODE", ItemID);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ITEM_DESCRIPTION", ItemDesc);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "REQUIRED_QTY", RequiredQty);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "UNIT", IUnit);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ACTUAL_QTY", ActualQty);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "REMARK", Remark);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "START_LOCATOR", StartLocator);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ARRIVE_LOCATOR", ArriveLocator);
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CREATED_BY", "CS");
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "UPDATED_BY", "CS");
					LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "STATUS", Status);
				}
			}
			string aaa = FuncEtc.DataSetToXmlString(ds);
			DataSet dsResult = bizServer.ExecBizRule("GMCS_SET_OUTSIDE_ISSUE_LIST", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				sTxnID = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_MSG"].ToString();
				MessageBox.Show(" Save Complete ");
				for (int i = 0; fpSearchDetail.ActiveSheet.Rows.Count > i; i++)
				{
					fpSearchDetail.ActiveSheet.Rows[i].BackColor = Color.White;
				}
				fpSearchMain.ActiveSheet.SetText(0, "STATUS", "");
				string _fromDate = dtpFromDate.Value.ToString("yyyyMMdd");
				string _toDate = dtpToDate.Value.ToString("yyyyMMdd");
				string _IssueID = fpSearchMain.ActiveSheet.GetText(back_row, fpSearchMain.ActiveSheet.GetColumnIndex("ISSUELIST_ID"));
				GetOutsideIssueDetailBIZ(_fromDate, _toDate, _IssueID);
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			sTxnID = "";
		}
		return sTxnID;
	}

	private void btnNew2_Click(object sender, EventArgs e)
	{
		fpSearchDetail.ActiveSheet.Rows.Add(0, 1);
		fpSearchDetail.ActiveSheet.SetText(0, "ORG_ID", "202351");
		fpSearchDetail.ActiveSheet.SetText(0, "ISSUELIST_ID", __IssueID);
		fpSearchDetail.ActiveSheet.SetText(0, "STATUS", "MER");
	}

	private void fpSearchMain_EditChange(object sender, EditorNotifyEventArgs e)
	{
	}

	private void fpSearchMain_CellClick(object sender, CellClickEventArgs e)
	{
		fpSearchMain.ActiveSheet.SetText(back_row, "CHK", "");
		fpSearchMain.ActiveSheet.Rows[back_row].BackColor = Color.White;
		fpSearchMain.ActiveSheet.Rows[back_row].ForeColor = Color.Black;
		fpSearchMain.ActiveSheet.SetText(e.Row, "CHK", "●");
		fpSearchMain.ActiveSheet.Rows[e.Row].BackColor = Color.Green;
		fpSearchMain.ActiveSheet.Rows[e.Row].ForeColor = Color.White;
		string _fromDate = dtpFromDate.Value.ToString("yyyyMMdd");
		string _toDate = dtpToDate.Value.ToString("yyyyMMdd");
		string _IssueID = fpSearchMain.ActiveSheet.GetText(e.Row, fpSearchMain.ActiveSheet.GetColumnIndex("ISSUELIST_ID"));
		GetOutsideIssueDetailBIZ(_fromDate, _toDate, _IssueID);
		__IssueID = fpSearchMain.ActiveSheet.GetText(e.Row, fpSearchMain.ActiveSheet.GetColumnIndex("ISSUELIST_ID"));
		btnNew2.Enabled = true;
		btnDelete.Enabled = true;
		btnSave.Enabled = false;
		if (fpSearchDetail.ActiveSheet.Rows.Count == 0)
		{
			btnPrint.Enabled = false;
		}
		else
		{
			btnPrint.Enabled = true;
		}
		back_row = e.Row;
	}

	public DataSet getChkOQCBlockBIZ()
	{
		DataSet dsInData = null;
		DataSet dsResult = null;
		string orgID = "202351";
		string SheetID = string.Empty;
		try
		{
			dsInData = new DataSet();
			BizService bizServer = new BizService();
			for (int i = 0; fpSearchDetail.ActiveSheet.Rows.Count > i; i++)
			{
				SheetID = fpSearchDetail.ActiveSheet.GetText(i, "SHEET_ID");
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "ORG_ID", orgID);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInData, "IN_DATA", "SHEET_ID", SheetID);
			}
			string aaa = FuncEtc.DataSetToXmlString(dsInData);
			dsResult = new BizService().ExecBizRule("BR_GET_OQC_BLOCK_CHECK", dsInData, "IN_DATA", "OUT_DATA");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
		}
		return dsResult;
	}

	private void fpPrintMain_Resize(object sender, EventArgs e)
	{
		spreadColumnResize(fpSearchMain);
		spreadColumnResize(fpSearchDetail);
	}

	private void spreadColumnResize(MCS.Common.FpSpread fpSpread)
	{
		MCS.Common.FpSpread spread = null;
		try
		{
			spread = fpSpread;
			if (spread == null || spread.Width <= 100 || spread.ActiveSheet.Columns.Count <= 0)
			{
				return;
			}
			float spreadWidth = spread.Width;
			if (spread.ActiveSheet.RowHeader.Visible)
			{
				for (int k = 0; k < spread.ActiveSheet.RowHeader.Columns.Count; k++)
				{
					if (spread.ActiveSheet.RowHeader.Columns[k].Visible)
					{
						spreadWidth -= spread.ActiveSheet.RowHeader.Columns[k].Width;
					}
				}
			}
			if (spread.VerticalScrollBarPolicy == ScrollBarPolicy.AsNeeded || spread.VerticalScrollBarPolicy == ScrollBarPolicy.Always)
			{
				spreadWidth -= 17f;
			}
			spreadWidth *= 2f - spread.ActiveSheet.ZoomFactor;
			float totalWidth = 0f;
			for (int j = 0; j < spread.ActiveSheet.ColumnCount; j++)
			{
				if (spread.ActiveSheet.Columns[j].Visible)
				{
					totalWidth += spread.ActiveSheet.Columns[j].Width;
				}
			}
			if (!(totalWidth > 0f))
			{
				return;
			}
			int addWidth = 0;
			int lastVisibleColumnIndex = -1;
			for (int i = 0; i < spread.ActiveSheet.ColumnCount; i++)
			{
				if (spread.ActiveSheet.Columns[i].Visible)
				{
					lastVisibleColumnIndex = i;
					float width2 = spread.ActiveSheet.Columns[i].Width;
					if (width2 > 0f)
					{
						width2 = width2 / totalWidth * spreadWidth;
						spread.ActiveSheet.Columns[i].Width = (int)width2;
						addWidth += (int)width2;
					}
					else
					{
						spread.ActiveSheet.Columns[i].Width = 0f;
					}
				}
			}
			if (lastVisibleColumnIndex > -1)
			{
				int width = (int)spread.ActiveSheet.Columns[lastVisibleColumnIndex].Width + (int)spreadWidth - addWidth;
				if (width > 0)
				{
					spread.ActiveSheet.Columns[lastVisibleColumnIndex].Width = width;
				}
			}
		}
		catch (Exception)
		{
		}
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmOutsideIssueList));
		FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer3 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer4 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		this.searchPanel1 = new MCS.Common.SearchPanel();
		this.tmRefresh = new System.Windows.Forms.Timer(this.components);
		this.dgvBuffer = new System.Windows.Forms.DataGridView();
		this.tmDemo = new System.Windows.Forms.Timer(this.components);
		this.backPanel21 = new MCS.Common.BackPanel2();
		this.txtCarrierID = new System.Windows.Forms.TextBox();
		this.btnSearch = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.cboPrintYN = new MCS.Common.ComboBox();
		this.label9 = new System.Windows.Forms.Label();
		this.dtpToDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.splitContainerMain = new System.Windows.Forms.SplitContainer();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.splitContainer3 = new System.Windows.Forms.SplitContainer();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnNew = new System.Windows.Forms.Button();
		this.btnPrint = new System.Windows.Forms.Button();
		this.fpSearchMain = new MCS.Common.FpSpread();
		this.splitContainer2 = new System.Windows.Forms.SplitContainer();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnNew2 = new System.Windows.Forms.Button();
		this.btnSave = new System.Windows.Forms.Button();
		this.btnDelete = new System.Windows.Forms.Button();
		this.fpSearchDetail = new MCS.Common.FpSpread();
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).BeginInit();
		this.backPanel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).BeginInit();
		this.splitContainerMain.Panel1.SuspendLayout();
		this.splitContainerMain.Panel2.SuspendLayout();
		this.splitContainerMain.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer3).BeginInit();
		this.splitContainer3.Panel1.SuspendLayout();
		this.splitContainer3.Panel2.SuspendLayout();
		this.splitContainer3.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpSearchMain).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).BeginInit();
		this.splitContainer2.Panel1.SuspendLayout();
		this.splitContainer2.Panel2.SuspendLayout();
		this.splitContainer2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpSearchDetail).BeginInit();
		base.SuspendLayout();
		this.searchPanel1.BackColor = System.Drawing.Color.Red;
		this.searchPanel1.BackgroundImage = (System.Drawing.Image)resources.GetObject("searchPanel1.BackgroundImage");
		this.searchPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.searchPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.searchPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.searchPanel1.Location = new System.Drawing.Point(0, 0);
		this.searchPanel1.Name = "searchPanel1";
		this.searchPanel1.Padding = new System.Windows.Forms.Padding(8);
		this.searchPanel1.Size = new System.Drawing.Size(1251, 761);
		this.searchPanel1.TabIndex = 70;
		this.tmRefresh.Interval = 5000;
		this.dgvBuffer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvBuffer.Location = new System.Drawing.Point(819, 12);
		this.dgvBuffer.Name = "dgvBuffer";
		this.dgvBuffer.RowTemplate.Height = 23;
		this.dgvBuffer.Size = new System.Drawing.Size(121, 45);
		this.dgvBuffer.TabIndex = 71;
		this.dgvBuffer.Visible = false;
		this.backPanel21.BackColor = System.Drawing.Color.FromArgb(224, 224, 224, 224);
		this.backPanel21.BackgroundImage = MCS.PrintBoard.Properties.Resources.background;
		this.backPanel21.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.backPanel21.Controls.Add(this.txtCarrierID);
		this.backPanel21.Controls.Add(this.btnSearch);
		this.backPanel21.Controls.Add(this.label1);
		this.backPanel21.Controls.Add(this.cboPrintYN);
		this.backPanel21.Controls.Add(this.label9);
		this.backPanel21.Controls.Add(this.dtpToDate);
		this.backPanel21.Controls.Add(this.label11);
		this.backPanel21.Controls.Add(this.dtpFromDate);
		this.backPanel21.Controls.Add(this.label10);
		this.backPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.backPanel21.Location = new System.Drawing.Point(0, 0);
		this.backPanel21.Margin = new System.Windows.Forms.Padding(0);
		this.backPanel21.Name = "backPanel21";
		this.backPanel21.Padding = new System.Windows.Forms.Padding(0);
		this.backPanel21.Size = new System.Drawing.Size(1251, 71);
		this.backPanel21.TabIndex = 107;
		this.txtCarrierID.BackColor = System.Drawing.Color.Black;
		this.txtCarrierID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtCarrierID.Font = new System.Drawing.Font("Arial", 30f, System.Drawing.FontStyle.Bold);
		this.txtCarrierID.ForeColor = System.Drawing.Color.FromArgb(235, 222, 0);
		this.txtCarrierID.Location = new System.Drawing.Point(897, 10);
		this.txtCarrierID.Name = "txtCarrierID";
		this.txtCarrierID.Size = new System.Drawing.Size(243, 53);
		this.txtCarrierID.TabIndex = 89;
		this.txtCarrierID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.txtCarrierID.Visible = false;
		this.txtCarrierID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCarrierID_KeyPress);
		this.btnSearch.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnSearch.BackgroundImage");
		this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSearch.Font = new System.Drawing.Font("Arial", 9f);
		this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSearch.Location = new System.Drawing.Point(1146, 22);
		this.btnSearch.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(100, 30);
		this.btnSearch.TabIndex = 115;
		this.btnSearch.Text = "SEARCH";
		this.btnSearch.UseVisualStyleBackColor = true;
		this.btnSearch.Click += new System.EventHandler(btnSearch_Click);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(701, 34);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 16);
		this.label1.TabIndex = 119;
		this.label1.Text = "Print Y/N";
		this.cboPrintYN.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboPrintYN.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboPrintYN.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboPrintYN.FormattingEnabled = true;
		this.cboPrintYN.Location = new System.Drawing.Point(767, 32);
		this.cboPrintYN.Name = "cboPrintYN";
		this.cboPrintYN.Size = new System.Drawing.Size(64, 20);
		this.cboPrintYN.TabIndex = 118;
		this.label9.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label9.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(22, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(270, 38);
		this.label9.TabIndex = 115;
		this.label9.Text = "Outside Issue List";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.dtpToDate.CustomFormat = "";
		this.dtpToDate.Font = new System.Drawing.Font("Arial", 10f);
		this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpToDate.Location = new System.Drawing.Point(575, 32);
		this.dtpToDate.Name = "dtpToDate";
		this.dtpToDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.dtpToDate.Size = new System.Drawing.Size(102, 23);
		this.dtpToDate.TabIndex = 114;
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.Transparent;
		this.label11.Font = new System.Drawing.Font("Arial", 10f);
		this.label11.ForeColor = System.Drawing.Color.Black;
		this.label11.Location = new System.Drawing.Point(551, 34);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(16, 16);
		this.label11.TabIndex = 117;
		this.label11.Text = "~";
		this.dtpFromDate.CustomFormat = "";
		this.dtpFromDate.Font = new System.Drawing.Font("Arial", 10f);
		this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpFromDate.Location = new System.Drawing.Point(440, 32);
		this.dtpFromDate.Name = "dtpFromDate";
		this.dtpFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.dtpFromDate.Size = new System.Drawing.Size(102, 23);
		this.dtpFromDate.TabIndex = 113;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Transparent;
		this.label10.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Black;
		this.label10.Location = new System.Drawing.Point(318, 34);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(94, 16);
		this.label10.TabIndex = 116;
		this.label10.Text = "Updated Date";
		this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
		this.splitContainerMain.Name = "splitContainerMain";
		this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainerMain.Panel1.Controls.Add(this.backPanel21);
		this.splitContainerMain.Panel2.Controls.Add(this.splitContainer1);
		this.splitContainerMain.Size = new System.Drawing.Size(1251, 761);
		this.splitContainerMain.SplitterDistance = 71;
		this.splitContainerMain.SplitterWidth = 1;
		this.splitContainerMain.TabIndex = 72;
		this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
		this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
		this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
		this.splitContainer1.Size = new System.Drawing.Size(1251, 689);
		this.splitContainer1.SplitterDistance = 329;
		this.splitContainer1.SplitterWidth = 1;
		this.splitContainer1.TabIndex = 73;
		this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer3.Location = new System.Drawing.Point(0, 0);
		this.splitContainer3.Name = "splitContainer3";
		this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer3.Panel1.Controls.Add(this.panel1);
		this.splitContainer3.Panel2.Controls.Add(this.fpSearchMain);
		this.splitContainer3.Size = new System.Drawing.Size(1251, 329);
		this.splitContainer3.SplitterDistance = 30;
		this.splitContainer3.SplitterWidth = 1;
		this.splitContainer3.TabIndex = 115;
		this.panel1.BackColor = System.Drawing.Color.Transparent;
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Font = new System.Drawing.Font("Arial", 9f);
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1251, 30);
		this.panel1.TabIndex = 108;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("굴림", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 129);
		this.label2.Location = new System.Drawing.Point(13, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(140, 27);
		this.label2.TabIndex = 72;
		this.label2.Text = "Issue List";
		this.panel3.Controls.Add(this.btnNew);
		this.panel3.Controls.Add(this.btnPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel3.Location = new System.Drawing.Point(839, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(412, 30);
		this.panel3.TabIndex = 118;
		this.btnNew.BackgroundImage = MCS.PrintBoard.Properties.Resources.sbtnbg;
		this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnNew.Font = new System.Drawing.Font("Arial", 9f);
		this.btnNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnNew.Location = new System.Drawing.Point(307, 3);
		this.btnNew.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(100, 30);
		this.btnNew.TabIndex = 113;
		this.btnNew.Text = "NEW";
		this.btnNew.UseVisualStyleBackColor = true;
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPrint.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnPrint.BackgroundImage");
		this.btnPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnPrint.Font = new System.Drawing.Font("Arial", 9f);
		this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPrint.Location = new System.Drawing.Point(201, 3);
		this.btnPrint.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 30);
		this.btnPrint.TabIndex = 114;
		this.btnPrint.Text = "PRINT";
		this.btnPrint.UseVisualStyleBackColor = true;
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.fpSearchMain.AccessibleDescription = "";
		this.fpSearchMain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.fpSearchMain.AutoSizeColumnWidth = false;
		this.fpSearchMain.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpSearchMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpSearchMain.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpSearchMain.EnableSort = false;
		this.fpSearchMain.FocusRenderer = defaultFocusIndicatorRenderer1;
		this.fpSearchMain.Font = new System.Drawing.Font("맑은 고딕", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpSearchMain.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpSearchMain.HorizontalScrollBar.Name = "";
		this.fpSearchMain.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
		this.fpSearchMain.HorizontalScrollBar.TabIndex = 0;
		this.fpSearchMain.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpSearchMain.Location = new System.Drawing.Point(9, 6);
		this.fpSearchMain.Name = "fpSearchMain";
		this.fpSearchMain.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpSearchMain.Size = new System.Drawing.Size(1233, 291);
		this.fpSearchMain.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpSearchMain.TabIndex = 71;
		this.fpSearchMain.TextTipDelay = 1000;
		this.fpSearchMain.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpSearchMain.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpSearchMain.VerticalScrollBar.Name = "";
		this.fpSearchMain.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
		this.fpSearchMain.VerticalScrollBar.TabIndex = 0;
		this.fpSearchMain.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpSearchMain.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.fpSearchMain.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(fpSearchMain_CellClick);
		this.fpSearchMain.EditChange += new FarPoint.Win.Spread.EditorNotifyEventHandler(fpSearchMain_EditChange);
		this.fpSearchMain.Resize += new System.EventHandler(fpPrintMain_Resize);
		this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer2.Location = new System.Drawing.Point(0, 0);
		this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
		this.splitContainer2.Name = "splitContainer2";
		this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.splitContainer2.Panel1.Controls.Add(this.panel2);
		this.splitContainer2.Panel1MinSize = 27;
		this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
		this.splitContainer2.Panel2.Controls.Add(this.fpSearchDetail);
		this.splitContainer2.Panel2MinSize = 27;
		this.splitContainer2.Size = new System.Drawing.Size(1251, 359);
		this.splitContainer2.SplitterDistance = 35;
		this.splitContainer2.SplitterWidth = 1;
		this.splitContainer2.TabIndex = 76;
		this.panel2.BackColor = System.Drawing.Color.Transparent;
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Font = new System.Drawing.Font("Arial", 9f);
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1251, 35);
		this.panel2.TabIndex = 107;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("굴림", 20.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 129);
		this.label3.Location = new System.Drawing.Point(13, 7);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(226, 27);
		this.label3.TabIndex = 72;
		this.label3.Text = "Issue List Detail";
		this.panel4.Controls.Add(this.btnNew2);
		this.panel4.Controls.Add(this.btnSave);
		this.panel4.Controls.Add(this.btnDelete);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel4.Location = new System.Drawing.Point(917, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(334, 35);
		this.panel4.TabIndex = 119;
		this.btnNew2.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnNew2.BackgroundImage");
		this.btnNew2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnNew2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnNew2.Font = new System.Drawing.Font("Arial", 9f);
		this.btnNew2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnNew2.Location = new System.Drawing.Point(17, 0);
		this.btnNew2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnNew2.Name = "btnNew2";
		this.btnNew2.Size = new System.Drawing.Size(100, 30);
		this.btnNew2.TabIndex = 117;
		this.btnNew2.Text = "NEW";
		this.btnNew2.UseVisualStyleBackColor = true;
		this.btnNew2.Click += new System.EventHandler(btnNew2_Click);
		this.btnSave.BackgroundImage = MCS.PrintBoard.Properties.Resources.sbtnbg;
		this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSave.Font = new System.Drawing.Font("Arial", 9f);
		this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSave.Location = new System.Drawing.Point(229, 0);
		this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(100, 30);
		this.btnSave.TabIndex = 115;
		this.btnSave.Text = "SAVE";
		this.btnSave.UseVisualStyleBackColor = true;
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnDelete.BackgroundImage");
		this.btnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnDelete.Font = new System.Drawing.Font("Arial", 9f);
		this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnDelete.Location = new System.Drawing.Point(123, 0);
		this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(100, 30);
		this.btnDelete.TabIndex = 116;
		this.btnDelete.Text = "DELETE";
		this.btnDelete.UseVisualStyleBackColor = true;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.fpSearchDetail.AccessibleDescription = "";
		this.fpSearchDetail.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.fpSearchDetail.AutoSizeColumnWidth = false;
		this.fpSearchDetail.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpSearchDetail.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpSearchDetail.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpSearchDetail.EnableSort = false;
		this.fpSearchDetail.FocusRenderer = defaultFocusIndicatorRenderer1;
		this.fpSearchDetail.Font = new System.Drawing.Font("맑은 고딕", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpSearchDetail.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpSearchDetail.HorizontalScrollBar.Name = "";
		this.fpSearchDetail.HorizontalScrollBar.Renderer = defaultScrollBarRenderer3;
		this.fpSearchDetail.HorizontalScrollBar.TabIndex = 0;
		this.fpSearchDetail.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpSearchDetail.Location = new System.Drawing.Point(9, 4);
		this.fpSearchDetail.Name = "fpSearchDetail";
		this.fpSearchDetail.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpSearchDetail.Size = new System.Drawing.Size(1233, 314);
		this.fpSearchDetail.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpSearchDetail.TabIndex = 73;
		this.fpSearchDetail.TextTipDelay = 1000;
		this.fpSearchDetail.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpSearchDetail.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpSearchDetail.VerticalScrollBar.Name = "";
		this.fpSearchDetail.VerticalScrollBar.Renderer = defaultScrollBarRenderer4;
		this.fpSearchDetail.VerticalScrollBar.TabIndex = 0;
		this.fpSearchDetail.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpSearchDetail.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.fpSearchDetail.EditChange += new FarPoint.Win.Spread.EditorNotifyEventHandler(fpSearchDetail_EditChange);
		this.fpSearchDetail.Resize += new System.EventHandler(fpPrintMain_Resize);
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		this.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(1251, 761);
		base.Controls.Add(this.splitContainerMain);
		base.Controls.Add(this.searchPanel1);
		base.Controls.Add(this.dgvBuffer);
		base.Name = "frmOutsideIssueList";
		this.Text = "MCS Outside Issue List";
		base.Load += new System.EventHandler(frmMain_Load);
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).EndInit();
		this.backPanel21.ResumeLayout(false);
		this.backPanel21.PerformLayout();
		this.splitContainerMain.Panel1.ResumeLayout(false);
		this.splitContainerMain.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).EndInit();
		this.splitContainerMain.ResumeLayout(false);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.splitContainer3.Panel1.ResumeLayout(false);
		this.splitContainer3.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer3).EndInit();
		this.splitContainer3.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpSearchMain).EndInit();
		this.splitContainer2.Panel1.ResumeLayout(false);
		this.splitContainer2.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).EndInit();
		this.splitContainer2.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpSearchDetail).EndInit();
		base.ResumeLayout(false);
	}
}
