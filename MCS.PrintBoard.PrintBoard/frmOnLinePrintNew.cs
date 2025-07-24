using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FarPoint.Win;
using FarPoint.Win.Spread;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;
using MCS.Common.Controls;
using MCS.PrintBoard.Properties;
using PresentationControls;

namespace MCS.PrintBoard.PrintBoard;

public class frmOnLinePrintNew : frmBase
{
	private TextBox[] tbary = new TextBox[10];

	private string[] CurrentItem = new string[4];

	private int iWO = -1;

	private Random ran = new Random();

	private string sLocatorGroupCode;

	private string sLocatorGroupName;

	private string SOrg = "";

	private DataSet dsLocatorGroup = new DataSet();

	private DataSet dsLocator = new DataSet();

	private DataTable dtPrintMain = new DataTable();

	private DataTable dtPrintSave = new DataTable();

	private DataTable dtGroupBy = new DataTable();

	private IContainer components = null;

	private Timer tmDemo;

	private DataGridView dgvBuffer;

	public Timer tmRefresh;

	private SearchPanel searchPanel1;

	private SplitContainer splitContainerMain;

	private BackPanel backPanel2;

	private BackPanel backPanel1;

	private MCS.Common.FpSpread fpProdResult;

	private Label label6;

	private Label label7;

	private Label label4;

	private Label label5;

	private Label label3;

	private Label label2;

	private TextBox txtPartNo;

	private TextBox txtWorkOrder;

	private TextBox txtLocatorGroup;

	private PictureBox picLocatorGroup;

	private MCS.Common.ComboBox cboOrg;

	private CheckBoxComboBox cmbLocator;

	private CheckBoxComboBox cmbLocatorGroup;

	private DateTimePicker dtpFromDate;

	private DateTimePicker dtpToDate;

	private Label label10;

	private Label label11;

	private MCS.Common.ComboBox cboPartType;

	private SplitContainer splitContainer1;

	private SplitContainer splitContainer2;

	private MCS.Common.FpSpread fpPrintMain;

	private PanelOnly panelOnly1;

	private Label label9;

	private Panel panel1;

	private System.Windows.Forms.Button btn_search;

	private System.Windows.Forms.Button btn_preview;

	private Panel panel2;

	private BackPanel backPanel3;

	private BackPanel backPanel4;

	private MCS.Common.FpSpread fpPrintSave;

	private System.Windows.Forms.Button btn_Down;

	private System.Windows.Forms.Button btn_deleteRow;

	private Panel panel3;

	private UserBox userBox1;

	private Label label1;

	private RadioButton rdoWorkOrder;

	private RadioButton rdoCarrierQty;

	public frmOnLinePrintNew()
	{
		InitializeComponent();
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		SetComboInit();
		SetSpreadInit();
		procMakeSheetColumn();
		dtpFromDate.Value = DateTime.Now;
		dtpToDate.Value = DateTime.Now.AddDays(3.0);
		SOrg = cboOrg.SelectedValue.ToString();
	}

	private void cmbLocatorGroup_CheckBoxCheckedChanged(object sender, EventArgs e)
	{
		SetcmbLocator("INIT");
	}

	private void btn_Confirm(object sender, EventArgs e)
	{
		SyncData();
		if (GetDataFromfpPrintMain())
		{
			if (dtGroupBy.Rows.Count <= 0)
			{
				MessageBox.Show("There is no Data to Print!!");
			}
		}
		else
		{
			MessageBox.Show("There is no Data to Print!!");
		}
	}

	private void btn_deleteRow_Click(object sender, EventArgs e)
	{
		if (fpPrintSave.ActiveSheet.RowCount > 0)
		{
			int iActiveRowIndex = fpPrintSave.ActiveSheet.ActiveRowIndex;
			fpPrintSave.ActiveSheet.RemoveRows(iActiveRowIndex, 1);
		}
	}

	private void fpPrintMain_ButtonClicked(object sender, EditorNotifyEventArgs e)
	{
		try
		{
			if (e.Column != fpPrintMain.ActiveSheet.GetColumnIndex("CHK"))
			{
				return;
			}
			if (fpPrintMain.ActiveSheet.GetText(e.Row, fpPrintMain.ActiveSheet.GetColumnIndex("CHK")) == "True")
			{
				if (e.Column != fpPrintMain.ActiveSheet.GetColumnIndex("CHK"))
				{
					fpPrintMain.ActiveSheet.SetText(e.Row, fpPrintMain.ActiveSheet.GetColumnIndex("CHK"), "False");
				}
			}
			else if (e.Column != fpPrintMain.ActiveSheet.GetColumnIndex("CHK"))
			{
				fpPrintMain.ActiveSheet.SetText(e.Row, fpPrintMain.ActiveSheet.GetColumnIndex("CHK"), "True");
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
	}

	private void btn_Print_Click(object sender, EventArgs e)
	{
		if (Save())
		{
			if (GetDataFromfpPrintMain())
			{
				frmOnLinePrintPreview frm = new frmOnLinePrintPreview();
				frm.dtGroupBy = dtGroupBy;
				frm.ShowDialog();
			}
			else
			{
				MessageBox.Show("There is no Data to Print!!");
			}
		}
	}

	private void fpPrintMain_CellClick(object sender, CellClickEventArgs e)
	{
	}

	private void btn_Search_Click(object sender, EventArgs e)
	{
		SetSpreadInit();
		DataSet ds = new DataSet();
		BizService bizServer = new BizService();
		using DataSet ds2 = MakeDataSet("RQSTDT(ORG_ID:STRING,PLAN_DATE_FROM:STRING,PLAN_DATE_TO:STRING,TO_WO_NAME:STRING,ITEM_GROUP_CODE:STRING,PLAN_WO_NAME:STRING,WRO_ITEM_CODE:STRING,LANGID:STRING)");
		string sWORK_ORDER = txtWorkOrder.Text.Trim();
		string sPART_NO = txtPartNo.Text.Trim();
		string sPART_TYPE = cboPartType.SelectedValue.ToString();
		DataRow dr1 = ds2.Tables["RQSTDT"].NewRow();
		dr1["ORG_ID"] = cboOrg.SelectedValue.ToString();
		dr1["PLAN_DATE_FROM"] = dtpFromDate.Value.ToString("yyyyMMdd");
		dr1["PLAN_DATE_TO"] = dtpToDate.Value.ToString("yyyyMMdd");
		dr1["ITEM_GROUP_CODE"] = ((sPART_TYPE == "") ? null : sPART_TYPE);
		dr1["PLAN_WO_NAME"] = ((sWORK_ORDER == "") ? null : sWORK_ORDER);
		dr1["WRO_ITEM_CODE"] = ((sPART_NO == "") ? null : sPART_NO);
		dr1["LANGID"] = CultureInfo.CurrentUICulture.Name;
		ds2.Tables["RQSTDT"].Rows.Add(dr1);
		ds = bizServer.ExecBizRule("SEL_NEW_PRINT_SHEET_INFO", ds2, "RQSTDT", "RSLTDT");
		fpPrintMain.DataSource = ds.Tables["RSLTDT"];
		fpPrintMain.Refresh();
		frmBase.gstBarMain.Items[0].Text = " Search has been completed ! [ " + fpPrintMain.ActiveSheet.RowCount + "  Rows ]";
	}

	private void cboOrg_SelectedIndexChanged(object sender, EventArgs e)
	{
		SOrg = cboOrg.SelectedValue.ToString();
		ProcPartType();
	}

	private bool Save()
	{
		bool bReturn = false;
		if (fpPrintSave.ActiveSheet.Rows.Count == 0)
		{
			return bReturn;
		}
		if (MessageBox.Show("Do you want to print(save) ?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
		{
			return bReturn;
		}
		string sCurrGroupID = string.Empty;
		string sBeforeGroupID = string.Empty;
		string sGetTxnID = string.Empty;
		string sGetSheedID = string.Empty;
		int iSheetSumQty = 0;
		for (int i = 0; dtPrintSave.Rows.Count > i; i++)
		{
			sCurrGroupID = dtPrintSave.Rows[i]["GROUP_ID"].ToString();
			sGetTxnID = GetTxnID();
			if (string.IsNullOrEmpty(sGetTxnID))
			{
				return bReturn;
			}
			if (string.IsNullOrEmpty(sBeforeGroupID))
			{
				sBeforeGroupID = sCurrGroupID;
				sGetSheedID = GetSheedID();
				iSheetSumQty = GetSheetSumlQty(sCurrGroupID);
			}
			if (sCurrGroupID != sBeforeGroupID)
			{
				sBeforeGroupID = sCurrGroupID;
				sGetSheedID = GetSheedID();
				iSheetSumQty = GetSheetSumlQty(sCurrGroupID);
			}
			dtPrintSave.Rows[i]["GROUP_TXN_ID"] = sGetTxnID;
			dtPrintSave.Rows[i]["SHEET_ID"] = sGetSheedID;
			dtPrintSave.Rows[i]["TOTAL_QTY"] = iSheetSumQty;
		}
		SetDeleverySheet();
		return true;
	}

	private int GetSheetSumlQty(string sGroupID)
	{
		int iSheetSumQty = 0;
		int iSheetQty = 0;
		for (int i = 0; dtPrintSave.Rows.Count > i; i++)
		{
			if (dtPrintSave.Rows[i]["GROUP_ID"].ToString() == sGroupID)
			{
				int.TryParse(dtPrintSave.Rows[i]["QTY"].ToString(), out iSheetQty);
				iSheetSumQty += iSheetQty;
			}
		}
		return iSheetSumQty;
	}

	private bool SetDeleverySheet()
	{
		bool bReturn = false;
		string sReturnMsg = string.Empty;
		string sReturnCode = string.Empty;
		string sDIV = "INS";
		string sSHEET_ID = string.Empty;
		string sSHEET_TYPE = "MCS_ONLINE";
		string sMADE_LOCATOR = string.Empty;
		string sDELIVERY_TO = string.Empty;
		string sITEM_CODE = string.Empty;
		string sWO_NAME = string.Empty;
		int iTOTAL_QTY = 0;
		int iSEQ = 0;
		string sTO_WO_NAME = string.Empty;
		string sTO_ITEM_CODE = string.Empty;
		int iTO_WO_QTY = 0;
		string sPRINT_YN = string.Empty;
		string sSTATUS = string.Empty;
		string sCLOSED_YN = string.Empty;
		string sGROUP_TXN_ID = string.Empty;
		string sMADE_BY = string.Empty;
		string ATTRIBUTE = string.Empty;
		string ATTRIBUTE2 = string.Empty;
		string ATTRIBUTE3 = string.Empty;
		string ATTRIBUTE4 = string.Empty;
		string ATTRIBUTE5 = string.Empty;
		string sCREATED_BY = string.Empty;
		string sUPDATED_BY = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			for (int i = 0; dtPrintSave.Rows.Count > i; i++)
			{
				sDIV = "INS";
				sSHEET_ID = dtPrintSave.Rows[i]["SHEET_ID"].ToString();
				sSHEET_TYPE = "MCS_ONLINE";
				sMADE_LOCATOR = null;
				sDELIVERY_TO = dtPrintSave.Rows[i]["LOCATOR"].ToString();
				sITEM_CODE = dtPrintSave.Rows[i]["PART_NO"].ToString();
				sWO_NAME = null;
				iTOTAL_QTY = int.Parse(dtPrintSave.Rows[i]["TOTAL_QTY"].ToString());
				iSEQ = int.Parse(dtPrintSave.Rows[i]["PLAN_SEQ"].ToString());
				sTO_WO_NAME = dtPrintSave.Rows[i]["WORK_ORDER"].ToString();
				sTO_ITEM_CODE = null;
				iTO_WO_QTY = int.Parse(dtPrintSave.Rows[i]["QTY"].ToString());
				sPRINT_YN = "N";
				sSTATUS = null;
				sCLOSED_YN = "N";
				sGROUP_TXN_ID = dtPrintSave.Rows[i]["GROUP_TXN_ID"].ToString();
				sMADE_BY = null;
				ATTRIBUTE = null;
				ATTRIBUTE2 = null;
				ATTRIBUTE3 = null;
				ATTRIBUTE4 = null;
				ATTRIBUTE5 = null;
				sCREATED_BY = null;
				sUPDATED_BY = null;
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DIV", (sDIV == "") ? null : sDIV);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (SOrg == "") ? null : SOrg);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_ID", (sSHEET_ID == "") ? null : sSHEET_ID);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_TYPE", sSHEET_TYPE);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "MADE_LOCATOR", (sMADE_LOCATOR == "") ? null : sMADE_LOCATOR);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DELIVERY_TO", (sDELIVERY_TO == "") ? null : sDELIVERY_TO);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ITEM_CODE", (sITEM_CODE == "") ? null : sITEM_CODE);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "WO_NAME", (sWO_NAME == "") ? null : sWO_NAME);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "TOTAL_QTY", iTOTAL_QTY);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SEQ", iSEQ);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "TO_WO_NAME", (sTO_WO_NAME == "") ? null : sTO_WO_NAME);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "TO_WO_QTY", iTO_WO_QTY);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "PRINT_YN", (sPRINT_YN == "") ? null : sPRINT_YN);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "STATUS", (sSTATUS == "") ? null : sSTATUS);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CLOSED_YN", (sCLOSED_YN == "") ? null : sCLOSED_YN);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "GROUP_TXN_ID", (sGROUP_TXN_ID == "") ? null : sGROUP_TXN_ID);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "MADE_BY", (sMADE_BY == "") ? null : sMADE_BY);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CREATED_BY", (sCREATED_BY == "") ? null : sCREATED_BY);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "UPDATED_BY", (sUPDATED_BY == "") ? null : sUPDATED_BY);
			}
			DataSet dsResult = bizServer.ExecBizRule("GMCS_SET_DELIVERY_SHEET", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				sReturnMsg = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_MSG"].ToString();
				sReturnCode = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_CODE"].ToString();
				if (sReturnCode.ToUpper() == "ERROR")
				{
					CommonBiz CallBiz = new CommonBiz();
					string sMsg = CallBiz.callGmcsSetError(sReturnMsg);
					MessageBox.Show(sMsg);
				}
			}
			if (sReturnCode == "OK")
			{
				bReturn = true;
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			bReturn = false;
		}
		return bReturn;
	}

	private bool GetDeleverySheet()
	{
		bool bReturn = false;
		string sReturnMsg = string.Empty;
		string sReturnCode = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (SOrg == "") ? null : SOrg);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_TYPE", "MCS_ONLINE");
			DataSet dsResult = bizServer.ExecBizRule("GMCS_GET_SHEET_ID", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				sReturnMsg = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_MSG"].ToString();
				sReturnCode = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_CODE"].ToString();
				if (sReturnCode.ToUpper() == "ERROR")
				{
					CommonBiz CallBiz = new CommonBiz();
					string sMsg = CallBiz.callGmcsSetError(sReturnMsg);
					MessageBox.Show(sMsg);
				}
			}
			if (sReturnCode == "OK")
			{
				bReturn = true;
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			bReturn = false;
		}
		return bReturn;
	}

	private string GetSheedID()
	{
		string sTxnID = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (SOrg == "") ? null : SOrg);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_TYPE", "MCS_ONLINE");
			DataSet dsResult = bizServer.ExecBizRule("GMCS_GET_SHEET_ID", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				sTxnID = dsResult.Tables["OUT_DATA"].Rows[0]["SHEET_ID"].ToString();
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			sTxnID = "";
		}
		return sTxnID;
	}

	private string GetLocator(string sPartNo)
	{
		string sLocator = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (SOrg == "") ? null : SOrg);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ITEM_CODE", sPartNo);
			DataSet dsResult = bizServer.ExecBizRule("GMCS_GET_DEPOTLOCATOR_BY_ITEM_CODE", ds, "IN_DATA", "OUT_DATA");
			using frmLocator_P frm = new frmLocator_P();
			frm.dtTemp = dsResult.Tables["OUT_DATA"];
			frm.sPartNo = sPartNo;
			if (frm.ShowDialog() == DialogResult.OK)
			{
				sLocator = frm.sLocator;
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			sLocator = "";
		}
		return sLocator;
	}

	private string GetTxnID()
	{
		string sTxnID = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			DataSet dsResult = bizServer.ExecBizRule("GMCS_GET_GROUP_TXN_ID", ds, null, "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				sTxnID = dsResult.Tables["OUT_DATA"].Rows[0]["GROUP_TXN_ID"].ToString();
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			sTxnID = "";
		}
		return sTxnID;
	}

	private void SyncData()
	{
		dtPrintSave.Clear();
		DataRow dr = dtPrintSave.NewRow();
		string sValue = string.Empty;
		string sCurrPartNo = string.Empty;
		string sBeforePartNo = string.Empty;
		string sLocator = string.Empty;
		int iGroupID = 1;
		int iTotalQty = 0;
		int iLeftQty = 0;
		int iQty = 0;
		int iWoQty = 0;
		int iGroupIDRowCount = 0;
		int iPackUnitqty = 0;
		int iQtyColIndex = fpPrintMain.ActiveSheet.GetColumnIndex("QTY");
		int iWoQtyColIndex = fpPrintMain.ActiveSheet.GetColumnIndex("WO_QTY");
		int iGroupIdSumQty = 0;
		for (int i = 0; fpPrintMain.ActiveSheet.Rows.Count > i; i++)
		{
			if (!(fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("CHK")) == "True"))
			{
				continue;
			}
			sCurrPartNo = fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("PART_NO"));
			if (string.IsNullOrEmpty(sBeforePartNo) || sCurrPartNo != sBeforePartNo)
			{
				sBeforePartNo = sCurrPartNo;
				sLocator = GetLocator(sCurrPartNo);
				if (string.IsNullOrEmpty(sLocator))
				{
					MessageBox.Show("Locator가 선택되지 않았습니다.");
					return;
				}
			}
			int.TryParse(fpPrintMain.ActiveSheet.GetValue(i, "PACK_UNIT_QTY").ToString(), out iPackUnitqty);
			int.TryParse(fpPrintMain.ActiveSheet.GetValue(i, "QTY").ToString(), out iQty);
			int.TryParse(fpPrintMain.ActiveSheet.GetValue(i, "WO_QTY").ToString(), out iWoQty);
			int icurrQty = 0;
			iGroupIDRowCount++;
			iGroupIdSumQty += iQty;
			if (iGroupIDRowCount > 5 || sCurrPartNo != sBeforePartNo || iGroupIdSumQty >= iPackUnitqty)
			{
				iGroupID++;
				iGroupIDRowCount = 0;
				sBeforePartNo = sCurrPartNo;
				iGroupIdSumQty = 0;
			}
			int iSumQty = 0;
			if (iPackUnitqty <= 0 || iQty <= 0)
			{
				continue;
			}
			for (int ipack = 0; iQty > ipack; ipack += iPackUnitqty)
			{
				iLeftQty = iQty - ipack;
				if (iLeftQty >= iPackUnitqty)
				{
					icurrQty = iPackUnitqty;
				}
				else if (iLeftQty < iPackUnitqty)
				{
					icurrQty = iLeftQty;
				}
				iSumQty += icurrQty;
				for (int j = 0; fpPrintMain.ActiveSheet.Columns.Count > j; j++)
				{
					try
					{
						sValue = fpPrintMain.ActiveSheet.GetValue(i, j).ToString();
						if (!string.IsNullOrEmpty(sValue))
						{
							if (j == iQtyColIndex)
							{
								dr[j] = icurrQty;
							}
							else if (j == iWoQtyColIndex)
							{
								dr[j] = iWoQty;
							}
							else
							{
								dr[j] = sValue;
							}
						}
					}
					catch (Exception)
					{
						sValue = string.Empty;
					}
				}
				dr["GROUP_ID"] = iGroupID;
				dr["LOCATOR"] = sLocator;
				dtPrintSave.Rows.Add(dr.ItemArray);
				if (iSumQty >= iPackUnitqty)
				{
					iGroupID++;
					iSumQty = 0;
				}
			}
		}
		fpPrintSave.Refresh();
	}

	private void procMakeSheetColumn()
	{
		try
		{
			MCS.Common.SheetView svPrintSearch = new MCS.Common.SheetView(fpPrintMain, "Search", OperationMode.Normal, bRowHeaderVisible: true);
			MCS.Common.SheetView svPrintSave = new MCS.Common.SheetView(fpPrintSave, "Save", OperationMode.Normal, bRowHeaderVisible: true);
			svPrintSearch.AddColumnCheckBox("", "CHK", 40, CellHorizontalAlignment.Center, bLocked: false, bVisible: true, "", "", "", bThreeState: false);
			svPrintSearch.AddColumnText("Part No", "PART_NO", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("Plan Date", "PLAN_DATE", 100, CellHorizontalAlignment.Center, bLocked: false, bVisible: true, 30);
			svPrintSearch.AddColumnText("Work Order", "WORK_ORDER", 120, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 50);
			svPrintSearch.AddColumnText("Model.Suffix", "MODEL_SUFFIX", 150, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 50);
			svPrintSearch.AddColumnText("WO_Qty", "WO_QTY", 100, CellHorizontalAlignment.Right, bLocked: true, bVisible: true, 30);
			svPrintSearch.AddColumnText("Daily Plan Qty", "DAILY_PLAN_QTY", 100, CellHorizontalAlignment.Right, bLocked: true, bVisible: true, 30);
			svPrintSearch.AddColumnText("Qty", "QTY", 80, CellHorizontalAlignment.Right, bLocked: false, bVisible: true, 120);
			svPrintSearch.AddColumnText("Description", "ITEM_DESC", 200, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 500);
			svPrintSearch.AddColumnText("Locator Group Code", "LOCATOR_GROUP", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 500);
			svPrintSearch.AddColumnText("Locator Group", "LOCATOR_GROUP_NAME", 180, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 500);
			svPrintSearch.AddColumnText("Locator", "LOCATOR", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 500);
			svPrintSearch.AddColumnText("", "PLAN_SEQ", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSearch.AddColumnText("", "PLAN_PROD_ST", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSearch.AddColumnText("", "PLAN_LINE_CODE", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSearch.AddColumnText("", "PLAN_SEQ_FROM", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSearch.AddColumnText("", "PLAN_SEQ_TO", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSearch.AddColumnText("", "MADE_BY", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSearch.AddColumnText("", "PACK_UNIT_QTY", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSearch.AddColumnText("", "SCHEDULE_GROUP_DESC", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnCheckBox("ALL", "CHK", 40, CellHorizontalAlignment.Center, bLocked: false, bVisible: false, "", "", "", bThreeState: false);
			svPrintSave.AddColumnText("Group_No", "GROUP_ID", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSave.AddColumnText("Part No", "PART_NO", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSave.AddColumnText("Plan Date", "PLAN_DATE", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 30);
			svPrintSave.AddColumnText("Work Order", "WORK_ORDER", 120, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 50);
			svPrintSave.AddColumnText("Model.Suffix", "MODEL_SUFFIX", 150, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 50);
			svPrintSave.AddColumnText("WO_Qty", "WO_QTY", 100, CellHorizontalAlignment.Right, bLocked: true, bVisible: true, 30);
			svPrintSave.AddColumnText("Daily Plan Qty", "DAILY_PLAN_QTY", 100, CellHorizontalAlignment.Right, bLocked: true, bVisible: true, 30);
			svPrintSave.AddColumnText("Qty", "QTY", 80, CellHorizontalAlignment.Right, bLocked: false, bVisible: true, 120);
			svPrintSave.AddColumnText("Description", "ITEM_DESC", 200, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 500);
			svPrintSave.AddColumnText("Locator Group Code", "LOCATOR_GROUP", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 500);
			svPrintSave.AddColumnText("Locator Group", "LOCATOR_GROUP_NAME", 180, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 500);
			svPrintSave.AddColumnText("Locator Group", "LOCATOR", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 500);
			svPrintSave.AddColumnText("", "SCHEDULE_GROUP_DESC", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnText("", "PLAN_SEQ", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnText("", "PLAN_PROD_ST", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnText("", "PLAN_LINE_CODE", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnText("", "PLAN_SEQ_FROM", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnText("", "PLAN_SEQ_TO", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnText("", "MADE_BY", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnText("", "SHEET_ID", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			svPrintSave.AddColumnText("", "GROUP_TXN_ID", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 100);
			dtPrintSave.Columns.Add("CHK");
			dtPrintSave.Columns.Add("PART_NO");
			dtPrintSave.Columns.Add("PLAN_DATE");
			dtPrintSave.Columns.Add("WORK_ORDER");
			dtPrintSave.Columns.Add("MODEL_SUFFIX");
			dtPrintSave.Columns.Add("WO_QTY");
			dtPrintSave.Columns.Add("DAILY_PLAN_QTY");
			dtPrintSave.Columns.Add("QTY");
			dtPrintSave.Columns.Add("ITEM_DESC");
			dtPrintSave.Columns.Add("LOCATOR_GROUP");
			dtPrintSave.Columns.Add("LOCATOR_GROUP_NAME");
			dtPrintSave.Columns.Add("LOCATOR");
			dtPrintSave.Columns.Add("PLAN_SEQ");
			dtPrintSave.Columns.Add("PLAN_PROD_ST");
			dtPrintSave.Columns.Add("PLAN_LINE_CODE");
			dtPrintSave.Columns.Add("PLAN_SEQ_FROM");
			dtPrintSave.Columns.Add("PLAN_SEQ_TO");
			dtPrintSave.Columns.Add("MADE_BY");
			dtPrintSave.Columns.Add("GROUP_ID");
			dtPrintSave.Columns.Add("SHEET_ID");
			dtPrintSave.Columns.Add("GROUP_TXN_ID");
			dtPrintSave.Columns.Add("TOTAL_QTY");
			dtPrintSave.Columns.Add("SCHEDULE_GROUP_DESC");
			svPrintSearch.RowHeader.Visible = true;
			svPrintSearch.Rows.Default.Height = 22f;
			svPrintSearch.ColumnHeader.Rows[0].Height = 25f;
			fpPrintMain.AutoSizeColumnWidth = false;
			fpPrintSave.AutoSizeColumnWidth = false;
			fpPrintSave.DataSource = dtPrintSave;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool GetDataFromfpPrintMain()
	{
		string sValue = string.Empty;
		bool bReturn = false;
		try
		{
			string[] sColnames = new string[1] { "GROUP_ID" };
			dtGroupBy = dtPrintSave.DefaultView.ToTable(distinct: true, sColnames);
			dtGroupBy.Columns.Add("TOTAL_QUANTITY");
			dtGroupBy.Columns.Add("MADE_BY");
			dtGroupBy.Columns.Add("CARRIER_ID");
			dtGroupBy.Columns.Add("TOTAL_SEQ_NO");
			dtGroupBy.Columns.Add("DESCRIPTION");
			dtGroupBy.Columns.Add("INSPECTION_FLAG");
			dtGroupBy.Columns.Add("PST");
			dtGroupBy.Columns.Add("PRINTED_DATE");
			dtGroupBy.Columns.Add("PART_NO");
			dtGroupBy.Columns.Add("QRCODE_VALUE");
			dtGroupBy.Columns.Add("SCHEDULE_GROUP_DESC");
			dtGroupBy.Columns.Add("PLAN_LINE_CODE");
			dtGroupBy.Columns.Add("LOCATOR_GROUP_NAME");
			dtGroupBy.Columns.Add("LOCATOR_GROUP");
			dtGroupBy.Columns.Add("WO_QUANTITY1");
			dtGroupBy.Columns.Add("PROD_SEQ_FROM1");
			dtGroupBy.Columns.Add("PROD_SEQ_TO1");
			dtGroupBy.Columns.Add("MODEL_SUFFIX1");
			dtGroupBy.Columns.Add("WORKER_ORDER1");
			dtGroupBy.Columns.Add("WO_QUANTITY2");
			dtGroupBy.Columns.Add("PROD_SEQ_FROM2");
			dtGroupBy.Columns.Add("PROD_SEQ_TO2");
			dtGroupBy.Columns.Add("MODEL_SUFFIX2");
			dtGroupBy.Columns.Add("WORKER_ORDER2");
			dtGroupBy.Columns.Add("WO_QUANTITY3");
			dtGroupBy.Columns.Add("PROD_SEQ_FROM3");
			dtGroupBy.Columns.Add("PROD_SEQ_TO3");
			dtGroupBy.Columns.Add("MODEL_SUFFIX3");
			dtGroupBy.Columns.Add("WORKER_ORDER3");
			dtGroupBy.Columns.Add("WO_QUANTITY4");
			dtGroupBy.Columns.Add("PROD_SEQ_FROM4");
			dtGroupBy.Columns.Add("PROD_SEQ_TO4");
			dtGroupBy.Columns.Add("MODEL_SUFFIX4");
			dtGroupBy.Columns.Add("WORKER_ORDER4");
			dtGroupBy.Columns.Add("WO_QUANTITY5");
			dtGroupBy.Columns.Add("PROD_SEQ_FROM5");
			dtGroupBy.Columns.Add("PROD_SEQ_TO5");
			dtGroupBy.Columns.Add("MODEL_SUFFIX5");
			dtGroupBy.Columns.Add("WORKER_ORDER5");
			for (int i = 0; dtGroupBy.Rows.Count > i; i++)
			{
				string sGROUP_ID = dtGroupBy.Rows[i]["GROUP_ID"].ToString();
				string sINSPECTION_FLAG = "";
				string sFilterExpression = $"GROUP_ID = '{sGROUP_ID}' ";
				int iTOTAL_QUANTITY = 0;
				string sTOTAL_SEQ_NO_FROM = string.Empty;
				string sTOTAL_SEQ_NO_TO = string.Empty;
				string sTOTAL_SEQ = string.Empty;
				DataRow[] Rows = dtPrintSave.Select(sFilterExpression);
				if (Rows.Length > 5)
				{
					MessageBox.Show("Error : WorkOrder Are More than 5.  ");
				}
				for (int j = 0; Rows.Length > j && 5 > j; j++)
				{
					string sSHEET_ID = Rows[j]["SHEET_ID"].ToString();
					string sDESCRIPTION = Rows[j]["ITEM_DESC"].ToString();
					string sPST = Rows[j]["PLAN_PROD_ST"].ToString();
					string sWORK_ORDER = Rows[j]["WORK_ORDER"].ToString();
					string sWO_QUANTITY = Rows[j]["TOTAL_QTY"].ToString();
					string sPROD_SEQ_FROM = Rows[j]["PLAN_SEQ_FROM"].ToString();
					string sPROD_SEQ_TO = Rows[j]["PLAN_SEQ_TO"].ToString();
					string sMODEL_SUFFIX = Rows[j]["MODEL_SUFFIX"].ToString();
					string sSCHEDULE_GROUP_DESC = Rows[j]["MADE_BY"].ToString();
					string sPLAN_LINE_CODE = Rows[j]["PLAN_LINE_CODE"].ToString();
					string sPART_NO = Rows[j]["PART_NO"].ToString();
					string sLOCATOR_GROUP_NAME = Rows[j]["LOCATOR_GROUP_NAME"].ToString();
					string sLOCATOR_GROUP = Rows[j]["LOCATOR_GROUP"].ToString();
					if (string.IsNullOrEmpty(sWO_QUANTITY))
					{
						sWO_QUANTITY = "0";
					}
					string WORKER_ORDER_Name = "WORKER_ORDER" + (j + 1);
					string WO_QUANTITY_Name = "WO_QUANTITY" + (j + 1);
					string PROD_SEQ_FROM_Name = "PROD_SEQ_FROM" + (j + 1);
					string PROD_SEQ_TO_Name = "PROD_SEQ_TO" + (j + 1);
					string MODEL_SUFFIX_Name = "MODEL_SUFFIX" + (j + 1);
					if (j == 0)
					{
						dtGroupBy.Rows[i]["PST"] = sPST;
						dtGroupBy.Rows[i]["INSPECTION_FLAG"] = sINSPECTION_FLAG;
						dtGroupBy.Rows[i]["DESCRIPTION"] = sDESCRIPTION;
						dtGroupBy.Rows[i]["MADE_BY"] = sSCHEDULE_GROUP_DESC;
						dtGroupBy.Rows[i]["PART_NO"] = sPART_NO;
						dtGroupBy.Rows[i]["LOCATOR_GROUP_NAME"] = sLOCATOR_GROUP_NAME;
						dtGroupBy.Rows[i]["LOCATOR_GROUP"] = sLOCATOR_GROUP;
						dtGroupBy.Rows[i]["PLAN_LINE_CODE"] = sPLAN_LINE_CODE;
						dtGroupBy.Rows[i]["CARRIER_ID"] = "";
						sTOTAL_SEQ_NO_FROM = sPROD_SEQ_FROM;
					}
					sTOTAL_SEQ_NO_TO = sPROD_SEQ_TO;
					sTOTAL_SEQ = sTOTAL_SEQ_NO_FROM + "  ~  " + sTOTAL_SEQ_NO_TO;
					iTOTAL_QUANTITY += int.Parse(sWO_QUANTITY);
					dtGroupBy.Rows[i]["PRINTED_DATE"] = DateTime.Now.ToString("yyyy.MM.dd");
					dtGroupBy.Rows[i]["QRCODE_VALUE"] = sSHEET_ID;
					dtGroupBy.Rows[i]["TOTAL_QUANTITY"] = iTOTAL_QUANTITY.ToString();
					dtGroupBy.Rows[i]["TOTAL_SEQ_NO"] = sTOTAL_SEQ;
					dtGroupBy.Rows[i][WO_QUANTITY_Name] = sWO_QUANTITY;
					dtGroupBy.Rows[i][PROD_SEQ_FROM_Name] = sPROD_SEQ_FROM;
					dtGroupBy.Rows[i][PROD_SEQ_TO_Name] = sPROD_SEQ_TO;
					dtGroupBy.Rows[i][MODEL_SUFFIX_Name] = sMODEL_SUFFIX;
					dtGroupBy.Rows[i][WORKER_ORDER_Name] = sWORK_ORDER;
				}
			}
			if (dtGroupBy.Rows.Count > 0)
			{
				return true;
			}
			return false;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private void picLocatorGroup_Click(object sender, EventArgs e)
	{
		using frmLocatorGroupTree_P frm = new frmLocatorGroupTree_P();
		if (frm.ShowDialog() != DialogResult.OK)
		{
			return;
		}
		sLocatorGroupCode = frm.sLocatorGroupCode;
		sLocatorGroupName = frm.sLocatorGroupName;
		txtLocatorGroup.Text = sLocatorGroupName;
		SetcmbLocator("INIT");
		cmbLocatorGroup.Text = "";
		cmbLocatorGroup.ClearSelection();
		if (string.IsNullOrEmpty(sLocatorGroupName))
		{
			return;
		}
		string[] Indatas = null;
		Indatas = sLocatorGroupName.Split(',');
		string[] array = Indatas;
		foreach (string item in array)
		{
			for (int i = 0; cmbLocatorGroup.Items.Count > i; i++)
			{
				if (item == cmbLocatorGroup.CheckBoxItems[i].Text.ToString())
				{
					cmbLocatorGroup.CheckBoxItems[i].Checked = true;
				}
			}
		}
		SetcmbLocator("INIT");
	}

	private void SetComboInit()
	{
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LANGID", CultureInfo.CurrentUICulture.Name);
			DataSet dsResult = bizServer.ExecBizRule("GMCS_GET_ORG_CBO", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				cboOrg.SetItemList(dsResult.Tables["OUT_DATA"], "COMBOBOX_ID", "COMBOBOX_NAME", AllFlag: false);
			}
			ProcPartType();
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void SetSpreadInit()
	{
		try
		{
			dtPrintMain.Clear();
			fpPrintMain.Refresh();
			dtPrintSave.Clear();
			fpPrintSave.Refresh();
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void ProcPartType()
	{
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "ORG_ID", (SOrg == "") ? null : SOrg);
			DataSet dsResult = bizServer.ExecBizRule("SEL_CM_PART_TYPE", ds, "RQSTDT", "RSLTDT");
			if (dsResult.Tables["RSLTDT"].Rows.Count > 0)
			{
				cboPartType.SetItemList(dsResult.Tables["RSLTDT"], "ITEM_GROUP_CODE", "ITEM_GROUP_CODE_NAME", AllFlag: true);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void SetcmbLocator(string pFilter)
	{
		try
		{
			DataSet ds = new DataSet();
			DataTable dtTemp = new DataTable();
			BizService bizServer = new BizService();
			string sLocator = string.Empty;
			string sLocatorName = string.Empty;
			string sLocatorGroupCodeTemp = string.Empty;
			if (cmbLocatorGroup.Tag != null)
			{
				sLocatorGroupCodeTemp = cmbLocatorGroup.Tag.ToString().Replace(",", "|");
			}
			DataRow[] Rows;
			if (pFilter == "INIT")
			{
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", null);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LANGID", CultureInfo.CurrentUICulture.Name);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LOCATOR_GROUP", (sLocatorGroupCodeTemp == "") ? null : sLocatorGroupCodeTemp);
				dsLocator = bizServer.ExecBizRule("GMCS_GET_LOCATOR_INFO", ds, "IN_DATA", "OUT_DATA");
				Rows = dsLocator.Tables["OUT_DATA"].Select();
			}
			else
			{
				string sFilterExpression = string.Format("LOCATOR_NAME LIKE  '{0}'", "%" + pFilter + "%");
				Rows = dsLocator.Tables["OUT_DATA"].Select(sFilterExpression);
			}
			DataTable DT = new DataTable("CHCEKED_BOX_MULTI_DATASET");
			DT.Columns.AddRange(new DataColumn[3]
			{
				new DataColumn("Id", typeof(int)),
				new DataColumn("SomePropertyOrColumnName", typeof(string)),
				new DataColumn("Description", typeof(string))
			});
			int iSeq = 0;
			if (Rows.Length != 0)
			{
				for (int i = 0; Rows.Length > i; i++)
				{
					iSeq = i;
					sLocator = Rows[i]["LOCATOR"].ToString();
					sLocatorName = Rows[i]["LOCATOR_NAME"].ToString();
					DT.Rows.Add(iSeq + 1, sLocatorName, sLocator);
				}
			}
			cmbLocator.DataSource = new ListSelectionWrapper<DataRow>(DT.Rows, "SomePropertyOrColumnName");
			cmbLocator.DisplayMemberSingleItem = "Name";
			cmbLocator.DisplayMember = "NameConcatenated";
			cmbLocator.ValueMember = "Selected";
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void SetcmbLocatorGroup(string pFilter)
	{
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			string sComboboxID = string.Empty;
			string sComboboxName = string.Empty;
			string sLocatorGroupCodeTemp = string.Empty;
			if (cmbLocatorGroup.Tag != null)
			{
				sLocatorGroupCodeTemp = cmbLocatorGroup.Tag.ToString().Replace(",", "|");
			}
			DataRow[] Rows;
			if (pFilter == "INIT")
			{
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "COMBOBOX_TYPE", "CODE");
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CODE_TYPE", "LOCATOR_GROUP_3LVL");
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LANGID", CultureInfo.CurrentUICulture.Name);
				dsLocatorGroup = bizServer.ExecBizRule("MCS_GET_COMBO_BOX_LIST", ds, "IN_DATA", "OUT_DATA");
				Rows = dsLocatorGroup.Tables["OUT_DATA"].Select();
			}
			else
			{
				string sFilterExpression = string.Format("COMBOBOX_NAME LIKE  '{0}'", "%" + pFilter + "%");
				Rows = dsLocatorGroup.Tables["OUT_DATA"].Select(sFilterExpression);
			}
			DataTable DT = new DataTable("CHCEKED_BOX_MULTI_DATASET");
			DT.Columns.AddRange(new DataColumn[3]
			{
				new DataColumn("Id", typeof(int)),
				new DataColumn("SomePropertyOrColumnName", typeof(string)),
				new DataColumn("Description", typeof(string))
			});
			int iSeq = 0;
			if (Rows.Length != 0)
			{
				for (int i = 0; Rows.Length > i; i++)
				{
					iSeq = i;
					sComboboxID = Rows[i]["COMBOBOX_ID"].ToString();
					sComboboxName = Rows[i]["COMBOBOX_NAME"].ToString();
					DT.Rows.Add(iSeq + 1, sComboboxName, sComboboxID);
				}
			}
			cmbLocatorGroup.DataSource = new ListSelectionWrapper<DataRow>(DT.Rows, "SomePropertyOrColumnName");
			cmbLocatorGroup.DisplayMemberSingleItem = "Name";
			cmbLocatorGroup.DisplayMember = "NameConcatenated";
			cmbLocatorGroup.ValueMember = "Selected";
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void btn_New_Test_Click(object sender, EventArgs e)
	{
		fpPrintSave.ActiveSheet.Rows.Clear();
		fpPrintSave.ActiveSheet.Rows.Add(0, 20);
	}

	private void btn_Save_Test_Click(object sender, EventArgs e)
	{
		if (fpPrintSave.ActiveSheet.Rows.Count == 0)
		{
			return;
		}
		string sCurrGroupID = string.Empty;
		string sBeforeGroupID = string.Empty;
		string sGetTxnID = string.Empty;
		string sGetSheedID = string.Empty;
		int iSheetSumQty = 0;
		for (int i = 0; dtPrintSave.Rows.Count > i; i++)
		{
			sCurrGroupID = dtPrintSave.Rows[i]["GROUP_ID"].ToString();
			if (string.IsNullOrEmpty(sBeforeGroupID))
			{
				sBeforeGroupID = sCurrGroupID;
				sGetSheedID = GetSheedID();
				iSheetSumQty = GetSheetSumlQty(sCurrGroupID);
			}
			if (sCurrGroupID != sBeforeGroupID)
			{
				sBeforeGroupID = sCurrGroupID;
				sGetSheedID = GetSheedID();
				iSheetSumQty = GetSheetSumlQty(sCurrGroupID);
			}
			dtPrintSave.Rows[i]["GROUP_TXN_ID"] = sGetTxnID;
			dtPrintSave.Rows[i]["SHEET_ID"] = sGetSheedID;
			dtPrintSave.Rows[i]["TOTAL_QTY"] = iSheetSumQty;
		}
	}

	private void button1_Click(object sender, EventArgs e)
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmOnLinePrintNew));
		PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
		PresentationControls.CheckBoxProperties checkBoxProperties2 = new PresentationControls.CheckBoxProperties();
		FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer3 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer4 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer5 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer6 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		this.tmDemo = new System.Windows.Forms.Timer(this.components);
		this.dgvBuffer = new System.Windows.Forms.DataGridView();
		this.tmRefresh = new System.Windows.Forms.Timer(this.components);
		this.searchPanel1 = new MCS.Common.SearchPanel();
		this.splitContainerMain = new System.Windows.Forms.SplitContainer();
		this.panelOnly1 = new MCS.Common.PanelOnly();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbLocator = new PresentationControls.CheckBoxComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cboPartType = new MCS.Common.ComboBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btn_search = new System.Windows.Forms.Button();
		this.cmbLocatorGroup = new PresentationControls.CheckBoxComboBox();
		this.picLocatorGroup = new System.Windows.Forms.PictureBox();
		this.label4 = new System.Windows.Forms.Label();
		this.cboOrg = new MCS.Common.ComboBox();
		this.dtpToDate = new System.Windows.Forms.DateTimePicker();
		this.txtWorkOrder = new System.Windows.Forms.TextBox();
		this.txtLocatorGroup = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtPartNo = new System.Windows.Forms.TextBox();
		this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.backPanel3 = new MCS.Common.BackPanel();
		this.fpPrintMain = new MCS.Common.FpSpread();
		this.splitContainer2 = new System.Windows.Forms.SplitContainer();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btn_preview = new System.Windows.Forms.Button();
		this.btn_deleteRow = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.rdoWorkOrder = new System.Windows.Forms.RadioButton();
		this.rdoCarrierQty = new System.Windows.Forms.RadioButton();
		this.userBox1 = new MCS.Common.Controls.UserBox();
		this.btn_Down = new System.Windows.Forms.Button();
		this.backPanel4 = new MCS.Common.BackPanel();
		this.fpPrintSave = new MCS.Common.FpSpread();
		this.backPanel2 = new MCS.Common.BackPanel();
		this.fpProdResult = new MCS.Common.FpSpread();
		this.backPanel1 = new MCS.Common.BackPanel();
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).BeginInit();
		this.splitContainerMain.Panel1.SuspendLayout();
		this.splitContainerMain.Panel2.SuspendLayout();
		this.splitContainerMain.SuspendLayout();
		this.panelOnly1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picLocatorGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		this.backPanel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpPrintMain).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).BeginInit();
		this.splitContainer2.Panel1.SuspendLayout();
		this.splitContainer2.Panel2.SuspendLayout();
		this.splitContainer2.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.backPanel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpPrintSave).BeginInit();
		this.backPanel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpProdResult).BeginInit();
		base.SuspendLayout();
		this.dgvBuffer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvBuffer.Location = new System.Drawing.Point(848, 12);
		this.dgvBuffer.Name = "dgvBuffer";
		this.dgvBuffer.RowTemplate.Height = 23;
		this.dgvBuffer.Size = new System.Drawing.Size(121, 45);
		this.dgvBuffer.TabIndex = 53;
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
		this.searchPanel1.Size = new System.Drawing.Size(1000, 109);
		this.searchPanel1.TabIndex = 0;
		this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
		this.splitContainerMain.Name = "splitContainerMain";
		this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainerMain.Panel1.Controls.Add(this.panelOnly1);
		this.splitContainerMain.Panel2.Controls.Add(this.splitContainer1);
		this.splitContainerMain.Size = new System.Drawing.Size(1251, 761);
		this.splitContainerMain.SplitterDistance = 117;
		this.splitContainerMain.SplitterWidth = 1;
		this.splitContainerMain.TabIndex = 66;
		this.panelOnly1.BackgroundImage = (System.Drawing.Image)resources.GetObject("panelOnly1.BackgroundImage");
		this.panelOnly1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.panelOnly1.Controls.Add(this.label9);
		this.panelOnly1.Controls.Add(this.cmbLocator);
		this.panelOnly1.Controls.Add(this.label3);
		this.panelOnly1.Controls.Add(this.label6);
		this.panelOnly1.Controls.Add(this.cboPartType);
		this.panelOnly1.Controls.Add(this.panel1);
		this.panelOnly1.Controls.Add(this.cboOrg);
		this.panelOnly1.Controls.Add(this.dtpToDate);
		this.panelOnly1.Controls.Add(this.txtWorkOrder);
		this.panelOnly1.Controls.Add(this.txtLocatorGroup);
		this.panelOnly1.Controls.Add(this.label2);
		this.panelOnly1.Controls.Add(this.label5);
		this.panelOnly1.Controls.Add(this.label11);
		this.panelOnly1.Controls.Add(this.txtPartNo);
		this.panelOnly1.Controls.Add(this.dtpFromDate);
		this.panelOnly1.Controls.Add(this.label10);
		this.panelOnly1.Controls.Add(this.label7);
		this.panelOnly1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelOnly1.Location = new System.Drawing.Point(0, 0);
		this.panelOnly1.Name = "panelOnly1";
		this.panelOnly1.Padding = new System.Windows.Forms.Padding(9);
		this.panelOnly1.Size = new System.Drawing.Size(1251, 117);
		this.panelOnly1.TabIndex = 82;
		this.label9.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label9.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(6, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(270, 38);
		this.label9.TabIndex = 106;
		this.label9.Text = "Online New Sheet";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbLocator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbLocator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		checkBoxProperties1.FlatAppearanceBorderSize = 3;
		checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.cmbLocator.CheckBoxProperties = checkBoxProperties1;
		this.cmbLocator.DisplayMemberSingleItem = "";
		this.cmbLocator.FormattingEnabled = true;
		this.cmbLocator.Location = new System.Drawing.Point(1008, 83);
		this.cmbLocator.MaxDropDownItems = 20;
		this.cmbLocator.Name = "cmbLocator";
		this.cmbLocator.Size = new System.Drawing.Size(28, 23);
		this.cmbLocator.TabIndex = 102;
		this.cmbLocator.Visible = false;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Transparent;
		this.label3.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(128, 51);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(31, 16);
		this.label3.TabIndex = 92;
		this.label3.Text = "Org";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Transparent;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Maroon;
		this.label6.Location = new System.Drawing.Point(971, 86);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(48, 15);
		this.label6.TabIndex = 96;
		this.label6.Text = "Locator";
		this.label6.Visible = false;
		this.cboPartType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboPartType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboPartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboPartType.Font = new System.Drawing.Font("Arial", 10f);
		this.cboPartType.FormattingEnabled = true;
		this.cboPartType.Location = new System.Drawing.Point(853, 49);
		this.cboPartType.Name = "cboPartType";
		this.cboPartType.Size = new System.Drawing.Size(183, 24);
		this.cboPartType.TabIndex = 4;
		this.panel1.BackColor = System.Drawing.Color.Transparent;
		this.panel1.Controls.Add(this.btn_search);
		this.panel1.Controls.Add(this.cmbLocatorGroup);
		this.panel1.Controls.Add(this.picLocatorGroup);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel1.Location = new System.Drawing.Point(1094, 9);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(148, 99);
		this.panel1.TabIndex = 78;
		this.btn_search.BackColor = System.Drawing.Color.Transparent;
		this.btn_search.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_search.BackgroundImage");
		this.btn_search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_search.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_search.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_search.ForeColor = System.Drawing.Color.Black;
		this.btn_search.Image = (System.Drawing.Image)resources.GetObject("btn_search.Image");
		this.btn_search.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_search.Location = new System.Drawing.Point(25, 0);
		this.btn_search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_search.Name = "btn_search";
		this.btn_search.Size = new System.Drawing.Size(100, 30);
		this.btn_search.TabIndex = 7;
		this.btn_search.Text = "   Search";
		this.btn_search.UseVisualStyleBackColor = false;
		this.btn_search.Click += new System.EventHandler(btn_Search_Click);
		this.cmbLocatorGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbLocatorGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		checkBoxProperties2.AutoEllipsis = true;
		checkBoxProperties2.AutoSize = true;
		checkBoxProperties2.FlatAppearanceBorderSize = 5;
		checkBoxProperties2.FlatAppearanceMouseOverBackColor = System.Drawing.Color.FromArgb(255, 224, 192);
		checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.cmbLocatorGroup.CheckBoxProperties = checkBoxProperties2;
		this.cmbLocatorGroup.DisplayMemberSingleItem = "";
		this.cmbLocatorGroup.FormattingEnabled = true;
		this.cmbLocatorGroup.Location = new System.Drawing.Point(-108, 73);
		this.cmbLocatorGroup.MaxDropDownItems = 20;
		this.cmbLocatorGroup.Name = "cmbLocatorGroup";
		this.cmbLocatorGroup.Size = new System.Drawing.Size(40, 23);
		this.cmbLocatorGroup.TabIndex = 105;
		this.cmbLocatorGroup.Visible = false;
		this.cmbLocatorGroup.CheckBoxCheckedChanged += new System.EventHandler(cmbLocatorGroup_CheckBoxCheckedChanged);
		this.picLocatorGroup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.picLocatorGroup.Image = (System.Drawing.Image)resources.GetObject("picLocatorGroup.Image");
		this.picLocatorGroup.Location = new System.Drawing.Point(-58, 73);
		this.picLocatorGroup.Margin = new System.Windows.Forms.Padding(0);
		this.picLocatorGroup.Name = "picLocatorGroup";
		this.picLocatorGroup.Size = new System.Drawing.Size(29, 24);
		this.picLocatorGroup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.picLocatorGroup.TabIndex = 98;
		this.picLocatorGroup.TabStop = false;
		this.picLocatorGroup.Visible = false;
		this.picLocatorGroup.Click += new System.EventHandler(picLocatorGroup_Click);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Transparent;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Maroon;
		this.label4.Location = new System.Drawing.Point(-218, 76);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(85, 15);
		this.label4.TabIndex = 94;
		this.label4.Text = "Locator Group";
		this.label4.Visible = false;
		this.cboOrg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboOrg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboOrg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboOrg.Font = new System.Drawing.Font("Arial", 10f);
		this.cboOrg.FormattingEnabled = true;
		this.cboOrg.Location = new System.Drawing.Point(168, 49);
		this.cboOrg.Name = "cboOrg";
		this.cboOrg.Size = new System.Drawing.Size(135, 24);
		this.cboOrg.TabIndex = 1;
		this.cboOrg.SelectedIndexChanged += new System.EventHandler(cboOrg_SelectedIndexChanged);
		this.dtpToDate.CustomFormat = "";
		this.dtpToDate.Font = new System.Drawing.Font("Arial", 10f);
		this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpToDate.Location = new System.Drawing.Point(581, 49);
		this.dtpToDate.Name = "dtpToDate";
		this.dtpToDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.dtpToDate.Size = new System.Drawing.Size(102, 23);
		this.dtpToDate.TabIndex = 3;
		this.txtWorkOrder.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtWorkOrder.Font = new System.Drawing.Font("Arial", 10f);
		this.txtWorkOrder.Location = new System.Drawing.Point(168, 78);
		this.txtWorkOrder.Name = "txtWorkOrder";
		this.txtWorkOrder.Size = new System.Drawing.Size(135, 23);
		this.txtWorkOrder.TabIndex = 5;
		this.txtLocatorGroup.Location = new System.Drawing.Point(1035, 84);
		this.txtLocatorGroup.Name = "txtLocatorGroup";
		this.txtLocatorGroup.ReadOnly = true;
		this.txtLocatorGroup.Size = new System.Drawing.Size(47, 21);
		this.txtLocatorGroup.TabIndex = 97;
		this.txtLocatorGroup.Visible = false;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Transparent;
		this.label2.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(78, 81);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 16);
		this.label2.TabIndex = 91;
		this.label2.Text = "Work Order";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Transparent;
		this.label5.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(780, 51);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(68, 16);
		this.label5.TabIndex = 93;
		this.label5.Text = "Part Type";
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.Transparent;
		this.label11.Font = new System.Drawing.Font("Arial", 10f);
		this.label11.ForeColor = System.Drawing.Color.Black;
		this.label11.Location = new System.Drawing.Point(555, 50);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(16, 16);
		this.label11.TabIndex = 112;
		this.label11.Text = "~";
		this.txtPartNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtPartNo.Font = new System.Drawing.Font("Arial", 10f);
		this.txtPartNo.Location = new System.Drawing.Point(445, 78);
		this.txtPartNo.Name = "txtPartNo";
		this.txtPartNo.Size = new System.Drawing.Size(135, 23);
		this.txtPartNo.TabIndex = 6;
		this.dtpFromDate.CustomFormat = "";
		this.dtpFromDate.Font = new System.Drawing.Font("Arial", 10f);
		this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.dtpFromDate.Location = new System.Drawing.Point(446, 49);
		this.dtpFromDate.Name = "dtpFromDate";
		this.dtpFromDate.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.dtpFromDate.Size = new System.Drawing.Size(102, 23);
		this.dtpFromDate.TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Transparent;
		this.label10.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Black;
		this.label10.Location = new System.Drawing.Point(371, 51);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(71, 16);
		this.label10.TabIndex = 111;
		this.label10.Text = "Prod Date";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Transparent;
		this.label7.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(387, 81);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(55, 16);
		this.label7.TabIndex = 95;
		this.label7.Text = "Part No";
		this.splitContainer1.BackColor = System.Drawing.Color.Transparent;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
		this.splitContainer1.Panel1.Controls.Add(this.backPanel3);
		this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
		this.splitContainer1.Size = new System.Drawing.Size(1251, 643);
		this.splitContainer1.SplitterDistance = 303;
		this.splitContainer1.SplitterWidth = 1;
		this.splitContainer1.TabIndex = 73;
		this.backPanel3.BackColor = System.Drawing.Color.FromArgb(215, 214, 216);
		this.backPanel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.backPanel3.Controls.Add(this.fpPrintMain);
		this.backPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.backPanel3.Location = new System.Drawing.Point(0, 0);
		this.backPanel3.Name = "backPanel3";
		this.backPanel3.Padding = new System.Windows.Forms.Padding(0);
		this.backPanel3.Size = new System.Drawing.Size(1251, 303);
		this.backPanel3.TabIndex = 114;
		this.backPanel3.Title = "Production Plan";
		this.fpPrintMain.AccessibleDescription = "";
		this.fpPrintMain.AutoSizeColumnWidth = true;
		this.fpPrintMain.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpPrintMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpPrintMain.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.fpPrintMain.EnableSort = false;
		this.fpPrintMain.FocusRenderer = defaultFocusIndicatorRenderer1;
		this.fpPrintMain.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpPrintMain.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPrintMain.HorizontalScrollBar.Name = "";
		this.fpPrintMain.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
		this.fpPrintMain.HorizontalScrollBar.TabIndex = 0;
		this.fpPrintMain.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPrintMain.Location = new System.Drawing.Point(7, 29);
		this.fpPrintMain.Name = "fpPrintMain";
		this.fpPrintMain.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintMain.Size = new System.Drawing.Size(1233, 263);
		this.fpPrintMain.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpPrintMain.TabIndex = 71;
		this.fpPrintMain.TextTipDelay = 1000;
		this.fpPrintMain.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpPrintMain.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPrintMain.VerticalScrollBar.Name = "";
		this.fpPrintMain.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
		this.fpPrintMain.VerticalScrollBar.TabIndex = 0;
		this.fpPrintMain.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPrintMain.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.fpPrintMain.ButtonClicked += new FarPoint.Win.Spread.EditorNotifyEventHandler(fpPrintMain_ButtonClicked);
		this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer2.Location = new System.Drawing.Point(0, 0);
		this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
		this.splitContainer2.Name = "splitContainer2";
		this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer2.Panel1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.splitContainer2.Panel1.Controls.Add(this.panel2);
		this.splitContainer2.Panel1.Controls.Add(this.panel3);
		this.splitContainer2.Panel1MinSize = 27;
		this.splitContainer2.Panel2.BackColor = System.Drawing.SystemColors.Control;
		this.splitContainer2.Panel2.Controls.Add(this.backPanel4);
		this.splitContainer2.Panel2MinSize = 27;
		this.splitContainer2.Size = new System.Drawing.Size(1251, 339);
		this.splitContainer2.SplitterDistance = 31;
		this.splitContainer2.SplitterWidth = 1;
		this.splitContainer2.TabIndex = 76;
		this.panel2.BackColor = System.Drawing.Color.Transparent;
		this.panel2.Controls.Add(this.btn_preview);
		this.panel2.Controls.Add(this.btn_deleteRow);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel2.Font = new System.Drawing.Font("Arial", 9f);
		this.panel2.Location = new System.Drawing.Point(1022, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(229, 31);
		this.panel2.TabIndex = 107;
		this.btn_preview.BackgroundImage = MCS.PrintBoard.Properties.Resources.sbtnbg;
		this.btn_preview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_preview.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_preview.Font = new System.Drawing.Font("Arial", 9f);
		this.btn_preview.Image = (System.Drawing.Image)resources.GetObject("btn_preview.Image");
		this.btn_preview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_preview.Location = new System.Drawing.Point(115, 1);
		this.btn_preview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_preview.Name = "btn_preview";
		this.btn_preview.Size = new System.Drawing.Size(100, 30);
		this.btn_preview.TabIndex = 76;
		this.btn_preview.Text = "      Save && Print";
		this.btn_preview.UseVisualStyleBackColor = true;
		this.btn_preview.Click += new System.EventHandler(btn_Print_Click);
		this.btn_deleteRow.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_deleteRow.BackgroundImage");
		this.btn_deleteRow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_deleteRow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_deleteRow.Font = new System.Drawing.Font("Arial", 9f);
		this.btn_deleteRow.Image = (System.Drawing.Image)resources.GetObject("btn_deleteRow.Image");
		this.btn_deleteRow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_deleteRow.Location = new System.Drawing.Point(15, 1);
		this.btn_deleteRow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_deleteRow.Name = "btn_deleteRow";
		this.btn_deleteRow.Size = new System.Drawing.Size(100, 30);
		this.btn_deleteRow.TabIndex = 112;
		this.btn_deleteRow.Text = "  Delete";
		this.btn_deleteRow.UseVisualStyleBackColor = true;
		this.btn_deleteRow.Visible = false;
		this.btn_deleteRow.Click += new System.EventHandler(btn_deleteRow_Click);
		this.panel3.BackColor = System.Drawing.Color.Transparent;
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.rdoWorkOrder);
		this.panel3.Controls.Add(this.rdoCarrierQty);
		this.panel3.Controls.Add(this.userBox1);
		this.panel3.Controls.Add(this.btn_Down);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Font = new System.Drawing.Font("Arial", 9f);
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(657, 31);
		this.panel3.TabIndex = 112;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(17, 10);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(80, 15);
		this.label1.TabIndex = 116;
		this.label1.Text = "Loading Type";
		this.label1.Visible = false;
		this.rdoWorkOrder.AutoSize = true;
		this.rdoWorkOrder.Location = new System.Drawing.Point(219, 8);
		this.rdoWorkOrder.Name = "rdoWorkOrder";
		this.rdoWorkOrder.Size = new System.Drawing.Size(87, 19);
		this.rdoWorkOrder.TabIndex = 115;
		this.rdoWorkOrder.Text = "Work Order";
		this.rdoWorkOrder.UseVisualStyleBackColor = true;
		this.rdoWorkOrder.Visible = false;
		this.rdoCarrierQty.AutoSize = true;
		this.rdoCarrierQty.Checked = true;
		this.rdoCarrierQty.Location = new System.Drawing.Point(120, 8);
		this.rdoCarrierQty.Name = "rdoCarrierQty";
		this.rdoCarrierQty.Size = new System.Drawing.Size(83, 19);
		this.rdoCarrierQty.TabIndex = 114;
		this.rdoCarrierQty.TabStop = true;
		this.rdoCarrierQty.Text = "Carrier Qty";
		this.rdoCarrierQty.UseVisualStyleBackColor = true;
		this.rdoCarrierQty.Visible = false;
		this.userBox1.BackColor = System.Drawing.Color.Transparent;
		this.userBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.userBox1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.userBox1.Location = new System.Drawing.Point(3, 3);
		this.userBox1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
		this.userBox1.Name = "userBox1";
		this.userBox1.Size = new System.Drawing.Size(341, 28);
		this.userBox1.TabIndex = 113;
		this.userBox1.Visible = false;
		this.btn_Down.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Down.BackgroundImage");
		this.btn_Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Down.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Down.Font = new System.Drawing.Font("Arial", 9f);
		this.btn_Down.Image = (System.Drawing.Image)resources.GetObject("btn_Down.Image");
		this.btn_Down.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Down.Location = new System.Drawing.Point(557, 0);
		this.btn_Down.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Down.Name = "btn_Down";
		this.btn_Down.Size = new System.Drawing.Size(100, 30);
		this.btn_Down.TabIndex = 109;
		this.btn_Down.Text = "        Down";
		this.btn_Down.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Down.UseVisualStyleBackColor = true;
		this.btn_Down.Click += new System.EventHandler(btn_Confirm);
		this.backPanel4.BackColor = System.Drawing.Color.FromArgb(215, 214, 216);
		this.backPanel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
		this.backPanel4.Controls.Add(this.fpPrintSave);
		this.backPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.backPanel4.Location = new System.Drawing.Point(0, 0);
		this.backPanel4.Name = "backPanel4";
		this.backPanel4.Padding = new System.Windows.Forms.Padding(0);
		this.backPanel4.Size = new System.Drawing.Size(1251, 307);
		this.backPanel4.TabIndex = 76;
		this.backPanel4.Title = "New Sheet";
		this.fpPrintSave.AccessibleDescription = "";
		this.fpPrintSave.AutoSizeColumnWidth = true;
		this.fpPrintSave.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpPrintSave.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpPrintSave.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintSave.Dock = System.Windows.Forms.DockStyle.Fill;
		this.fpPrintSave.EnableSort = false;
		this.fpPrintSave.FocusRenderer = defaultFocusIndicatorRenderer1;
		this.fpPrintSave.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpPrintSave.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPrintSave.HorizontalScrollBar.Name = "";
		this.fpPrintSave.HorizontalScrollBar.Renderer = defaultScrollBarRenderer3;
		this.fpPrintSave.HorizontalScrollBar.TabIndex = 0;
		this.fpPrintSave.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPrintSave.Location = new System.Drawing.Point(7, 29);
		this.fpPrintSave.Name = "fpPrintSave";
		this.fpPrintSave.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintSave.Size = new System.Drawing.Size(1233, 267);
		this.fpPrintSave.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpPrintSave.TabIndex = 73;
		this.fpPrintSave.TextTipDelay = 1000;
		this.fpPrintSave.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpPrintSave.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPrintSave.VerticalScrollBar.Name = "";
		this.fpPrintSave.VerticalScrollBar.Renderer = defaultScrollBarRenderer4;
		this.fpPrintSave.VerticalScrollBar.TabIndex = 0;
		this.fpPrintSave.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPrintSave.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.backPanel2.BackColor = System.Drawing.Color.FromArgb(215, 214, 216);
		this.backPanel2.Controls.Add(this.fpProdResult);
		this.backPanel2.Location = new System.Drawing.Point(10, 63);
		this.backPanel2.Name = "backPanel2";
		this.backPanel2.Padding = new System.Windows.Forms.Padding(0);
		this.backPanel2.Size = new System.Drawing.Size(650, 285);
		this.backPanel2.TabIndex = 69;
		this.backPanel2.Title = "생산실적";
		this.fpProdResult.AccessibleDescription = "";
		this.fpProdResult.AutoSizeColumnWidth = true;
		this.fpProdResult.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpProdResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpProdResult.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpProdResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.fpProdResult.EnableSort = false;
		this.fpProdResult.FocusRenderer = defaultFocusIndicatorRenderer1;
		this.fpProdResult.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpProdResult.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpProdResult.HorizontalScrollBar.Name = "";
		this.fpProdResult.HorizontalScrollBar.Renderer = defaultScrollBarRenderer5;
		this.fpProdResult.HorizontalScrollBar.TabIndex = 0;
		this.fpProdResult.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpProdResult.Location = new System.Drawing.Point(7, 29);
		this.fpProdResult.Name = "fpProdResult";
		this.fpProdResult.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpProdResult.Size = new System.Drawing.Size(636, 249);
		this.fpProdResult.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpProdResult.TabIndex = 66;
		this.fpProdResult.TextTipDelay = 1000;
		this.fpProdResult.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpProdResult.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpProdResult.VerticalScrollBar.Name = "";
		this.fpProdResult.VerticalScrollBar.Renderer = defaultScrollBarRenderer6;
		this.fpProdResult.VerticalScrollBar.TabIndex = 0;
		this.fpProdResult.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpProdResult.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.backPanel1.BackColor = System.Drawing.Color.FromArgb(215, 214, 216);
		this.backPanel1.Location = new System.Drawing.Point(661, 63);
		this.backPanel1.Name = "backPanel1";
		this.backPanel1.Padding = new System.Windows.Forms.Padding(0);
		this.backPanel1.Size = new System.Drawing.Size(647, 285);
		this.backPanel1.TabIndex = 68;
		this.backPanel1.Title = "생산계획";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(1251, 761);
		base.Controls.Add(this.splitContainerMain);
		this.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOnLinePrintNew";
		this.Text = "MCS Online New Sheet";
		base.Title = "MCS Online New Sheet";
		base.Load += new System.EventHandler(frmMain_Load);
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).EndInit();
		this.splitContainerMain.Panel1.ResumeLayout(false);
		this.splitContainerMain.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).EndInit();
		this.splitContainerMain.ResumeLayout(false);
		this.panelOnly1.ResumeLayout(false);
		this.panelOnly1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picLocatorGroup).EndInit();
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.backPanel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpPrintMain).EndInit();
		this.splitContainer2.Panel1.ResumeLayout(false);
		this.splitContainer2.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).EndInit();
		this.splitContainer2.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.backPanel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpPrintSave).EndInit();
		this.backPanel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpProdResult).EndInit();
		base.ResumeLayout(false);
	}
}
