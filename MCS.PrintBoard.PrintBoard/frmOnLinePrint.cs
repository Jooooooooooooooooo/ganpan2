using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FarPoint.Win;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;
using MCS.Common.BizRule;
using PresentationControls;

namespace MCS.PrintBoard.PrintBoard;

public class frmOnLinePrint : frmBase
{
	private TextBox[] tbary = new TextBox[10];

	private string[] CurrentItem = new string[4];

	private int iWO = -1;

	private Random ran = new Random();

	private int iActiveRow = 0;

	private int iSheetQty = 0;

	private string sLocatorGroupCode;

	private string sLocatorGroupName;

	private string sOrgID = string.Empty;

	private DataSet dsLocatorGroup = new DataSet();

	private DataSet dsLocator = new DataSet();

	private DataTable dtPrintMain = new DataTable();

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

	private MCS.Common.FpSpread fpPrintMain;

	private Label label1;

	private System.Windows.Forms.Button btn_Split;

	private System.Windows.Forms.Button btn_Edit;

	private Label label6;

	private Label label7;

	private Label label4;

	private Label label5;

	private Label label3;

	private Label label2;

	private TextBox txtSheetID;

	private TextBox txtPartNo;

	private TextBox txtWorkOrder;

	private TextBox txtLocatorGroup;

	private PictureBox picLocatorGroup;

	private MCS.Common.ComboBox cboOrg;

	private CheckBoxComboBox cmbLocator;

	private CheckBoxComboBox cmbLocatorGroup;

	private System.Windows.Forms.Button btn_Search;

	private PanelOnly panelOnly1;

	private Panel panel1;

	private System.Windows.Forms.Button btn_preview;

	private System.Windows.Forms.Button btn_Delete;

	private Label label9;

	private System.Windows.Forms.Button btn_Merge;

	public frmOnLinePrint()
	{
		InitializeComponent();
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		SetComboInit();
		SetcmbLocator("INIT");
		SetcmbLocatorGroup("INIT");
		procMakeSheetColumn();
		sOrgID = cboOrg.SelectedValue.ToString();
	}

	private void cmbLocatorGroup_CheckBoxCheckedChanged(object sender, EventArgs e)
	{
		SetcmbLocator("INIT");
	}

	private void cmbLocator_CheckBoxCheckedChanged(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
	}

	private void btn_preview_Click(object sender, EventArgs e)
	{
		Cursor = Cursors.WaitCursor;
		OfflinePrintBoard rpt = new OfflinePrintBoard();
		rpt.DataSource = dtGroupBy;
		rpt.DataMember = dtGroupBy.TableName;
		Cursor = Cursors.Arrow;
	}

	private void btn_Print_Click(object sender, EventArgs e)
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

	private void cmbLocatorGroup_KeyUp(object sender, KeyEventArgs e)
	{
	}

	private void cmbLocator_KeyUp(object sender, KeyEventArgs e)
	{
	}

	private void fpPrintMain_CellClick(object sender, CellClickEventArgs e)
	{
		try
		{
			if (e.ColumnHeader && e.Column == fpPrintMain.ActiveSheet.GetColumnIndex("CHK"))
			{
				string sTrue = "True";
				if (fpPrintMain.ActiveSheet.GetText(0, fpPrintMain.ActiveSheet.GetColumnIndex("CHK")) == "True")
				{
					sTrue = "False";
				}
				for (int i = 0; fpPrintMain.ActiveSheet.Rows.Count > i; i++)
				{
					fpPrintMain.ActiveSheet.SetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("CHK"), sTrue);
				}
			}
			else
			{
				if (e.ColumnHeader)
				{
					return;
				}
				if (fpPrintMain.ActiveSheet.GetText(e.Row, fpPrintMain.ActiveSheet.GetColumnIndex("CHK")) == "True")
				{
					if (e.Column != fpPrintMain.ActiveSheet.GetColumnIndex("CHK"))
					{
						fpPrintMain.ActiveSheet.SetText(e.Row, fpPrintMain.ActiveSheet.GetColumnIndex("CHK"), "False");
					}
					string sSHEET_ID2 = fpPrintMain.ActiveSheet.GetText(e.Row, "SHEET_ID");
					for (int j = 0; fpPrintMain.ActiveSheet.Rows.Count > j; j++)
					{
						if (sSHEET_ID2 == fpPrintMain.ActiveSheet.GetText(j, "SHEET_ID"))
						{
							fpPrintMain.ActiveSheet.SetText(j, fpPrintMain.ActiveSheet.GetColumnIndex("CHK"), "False");
						}
					}
					return;
				}
				if (e.Column != fpPrintMain.ActiveSheet.GetColumnIndex("CHK"))
				{
					fpPrintMain.ActiveSheet.SetText(e.Row, fpPrintMain.ActiveSheet.GetColumnIndex("CHK"), "True");
				}
				string sSHEET_ID = fpPrintMain.ActiveSheet.GetText(e.Row, "SHEET_ID");
				for (int k = 0; fpPrintMain.ActiveSheet.Rows.Count > k; k++)
				{
					if (sSHEET_ID == fpPrintMain.ActiveSheet.GetText(k, "SHEET_ID"))
					{
						fpPrintMain.ActiveSheet.SetText(k, fpPrintMain.ActiveSheet.GetColumnIndex("CHK"), "True");
					}
				}
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void btn_Search_Click(object sender, EventArgs e)
	{
		DataSet ds = new DataSet();
		BizService bizServer = new BizService();
		string sLOCATOR_GROUP = null;
		string sLOCATOR = null;
		string sWORK_ORDER = txtWorkOrder.Text.Trim();
		string sPART_NO = txtPartNo.Text.Trim();
		string sSHEET_ID = txtSheetID.Text.Trim();
		if (cmbLocatorGroup.Tag != null)
		{
			sLOCATOR_GROUP = cmbLocatorGroup.Tag.ToString().Replace(",", "|");
		}
		if (cmbLocator.Tag != null)
		{
			sLOCATOR = cmbLocator.Tag.ToString().Replace(",", "|");
		}
		LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "ORG_ID", (sOrgID == "") ? null : sOrgID);
		LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "LOCATOR", (sLOCATOR == "") ? null : sLOCATOR);
		LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "LOCATOR_GROUP", (sLOCATOR_GROUP == "") ? null : sLOCATOR_GROUP);
		LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "WORK_ORDER", (sWORK_ORDER == "") ? null : sWORK_ORDER);
		LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "PART_NO", (sPART_NO == "") ? null : sPART_NO);
		LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "SHEET_ID", (sSHEET_ID == "") ? null : sSHEET_ID);
		LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "RQSTDT", "LANGID", CultureInfo.CurrentUICulture.Name);
		DataSet dsResult = bizServer.ExecBizRule("SEL_PRINT_SHEET_INFO", ds, "RQSTDT", "RSLTDT");
		fpPrintMain.DataSource = dsResult;
		fpPrintMain.Refresh();
		frmBase.gstBarMain.Items[0].Text = " Search has been completed ! [ " + fpPrintMain.ActiveSheet.RowCount + "  Rows ]";
	}

	private void button1_Click(object sender, EventArgs e)
	{
		fpPrintMain.ActiveSheet.Rows.Clear();
		fpPrintMain.ActiveSheet.Rows.Add(0, 100);
	}

	private void btn_Edit_Click(object sender, EventArgs e)
	{
		string sSheetIDNow = string.Empty;
		string sSheetIDOld = string.Empty;
		int iSheetCount = 0;
		int iQty = 0;
		iSheetQty = 0;
		for (int i = 0; i < fpPrintMain.ActiveSheet.Rows.Count; i++)
		{
			if (fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("CHK")) == "True")
			{
				sSheetIDNow = fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("SHEET_ID"));
				int.TryParse(fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("QTY")).ToString(), out iQty);
				if (string.IsNullOrEmpty(sSheetIDOld))
				{
					sSheetIDOld = sSheetIDNow;
					iSheetCount = 1;
					iActiveRow = i;
				}
				if (sSheetIDNow != sSheetIDOld)
				{
					iSheetCount++;
				}
				else
				{
					iSheetQty += iQty;
				}
			}
		}
		if (iSheetCount > 1)
		{
			MessageBox.Show("More than one Sheet ID selected. Please select only one SHEET ID.");
		}
		else
		{
			if (iSheetCount == 0)
			{
				return;
			}
			using frmOnLinePrintEdit_P frm = new frmOnLinePrintEdit_P();
			frm.sGROUP_TXN_ID = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("GROUP_TXN_ID"));
			frm.sPartNo = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("PART_NO"));
			frm.sSheetID = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("SHEET_ID"));
			frm.sLocator = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("LOCATOR"));
			frm.sTotalQty = iSheetQty.ToString();
			frm.sPlanSeq = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("SEQ"));
			frm.sWorkOrder = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("WORK_ORDER"));
			frm.sWoQty = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("QTY"));
			frm.sOrgID = sOrgID;
			frm.sCarrierID = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("CARRIER_ID"));
			if (frm.ShowDialog() == DialogResult.OK)
			{
				btn_Search_Click(null, null);
			}
		}
	}

	private void btn_Delete_Click(object sender, EventArgs e)
	{
		string sSheetIDNow = string.Empty;
		string sSheetIDOld = string.Empty;
		int iSheetCount = 0;
		int iQty = 0;
		iSheetQty = 0;
		if (fpPrintMain.ActiveSheet.RowCount == 0)
		{
			return;
		}
		for (int i = 0; i < fpPrintMain.ActiveSheet.Rows.Count; i++)
		{
			if (fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("CHK")) == "True")
			{
				sSheetIDNow = fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("SHEET_ID"));
				int.TryParse(fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("QTY")).ToString(), out iQty);
				if (string.IsNullOrEmpty(sSheetIDOld))
				{
					sSheetIDOld = sSheetIDNow;
					iSheetCount = 1;
					iActiveRow = i;
				}
				if (sSheetIDNow != sSheetIDOld)
				{
					iSheetCount++;
				}
				else
				{
					iSheetQty += iQty;
				}
			}
		}
		if (iSheetCount > 1)
		{
			MessageBox.Show("More than one Sheet ID selected. Please select only one SHEET ID.");
		}
		else if (iSheetCount != 0 && MessageBox.Show("Do you want to Delete SheetID [ " + sSheetIDNow + " ] ?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
		{
			string pPartNo = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("PART_NO"));
			string pSheetID = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("SHEET_ID"));
			string pLocator = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("LOCATOR"));
			string pTotalQty = iSheetQty.ToString();
			string pPlanSeq = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("SEQ"));
			string pWorkOrder = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("WORK_ORDER"));
			string pWoQty = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("QTY"));
			if (SetDeleverySheet(pSheetID, pLocator, pPartNo, pTotalQty, pPlanSeq, pWorkOrder, pWoQty))
			{
				MessageBox.Show("Deleting is complete.");
				btn_Search_Click(null, null);
			}
		}
	}

	private void btn_Split_Click(object sender, EventArgs e)
	{
		string sSheetIDNow = string.Empty;
		string sSheetIDOld = string.Empty;
		int iSheetCount = 0;
		int iQty = 0;
		iSheetQty = 0;
		if (fpPrintMain.ActiveSheet.Rows.Count < 1)
		{
			return;
		}
		for (int i = 0; i < fpPrintMain.ActiveSheet.Rows.Count; i++)
		{
			if (fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("CHK")) == "True")
			{
				sSheetIDNow = fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("SHEET_ID"));
				int.TryParse(fpPrintMain.ActiveSheet.GetText(i, fpPrintMain.ActiveSheet.GetColumnIndex("QTY")).ToString(), out iQty);
				if (string.IsNullOrEmpty(sSheetIDOld))
				{
					sSheetIDOld = sSheetIDNow;
					iSheetCount = 1;
					iActiveRow = i;
				}
				if (sSheetIDNow != sSheetIDOld)
				{
					iSheetCount++;
				}
				else
				{
					iSheetQty += iQty;
				}
			}
		}
		if (iSheetCount > 1)
		{
			MessageBox.Show("More than one Sheet ID selected. Please select only one SHEET ID.");
		}
		else
		{
			if (iSheetCount == 0)
			{
				return;
			}
			using frmOnLinePrintSplit_P frm = new frmOnLinePrintSplit_P();
			frm.sPartNo = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("PART_NO"));
			frm.sSheetID = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("SHEET_ID"));
			frm.sLocator = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("LOCATOR"));
			frm.sTotalQty = iSheetQty.ToString();
			frm.sPlanSeq = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("SEQ"));
			frm.sWorkOrder = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("WORK_ORDER"));
			frm.sWoQty = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("QTY"));
			frm.sOrgID = sOrgID;
			frm.sGROUP_TXN_ID = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("GROUP_TXN_ID"));
			frm.sSheetID = fpPrintMain.ActiveSheet.GetText(iActiveRow, fpPrintMain.ActiveSheet.GetColumnIndex("SHEET_ID"));
			if (frm.ShowDialog() == DialogResult.OK)
			{
				btn_Search_Click(null, null);
			}
		}
	}

	private void btn_Merge_Click(object sender, EventArgs e)
	{
	}

	private void procMakeSheetColumn()
	{
		try
		{
			MCS.Common.SheetView svPrintSearch = new MCS.Common.SheetView(fpPrintMain, "Search", OperationMode.ReadOnly, bRowHeaderVisible: true, "BackColor White");
			svPrintSearch.AddColumnCheckBox("ALL", "CHK", 40, CellHorizontalAlignment.Center, bLocked: false, bVisible: true, "", "", "", bThreeState: false);
			svPrintSearch.AddColumnText("Line", "LINE", 60, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 30);
			svPrintSearch.AddColumnText("Sheet ID", "SHEET_ID", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 30);
			svPrintSearch.AddColumnText("Part No", "PART_NO", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("Description", "DESCRIPTION", 200, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 500);
			svPrintSearch.AddColumnText("Locator Group", "LOCATOR_GROUP", 120, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("Locator", "LOCATOR", 120, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("Carrier ID", "CARRIER_ID", 100, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 300);
			svPrintSearch.AddColumnText("Seq", "SEQ", 60, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 10);
			svPrintSearch.AddColumnText("Work Order", "WORK_ORDER", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 50);
			svPrintSearch.AddColumnText("From", "SEQ_FROM", 80, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 120);
			svPrintSearch.AddColumnText("To", "SEQ_TO", 80, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 120);
			svPrintSearch.AddColumnText("Qty", "QTY", 80, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 120);
			svPrintSearch.AddColumnText("Model.Suffix", "MODEL_SUFFIX", 100, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 50);
			svPrintSearch.AddColumnText("Print Count", "PRINT_COUNT", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 50);
			svPrintSearch.AddColumnText("PST", "PST", 80, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("CREATED_DATE", "CREATED_DATE", 120, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("CREATED_BY", "CREATED_BY", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("UPDATED_DATE", "UPDATED_DATE", 120, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("UPDATED_BY", "UPDATED_BY", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintSearch.AddColumnText("", "GROUP_TXN_ID", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 200);
			svPrintSearch.AddColumnText("", "MADE_BY", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 200);
			svPrintSearch.SetColumnMerge(0, MergePolicy.None);
			svPrintSearch.SetColumnMerge(1, MergePolicy.Always);
			svPrintSearch.SetColumnMerge(2, MergePolicy.Always);
			svPrintSearch.SetColumnMerge(3, MergePolicy.Always);
			svPrintSearch.SetColumnMerge(4, MergePolicy.Always);
			svPrintSearch.SetColumnMerge(5, MergePolicy.Always);
			dtPrintMain.Columns.Add("CHK");
			dtPrintMain.Columns.Add("LINE");
			dtPrintMain.Columns.Add("SHEET_ID");
			dtPrintMain.Columns.Add("PART_NO");
			dtPrintMain.Columns.Add("DESCRIPTION");
			dtPrintMain.Columns.Add("LOCATOR_GROUP");
			dtPrintMain.Columns.Add("LOCATOR");
			dtPrintMain.Columns.Add("CARRIER_ID");
			dtPrintMain.Columns.Add("SEQ");
			dtPrintMain.Columns.Add("WORK_ORDER");
			dtPrintMain.Columns.Add("SEQ_FROM");
			dtPrintMain.Columns.Add("SEQ_TO");
			dtPrintMain.Columns.Add("QTY");
			dtPrintMain.Columns.Add("MODEL_SUFFIX");
			dtPrintMain.Columns.Add("PRINT_COUNT");
			dtPrintMain.Columns.Add("PST");
			dtPrintMain.Columns.Add("CREATED_DATE");
			dtPrintMain.Columns.Add("CREATED_BY");
			dtPrintMain.Columns.Add("UPDATED_DATE");
			dtPrintMain.Columns.Add("UPDATED_BY");
			dtPrintMain.Columns.Add("GROUP_TXN_ID");
			dtPrintMain.Columns.Add("MADE_BY");
			svPrintSearch.RowHeader.Visible = true;
			svPrintSearch.Rows.Default.Height = 22f;
			svPrintSearch.ColumnHeader.Rows[0].Height = 25f;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SearchProdPlan()
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			DataSet dsTemp = Oracle_BIZ.PROD_PLAN(bFlag: false);
			if (dsTemp != null)
			{
			}
			Cursor = Cursors.Default;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
			Cursor = Cursors.Default;
		}
	}

	private bool GetDataFromfpPrintMain()
	{
		string sValue = string.Empty;
		DataTable dtGroupTemp = new DataTable();
		DataTable dtFpTemp = new DataTable();
		bool bReturn = false;
		dtPrintMain.Clear();
		try
		{
			for (int r = 0; fpPrintMain.ActiveSheet.Rows.Count > r; r++)
			{
				DataRow dr = dtPrintMain.NewRow();
				for (int c = 0; fpPrintMain.ActiveSheet.Columns.Count > c; c++)
				{
					if (fpPrintMain.ActiveSheet.GetText(r, c) == "False")
					{
						continue;
					}
					try
					{
						sValue = fpPrintMain.ActiveSheet.GetValue(r, c).ToString();
						if (!string.IsNullOrEmpty(sValue))
						{
							dr[c] = sValue;
						}
					}
					catch (Exception)
					{
						sValue = string.Empty;
					}
				}
				if (dr["CHK"].ToString() == "True" && dr["SHEET_ID"].ToString() != "" && dr["LOCATOR_GROUP"].ToString() != "" && dr["PART_NO"].ToString() != "" && dr["WORK_ORDER"].ToString() != "")
				{
					dtPrintMain.Rows.Add(dr);
				}
			}
			dtGroupTemp = dtPrintMain.Copy();
			string[] sColnames = new string[4] { "LINE", "LOCATOR_GROUP", "SHEET_ID", "PART_NO" };
			dtGroupBy = dtGroupTemp.DefaultView.ToTable(distinct: true, sColnames);
			dtGroupBy.Columns.Add("TOTAL_QUANTITY");
			dtGroupBy.Columns.Add("MADE_BY");
			dtGroupBy.Columns.Add("CARRIER_ID");
			dtGroupBy.Columns.Add("TOTAL_SEQ_NO");
			dtGroupBy.Columns.Add("DESCRIPTION");
			dtGroupBy.Columns.Add("INSPECTION_FLAG");
			dtGroupBy.Columns.Add("PST");
			dtGroupBy.Columns.Add("PRINTED_DATE");
			dtGroupBy.Columns.Add("SCHEDULE_GROUP_DESC");
			dtGroupBy.Columns.Add("QRCODE_VALUE");
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
				string sLINE = dtGroupBy.Rows[i]["LINE"].ToString();
				string sLOCATOR_GROUP = dtGroupBy.Rows[i]["LOCATOR_GROUP"].ToString();
				string sSHEET_ID = dtGroupBy.Rows[i]["SHEET_ID"].ToString();
				string sPART_NO = dtGroupBy.Rows[i]["PART_NO"].ToString();
				string sINSPECTION_FLAG = "";
				string sFilterExpression = $"LINE = '{sLINE}' AND LOCATOR_GROUP = '{sLOCATOR_GROUP}' AND SHEET_ID = '{sSHEET_ID}' AND PART_NO = '{sPART_NO}'";
				int iTOTAL_QUANTITY = 0;
				string sTOTAL_SEQ_NO_FROM = string.Empty;
				string sTOTAL_SEQ_NO_TO = string.Empty;
				string sTOTAL_SEQ = string.Empty;
				DataRow[] Rows = dtGroupTemp.Select(sFilterExpression);
				if (Rows.Length > 5)
				{
					MessageBox.Show("Error : WorkOrder Are More than 5.  ");
				}
				for (int j = 0; Rows.Length > j && 5 > j; j++)
				{
					string sCARRIER_ID = Rows[j]["CARRIER_ID"].ToString();
					string sDESCRIPTION = Rows[j]["DESCRIPTION"].ToString();
					string sPST = Rows[j]["PST"].ToString();
					string sWORK_ORDER = Rows[j]["WORK_ORDER"].ToString();
					string sWO_QUANTITY = Rows[j]["QTY"].ToString();
					string sPROD_SEQ_FROM = Rows[j]["SEQ_FROM"].ToString();
					string sPROD_SEQ_TO = Rows[j]["SEQ_TO"].ToString();
					string sMODEL_SUFFIX = Rows[j]["MODEL_SUFFIX"].ToString();
					string sSCHEDULE_GROUP_DESC = Rows[j]["MADE_BY"].ToString();
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
						dtGroupBy.Rows[i]["CARRIER_ID"] = sCARRIER_ID;
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
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", null);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LANGID", CultureInfo.CurrentUICulture.Name);
			DataSet dsResult = bizServer.ExecBizRule("SEL_CM_ORGNAME", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				cboOrg.SetItemList(dsResult.Tables["OUT_DATA"], "ORG_ID", "ORG_NAME", AllFlag: false);
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
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
				sOrgID = cboOrg.SelectedValue.ToString();
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
			for (int i = 0; dtPrintMain.Rows.Count > i; i++)
			{
				sDIV = "INS";
				sSHEET_ID = dtPrintMain.Rows[i]["SHEET_ID"].ToString();
				sSHEET_TYPE = "MCS_ONLINE";
				sMADE_LOCATOR = null;
				sDELIVERY_TO = dtPrintMain.Rows[i]["LOCATOR"].ToString();
				sITEM_CODE = dtPrintMain.Rows[i]["PART_NO"].ToString();
				sWO_NAME = null;
				iTOTAL_QTY = int.Parse(dtPrintMain.Rows[i]["TOTAL_QTY"].ToString());
				iSEQ = int.Parse(dtPrintMain.Rows[i]["PLAN_SEQ"].ToString());
				sTO_WO_NAME = dtPrintMain.Rows[i]["WORK_ORDER"].ToString();
				sTO_ITEM_CODE = null;
				iTO_WO_QTY = int.Parse(dtPrintMain.Rows[i]["WO_QTY"].ToString());
				sPRINT_YN = "N";
				sSTATUS = null;
				sCLOSED_YN = "N";
				sGROUP_TXN_ID = dtPrintMain.Rows[i]["GROUP_TXN_ID"].ToString();
				sMADE_BY = null;
				ATTRIBUTE = null;
				ATTRIBUTE2 = null;
				ATTRIBUTE3 = null;
				ATTRIBUTE4 = null;
				ATTRIBUTE5 = null;
				sCREATED_BY = null;
				sUPDATED_BY = null;
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DIV", (sDIV == "") ? null : sDIV);
				LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sOrgID == "") ? null : sOrgID);
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

	private bool SetDeleverySheet(string pSheetID, string pLocator, string pPartNo, string pTotalQty, string pPlanSeq, string pWorkOrder, string pWoQty)
	{
		bool bReturn = false;
		string sReturnMsg = string.Empty;
		string sReturnCode = string.Empty;
		string sDIV = "DEL";
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
			sDIV = "DEL";
			sSHEET_ID = pSheetID;
			sSHEET_TYPE = "MCS_ONLINE";
			sMADE_LOCATOR = null;
			sDELIVERY_TO = pLocator;
			sITEM_CODE = pPartNo;
			sWO_NAME = null;
			iTOTAL_QTY = int.Parse(pTotalQty);
			iSEQ = int.Parse(pPlanSeq);
			sTO_WO_NAME = pWorkOrder;
			sTO_ITEM_CODE = null;
			iTO_WO_QTY = int.Parse(pWoQty);
			sPRINT_YN = "N";
			sSTATUS = null;
			sCLOSED_YN = "N";
			sGROUP_TXN_ID = null;
			sMADE_BY = null;
			ATTRIBUTE = null;
			ATTRIBUTE2 = null;
			ATTRIBUTE3 = null;
			ATTRIBUTE4 = null;
			ATTRIBUTE5 = null;
			sCREATED_BY = null;
			sUPDATED_BY = null;
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DIV", (sDIV == "") ? null : sDIV);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sOrgID == "") ? null : sOrgID);
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

	private void cmbLocatorGroup_SelectedIndexChanged(object sender, EventArgs e)
	{
	}

	private void cboOrg_SelectedIndexChanged(object sender, EventArgs e)
	{
		sOrgID = cboOrg.SelectedValue.ToString();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmOnLinePrint));
		PresentationControls.CheckBoxProperties checkBoxProperties1 = new PresentationControls.CheckBoxProperties();
		PresentationControls.CheckBoxProperties checkBoxProperties2 = new PresentationControls.CheckBoxProperties();
		FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer3 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer4 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		this.tmDemo = new System.Windows.Forms.Timer(this.components);
		this.dgvBuffer = new System.Windows.Forms.DataGridView();
		this.tmRefresh = new System.Windows.Forms.Timer(this.components);
		this.searchPanel1 = new MCS.Common.SearchPanel();
		this.splitContainerMain = new System.Windows.Forms.SplitContainer();
		this.panelOnly1 = new MCS.Common.PanelOnly();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbLocatorGroup = new PresentationControls.CheckBoxComboBox();
		this.txtSheetID = new System.Windows.Forms.TextBox();
		this.cmbLocator = new PresentationControls.CheckBoxComboBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtLocatorGroup = new System.Windows.Forms.TextBox();
		this.cboOrg = new MCS.Common.ComboBox();
		this.picLocatorGroup = new System.Windows.Forms.PictureBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtWorkOrder = new System.Windows.Forms.TextBox();
		this.txtPartNo = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btn_Merge = new System.Windows.Forms.Button();
		this.btn_Search = new System.Windows.Forms.Button();
		this.btn_preview = new System.Windows.Forms.Button();
		this.btn_Split = new System.Windows.Forms.Button();
		this.btn_Delete = new System.Windows.Forms.Button();
		this.btn_Edit = new System.Windows.Forms.Button();
		this.fpPrintMain = new MCS.Common.FpSpread();
		this.label1 = new System.Windows.Forms.Label();
		this.backPanel2 = new MCS.Common.BackPanel();
		this.fpProdResult = new MCS.Common.FpSpread();
		this.backPanel1 = new MCS.Common.BackPanel();
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).BeginInit();
		this.splitContainerMain.Panel1.SuspendLayout();
		this.splitContainerMain.Panel2.SuspendLayout();
		this.splitContainerMain.SuspendLayout();
		this.panelOnly1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picLocatorGroup).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpPrintMain).BeginInit();
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
		this.splitContainerMain.BackColor = System.Drawing.Color.Transparent;
		this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainerMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
		this.splitContainerMain.Name = "splitContainerMain";
		this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainerMain.Panel1.Controls.Add(this.panelOnly1);
		this.splitContainerMain.Panel2.Controls.Add(this.fpPrintMain);
		this.splitContainerMain.Panel2.Controls.Add(this.label1);
		this.splitContainerMain.Size = new System.Drawing.Size(1197, 761);
		this.splitContainerMain.SplitterDistance = 121;
		this.splitContainerMain.SplitterWidth = 1;
		this.splitContainerMain.TabIndex = 66;
		this.panelOnly1.BackgroundImage = (System.Drawing.Image)resources.GetObject("panelOnly1.BackgroundImage");
		this.panelOnly1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.panelOnly1.Controls.Add(this.label9);
		this.panelOnly1.Controls.Add(this.cmbLocatorGroup);
		this.panelOnly1.Controls.Add(this.txtSheetID);
		this.panelOnly1.Controls.Add(this.cmbLocator);
		this.panelOnly1.Controls.Add(this.label6);
		this.panelOnly1.Controls.Add(this.label7);
		this.panelOnly1.Controls.Add(this.txtLocatorGroup);
		this.panelOnly1.Controls.Add(this.cboOrg);
		this.panelOnly1.Controls.Add(this.picLocatorGroup);
		this.panelOnly1.Controls.Add(this.label2);
		this.panelOnly1.Controls.Add(this.label3);
		this.panelOnly1.Controls.Add(this.txtWorkOrder);
		this.panelOnly1.Controls.Add(this.txtPartNo);
		this.panelOnly1.Controls.Add(this.label4);
		this.panelOnly1.Controls.Add(this.label5);
		this.panelOnly1.Controls.Add(this.panel1);
		this.panelOnly1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelOnly1.Location = new System.Drawing.Point(0, 0);
		this.panelOnly1.Name = "panelOnly1";
		this.panelOnly1.Padding = new System.Windows.Forms.Padding(8);
		this.panelOnly1.Size = new System.Drawing.Size(1197, 121);
		this.panelOnly1.TabIndex = 81;
		this.label9.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label9.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(270, 31);
		this.label9.TabIndex = 106;
		this.label9.Text = "Online Sheet Management";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbLocatorGroup.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbLocatorGroup.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		checkBoxProperties1.AutoEllipsis = true;
		checkBoxProperties1.AutoSize = true;
		checkBoxProperties1.FlatAppearanceBorderSize = 5;
		checkBoxProperties1.FlatAppearanceMouseOverBackColor = System.Drawing.Color.FromArgb(255, 224, 192);
		checkBoxProperties1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.cmbLocatorGroup.CheckBoxProperties = checkBoxProperties1;
		this.cmbLocatorGroup.DisplayMemberSingleItem = "";
		this.cmbLocatorGroup.Font = new System.Drawing.Font("Arial", 9f);
		this.cmbLocatorGroup.ForeColor = System.Drawing.Color.Black;
		this.cmbLocatorGroup.FormattingEnabled = true;
		this.cmbLocatorGroup.Location = new System.Drawing.Point(509, 55);
		this.cmbLocatorGroup.MaxDropDownItems = 20;
		this.cmbLocatorGroup.Name = "cmbLocatorGroup";
		this.cmbLocatorGroup.Size = new System.Drawing.Size(159, 23);
		this.cmbLocatorGroup.TabIndex = 105;
		this.cmbLocatorGroup.Visible = false;
		this.cmbLocatorGroup.CheckBoxCheckedChanged += new System.EventHandler(cmbLocatorGroup_CheckBoxCheckedChanged);
		this.cmbLocatorGroup.SelectedIndexChanged += new System.EventHandler(cmbLocatorGroup_SelectedIndexChanged);
		this.txtSheetID.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtSheetID.Font = new System.Drawing.Font("Arial", 9f);
		this.txtSheetID.Location = new System.Drawing.Point(918, 85);
		this.txtSheetID.Name = "txtSheetID";
		this.txtSheetID.Size = new System.Drawing.Size(165, 21);
		this.txtSheetID.TabIndex = 90;
		this.cmbLocator.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cmbLocator.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		checkBoxProperties2.FlatAppearanceBorderSize = 3;
		checkBoxProperties2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.cmbLocator.CheckBoxProperties = checkBoxProperties2;
		this.cmbLocator.DisplayMemberSingleItem = "";
		this.cmbLocator.Font = new System.Drawing.Font("Arial", 9f);
		this.cmbLocator.FormattingEnabled = true;
		this.cmbLocator.Location = new System.Drawing.Point(918, 55);
		this.cmbLocator.MaxDropDownItems = 20;
		this.cmbLocator.Name = "cmbLocator";
		this.cmbLocator.Size = new System.Drawing.Size(165, 23);
		this.cmbLocator.TabIndex = 102;
		this.cmbLocator.Visible = false;
		this.cmbLocator.CheckBoxCheckedChanged += new System.EventHandler(cmbLocator_CheckBoxCheckedChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(849, 58);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(61, 16);
		this.label6.TabIndex = 96;
		this.label6.Text = "Locator";
		this.label6.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(785, 86);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(126, 16);
		this.label7.TabIndex = 95;
		this.label7.Text = "Sheet(Carrier) ID";
		this.txtLocatorGroup.Location = new System.Drawing.Point(674, 85);
		this.txtLocatorGroup.Name = "txtLocatorGroup";
		this.txtLocatorGroup.ReadOnly = true;
		this.txtLocatorGroup.Size = new System.Drawing.Size(11, 21);
		this.txtLocatorGroup.TabIndex = 97;
		this.txtLocatorGroup.Visible = false;
		this.cboOrg.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboOrg.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboOrg.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboOrg.Font = new System.Drawing.Font("Arial", 9f);
		this.cboOrg.ForeColor = System.Drawing.Color.Black;
		this.cboOrg.FormattingEnabled = true;
		this.cboOrg.Location = new System.Drawing.Point(112, 55);
		this.cboOrg.Name = "cboOrg";
		this.cboOrg.Size = new System.Drawing.Size(152, 23);
		this.cboOrg.TabIndex = 99;
		this.cboOrg.SelectedIndexChanged += new System.EventHandler(cboOrg_SelectedIndexChanged);
		this.picLocatorGroup.Image = (System.Drawing.Image)resources.GetObject("picLocatorGroup.Image");
		this.picLocatorGroup.Location = new System.Drawing.Point(671, 55);
		this.picLocatorGroup.Margin = new System.Windows.Forms.Padding(0);
		this.picLocatorGroup.Name = "picLocatorGroup";
		this.picLocatorGroup.Size = new System.Drawing.Size(23, 23);
		this.picLocatorGroup.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		this.picLocatorGroup.TabIndex = 98;
		this.picLocatorGroup.TabStop = false;
		this.picLocatorGroup.Visible = false;
		this.picLocatorGroup.Click += new System.EventHandler(picLocatorGroup_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(16, 86);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(89, 16);
		this.label2.TabIndex = 91;
		this.label2.Text = "Work Order";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(69, 58);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(34, 16);
		this.label3.TabIndex = 92;
		this.label3.Text = "Org";
		this.txtWorkOrder.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtWorkOrder.Font = new System.Drawing.Font("Arial", 9f);
		this.txtWorkOrder.Location = new System.Drawing.Point(112, 85);
		this.txtWorkOrder.Name = "txtWorkOrder";
		this.txtWorkOrder.Size = new System.Drawing.Size(152, 21);
		this.txtWorkOrder.TabIndex = 88;
		this.txtPartNo.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtPartNo.Font = new System.Drawing.Font("Arial", 9f);
		this.txtPartNo.Location = new System.Drawing.Point(509, 85);
		this.txtPartNo.Name = "txtPartNo";
		this.txtPartNo.Size = new System.Drawing.Size(159, 21);
		this.txtPartNo.TabIndex = 89;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(395, 58);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(109, 16);
		this.label4.TabIndex = 94;
		this.label4.Text = "Locator Group";
		this.label4.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(444, 86);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(59, 16);
		this.label5.TabIndex = 93;
		this.label5.Text = "Part No";
		this.panel1.BackColor = System.Drawing.Color.Transparent;
		this.panel1.Controls.Add(this.btn_Merge);
		this.panel1.Controls.Add(this.btn_Search);
		this.panel1.Controls.Add(this.btn_preview);
		this.panel1.Controls.Add(this.btn_Split);
		this.panel1.Controls.Add(this.btn_Delete);
		this.panel1.Controls.Add(this.btn_Edit);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel1.Location = new System.Drawing.Point(533, 8);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(656, 105);
		this.panel1.TabIndex = 78;
		this.btn_Merge.BackColor = System.Drawing.Color.Transparent;
		this.btn_Merge.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Merge.BackgroundImage");
		this.btn_Merge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Merge.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Merge.Font = new System.Drawing.Font("Arial Narrow", 10f, System.Drawing.FontStyle.Bold);
		this.btn_Merge.ForeColor = System.Drawing.Color.Black;
		this.btn_Merge.Image = (System.Drawing.Image)resources.GetObject("btn_Merge.Image");
		this.btn_Merge.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Merge.Location = new System.Drawing.Point(244, 2);
		this.btn_Merge.Margin = new System.Windows.Forms.Padding(0);
		this.btn_Merge.Name = "btn_Merge";
		this.btn_Merge.Size = new System.Drawing.Size(100, 30);
		this.btn_Merge.TabIndex = 6;
		this.btn_Merge.Text = "     Sheet Merge";
		this.btn_Merge.UseVisualStyleBackColor = false;
		this.btn_Merge.Click += new System.EventHandler(btn_Merge_Click);
		this.btn_Search.BackColor = System.Drawing.Color.Transparent;
		this.btn_Search.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Search.BackgroundImage");
		this.btn_Search.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Search.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Search.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_Search.ForeColor = System.Drawing.Color.Black;
		this.btn_Search.Image = (System.Drawing.Image)resources.GetObject("btn_Search.Image");
		this.btn_Search.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Search.Location = new System.Drawing.Point(40, 2);
		this.btn_Search.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Search.Name = "btn_Search";
		this.btn_Search.Size = new System.Drawing.Size(100, 30);
		this.btn_Search.TabIndex = 1;
		this.btn_Search.Text = "  Search";
		this.btn_Search.UseVisualStyleBackColor = false;
		this.btn_Search.Click += new System.EventHandler(btn_Search_Click);
		this.btn_preview.BackColor = System.Drawing.Color.Transparent;
		this.btn_preview.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_preview.BackgroundImage");
		this.btn_preview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_preview.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_preview.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_preview.ForeColor = System.Drawing.Color.Black;
		this.btn_preview.Image = (System.Drawing.Image)resources.GetObject("btn_preview.Image");
		this.btn_preview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_preview.Location = new System.Drawing.Point(142, 2);
		this.btn_preview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_preview.Name = "btn_preview";
		this.btn_preview.Size = new System.Drawing.Size(100, 30);
		this.btn_preview.TabIndex = 2;
		this.btn_preview.Text = "Print";
		this.btn_preview.UseVisualStyleBackColor = false;
		this.btn_preview.Click += new System.EventHandler(btn_Print_Click);
		this.btn_Split.BackColor = System.Drawing.Color.Transparent;
		this.btn_Split.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Split.BackgroundImage");
		this.btn_Split.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Split.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Split.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Bold);
		this.btn_Split.ForeColor = System.Drawing.Color.Black;
		this.btn_Split.Image = (System.Drawing.Image)resources.GetObject("btn_Split.Image");
		this.btn_Split.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Split.Location = new System.Drawing.Point(346, 2);
		this.btn_Split.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Split.Name = "btn_Split";
		this.btn_Split.Size = new System.Drawing.Size(100, 30);
		this.btn_Split.TabIndex = 3;
		this.btn_Split.Text = "   W/O Split";
		this.btn_Split.UseVisualStyleBackColor = false;
		this.btn_Split.Click += new System.EventHandler(btn_Split_Click);
		this.btn_Delete.BackColor = System.Drawing.Color.Transparent;
		this.btn_Delete.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Delete.BackgroundImage");
		this.btn_Delete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Delete.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Delete.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_Delete.ForeColor = System.Drawing.Color.Black;
		this.btn_Delete.Image = (System.Drawing.Image)resources.GetObject("btn_Delete.Image");
		this.btn_Delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Delete.Location = new System.Drawing.Point(448, 2);
		this.btn_Delete.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Delete.Name = "btn_Delete";
		this.btn_Delete.Size = new System.Drawing.Size(100, 30);
		this.btn_Delete.TabIndex = 4;
		this.btn_Delete.Text = "  Delete";
		this.btn_Delete.UseVisualStyleBackColor = false;
		this.btn_Delete.Click += new System.EventHandler(btn_Delete_Click);
		this.btn_Edit.BackColor = System.Drawing.Color.Transparent;
		this.btn_Edit.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Edit.BackgroundImage");
		this.btn_Edit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Edit.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Edit.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_Edit.ForeColor = System.Drawing.Color.Black;
		this.btn_Edit.Image = (System.Drawing.Image)resources.GetObject("btn_Edit.Image");
		this.btn_Edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Edit.Location = new System.Drawing.Point(550, 2);
		this.btn_Edit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Edit.Name = "btn_Edit";
		this.btn_Edit.Size = new System.Drawing.Size(100, 30);
		this.btn_Edit.TabIndex = 5;
		this.btn_Edit.Text = "  Edit";
		this.btn_Edit.UseVisualStyleBackColor = false;
		this.btn_Edit.Click += new System.EventHandler(btn_Edit_Click);
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
		this.fpPrintMain.Location = new System.Drawing.Point(0, 0);
		this.fpPrintMain.Name = "fpPrintMain";
		this.fpPrintMain.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintMain.Size = new System.Drawing.Size(1197, 639);
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
		this.fpPrintMain.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(fpPrintMain_CellClick);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(56, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(0, 15);
		this.label1.TabIndex = 0;
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
		this.fpProdResult.HorizontalScrollBar.Renderer = defaultScrollBarRenderer3;
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
		this.fpProdResult.VerticalScrollBar.Renderer = defaultScrollBarRenderer4;
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
		this.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(1197, 761);
		base.Controls.Add(this.splitContainerMain);
		this.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.DoubleBuffered = true;
		this.Font = new System.Drawing.Font("Arial", 9f);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOnLinePrint";
		this.Text = "MCS Online Sheet";
		base.Title = "MCS Online Sheet";
		base.Load += new System.EventHandler(frmMain_Load);
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).EndInit();
		this.splitContainerMain.Panel1.ResumeLayout(false);
		this.splitContainerMain.Panel2.ResumeLayout(false);
		this.splitContainerMain.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).EndInit();
		this.splitContainerMain.ResumeLayout(false);
		this.panelOnly1.ResumeLayout(false);
		this.panelOnly1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picLocatorGroup).EndInit();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpPrintMain).EndInit();
		this.backPanel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpProdResult).EndInit();
		base.ResumeLayout(false);
	}
}
