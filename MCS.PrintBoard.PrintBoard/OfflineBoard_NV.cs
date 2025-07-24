using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Resources;
using DDCssLib;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Controls;
using GrapeCity.ActiveReports.SectionReportModel;

namespace MCS.PrintBoard.PrintBoard;

public class OfflineBoard_NV : SectionReport
{
	private Detail detail;

	private Shape shape1;

	private Line line1;

	private Line line2;

	private Line line3;

	private Line line4;

	private Line line5;

	private Line line6;

	private Line line7;

	private Line line8;

	private Line line9;

	private Line line10;

	private Line line11;

	private Line line12;

	private Shape shape2;

	private TextBox txtTOTAL_QUANTITY;

	private TextBox txtCARRIER_ID;

	private TextBox txtMADE_BY;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private TextBox txtLOCATOR_GROUP;

	private TextBox txtLINE;

	private TextBox txtDESCRIPTION;

	private TextBox txtINSPECTION_FLAG;

	private TextBox txtTOTAL_SEQ_NO;

	private Label label6;

	private Label label7;

	private Label label8;

	private Label label9;

	private TextBox txtPART_NO;

	private TextBox txtPST;

	private TextBox txtWO_QUANTITY1;

	private Label label10;

	private TextBox txtWO_QUANTITY2;

	private TextBox txtWO_QUANTITY3;

	private TextBox txtWO_QUANTITY4;

	private TextBox txtWO_QUANTITY5;

	private Barcode barcode;

	private TextBox txtMODEL_SUFFIX1;

	private Label label12;

	private TextBox textBox4;

	private TextBox textBox9;

	private TextBox textBox14;

	private TextBox textBox19;

	private Label label15;

	private TextBox txtPrintedDate;

	private TextBox txtSheetID;

	private Label label17;

	private TextBox txtWORKER_ORDER1;

	private Label label13;

	private TextBox textBox5;

	private TextBox textBox10;

	private TextBox textBox15;

	private TextBox textBox20;

	public OfflineBoard_NV()
	{
		InitializeComponent();
	}

	private void detail_Format(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		ResourceManager resources = new ResourceManager(typeof(OfflineBoard_NV));
		detail = new Detail();
		shape1 = new Shape();
		shape2 = new Shape();
		line1 = new Line();
		line2 = new Line();
		line3 = new Line();
		line4 = new Line();
		line5 = new Line();
		line6 = new Line();
		line7 = new Line();
		line8 = new Line();
		line9 = new Line();
		line10 = new Line();
		line11 = new Line();
		line12 = new Line();
		txtTOTAL_QUANTITY = new TextBox();
		txtCARRIER_ID = new TextBox();
		txtMADE_BY = new TextBox();
		label1 = new Label();
		label2 = new Label();
		label3 = new Label();
		label4 = new Label();
		label5 = new Label();
		txtLOCATOR_GROUP = new TextBox();
		txtLINE = new TextBox();
		txtDESCRIPTION = new TextBox();
		txtINSPECTION_FLAG = new TextBox();
		txtTOTAL_SEQ_NO = new TextBox();
		label6 = new Label();
		label7 = new Label();
		label8 = new Label();
		label9 = new Label();
		txtPART_NO = new TextBox();
		txtPST = new TextBox();
		txtWO_QUANTITY1 = new TextBox();
		label10 = new Label();
		txtWO_QUANTITY2 = new TextBox();
		txtWO_QUANTITY3 = new TextBox();
		txtWO_QUANTITY4 = new TextBox();
		txtWO_QUANTITY5 = new TextBox();
		barcode = new Barcode();
		txtMODEL_SUFFIX1 = new TextBox();
		label12 = new Label();
		textBox4 = new TextBox();
		textBox9 = new TextBox();
		textBox14 = new TextBox();
		textBox19 = new TextBox();
		label15 = new Label();
		txtPrintedDate = new TextBox();
		txtSheetID = new TextBox();
		label17 = new Label();
		txtWORKER_ORDER1 = new TextBox();
		label13 = new Label();
		textBox5 = new TextBox();
		textBox10 = new TextBox();
		textBox15 = new TextBox();
		textBox20 = new TextBox();
		((ISupportInitialize)txtTOTAL_QUANTITY).BeginInit();
		((ISupportInitialize)txtCARRIER_ID).BeginInit();
		((ISupportInitialize)txtMADE_BY).BeginInit();
		((ISupportInitialize)label1).BeginInit();
		((ISupportInitialize)label2).BeginInit();
		((ISupportInitialize)label3).BeginInit();
		((ISupportInitialize)label4).BeginInit();
		((ISupportInitialize)label5).BeginInit();
		((ISupportInitialize)txtLOCATOR_GROUP).BeginInit();
		((ISupportInitialize)txtLINE).BeginInit();
		((ISupportInitialize)txtDESCRIPTION).BeginInit();
		((ISupportInitialize)txtINSPECTION_FLAG).BeginInit();
		((ISupportInitialize)txtTOTAL_SEQ_NO).BeginInit();
		((ISupportInitialize)label6).BeginInit();
		((ISupportInitialize)label7).BeginInit();
		((ISupportInitialize)label8).BeginInit();
		((ISupportInitialize)label9).BeginInit();
		((ISupportInitialize)txtPART_NO).BeginInit();
		((ISupportInitialize)txtPST).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY1).BeginInit();
		((ISupportInitialize)label10).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY2).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY3).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY4).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY5).BeginInit();
		((ISupportInitialize)txtMODEL_SUFFIX1).BeginInit();
		((ISupportInitialize)label12).BeginInit();
		((ISupportInitialize)textBox4).BeginInit();
		((ISupportInitialize)textBox9).BeginInit();
		((ISupportInitialize)textBox14).BeginInit();
		((ISupportInitialize)textBox19).BeginInit();
		((ISupportInitialize)label15).BeginInit();
		((ISupportInitialize)txtPrintedDate).BeginInit();
		((ISupportInitialize)txtSheetID).BeginInit();
		((ISupportInitialize)label17).BeginInit();
		((ISupportInitialize)txtWORKER_ORDER1).BeginInit();
		((ISupportInitialize)label13).BeginInit();
		((ISupportInitialize)textBox5).BeginInit();
		((ISupportInitialize)textBox10).BeginInit();
		((ISupportInitialize)textBox15).BeginInit();
		((ISupportInitialize)textBox20).BeginInit();
		((ISupportInitialize)this).BeginInit();
		detail.ColumnDirection = ColumnDirection.AcrossDown;
		detail.Controls.AddRange(new ARControl[56]
		{
			shape2, shape1, line1, line2, line6, line8, txtTOTAL_QUANTITY, txtCARRIER_ID, txtMADE_BY, label1,
			label2, label3, label4, label5, txtLOCATOR_GROUP, txtLINE, txtDESCRIPTION, txtINSPECTION_FLAG, txtTOTAL_SEQ_NO, label6,
			label7, label8, label9, txtPART_NO, txtPST, txtWO_QUANTITY1, label10, txtWO_QUANTITY2, txtWO_QUANTITY3, txtWO_QUANTITY4,
			txtWO_QUANTITY5, barcode, txtMODEL_SUFFIX1, label12, textBox4, textBox9, textBox14, textBox19, label15, txtPrintedDate,
			txtSheetID, label17, txtWORKER_ORDER1, label13, textBox5, textBox10, textBox15, textBox20, line9, line7,
			line3, line4, line5, line11, line12, line10
		});
		detail.Height = 3.066112f;
		detail.Name = "detail";
		detail.Format += detail_Format;
		shape1.Height = 2.836f;
		shape1.Left = 0.08000001f;
		shape1.Name = "shape1";
		shape1.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		shape1.Top = 0.04f;
		shape1.Width = 3.81f;
		shape2.Height = 2.793f;
		shape2.Left = 0.1f;
		shape2.Name = "shape2";
		shape2.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		shape2.Top = 0.06f;
		shape2.Width = 3.768f;
		line1.Height = 0f;
		line1.Left = 0.1f;
		line1.LineWeight = 1.5f;
		line1.Name = "line1";
		line1.Top = 0.5f;
		line1.Width = 2.875f;
		line1.X1 = 0.1f;
		line1.X2 = 2.975f;
		line1.Y1 = 0.5f;
		line1.Y2 = 0.5f;
		line2.Height = 0f;
		line2.Left = 0.1f;
		line2.LineWeight = 1.5f;
		line2.Name = "line2";
		line2.Top = 0.998f;
		line2.Width = 3.768f;
		line2.X1 = 0.1f;
		line2.X2 = 3.868f;
		line2.Y1 = 0.998f;
		line2.Y2 = 0.998f;
		line3.Height = 1.192093E-07f;
		line3.Left = 0.1f;
		line3.LineWeight = 1.5f;
		line3.Name = "line3";
		line3.Top = 1.5f;
		line3.Width = 3.768f;
		line3.X1 = 0.1f;
		line3.X2 = 3.868f;
		line3.Y1 = 1.5f;
		line3.Y2 = 1.5f;
		line4.Height = 0f;
		line4.Left = 0.105f;
		line4.LineWeight = 1.5f;
		line4.Name = "line4";
		line4.Top = 1.992f;
		line4.Width = 3.763f;
		line4.X1 = 0.105f;
		line4.X2 = 3.868f;
		line4.Y1 = 1.992f;
		line4.Y2 = 1.992f;
		line5.Height = 0f;
		line5.Left = 0.1f;
		line5.LineWeight = 1.5f;
		line5.Name = "line5";
		line5.Top = 2.504f;
		line5.Width = 3.768f;
		line5.X1 = 0.1f;
		line5.X2 = 3.868f;
		line5.Y1 = 2.504f;
		line5.Y2 = 2.504f;
		line6.Height = 2.444f;
		line6.Left = 0.7459999f;
		line6.LineWeight = 1.5f;
		line6.Name = "line6";
		line6.Top = 0.06f;
		line6.Width = 0f;
		line6.X1 = 0.7459999f;
		line6.X2 = 0.7459999f;
		line6.Y1 = 0.06f;
		line6.Y2 = 2.504f;
		line7.Height = 1.506f;
		line7.Left = 1.644f;
		line7.LineWeight = 1.5f;
		line7.Name = "line7";
		line7.Top = 0.998f;
		line7.Width = 0f;
		line7.X1 = 1.644f;
		line7.X2 = 1.644f;
		line7.Y1 = 0.998f;
		line7.Y2 = 2.504f;
		line8.Height = 1.506f;
		line8.Left = 1.97f;
		line8.LineWeight = 1.5f;
		line8.Name = "line8";
		line8.Top = 0.998f;
		line8.Width = 0f;
		line8.X1 = 1.97f;
		line8.X2 = 1.97f;
		line8.Y1 = 0.998f;
		line8.Y2 = 2.504f;
		line9.Height = 2.444f;
		line9.Left = 2.979f;
		line9.LineWeight = 1.5f;
		line9.Name = "line9";
		line9.Top = 0.06f;
		line9.Width = 0f;
		line9.X1 = 2.979f;
		line9.X2 = 2.979f;
		line9.Y1 = 0.06f;
		line9.Y2 = 2.504f;
		line10.Height = 0f;
		line10.Left = 1.644f;
		line10.LineWeight = 1.5f;
		line10.Name = "line10";
		line10.Top = 1.25f;
		line10.Width = 2.224f;
		line10.X1 = 1.644f;
		line10.X2 = 3.868f;
		line10.Y1 = 1.25f;
		line10.Y2 = 1.25f;
		line11.Height = 0f;
		line11.Left = 1.644f;
		line11.LineWeight = 1.5f;
		line11.Name = "line11";
		line11.Top = 1.75f;
		line11.Width = 2.224f;
		line11.X1 = 1.644f;
		line11.X2 = 3.868f;
		line11.Y1 = 1.75f;
		line11.Y2 = 1.75f;
		line12.Height = 0f;
		line12.Left = 1.644f;
		line12.LineWeight = 1.5f;
		line12.Name = "line12";
		line12.Top = 2.252f;
		line12.Width = 2.224f;
		line12.X1 = 1.644f;
		line12.X2 = 3.868f;
		line12.Y1 = 2.252f;
		line12.Y2 = 2.252f;
		txtTOTAL_QUANTITY.CanGrow = false;
		txtTOTAL_QUANTITY.DataField = "TOTAL_QUANTITY";
		txtTOTAL_QUANTITY.Height = 0.2f;
		txtTOTAL_QUANTITY.Left = 0.11f;
		txtTOTAL_QUANTITY.Name = "txtTOTAL_QUANTITY";
		txtTOTAL_QUANTITY.Style = "background-color: Transparent; font-size: 10pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtTOTAL_QUANTITY.Text = "TOTAL_QUANTITY";
		txtTOTAL_QUANTITY.Top = 0.689f;
		txtTOTAL_QUANTITY.Width = 0.6253223f;
		txtCARRIER_ID.DataField = "CARRIER_ID";
		txtCARRIER_ID.Height = 0.2468503f;
		txtCARRIER_ID.Left = 0.107f;
		txtCARRIER_ID.Name = "txtCARRIER_ID";
		txtCARRIER_ID.Style = "background-color: Transparent; font-size: 9pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtCARRIER_ID.Text = "CARRIER_ID";
		txtCARRIER_ID.Top = 1.666f;
		txtCARRIER_ID.Width = 0.65f;
		txtMADE_BY.CanGrow = false;
		txtMADE_BY.DataField = "MADE_BY";
		txtMADE_BY.Height = 0.2570866f;
		txtMADE_BY.Left = 0.115f;
		txtMADE_BY.Name = "txtMADE_BY";
		txtMADE_BY.OutputFormat = resources.GetString("txtMADE_BY.OutputFormat");
		txtMADE_BY.Style = "background-color: Transparent; font-size: 9pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-font-vertical: true";
		txtMADE_BY.Text = "MADE_BY";
		txtMADE_BY.Top = 1.173f;
		txtMADE_BY.Width = 0.642f;
		label1.Height = 0.115f;
		label1.HyperLink = null;
		label1.Left = 0.115f;
		label1.Name = "label1";
		label1.Style = "font-family: Arial; font-size: 6pt";
		label1.Text = "Line";
		label1.Top = 0.07999989f;
		label1.Width = 0.281f;
		label2.Height = 0.109f;
		label2.HyperLink = null;
		label2.Left = 0.115f;
		label2.Name = "label2";
		label2.Style = "font-family: Arial; font-size: 6pt";
		label2.Text = "Qty";
		label2.Top = 0.5270002f;
		label2.Width = 0.218f;
		label3.Height = 0.1562992f;
		label3.HyperLink = null;
		label3.Left = 0.115f;
		label3.Name = "label3";
		label3.Style = "font-size: 6pt";
		label3.Text = "Made by";
		label3.Top = 1.008f;
		label3.Width = 0.6350001f;
		label4.Height = 0.1562992f;
		label4.HyperLink = null;
		label4.Left = 0.115f;
		label4.Name = "label4";
		label4.Style = "font-family: Arial; font-size: 6pt";
		label4.Text = "Carrier ID";
		label4.Top = 1.52f;
		label4.Width = 0.6350001f;
		label5.Height = 0.1562992f;
		label5.HyperLink = null;
		label5.Left = 0.115f;
		label5.Name = "label5";
		label5.Style = "font-family: Arial; font-size: 5.5pt; font-weight: normal; ddo-char-set: 1";
		label5.Text = "To Locator Group";
		label5.Top = 2.017f;
		label5.Width = 0.681f;
		txtLOCATOR_GROUP.DataField = "LOCATOR_GROUP";
		txtLOCATOR_GROUP.Height = 0.2468504f;
		txtLOCATOR_GROUP.Left = 0.115f;
		txtLOCATOR_GROUP.Name = "txtLOCATOR_GROUP";
		txtLOCATOR_GROUP.Style = "background-color: Transparent; font-size: 9pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtLOCATOR_GROUP.Text = "LOCATOR_GROUP";
		txtLOCATOR_GROUP.Top = 2.195f;
		txtLOCATOR_GROUP.Width = 0.622f;
		txtLINE.DataField = "LINE";
		txtLINE.Height = 0.2468504f;
		txtLINE.Left = 0.115f;
		txtLINE.Name = "txtLINE";
		txtLINE.Style = "background-color: Transparent; font-size: 9.5pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtLINE.Text = "LINE";
		txtLINE.Top = 0.183f;
		txtLINE.Width = 0.6100001f;
		txtDESCRIPTION.CanGrow = false;
		txtDESCRIPTION.DataField = "DESCRIPTION";
		txtDESCRIPTION.Height = 0.342f;
		txtDESCRIPTION.Left = 0.7609999f;
		txtDESCRIPTION.Name = "txtDESCRIPTION";
		txtDESCRIPTION.OutputFormat = resources.GetString("txtDESCRIPTION.OutputFormat");
		txtDESCRIPTION.Style = "background-color: Transparent; font-size: 14pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtDESCRIPTION.Text = "DESCRIPTION";
		txtDESCRIPTION.Top = 0.646f;
		txtDESCRIPTION.Width = 2.214f;
		txtINSPECTION_FLAG.CanGrow = false;
		txtINSPECTION_FLAG.DataField = "INSPECTION_FLAG";
		txtINSPECTION_FLAG.Height = 0.2f;
		txtINSPECTION_FLAG.Left = 0.7499999f;
		txtINSPECTION_FLAG.Name = "txtINSPECTION_FLAG";
		txtINSPECTION_FLAG.OutputFormat = resources.GetString("txtINSPECTION_FLAG.OutputFormat");
		txtINSPECTION_FLAG.Style = "background-color: Transparent; font-size: 9.5pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		txtINSPECTION_FLAG.Text = "INSPECTION_FLAG";
		txtINSPECTION_FLAG.Top = 1.665f;
		txtINSPECTION_FLAG.Width = 0.8736861f;
		txtTOTAL_SEQ_NO.DataField = "TOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.Height = 0.3354331f;
		txtTOTAL_SEQ_NO.Left = 0.929f;
		txtTOTAL_SEQ_NO.Name = "txtTOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.OutputFormat = resources.GetString("txtTOTAL_SEQ_NO.OutputFormat");
		txtTOTAL_SEQ_NO.Style = "background-color: Transparent; font-size: 14pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-font-vertical: none";
		txtTOTAL_SEQ_NO.Text = "TOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.Top = 0.144f;
		txtTOTAL_SEQ_NO.Width = 1.943907f;
		label6.Height = 0.1562992f;
		label6.HyperLink = null;
		label6.Left = 0.767f;
		label6.Name = "label6";
		label6.Style = "font-family: Arial; font-size: 6pt";
		label6.Text = "Seq No";
		label6.Top = 0.07700001f;
		label6.Width = 0.958268f;
		label7.Height = 0.1562992f;
		label7.HyperLink = null;
		label7.Left = 0.767f;
		label7.Name = "label7";
		label7.Style = "font-family: Arial; font-size: 6pt";
		label7.Text = "Description";
		label7.Top = 0.515f;
		label7.Width = 0.958268f;
		label8.Height = 0.1562992f;
		label8.HyperLink = null;
		label8.Left = 0.767f;
		label8.Name = "label8";
		label8.Style = "font-family: Arial; font-size: 6pt";
		label8.Text = "Part No";
		label8.Top = 1.008f;
		label8.Width = 0.365f;
		label9.Height = 0.1562992f;
		label9.HyperLink = null;
		label9.Left = 0.767f;
		label9.Name = "label9";
		label9.Style = "font-family: Arial; font-size: 6pt";
		label9.Text = "PST";
		label9.Top = 2.017f;
		label9.Width = 0.26f;
		txtPART_NO.DataField = "PART_NO";
		txtPART_NO.Height = 0.2570866f;
		txtPART_NO.Left = 0.7599999f;
		txtPART_NO.Name = "txtPART_NO";
		txtPART_NO.Style = "background-color: Transparent; font-size: 9pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtPART_NO.Text = "PART_NO";
		txtPART_NO.Top = 1.14f;
		txtPART_NO.Width = 0.8840001f;
		txtPST.CanGrow = false;
		txtPST.DataField = "PST";
		txtPST.Height = 0.3308665f;
		txtPST.Left = 0.8009999f;
		txtPST.Name = "txtPST";
		txtPST.Style = "background-color: Transparent; font-family: Arial; font-size: 9pt; font-weight: bold; text-align: center; vertical-align: top; ddo-char-set: 1";
		txtPST.Text = "2019-01-30 PM  12:12:11";
		txtPST.Top = 2.132f;
		txtPST.Width = 0.8153546f;
		txtWO_QUANTITY1.CanGrow = false;
		txtWO_QUANTITY1.DataField = "WO_QUANTITY1";
		txtWO_QUANTITY1.Height = 0.1811024f;
		txtWO_QUANTITY1.Left = 1.664f;
		txtWO_QUANTITY1.Name = "txtWO_QUANTITY1";
		txtWO_QUANTITY1.OutputFormat = resources.GetString("txtWO_QUANTITY1.OutputFormat");
		txtWO_QUANTITY1.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY1.Text = "WO_QUANTITY1";
		txtWO_QUANTITY1.Top = 1.293f;
		txtWO_QUANTITY1.Width = 0.2740002f;
		label10.Height = 0.1811024f;
		label10.HyperLink = null;
		label10.Left = 1.654f;
		label10.Name = "label10";
		label10.Style = "font-size: 8pt; font-weight: bold; text-align: center";
		label10.Text = "Qty";
		label10.Top = 1.037f;
		label10.Width = 0.3060002f;
		txtWO_QUANTITY2.CanGrow = false;
		txtWO_QUANTITY2.DataField = "WO_QUANTITY2";
		txtWO_QUANTITY2.Height = 0.1811024f;
		txtWO_QUANTITY2.Left = 1.664f;
		txtWO_QUANTITY2.Name = "txtWO_QUANTITY2";
		txtWO_QUANTITY2.OutputFormat = resources.GetString("txtWO_QUANTITY2.OutputFormat");
		txtWO_QUANTITY2.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY2.Text = "WO_QUANTITY2";
		txtWO_QUANTITY2.Top = 1.54f;
		txtWO_QUANTITY2.Width = 0.2740002f;
		txtWO_QUANTITY3.CanGrow = false;
		txtWO_QUANTITY3.DataField = "WO_QUANTITY3";
		txtWO_QUANTITY3.Height = 0.1811024f;
		txtWO_QUANTITY3.Left = 1.664f;
		txtWO_QUANTITY3.Name = "txtWO_QUANTITY3";
		txtWO_QUANTITY3.OutputFormat = resources.GetString("txtWO_QUANTITY3.OutputFormat");
		txtWO_QUANTITY3.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY3.Text = "WO_QUANTITY3";
		txtWO_QUANTITY3.Top = 1.795f;
		txtWO_QUANTITY3.Width = 0.2740002f;
		txtWO_QUANTITY4.CanGrow = false;
		txtWO_QUANTITY4.DataField = "WO_QUANTITY4";
		txtWO_QUANTITY4.Height = 0.1811024f;
		txtWO_QUANTITY4.Left = 1.664f;
		txtWO_QUANTITY4.Name = "txtWO_QUANTITY4";
		txtWO_QUANTITY4.OutputFormat = resources.GetString("txtWO_QUANTITY4.OutputFormat");
		txtWO_QUANTITY4.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY4.Text = "WO_QUANTITY3";
		txtWO_QUANTITY4.Top = 2.044f;
		txtWO_QUANTITY4.Width = 0.2740002f;
		txtWO_QUANTITY5.CanGrow = false;
		txtWO_QUANTITY5.DataField = "WO_QUANTITY5";
		txtWO_QUANTITY5.Height = 0.1811026f;
		txtWO_QUANTITY5.Left = 1.664f;
		txtWO_QUANTITY5.Name = "txtWO_QUANTITY5";
		txtWO_QUANTITY5.OutputFormat = resources.GetString("txtWO_QUANTITY5.OutputFormat");
		txtWO_QUANTITY5.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY5.Text = "WO_QUANTITY3";
		txtWO_QUANTITY5.Top = 2.292f;
		txtWO_QUANTITY5.Width = 0.2740002f;
		barcode.BackColor = Color.FromArgb(255, 255, 255);
		barcode.DataField = "QRCODE_VALUE";
		barcode.Font = new Font("Courier New", 8f);
		barcode.Height = 0.8284723f;
		barcode.Left = 3.009f;
		barcode.Name = "barcode";
		barcode.QuietZoneBottom = 0f;
		barcode.QuietZoneLeft = 0f;
		barcode.QuietZoneRight = 0f;
		barcode.QuietZoneTop = 0f;
		barcode.Style = BarCodeStyle.QRCode;
		barcode.Text = "barcod";
		barcode.Top = 0.11f;
		barcode.Width = 0.8284723f;
		txtMODEL_SUFFIX1.CanGrow = false;
		txtMODEL_SUFFIX1.DataField = "MODEL_SUFFIX1";
		txtMODEL_SUFFIX1.Height = 0.227f;
		txtMODEL_SUFFIX1.Left = 1.962f;
		txtMODEL_SUFFIX1.Name = "txtMODEL_SUFFIX1";
		txtMODEL_SUFFIX1.OutputFormat = resources.GetString("txtMODEL_SUFFIX1.OutputFormat");
		txtMODEL_SUFFIX1.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		txtMODEL_SUFFIX1.Text = "MODEL_SUFFIX1";
		txtMODEL_SUFFIX1.Top = 1.263f;
		txtMODEL_SUFFIX1.Width = 1.013f;
		label12.Height = 0.1811024f;
		label12.HyperLink = null;
		label12.Left = 1.972f;
		label12.Name = "label12";
		label12.Style = "font-size: 9.75pt; font-weight: bold; text-align: center";
		label12.Text = "Model.Suffix";
		label12.Top = 1.037f;
		label12.Width = 0.9819999f;
		textBox4.CanGrow = false;
		textBox4.DataField = "MODEL_SUFFIX2";
		textBox4.Height = 0.227f;
		textBox4.Left = 1.962f;
		textBox4.Name = "textBox4";
		textBox4.OutputFormat = resources.GetString("textBox4.OutputFormat");
		textBox4.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		textBox4.Text = "MODEL_SUFFIX2";
		textBox4.Top = 1.51f;
		textBox4.Width = 1.013f;
		textBox9.CanGrow = false;
		textBox9.DataField = "MODEL_SUFFIX3";
		textBox9.Height = 0.227f;
		textBox9.Left = 1.962f;
		textBox9.Name = "textBox9";
		textBox9.OutputFormat = resources.GetString("textBox9.OutputFormat");
		textBox9.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		textBox9.Text = "MODEL_SUFFIX3";
		textBox9.Top = 1.765f;
		textBox9.Width = 1.013f;
		textBox14.CanGrow = false;
		textBox14.DataField = "MODEL_SUFFIX4";
		textBox14.Height = 0.227f;
		textBox14.Left = 1.962f;
		textBox14.Name = "textBox14";
		textBox14.OutputFormat = resources.GetString("textBox14.OutputFormat");
		textBox14.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		textBox14.Text = "MODEL_SUFFIX4";
		textBox14.Top = 2.014f;
		textBox14.Width = 1.013f;
		textBox19.CanGrow = false;
		textBox19.DataField = "MODEL_SUFFIX5";
		textBox19.Height = 0.227f;
		textBox19.Left = 1.962f;
		textBox19.Name = "textBox19";
		textBox19.OutputFormat = resources.GetString("textBox19.OutputFormat");
		textBox19.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		textBox19.Text = "MODEL_SUFFIX5";
		textBox19.Top = 2.262f;
		textBox19.Width = 1.013f;
		label15.Height = 0.202362f;
		label15.HyperLink = null;
		label15.Left = 0.117f;
		label15.Name = "label15";
		label15.Style = "font-size: 9pt; font-weight: normal; vertical-align: middle";
		label15.Text = "Printed Date : ";
		label15.Top = 2.584f;
		label15.Width = 0.8280001f;
		txtPrintedDate.CanGrow = false;
		txtPrintedDate.DataField = "PRINTED_DATE";
		txtPrintedDate.Height = 0.222047f;
		txtPrintedDate.Left = 0.9449999f;
		txtPrintedDate.Name = "txtPrintedDate";
		txtPrintedDate.Style = "background-color: Transparent; font-size: 9pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtPrintedDate.Text = "PRINTED_DATE";
		txtPrintedDate.Top = 2.584f;
		txtPrintedDate.Width = 1.831f;
		txtSheetID.CanGrow = false;
		txtSheetID.DataField = "SHEET_ID";
		txtSheetID.Height = 0.222047f;
		txtSheetID.Left = 2.085f;
		txtSheetID.Name = "txtSheetID";
		txtSheetID.Style = "background-color: Transparent; font-size: 9pt; font-weight: normal; text-align: right; vertical-align: middle";
		txtSheetID.Text = "SHEET_ID";
		txtSheetID.Top = 2.584f;
		txtSheetID.Width = 1.762567f;
		label17.Height = 0.1503622f;
		label17.HyperLink = null;
		label17.Left = 3.009f;
		label17.Name = "label17";
		label17.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; vertical-align: middle";
		label17.Text = "Printed by MCS";
		label17.Top = 2.896f;
		label17.Width = 0.8590002f;
		txtWORKER_ORDER1.CanGrow = false;
		txtWORKER_ORDER1.DataField = "WORKER_ORDER1";
		txtWORKER_ORDER1.Height = 0.1811024f;
		txtWORKER_ORDER1.Left = 3.023f;
		txtWORKER_ORDER1.Name = "txtWORKER_ORDER1";
		txtWORKER_ORDER1.OutputFormat = resources.GetString("txtWORKER_ORDER1.OutputFormat");
		txtWORKER_ORDER1.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWORKER_ORDER1.Text = "WORKER_ORDER1";
		txtWORKER_ORDER1.Top = 1.293f;
		txtWORKER_ORDER1.Width = 0.8150002f;
		label13.Height = 0.1811024f;
		label13.HyperLink = null;
		label13.Left = 3.003f;
		label13.Name = "label13";
		label13.Style = "font-size: 9.75pt; font-weight: bold; text-align: center";
		label13.Text = "W/O";
		label13.Top = 1.037f;
		label13.Width = 0.8350003f;
		textBox5.CanGrow = false;
		textBox5.DataField = "WORKER_ORDER2";
		textBox5.Height = 0.1811024f;
		textBox5.Left = 3.023f;
		textBox5.Name = "textBox5";
		textBox5.OutputFormat = resources.GetString("textBox5.OutputFormat");
		textBox5.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox5.Text = "WORKER_ORDER2";
		textBox5.Top = 1.54f;
		textBox5.Width = 0.8150002f;
		textBox10.CanGrow = false;
		textBox10.DataField = "WORKER_ORDER3";
		textBox10.Height = 0.1811024f;
		textBox10.Left = 3.023f;
		textBox10.Name = "textBox10";
		textBox10.OutputFormat = resources.GetString("textBox10.OutputFormat");
		textBox10.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox10.Text = "WORKER_ORDER3";
		textBox10.Top = 1.795f;
		textBox10.Width = 0.8150002f;
		textBox15.CanGrow = false;
		textBox15.DataField = "WORKER_ORDER4";
		textBox15.Height = 0.1811024f;
		textBox15.Left = 3.023f;
		textBox15.Name = "textBox15";
		textBox15.OutputFormat = resources.GetString("textBox15.OutputFormat");
		textBox15.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox15.Text = "WORKER_ORDER4";
		textBox15.Top = 2.044f;
		textBox15.Width = 0.8150002f;
		textBox20.CanGrow = false;
		textBox20.DataField = "WORKER_ORDER5";
		textBox20.Height = 0.1811024f;
		textBox20.Left = 3.023f;
		textBox20.Name = "textBox20";
		textBox20.OutputFormat = resources.GetString("textBox20.OutputFormat");
		textBox20.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox20.Text = "WORKER_ORDER5";
		textBox20.Top = 2.292f;
		textBox20.Width = 0.8150002f;
		base.MasterReport = false;
		base.PageSettings.DefaultPaperSize = false;
		base.PageSettings.Margins.Bottom = 0f;
		base.PageSettings.Margins.Left = 0f;
		base.PageSettings.Margins.Right = 0f;
		base.PageSettings.Margins.Top = 0f;
		base.PageSettings.PaperHeight = 3.2f;
		base.PageSettings.PaperKind = PaperKind.Custom;
		base.PageSettings.PaperName = "Custom paper";
		base.PageSettings.PaperWidth = 4f;
		base.PrintWidth = 3.945296f;
		base.Sections.Add(detail);
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; color: Black", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: italic", "Heading2", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
		((ISupportInitialize)txtTOTAL_QUANTITY).EndInit();
		((ISupportInitialize)txtCARRIER_ID).EndInit();
		((ISupportInitialize)txtMADE_BY).EndInit();
		((ISupportInitialize)label1).EndInit();
		((ISupportInitialize)label2).EndInit();
		((ISupportInitialize)label3).EndInit();
		((ISupportInitialize)label4).EndInit();
		((ISupportInitialize)label5).EndInit();
		((ISupportInitialize)txtLOCATOR_GROUP).EndInit();
		((ISupportInitialize)txtLINE).EndInit();
		((ISupportInitialize)txtDESCRIPTION).EndInit();
		((ISupportInitialize)txtINSPECTION_FLAG).EndInit();
		((ISupportInitialize)txtTOTAL_SEQ_NO).EndInit();
		((ISupportInitialize)label6).EndInit();
		((ISupportInitialize)label7).EndInit();
		((ISupportInitialize)label8).EndInit();
		((ISupportInitialize)label9).EndInit();
		((ISupportInitialize)txtPART_NO).EndInit();
		((ISupportInitialize)txtPST).EndInit();
		((ISupportInitialize)txtWO_QUANTITY1).EndInit();
		((ISupportInitialize)label10).EndInit();
		((ISupportInitialize)txtWO_QUANTITY2).EndInit();
		((ISupportInitialize)txtWO_QUANTITY3).EndInit();
		((ISupportInitialize)txtWO_QUANTITY4).EndInit();
		((ISupportInitialize)txtWO_QUANTITY5).EndInit();
		((ISupportInitialize)txtMODEL_SUFFIX1).EndInit();
		((ISupportInitialize)label12).EndInit();
		((ISupportInitialize)textBox4).EndInit();
		((ISupportInitialize)textBox9).EndInit();
		((ISupportInitialize)textBox14).EndInit();
		((ISupportInitialize)textBox19).EndInit();
		((ISupportInitialize)label15).EndInit();
		((ISupportInitialize)txtPrintedDate).EndInit();
		((ISupportInitialize)txtSheetID).EndInit();
		((ISupportInitialize)label17).EndInit();
		((ISupportInitialize)txtWORKER_ORDER1).EndInit();
		((ISupportInitialize)label13).EndInit();
		((ISupportInitialize)textBox5).EndInit();
		((ISupportInitialize)textBox10).EndInit();
		((ISupportInitialize)textBox15).EndInit();
		((ISupportInitialize)textBox20).EndInit();
		((ISupportInitialize)this).EndInit();
	}
}
