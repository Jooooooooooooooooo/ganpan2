using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;

namespace MCS.PrintBoard.PrintBoard;

public class frmOrgSave_P : frmBase
{
	private Font fBold = new Font("맑은 고딕", 9f, FontStyle.Bold);

	private Font fRegular = new Font("맑은 고딕", 9f, FontStyle.Regular);

	private string _Org;

	public DataTable dtTemp = new DataTable();

	private IContainer components = null;

	private SplitContainer splitContainer1;

	private ImageList imgList;

	private System.Windows.Forms.Button btn_Close;

	private Label label4;

	private TextBox txtOrg;

	private System.Windows.Forms.Button btn_Save;

	private Label label1;

	public string sOrg
	{
		get
		{
			return _Org;
		}
		set
		{
			_Org = value;
		}
	}

	public frmOrgSave_P()
	{
		InitializeComponent();
	}

	private void frmOrgSave_P_Load(object sender, EventArgs e)
	{
		try
		{
			txtOrg.Text = sOrg;
			txtOrg.Focus();
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void btn_close_Click(object sender, EventArgs e)
	{
		CloseForm();
	}

	private void btn_Save_Click(object sender, EventArgs e)
	{
		Save();
	}

	private void Save()
	{
		string sOrgID = txtOrg.Text.Trim();
		if (sOrgID.Length != 3)
		{
			MessageBox.Show("OrgName must be 3 characters !!");
		}
		else
		{
			if (MessageBox.Show("Do you want to Save Org ?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
			{
				return;
			}
			string path = Application.StartupPath;
			string filePath = path + "\\\\ORG.txt";
			StreamWriter fs = null;
			try
			{
				using (fs = new StreamWriter(filePath, append: false))
				{
					fs.WriteLine(sOrgID);
					sOrg = sOrgID;
					fs.Close();
				}
			}
			catch (Exception ex)
			{
				fs?.Close();
				throw ex;
			}
			MessageBox.Show("Saving is complete.");
			CloseForm();
		}
	}

	private void CloseForm()
	{
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private bool SetDeleverySheet(string pSheetID, string pLocator, string pPartNo, string pTotalQty, string pPlanSeq, string pWorkOrder, string pWoQty)
	{
		bool bReturn = false;
		string sReturnMsg = string.Empty;
		string sReturnCode = string.Empty;
		string sDIV = "UPD_QTY";
		string sORG_ID = "203";
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
			sDIV = "UPD_QTY";
			sORG_ID = "203";
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
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sORG_ID == "") ? null : sORG_ID);
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

	private void GetLocationGroup()
	{
		try
		{
			DataSet ds = new DataSet();
			string sOrgCode = "203";
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sOrgCode == "") ? null : sOrgCode);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmOrgSave_P));
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtOrg = new System.Windows.Forms.TextBox();
		this.btn_Close = new System.Windows.Forms.Button();
		this.btn_Save = new System.Windows.Forms.Button();
		this.imgList = new System.Windows.Forms.ImageList(this.components);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		base.SuspendLayout();
		this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.label1);
		this.splitContainer1.Panel1.Controls.Add(this.label4);
		this.splitContainer1.Panel1.Controls.Add(this.txtOrg);
		this.splitContainer1.Panel2.Controls.Add(this.btn_Close);
		this.splitContainer1.Panel2.Controls.Add(this.btn_Save);
		this.splitContainer1.Size = new System.Drawing.Size(314, 117);
		this.splitContainer1.SplitterDistance = 62;
		this.splitContainer1.SplitterWidth = 1;
		this.splitContainer1.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Arial", 9.75f);
		this.label1.Location = new System.Drawing.Point(16, 35);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(116, 19);
		this.label1.TabIndex = 87;
		this.label1.Text = "(Org Length 3)";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(27, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(187, 19);
		this.label4.TabIndex = 86;
		this.label4.Text = "Please Enter OrgName";
		this.txtOrg.BackColor = System.Drawing.Color.White;
		this.txtOrg.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
		this.txtOrg.Font = new System.Drawing.Font("Arial", 9f);
		this.txtOrg.Location = new System.Drawing.Point(143, 34);
		this.txtOrg.MaxLength = 3;
		this.txtOrg.Name = "txtOrg";
		this.txtOrg.Size = new System.Drawing.Size(116, 25);
		this.txtOrg.TabIndex = 0;
		this.txtOrg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.btn_Close.BackColor = System.Drawing.Color.Transparent;
		this.btn_Close.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Close.BackgroundImage");
		this.btn_Close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Close.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Close.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btn_Close.ForeColor = System.Drawing.Color.Black;
		this.btn_Close.Image = (System.Drawing.Image)resources.GetObject("btn_Close.Image");
		this.btn_Close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Close.Location = new System.Drawing.Point(162, 14);
		this.btn_Close.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Close.Name = "btn_Close";
		this.btn_Close.Size = new System.Drawing.Size(100, 30);
		this.btn_Close.TabIndex = 1;
		this.btn_Close.Text = "  Close";
		this.btn_Close.UseVisualStyleBackColor = false;
		this.btn_Close.Click += new System.EventHandler(btn_close_Click);
		this.btn_Save.BackColor = System.Drawing.Color.Transparent;
		this.btn_Save.BackgroundImage = (System.Drawing.Image)resources.GetObject("btn_Save.BackgroundImage");
		this.btn_Save.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Save.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btn_Save.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btn_Save.ForeColor = System.Drawing.Color.Black;
		this.btn_Save.Image = (System.Drawing.Image)resources.GetObject("btn_Save.Image");
		this.btn_Save.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Save.Location = new System.Drawing.Point(55, 14);
		this.btn_Save.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Save.Name = "btn_Save";
		this.btn_Save.Size = new System.Drawing.Size(100, 30);
		this.btn_Save.TabIndex = 10;
		this.btn_Save.Text = "  Save";
		this.btn_Save.UseVisualStyleBackColor = false;
		this.btn_Save.Click += new System.EventHandler(btn_Save_Click);
		this.imgList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgList.ImageStream");
		this.imgList.TransparentColor = System.Drawing.Color.Transparent;
		this.imgList.Images.SetKeyName(0, "editclear.png");
		this.imgList.Images.SetKeyName(1, "NewDocumentC.png");
		this.imgList.Images.SetKeyName(2, "new-document.png");
		this.imgList.Images.SetKeyName(3, "newN.png");
		base.AcceptButton = this.btn_Save;
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 17f);
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(314, 117);
		base.Controls.Add(this.splitContainer1);
		this.Font = new System.Drawing.Font("Arial", 9f);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmOrgSave_P";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Org Save";
		base.Load += new System.EventHandler(frmOrgSave_P_Load);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel1.PerformLayout();
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
