using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;

namespace MCS.PrintBoard.PrintBoard;

public class frmOnLinePrintEdit_P : frmBase
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

	private string _GROUP_TXN_ID;

	private string _CarrierID;

	public DataTable dtTemp = new DataTable();

	private IContainer components = null;

	private SplitContainer splitContainer1;

	private ImageList imgList;

	private System.Windows.Forms.Button btn_Save;

	private Label label3;

	private Label label2;

	private TextBox txtWorkOrder;

	private Label label4;

	private TextBox txtSheetID;

	private Label label1;

	private TextBox txtPartNo;

	private NumericUpDown nUDQty;

	private System.Windows.Forms.Button btn_close;

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

	public string sCarrierID
	{
		get
		{
			return _CarrierID;
		}
		set
		{
			_CarrierID = value;
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

	public frmOnLinePrintEdit_P()
	{
		InitializeComponent();
	}

	private void frmLocator_P_Load(object sender, EventArgs e)
	{
		decimal dQty = default(decimal);
		decimal.TryParse(sWoQty, out dQty);
		try
		{
			txtPartNo.Text = sPartNo;
			txtSheetID.Text = sSheetID;
			txtWorkOrder.Text = sWorkOrder;
			nUDQty.Value = dQty;
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
		if (MessageBox.Show("Do you want to edit Qty ?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
		{
			string sSheedID = string.Empty;
			bool bReturn = SetChangeQty(sSheetID, sLocator, sPartNo, sTotalQty, sPlanSeq, sWorkOrder, sWoQty);
			if (bReturn)
			{
				MessageBox.Show("Saving is complete.");
				CloseForm(bReturn);
			}
		}
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

	private bool SetDeleverySheet(string pSheetID, string pLocator, string pPartNo, string pTotalQty, string pPlanSeq, string pWorkOrder, string pWoQty)
	{
		bool bReturn = false;
		string sReturnMsg = string.Empty;
		string sReturnCode = string.Empty;
		string sDIV = "UPD_QTY";
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
			sDIV = "UPD_QTY";
			sSHEET_ID = pSheetID;
			sSHEET_TYPE = "MCS_ONLINE";
			sMADE_LOCATOR = null;
			sDELIVERY_TO = pLocator;
			sITEM_CODE = pPartNo;
			sWO_NAME = null;
			iTOTAL_QTY = int.Parse(nUDQty.Value.ToString());
			iSEQ = int.Parse(pPlanSeq);
			sTO_WO_NAME = pWorkOrder;
			sTO_ITEM_CODE = null;
			iTO_WO_QTY = int.Parse(nUDQty.Value.ToString());
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

	private bool SetChangeQty(string pSheetID, string pLocator, string pPartNo, string pTotalQty, string pPlanSeq, string pWorkOrder, string pWoQty)
	{
		bool bReturn = false;
		string sReturnMsg = string.Empty;
		string sReturnCode = string.Empty;
		string sI_START_END = "MANUAL";
		string sI_CARRIER_ID = sCarrierID;
		string sI_MAKE_REMOVE = "UPDATE";
		string sI_QTY = "";
		string sI_USER_ID = "UI_CS";
		string sI_TXN_FROM = "UI";
		string sCHANGE_QTY = nUDQty.Value.ToString();
		string sDATASTATE = "UPD";
		int iSEQ = int.Parse(sPlanSeq);
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_ORG_ID", (sOrgID == "") ? null : sOrgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_START_END", (sI_START_END == "") ? null : sI_START_END);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_CARRIER_ID", sI_CARRIER_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_SHEET_ID", (pSheetID == "") ? null : pSheetID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_MAKE_REMOVE", (sI_MAKE_REMOVE == "") ? null : sI_MAKE_REMOVE);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_LOCATOR", pLocator);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_ITEM_CODE", pPartNo);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_QTY", sI_QTY);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_USER_ID", sI_USER_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TXN_FROM", sI_TXN_FROM);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_WO_NAME", null);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_COMMENTS", null);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "CHANGE_QTY", sCHANGE_QTY);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "DATASTATE", (sDATASTATE == "") ? null : sDATASTATE);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TO_WO_SEQ", iSEQ);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_SHEET_TYPE", "MCS_ONLINE");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "STATUS", "Y");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "PRINT_YN", "Y");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_MAP_TYPE", "PROD_AFTER_MAP");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TXN_UNIT", "SHEET");
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_TXN_ID", (pSheetID == "") ? null : pSheetID);
			DataSet dsResult = bizServer.ExecBizRule("GMCS_SET_CHANGE_QTY", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				sReturnMsg = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_MSG"].ToString();
				sReturnCode = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_CODE"].ToString();
				if (sReturnCode.ToUpper() == "ERROR")
				{
					CommonBiz CallBiz2 = new CommonBiz();
					string sMsg2 = CallBiz2.callGmcsSetError(sReturnMsg);
					MessageBox.Show(sMsg2);
				}
			}
			if (sReturnCode == "OK")
			{
				bReturn = true;
			}
		}
		catch (Exception)
		{
			string[] sSplit = sReturnMsg.Split('^');
			string sTemp = string.Empty;
			if (sSplit.Length == 1)
			{
				sTemp = sSplit[0];
			}
			else if (sSplit.Length > 1)
			{
				sTemp = sSplit[1];
			}
			CommonBiz CallBiz = new CommonBiz();
			string sMsg = CallBiz.callGmcsSetError(sTemp);
			MessageBox.Show(sMsg);
			bReturn = false;
		}
		return bReturn;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmOnLinePrintEdit_P));
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.nUDQty = new System.Windows.Forms.NumericUpDown();
		this.label4 = new System.Windows.Forms.Label();
		this.txtSheetID = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtPartNo = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtWorkOrder = new System.Windows.Forms.TextBox();
		this.btn_Save = new System.Windows.Forms.Button();
		this.btn_close = new System.Windows.Forms.Button();
		this.imgList = new System.Windows.Forms.ImageList(this.components);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.nUDQty).BeginInit();
		base.SuspendLayout();
		this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.nUDQty);
		this.splitContainer1.Panel1.Controls.Add(this.label4);
		this.splitContainer1.Panel1.Controls.Add(this.txtSheetID);
		this.splitContainer1.Panel1.Controls.Add(this.label1);
		this.splitContainer1.Panel1.Controls.Add(this.label3);
		this.splitContainer1.Panel1.Controls.Add(this.txtPartNo);
		this.splitContainer1.Panel1.Controls.Add(this.label2);
		this.splitContainer1.Panel1.Controls.Add(this.txtWorkOrder);
		this.splitContainer1.Panel2.Controls.Add(this.btn_Save);
		this.splitContainer1.Panel2.Controls.Add(this.btn_close);
		this.splitContainer1.Size = new System.Drawing.Size(314, 210);
		this.splitContainer1.SplitterDistance = 150;
		this.splitContainer1.SplitterWidth = 1;
		this.splitContainer1.TabIndex = 0;
		this.nUDQty.Font = new System.Drawing.Font("Arial", 10f);
		this.nUDQty.Location = new System.Drawing.Point(111, 107);
		this.nUDQty.Maximum = new decimal(new int[4] { 1000, 0, 0, 0 });
		this.nUDQty.Name = "nUDQty";
		this.nUDQty.Size = new System.Drawing.Size(173, 23);
		this.nUDQty.TabIndex = 0;
		this.nUDQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(41, 27);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 16);
		this.label4.TabIndex = 86;
		this.label4.Text = "SheetID";
		this.txtSheetID.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtSheetID.Font = new System.Drawing.Font("Arial", 10f);
		this.txtSheetID.Location = new System.Drawing.Point(112, 23);
		this.txtSheetID.Name = "txtSheetID";
		this.txtSheetID.ReadOnly = true;
		this.txtSheetID.Size = new System.Drawing.Size(172, 23);
		this.txtSheetID.TabIndex = 85;
		this.txtSheetID.TabStop = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(49, 54);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(55, 16);
		this.label1.TabIndex = 84;
		this.label1.Text = "PartNo";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(73, 109);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(31, 16);
		this.label3.TabIndex = 76;
		this.label3.Text = "Qty";
		this.txtPartNo.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtPartNo.Font = new System.Drawing.Font("Arial", 10f);
		this.txtPartNo.Location = new System.Drawing.Point(112, 51);
		this.txtPartNo.Name = "txtPartNo";
		this.txtPartNo.ReadOnly = true;
		this.txtPartNo.Size = new System.Drawing.Size(172, 23);
		this.txtPartNo.TabIndex = 83;
		this.txtPartNo.TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(15, 82);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(89, 16);
		this.label2.TabIndex = 74;
		this.label2.Text = "Work Order";
		this.txtWorkOrder.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtWorkOrder.Font = new System.Drawing.Font("Arial", 10f);
		this.txtWorkOrder.Location = new System.Drawing.Point(112, 79);
		this.txtWorkOrder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.txtWorkOrder.Name = "txtWorkOrder";
		this.txtWorkOrder.ReadOnly = true;
		this.txtWorkOrder.Size = new System.Drawing.Size(172, 23);
		this.txtWorkOrder.TabIndex = 73;
		this.txtWorkOrder.TabStop = false;
		this.btn_Save.BackColor = System.Drawing.Color.Transparent;
		this.btn_Save.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Save.BackgroundImage");
		this.btn_Save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Save.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_Save.ForeColor = System.Drawing.Color.Black;
		this.btn_Save.Image = (System.Drawing.Image)resources.GetObject("btn_Save.Image");
		this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Save.Location = new System.Drawing.Point(56, 14);
		this.btn_Save.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Save.Name = "btn_Save";
		this.btn_Save.Size = new System.Drawing.Size(100, 30);
		this.btn_Save.TabIndex = 1;
		this.btn_Save.Text = "  Save";
		this.btn_Save.UseVisualStyleBackColor = false;
		this.btn_Save.Click += new System.EventHandler(btn_Save_Click);
		this.btn_close.BackColor = System.Drawing.Color.Transparent;
		this.btn_close.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_close.BackgroundImage");
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_close.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_close.ForeColor = System.Drawing.Color.Black;
		this.btn_close.Image = (System.Drawing.Image)resources.GetObject("btn_close.Image");
		this.btn_close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_close.Location = new System.Drawing.Point(170, 14);
		this.btn_close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(100, 30);
		this.btn_close.TabIndex = 10;
		this.btn_close.Text = "  Close";
		this.btn_close.UseVisualStyleBackColor = false;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		this.imgList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgList.ImageStream");
		this.imgList.TransparentColor = System.Drawing.Color.Transparent;
		this.imgList.Images.SetKeyName(0, "editclear.png");
		this.imgList.Images.SetKeyName(1, "NewDocumentC.png");
		this.imgList.Images.SetKeyName(2, "new-document.png");
		this.imgList.Images.SetKeyName(3, "newN.png");
		base.AcceptButton = this.btn_close;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 16f);
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(314, 210);
		base.Controls.Add(this.splitContainer1);
		this.Font = new System.Drawing.Font("Arial", 10f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmOnLinePrintEdit_P";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Edit Sheet";
		base.Load += new System.EventHandler(frmLocator_P_Load);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel1.PerformLayout();
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.nUDQty).EndInit();
		base.ResumeLayout(false);
	}
}
