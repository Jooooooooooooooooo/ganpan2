using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using MCS.Common;

namespace MCS.PrintBoard.PrintBoard;

public class frmConfig : Form
{
	private List<string> ComPortTypeItem = SerialPort.GetPortNames().ToList();

	private IContainer components = null;

	private MCS.Common.ComboBox cboScanBaudRate;

	private MCS.Common.ComboBox cboScanPort;

	private Label label2;

	private Label label3;

	private Label label4;

	private TextBox txtOrgID;

	private MCS.Common.ComboBox cboScanStopbits;

	private MCS.Common.ComboBox cboScanDatabits;

	private Label label1;

	private Label label5;

	private MCS.Common.ComboBox cboScanHandshake;

	private MCS.Common.ComboBox cboScanParity;

	private Label label6;

	private Label label7;

	private System.Windows.Forms.Button btnSave;

	private System.Windows.Forms.Button btnClose;

	private PanelOnly panelOnly1;

	private Label label8;

	public frmConfig()
	{
		InitializeComponent();
	}

	private void frmConfig_Load(object sender, EventArgs e)
	{
		InitControls();
	}

	private void InitControls()
	{
		try
		{
			ComPortTypeItem.Sort();
			cboScanPort.DataSource = ComPortTypeItem;
			cboScanBaudRate.DataSource = Variables.BuadrateItem;
			cboScanDatabits.DataSource = Variables.DatabitItem;
			cboScanStopbits.DataSource = Enum.GetValues(typeof(StopBits));
			cboScanParity.DataSource = Enum.GetValues(typeof(Parity));
			cboScanHandshake.DataSource = Enum.GetValues(typeof(Handshake));
			txtOrgID.Text = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "ORG_ID", "302550");
			cboScanPort.SelectedItem = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Port", "COM1");
			cboScanBaudRate.SelectedItem = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Baud", "9600");
			cboScanDatabits.SelectedItem = FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Databits", "8");
			cboScanStopbits.SelectedItem = Enum.Parse(typeof(StopBits), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Stopbits", "One"));
			cboScanParity.SelectedItem = Enum.Parse(typeof(Parity), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Parity", "None"));
			cboScanHandshake.SelectedItem = Enum.Parse(typeof(Handshake), FuncXml.Instance.ReadXml(Variables.LoginXmlItemType.scannerNodeName, "Handshake", "None"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		ConfigSave();
	}

	private void btnClose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void ConfigSave()
	{
		try
		{
			FuncXml.Instance.WriteXml(Variables.LoginXmlItemType.scannerNodeName, "ORG_ID", txtOrgID.Text);
			FuncXml.Instance.WriteXml(Variables.LoginXmlItemType.scannerNodeName, "Port", cboScanPort.Text);
			FuncXml.Instance.WriteXml(Variables.LoginXmlItemType.scannerNodeName, "Baud", cboScanBaudRate.Text);
			FuncXml.Instance.WriteXml(Variables.LoginXmlItemType.scannerNodeName, "Databits", cboScanDatabits.Text);
			FuncXml.Instance.WriteXml(Variables.LoginXmlItemType.scannerNodeName, "Stopbits", cboScanStopbits.Text);
			FuncXml.Instance.WriteXml(Variables.LoginXmlItemType.scannerNodeName, "Parity", cboScanParity.Text);
			FuncXml.Instance.WriteXml(Variables.LoginXmlItemType.scannerNodeName, "Handshake", cboScanHandshake.Text);
			InitControls();
			MessageBox.Show("Save Success!", Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmConfig));
		this.cboScanBaudRate = new MCS.Common.ComboBox();
		this.cboScanPort = new MCS.Common.ComboBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtOrgID = new System.Windows.Forms.TextBox();
		this.cboScanStopbits = new MCS.Common.ComboBox();
		this.cboScanDatabits = new MCS.Common.ComboBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cboScanHandshake = new MCS.Common.ComboBox();
		this.cboScanParity = new MCS.Common.ComboBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.btnSave = new System.Windows.Forms.Button();
		this.btnClose = new System.Windows.Forms.Button();
		this.panelOnly1 = new MCS.Common.PanelOnly();
		this.label8 = new System.Windows.Forms.Label();
		this.panelOnly1.SuspendLayout();
		base.SuspendLayout();
		this.cboScanBaudRate.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboScanBaudRate.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboScanBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboScanBaudRate.Font = new System.Drawing.Font("Arial", 20f);
		this.cboScanBaudRate.ForeColor = System.Drawing.Color.Black;
		this.cboScanBaudRate.FormattingEnabled = true;
		this.cboScanBaudRate.Location = new System.Drawing.Point(184, 167);
		this.cboScanBaudRate.Name = "cboScanBaudRate";
		this.cboScanBaudRate.Size = new System.Drawing.Size(346, 40);
		this.cboScanBaudRate.TabIndex = 108;
		this.cboScanPort.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboScanPort.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboScanPort.DropDownHeight = 200;
		this.cboScanPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboScanPort.Font = new System.Drawing.Font("Arial", 20f);
		this.cboScanPort.ForeColor = System.Drawing.Color.Black;
		this.cboScanPort.FormattingEnabled = true;
		this.cboScanPort.IntegralHeight = false;
		this.cboScanPort.ItemHeight = 32;
		this.cboScanPort.Location = new System.Drawing.Point(184, 120);
		this.cboScanPort.MaxDropDownItems = 5;
		this.cboScanPort.Name = "cboScanPort";
		this.cboScanPort.Size = new System.Drawing.Size(346, 40);
		this.cboScanPort.TabIndex = 107;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(21, 171);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(151, 32);
		this.label2.TabIndex = 105;
		this.label2.Text = "Baud Rate";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(102, 124);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 32);
		this.label3.TabIndex = 106;
		this.label3.Text = "Port";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(59, 77);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(113, 32);
		this.label4.TabIndex = 110;
		this.label4.Text = "ORG ID";
		this.txtOrgID.Font = new System.Drawing.Font("Arial", 20f);
		this.txtOrgID.Location = new System.Drawing.Point(184, 75);
		this.txtOrgID.Name = "txtOrgID";
		this.txtOrgID.Size = new System.Drawing.Size(346, 38);
		this.txtOrgID.TabIndex = 109;
		this.txtOrgID.Text = "302550";
		this.txtOrgID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.cboScanStopbits.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboScanStopbits.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboScanStopbits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboScanStopbits.Font = new System.Drawing.Font("Arial", 20f);
		this.cboScanStopbits.ForeColor = System.Drawing.Color.Black;
		this.cboScanStopbits.FormattingEnabled = true;
		this.cboScanStopbits.Location = new System.Drawing.Point(184, 261);
		this.cboScanStopbits.Name = "cboScanStopbits";
		this.cboScanStopbits.Size = new System.Drawing.Size(346, 40);
		this.cboScanStopbits.TabIndex = 114;
		this.cboScanDatabits.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboScanDatabits.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboScanDatabits.DropDownHeight = 200;
		this.cboScanDatabits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboScanDatabits.Font = new System.Drawing.Font("Arial", 20f);
		this.cboScanDatabits.ForeColor = System.Drawing.Color.Black;
		this.cboScanDatabits.FormattingEnabled = true;
		this.cboScanDatabits.IntegralHeight = false;
		this.cboScanDatabits.ItemHeight = 32;
		this.cboScanDatabits.Location = new System.Drawing.Point(184, 214);
		this.cboScanDatabits.MaxDropDownItems = 5;
		this.cboScanDatabits.Name = "cboScanDatabits";
		this.cboScanDatabits.Size = new System.Drawing.Size(346, 40);
		this.cboScanDatabits.TabIndex = 113;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.Black;
		this.label1.Location = new System.Drawing.Point(54, 265);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(118, 32);
		this.label1.TabIndex = 111;
		this.label1.Text = "Stop bit";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(56, 218);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(116, 32);
		this.label5.TabIndex = 112;
		this.label5.Text = "Data bit";
		this.cboScanHandshake.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboScanHandshake.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboScanHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboScanHandshake.Font = new System.Drawing.Font("Arial", 20f);
		this.cboScanHandshake.ForeColor = System.Drawing.Color.Black;
		this.cboScanHandshake.FormattingEnabled = true;
		this.cboScanHandshake.Location = new System.Drawing.Point(184, 355);
		this.cboScanHandshake.Name = "cboScanHandshake";
		this.cboScanHandshake.Size = new System.Drawing.Size(346, 40);
		this.cboScanHandshake.TabIndex = 118;
		this.cboScanParity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.cboScanParity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.cboScanParity.DropDownHeight = 200;
		this.cboScanParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboScanParity.Font = new System.Drawing.Font("Arial", 20f);
		this.cboScanParity.ForeColor = System.Drawing.Color.Black;
		this.cboScanParity.FormattingEnabled = true;
		this.cboScanParity.IntegralHeight = false;
		this.cboScanParity.ItemHeight = 32;
		this.cboScanParity.Location = new System.Drawing.Point(184, 308);
		this.cboScanParity.MaxDropDownItems = 5;
		this.cboScanParity.Name = "cboScanParity";
		this.cboScanParity.Size = new System.Drawing.Size(346, 40);
		this.cboScanParity.TabIndex = 117;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.Black;
		this.label6.Location = new System.Drawing.Point(11, 359);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(161, 32);
		this.label6.TabIndex = 115;
		this.label6.Text = "Handshake";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.Black;
		this.label7.Location = new System.Drawing.Point(81, 312);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 32);
		this.label7.TabIndex = 116;
		this.label7.Text = "Parity";
		this.btnSave.BackColor = System.Drawing.Color.DimGray;
		this.btnSave.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.btnSave.ForeColor = System.Drawing.Color.White;
		this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnSave.Location = new System.Drawing.Point(64, 435);
		this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(160, 84);
		this.btnSave.TabIndex = 119;
		this.btnSave.Text = "SAVE";
		this.btnSave.UseVisualStyleBackColor = false;
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnClose.BackColor = System.Drawing.Color.DimGray;
		this.btnClose.Font = new System.Drawing.Font("Arial", 20f, System.Drawing.FontStyle.Bold);
		this.btnClose.ForeColor = System.Drawing.Color.White;
		this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnClose.Location = new System.Drawing.Point(320, 435);
		this.btnClose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnClose.Name = "btnClose";
		this.btnClose.Size = new System.Drawing.Size(160, 84);
		this.btnClose.TabIndex = 120;
		this.btnClose.Text = "CLOSE";
		this.btnClose.UseVisualStyleBackColor = false;
		this.btnClose.Click += new System.EventHandler(btnClose_Click);
		this.panelOnly1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.panelOnly1.BackColor = System.Drawing.Color.Transparent;
		this.panelOnly1.BackgroundImage = (System.Drawing.Image)resources.GetObject("panelOnly1.BackgroundImage");
		this.panelOnly1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.panelOnly1.Controls.Add(this.label8);
		this.panelOnly1.Location = new System.Drawing.Point(5, 5);
		this.panelOnly1.Name = "panelOnly1";
		this.panelOnly1.Padding = new System.Windows.Forms.Padding(8);
		this.panelOnly1.Size = new System.Drawing.Size(524, 52);
		this.panelOnly1.TabIndex = 121;
		this.label8.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(5, 7);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(145, 38);
		this.label8.TabIndex = 79;
		this.label8.Text = "Setting";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(541, 560);
		base.Controls.Add(this.panelOnly1);
		base.Controls.Add(this.btnClose);
		base.Controls.Add(this.btnSave);
		base.Controls.Add(this.cboScanHandshake);
		base.Controls.Add(this.cboScanParity);
		base.Controls.Add(this.label6);
		base.Controls.Add(this.label7);
		base.Controls.Add(this.cboScanStopbits);
		base.Controls.Add(this.cboScanDatabits);
		base.Controls.Add(this.label1);
		base.Controls.Add(this.label5);
		base.Controls.Add(this.label4);
		base.Controls.Add(this.txtOrgID);
		base.Controls.Add(this.cboScanBaudRate);
		base.Controls.Add(this.cboScanPort);
		base.Controls.Add(this.label2);
		base.Controls.Add(this.label3);
		base.Name = "frmConfig";
		this.Text = "Setting";
		base.Load += new System.EventHandler(frmConfig_Load);
		this.panelOnly1.ResumeLayout(false);
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
