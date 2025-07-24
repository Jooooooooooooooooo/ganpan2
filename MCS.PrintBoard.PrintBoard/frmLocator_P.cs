using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win;
using FarPoint.Win.Spread;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;
using MCS.PrintBoard.Properties;

namespace MCS.PrintBoard.PrintBoard;

public class frmLocator_P : frmBase
{
	private Font fBold = new Font("맑은 고딕", 9f, FontStyle.Bold);

	private Font fRegular = new Font("맑은 고딕", 9f, FontStyle.Regular);

	private TreeNode tnTemp = null;

	private string _LocatorGroupCode;

	private string _LocatorGroupName;

	private string _PartNo;

	private string _Locator;

	public DataTable dtTemp = new DataTable();

	private IContainer components = null;

	private SplitContainer splitContainer1;

	private System.Windows.Forms.Button btn_close;

	private ImageList imgList;

	private MCS.Common.FpSpread fpPrintMain;

	private Label label1;

	private TextBox txtLocator;

	private SplitContainer splitContainer2;

	private Label label2;

	private TextBox txtItem;

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

	public frmLocator_P()
	{
		InitializeComponent();
	}

	private void frmLocator_P_Load(object sender, EventArgs e)
	{
		try
		{
			procMakeSheetColumn();
			fpPrintMain.Refresh();
			if (fpPrintMain.ActiveSheet.Rows.Count == 1)
			{
				sLocator = fpPrintMain.ActiveSheet.GetText(0, "LOCATOR");
				string sLocatorDesc = fpPrintMain.ActiveSheet.GetText(0, "LOC_DESC");
				txtLocator.Text = "[" + sLocator + "]" + sLocatorDesc;
			}
			txtItem.Text = sPartNo;
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void btn_close_Click(object sender, EventArgs e)
	{
		if (string.IsNullOrEmpty(sLocator))
		{
			MessageBox.Show("선택 된  Locator가 없습니다.");
			return;
		}
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void procMakeSheetColumn()
	{
		try
		{
			MCS.Common.SheetView svPrintSearch = new MCS.Common.SheetView(fpPrintMain, "Search", OperationMode.ReadOnly, bRowHeaderVisible: true);
			svPrintSearch.AddColumnText("Code", "LOCATOR", 100, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 500);
			svPrintSearch.AddColumnText("Locator", "LOC_DESC", 250, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 500);
			svPrintSearch.ColumnHeader.Visible = true;
			svPrintSearch.RowHeader.Visible = true;
			svPrintSearch.Rows.Default.Height = 25f;
			svPrintSearch.ColumnHeader.Rows[0].Height = 25f;
			fpPrintMain.AutoSizeColumnWidth = false;
			fpPrintMain.DataSource = dtTemp;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
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

	private void fpPrintMain_CellClick(object sender, CellClickEventArgs e)
	{
		if (e.Column == fpPrintMain.ActiveSheet.GetColumnIndex("LOCATOR") && e.Row >= 0 && !e.ColumnHeader)
		{
			sLocator = fpPrintMain.ActiveSheet.GetText(e.Row, "LOCATOR");
			string sLocatorDesc = fpPrintMain.ActiveSheet.GetText(e.Row, "LOC_DESC");
			txtLocator.Text = "[" + sLocator + "]" + sLocatorDesc;
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
		FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmLocator_P));
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.splitContainer2 = new System.Windows.Forms.SplitContainer();
		this.label2 = new System.Windows.Forms.Label();
		this.txtItem = new System.Windows.Forms.TextBox();
		this.fpPrintMain = new MCS.Common.FpSpread();
		this.label1 = new System.Windows.Forms.Label();
		this.txtLocator = new System.Windows.Forms.TextBox();
		this.btn_close = new System.Windows.Forms.Button();
		this.imgList = new System.Windows.Forms.ImageList(this.components);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).BeginInit();
		this.splitContainer2.Panel1.SuspendLayout();
		this.splitContainer2.Panel2.SuspendLayout();
		this.splitContainer2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpPrintMain).BeginInit();
		base.SuspendLayout();
		this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
		this.splitContainer1.Panel2.Controls.Add(this.label1);
		this.splitContainer1.Panel2.Controls.Add(this.txtLocator);
		this.splitContainer1.Panel2.Controls.Add(this.btn_close);
		this.splitContainer1.Size = new System.Drawing.Size(403, 225);
		this.splitContainer1.SplitterDistance = 144;
		this.splitContainer1.SplitterWidth = 1;
		this.splitContainer1.TabIndex = 0;
		this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer2.Location = new System.Drawing.Point(0, 0);
		this.splitContainer2.Name = "splitContainer2";
		this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer2.Panel1.Controls.Add(this.label2);
		this.splitContainer2.Panel1.Controls.Add(this.txtItem);
		this.splitContainer2.Panel2.Controls.Add(this.fpPrintMain);
		this.splitContainer2.Size = new System.Drawing.Size(401, 142);
		this.splitContainer2.SplitterDistance = 25;
		this.splitContainer2.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(20, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 16);
		this.label2.TabIndex = 4;
		this.label2.Text = "Item  :";
		this.txtItem.BackColor = System.Drawing.Color.WhiteSmoke;
		this.txtItem.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtItem.Font = new System.Drawing.Font("Arial", 9.75f);
		this.txtItem.Location = new System.Drawing.Point(71, 5);
		this.txtItem.Name = "txtItem";
		this.txtItem.ReadOnly = true;
		this.txtItem.Size = new System.Drawing.Size(271, 15);
		this.txtItem.TabIndex = 3;
		this.txtItem.TabStop = false;
		this.txtItem.Text = "3550DDDDF";
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
		this.fpPrintMain.Size = new System.Drawing.Size(401, 113);
		this.fpPrintMain.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpPrintMain.TabIndex = 72;
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
		this.label1.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(7, 15);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(56, 16);
		this.label1.TabIndex = 2;
		this.label1.Text = "Locator";
		this.txtLocator.BackColor = System.Drawing.Color.WhiteSmoke;
		this.txtLocator.Font = new System.Drawing.Font("Arial", 9.75f);
		this.txtLocator.Location = new System.Drawing.Point(65, 12);
		this.txtLocator.Name = "txtLocator";
		this.txtLocator.ReadOnly = true;
		this.txtLocator.Size = new System.Drawing.Size(323, 22);
		this.txtLocator.TabIndex = 1;
		this.btn_close.BackColor = System.Drawing.Color.Transparent;
		this.btn_close.BackgroundImage = MCS.PrintBoard.Properties.Resources.sbtnbg;
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_close.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_close.ForeColor = System.Drawing.Color.Black;
		this.btn_close.Image = (System.Drawing.Image)resources.GetObject("btn_close.Image");
		this.btn_close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_close.Location = new System.Drawing.Point(261, 44);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(126, 28);
		this.btn_close.TabIndex = 0;
		this.btn_close.Text = "  Apply";
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
		base.ClientSize = new System.Drawing.Size(403, 225);
		base.Controls.Add(this.splitContainer1);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmLocator_P";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "Locator ";
		base.Load += new System.EventHandler(frmLocator_P_Load);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		this.splitContainer1.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		this.splitContainer2.Panel1.ResumeLayout(false);
		this.splitContainer2.Panel1.PerformLayout();
		this.splitContainer2.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).EndInit();
		this.splitContainer2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpPrintMain).EndInit();
		base.ResumeLayout(false);
	}
}
