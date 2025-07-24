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

namespace MCS.PrintBoard.PrintBoard;

public class frmDecantPlan : Form
{
	private IContainer components = null;

	private Panel panel3;

	private MCS.Common.FpSpread fpPlanInfo;

	private PanelOnly panelOnly1;

	private Label label8;

	private PanelOnly panelOnly2;

	private System.Windows.Forms.Button btnClose;

	public string _orgID { get; set; }

	public string _toLineCode { get; set; }

	public frmDecantPlan()
	{
		InitializeComponent();
	}

	private void frmDecantPlan_Load(object sender, EventArgs e)
	{
		InitControls();
		DataBindPlanInfo();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void InitControls()
	{
		try
		{
			setSheetColumnPlanInfo();
		}
		catch (Exception)
		{
		}
	}

	private void setSheetColumnPlanInfo()
	{
		try
		{
			MCS.Common.SheetView svPlanInfo = new MCS.Common.SheetView(fpPlanInfo, "Search", OperationMode.ReadOnly, bRowHeaderVisible: true, "BackColor White");
			svPlanInfo.AddColumnDateTime("Plan Date", "PLAN_YYYYMMDD", 80, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern);
			svPlanInfo.AddColumnText("Line", "LINE_CODE", 65, CellHorizontalAlignment.Left, bLocked: false, bVisible: true, 100);
			svPlanInfo.AddColumnText("Model", "MODEL_SUFFIX", 195, CellHorizontalAlignment.Left, bLocked: false, bVisible: true, 500);
			svPlanInfo.AddColumnText("Workorder", "WO_NAME", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 300);
			svPlanInfo.AddColumnText("Total Qty", "TOTAL_PLAN_QTY", 1, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 1);
			svPlanInfo.AddColumnText("Daily Qty", "DAILY_PLAN_QTY", 1, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 1);
			svPlanInfo.AddColumnText("Daily/Total", "DAILY_TOTA_QTY", 90, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 300);
			svPlanInfo.AddColumnText("Result Qty", "RESULT_QTY", 90, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPlanInfo.AddColumnText("Plan Seq", "PLAN_SEQ", 85, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPlanInfo.RowHeader.Visible = true;
			svPlanInfo.Rows.Default.Height = 45f;
			svPlanInfo.ColumnHeader.Rows[0].Height = 50f;
			svPlanInfo.ColumnHeader.Rows[0].Font = new Font(new FontFamily("Arial"), 11f, FontStyle.Bold);
			svPlanInfo.Rows.Default.Font = new Font(new FontFamily("Arial"), 11f);
			fpPlanInfo.AutoSizeColumnWidth = false;
		}
		catch (Exception)
		{
		}
	}

	private void DataBindPlanInfo()
	{
		DataSet dsResult = null;
		try
		{
			dsResult = GetPlanInfo(_orgID, _toLineCode);
			fpPlanInfo.DataSource = dsResult.Tables["OUT_DATA"];
			fpPlanInfo.Refresh();
		}
		catch (Exception)
		{
		}
	}

	private DataSet GetPlanInfo(string orgID, string toLineCode)
	{
		DataSet ds = null;
		DataSet dsResult = null;
		try
		{
			ds = new DataSet();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", orgID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "LINE_CODE", toLineCode);
			dsResult = new BizService().ExecBizRule("GMCS_GET_DECANT_SUPPLY_PLAN", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception)
		{
		}
		return dsResult;
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
		FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer1 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panelOnly2 = new MCS.Common.PanelOnly();
		this.btnClose = new System.Windows.Forms.Button();
		this.panelOnly1 = new MCS.Common.PanelOnly();
		this.label8 = new System.Windows.Forms.Label();
		this.fpPlanInfo = new MCS.Common.FpSpread();
		this.panel3.SuspendLayout();
		this.panelOnly2.SuspendLayout();
		this.panelOnly1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpPlanInfo).BeginInit();
		base.SuspendLayout();
		this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(91, 91, 91);
		this.panel3.Controls.Add(this.panelOnly2);
		this.panel3.Controls.Add(this.panelOnly1);
		this.panel3.Controls.Add(this.fpPlanInfo);
		this.panel3.Font = new System.Drawing.Font("Arial", 9f);
		this.panel3.Location = new System.Drawing.Point(2, 2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(899, 737);
		this.panel3.TabIndex = 115;
		this.panelOnly2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelOnly2.BackColor = System.Drawing.SystemColors.Control;
		this.panelOnly2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.panelOnly2.Controls.Add(this.btnClose);
		this.panelOnly2.Location = new System.Drawing.Point(2, 645);
		this.panelOnly2.Name = "panelOnly2";
		this.panelOnly2.Padding = new System.Windows.Forms.Padding(8);
		this.panelOnly2.Size = new System.Drawing.Size(895, 91);
		this.panelOnly2.TabIndex = 107;
		this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
		this.btnClose.BackColor = System.Drawing.Color.Black;
		this.btnClose.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.btnClose.ForeColor = System.Drawing.Color.White;
		this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnClose.Location = new System.Drawing.Point(762, 4);
		this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(129, 84);
		this.btnClose.TabIndex = 121;
		this.btnClose.Text = "Close";
		this.btnClose.UseVisualStyleBackColor = false;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.panelOnly1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelOnly1.BackColor = System.Drawing.SystemColors.Control;
		this.panelOnly1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		this.panelOnly1.Controls.Add(this.label8);
		this.panelOnly1.Location = new System.Drawing.Point(2, 2);
		this.panelOnly1.Name = "panelOnly1";
		this.panelOnly1.Padding = new System.Windows.Forms.Padding(8);
		this.panelOnly1.Size = new System.Drawing.Size(895, 52);
		this.panelOnly1.TabIndex = 106;
		this.label8.BackColor = System.Drawing.SystemColors.Control;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Black;
		this.label8.Location = new System.Drawing.Point(6, 7);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(173, 38);
		this.label8.TabIndex = 79;
		this.label8.Text = "Decant Plan";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.fpPlanInfo.AccessibleDescription = "";
		this.fpPlanInfo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.fpPlanInfo.AutoSizeColumnWidth = true;
		this.fpPlanInfo.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpPlanInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpPlanInfo.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPlanInfo.EnableSort = false;
		this.fpPlanInfo.FocusRenderer = defaultFocusIndicatorRenderer1;
		this.fpPlanInfo.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpPlanInfo.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPlanInfo.HorizontalScrollBar.Name = "";
		this.fpPlanInfo.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
		this.fpPlanInfo.HorizontalScrollBar.TabIndex = 0;
		this.fpPlanInfo.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPlanInfo.Location = new System.Drawing.Point(2, 56);
		this.fpPlanInfo.Name = "fpPlanInfo";
		this.fpPlanInfo.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPlanInfo.Size = new System.Drawing.Size(895, 586);
		this.fpPlanInfo.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpPlanInfo.TabIndex = 105;
		this.fpPlanInfo.TextTipDelay = 1000;
		this.fpPlanInfo.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpPlanInfo.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPlanInfo.VerticalScrollBar.Name = "";
		this.fpPlanInfo.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
		this.fpPlanInfo.VerticalScrollBar.TabIndex = 0;
		this.fpPlanInfo.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPlanInfo.VisualStyles = FarPoint.Win.VisualStyles.Off;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(904, 742);
		base.Controls.Add(this.panel3);
		base.Name = "frmDecantPlan";
		this.Text = "Prod Status";
		base.Load += new System.EventHandler(frmDecantPlan_Load);
		this.panel3.ResumeLayout(false);
		this.panelOnly2.ResumeLayout(false);
		this.panelOnly1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpPlanInfo).EndInit();
		base.ResumeLayout(false);
	}
}
