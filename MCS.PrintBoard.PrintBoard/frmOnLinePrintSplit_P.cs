using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;

namespace MCS.PrintBoard.PrintBoard;

public class frmOnLinePrintSplit_P : frmBase
{
	private Font fBold = new Font("맑은 고딕", 9f, FontStyle.Bold);

	private Font fRegular = new Font("맑은 고딕", 9f, FontStyle.Regular);

	private TreeNode tnTemp = null;

	private string _LocatorGroupCode;

	private string _LocatorGroupName;

	private string _PartNo;

	private string _Locator;

	private string _OrgID;

	private string _SheetID;

	private string _TotalQty;

	private string _PlanSeq;

	private string _WorkOrder;

	private string _WoQty;

	private string _LineCode;

	private string _GROUP_TXN_ID;

	public DataTable dtTemp = new DataTable();

	private IContainer components = null;

	private SplitContainer splitContainer1;

	private System.Windows.Forms.Button btn_close;

	private ImageList imgList;

	private System.Windows.Forms.Button btn_Save;

	private Label label3;

	private Label label2;

	private TextBox txtWorkOrderOld;

	private MCS.Common.ComboBox cboWorkOrderNew;

	private TextBox txtTotalQty;

	private Label label5;

	private NumericUpDown nUDQtyOld;

	private Label SheetID;

	private TextBox txtSheetID;

	private Label label1;

	private TextBox txtPartNo;

	private NumericUpDown nUDQtyNew;

	private TextBox textBox5;

	private TextBox textBox6;

	private TextBox textBox7;

	private TextBox textBox8;

	private TextBox textBox9;

	public string sGROUP_TXN_ID
	{
		get
		{
			return _GROUP_TXN_ID;
		}
		set
		{
			_GROUP_TXN_ID = value;
		}
	}

	public string sPartNo
	{
		get
		{
			return _PartNo;
		}
		set
		{
			_PartNo = value;
		}
	}

	public string sOrgID
	{
		get
		{
			return _OrgID;
		}
		set
		{
			_OrgID = value;
		}
	}

	public string sLocator
	{
		get
		{
			return _Locator;
		}
		set
		{
			_Locator = value;
		}
	}

	public string sLocatorGroupCode
	{
		get
		{
			return _LocatorGroupCode;
		}
		set
		{
			_LocatorGroupCode = value;
		}
	}

	public string sLocatorGroupName
	{
		get
		{
			return _LocatorGroupName;
		}
		set
		{
			_LocatorGroupName = value;
		}
	}

	public string sSheetID
	{
		get
		{
			return _SheetID;
		}
		set
		{
			_SheetID = value;
		}
	}

	public string sTotalQty
	{
		get
		{
			return _TotalQty;
		}
		set
		{
			_TotalQty = value;
		}
	}

	public string sPlanSeq
	{
		get
		{
			return _PlanSeq;
		}
		set
		{
			_PlanSeq = value;
		}
	}

	public string sWorkOrder
	{
		get
		{
			return _WorkOrder;
		}
		set
		{
			_WorkOrder = value;
		}
	}

	public string sWoQty
	{
		get
		{
			return _WoQty;
		}
		set
		{
			_WoQty = value;
		}
	}

	public string sLineCode
	{
		get
		{
			return _LineCode;
		}
		set
		{
			_LineCode = value;
		}
	}

	public frmOnLinePrintSplit_P()
	{
		InitializeComponent();
	}

	private void frmLocator_P_Load(object sender, EventArgs e)
	{
		try
		{
			txtSheetID.Text = sSheetID;
			txtPartNo.Text = sPartNo;
			txtWorkOrderOld.Text = sWorkOrder;
			txtTotalQty.Text = sWoQty;
			GetcboWo();
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void btn_close_Click(object sender, EventArgs e)
	{
		CloseForm(bReturn: false);
	}

	private void btn_Save_Click(object sender, EventArgs e)
	{
		Save();
	}

	private void Save()
	{
		if (MessageBox.Show("Do you want to Separate?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
		{
			return;
		}
		string sSheedID = string.Empty;
		try
		{
			string sWorkOrderOld = txtWorkOrderOld.Text.Trim();
			string sWorkOrderNew = cboWorkOrderNew.SelectedValue.ToString();
			string sWoQtyOld = nUDQtyOld.Value.ToString();
			string sWoQtyNew = nUDQtyNew.Value.ToString();
			if (int.Parse(txtTotalQty.Text) != int.Parse(sWoQtyOld) + int.Parse(sWoQtyNew))
			{
				MessageBox.Show("The quantity is different. Please modify the quantity. ");
				return;
			}
			bool bReturn1 = SetDeleverySheet("DEL", sSheetID, sLocator, sPartNo, sTotalQty, sPlanSeq, sWorkOrderOld, sWoQtyOld);
			bool bReturn2 = SetDeleverySheet("INS", GetSheedID(), sLocator, sPartNo, sTotalQty, sPlanSeq, sWorkOrderOld, sWoQtyOld);
			bool bReturn3 = SetDeleverySheet("INS", GetSheedID(), sLocator, sPartNo, sTotalQty, sPlanSeq, sWorkOrderNew, sWoQtyNew);
			if (bReturn1 && bReturn2 && bReturn3)
			{
				MessageBox.Show("Separation is complete.");
				CloseForm(bReturn: true);
			}
		}
		catch (Exception)
		{
		}
	}

	private string GetSheedID()
	{
		string sTxnID = string.Empty;
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sOrgID == "") ? null : sOrgID);
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

	private void CloseForm(bool bReturn)
	{
		if (bReturn)
		{
			base.DialogResult = DialogResult.OK;
		}
		else
		{
			base.DialogResult = DialogResult.None;
		}
		Close();
	}

	private void GetLocationGroup()
	{
		try
		{
			DataSet ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sOrgID == "") ? null : sOrgID);
			BizService bizServer = new BizService();
		}
		catch (Exception)
		{
		}
	}

	private bool SetDeleverySheet(string pDIV, string pSheetID, string pLocator, string pPartNo, string pTotalQty, string pPlanSeq, string pWorkOrder, string pWoQty)
	{
		bool bReturn = false;
		string sReturnMsg = string.Empty;
		string sReturnCode = string.Empty;
		string sDIV = pDIV;
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
			sDIV = pDIV;
			sSHEET_TYPE = "MCS_ONLINE";
			sMADE_LOCATOR = null;
			sDELIVERY_TO = pLocator;
			sITEM_CODE = pPartNo;
			sWO_NAME = null;
			iTOTAL_QTY = int.Parse(pWoQty);
			iSEQ = int.Parse(pPlanSeq);
			sTO_WO_NAME = pWorkOrder;
			sTO_ITEM_CODE = null;
			iTO_WO_QTY = int.Parse(pWoQty);
			sPRINT_YN = "N";
			sSTATUS = null;
			sCLOSED_YN = "N";
			sMADE_BY = null;
			ATTRIBUTE = null;
			ATTRIBUTE2 = null;
			ATTRIBUTE3 = null;
			ATTRIBUTE4 = null;
			ATTRIBUTE5 = null;
			sCREATED_BY = null;
			sUPDATED_BY = null;
			if (sDIV == "INS")
			{
				sGROUP_TXN_ID = GetTxnID();
				sSheetID = pSheetID;
			}
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DIV", (sDIV == "") ? null : sDIV);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sOrgID == "") ? null : sOrgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "SHEET_ID", (sSheetID == "") ? null : sSheetID);
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

	private void GetcboWo()
	{
		try
		{
			string sLINE_CODE = string.Empty;
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sOrgID == "") ? null : sOrgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LINE_CODE", (sLINE_CODE == "") ? null : sLINE_CODE);
			DataSet dsResult = bizServer.ExecBizRule("GMCS_GET_WO_NAME_WITHIN_D1_DAY", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				cboWorkOrderNew.SetItemList(dsResult.Tables["OUT_DATA"], "WO_NAME", "WO_NAME", AllFlag: false);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void setControlInit()
	{
		try
		{
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmOnLinePrintSplit_P));
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.txtTotalQty = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.nUDQtyOld = new System.Windows.Forms.NumericUpDown();
		this.SheetID = new System.Windows.Forms.Label();
		this.txtSheetID = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtPartNo = new System.Windows.Forms.TextBox();
		this.nUDQtyNew = new System.Windows.Forms.NumericUpDown();
		this.cboWorkOrderNew = new MCS.Common.ComboBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtWorkOrderOld = new System.Windows.Forms.TextBox();
		this.textBox5 = new System.Windows.Forms.TextBox();
		this.textBox6 = new System.Windows.Forms.TextBox();
		this.textBox7 = new System.Windows.Forms.TextBox();
		this.textBox8 = new System.Windows.Forms.TextBox();
		this.textBox9 = new System.Windows.Forms.TextBox();
		this.btn_Save = new System.Windows.Forms.Button();
		this.btn_close = new System.Windows.Forms.Button();
		this.imgList = new System.Windows.Forms.ImageList(this.components);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nUDQtyOld).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.nUDQtyNew).BeginInit();
		base.SuspendLayout();
		this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.txtTotalQty);
		this.splitContainer1.Panel1.Controls.Add(this.label5);
		this.splitContainer1.Panel1.Controls.Add(this.nUDQtyOld);
		this.splitContainer1.Panel1.Controls.Add(this.SheetID);
		this.splitContainer1.Panel1.Controls.Add(this.txtSheetID);
		this.splitContainer1.Panel1.Controls.Add(this.label1);
		this.splitContainer1.Panel1.Controls.Add(this.txtPartNo);
		this.splitContainer1.Panel1.Controls.Add(this.nUDQtyNew);
		this.splitContainer1.Panel1.Controls.Add(this.cboWorkOrderNew);
		this.splitContainer1.Panel1.Controls.Add(this.label3);
		this.splitContainer1.Panel1.Controls.Add(this.label2);
		this.splitContainer1.Panel1.Controls.Add(this.txtWorkOrderOld);
		this.splitContainer1.Panel1.Controls.Add(this.textBox5);
		this.splitContainer1.Panel1.Controls.Add(this.textBox6);
		this.splitContainer1.Panel1.Controls.Add(this.textBox7);
		this.splitContainer1.Panel1.Controls.Add(this.textBox8);
		this.splitContainer1.Panel1.Controls.Add(this.textBox9);
		this.splitContainer1.Panel2.Controls.Add(this.btn_Save);
		this.splitContainer1.Panel2.Controls.Add(this.btn_close);
		this.splitContainer1.Size = new System.Drawing.Size(648, 233);
		this.splitContainer1.SplitterDistance = 167;
		this.splitContainer1.SplitterWidth = 1;
		this.splitContainer1.TabIndex = 0;
		this.txtTotalQty.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtTotalQty.Font = new System.Drawing.Font("Arial", 9f);
		this.txtTotalQty.Location = new System.Drawing.Point(492, 98);
		this.txtTotalQty.Name = "txtTotalQty";
		this.txtTotalQty.ReadOnly = true;
		this.txtTotalQty.Size = new System.Drawing.Size(106, 21);
		this.txtTotalQty.TabIndex = 90;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(427, 101);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 16);
		this.label5.TabIndex = 89;
		this.label5.Text = "Total Qty";
		this.nUDQtyOld.Font = new System.Drawing.Font("Arial", 9f);
		this.nUDQtyOld.Location = new System.Drawing.Point(492, 49);
		this.nUDQtyOld.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		this.nUDQtyOld.Name = "nUDQtyOld";
		this.nUDQtyOld.Size = new System.Drawing.Size(106, 21);
		this.nUDQtyOld.TabIndex = 87;
		this.nUDQtyOld.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.SheetID.AutoSize = true;
		this.SheetID.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Bold);
		this.SheetID.Location = new System.Drawing.Point(72, 29);
		this.SheetID.Name = "SheetID";
		this.SheetID.Size = new System.Drawing.Size(51, 15);
		this.SheetID.TabIndex = 84;
		this.SheetID.Text = "SheetID";
		this.txtSheetID.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtSheetID.Font = new System.Drawing.Font("Arial", 9f);
		this.txtSheetID.Location = new System.Drawing.Point(32, 49);
		this.txtSheetID.Name = "txtSheetID";
		this.txtSheetID.ReadOnly = true;
		this.txtSheetID.Size = new System.Drawing.Size(131, 21);
		this.txtSheetID.TabIndex = 83;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(215, 29);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(51, 16);
		this.label1.TabIndex = 82;
		this.label1.Text = "PartNo";
		this.txtPartNo.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtPartNo.Font = new System.Drawing.Font("Arial", 9f);
		this.txtPartNo.Location = new System.Drawing.Point(165, 49);
		this.txtPartNo.Name = "txtPartNo";
		this.txtPartNo.ReadOnly = true;
		this.txtPartNo.Size = new System.Drawing.Size(150, 21);
		this.txtPartNo.TabIndex = 81;
		this.nUDQtyNew.Font = new System.Drawing.Font("Arial", 9f);
		this.nUDQtyNew.Location = new System.Drawing.Point(492, 72);
		this.nUDQtyNew.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		this.nUDQtyNew.Name = "nUDQtyNew";
		this.nUDQtyNew.Size = new System.Drawing.Size(106, 21);
		this.nUDQtyNew.TabIndex = 80;
		this.nUDQtyNew.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.cboWorkOrderNew.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboWorkOrderNew.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboWorkOrderNew.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboWorkOrderNew.Font = new System.Drawing.Font("Arial", 9f);
		this.cboWorkOrderNew.FormattingEnabled = true;
		this.cboWorkOrderNew.Location = new System.Drawing.Point(317, 72);
		this.cboWorkOrderNew.Name = "cboWorkOrderNew";
		this.cboWorkOrderNew.Size = new System.Drawing.Size(173, 23);
		this.cboWorkOrderNew.TabIndex = 79;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(531, 29);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(29, 16);
		this.label3.TabIndex = 76;
		this.label3.Text = "Qty";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(368, 29);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 16);
		this.label2.TabIndex = 74;
		this.label2.Text = "Work Order";
		this.txtWorkOrderOld.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtWorkOrderOld.Font = new System.Drawing.Font("Arial", 9f);
		this.txtWorkOrderOld.Location = new System.Drawing.Point(317, 49);
		this.txtWorkOrderOld.Name = "txtWorkOrderOld";
		this.txtWorkOrderOld.ReadOnly = true;
		this.txtWorkOrderOld.Size = new System.Drawing.Size(173, 21);
		this.txtWorkOrderOld.TabIndex = 73;
		this.textBox5.BackColor = System.Drawing.SystemColors.Menu;
		this.textBox5.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox5.Location = new System.Drawing.Point(32, 26);
		this.textBox5.Name = "textBox5";
		this.textBox5.ReadOnly = true;
		this.textBox5.Size = new System.Drawing.Size(131, 22);
		this.textBox5.TabIndex = 91;
		this.textBox6.BackColor = System.Drawing.SystemColors.Menu;
		this.textBox6.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox6.Location = new System.Drawing.Point(165, 26);
		this.textBox6.Name = "textBox6";
		this.textBox6.ReadOnly = true;
		this.textBox6.Size = new System.Drawing.Size(150, 21);
		this.textBox6.TabIndex = 92;
		this.textBox7.BackColor = System.Drawing.SystemColors.Menu;
		this.textBox7.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox7.Location = new System.Drawing.Point(317, 26);
		this.textBox7.Name = "textBox7";
		this.textBox7.ReadOnly = true;
		this.textBox7.Size = new System.Drawing.Size(173, 21);
		this.textBox7.TabIndex = 93;
		this.textBox8.BackColor = System.Drawing.SystemColors.Menu;
		this.textBox8.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox8.Location = new System.Drawing.Point(492, 26);
		this.textBox8.Name = "textBox8";
		this.textBox8.ReadOnly = true;
		this.textBox8.Size = new System.Drawing.Size(106, 21);
		this.textBox8.TabIndex = 94;
		this.textBox9.BackColor = System.Drawing.SystemColors.Menu;
		this.textBox9.Font = new System.Drawing.Font("Arial", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox9.Location = new System.Drawing.Point(317, 98);
		this.textBox9.Name = "textBox9";
		this.textBox9.ReadOnly = true;
		this.textBox9.Size = new System.Drawing.Size(173, 21);
		this.textBox9.TabIndex = 95;
		this.btn_Save.BackColor = System.Drawing.Color.Transparent;
		this.btn_Save.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Save.BackgroundImage");
		this.btn_Save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Save.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btn_Save.ForeColor = System.Drawing.Color.Black;
		this.btn_Save.Image = (System.Drawing.Image)resources.GetObject("btn_Save.Image");
		this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Save.Location = new System.Drawing.Point(189, 17);
		this.btn_Save.Name = "btn_Save";
		this.btn_Save.Size = new System.Drawing.Size(126, 30);
		this.btn_Save.TabIndex = 1;
		this.btn_Save.Text = "  Separate";
		this.btn_Save.UseVisualStyleBackColor = false;
		this.btn_Save.Click += new System.EventHandler(btn_Save_Click);
		this.btn_close.BackColor = System.Drawing.Color.Transparent;
		this.btn_close.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close.BackgroundImage");
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_close.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btn_close.ForeColor = System.Drawing.Color.Black;
		this.btn_close.Image = (System.Drawing.Image)resources.GetObject("btn_close.Image");
		this.btn_close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_close.Location = new System.Drawing.Point(321, 17);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(126, 30);
		this.btn_close.TabIndex = 10;
		this.btn_close.Text = "Close";
		this.btn_close.UseVisualStyleBackColor = false;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		this.imgList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgList.ImageStream");
		this.imgList.TransparentColor = System.Drawing.Color.Transparent;
		this.imgList.Images.SetKeyName(0, "editclear.png");
		this.imgList.Images.SetKeyName(1, "NewDocumentC.png");
		this.imgList.Images.SetKeyName(2, "new-document.png");
		this.imgList.Images.SetKeyName(3, "newN.png");
		base.AcceptButton = this.btn_close;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(648, 233);
		base.Controls.Add(this.splitContainer1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmOnLinePrintSplit_P";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Separate Sheet";
		base.Load += new System.EventHandler(frmLocator_P_Load);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel1.PerformLayout();
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.nUDQtyOld).EndInit();
		((System.ComponentModel.ISupportInitialize)this.nUDQtyNew).EndInit();
		base.ResumeLayout(false);
	}
}
