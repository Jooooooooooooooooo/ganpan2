using System.ComponentModel;
using System.Drawing;
using System.Resources;
using DDCssLib;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Controls;
using GrapeCity.ActiveReports.SectionReportModel;

namespace MCS.PrintBoard;

public class OfflineBoard_VerticalA4 : SectionReport
{
	private Detail detail;

	private Shape shape2;

	private TextBox txtTOTAL_QUANTITY;

	private TextBox txtCARRIER_ID;

	private TextBox txtMADE_BY;

	private TextBox txtDESCRIPTION;

	private TextBox txtINSPECTION_FLAG;

	private TextBox txtWO_QUANTITY1;

	private TextBox txtTOTAL_SEQ_NO;

	private TextBox txtMODEL_SUFFIX1;

	private TextBox txtWORKER_ORDER1;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label8;

	private Label label9;

	private Label label10;

	private Label label12;

	private Label label13;

	private Label label15;

	private TextBox txtPrintedDate;

	private Label label17;

	private TextBox txtWO_QUANTITY2;

	private TextBox textBox4;

	private TextBox textBox5;

	private TextBox txtWO_QUANTITY3;

	private TextBox textBox9;

	private TextBox textBox10;

	private TextBox txtWO_QUANTITY4;

	private TextBox textBox14;

	private TextBox textBox15;

	private TextBox txtWO_QUANTITY5;

	private TextBox textBox19;

	private TextBox textBox20;

	private Barcode barcode;

	private TextBox txtLOCATOR_GROUP;

	private TextBox txtLINE;

	private TextBox txtPART_NO;

	private Line line9;

	private Line line13;

	private TextBox txtSheetID;

	private TextBox txtPST;

	private Label label11;

	private Line line7;

	private Line line3;

	private Line line4;

	private Line line11;

	private Line line12;

	private Line line15;

	private Line line17;

	private Label label14;

	private Line line1;

	private Line line6;

	private Line line14;

	private Line line2;

	private Line line5;

	private Line line8;

	private Line line16;

	private Line line18;

	private Label label7;

	public OfflineBoard_VerticalA4()
	{
		InitializeComponent();
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
		ResourceManager resources = new ResourceManager(typeof(OfflineBoard_VerticalA4));
		detail = new Detail();
		shape2 = new Shape();
		txtTOTAL_QUANTITY = new TextBox();
		txtCARRIER_ID = new TextBox();
		txtMADE_BY = new TextBox();
		txtDESCRIPTION = new TextBox();
		txtINSPECTION_FLAG = new TextBox();
		txtWO_QUANTITY1 = new TextBox();
		txtTOTAL_SEQ_NO = new TextBox();
		txtMODEL_SUFFIX1 = new TextBox();
		txtWORKER_ORDER1 = new TextBox();
		label1 = new Label();
		label2 = new Label();
		label3 = new Label();
		label4 = new Label();
		label5 = new Label();
		label6 = new Label();
		label8 = new Label();
		label9 = new Label();
		label10 = new Label();
		label12 = new Label();
		label13 = new Label();
		label15 = new Label();
		txtPrintedDate = new TextBox();
		label17 = new Label();
		txtWO_QUANTITY2 = new TextBox();
		textBox4 = new TextBox();
		textBox5 = new TextBox();
		txtWO_QUANTITY3 = new TextBox();
		textBox9 = new TextBox();
		textBox10 = new TextBox();
		txtWO_QUANTITY4 = new TextBox();
		textBox14 = new TextBox();
		textBox15 = new TextBox();
		txtWO_QUANTITY5 = new TextBox();
		textBox19 = new TextBox();
		textBox20 = new TextBox();
		barcode = new Barcode();
		txtLOCATOR_GROUP = new TextBox();
		txtLINE = new TextBox();
		txtPART_NO = new TextBox();
		line9 = new Line();
		line13 = new Line();
		txtSheetID = new TextBox();
		txtPST = new TextBox();
		label11 = new Label();
		line7 = new Line();
		line3 = new Line();
		line4 = new Line();
		line11 = new Line();
		line12 = new Line();
		line15 = new Line();
		line17 = new Line();
		label14 = new Label();
		line1 = new Line();
		line6 = new Line();
		line14 = new Line();
		line2 = new Line();
		line5 = new Line();
		line8 = new Line();
		line16 = new Line();
		line18 = new Line();
		label7 = new Label();
		((ISupportInitialize)txtTOTAL_QUANTITY).BeginInit();
		((ISupportInitialize)txtCARRIER_ID).BeginInit();
		((ISupportInitialize)txtMADE_BY).BeginInit();
		((ISupportInitialize)txtDESCRIPTION).BeginInit();
		((ISupportInitialize)txtINSPECTION_FLAG).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY1).BeginInit();
		((ISupportInitialize)txtTOTAL_SEQ_NO).BeginInit();
		((ISupportInitialize)txtMODEL_SUFFIX1).BeginInit();
		((ISupportInitialize)txtWORKER_ORDER1).BeginInit();
		((ISupportInitialize)label1).BeginInit();
		((ISupportInitialize)label2).BeginInit();
		((ISupportInitialize)label3).BeginInit();
		((ISupportInitialize)label4).BeginInit();
		((ISupportInitialize)label5).BeginInit();
		((ISupportInitialize)label6).BeginInit();
		((ISupportInitialize)label8).BeginInit();
		((ISupportInitialize)label9).BeginInit();
		((ISupportInitialize)label10).BeginInit();
		((ISupportInitialize)label12).BeginInit();
		((ISupportInitialize)label13).BeginInit();
		((ISupportInitialize)label15).BeginInit();
		((ISupportInitialize)txtPrintedDate).BeginInit();
		((ISupportInitialize)label17).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY2).BeginInit();
		((ISupportInitialize)textBox4).BeginInit();
		((ISupportInitialize)textBox5).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY3).BeginInit();
		((ISupportInitialize)textBox9).BeginInit();
		((ISupportInitialize)textBox10).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY4).BeginInit();
		((ISupportInitialize)textBox14).BeginInit();
		((ISupportInitialize)textBox15).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY5).BeginInit();
		((ISupportInitialize)textBox19).BeginInit();
		((ISupportInitialize)textBox20).BeginInit();
		((ISupportInitialize)txtLOCATOR_GROUP).BeginInit();
		((ISupportInitialize)txtLINE).BeginInit();
		((ISupportInitialize)txtPART_NO).BeginInit();
		((ISupportInitialize)txtSheetID).BeginInit();
		((ISupportInitialize)txtPST).BeginInit();
		((ISupportInitialize)label11).BeginInit();
		((ISupportInitialize)label14).BeginInit();
		((ISupportInitialize)label7).BeginInit();
		((ISupportInitialize)this).BeginInit();
		detail.ColumnCount = 2;
		detail.ColumnDirection = ColumnDirection.AcrossDown;
		detail.Controls.AddRange(new ARControl[62]
		{
			shape2, txtTOTAL_QUANTITY, txtCARRIER_ID, txtMADE_BY, txtDESCRIPTION, txtINSPECTION_FLAG, txtWO_QUANTITY1, txtTOTAL_SEQ_NO, txtMODEL_SUFFIX1, txtWORKER_ORDER1,
			label1, label2, label3, label4, label5, label6, label8, label9, label10, label12,
			label13, label15, txtPrintedDate, label17, txtWO_QUANTITY2, textBox4, textBox5, txtWO_QUANTITY3, textBox9, textBox10,
			txtWO_QUANTITY4, textBox14, textBox15, txtWO_QUANTITY5, textBox19, textBox20, barcode, txtLOCATOR_GROUP, txtLINE, txtPART_NO,
			line9, line13, txtSheetID, txtPST, label11, line7, line3, line4, line11, line12,
			line15, line17, label14, line1, line6, line14, line2, line5, line8, line16,
			line18, label7
		});
		detail.Height = 5.715f;
		detail.Name = "detail";
		shape2.Height = 4.22f;
		shape2.Left = 0.3010006f;
		shape2.Name = "shape2";
		shape2.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		shape2.Top = 0.844f;
		shape2.Width = 3.599f;
		txtTOTAL_QUANTITY.CanGrow = false;
		txtTOTAL_QUANTITY.DataField = "TOTAL_QUANTITY";
		txtTOTAL_QUANTITY.Height = 0.2680001f;
		txtTOTAL_QUANTITY.Left = 1.669001f;
		txtTOTAL_QUANTITY.Name = "txtTOTAL_QUANTITY";
		txtTOTAL_QUANTITY.Style = "background-color: Transparent; font-size: 20pt; font-weight: bold; text-align: left; vertical-align: middle";
		txtTOTAL_QUANTITY.Text = "TOTAL_QTY";
		txtTOTAL_QUANTITY.Top = 1.776f;
		txtTOTAL_QUANTITY.Width = 2.049f;
		txtCARRIER_ID.DataField = "CARRIER_ID";
		txtCARRIER_ID.Height = 0.1562992f;
		txtCARRIER_ID.Left = 1.669001f;
		txtCARRIER_ID.Name = "txtCARRIER_ID";
		txtCARRIER_ID.Style = "background-color: Transparent; font-size: 11pt; font-weight: bold; text-align: left; vertical-align: middle";
		txtCARRIER_ID.Text = "CARRIER_ID";
		txtCARRIER_ID.Top = 2.367f;
		txtCARRIER_ID.Width = 2.049f;
		txtMADE_BY.CanGrow = false;
		txtMADE_BY.DataField = "MADE_BY";
		txtMADE_BY.Height = 0.1562992f;
		txtMADE_BY.Left = 1.669001f;
		txtMADE_BY.Name = "txtMADE_BY";
		txtMADE_BY.OutputFormat = resources.GetString("txtMADE_BY.OutputFormat");
		txtMADE_BY.Style = "background-color: Transparent; font-size: 11pt; font-weight: bold; text-align: left; vertical-align: middle; ddo-font-vertical: none";
		txtMADE_BY.Text = "MADE_BY";
		txtMADE_BY.Top = 2.864f;
		txtMADE_BY.Width = 2.049f;
		txtDESCRIPTION.CanGrow = false;
		txtDESCRIPTION.DataField = "DESCRIPTION";
		txtDESCRIPTION.Height = 0.2625981f;
		txtDESCRIPTION.Left = 1.669001f;
		txtDESCRIPTION.Name = "txtDESCRIPTION";
		txtDESCRIPTION.OutputFormat = resources.GetString("txtDESCRIPTION.OutputFormat");
		txtDESCRIPTION.Style = "background-color: Transparent; font-size: 11pt; font-weight: bold; text-align: left; vertical-align: middle";
		txtDESCRIPTION.Text = "DESCRIPTION";
		txtDESCRIPTION.Top = 1.447f;
		txtDESCRIPTION.Width = 2.049f;
		txtINSPECTION_FLAG.CanGrow = false;
		txtINSPECTION_FLAG.DataField = "INSPECTION_FLAG";
		txtINSPECTION_FLAG.Height = 0.1562992f;
		txtINSPECTION_FLAG.Left = 1.669001f;
		txtINSPECTION_FLAG.Name = "txtINSPECTION_FLAG";
		txtINSPECTION_FLAG.OutputFormat = resources.GetString("txtINSPECTION_FLAG.OutputFormat");
		txtINSPECTION_FLAG.Style = "background-color: Transparent; font-size: 11pt; font-weight: bold; text-align: left; vertical-align: middle; ddo-char-set: 1";
		txtINSPECTION_FLAG.Text = "INSPECTION_FLAG";
		txtINSPECTION_FLAG.Top = 3.108001f;
		txtINSPECTION_FLAG.Width = 2.049f;
		txtWO_QUANTITY1.CanGrow = false;
		txtWO_QUANTITY1.DataField = "WO_QUANTITY1";
		txtWO_QUANTITY1.Height = 0.1811024f;
		txtWO_QUANTITY1.Left = 3.378f;
		txtWO_QUANTITY1.Name = "txtWO_QUANTITY1";
		txtWO_QUANTITY1.OutputFormat = resources.GetString("txtWO_QUANTITY1.OutputFormat");
		txtWO_QUANTITY1.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY1.Text = "WO_QUANTITY1";
		txtWO_QUANTITY1.Top = 3.853f;
		txtWO_QUANTITY1.Width = 0.3996062f;
		txtTOTAL_SEQ_NO.DataField = "TOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.Height = 0.1562992f;
		txtTOTAL_SEQ_NO.Left = 1.669001f;
		txtTOTAL_SEQ_NO.Name = "txtTOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.OutputFormat = resources.GetString("txtTOTAL_SEQ_NO.OutputFormat");
		txtTOTAL_SEQ_NO.Style = "background-color: Transparent; font-size: 11pt; font-weight: bold; text-align: left; vertical-align: middle; ddo-font-vertical: none";
		txtTOTAL_SEQ_NO.Text = "TOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.Top = 2.614f;
		txtTOTAL_SEQ_NO.Width = 2.049f;
		txtMODEL_SUFFIX1.CanGrow = false;
		txtMODEL_SUFFIX1.DataField = "MODEL_SUFFIX1";
		txtMODEL_SUFFIX1.Height = 0.1811024f;
		txtMODEL_SUFFIX1.Left = 1.719f;
		txtMODEL_SUFFIX1.Name = "txtMODEL_SUFFIX1";
		txtMODEL_SUFFIX1.OutputFormat = resources.GetString("txtMODEL_SUFFIX1.OutputFormat");
		txtMODEL_SUFFIX1.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtMODEL_SUFFIX1.Text = "MODEL_SUFFIX1";
		txtMODEL_SUFFIX1.Top = 3.853f;
		txtMODEL_SUFFIX1.Width = 1.315355f;
		txtWORKER_ORDER1.CanGrow = false;
		txtWORKER_ORDER1.DataField = "WORKER_ORDER1";
		txtWORKER_ORDER1.Height = 0.1811024f;
		txtWORKER_ORDER1.Left = 0.4540005f;
		txtWORKER_ORDER1.Name = "txtWORKER_ORDER1";
		txtWORKER_ORDER1.OutputFormat = resources.GetString("txtWORKER_ORDER1.OutputFormat");
		txtWORKER_ORDER1.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWORKER_ORDER1.Text = "WORKER_ORDER1";
		txtWORKER_ORDER1.Top = 3.853f;
		txtWORKER_ORDER1.Width = 0.9145666f;
		label1.Height = 0.1562992f;
		label1.HyperLink = null;
		label1.Left = 0.419f;
		label1.Name = "label1";
		label1.Style = "font-family: Arial; font-size: 8pt";
		label1.Text = "Line";
		label1.Top = 0.919f;
		label1.Width = 1.012268f;
		label2.Height = 0.1562992f;
		label2.HyperLink = null;
		label2.Left = 0.4190006f;
		label2.Name = "label2";
		label2.Style = "font-family: Arial; font-size: 8pt";
		label2.Text = "Qty";
		label2.Top = 1.816f;
		label2.Width = 1.012268f;
		label3.Height = 0.1562992f;
		label3.HyperLink = null;
		label3.Left = 0.4190006f;
		label3.Name = "label3";
		label3.Style = "font-size: 8pt";
		label3.Text = "Made by";
		label3.Top = 2.864f;
		label3.Width = 1.012268f;
		label4.Height = 0.1562992f;
		label4.HyperLink = null;
		label4.Left = 0.4190006f;
		label4.Name = "label4";
		label4.Style = "font-family: Arial; font-size: 8pt";
		label4.Text = "Carrier ID";
		label4.Top = 2.367f;
		label4.Width = 1.012268f;
		label5.Height = 0.1562992f;
		label5.HyperLink = null;
		label5.Left = 0.4190006f;
		label5.Name = "label5";
		label5.Style = "font-family: Arial; font-size: 8pt";
		label5.Text = "To Locator Group";
		label5.Top = 2.11f;
		label5.Width = 1.012268f;
		label6.Height = 0.1562992f;
		label6.HyperLink = null;
		label6.Left = 0.4190006f;
		label6.Name = "label6";
		label6.Style = "font-family: Arial; font-size: 8pt";
		label6.Text = "Seq No";
		label6.Top = 2.614f;
		label6.Width = 1.012268f;
		label8.Height = 0.1562992f;
		label8.HyperLink = null;
		label8.Left = 0.419f;
		label8.Name = "label8";
		label8.Style = "font-family: Arial; font-size: 8pt";
		label8.Text = "Part No";
		label8.Top = 1.357f;
		label8.Width = 1.012268f;
		label9.Height = 0.1562992f;
		label9.HyperLink = null;
		label9.Left = 0.4190006f;
		label9.Name = "label9";
		label9.Style = "font-family: Arial; font-size: 8pt";
		label9.Text = "PST";
		label9.Top = 3.348f;
		label9.Width = 1.012268f;
		label10.Height = 0.1850394f;
		label10.HyperLink = null;
		label10.Left = 3.378f;
		label10.Name = "label10";
		label10.Style = "font-size: 9.75pt; font-weight: bold; text-align: center";
		label10.Text = "Qty";
		label10.Top = 3.594f;
		label10.Width = 0.3996062f;
		label12.Height = 0.1811024f;
		label12.HyperLink = null;
		label12.Left = 1.719f;
		label12.Name = "label12";
		label12.Style = "font-size: 9.75pt; font-weight: bold; text-align: center";
		label12.Text = "Model.Suffix";
		label12.Top = 3.594f;
		label12.Width = 1.315355f;
		label13.Height = 0.1811024f;
		label13.HyperLink = null;
		label13.Left = 0.4540005f;
		label13.Name = "label13";
		label13.Style = "font-size: 9.75pt; font-weight: bold; text-align: center";
		label13.Text = "W/O";
		label13.Top = 3.594f;
		label13.Width = 0.9145666f;
		label15.Height = 0.202362f;
		label15.HyperLink = null;
		label15.Left = 0.2910004f;
		label15.Name = "label15";
		label15.Style = "font-size: 10pt; font-weight: normal; vertical-align: middle";
		label15.Text = "Printed Date : ";
		label15.Top = 5.093999f;
		label15.Width = 1.140158f;
		txtPrintedDate.CanGrow = false;
		txtPrintedDate.DataField = "PRINTED_DATE";
		txtPrintedDate.Height = 0.222047f;
		txtPrintedDate.Left = 1.369f;
		txtPrintedDate.Name = "txtPrintedDate";
		txtPrintedDate.Style = "background-color: Transparent; font-size: 10pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtPrintedDate.Text = "PRINTED_DATE";
		txtPrintedDate.Top = 5.093999f;
		txtPrintedDate.Width = 2.28937f;
		label17.Height = 0.202362f;
		label17.HyperLink = null;
		label17.Left = 2.885001f;
		label17.Name = "label17";
		label17.Style = "font-family: Arial; font-size: 9pt; font-weight: normal; vertical-align: middle";
		label17.Text = "Printed by MCS";
		label17.Top = 5.093999f;
		label17.Width = 1.014961f;
		txtWO_QUANTITY2.CanGrow = false;
		txtWO_QUANTITY2.DataField = "WO_QUANTITY2";
		txtWO_QUANTITY2.Height = 0.1811024f;
		txtWO_QUANTITY2.Left = 3.378f;
		txtWO_QUANTITY2.Name = "txtWO_QUANTITY2";
		txtWO_QUANTITY2.OutputFormat = resources.GetString("txtWO_QUANTITY2.OutputFormat");
		txtWO_QUANTITY2.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY2.Text = "WO_QUANTITY2";
		txtWO_QUANTITY2.Top = 4.113998f;
		txtWO_QUANTITY2.Width = 0.3996062f;
		textBox4.CanGrow = false;
		textBox4.DataField = "MODEL_SUFFIX2";
		textBox4.Height = 0.1811024f;
		textBox4.Left = 1.719f;
		textBox4.Name = "textBox4";
		textBox4.OutputFormat = resources.GetString("textBox4.OutputFormat");
		textBox4.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox4.Text = "MODEL_SUFFIX2";
		textBox4.Top = 4.113998f;
		textBox4.Width = 1.315355f;
		textBox5.CanGrow = false;
		textBox5.DataField = "WORKER_ORDER2";
		textBox5.Height = 0.1811024f;
		textBox5.Left = 0.4540005f;
		textBox5.Name = "textBox5";
		textBox5.OutputFormat = resources.GetString("textBox5.OutputFormat");
		textBox5.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox5.Text = "WORKER_ORDER2";
		textBox5.Top = 4.113998f;
		textBox5.Width = 0.9145666f;
		txtWO_QUANTITY3.CanGrow = false;
		txtWO_QUANTITY3.DataField = "WO_QUANTITY3";
		txtWO_QUANTITY3.Height = 0.1811024f;
		txtWO_QUANTITY3.Left = 3.378f;
		txtWO_QUANTITY3.Name = "txtWO_QUANTITY3";
		txtWO_QUANTITY3.OutputFormat = resources.GetString("txtWO_QUANTITY3.OutputFormat");
		txtWO_QUANTITY3.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY3.Text = "WO_QUANTITY3";
		txtWO_QUANTITY3.Top = 4.362999f;
		txtWO_QUANTITY3.Width = 0.3996062f;
		textBox9.CanGrow = false;
		textBox9.DataField = "MODEL_SUFFIX3";
		textBox9.Height = 0.1811024f;
		textBox9.Left = 1.719f;
		textBox9.Name = "textBox9";
		textBox9.OutputFormat = resources.GetString("textBox9.OutputFormat");
		textBox9.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox9.Text = "MODEL_SUFFIX3";
		textBox9.Top = 4.362999f;
		textBox9.Width = 1.315355f;
		textBox10.CanGrow = false;
		textBox10.DataField = "WORKER_ORDER3";
		textBox10.Height = 0.1811024f;
		textBox10.Left = 0.4540005f;
		textBox10.Name = "textBox10";
		textBox10.OutputFormat = resources.GetString("textBox10.OutputFormat");
		textBox10.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox10.Text = "WORKER_ORDER3";
		textBox10.Top = 4.362999f;
		textBox10.Width = 0.9145666f;
		txtWO_QUANTITY4.CanGrow = false;
		txtWO_QUANTITY4.DataField = "WO_QUANTITY4";
		txtWO_QUANTITY4.Height = 0.1811024f;
		txtWO_QUANTITY4.Left = 3.378f;
		txtWO_QUANTITY4.Name = "txtWO_QUANTITY4";
		txtWO_QUANTITY4.OutputFormat = resources.GetString("txtWO_QUANTITY4.OutputFormat");
		txtWO_QUANTITY4.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY4.Text = "WO_QUANTITY3";
		txtWO_QUANTITY4.Top = 4.612999f;
		txtWO_QUANTITY4.Width = 0.3996062f;
		textBox14.CanGrow = false;
		textBox14.DataField = "MODEL_SUFFIX4";
		textBox14.Height = 0.1811024f;
		textBox14.Left = 1.719f;
		textBox14.Name = "textBox14";
		textBox14.OutputFormat = resources.GetString("textBox14.OutputFormat");
		textBox14.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox14.Text = "MODEL_SUFFIX4";
		textBox14.Top = 4.612999f;
		textBox14.Width = 1.315355f;
		textBox15.CanGrow = false;
		textBox15.DataField = "WORKER_ORDER4";
		textBox15.Height = 0.1811024f;
		textBox15.Left = 0.4540005f;
		textBox15.Name = "textBox15";
		textBox15.OutputFormat = resources.GetString("textBox15.OutputFormat");
		textBox15.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox15.Text = "WORKER_ORDER4";
		textBox15.Top = 4.612999f;
		textBox15.Width = 0.9145666f;
		txtWO_QUANTITY5.CanGrow = false;
		txtWO_QUANTITY5.DataField = "WO_QUANTITY5";
		txtWO_QUANTITY5.Height = 0.1811026f;
		txtWO_QUANTITY5.Left = 3.378f;
		txtWO_QUANTITY5.Name = "txtWO_QUANTITY5";
		txtWO_QUANTITY5.OutputFormat = resources.GetString("txtWO_QUANTITY5.OutputFormat");
		txtWO_QUANTITY5.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY5.Text = "WO_QUANTITY3";
		txtWO_QUANTITY5.Top = 4.862999f;
		txtWO_QUANTITY5.Width = 0.3996062f;
		textBox19.CanGrow = false;
		textBox19.DataField = "MODEL_SUFFIX5";
		textBox19.Height = 0.1811024f;
		textBox19.Left = 1.719f;
		textBox19.Name = "textBox19";
		textBox19.OutputFormat = resources.GetString("textBox19.OutputFormat");
		textBox19.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox19.Text = "MODEL_SUFFIX5";
		textBox19.Top = 4.862999f;
		textBox19.Width = 1.315355f;
		textBox20.CanGrow = false;
		textBox20.DataField = "WORKER_ORDER5";
		textBox20.Height = 0.1811024f;
		textBox20.Left = 0.4540005f;
		textBox20.Name = "textBox20";
		textBox20.OutputFormat = resources.GetString("textBox20.OutputFormat");
		textBox20.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox20.Text = "WORKER_ORDER5";
		textBox20.Top = 4.862999f;
		textBox20.Width = 0.9145666f;
		barcode.BackColor = Color.FromArgb(255, 255, 255);
		barcode.DataField = "QRCODE_VALUE";
		barcode.Font = new Font("Courier New", 8f);
		barcode.Height = 0.5798611f;
		barcode.Left = 0.2910004f;
		barcode.Name = "barcode";
		barcode.QuietZoneBottom = 0f;
		barcode.QuietZoneLeft = 0f;
		barcode.QuietZoneRight = 0f;
		barcode.QuietZoneTop = 0f;
		barcode.Style = BarCodeStyle.QRCode;
		barcode.Text = "barcod";
		barcode.Top = 0.201f;
		barcode.Width = 0.5798611f;
		txtLOCATOR_GROUP.DataField = "LOCATOR_GROUP";
		txtLOCATOR_GROUP.Height = 0.1562992f;
		txtLOCATOR_GROUP.Left = 1.669001f;
		txtLOCATOR_GROUP.Name = "txtLOCATOR_GROUP";
		txtLOCATOR_GROUP.Style = "background-color: Transparent; font-size: 11pt; font-weight: bold; text-align: left; vertical-align: middle";
		txtLOCATOR_GROUP.Text = "LOCATOR_GROUP";
		txtLOCATOR_GROUP.Top = 2.11f;
		txtLOCATOR_GROUP.Width = 2.049f;
		txtLINE.DataField = "LINE";
		txtLINE.Height = 0.2468504f;
		txtLINE.Left = 1.669001f;
		txtLINE.Name = "txtLINE";
		txtLINE.Style = "background-color: Transparent; font-size: 12pt; font-weight: bold; text-align: left; vertical-align: middle";
		txtLINE.Text = "LINE";
		txtLINE.Top = 0.8680001f;
		txtLINE.Width = 2.049f;
		txtPART_NO.DataField = "PART_NO";
		txtPART_NO.Height = 0.3100002f;
		txtPART_NO.Left = 1.669001f;
		txtPART_NO.Name = "txtPART_NO";
		txtPART_NO.Style = "background-color: Transparent; font-size: 20pt; font-weight: bold; text-align: left; vertical-align: middle";
		txtPART_NO.Text = "PART_NO";
		txtPART_NO.Top = 1.194f;
		txtPART_NO.Width = 2.049f;
		line9.Height = 0.9199982f;
		line9.Left = 1.544001f;
		line9.LineWeight = 1.5f;
		line9.Name = "line9";
		line9.Top = 4.144f;
		line9.Width = 0f;
		line9.X1 = 1.544001f;
		line9.X2 = 1.544001f;
		line9.Y1 = 4.144f;
		line9.Y2 = 5.063998f;
		line13.Height = 0f;
		line13.Left = 0.3100004f;
		line13.LineWeight = 1.5f;
		line13.Name = "line13";
		line13.Top = 3.814f;
		line13.Width = 3.589999f;
		line13.X1 = 0.3100004f;
		line13.X2 = 3.9f;
		line13.Y1 = 3.814f;
		line13.Y2 = 3.814f;
		txtSheetID.CanGrow = false;
		txtSheetID.DataField = "SHEET_ID";
		txtSheetID.Height = 0.222047f;
		txtSheetID.Left = 0.9540005f;
		txtSheetID.Name = "txtSheetID";
		txtSheetID.Style = "background-color: Transparent; font-size: 10pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtSheetID.Text = "SHEET_ID";
		txtSheetID.Top = 0.5790001f;
		txtSheetID.Width = 2.214567f;
		txtPST.CanGrow = false;
		txtPST.DataField = "PST";
		txtPST.Height = 0.1562992f;
		txtPST.Left = 1.669001f;
		txtPST.Name = "txtPST";
		txtPST.Style = "background-color: Transparent; font-family: Arial; font-size: 11pt; font-weight: bold; text-align: left; vertical-align: top; ddo-char-set: 1";
		txtPST.Text = "2019-01-30 PM  12:12:11";
		txtPST.Top = 3.348f;
		txtPST.Width = 2.049f;
		label11.Height = 0.3645833f;
		label11.HyperLink = null;
		label11.Left = 1.033f;
		label11.Name = "label11";
		label11.Style = "font-size: 16pt";
		label11.Text = "WIP Delivery Sheet";
		label11.Top = 0.192f;
		label11.Width = 2.136f;
		line7.Height = 0f;
		line7.Left = 0.3010006f;
		line7.LineWeight = 1.5f;
		line7.Name = "line7";
		line7.Top = 1.144f;
		line7.Width = 3.598999f;
		line7.X1 = 0.3010006f;
		line7.X2 = 3.9f;
		line7.Y1 = 1.144f;
		line7.Y2 = 1.144f;
		line3.Height = 1.493998f;
		line3.Left = 3.244f;
		line3.LineWeight = 1.5f;
		line3.Name = "line3";
		line3.Top = 3.57f;
		line3.Width = 0f;
		line3.X1 = 3.244f;
		line3.X2 = 3.244f;
		line3.Y1 = 3.57f;
		line3.Y2 = 5.063998f;
		line4.Height = 0f;
		line4.Left = 0.3100004f;
		line4.LineWeight = 1.5f;
		line4.Name = "line4";
		line4.Top = 4.314002f;
		line4.Width = 3.589999f;
		line4.X1 = 0.3100004f;
		line4.X2 = 3.9f;
		line4.Y1 = 4.314002f;
		line4.Y2 = 4.314002f;
		line11.Height = 0f;
		line11.Left = 0.3100004f;
		line11.LineWeight = 1.5f;
		line11.Name = "line11";
		line11.Top = 4.063996f;
		line11.Width = 3.589999f;
		line11.X1 = 0.3100004f;
		line11.X2 = 3.9f;
		line11.Y1 = 4.063996f;
		line11.Y2 = 4.063996f;
		line12.Height = 0f;
		line12.Left = 0.3100004f;
		line12.LineWeight = 1.5f;
		line12.Name = "line12";
		line12.Top = 4.564002f;
		line12.Width = 3.589999f;
		line12.X1 = 0.3100004f;
		line12.X2 = 3.9f;
		line12.Y1 = 4.564002f;
		line12.Y2 = 4.564002f;
		line15.Height = 0f;
		line15.Left = 0.3100004f;
		line15.LineWeight = 1.5f;
		line15.Name = "line15";
		line15.Top = 4.814002f;
		line15.Width = 3.589999f;
		line15.X1 = 0.3100004f;
		line15.X2 = 3.9f;
		line15.Y1 = 4.814002f;
		line15.Y2 = 4.814002f;
		line17.Height = 3.3f;
		line17.Left = 1.544001f;
		line17.LineWeight = 1.5f;
		line17.Name = "line17";
		line17.Top = 0.844f;
		line17.Width = 0f;
		line17.X1 = 1.544001f;
		line17.X2 = 1.544001f;
		line17.Y1 = 0.844f;
		line17.Y2 = 4.144f;
		label14.Height = 0.1562992f;
		label14.HyperLink = null;
		label14.Left = 0.4190006f;
		label14.Name = "label14";
		label14.Style = "font-family: Arial; font-size: 8pt";
		label14.Text = "Inspection Flag";
		label14.Top = 3.108001f;
		label14.Width = 1.012268f;
		line1.Height = 0f;
		line1.Left = 0.3100004f;
		line1.LineWeight = 1.5f;
		line1.Name = "line1";
		line1.Top = 1.744f;
		line1.Width = 3.589999f;
		line1.X1 = 0.3100004f;
		line1.X2 = 3.9f;
		line1.Y1 = 1.744f;
		line1.Y2 = 1.744f;
		line6.Height = 0f;
		line6.Left = 0.3100004f;
		line6.LineWeight = 1.5f;
		line6.Name = "line6";
		line6.Top = 2.064f;
		line6.Width = 3.589999f;
		line6.X1 = 0.3100004f;
		line6.X2 = 3.9f;
		line6.Y1 = 2.064f;
		line6.Y2 = 2.064f;
		line14.Height = 0f;
		line14.Left = 0.3100004f;
		line14.LineWeight = 1.5f;
		line14.Name = "line14";
		line14.Top = 2.314f;
		line14.Width = 3.589999f;
		line14.X1 = 0.3100004f;
		line14.X2 = 3.9f;
		line14.Y1 = 2.314f;
		line14.Y2 = 2.314f;
		line2.Height = 0f;
		line2.Left = 0.3100004f;
		line2.LineWeight = 1.5f;
		line2.Name = "line2";
		line2.Top = 2.564f;
		line2.Width = 3.589999f;
		line2.X1 = 0.3100004f;
		line2.X2 = 3.9f;
		line2.Y1 = 2.564f;
		line2.Y2 = 2.564f;
		line5.Height = 0f;
		line5.Left = 0.3100004f;
		line5.LineWeight = 1.5f;
		line5.Name = "line5";
		line5.Top = 2.814f;
		line5.Width = 3.589999f;
		line5.X1 = 0.3100004f;
		line5.X2 = 3.9f;
		line5.Y1 = 2.814f;
		line5.Y2 = 2.814f;
		line8.Height = 0f;
		line8.Left = 0.3100004f;
		line8.LineWeight = 1.5f;
		line8.Name = "line8";
		line8.Top = 3.064f;
		line8.Width = 3.589999f;
		line8.X1 = 0.3100004f;
		line8.X2 = 3.9f;
		line8.Y1 = 3.064f;
		line8.Y2 = 3.064f;
		line16.Height = 0f;
		line16.Left = 0.3100004f;
		line16.LineWeight = 1.5f;
		line16.Name = "line16";
		line16.Top = 3.314f;
		line16.Width = 3.589999f;
		line16.X1 = 0.3100004f;
		line16.X2 = 3.9f;
		line16.Y1 = 3.314f;
		line16.Y2 = 3.314f;
		line18.Height = 0f;
		line18.Left = 0.3100004f;
		line18.LineWeight = 1.5f;
		line18.Name = "line18";
		line18.Top = 3.564f;
		line18.Width = 3.589999f;
		line18.X1 = 0.3100004f;
		line18.X2 = 3.9f;
		line18.Y1 = 3.564f;
		line18.Y2 = 3.564f;
		label7.DataField = "PB";
		label7.Height = 0.7180001f;
		label7.HyperLink = null;
		label7.Left = 3.209f;
		label7.Name = "label7";
		label7.Style = "font-family: Arial; font-size: 50pt; font-weight: bold; text-align: center; vertical-align: bottom; ddo-char-set: 1";
		label7.Text = "P";
		label7.Top = 0.191f;
		label7.Width = 0.6881945f;
		base.MasterReport = false;
		base.PageSettings.Margins.Bottom = 0f;
		base.PageSettings.Margins.Left = 0f;
		base.PageSettings.Margins.Right = 0f;
		base.PageSettings.Margins.Top = 0f;
		base.PageSettings.PaperHeight = 11f;
		base.PageSettings.PaperWidth = 8.5f;
		base.PrintWidth = 8.239583f;
		base.Sections.Add(detail);
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; color: Black", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: italic", "Heading2", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
		((ISupportInitialize)txtTOTAL_QUANTITY).EndInit();
		((ISupportInitialize)txtCARRIER_ID).EndInit();
		((ISupportInitialize)txtMADE_BY).EndInit();
		((ISupportInitialize)txtDESCRIPTION).EndInit();
		((ISupportInitialize)txtINSPECTION_FLAG).EndInit();
		((ISupportInitialize)txtWO_QUANTITY1).EndInit();
		((ISupportInitialize)txtTOTAL_SEQ_NO).EndInit();
		((ISupportInitialize)txtMODEL_SUFFIX1).EndInit();
		((ISupportInitialize)txtWORKER_ORDER1).EndInit();
		((ISupportInitialize)label1).EndInit();
		((ISupportInitialize)label2).EndInit();
		((ISupportInitialize)label3).EndInit();
		((ISupportInitialize)label4).EndInit();
		((ISupportInitialize)label5).EndInit();
		((ISupportInitialize)label6).EndInit();
		((ISupportInitialize)label8).EndInit();
		((ISupportInitialize)label9).EndInit();
		((ISupportInitialize)label10).EndInit();
		((ISupportInitialize)label12).EndInit();
		((ISupportInitialize)label13).EndInit();
		((ISupportInitialize)label15).EndInit();
		((ISupportInitialize)txtPrintedDate).EndInit();
		((ISupportInitialize)label17).EndInit();
		((ISupportInitialize)txtWO_QUANTITY2).EndInit();
		((ISupportInitialize)textBox4).EndInit();
		((ISupportInitialize)textBox5).EndInit();
		((ISupportInitialize)txtWO_QUANTITY3).EndInit();
		((ISupportInitialize)textBox9).EndInit();
		((ISupportInitialize)textBox10).EndInit();
		((ISupportInitialize)txtWO_QUANTITY4).EndInit();
		((ISupportInitialize)textBox14).EndInit();
		((ISupportInitialize)textBox15).EndInit();
		((ISupportInitialize)txtWO_QUANTITY5).EndInit();
		((ISupportInitialize)textBox19).EndInit();
		((ISupportInitialize)textBox20).EndInit();
		((ISupportInitialize)txtLOCATOR_GROUP).EndInit();
		((ISupportInitialize)txtLINE).EndInit();
		((ISupportInitialize)txtPART_NO).EndInit();
		((ISupportInitialize)txtSheetID).EndInit();
		((ISupportInitialize)txtPST).EndInit();
		((ISupportInitialize)label11).EndInit();
		((ISupportInitialize)label14).EndInit();
		((ISupportInitialize)label7).EndInit();
		((ISupportInitialize)this).EndInit();
	}
}
