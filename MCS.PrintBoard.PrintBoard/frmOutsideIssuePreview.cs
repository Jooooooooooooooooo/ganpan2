using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using GrapeCity.ActiveReports.Viewer.Win;
using GrapeCity.Viewer.Common;
using GrapeCity.Viewer.Common.Model;
using MCS.Common;

namespace MCS.PrintBoard.PrintBoard;

public class frmOutsideIssuePreview : frmBase
{
	public DataTable dtGroupBy = new DataTable();

	private IContainer components = null;

	private Viewer arvMain;

	private SplitContainer splitContainer1;

	private TreeView tvLocatorGroup;

	private System.Windows.Forms.Button btn_close;

	public frmOutsideIssuePreview()
	{
		InitializeComponent();
	}

	private void arvMain_Load(object sender, EventArgs e)
	{
		try
		{
			OutsideIssueList rptA4 = new OutsideIssueList();
			OnLineBoard rptA5 = new OnLineBoard();
			rptA4.DataSource = dtGroupBy;
			rptA4.DataMember = dtGroupBy.TableName;
			arvMain.LoadDocument(rptA4);
			Cursor = Cursors.Arrow;
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void btn_close_Click(object sender, EventArgs e)
	{
		Close();
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
		this.arvMain = new GrapeCity.ActiveReports.Viewer.Win.Viewer();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.tvLocatorGroup = new System.Windows.Forms.TreeView();
		this.btn_close = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		base.SuspendLayout();
		this.arvMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.arvMain.CurrentPage = 0;
		this.arvMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.arvMain.Location = new System.Drawing.Point(0, 0);
		this.arvMain.Name = "arvMain";
		this.arvMain.PreviewPages = 0;
		this.arvMain.PrintingSettings = GrapeCity.Viewer.Common.PrintingSettings.ShowPrintDialog | GrapeCity.Viewer.Common.PrintingSettings.ShowPrintProgressDialog | GrapeCity.Viewer.Common.PrintingSettings.UsePrintingThread | GrapeCity.Viewer.Common.PrintingSettings.UseStandardDialog;
		this.arvMain.RepositionPage = true;
		this.arvMain.Sidebar.ParametersPanel.ContextMenu = null;
		this.arvMain.Sidebar.ParametersPanel.Text = "Parameters";
		this.arvMain.Sidebar.ParametersPanel.Width = 200;
		this.arvMain.Sidebar.SearchPanel.ContextMenu = null;
		this.arvMain.Sidebar.SearchPanel.Text = "Search results";
		this.arvMain.Sidebar.SearchPanel.Width = 200;
		this.arvMain.Sidebar.ThumbnailsPanel.ContextMenu = null;
		this.arvMain.Sidebar.ThumbnailsPanel.Text = "Page thumbnails";
		this.arvMain.Sidebar.ThumbnailsPanel.Width = 200;
		this.arvMain.Sidebar.ThumbnailsPanel.Zoom = 0.1;
		this.arvMain.Sidebar.TocPanel.ContextMenu = null;
		this.arvMain.Sidebar.TocPanel.Expanded = true;
		this.arvMain.Sidebar.TocPanel.Text = "Document map";
		this.arvMain.Sidebar.TocPanel.Width = 200;
		this.arvMain.Sidebar.Visible = true;
		this.arvMain.Sidebar.Width = 200;
		this.arvMain.Size = new System.Drawing.Size(1196, 679);
		this.arvMain.TabIndex = 74;
		this.arvMain.ViewType = GrapeCity.Viewer.Common.Model.ViewType.Continuous;
		this.arvMain.Load += new System.EventHandler(arvMain_Load);
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.arvMain);
		this.splitContainer1.Panel1.Controls.Add(this.tvLocatorGroup);
		this.splitContainer1.Panel2.Controls.Add(this.btn_close);
		this.splitContainer1.Size = new System.Drawing.Size(1196, 725);
		this.splitContainer1.SplitterDistance = 679;
		this.splitContainer1.SplitterWidth = 5;
		this.splitContainer1.TabIndex = 1;
		this.tvLocatorGroup.BackColor = System.Drawing.Color.White;
		this.tvLocatorGroup.CheckBoxes = true;
		this.tvLocatorGroup.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tvLocatorGroup.Location = new System.Drawing.Point(0, 0);
		this.tvLocatorGroup.Name = "tvLocatorGroup";
		this.tvLocatorGroup.Size = new System.Drawing.Size(1196, 679);
		this.tvLocatorGroup.TabIndex = 0;
		this.btn_close.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.btn_close.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_close.ForeColor = System.Drawing.Color.White;
		this.btn_close.Location = new System.Drawing.Point(539, 1);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(136, 30);
		this.btn_close.TabIndex = 0;
		this.btn_close.Text = "Close";
		this.btn_close.UseVisualStyleBackColor = false;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1196, 725);
		base.Controls.Add(this.splitContainer1);
		base.Name = "frmOutsideIssuePreview";
		this.Text = "frmOutsideIssuePreview";
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
