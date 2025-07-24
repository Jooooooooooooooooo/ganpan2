using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;
using MCS.Common.Controls;
using MCS.PrintBoard.Properties;

namespace MCS.PrintBoard.PrintBoard;

public class frmQtyChangePrint : frmBase
{
	private static DataTable dtGroupBy = new DataTable();

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

	private DataSet dsResultSheet = new DataSet();

	private DataSet dsOrg = null;

	private IContainer components = null;

	private SplitContainer splitContainer1;

	private SplitContainer splitContainerMain;

	private BackPanel2 backPanel21;

	private TextBox txtCarrierID;

	private System.Windows.Forms.Button btnSave;

	private Label label9;

	private System.Windows.Forms.Timer tmDemo;

	private DataGridView dgvBuffer;

	public System.Windows.Forms.Timer tmRefresh;

	private SearchPanel searchPanel1;

	private TextBox textBox9;

	private TextBox txtPartNo;

	private Label label12;

	private Label label11;

	private Label label13;

	private Label lblBoxQty;

	private Label lblPartNo;

	private TextBox txtPartName;

	private TableLayoutPanel tableLayoutPanel7;

	private TableLayoutPanel tableLayoutPanel8;

	private TableLayoutPanel tableLayoutPanel10;

	private Label label4;

	private TableLayoutPanel tableLayoutPanel11;

	private TableLayoutPanel tableLayoutPanel12;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label10;

	private Label label1;

	private TableLayoutPanel tableLayoutPanel13;

	private Label label3;

	private Label lblProdDate;

	private Label lblLine;

	private Label label5;

	private Label lblOrg;

	private TableLayoutPanel tableLayoutPanel14;

	private Label lblWorkorder_1;

	private Label lblPlanQty_1;

	private Label lblPlanDate_1;

	private Label lblPreQty_1b;

	private Label lblWorkorder_2;

	private TextBox lblTobeQty_4;

	private Label lblPlanQty_2;

	private Label lblPlanDate_2;

	private Label lblPreQty_4b;

	private Label lblWorkorder_4;

	private Label lblPlanDate_4;

	private TextBox lblTobeQty_3;

	private Label lblPlanQty_4;

	private TextBox lblTobeQty_1;

	private Label lblWorkorder_3;

	private TextBox lblTobeQty_2;

	private Label lblPreQty_3b;

	private Label lblPreQty_2b;

	private Label lblPlanQty_3;

	private Label lblPlanDate_3;

	private TableLayoutPanel tableLayoutPanel15;

	private Label label14;

	private Label lblModel;

	private TableLayoutPanel tableLayoutPanel16;

	private Label lblPartNo_1;

	private Label lblPartNo_3;

	private Label lblPartNo_2;

	private Label label2;

	private TableLayoutPanel tableLayoutPanel1;

	private TableLayoutPanel tableLayoutPanel2;

	private TableLayoutPanel tableLayoutPanel4;

	private TableLayoutPanel tableLayoutPanel5;

	private TableLayoutPanel tableLayoutPanel6;

	private Label label60;

	private Label label61;

	private Label label58;

	private Label label43;

	private Label label46;

	private TableLayoutPanel tableLayoutPanel3;

	private Label lblFAWorkorder_1;

	private TextBox lblFATobeQty_4;

	private Label lblFAPartNo_1;

	private Label lblFAQty_4;

	private Label lblFAPartNo_4;

	private Label lblFAWorkorder_4;

	private TextBox lblFATobeQty_3;

	private Label lblFAQty_1;

	private TextBox lblFATobeQty_2;

	private Label lblFAWorkorder_3;

	private Label lblFAPartNo_3;

	private Label lblFAQty_3;

	private TextBox lblFATobeQty_1;

	private Label lblFAWorkorder_2;

	private Label lblFAPartNo_2;

	private Label lblFAQty_2;

	private Label label33;

	private RadioButton rdoP;

	private RadioButton rdoB;

	private UserBox userBox1;

	public frmQtyChangePrint()
	{
		InitializeComponent();
	}

	private void frmQtyChangePrint_Load(object sender, EventArgs e)
	{
		try
		{
			_orgID = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "ORG_ID", "202351");
			_langID = Thread.CurrentThread.CurrentCulture.ToString();
			InitControls();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			string SheetID = txtCarrierID.Text;
			GetdtGroupBy();
			SetdtGroupby();
			dtGroupBy.Rows[0]["QRCODE_VALUE"] = MakeQRData();
			if (rdoP.Checked)
			{
				dtGroupBy.Rows[0]["PB"] = "P";
			}
			else
			{
				dtGroupBy.Rows[0]["PB"] = "";
			}
			frmQtyChangePrintPreview frm = new frmQtyChangePrintPreview();
			frm.dtGroupBy = dtGroupBy;
			GetSheetInfo(SheetID);
			if (dsResultSheet.Tables["OUT_DATA"].Rows.Count != 0)
			{
				SaveBizChangeQty();
			}
			frm.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtCarrierID_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			if (e.KeyChar == '\r' && !string.IsNullOrWhiteSpace(txtCarrierID.Text))
			{
				GMESInit();
				GetLabelInfo(txtCarrierID.Text);
				SetToBe();
			}
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
				MessageBox.Show("Connect the scanner.");
			}
		}
		catch (Exception)
		{
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
			if (ds != null && ds.Tables["OUT_DATA"].Rows.Count == 0)
			{
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
			string sCheckstring2 = string.Empty;
			bytelength = Variables.scannerPort.SerialPort.BytesToRead;
			byte[] bytes = new byte[bytelength];
			Variables.scannerPort.SerialPort.Read(bytes, 0, bytelength);
			sCheckstring2 += Encoding.Default.GetString(bytes);
			sCheckstring += Encoding.GetEncoding("iso-8859-1").GetString(bytes);
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
				sFirstScanData = sFirstScanData.Replace("Â", "");
				sFirstScanData = sFirstScanData.Replace("\u0002", "");
				sFirstScanData = sFirstScanData.Replace("\u0003", "");
				Invoke((MethodInvoker)delegate
				{
					GetLabelInfo(sFirstScanData);
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

	private void GetLabelInfo(string strBarcodeNo)
	{
		DataSet ds = null;
		DataSet ds2DBarcode = null;
		string barcode = strBarcodeNo;
		string scanType = string.Empty;
		string scanStr = string.Empty;
		GMESInit();
		if (barcode.Contains("¿"))
		{
			barcode = "\u0002" + barcode + "\u0003";
			ds2DBarcode = makeManualSheetdata(barcode);
			scanType = "SHEET_ID";
			scanStr = ds2DBarcode.Tables["OUT_DATA"].Rows[0]["SCAN_STR"].ToString();
			ds = getChkScanType(_orgID, _langID, scanStr, "MANUAL_IN", string.Empty, string.Empty);
			try
			{
				if (ds.Tables["ALL_SHEET_INFO"].Rows[0] == null)
				{
					ds = ds2DBarcode;
				}
			}
			catch
			{
				ds = ds2DBarcode;
			}
		}
		else
		{
			ds = getChkScanType(_orgID, _langID, barcode, "MANUAL_IN", string.Empty, string.Empty);
		}
		if (ds != null && ds.Tables["OUT_DATA"].Rows.Count != 0)
		{
			scanType = ds.Tables["OUT_DATA"].Rows[0]["SCAN_TYPE"].ToString();
			scanStr = ds.Tables["OUT_DATA"].Rows[0]["SCAN_STR"].ToString();
			txtPartNo.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "ITEM_CODE", 1);
			txtPartName.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "ITEM_DESC", 1);
			GetOrg("202351");
			lblOrg.Text = dsOrg.Tables["OUT_DATA"].Rows[0]["ORG_NAME"].ToString();
			lblFAWorkorder_1.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "TO_WO_NAME", 1);
			lblFAPartNo_1.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "ITEM_CODE", 1);
			lblFAQty_1.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "TO_WO_QTY", 1);
			lblFAWorkorder_2.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "TO_WO_NAME", 2);
			lblFAPartNo_2.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "ITEM_CODE", 2);
			lblFAQty_2.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "TO_WO_QTY", 2);
			lblFAWorkorder_3.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "TO_WO_NAME", 3);
			lblFAPartNo_3.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "ITEM_CODE", 3);
			lblFAQty_3.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "TO_WO_QTY", 3);
			lblFAWorkorder_4.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "TO_WO_NAME", 4);
			lblFAPartNo_4.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "ITEM_CODE", 4);
			lblFAQty_4.Text = GetCell(ds.Tables["ALL_SHEET_INFO"], "TO_WO_QTY", 4);
			SetToBe();
			GetGMESLabelInfo(scanStr);
			if (lblFATobeQty_1.Text != "")
			{
				int Tobe5 = ((!(lblFATobeQty_1.Text == "")) ? int.Parse(lblFATobeQty_1.Text) : 0);
				int Tobe6 = ((!(lblFATobeQty_2.Text == "")) ? int.Parse(lblFATobeQty_2.Text) : 0);
				int Tobe7 = ((!(lblFATobeQty_3.Text == "")) ? int.Parse(lblFATobeQty_3.Text) : 0);
				int Tobe8 = ((!(lblFATobeQty_4.Text == "")) ? int.Parse(lblFATobeQty_4.Text) : 0);
				lblBoxQty.Text = (Tobe5 + Tobe6 + Tobe7 + Tobe8).ToString();
			}
			txtCarrierID.Text = scanStr;
		}
		else
		{
			MessageBox.Show("No Data!");
		}
	}

	private void GetGMESLabelInfo(string strBarcodeNo)
	{
		try
		{
			DataTable dtLabelInfo = GetlabelPubRec(strBarcodeNo).Tables["RSLTDT"];
			DataTable dtWorkorderInfo = GetWorkorderByWoid(dtLabelInfo.Rows[0]["WOID"].ToString()).Tables["RSLTDT"];
			DataTable dtProcessSegmentInfo = GetProcesssegmentInfo(_langID, dtWorkorderInfo.Rows[0]["PCSGID"].ToString()).Tables["RSLTDT"];
			DataTable dtOrgInfo = GetOrgbyErpLine(_langID, dtLabelInfo.Rows[0]["PCSGID"].ToString()).Tables["RSLTDT"];
			DataTable dtModelInfo = GetModlByChildMtrl(dtWorkorderInfo.Rows[0]["PRODID"].ToString()).Tables["RSLTDT"];
			DataTable dtChildMtrlInfo = GetChildMtrlByMtrl(dtWorkorderInfo.Rows[0]["PRODID"].ToString()).Tables["RSLTDT"];
			DataTable dtLotbyBoxID = GetLotInfoByBoxID(strBarcodeNo).Tables["RSLTDT"];
			DataTable dtOrgMtrlInfo = GetOrgMtrl(dtWorkorderInfo.Rows[0]["ORG_ID"].ToString(), dtWorkorderInfo.Rows[0]["PRODID"].ToString()).Tables["RSLTDT"];
			DataTable dtWOIDList = dtLotbyBoxID.DefaultView.ToTable(true, "WOID");
			string ORG_NAME = dtOrgInfo.Rows[0]["ORG_NAME"].ToString();
			string ORG_ABBR_NAME = dtOrgInfo.Rows[0]["ORG_ABBR_NAME"].ToString();
			string PCSGNAME = dtProcessSegmentInfo.Rows[0]["PCSGNAME"].ToString();
			string PROD_DATE = Convert.ToDateTime(dtLotbyBoxID.Rows[0]["LOTDTTM_CR"]).ToString("yyyy-MM-dd");
			string PART_NO = dtWorkorderInfo.Rows[0]["PRODID"].ToString();
			string PART_NAME = dtOrgMtrlInfo.Rows[0]["MTRLNAME"].ToString();
			int BOXQTY = Convert.ToInt32(dtLotbyBoxID.Compute("SUM(LOTQTY_CR)", string.Empty));
			string WO_NAME_1 = GetWorkorderInfo(dtWOIDList, "WO_NAME", 1);
			string PLANQTY_1 = GetWorkorderInfo(dtWOIDList, "PLANQTY", 1);
			string PLANSTDTTM_1 = Convert.ToDateTime(GetWorkorderInfo(dtWOIDList, "PLANSTDTTM", 1)).ToString("yyyy-MM-dd");
			string WO_NAME_2 = GetWorkorderInfo(dtWOIDList, "WO_NAME", 2);
			string PLANQTY_2 = GetWorkorderInfo(dtWOIDList, "PLANQTY", 2);
			string PLANSTDTTM_2 = ((GetWorkorderInfo(dtWOIDList, "PLANSTDTTM", 2) == string.Empty) ? string.Empty : Convert.ToDateTime(GetWorkorderInfo(dtWOIDList, "PLANSTDTTM", 2)).ToString("yyyy-MM-dd"));
			string WO_NAME_3 = GetWorkorderInfo(dtWOIDList, "WO_NAME", 3);
			string PLANQTY_3 = GetWorkorderInfo(dtWOIDList, "PLANQTY", 3);
			string PLANSTDTTM_3 = ((GetWorkorderInfo(dtWOIDList, "PLANSTDTTM", 3) == string.Empty) ? string.Empty : Convert.ToDateTime(GetWorkorderInfo(dtWOIDList, "PLANSTDTTM", 3)).ToString("yyyy-MM-dd"));
			string WO_NAME_4 = GetWorkorderInfo(dtWOIDList, "WO_NAME", 4);
			string PLANQTY_4 = GetWorkorderInfo(dtWOIDList, "PLANQTY", 4);
			string PLANSTDTTM_4 = ((GetWorkorderInfo(dtWOIDList, "PLANSTDTTM", 4) == string.Empty) ? string.Empty : Convert.ToDateTime(GetWorkorderInfo(dtWOIDList, "PLANSTDTTM", 4)).ToString("yyyy-MM-dd"));
			string MODLID_1 = GetCell(dtModelInfo, "MODLID", 1);
			string MODLID_2 = GetCell(dtModelInfo, "MODLID", 2);
			string MODLID_3 = GetCell(dtModelInfo, "MODLID", 3);
			string MODLID_4 = GetCell(dtModelInfo, "MODLID", 4);
			string MODLID_5 = GetCell(dtModelInfo, "MODLID", 5);
			string MODLID_6 = GetCell(dtModelInfo, "MODLID", 6);
			string PARTNO_1 = string.Empty;
			string PARTNO_2 = string.Empty;
			string PARTNO_3 = string.Empty;
			for (int i = 0; i < dtChildMtrlInfo.Rows.Count; i++)
			{
				PARTNO_1 = dtChildMtrlInfo.Rows[i]["CHILD_MTRLID"].ToString();
				PARTNO_2 = dtChildMtrlInfo.Rows[i]["CHILD_MTRLID"].ToString();
				PARTNO_3 = ((dtChildMtrlInfo.Rows[i]["CHILD_MTRL_SPEC"].ToString().Length >= 30) ? dtChildMtrlInfo.Rows[i]["CHILD_MTRL_SPEC"].ToString().Substring(0, 30) : dtChildMtrlInfo.Rows[i]["CHILD_MTRL_SPEC"].ToString());
			}
			txtPartName.Text = PART_NAME;
			txtPartNo.Text = PART_NO;
			lblBoxQty.Text = BOXQTY.ToString();
			lblProdDate.Text = PROD_DATE;
			lblOrg.Text = ORG_NAME + "  " + ORG_ABBR_NAME;
			lblLine.Text = PCSGNAME;
			lblWorkorder_1.Text = WO_NAME_1;
			lblPlanQty_1.Text = PLANQTY_1;
			lblPlanDate_1.Text = PLANSTDTTM_1;
			lblWorkorder_2.Text = WO_NAME_2;
			lblPlanQty_2.Text = PLANQTY_2;
			lblPlanDate_2.Text = PLANSTDTTM_2;
			lblWorkorder_3.Text = WO_NAME_3;
			lblPlanQty_3.Text = PLANQTY_3;
			lblPlanDate_3.Text = PLANSTDTTM_3;
			lblWorkorder_4.Text = WO_NAME_4;
			lblPlanQty_4.Text = PLANQTY_4;
			lblPlanDate_4.Text = PLANSTDTTM_4;
			lblModel.Text = MODLID_1 + "  " + MODLID_2 + "  " + MODLID_3 + "  " + MODLID_4 + "  " + MODLID_5 + "  " + MODLID_6;
			lblPartNo_1.Text = PARTNO_1;
			lblPartNo_2.Text = PARTNO_2;
			lblPartNo_3.Text = PARTNO_3;
		}
		catch (Exception)
		{
		}
	}

	private void GetdtGroupBy()
	{
		dtGroupBy.Clear();
		dtGroupBy.Reset();
		dtGroupBy.Columns.Add("QRCODE_VALUE");
		dtGroupBy.Columns.Add("SHEET_ID");
		dtGroupBy.Columns.Add("PART_NO");
		dtGroupBy.Columns.Add("PART_NAME");
		dtGroupBy.Columns.Add("BOXQTY");
		dtGroupBy.Columns.Add("ORG_ID");
		dtGroupBy.Columns.Add("LINE_CODE");
		dtGroupBy.Columns.Add("PROD_DATE");
		dtGroupBy.Columns.Add("WORK_ORDER1");
		dtGroupBy.Columns.Add("PLAN_QTY1");
		dtGroupBy.Columns.Add("PLAN_DATE1");
		dtGroupBy.Columns.Add("ASIS_QTY1");
		dtGroupBy.Columns.Add("TOBE_QTY1");
		dtGroupBy.Columns.Add("WORK_ORDER2");
		dtGroupBy.Columns.Add("PLAN_QTY2");
		dtGroupBy.Columns.Add("PLAN_DATE2");
		dtGroupBy.Columns.Add("ASIS_QTY2");
		dtGroupBy.Columns.Add("TOBE_QTY2");
		dtGroupBy.Columns.Add("WORK_ORDER3");
		dtGroupBy.Columns.Add("PLAN_QTY3");
		dtGroupBy.Columns.Add("PLAN_DATE3");
		dtGroupBy.Columns.Add("ASIS_QTY3");
		dtGroupBy.Columns.Add("TOBE_QTY3");
		dtGroupBy.Columns.Add("WORK_ORDER4");
		dtGroupBy.Columns.Add("PLAN_QTY4");
		dtGroupBy.Columns.Add("PLAN_DATE4");
		dtGroupBy.Columns.Add("ASIS_QTY4");
		dtGroupBy.Columns.Add("TOBE_QTY4");
		dtGroupBy.Columns.Add("APPLY_MODEL");
		dtGroupBy.Columns.Add("PART_NO_1");
		dtGroupBy.Columns.Add("PART_NO_2");
		dtGroupBy.Columns.Add("PART_NO_3");
		dtGroupBy.Columns.Add("MODEL_SUFFIX1");
		dtGroupBy.Columns.Add("MODEL_SUFFIX2");
		dtGroupBy.Columns.Add("MODEL_SUFFIX3");
		dtGroupBy.Columns.Add("MODEL_SUFFIX4");
		dtGroupBy.Columns.Add("PRINTED_DATE");
		dtGroupBy.Columns.Add("PB");
	}

	private void SetdtGroupby()
	{
		dtGroupBy.Rows.Add(0, 1);
		dtGroupBy.Rows[0]["SHEET_ID"] = txtCarrierID.Text;
		dtGroupBy.Rows[0]["PART_NO"] = txtPartNo.Text;
		dtGroupBy.Rows[0]["PART_NAME"] = txtPartName.Text;
		dtGroupBy.Rows[0]["BOXQTY"] = lblBoxQty.Text;
		dtGroupBy.Rows[0]["ORG_ID"] = lblOrg.Text;
		dtGroupBy.Rows[0]["LINE_CODE"] = lblLine.Text;
		dtGroupBy.Rows[0]["PROD_DATE"] = lblProdDate.Text;
		dtGroupBy.Rows[0]["WORK_ORDER1"] = lblWorkorder_1.Text;
		dtGroupBy.Rows[0]["PLAN_QTY1"] = lblPlanQty_1.Text;
		dtGroupBy.Rows[0]["PLAN_DATE1"] = lblPlanDate_1.Text;
		dtGroupBy.Rows[0]["ASIS_QTY1"] = lblPreQty_1b.Text;
		dtGroupBy.Rows[0]["TOBE_QTY1"] = lblTobeQty_1.Text;
		dtGroupBy.Rows[0]["WORK_ORDER2"] = lblWorkorder_2.Text;
		dtGroupBy.Rows[0]["PLAN_QTY2"] = lblPlanQty_2.Text;
		dtGroupBy.Rows[0]["PLAN_DATE2"] = lblPlanDate_2.Text;
		dtGroupBy.Rows[0]["ASIS_QTY2"] = lblPreQty_2b.Text;
		dtGroupBy.Rows[0]["TOBE_QTY2"] = lblTobeQty_2.Text;
		dtGroupBy.Rows[0]["WORK_ORDER3"] = lblWorkorder_3.Text;
		dtGroupBy.Rows[0]["PLAN_QTY3"] = lblPlanQty_3.Text;
		dtGroupBy.Rows[0]["PLAN_DATE3"] = lblPlanDate_3.Text;
		dtGroupBy.Rows[0]["ASIS_QTY3"] = lblPreQty_3b.Text;
		dtGroupBy.Rows[0]["TOBE_QTY3"] = lblTobeQty_3.Text;
		dtGroupBy.Rows[0]["WORK_ORDER4"] = lblWorkorder_4.Text;
		dtGroupBy.Rows[0]["PLAN_QTY4"] = lblPlanQty_4.Text;
		dtGroupBy.Rows[0]["PLAN_DATE4"] = lblPlanDate_4.Text;
		dtGroupBy.Rows[0]["ASIS_QTY4"] = lblPreQty_4b.Text;
		dtGroupBy.Rows[0]["TOBE_QTY4"] = lblTobeQty_4.Text;
		dtGroupBy.Rows[0]["APPLY_MODEL"] = lblModel.Text;
		dtGroupBy.Rows[0]["PART_NO_1"] = lblPartNo_1.Text;
		dtGroupBy.Rows[0]["PART_NO_2"] = lblPartNo_2.Text;
		dtGroupBy.Rows[0]["PART_NO_3"] = lblPartNo_3.Text;
		dtGroupBy.Rows[0]["PRINTED_DATE"] = DateTime.Now.ToString("yyyy.MM.dd HH:mm");
	}

	private string MakeQRData()
	{
		StringBuilder sbData = new StringBuilder();
		string sResult = string.Empty;
		char sSTX = '\u0002';
		char sETX = '\u0003';
		try
		{
			sbData.Append(dtGroupBy.Rows[0]["SHEET_ID"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["LINE_CODE"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["PART_NO"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["BOXQTY"].ToString());
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["MODEL_SUFFIX1"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["WORK_ORDER1"].ToString());
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["MODEL_SUFFIX2"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["WORK_ORDER2"].ToString());
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["MODEL_SUFFIX3"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["WORK_ORDER3"].ToString());
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["MODEL_SUFFIX4"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[0]["WORK_ORDER4"].ToString());
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sbData.Append("¿");
			sResult = sbData.ToString();
			sResult = sSTX + sResult + sETX;
		}
		catch (Exception)
		{
		}
		return sResult;
	}

	private DataSet makeManualSheetdata(string sheetData)
	{
		string replaceItem = sheetData;
		DataSet ds = null;
		DataTable dtSheetInfo = null;
		DataTable dtCarrierMap = null;
		DataTable dtLocatorInfo = null;
		DataTable dtOutData = null;
		DataRow drSheetInfo = null;
		DataRow drOutData = null;
		int startInx = 6;
		try
		{
			ds = new DataSet();
			dtSheetInfo = new DataTable("ALL_SHEET_INFO");
			dtCarrierMap = new DataTable("CURRENT_CARRIER_MAP");
			dtLocatorInfo = new DataTable("LOCATOR_INFO");
			dtOutData = new DataTable("OUT_DATA");
			ds.Tables.Add(dtSheetInfo);
			ds.Tables.Add(dtCarrierMap);
			ds.Tables.Add(dtLocatorInfo);
			ds.Tables.Add(dtOutData);
			string stx = '\u0002'.ToString();
			string etx = '\u0003'.ToString();
			string[] delimiters = new string[1] { "¿" };
			dtSheetInfo.Columns.Add(new DataColumn("SHEET_ID", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("LINE", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("ITEM_CODE", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("TOTAL_QTY", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("CARRIER_ID", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("TOTAL_SEQ_NO", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("SHEET_TYPE", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("TO_WO_QTY", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("PROD_SEQ_FROM", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("PROD_SEQ_TO", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("MODEL_SUFFIX", typeof(string)));
			dtSheetInfo.Columns.Add(new DataColumn("TO_WO_NAME", typeof(string)));
			dtOutData.Columns.Add(new DataColumn("SCAN_TYPE", typeof(string)));
			dtOutData.Columns.Add(new DataColumn("SCAN_STR", typeof(string)));
			if (!sheetData.StartsWith(stx) || !sheetData.EndsWith(etx))
			{
				return ds;
			}
			string[] removeList = new string[2] { stx, etx };
			removeList.ToList().ForEach(delegate(string o)
			{
				replaceItem = replaceItem.Replace(o, string.Empty);
			});
			string[] sheetItem = replaceItem.Split(delimiters, StringSplitOptions.None);
			int i;
			for (i = startInx; i < sheetItem.Length; i++)
			{
				drSheetInfo = dtSheetInfo.NewRow();
				drSheetInfo["SHEET_ID"] = sheetItem[0];
				drSheetInfo["LINE"] = sheetItem[1];
				drSheetInfo["ITEM_CODE"] = sheetItem[2];
				drSheetInfo["TOTAL_QTY"] = sheetItem[3];
				drSheetInfo["CARRIER_ID"] = sheetItem[4];
				drSheetInfo["TOTAL_SEQ_NO"] = sheetItem[5];
				drSheetInfo["SHEET_TYPE"] = "LOCATOR_CARRIER_OFFLINE";
				drSheetInfo["TO_WO_QTY"] = sheetItem[i];
				i++;
				drSheetInfo["PROD_SEQ_FROM"] = sheetItem[i];
				i++;
				drSheetInfo["PROD_SEQ_TO"] = sheetItem[i];
				i++;
				drSheetInfo["MODEL_SUFFIX"] = sheetItem[i];
				i++;
				drSheetInfo["TO_WO_NAME"] = sheetItem[i];
				if (drSheetInfo["TO_WO_NAME"] != DBNull.Value && drSheetInfo["TO_WO_NAME"].ToString() != string.Empty)
				{
					dtSheetInfo.Rows.Add(drSheetInfo);
				}
			}
			drOutData = dtOutData.NewRow();
			drOutData["SCAN_TYPE"] = "SHEET_ID";
			drOutData["SCAN_STR"] = dtSheetInfo.Rows[0]["SHEET_ID"].ToString();
			dtOutData.Rows.Add(drOutData);
			return ds;
		}
		catch (Exception)
		{
			return null;
		}
	}

	private DataSet GetlabelPubRec(string barcodNo)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "BARCODE_NO", barcodNo);
			dsResult = new BizService().ExecBizRule("SEL_GMES_TB_SFC_LBL_PUB_REC_IF_BY_BARCODE", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetOrgMtrl(string orgID, string mtrlID)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "MTRLID", mtrlID);
			dsResult = new BizService().ExecBizRule("SEL_GMES_TB_POM_ORG_MTRL_BAS", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetWorkorderByWoid(string woid)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "WOID", woid);
			dsResult = new BizService().ExecBizRule("SEL_GMES_WORKORDER_TBL", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetProcesssegmentInfo(string langid, string pcsgid)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "LANGID", langid);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "PCSGID", pcsgid);
			dsResult = new BizService().ExecBizRule("SEL_GMES_PROCESSSEGMENT_BAS", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetOrgbyErpLine(string langid, string pcsgid)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "LANGID", langid);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "PCSGID", pcsgid);
			dsResult = new BizService().ExecBizRule("SEL_GMES_ORG_BY_ERPLINE", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetModlByChildMtrl(string mtrlID)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "CHILD_MTRLID", mtrlID);
			dsResult = new BizService().ExecBizRule("SEL_GMES_POM_MODLID_BY_CHILD_MTRLID", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetChildMtrlByMtrl(string mtrlID)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "PARENT_MTRLID", mtrlID);
			dsResult = new BizService().ExecBizRule("SEL_GMES_POM_CHILD_MTRLID_BY_PARENT_MTRLID", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetEquipmentInfoByEqptID(string langID, string eqptID)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "LANGID", langID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "EQPTID", eqptID);
			dsResult = new BizService().ExecBizRule("SEL_GMES_EQUIPMENT_BY_EQPTID", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetLotInfoByLotID(string lotID)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "LOTID", lotID);
			dsResult = new BizService().ExecBizRule("SEL_GMES_SEL_LOT_BY_LOTID", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private DataSet GetLotInfoByBoxID(string boxID)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "BOXID", boxID);
			dsResult = new BizService().ExecBizRule("SEL_GMES_SEL_LOT_BY_BOXID", ds, "RQSTDT", "RSLTDT");
		}
		catch (Exception)
		{
		}
		return dsResult;
	}

	private string GetCell(DataTable dt, string columnName, int index)
	{
		try
		{
			if (dt.Rows.Count > index - 1)
			{
				for (int i = 0; i < dt.Columns.Count; i++)
				{
					if (dt.Columns[i].ColumnName == columnName)
					{
						return dt.Rows[index - 1][columnName].ToString();
					}
				}
			}
			return string.Empty;
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	public DataSet getChkScanType(string orgID, string langID, string scanData, string SheetInfoFromDB, string carrierInfoFromDB, string locatorInfoFromDB)
	{
		DataSet dsResult = null;
		DataSet ds = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LANGID", langID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SCAN_STR", scanData);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_INFO_FROM_DB", SheetInfoFromDB);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CARRIER_INFO_FROM_DB", carrierInfoFromDB);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LOCATOR_INFO_FROM_DB", locatorInfoFromDB);
			return new BizService().ExecBizRule("GMCS_GET_SCAN_INFO", ds, "IN_DATA", "OUT_DATA,LOCATOR_INFO,CURRENT_CARRIER_MAP,ALL_SHEET_INFO,ITEM_INFO,LOCATOR_DETAIL,LOCATOR_CARRIER_INFO,ITEM_SN_INFO");
		}
		catch (Exception)
		{
			return null;
		}
		finally
		{
		}
	}

	private string GetWorkorderInfo(DataTable dt, string columnName, int index)
	{
		DataTable dtWorkorder = null;
		try
		{
			if (dt.Rows.Count > index - 1)
			{
				dtWorkorder = GetWorkorderByWoid(dt.Rows[index - 1]["WOID"].ToString()).Tables["RSLTDT"];
				for (int i = 0; i < dtWorkorder.Columns.Count; i++)
				{
					if (dtWorkorder.Columns[i].ColumnName == columnName)
					{
						return dtWorkorder.Rows[0][columnName].ToString();
					}
				}
			}
			return string.Empty;
		}
		catch (Exception)
		{
			return string.Empty;
		}
	}

	private void SetToBe()
	{
		if (!(lblPreQty_1b.Text != "") || !(lblFAQty_1.Text == ""))
		{
			if (lblFAQty_1.Text == "")
			{
				if (lblPreQty_1b.Text != "")
				{
					lblTobeQty_1.Enabled = true;
					lblTobeQty_1.BackColor = Color.PaleGreen;
				}
				if (lblPreQty_2b.Text != "")
				{
					lblTobeQty_2.Enabled = true;
					lblTobeQty_2.BackColor = Color.PaleGreen;
				}
				if (lblPreQty_3b.Text != "")
				{
					lblTobeQty_3.Enabled = true;
					lblTobeQty_3.BackColor = Color.PaleGreen;
				}
				if (lblPreQty_4b.Text != "")
				{
					lblTobeQty_4.BackColor = Color.PaleGreen;
					lblTobeQty_4.Enabled = true;
				}
			}
			else if (lblPreQty_1b.Text == "")
			{
				if (lblFAQty_1.Text != "")
				{
					lblFATobeQty_1.Enabled = true;
					lblFATobeQty_1.BackColor = Color.PaleGreen;
				}
				if (lblFAQty_2.Text != "")
				{
					lblFATobeQty_2.Enabled = true;
					lblFATobeQty_2.BackColor = Color.PaleGreen;
				}
				if (lblFAQty_3.Text != "")
				{
					lblFATobeQty_3.Enabled = true;
					lblFATobeQty_3.BackColor = Color.PaleGreen;
				}
				if (lblFAQty_4.Text != "")
				{
					lblFATobeQty_4.Enabled = true;
					lblFATobeQty_4.BackColor = Color.PaleGreen;
				}
			}
		}
		lblTobeQty_1.Text = lblPreQty_1b.Text;
		lblTobeQty_2.Text = lblPreQty_2b.Text;
		lblTobeQty_3.Text = lblPreQty_3b.Text;
		lblTobeQty_4.Text = lblPreQty_4b.Text;
		lblFATobeQty_1.Text = lblFAQty_1.Text;
		lblFATobeQty_2.Text = lblFAQty_2.Text;
		lblFATobeQty_3.Text = lblFAQty_3.Text;
		lblFATobeQty_4.Text = lblFAQty_4.Text;
	}

	private DataSet GetSheetInfo(string SheetID)
	{
		try
		{
			DataSet ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_ID", SheetID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CLOSED_YN", "N");
			dsResultSheet = new BizService().ExecBizRule("GMCS_GET_CURRENT_CARRIER_MAP", ds, "IN_DATA", "OUT_DATA");
			if (ds != null && dsResultSheet.Tables["OUT_DATA"].Rows.Count == 0)
			{
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
		return dsResultSheet;
	}

	private void SaveBizChangeQty()
	{
		DataSet dsResult = null;
		DataSet ds = new DataSet();
		ds = new DataSet();
		for (int i = 0; i < dsResultSheet.Tables["OUT_DATA"].Rows.Count; i++)
		{
			if (dsResultSheet.Tables["OUT_DATA"].Rows[0]["ENABLE_FLAG"].ToString() != "Y")
			{
				return;
			}
			string _lblFATobeQty = "lblFATobeQty_" + (i + 1);
			string _lblFAQty = "lblFAQty_" + (i + 1);
			switch (i)
			{
			case 0:
				_lblFATobeQty = lblFATobeQty_1.Text;
				_lblFAQty = lblFAQty_1.Text;
				break;
			case 1:
				_lblFATobeQty = lblFATobeQty_2.Text;
				_lblFAQty = lblFAQty_2.Text;
				break;
			case 2:
				_lblFATobeQty = lblFATobeQty_3.Text;
				_lblFAQty = lblFAQty_3.Text;
				break;
			case 3:
				_lblFATobeQty = lblFATobeQty_4.Text;
				_lblFAQty = lblFAQty_4.Text;
				break;
			}
			string I_ORG_ID = dsResultSheet.Tables["OUT_DATA"].Rows[0]["ORG_ID"].ToString();
			string I_SHEET_ID = dsResultSheet.Tables["OUT_DATA"].Rows[0]["SHEET_ID"].ToString();
			string I_LOCATOR = dsResultSheet.Tables["OUT_DATA"].Rows[0]["CARRIER_LOCATOR"].ToString();
			string I_ITEM_CODE = dsResultSheet.Tables["OUT_DATA"].Rows[0]["ITEM_CODE"].ToString();
			string I_QTY = _lblFAQty;
			string I_TO_WO_NAME = dsResultSheet.Tables["OUT_DATA"].Rows[0]["TO_WO_NAME"].ToString();
			string I_TOTAL_QTY = lblBoxQty.Text;
			string I_CHANGE_QTY = _lblFATobeQty;
			string I_TO_WO_SEQ = dsResultSheet.Tables["OUT_DATA"].Rows[0]["TO_WO_SEQ"].ToString();
			string I_SHEET_TYPE = dsResultSheet.Tables["OUT_DATA"].Rows[0]["SHEET_TYPE"].ToString();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_ORG_ID", I_ORG_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_START_END", "MANUAL");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_CARRIER_ID", "CAR" + I_SHEET_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_SHEET_ID", I_SHEET_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_MAKE_REMOVE", "UPDATE");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_LOCATOR", I_LOCATOR);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_ITEM_CODE", I_ITEM_CODE);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_QTY", I_QTY);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_USER_ID", "CS");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TXN_FROM", "CS");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TO_WO_NAME", I_TO_WO_NAME);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_COMMENTS", "");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "TOTAL_QTY", I_TOTAL_QTY);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CHANGE_QTY", I_CHANGE_QTY);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DATASTATE", "UPD");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TO_WO_SEQ", I_TO_WO_SEQ);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_SHEET_TYPE", I_SHEET_TYPE);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "STATUS", "");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "PRINT_YN", "Y");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_MAP_TYPE", "PROD_AFTER_MAP");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TXN_UNIT", "SHEET");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TXN_ID", I_SHEET_ID);
		}
		dsResult = new BizService().ExecBizRule("GMCS_SET_CHANGE_QTY", ds, "IN_DATA", "OUT_DATA");
	}

	private void GMESInit()
	{
		txtPartName.Text = "";
		txtPartNo.Text = "";
		lblBoxQty.Text = "";
		lblProdDate.Text = "";
		lblOrg.Text = "";
		lblLine.Text = "";
		lblWorkorder_1.Text = "";
		lblPlanQty_1.Text = "";
		lblPlanDate_1.Text = "";
		lblWorkorder_2.Text = "";
		lblPlanQty_2.Text = "";
		lblPlanDate_2.Text = "";
		lblWorkorder_3.Text = "";
		lblPlanQty_3.Text = "";
		lblPlanDate_3.Text = "";
		lblWorkorder_4.Text = "";
		lblPlanQty_4.Text = "";
		lblPlanDate_4.Text = "";
		lblModel.Text = "";
		lblPartNo_1.Text = "";
		lblPartNo_2.Text = "";
		lblPartNo_3.Text = "";
		lblPreQty_1b.Text = "";
		lblPreQty_2b.Text = "";
		lblPreQty_3b.Text = "";
		lblPreQty_4b.Text = "";
		lblFAWorkorder_1.Text = "";
		lblFAPartNo_1.Text = "";
		lblFAQty_1.Text = "";
		lblFATobeQty_1.Text = "";
		lblFAWorkorder_2.Text = "";
		lblFAPartNo_2.Text = "";
		lblFAQty_2.Text = "";
		lblFATobeQty_2.Text = "";
		lblFAWorkorder_3.Text = "";
		lblFAPartNo_3.Text = "";
		lblFAQty_3.Text = "";
		lblFATobeQty_3.Text = "";
		lblFAWorkorder_4.Text = "";
		lblFAPartNo_4.Text = "";
		lblFAQty_4.Text = "";
		lblFATobeQty_4.Text = "";
		lblTobeQty_1.BackColor = Color.White;
		lblTobeQty_2.BackColor = Color.White;
		lblTobeQty_3.BackColor = Color.White;
		lblTobeQty_4.BackColor = Color.White;
		lblFATobeQty_1.BackColor = Color.White;
		lblFATobeQty_2.BackColor = Color.White;
		lblFATobeQty_3.BackColor = Color.White;
		lblFATobeQty_4.BackColor = Color.White;
		lblTobeQty_1.Enabled = false;
		lblTobeQty_2.Enabled = false;
		lblTobeQty_3.Enabled = false;
		lblTobeQty_4.Enabled = false;
		lblFATobeQty_1.Enabled = false;
		lblFATobeQty_2.Enabled = false;
		lblFATobeQty_3.Enabled = false;
		lblFATobeQty_4.Enabled = false;
	}

	private void lblTobeQty_1_TextChanged(object sender, EventArgs e)
	{
		int Tobe1 = ((!(lblTobeQty_1.Text == "")) ? int.Parse(lblTobeQty_1.Text) : 0);
		int Tobe2 = ((!(lblTobeQty_2.Text == "")) ? int.Parse(lblTobeQty_2.Text) : 0);
		int Tobe3 = ((!(lblTobeQty_3.Text == "")) ? int.Parse(lblTobeQty_3.Text) : 0);
		int Tobe4 = ((!(lblTobeQty_4.Text == "")) ? int.Parse(lblTobeQty_4.Text) : 0);
		int Tobe5 = ((!(lblFATobeQty_1.Text == "")) ? int.Parse(lblFATobeQty_1.Text) : 0);
		int Tobe6 = ((!(lblFATobeQty_2.Text == "")) ? int.Parse(lblFATobeQty_2.Text) : 0);
		int Tobe7 = ((!(lblFATobeQty_3.Text == "")) ? int.Parse(lblFATobeQty_3.Text) : 0);
		int Tobe8 = ((!(lblFATobeQty_4.Text == "")) ? int.Parse(lblFATobeQty_4.Text) : 0);
		lblBoxQty.Text = (Tobe1 + Tobe2 + Tobe3 + Tobe4 + Tobe5 + Tobe6 + Tobe7 + Tobe8).ToString();
	}

	private DataSet GetOrg(string Orgid)
	{
		DataSet ds = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", Orgid);
			dsOrg = new BizService().ExecBizRule("GMCS_GET_ORG_INFO", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception)
		{
		}
		return dsOrg;
	}

	private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmQtyChangePrint));
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.rdoP = new System.Windows.Forms.RadioButton();
		this.rdoB = new System.Windows.Forms.RadioButton();
		this.userBox1 = new MCS.Common.Controls.UserBox();
		this.txtCarrierID = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.lblBoxQty = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtPartName = new System.Windows.Forms.TextBox();
		this.txtPartNo = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.lblPartNo = new System.Windows.Forms.Label();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
		this.lblFAWorkorder_1 = new System.Windows.Forms.Label();
		this.lblFATobeQty_4 = new System.Windows.Forms.TextBox();
		this.lblFAPartNo_1 = new System.Windows.Forms.Label();
		this.lblFAQty_4 = new System.Windows.Forms.Label();
		this.lblFAPartNo_4 = new System.Windows.Forms.Label();
		this.lblFAWorkorder_4 = new System.Windows.Forms.Label();
		this.lblFATobeQty_3 = new System.Windows.Forms.TextBox();
		this.lblFAQty_1 = new System.Windows.Forms.Label();
		this.lblFATobeQty_2 = new System.Windows.Forms.TextBox();
		this.lblFAWorkorder_3 = new System.Windows.Forms.Label();
		this.lblFAPartNo_3 = new System.Windows.Forms.Label();
		this.lblFAQty_3 = new System.Windows.Forms.Label();
		this.lblFATobeQty_1 = new System.Windows.Forms.TextBox();
		this.lblFAWorkorder_2 = new System.Windows.Forms.Label();
		this.lblFAPartNo_2 = new System.Windows.Forms.Label();
		this.lblFAQty_2 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel10 = new System.Windows.Forms.TableLayoutPanel();
		this.label4 = new System.Windows.Forms.Label();
		this.tableLayoutPanel11 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel12 = new System.Windows.Forms.TableLayoutPanel();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.tableLayoutPanel13 = new System.Windows.Forms.TableLayoutPanel();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.lblLine = new System.Windows.Forms.Label();
		this.lblProdDate = new System.Windows.Forms.Label();
		this.lblOrg = new System.Windows.Forms.Label();
		this.tableLayoutPanel14 = new System.Windows.Forms.TableLayoutPanel();
		this.lblWorkorder_1 = new System.Windows.Forms.Label();
		this.lblPlanQty_1 = new System.Windows.Forms.Label();
		this.lblPlanDate_1 = new System.Windows.Forms.Label();
		this.lblPreQty_1b = new System.Windows.Forms.Label();
		this.lblWorkorder_2 = new System.Windows.Forms.Label();
		this.lblTobeQty_4 = new System.Windows.Forms.TextBox();
		this.lblPlanQty_2 = new System.Windows.Forms.Label();
		this.lblPlanDate_2 = new System.Windows.Forms.Label();
		this.lblPreQty_4b = new System.Windows.Forms.Label();
		this.lblWorkorder_4 = new System.Windows.Forms.Label();
		this.lblPlanDate_4 = new System.Windows.Forms.Label();
		this.lblTobeQty_3 = new System.Windows.Forms.TextBox();
		this.lblPlanQty_4 = new System.Windows.Forms.Label();
		this.lblTobeQty_1 = new System.Windows.Forms.TextBox();
		this.lblWorkorder_3 = new System.Windows.Forms.Label();
		this.lblTobeQty_2 = new System.Windows.Forms.TextBox();
		this.lblPreQty_3b = new System.Windows.Forms.Label();
		this.lblPreQty_2b = new System.Windows.Forms.Label();
		this.lblPlanQty_3 = new System.Windows.Forms.Label();
		this.lblPlanDate_3 = new System.Windows.Forms.Label();
		this.tableLayoutPanel15 = new System.Windows.Forms.TableLayoutPanel();
		this.label14 = new System.Windows.Forms.Label();
		this.lblModel = new System.Windows.Forms.Label();
		this.tableLayoutPanel16 = new System.Windows.Forms.TableLayoutPanel();
		this.lblPartNo_1 = new System.Windows.Forms.Label();
		this.lblPartNo_3 = new System.Windows.Forms.Label();
		this.lblPartNo_2 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.textBox9 = new System.Windows.Forms.TextBox();
		this.splitContainerMain = new System.Windows.Forms.SplitContainer();
		this.backPanel21 = new MCS.Common.BackPanel2();
		this.btnSave = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.tmDemo = new System.Windows.Forms.Timer(this.components);
		this.dgvBuffer = new System.Windows.Forms.DataGridView();
		this.tmRefresh = new System.Windows.Forms.Timer(this.components);
		this.searchPanel1 = new MCS.Common.SearchPanel();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.tableLayoutPanel1.SuspendLayout();
		this.tableLayoutPanel2.SuspendLayout();
		this.tableLayoutPanel4.SuspendLayout();
		this.tableLayoutPanel5.SuspendLayout();
		this.tableLayoutPanel6.SuspendLayout();
		this.tableLayoutPanel3.SuspendLayout();
		this.tableLayoutPanel7.SuspendLayout();
		this.tableLayoutPanel8.SuspendLayout();
		this.tableLayoutPanel10.SuspendLayout();
		this.tableLayoutPanel11.SuspendLayout();
		this.tableLayoutPanel12.SuspendLayout();
		this.tableLayoutPanel13.SuspendLayout();
		this.tableLayoutPanel14.SuspendLayout();
		this.tableLayoutPanel15.SuspendLayout();
		this.tableLayoutPanel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).BeginInit();
		this.splitContainerMain.Panel1.SuspendLayout();
		this.splitContainerMain.Panel2.SuspendLayout();
		this.splitContainerMain.SuspendLayout();
		this.backPanel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).BeginInit();
		base.SuspendLayout();
		this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
		this.splitContainer1.Panel1.Controls.Add(this.rdoP);
		this.splitContainer1.Panel1.Controls.Add(this.rdoB);
		this.splitContainer1.Panel1.Controls.Add(this.userBox1);
		this.splitContainer1.Panel1.Controls.Add(this.txtCarrierID);
		this.splitContainer1.Panel1.Controls.Add(this.label13);
		this.splitContainer1.Panel1.Controls.Add(this.lblBoxQty);
		this.splitContainer1.Panel1.Controls.Add(this.label12);
		this.splitContainer1.Panel1.Controls.Add(this.txtPartName);
		this.splitContainer1.Panel1.Controls.Add(this.txtPartNo);
		this.splitContainer1.Panel1.Controls.Add(this.label11);
		this.splitContainer1.Panel1.Controls.Add(this.lblPartNo);
		this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
		this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
		this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel7);
		this.splitContainer1.Size = new System.Drawing.Size(1251, 689);
		this.splitContainer1.SplitterDistance = 131;
		this.splitContainer1.SplitterWidth = 1;
		this.splitContainer1.TabIndex = 73;
		this.rdoP.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoP.AutoSize = true;
		this.rdoP.Font = new System.Drawing.Font("Arial", 20.25f, System.Drawing.FontStyle.Bold);
		this.rdoP.Location = new System.Drawing.Point(1068, 7);
		this.rdoP.Name = "rdoP";
		this.rdoP.Size = new System.Drawing.Size(51, 36);
		this.rdoP.TabIndex = 118;
		this.rdoP.Text = "P";
		this.rdoP.UseVisualStyleBackColor = true;
		this.rdoB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoB.AutoSize = true;
		this.rdoB.Checked = true;
		this.rdoB.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.rdoB.Location = new System.Drawing.Point(987, 7);
		this.rdoB.Name = "rdoB";
		this.rdoB.Size = new System.Drawing.Size(53, 36);
		this.rdoB.TabIndex = 117;
		this.rdoB.TabStop = true;
		this.rdoB.Text = "B";
		this.rdoB.UseVisualStyleBackColor = true;
		this.userBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.userBox1.BackColor = System.Drawing.Color.Transparent;
		this.userBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.userBox1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.userBox1.Location = new System.Drawing.Point(953, 3);
		this.userBox1.Name = "userBox1";
		this.userBox1.Size = new System.Drawing.Size(198, 42);
		this.userBox1.TabIndex = 116;
		this.txtCarrierID.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.txtCarrierID.BackColor = System.Drawing.Color.Black;
		this.txtCarrierID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtCarrierID.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.txtCarrierID.ForeColor = System.Drawing.Color.FromArgb(235, 222, 0);
		this.txtCarrierID.Location = new System.Drawing.Point(256, 1);
		this.txtCarrierID.Name = "txtCarrierID";
		this.txtCarrierID.Size = new System.Drawing.Size(695, 46);
		this.txtCarrierID.TabIndex = 89;
		this.txtCarrierID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCarrierID_KeyPress);
		this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label13.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label13.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(821, 47);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(130, 75);
		this.label13.TabIndex = 115;
		this.label13.Text = "Qty";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblBoxQty.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.lblBoxQty.BackColor = System.Drawing.Color.White;
		this.lblBoxQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblBoxQty.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblBoxQty.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.lblBoxQty.ForeColor = System.Drawing.Color.Black;
		this.lblBoxQty.Location = new System.Drawing.Point(953, 47);
		this.lblBoxQty.Name = "lblBoxQty";
		this.lblBoxQty.Size = new System.Drawing.Size(198, 75);
		this.lblBoxQty.TabIndex = 115;
		this.lblBoxQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label12.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label12.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label12.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(51, 47);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(203, 75);
		this.label12.TabIndex = 115;
		this.label12.Text = "PART NO";
		this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPartName.BackColor = System.Drawing.Color.White;
		this.txtPartName.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtPartName.Enabled = false;
		this.txtPartName.Font = new System.Drawing.Font("Arial", 19f, System.Drawing.FontStyle.Bold);
		this.txtPartName.Location = new System.Drawing.Point(264, 85);
		this.txtPartName.Name = "txtPartName";
		this.txtPartName.Size = new System.Drawing.Size(525, 30);
		this.txtPartName.TabIndex = 90;
		this.txtPartNo.BackColor = System.Drawing.Color.White;
		this.txtPartNo.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtPartNo.Enabled = false;
		this.txtPartNo.Font = new System.Drawing.Font("Arial", 25f, System.Drawing.FontStyle.Bold);
		this.txtPartNo.Location = new System.Drawing.Point(264, 52);
		this.txtPartNo.Name = "txtPartNo";
		this.txtPartNo.Size = new System.Drawing.Size(525, 39);
		this.txtPartNo.TabIndex = 90;
		this.label11.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label11.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(51, 3);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(203, 41);
		this.label11.TabIndex = 115;
		this.label11.Text = "SHEET ID";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPartNo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.lblPartNo.BackColor = System.Drawing.Color.White;
		this.lblPartNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPartNo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPartNo.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPartNo.ForeColor = System.Drawing.Color.Black;
		this.lblPartNo.Location = new System.Drawing.Point(258, 47);
		this.lblPartNo.Name = "lblPartNo";
		this.lblPartNo.Size = new System.Drawing.Size(562, 75);
		this.lblPartNo.TabIndex = 115;
		this.lblPartNo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.19f));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90.81f));
		this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.label33, 0, 0);
		this.tableLayoutPanel1.Location = new System.Drawing.Point(51, 340);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(1100, 211);
		this.tableLayoutPanel1.TabIndex = 192;
		this.tableLayoutPanel2.ColumnCount = 1;
		this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 0);
		this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 1);
		this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel2.Location = new System.Drawing.Point(101, 0);
		this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel2.Name = "tableLayoutPanel2";
		this.tableLayoutPanel2.RowCount = 2;
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33f));
		this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67f));
		this.tableLayoutPanel2.Size = new System.Drawing.Size(999, 211);
		this.tableLayoutPanel2.TabIndex = 193;
		this.tableLayoutPanel4.ColumnCount = 3;
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
		this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40f));
		this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 2, 0);
		this.tableLayoutPanel4.Controls.Add(this.label43, 0, 0);
		this.tableLayoutPanel4.Controls.Add(this.label46, 1, 0);
		this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel4.Name = "tableLayoutPanel4";
		this.tableLayoutPanel4.RowCount = 1;
		this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 69f));
		this.tableLayoutPanel4.Size = new System.Drawing.Size(999, 69);
		this.tableLayoutPanel4.TabIndex = 194;
		this.tableLayoutPanel5.ColumnCount = 1;
		this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 0, 1);
		this.tableLayoutPanel5.Controls.Add(this.label58, 0, 0);
		this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel5.Location = new System.Drawing.Point(598, 0);
		this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel5.Name = "tableLayoutPanel5";
		this.tableLayoutPanel5.RowCount = 2;
		this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel5.Size = new System.Drawing.Size(401, 69);
		this.tableLayoutPanel5.TabIndex = 193;
		this.tableLayoutPanel6.ColumnCount = 2;
		this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel6.Controls.Add(this.label60, 0, 0);
		this.tableLayoutPanel6.Controls.Add(this.label61, 1, 0);
		this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel6.Location = new System.Drawing.Point(0, 34);
		this.tableLayoutPanel6.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel6.Name = "tableLayoutPanel6";
		this.tableLayoutPanel6.RowCount = 1;
		this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel6.Size = new System.Drawing.Size(401, 35);
		this.tableLayoutPanel6.TabIndex = 194;
		this.label60.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label60.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label60.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label60.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label60.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.White;
		this.label60.Location = new System.Drawing.Point(3, 3);
		this.label60.Margin = new System.Windows.Forms.Padding(3);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(194, 29);
		this.label60.TabIndex = 168;
		this.label60.Text = "As-Is";
		this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label61.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label61.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label61.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label61.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label61.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.White;
		this.label61.Location = new System.Drawing.Point(203, 3);
		this.label61.Margin = new System.Windows.Forms.Padding(3);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(195, 29);
		this.label61.TabIndex = 175;
		this.label61.Text = "To-be";
		this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label58.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label58.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label58.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label58.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label58.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.White;
		this.label58.Location = new System.Drawing.Point(3, 0);
		this.label58.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(395, 31);
		this.label58.TabIndex = 184;
		this.label58.Text = "Qty";
		this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label43.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label43.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label43.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label43.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label43.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(3, 0);
		this.label43.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(293, 66);
		this.label43.TabIndex = 186;
		this.label43.Text = "W/O";
		this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label46.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label46.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label46.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label46.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label46.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.White;
		this.label46.Location = new System.Drawing.Point(302, 0);
		this.label46.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(293, 66);
		this.label46.TabIndex = 166;
		this.label46.Text = "Model.Suffix";
		this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tableLayoutPanel3.ColumnCount = 4;
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel3.Controls.Add(this.lblFAWorkorder_1, 0, 0);
		this.tableLayoutPanel3.Controls.Add(this.lblFATobeQty_4, 3, 3);
		this.tableLayoutPanel3.Controls.Add(this.lblFAPartNo_1, 1, 0);
		this.tableLayoutPanel3.Controls.Add(this.lblFAQty_4, 2, 3);
		this.tableLayoutPanel3.Controls.Add(this.lblFAPartNo_4, 1, 3);
		this.tableLayoutPanel3.Controls.Add(this.lblFAWorkorder_4, 0, 3);
		this.tableLayoutPanel3.Controls.Add(this.lblFATobeQty_3, 3, 2);
		this.tableLayoutPanel3.Controls.Add(this.lblFAQty_1, 2, 0);
		this.tableLayoutPanel3.Controls.Add(this.lblFATobeQty_2, 3, 1);
		this.tableLayoutPanel3.Controls.Add(this.lblFAWorkorder_3, 0, 2);
		this.tableLayoutPanel3.Controls.Add(this.lblFAPartNo_3, 1, 2);
		this.tableLayoutPanel3.Controls.Add(this.lblFAQty_3, 2, 2);
		this.tableLayoutPanel3.Controls.Add(this.lblFATobeQty_1, 3, 0);
		this.tableLayoutPanel3.Controls.Add(this.lblFAWorkorder_2, 0, 1);
		this.tableLayoutPanel3.Controls.Add(this.lblFAPartNo_2, 1, 1);
		this.tableLayoutPanel3.Controls.Add(this.lblFAQty_2, 2, 1);
		this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 69);
		this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel3.Name = "tableLayoutPanel3";
		this.tableLayoutPanel3.RowCount = 4;
		this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel3.Size = new System.Drawing.Size(999, 142);
		this.tableLayoutPanel3.TabIndex = 193;
		this.lblFAWorkorder_1.BackColor = System.Drawing.Color.White;
		this.lblFAWorkorder_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAWorkorder_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAWorkorder_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAWorkorder_1.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAWorkorder_1.ForeColor = System.Drawing.Color.Black;
		this.lblFAWorkorder_1.Location = new System.Drawing.Point(3, 3);
		this.lblFAWorkorder_1.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAWorkorder_1.Name = "lblFAWorkorder_1";
		this.lblFAWorkorder_1.Size = new System.Drawing.Size(293, 29);
		this.lblFAWorkorder_1.TabIndex = 181;
		this.lblFAWorkorder_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFATobeQty_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFATobeQty_4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFATobeQty_4.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.lblFATobeQty_4.Location = new System.Drawing.Point(800, 108);
		this.lblFATobeQty_4.Name = "lblFATobeQty_4";
		this.lblFATobeQty_4.Size = new System.Drawing.Size(196, 30);
		this.lblFATobeQty_4.TabIndex = 190;
		this.lblFATobeQty_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblFATobeQty_4.TextChanged += new System.EventHandler(lblTobeQty_1_TextChanged);
		this.lblFAPartNo_1.BackColor = System.Drawing.Color.White;
		this.lblFAPartNo_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAPartNo_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAPartNo_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAPartNo_1.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAPartNo_1.ForeColor = System.Drawing.Color.Black;
		this.lblFAPartNo_1.Location = new System.Drawing.Point(302, 3);
		this.lblFAPartNo_1.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAPartNo_1.Name = "lblFAPartNo_1";
		this.lblFAPartNo_1.Size = new System.Drawing.Size(293, 29);
		this.lblFAPartNo_1.TabIndex = 167;
		this.lblFAPartNo_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFAQty_4.BackColor = System.Drawing.Color.White;
		this.lblFAQty_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAQty_4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAQty_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAQty_4.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAQty_4.ForeColor = System.Drawing.Color.Black;
		this.lblFAQty_4.Location = new System.Drawing.Point(601, 108);
		this.lblFAQty_4.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAQty_4.Name = "lblFAQty_4";
		this.lblFAQty_4.Size = new System.Drawing.Size(193, 31);
		this.lblFAQty_4.TabIndex = 173;
		this.lblFAQty_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFAPartNo_4.BackColor = System.Drawing.Color.White;
		this.lblFAPartNo_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAPartNo_4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAPartNo_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAPartNo_4.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAPartNo_4.ForeColor = System.Drawing.Color.Black;
		this.lblFAPartNo_4.Location = new System.Drawing.Point(302, 108);
		this.lblFAPartNo_4.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAPartNo_4.Name = "lblFAPartNo_4";
		this.lblFAPartNo_4.Size = new System.Drawing.Size(293, 31);
		this.lblFAPartNo_4.TabIndex = 180;
		this.lblFAPartNo_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFAWorkorder_4.BackColor = System.Drawing.Color.White;
		this.lblFAWorkorder_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAWorkorder_4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAWorkorder_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAWorkorder_4.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAWorkorder_4.ForeColor = System.Drawing.Color.Black;
		this.lblFAWorkorder_4.Location = new System.Drawing.Point(3, 108);
		this.lblFAWorkorder_4.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAWorkorder_4.Name = "lblFAWorkorder_4";
		this.lblFAWorkorder_4.Size = new System.Drawing.Size(293, 31);
		this.lblFAWorkorder_4.TabIndex = 185;
		this.lblFAWorkorder_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFATobeQty_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFATobeQty_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFATobeQty_3.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.lblFATobeQty_3.Location = new System.Drawing.Point(800, 73);
		this.lblFATobeQty_3.Name = "lblFATobeQty_3";
		this.lblFATobeQty_3.Size = new System.Drawing.Size(196, 30);
		this.lblFATobeQty_3.TabIndex = 189;
		this.lblFATobeQty_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblFATobeQty_3.TextChanged += new System.EventHandler(lblTobeQty_1_TextChanged);
		this.lblFAQty_1.BackColor = System.Drawing.Color.White;
		this.lblFAQty_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAQty_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAQty_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAQty_1.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAQty_1.ForeColor = System.Drawing.Color.Black;
		this.lblFAQty_1.Location = new System.Drawing.Point(601, 3);
		this.lblFAQty_1.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAQty_1.Name = "lblFAQty_1";
		this.lblFAQty_1.Size = new System.Drawing.Size(193, 29);
		this.lblFAQty_1.TabIndex = 170;
		this.lblFAQty_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFATobeQty_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFATobeQty_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFATobeQty_2.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.lblFATobeQty_2.Location = new System.Drawing.Point(800, 38);
		this.lblFATobeQty_2.Name = "lblFATobeQty_2";
		this.lblFATobeQty_2.Size = new System.Drawing.Size(196, 30);
		this.lblFATobeQty_2.TabIndex = 188;
		this.lblFATobeQty_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblFATobeQty_2.TextChanged += new System.EventHandler(lblTobeQty_1_TextChanged);
		this.lblFAWorkorder_3.BackColor = System.Drawing.Color.White;
		this.lblFAWorkorder_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAWorkorder_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAWorkorder_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAWorkorder_3.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAWorkorder_3.ForeColor = System.Drawing.Color.Black;
		this.lblFAWorkorder_3.Location = new System.Drawing.Point(3, 73);
		this.lblFAWorkorder_3.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAWorkorder_3.Name = "lblFAWorkorder_3";
		this.lblFAWorkorder_3.Size = new System.Drawing.Size(293, 29);
		this.lblFAWorkorder_3.TabIndex = 183;
		this.lblFAWorkorder_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFAPartNo_3.BackColor = System.Drawing.Color.White;
		this.lblFAPartNo_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAPartNo_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAPartNo_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAPartNo_3.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAPartNo_3.ForeColor = System.Drawing.Color.Black;
		this.lblFAPartNo_3.Location = new System.Drawing.Point(302, 73);
		this.lblFAPartNo_3.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAPartNo_3.Name = "lblFAPartNo_3";
		this.lblFAPartNo_3.Size = new System.Drawing.Size(293, 29);
		this.lblFAPartNo_3.TabIndex = 174;
		this.lblFAPartNo_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFAQty_3.BackColor = System.Drawing.Color.White;
		this.lblFAQty_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAQty_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAQty_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAQty_3.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAQty_3.ForeColor = System.Drawing.Color.Black;
		this.lblFAQty_3.Location = new System.Drawing.Point(601, 73);
		this.lblFAQty_3.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAQty_3.Name = "lblFAQty_3";
		this.lblFAQty_3.Size = new System.Drawing.Size(193, 29);
		this.lblFAQty_3.TabIndex = 172;
		this.lblFAQty_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFATobeQty_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFATobeQty_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFATobeQty_1.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.lblFATobeQty_1.Location = new System.Drawing.Point(800, 3);
		this.lblFATobeQty_1.Name = "lblFATobeQty_1";
		this.lblFATobeQty_1.Size = new System.Drawing.Size(196, 30);
		this.lblFATobeQty_1.TabIndex = 187;
		this.lblFATobeQty_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblFATobeQty_1.TextChanged += new System.EventHandler(lblTobeQty_1_TextChanged);
		this.lblFAWorkorder_2.BackColor = System.Drawing.Color.White;
		this.lblFAWorkorder_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAWorkorder_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAWorkorder_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAWorkorder_2.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAWorkorder_2.ForeColor = System.Drawing.Color.Black;
		this.lblFAWorkorder_2.Location = new System.Drawing.Point(3, 38);
		this.lblFAWorkorder_2.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAWorkorder_2.Name = "lblFAWorkorder_2";
		this.lblFAWorkorder_2.Size = new System.Drawing.Size(293, 29);
		this.lblFAWorkorder_2.TabIndex = 182;
		this.lblFAWorkorder_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFAPartNo_2.BackColor = System.Drawing.Color.White;
		this.lblFAPartNo_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAPartNo_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAPartNo_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAPartNo_2.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAPartNo_2.ForeColor = System.Drawing.Color.Black;
		this.lblFAPartNo_2.Location = new System.Drawing.Point(302, 38);
		this.lblFAPartNo_2.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAPartNo_2.Name = "lblFAPartNo_2";
		this.lblFAPartNo_2.Size = new System.Drawing.Size(293, 29);
		this.lblFAPartNo_2.TabIndex = 169;
		this.lblFAPartNo_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblFAQty_2.BackColor = System.Drawing.Color.White;
		this.lblFAQty_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblFAQty_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblFAQty_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblFAQty_2.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblFAQty_2.ForeColor = System.Drawing.Color.Black;
		this.lblFAQty_2.Location = new System.Drawing.Point(601, 38);
		this.lblFAQty_2.Margin = new System.Windows.Forms.Padding(3);
		this.lblFAQty_2.Name = "lblFAQty_2";
		this.lblFAQty_2.Size = new System.Drawing.Size(193, 29);
		this.lblFAQty_2.TabIndex = 171;
		this.lblFAQty_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label33.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label33.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label33.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label33.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(3, 0);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(95, 211);
		this.label33.TabIndex = 177;
		this.label33.Text = "FA";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tableLayoutPanel7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.tableLayoutPanel7.ColumnCount = 2;
		this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.190173f));
		this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 90.80983f));
		this.tableLayoutPanel7.Controls.Add(this.tableLayoutPanel8, 1, 0);
		this.tableLayoutPanel7.Controls.Add(this.label2, 0, 0);
		this.tableLayoutPanel7.Location = new System.Drawing.Point(51, 7);
		this.tableLayoutPanel7.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel7.Name = "tableLayoutPanel7";
		this.tableLayoutPanel7.RowCount = 1;
		this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 324f));
		this.tableLayoutPanel7.Size = new System.Drawing.Size(1100, 324);
		this.tableLayoutPanel7.TabIndex = 165;
		this.tableLayoutPanel8.ColumnCount = 1;
		this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel10, 0, 1);
		this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel13, 0, 0);
		this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel14, 0, 2);
		this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel15, 0, 3);
		this.tableLayoutPanel8.Controls.Add(this.tableLayoutPanel16, 0, 4);
		this.tableLayoutPanel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel8.Location = new System.Drawing.Point(101, 0);
		this.tableLayoutPanel8.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel8.Name = "tableLayoutPanel8";
		this.tableLayoutPanel8.RowCount = 5;
		this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36f));
		this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72f));
		this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 144f));
		this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35f));
		this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24f));
		this.tableLayoutPanel8.Size = new System.Drawing.Size(999, 324);
		this.tableLayoutPanel8.TabIndex = 2;
		this.tableLayoutPanel10.ColumnCount = 4;
		this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel10.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40f));
		this.tableLayoutPanel10.Controls.Add(this.label4, 0, 0);
		this.tableLayoutPanel10.Controls.Add(this.tableLayoutPanel11, 3, 0);
		this.tableLayoutPanel10.Controls.Add(this.label10, 1, 0);
		this.tableLayoutPanel10.Controls.Add(this.label1, 2, 0);
		this.tableLayoutPanel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel10.Location = new System.Drawing.Point(0, 36);
		this.tableLayoutPanel10.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel10.Name = "tableLayoutPanel10";
		this.tableLayoutPanel10.RowCount = 1;
		this.tableLayoutPanel10.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel10.Size = new System.Drawing.Size(999, 72);
		this.tableLayoutPanel10.TabIndex = 2;
		this.label4.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label4.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Margin = new System.Windows.Forms.Padding(3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(193, 66);
		this.label4.TabIndex = 137;
		this.label4.Text = "Work Order";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tableLayoutPanel11.ColumnCount = 1;
		this.tableLayoutPanel11.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel11.Controls.Add(this.tableLayoutPanel12, 0, 1);
		this.tableLayoutPanel11.Controls.Add(this.label8, 0, 0);
		this.tableLayoutPanel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel11.Location = new System.Drawing.Point(597, 0);
		this.tableLayoutPanel11.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel11.Name = "tableLayoutPanel11";
		this.tableLayoutPanel11.RowCount = 2;
		this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel11.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
		this.tableLayoutPanel11.Size = new System.Drawing.Size(402, 72);
		this.tableLayoutPanel11.TabIndex = 165;
		this.tableLayoutPanel12.ColumnCount = 2;
		this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel12.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel12.Controls.Add(this.label6, 0, 0);
		this.tableLayoutPanel12.Controls.Add(this.label7, 1, 0);
		this.tableLayoutPanel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel12.Location = new System.Drawing.Point(0, 36);
		this.tableLayoutPanel12.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel12.Name = "tableLayoutPanel12";
		this.tableLayoutPanel12.RowCount = 1;
		this.tableLayoutPanel12.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel12.Size = new System.Drawing.Size(402, 36);
		this.tableLayoutPanel12.TabIndex = 166;
		this.label6.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label6.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 3);
		this.label6.Margin = new System.Windows.Forms.Padding(3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(195, 30);
		this.label6.TabIndex = 124;
		this.label6.Text = "As-Is";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label7.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(204, 3);
		this.label7.Margin = new System.Windows.Forms.Padding(3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(195, 30);
		this.label7.TabIndex = 161;
		this.label7.Text = "To-be";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label8.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 3);
		this.label8.Margin = new System.Windows.Forms.Padding(3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(396, 30);
		this.label8.TabIndex = 153;
		this.label8.Text = "Qty";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label10.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label10.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label10.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(202, 3);
		this.label10.Margin = new System.Windows.Forms.Padding(3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(193, 66);
		this.label10.TabIndex = 135;
		this.label10.Text = "Plan Qty";
		this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label1.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label1.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(401, 3);
		this.label1.Margin = new System.Windows.Forms.Padding(3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(193, 66);
		this.label1.TabIndex = 128;
		this.label1.Text = "Plan Date";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tableLayoutPanel13.ColumnCount = 5;
		this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel13.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel13.Controls.Add(this.label3, 0, 0);
		this.tableLayoutPanel13.Controls.Add(this.label5, 2, 0);
		this.tableLayoutPanel13.Controls.Add(this.lblLine, 3, 0);
		this.tableLayoutPanel13.Controls.Add(this.lblProdDate, 4, 0);
		this.tableLayoutPanel13.Controls.Add(this.lblOrg, 1, 0);
		this.tableLayoutPanel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel13.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel13.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel13.Name = "tableLayoutPanel13";
		this.tableLayoutPanel13.RowCount = 1;
		this.tableLayoutPanel13.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel13.Size = new System.Drawing.Size(999, 36);
		this.tableLayoutPanel13.TabIndex = 3;
		this.label3.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label3.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 0);
		this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(193, 33);
		this.label3.TabIndex = 145;
		this.label3.Text = "Org";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label5.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label5.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(401, 0);
		this.label5.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(193, 33);
		this.label5.TabIndex = 133;
		this.label5.Text = "Line/Prod";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblLine.BackColor = System.Drawing.Color.White;
		this.lblLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblLine.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblLine.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLine.ForeColor = System.Drawing.Color.Black;
		this.lblLine.Location = new System.Drawing.Point(600, 0);
		this.lblLine.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
		this.lblLine.Name = "lblLine";
		this.lblLine.Size = new System.Drawing.Size(193, 33);
		this.lblLine.TabIndex = 151;
		this.lblLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblProdDate.BackColor = System.Drawing.Color.White;
		this.lblProdDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblProdDate.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblProdDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblProdDate.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblProdDate.ForeColor = System.Drawing.Color.Black;
		this.lblProdDate.Location = new System.Drawing.Point(799, 0);
		this.lblProdDate.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
		this.lblProdDate.Name = "lblProdDate";
		this.lblProdDate.Size = new System.Drawing.Size(197, 33);
		this.lblProdDate.TabIndex = 150;
		this.lblProdDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblOrg.BackColor = System.Drawing.Color.White;
		this.lblOrg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblOrg.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblOrg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblOrg.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblOrg.ForeColor = System.Drawing.Color.Black;
		this.lblOrg.Location = new System.Drawing.Point(202, 0);
		this.lblOrg.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
		this.lblOrg.Name = "lblOrg";
		this.lblOrg.Size = new System.Drawing.Size(193, 33);
		this.lblOrg.TabIndex = 131;
		this.lblOrg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tableLayoutPanel14.ColumnCount = 5;
		this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel14.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel14.Controls.Add(this.lblWorkorder_1, 0, 0);
		this.tableLayoutPanel14.Controls.Add(this.lblPlanQty_1, 1, 0);
		this.tableLayoutPanel14.Controls.Add(this.lblPlanDate_1, 2, 0);
		this.tableLayoutPanel14.Controls.Add(this.lblPreQty_1b, 3, 0);
		this.tableLayoutPanel14.Controls.Add(this.lblWorkorder_2, 0, 1);
		this.tableLayoutPanel14.Controls.Add(this.lblTobeQty_4, 4, 3);
		this.tableLayoutPanel14.Controls.Add(this.lblPlanQty_2, 1, 1);
		this.tableLayoutPanel14.Controls.Add(this.lblPlanDate_2, 2, 1);
		this.tableLayoutPanel14.Controls.Add(this.lblPreQty_4b, 3, 3);
		this.tableLayoutPanel14.Controls.Add(this.lblWorkorder_4, 0, 3);
		this.tableLayoutPanel14.Controls.Add(this.lblPlanDate_4, 2, 3);
		this.tableLayoutPanel14.Controls.Add(this.lblTobeQty_3, 4, 2);
		this.tableLayoutPanel14.Controls.Add(this.lblPlanQty_4, 1, 3);
		this.tableLayoutPanel14.Controls.Add(this.lblTobeQty_1, 4, 0);
		this.tableLayoutPanel14.Controls.Add(this.lblWorkorder_3, 0, 2);
		this.tableLayoutPanel14.Controls.Add(this.lblTobeQty_2, 4, 1);
		this.tableLayoutPanel14.Controls.Add(this.lblPreQty_3b, 3, 2);
		this.tableLayoutPanel14.Controls.Add(this.lblPreQty_2b, 3, 1);
		this.tableLayoutPanel14.Controls.Add(this.lblPlanQty_3, 1, 2);
		this.tableLayoutPanel14.Controls.Add(this.lblPlanDate_3, 2, 2);
		this.tableLayoutPanel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel14.Location = new System.Drawing.Point(0, 108);
		this.tableLayoutPanel14.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel14.Name = "tableLayoutPanel14";
		this.tableLayoutPanel14.RowCount = 4;
		this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel14.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25f));
		this.tableLayoutPanel14.Size = new System.Drawing.Size(999, 144);
		this.tableLayoutPanel14.TabIndex = 1;
		this.lblWorkorder_1.BackColor = System.Drawing.Color.White;
		this.lblWorkorder_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblWorkorder_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWorkorder_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblWorkorder_1.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWorkorder_1.ForeColor = System.Drawing.Color.Black;
		this.lblWorkorder_1.Location = new System.Drawing.Point(3, 3);
		this.lblWorkorder_1.Margin = new System.Windows.Forms.Padding(3);
		this.lblWorkorder_1.Name = "lblWorkorder_1";
		this.lblWorkorder_1.Size = new System.Drawing.Size(193, 30);
		this.lblWorkorder_1.TabIndex = 138;
		this.lblWorkorder_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPlanQty_1.BackColor = System.Drawing.Color.White;
		this.lblPlanQty_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPlanQty_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanQty_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPlanQty_1.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanQty_1.ForeColor = System.Drawing.Color.Black;
		this.lblPlanQty_1.Location = new System.Drawing.Point(202, 3);
		this.lblPlanQty_1.Margin = new System.Windows.Forms.Padding(3);
		this.lblPlanQty_1.Name = "lblPlanQty_1";
		this.lblPlanQty_1.Size = new System.Drawing.Size(193, 30);
		this.lblPlanQty_1.TabIndex = 126;
		this.lblPlanQty_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPlanDate_1.BackColor = System.Drawing.Color.White;
		this.lblPlanDate_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPlanDate_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanDate_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPlanDate_1.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanDate_1.ForeColor = System.Drawing.Color.Black;
		this.lblPlanDate_1.Location = new System.Drawing.Point(401, 3);
		this.lblPlanDate_1.Margin = new System.Windows.Forms.Padding(3);
		this.lblPlanDate_1.Name = "lblPlanDate_1";
		this.lblPlanDate_1.Size = new System.Drawing.Size(193, 30);
		this.lblPlanDate_1.TabIndex = 130;
		this.lblPlanDate_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPreQty_1b.BackColor = System.Drawing.Color.White;
		this.lblPreQty_1b.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPreQty_1b.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPreQty_1b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPreQty_1b.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPreQty_1b.ForeColor = System.Drawing.Color.Black;
		this.lblPreQty_1b.Location = new System.Drawing.Point(600, 3);
		this.lblPreQty_1b.Margin = new System.Windows.Forms.Padding(3);
		this.lblPreQty_1b.Name = "lblPreQty_1b";
		this.lblPreQty_1b.Size = new System.Drawing.Size(193, 30);
		this.lblPreQty_1b.TabIndex = 148;
		this.lblPreQty_1b.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblWorkorder_2.BackColor = System.Drawing.Color.White;
		this.lblWorkorder_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblWorkorder_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWorkorder_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblWorkorder_2.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWorkorder_2.ForeColor = System.Drawing.Color.Black;
		this.lblWorkorder_2.Location = new System.Drawing.Point(3, 39);
		this.lblWorkorder_2.Margin = new System.Windows.Forms.Padding(3);
		this.lblWorkorder_2.Name = "lblWorkorder_2";
		this.lblWorkorder_2.Size = new System.Drawing.Size(193, 30);
		this.lblWorkorder_2.TabIndex = 136;
		this.lblWorkorder_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTobeQty_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTobeQty_4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTobeQty_4.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.lblTobeQty_4.Location = new System.Drawing.Point(799, 111);
		this.lblTobeQty_4.Name = "lblTobeQty_4";
		this.lblTobeQty_4.Size = new System.Drawing.Size(197, 30);
		this.lblTobeQty_4.TabIndex = 162;
		this.lblTobeQty_4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblTobeQty_4.TextChanged += new System.EventHandler(lblTobeQty_1_TextChanged);
		this.lblPlanQty_2.BackColor = System.Drawing.Color.White;
		this.lblPlanQty_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPlanQty_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanQty_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPlanQty_2.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanQty_2.ForeColor = System.Drawing.Color.Black;
		this.lblPlanQty_2.Location = new System.Drawing.Point(202, 39);
		this.lblPlanQty_2.Margin = new System.Windows.Forms.Padding(3);
		this.lblPlanQty_2.Name = "lblPlanQty_2";
		this.lblPlanQty_2.Size = new System.Drawing.Size(193, 30);
		this.lblPlanQty_2.TabIndex = 140;
		this.lblPlanQty_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPlanDate_2.BackColor = System.Drawing.Color.White;
		this.lblPlanDate_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPlanDate_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanDate_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPlanDate_2.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanDate_2.ForeColor = System.Drawing.Color.Black;
		this.lblPlanDate_2.Location = new System.Drawing.Point(401, 39);
		this.lblPlanDate_2.Margin = new System.Windows.Forms.Padding(3);
		this.lblPlanDate_2.Name = "lblPlanDate_2";
		this.lblPlanDate_2.Size = new System.Drawing.Size(193, 30);
		this.lblPlanDate_2.TabIndex = 127;
		this.lblPlanDate_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPreQty_4b.BackColor = System.Drawing.Color.White;
		this.lblPreQty_4b.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPreQty_4b.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPreQty_4b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPreQty_4b.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPreQty_4b.ForeColor = System.Drawing.Color.Black;
		this.lblPreQty_4b.Location = new System.Drawing.Point(600, 111);
		this.lblPreQty_4b.Margin = new System.Windows.Forms.Padding(3);
		this.lblPreQty_4b.Name = "lblPreQty_4b";
		this.lblPreQty_4b.Size = new System.Drawing.Size(193, 30);
		this.lblPreQty_4b.TabIndex = 144;
		this.lblPreQty_4b.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblWorkorder_4.BackColor = System.Drawing.Color.White;
		this.lblWorkorder_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblWorkorder_4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWorkorder_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblWorkorder_4.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWorkorder_4.ForeColor = System.Drawing.Color.Black;
		this.lblWorkorder_4.Location = new System.Drawing.Point(3, 111);
		this.lblWorkorder_4.Margin = new System.Windows.Forms.Padding(3);
		this.lblWorkorder_4.Name = "lblWorkorder_4";
		this.lblWorkorder_4.Size = new System.Drawing.Size(193, 30);
		this.lblWorkorder_4.TabIndex = 132;
		this.lblWorkorder_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPlanDate_4.BackColor = System.Drawing.Color.White;
		this.lblPlanDate_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPlanDate_4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanDate_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPlanDate_4.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanDate_4.ForeColor = System.Drawing.Color.Black;
		this.lblPlanDate_4.Location = new System.Drawing.Point(401, 111);
		this.lblPlanDate_4.Margin = new System.Windows.Forms.Padding(3);
		this.lblPlanDate_4.Name = "lblPlanDate_4";
		this.lblPlanDate_4.Size = new System.Drawing.Size(193, 30);
		this.lblPlanDate_4.TabIndex = 123;
		this.lblPlanDate_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTobeQty_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTobeQty_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTobeQty_3.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.lblTobeQty_3.Location = new System.Drawing.Point(799, 75);
		this.lblTobeQty_3.Name = "lblTobeQty_3";
		this.lblTobeQty_3.Size = new System.Drawing.Size(197, 30);
		this.lblTobeQty_3.TabIndex = 161;
		this.lblTobeQty_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblTobeQty_3.TextChanged += new System.EventHandler(lblTobeQty_1_TextChanged);
		this.lblPlanQty_4.BackColor = System.Drawing.Color.White;
		this.lblPlanQty_4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPlanQty_4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanQty_4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPlanQty_4.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanQty_4.ForeColor = System.Drawing.Color.Black;
		this.lblPlanQty_4.Location = new System.Drawing.Point(202, 111);
		this.lblPlanQty_4.Margin = new System.Windows.Forms.Padding(3);
		this.lblPlanQty_4.Name = "lblPlanQty_4";
		this.lblPlanQty_4.Size = new System.Drawing.Size(193, 30);
		this.lblPlanQty_4.TabIndex = 152;
		this.lblPlanQty_4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTobeQty_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTobeQty_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTobeQty_1.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.lblTobeQty_1.Location = new System.Drawing.Point(799, 3);
		this.lblTobeQty_1.Name = "lblTobeQty_1";
		this.lblTobeQty_1.Size = new System.Drawing.Size(197, 30);
		this.lblTobeQty_1.TabIndex = 163;
		this.lblTobeQty_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblTobeQty_1.TextChanged += new System.EventHandler(lblTobeQty_1_TextChanged);
		this.lblWorkorder_3.BackColor = System.Drawing.Color.White;
		this.lblWorkorder_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblWorkorder_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblWorkorder_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblWorkorder_3.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblWorkorder_3.ForeColor = System.Drawing.Color.Black;
		this.lblWorkorder_3.Location = new System.Drawing.Point(3, 75);
		this.lblWorkorder_3.Margin = new System.Windows.Forms.Padding(3);
		this.lblWorkorder_3.Name = "lblWorkorder_3";
		this.lblWorkorder_3.Size = new System.Drawing.Size(193, 30);
		this.lblWorkorder_3.TabIndex = 134;
		this.lblWorkorder_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTobeQty_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblTobeQty_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblTobeQty_2.Font = new System.Drawing.Font("Arial", 15f, System.Drawing.FontStyle.Bold);
		this.lblTobeQty_2.Location = new System.Drawing.Point(799, 39);
		this.lblTobeQty_2.Name = "lblTobeQty_2";
		this.lblTobeQty_2.Size = new System.Drawing.Size(197, 30);
		this.lblTobeQty_2.TabIndex = 160;
		this.lblTobeQty_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblTobeQty_2.TextChanged += new System.EventHandler(lblTobeQty_1_TextChanged);
		this.lblPreQty_3b.BackColor = System.Drawing.Color.White;
		this.lblPreQty_3b.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPreQty_3b.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPreQty_3b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPreQty_3b.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPreQty_3b.ForeColor = System.Drawing.Color.Black;
		this.lblPreQty_3b.Location = new System.Drawing.Point(600, 75);
		this.lblPreQty_3b.Margin = new System.Windows.Forms.Padding(3);
		this.lblPreQty_3b.Name = "lblPreQty_3b";
		this.lblPreQty_3b.Size = new System.Drawing.Size(193, 30);
		this.lblPreQty_3b.TabIndex = 149;
		this.lblPreQty_3b.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPreQty_2b.BackColor = System.Drawing.Color.White;
		this.lblPreQty_2b.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPreQty_2b.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPreQty_2b.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPreQty_2b.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPreQty_2b.ForeColor = System.Drawing.Color.Black;
		this.lblPreQty_2b.Location = new System.Drawing.Point(600, 39);
		this.lblPreQty_2b.Margin = new System.Windows.Forms.Padding(3);
		this.lblPreQty_2b.Name = "lblPreQty_2b";
		this.lblPreQty_2b.Size = new System.Drawing.Size(193, 30);
		this.lblPreQty_2b.TabIndex = 146;
		this.lblPreQty_2b.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPlanQty_3.BackColor = System.Drawing.Color.White;
		this.lblPlanQty_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPlanQty_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanQty_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPlanQty_3.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanQty_3.ForeColor = System.Drawing.Color.Black;
		this.lblPlanQty_3.Location = new System.Drawing.Point(202, 75);
		this.lblPlanQty_3.Margin = new System.Windows.Forms.Padding(3);
		this.lblPlanQty_3.Name = "lblPlanQty_3";
		this.lblPlanQty_3.Size = new System.Drawing.Size(193, 30);
		this.lblPlanQty_3.TabIndex = 143;
		this.lblPlanQty_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPlanDate_3.BackColor = System.Drawing.Color.White;
		this.lblPlanDate_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPlanDate_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPlanDate_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPlanDate_3.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPlanDate_3.ForeColor = System.Drawing.Color.Black;
		this.lblPlanDate_3.Location = new System.Drawing.Point(401, 75);
		this.lblPlanDate_3.Margin = new System.Windows.Forms.Padding(3);
		this.lblPlanDate_3.Name = "lblPlanDate_3";
		this.lblPlanDate_3.Size = new System.Drawing.Size(193, 30);
		this.lblPlanDate_3.TabIndex = 125;
		this.lblPlanDate_3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tableLayoutPanel15.ColumnCount = 2;
		this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.02002f));
		this.tableLayoutPanel15.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 79.97998f));
		this.tableLayoutPanel15.Controls.Add(this.label14, 0, 0);
		this.tableLayoutPanel15.Controls.Add(this.lblModel, 1, 0);
		this.tableLayoutPanel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel15.Location = new System.Drawing.Point(0, 252);
		this.tableLayoutPanel15.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel15.Name = "tableLayoutPanel15";
		this.tableLayoutPanel15.RowCount = 1;
		this.tableLayoutPanel15.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50f));
		this.tableLayoutPanel15.Size = new System.Drawing.Size(999, 35);
		this.tableLayoutPanel15.TabIndex = 1;
		this.label14.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label14.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 3);
		this.label14.Margin = new System.Windows.Forms.Padding(3);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(193, 29);
		this.label14.TabIndex = 139;
		this.label14.Text = "Apply Model";
		this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblModel.BackColor = System.Drawing.Color.White;
		this.lblModel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblModel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblModel.Font = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Bold);
		this.lblModel.ForeColor = System.Drawing.Color.Black;
		this.lblModel.Location = new System.Drawing.Point(202, 3);
		this.lblModel.Margin = new System.Windows.Forms.Padding(3);
		this.lblModel.Name = "lblModel";
		this.lblModel.Size = new System.Drawing.Size(794, 29);
		this.lblModel.TabIndex = 154;
		this.lblModel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
		this.tableLayoutPanel16.ColumnCount = 3;
		this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20f));
		this.tableLayoutPanel16.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60f));
		this.tableLayoutPanel16.Controls.Add(this.lblPartNo_1, 0, 0);
		this.tableLayoutPanel16.Controls.Add(this.lblPartNo_3, 2, 0);
		this.tableLayoutPanel16.Controls.Add(this.lblPartNo_2, 1, 0);
		this.tableLayoutPanel16.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel16.Location = new System.Drawing.Point(0, 287);
		this.tableLayoutPanel16.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel16.Name = "tableLayoutPanel16";
		this.tableLayoutPanel16.RowCount = 1;
		this.tableLayoutPanel16.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
		this.tableLayoutPanel16.Size = new System.Drawing.Size(999, 37);
		this.tableLayoutPanel16.TabIndex = 1;
		this.lblPartNo_1.BackColor = System.Drawing.Color.White;
		this.lblPartNo_1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPartNo_1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPartNo_1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPartNo_1.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPartNo_1.ForeColor = System.Drawing.Color.Black;
		this.lblPartNo_1.Location = new System.Drawing.Point(3, 3);
		this.lblPartNo_1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
		this.lblPartNo_1.Name = "lblPartNo_1";
		this.lblPartNo_1.Size = new System.Drawing.Size(193, 34);
		this.lblPartNo_1.TabIndex = 129;
		this.lblPartNo_1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblPartNo_3.BackColor = System.Drawing.Color.White;
		this.lblPartNo_3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPartNo_3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPartNo_3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPartNo_3.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPartNo_3.ForeColor = System.Drawing.Color.Black;
		this.lblPartNo_3.Location = new System.Drawing.Point(401, 3);
		this.lblPartNo_3.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
		this.lblPartNo_3.Name = "lblPartNo_3";
		this.lblPartNo_3.Size = new System.Drawing.Size(595, 34);
		this.lblPartNo_3.TabIndex = 122;
		this.lblPartNo_3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblPartNo_2.BackColor = System.Drawing.Color.White;
		this.lblPartNo_2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPartNo_2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblPartNo_2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.lblPartNo_2.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPartNo_2.ForeColor = System.Drawing.Color.Black;
		this.lblPartNo_2.Location = new System.Drawing.Point(202, 3);
		this.lblPartNo_2.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
		this.lblPartNo_2.Name = "lblPartNo_2";
		this.lblPartNo_2.Size = new System.Drawing.Size(193, 34);
		this.lblPartNo_2.TabIndex = 147;
		this.lblPartNo_2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label2.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(95, 324);
		this.label2.TabIndex = 142;
		this.label2.Text = "Injection    /  Sub";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.textBox9.Font = new System.Drawing.Font("Arial", 22f);
		this.textBox9.Location = new System.Drawing.Point(1032, 80);
		this.textBox9.Name = "textBox9";
		this.textBox9.Size = new System.Drawing.Size(203, 41);
		this.textBox9.TabIndex = 90;
		this.textBox9.Text = "Sheet ID ";
		this.textBox9.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
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
		this.splitContainerMain.TabIndex = 75;
		this.backPanel21.BackColor = System.Drawing.Color.FromArgb(224, 224, 224, 224);
		this.backPanel21.BackgroundImage = MCS.PrintBoard.Properties.Resources.background;
		this.backPanel21.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.backPanel21.Controls.Add(this.btnSave);
		this.backPanel21.Controls.Add(this.label9);
		this.backPanel21.Controls.Add(this.textBox9);
		this.backPanel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.backPanel21.Location = new System.Drawing.Point(0, 0);
		this.backPanel21.Margin = new System.Windows.Forms.Padding(0);
		this.backPanel21.Name = "backPanel21";
		this.backPanel21.Padding = new System.Windows.Forms.Padding(0);
		this.backPanel21.Size = new System.Drawing.Size(1251, 71);
		this.backPanel21.TabIndex = 107;
		this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSave.BackgroundImage = (System.Drawing.Image)resources.GetObject("btnSave.BackgroundImage");
		this.btnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnSave.Font = new System.Drawing.Font("Arial", 9f);
		this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSave.Location = new System.Drawing.Point(1056, 32);
		this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(100, 30);
		this.btnSave.TabIndex = 115;
		this.btnSave.Text = "SAVE ＆ PRINT";
		this.btnSave.UseVisualStyleBackColor = true;
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.label9.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label9.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(51, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(270, 38);
		this.label9.TabIndex = 115;
		this.label9.Text = "Qty Change ＆ Print";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.dgvBuffer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvBuffer.Location = new System.Drawing.Point(491, 12);
		this.dgvBuffer.Name = "dgvBuffer";
		this.dgvBuffer.RowTemplate.Height = 23;
		this.dgvBuffer.Size = new System.Drawing.Size(121, 45);
		this.dgvBuffer.TabIndex = 74;
		this.dgvBuffer.Visible = false;
		this.tmRefresh.Interval = 5000;
		this.searchPanel1.BackColor = System.Drawing.Color.Red;
		this.searchPanel1.BackgroundImage = (System.Drawing.Image)resources.GetObject("searchPanel1.BackgroundImage");
		this.searchPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.searchPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.searchPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.searchPanel1.Location = new System.Drawing.Point(0, 0);
		this.searchPanel1.Name = "searchPanel1";
		this.searchPanel1.Padding = new System.Windows.Forms.Padding(8);
		this.searchPanel1.Size = new System.Drawing.Size(1251, 761);
		this.searchPanel1.TabIndex = 73;
		base.AutoScaleDimensions = new System.Drawing.SizeF(96f, 96f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(1251, 761);
		base.Controls.Add(this.splitContainerMain);
		base.Controls.Add(this.dgvBuffer);
		base.Controls.Add(this.searchPanel1);
		base.Name = "frmQtyChangePrint";
		this.Text = "frmQtyChangePrint";
		base.Load += new System.EventHandler(frmQtyChangePrint_Load);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel1.PerformLayout();
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel2.ResumeLayout(false);
		this.tableLayoutPanel4.ResumeLayout(false);
		this.tableLayoutPanel5.ResumeLayout(false);
		this.tableLayoutPanel6.ResumeLayout(false);
		this.tableLayoutPanel3.ResumeLayout(false);
		this.tableLayoutPanel3.PerformLayout();
		this.tableLayoutPanel7.ResumeLayout(false);
		this.tableLayoutPanel8.ResumeLayout(false);
		this.tableLayoutPanel10.ResumeLayout(false);
		this.tableLayoutPanel11.ResumeLayout(false);
		this.tableLayoutPanel12.ResumeLayout(false);
		this.tableLayoutPanel13.ResumeLayout(false);
		this.tableLayoutPanel14.ResumeLayout(false);
		this.tableLayoutPanel14.PerformLayout();
		this.tableLayoutPanel15.ResumeLayout(false);
		this.tableLayoutPanel16.ResumeLayout(false);
		this.splitContainerMain.Panel1.ResumeLayout(false);
		this.splitContainerMain.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).EndInit();
		this.splitContainerMain.ResumeLayout(false);
		this.backPanel21.ResumeLayout(false);
		this.backPanel21.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).EndInit();
		base.ResumeLayout(false);
	}
}
