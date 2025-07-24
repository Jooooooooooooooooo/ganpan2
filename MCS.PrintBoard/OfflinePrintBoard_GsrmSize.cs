using System.ComponentModel;
using System.Drawing;
using System.Resources;
using DDCssLib;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Controls;
using GrapeCity.ActiveReports.Document.Section;
using GrapeCity.ActiveReports.SectionReportModel;

namespace MCS.PrintBoard;

public class OfflinePrintBoard_GsrmSize : SectionReport
{
	private TextBox txtTOTAL_SEQ_NO;

	private Detail Detail;

	private TextBox txtTOTAL_QUANTITY;

	private TextBox txtCARRIER_ID;

	private TextBox txtMADE_BY;

	private TextBox txtDESCRIPTION;

	private TextBox txtINSPECTION_FLAG;

	private TextBox txtWO_QUANTITY1;

	private TextBox txtMODEL_SUFFIX1;

	private TextBox txtWORKER_ORDER1;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

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

	private Line line1;

	private Shape shape2;

	private Shape shape1;

	private Line line2;

	private Line line3;

	private Line line4;

	private Line line5;

	private Line line6;

	private Line line8;

	private Line line9;

	private Line line10;

	private Line line11;

	private Line line12;

	private Line line13;

	private TextBox txtSheetID;

	private TextBox txtPST;

	public OfflinePrintBoard_GsrmSize()
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

	public void InitializeComponent()
	{
		ResourceManager resources = new ResourceManager(typeof(OfflinePrintBoard_GsrmSize));
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
		label7 = new Label();
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
		line1 = new Line();
		shape1 = new Shape();
		shape2 = new Shape();
		line2 = new Line();
		line3 = new Line();
		line4 = new Line();
		line5 = new Line();
		line6 = new Line();
		line8 = new Line();
		line9 = new Line();
		line10 = new Line();
		line11 = new Line();
		line12 = new Line();
		line13 = new Line();
		txtSheetID = new TextBox();
		txtPST = new TextBox();
		Detail = new Detail();
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
		((ISupportInitialize)label7).BeginInit();
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
		((ISupportInitialize)this).BeginInit();
		txtTOTAL_QUANTITY.CanGrow = false;
		txtTOTAL_QUANTITY.DataField = "TOTAL_QUANTITY";
		txtTOTAL_QUANTITY.Height = 0.2f;
		txtTOTAL_QUANTITY.Left = 0.2719725f;
		txtTOTAL_QUANTITY.Name = "txtTOTAL_QUANTITY";
		txtTOTAL_QUANTITY.Style = "background-color: Transparent; font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtTOTAL_QUANTITY.Text = "TOTAL_QUANTITY";
		txtTOTAL_QUANTITY.Top = 1.069929f;
		txtTOTAL_QUANTITY.Width = 1.052756f;
		txtCARRIER_ID.DataField = "CARRIER_ID";
		txtCARRIER_ID.Height = 0.2468503f;
		txtCARRIER_ID.Left = 0.2404764f;
		txtCARRIER_ID.Name = "txtCARRIER_ID";
		txtCARRIER_ID.Style = "background-color: Transparent; font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtCARRIER_ID.Text = "CARRIER_ID";
		txtCARRIER_ID.Top = 2.121504f;
		txtCARRIER_ID.Width = 1.079921f;
		txtMADE_BY.CanGrow = false;
		txtMADE_BY.DataField = "MADE_BY";
		txtMADE_BY.Height = 0.2570866f;
		txtMADE_BY.Left = 0.2703975f;
		txtMADE_BY.Name = "txtMADE_BY";
		txtMADE_BY.OutputFormat = resources.GetString("txtMADE_BY.OutputFormat");
		txtMADE_BY.Style = "background-color: Transparent; font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-font-vertical: true";
		txtMADE_BY.Text = "MADE_BY";
		txtMADE_BY.Top = 1.593551f;
		txtMADE_BY.Width = 1.029528f;
		txtDESCRIPTION.CanGrow = false;
		txtDESCRIPTION.DataField = "DESCRIPTION";
		txtDESCRIPTION.Height = 0.2625981f;
		txtDESCRIPTION.Left = 1.334964f;
		txtDESCRIPTION.Name = "txtDESCRIPTION";
		txtDESCRIPTION.OutputFormat = resources.GetString("txtDESCRIPTION.OutputFormat");
		txtDESCRIPTION.Style = "background-color: Transparent; font-size: 14.25pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtDESCRIPTION.Text = "DESCRIPTION";
		txtDESCRIPTION.Top = 1.008906f;
		txtDESCRIPTION.Width = 3.041733f;
		txtINSPECTION_FLAG.CanGrow = false;
		txtINSPECTION_FLAG.DataField = "INSPECTION_FLAG";
		txtINSPECTION_FLAG.Height = 0.2f;
		txtINSPECTION_FLAG.Left = 1.353467f;
		txtINSPECTION_FLAG.Name = "txtINSPECTION_FLAG";
		txtINSPECTION_FLAG.OutputFormat = resources.GetString("txtINSPECTION_FLAG.OutputFormat");
		txtINSPECTION_FLAG.Style = "background-color: Transparent; font-size: 11pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		txtINSPECTION_FLAG.Text = "INSPECTION_FLAG";
		txtINSPECTION_FLAG.Top = 2.088039f;
		txtINSPECTION_FLAG.Width = 1.343307f;
		txtWO_QUANTITY1.CanGrow = false;
		txtWO_QUANTITY1.DataField = "WO_QUANTITY1";
		txtWO_QUANTITY1.Height = 0.1811024f;
		txtWO_QUANTITY1.Left = 2.743232f;
		txtWO_QUANTITY1.Name = "txtWO_QUANTITY1";
		txtWO_QUANTITY1.OutputFormat = resources.GetString("txtWO_QUANTITY1.OutputFormat");
		txtWO_QUANTITY1.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY1.Text = "WO_QUANTITY1";
		txtWO_QUANTITY1.Top = 1.693551f;
		txtWO_QUANTITY1.Width = 0.3996062f;
		txtTOTAL_SEQ_NO.DataField = "TOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.Height = 0.3354331f;
		txtTOTAL_SEQ_NO.Left = 1.557404f;
		txtTOTAL_SEQ_NO.Name = "txtTOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.OutputFormat = resources.GetString("txtTOTAL_SEQ_NO.OutputFormat");
		txtTOTAL_SEQ_NO.Style = "background-color: Transparent; font-size: 15.75pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-font-vertical: none";
		txtTOTAL_SEQ_NO.Text = "TOTAL_SEQ_NO";
		txtTOTAL_SEQ_NO.Top = 0.4864648f;
		txtTOTAL_SEQ_NO.Width = 2.834252f;
		txtMODEL_SUFFIX1.CanGrow = false;
		txtMODEL_SUFFIX1.DataField = "MODEL_SUFFIX1";
		txtMODEL_SUFFIX1.Height = 0.1811024f;
		txtMODEL_SUFFIX1.Left = 3.208184f;
		txtMODEL_SUFFIX1.Name = "txtMODEL_SUFFIX1";
		txtMODEL_SUFFIX1.OutputFormat = resources.GetString("txtMODEL_SUFFIX1.OutputFormat");
		txtMODEL_SUFFIX1.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		txtMODEL_SUFFIX1.Text = "MODEL_SUFFIX1";
		txtMODEL_SUFFIX1.Top = 1.685677f;
		txtMODEL_SUFFIX1.Width = 1.315355f;
		txtWORKER_ORDER1.CanGrow = false;
		txtWORKER_ORDER1.DataField = "WORKER_ORDER1";
		txtWORKER_ORDER1.Height = 0.1811024f;
		txtWORKER_ORDER1.Left = 4.552669f;
		txtWORKER_ORDER1.Name = "txtWORKER_ORDER1";
		txtWORKER_ORDER1.OutputFormat = resources.GetString("txtWORKER_ORDER1.OutputFormat");
		txtWORKER_ORDER1.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWORKER_ORDER1.Text = "WORKER_ORDER1";
		txtWORKER_ORDER1.Top = 1.677803f;
		txtWORKER_ORDER1.Width = 0.9145669f;
		label1.Height = 0.1562992f;
		label1.HyperLink = null;
		label1.Left = 0.2444137f;
		label1.Name = "label1";
		label1.Style = "font-family: Arial; font-size: 6.75pt";
		label1.Text = "Line";
		label1.Top = 0.417567f;
		label1.Width = 0.9582678f;
		label2.Height = 0.1562992f;
		label2.HyperLink = null;
		label2.Left = 0.2444137f;
		label2.Name = "label2";
		label2.Style = "font-family: Arial; font-size: 6.75pt";
		label2.Text = "Qty";
		label2.Top = 0.890008f;
		label2.Width = 0.958268f;
		label3.Height = 0.1562992f;
		label3.HyperLink = null;
		label3.Left = 0.2444137f;
		label3.Name = "label3";
		label3.Style = "font-size: 6.75pt";
		label3.Text = "Made by";
		label3.Top = 1.409693f;
		label3.Width = 0.958268f;
		label4.Height = 0.1562992f;
		label4.HyperLink = null;
		label4.Left = 0.2444137f;
		label4.Name = "label4";
		label4.Style = "font-family: Arial; font-size: 6.75pt";
		label4.Text = "Carrier ID";
		label4.Top = 1.964811f;
		label4.Width = 0.958268f;
		label5.Height = 0.1562992f;
		label5.HyperLink = null;
		label5.Left = 0.2562245f;
		label5.Name = "label5";
		label5.Style = "font-family: Arial; font-size: 6.75pt";
		label5.Text = "To Locator Group";
		label5.Top = 2.472685f;
		label5.Width = 0.958268f;
		label6.Height = 0.1562992f;
		label6.HyperLink = null;
		label6.Left = 1.363311f;
		label6.Name = "label6";
		label6.Style = "font-family: Arial; font-size: 6.75pt";
		label6.Text = "Seq No";
		label6.Top = 0.3923701f;
		label6.Width = 0.958268f;
		label7.Height = 0.1562992f;
		label7.HyperLink = null;
		label7.Left = 1.371184f;
		label7.Name = "label7";
		label7.Style = "font-family: Arial; font-size: 6.75pt";
		label7.Text = "Description";
		label7.Top = 0.8884332f;
		label7.Width = 0.958268f;
		label8.Height = 0.1562992f;
		label8.HyperLink = null;
		label8.Left = 1.371184f;
		label8.Name = "label8";
		label8.Style = "font-family: Arial; font-size: 6.75pt";
		label8.Text = "Part No";
		label8.Top = 1.400244f;
		label8.Width = 0.958268f;
		label9.Height = 0.1562992f;
		label9.HyperLink = null;
		label9.Left = 1.375121f;
		label9.Name = "label9";
		label9.Style = "font-family: Arial; font-size: 6.75pt";
		label9.Text = "PST";
		label9.Top = 2.478984f;
		label9.Width = 0.958268f;
		label10.Height = 0.1850394f;
		label10.HyperLink = null;
		label10.Left = 2.743232f;
		label10.Name = "label10";
		label10.Style = "font-size: 9.75pt; font-weight: bold; text-align: center";
		label10.Text = "Qty";
		label10.Top = 1.427016f;
		label10.Width = 0.3996062f;
		label12.Height = 0.1811024f;
		label12.HyperLink = null;
		label12.Left = 3.208184f;
		label12.Name = "label12";
		label12.Style = "font-size: 9.75pt; font-weight: bold; text-align: center";
		label12.Text = "Model.Suffix";
		label12.Top = 1.423079f;
		label12.Width = 1.315355f;
		label13.Height = 0.1811024f;
		label13.HyperLink = null;
		label13.Left = 4.552669f;
		label13.Name = "label13";
		label13.Style = "font-size: 9.75pt; font-weight: bold; text-align: center";
		label13.Text = "W/O";
		label13.Top = 1.419142f;
		label13.Width = 0.9145669f;
		label15.Height = 0.202362f;
		label15.HyperLink = null;
		label15.Left = 0.3318151f;
		label15.Name = "label15";
		label15.Style = "font-size: 11.25pt; font-weight: normal; vertical-align: middle";
		label15.Text = "Printed Date : ";
		label15.Top = 3.16363f;
		label15.Width = 1.140157f;
		txtPrintedDate.CanGrow = false;
		txtPrintedDate.DataField = "PRINTED_DATE";
		txtPrintedDate.Height = 0.222047f;
		txtPrintedDate.Left = 1.41961f;
		txtPrintedDate.Name = "txtPrintedDate";
		txtPrintedDate.Style = "background-color: Transparent; font-size: 11.25pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtPrintedDate.Text = "PRINTED_DATE";
		txtPrintedDate.Top = 3.16363f;
		txtPrintedDate.Width = 2.28937f;
		label17.Height = 0.202362f;
		label17.HyperLink = null;
		label17.Left = 4.513692f;
		label17.Name = "label17";
		label17.Style = "font-family: Arial; font-size: 9pt; font-weight: normal; vertical-align: middle";
		label17.Text = "Printed by MCS";
		label17.Top = 3.578591f;
		label17.Width = 1.014961f;
		txtWO_QUANTITY2.CanGrow = false;
		txtWO_QUANTITY2.DataField = "WO_QUANTITY2";
		txtWO_QUANTITY2.Height = 0.1811024f;
		txtWO_QUANTITY2.Left = 2.743232f;
		txtWO_QUANTITY2.Name = "txtWO_QUANTITY2";
		txtWO_QUANTITY2.OutputFormat = resources.GetString("txtWO_QUANTITY2.OutputFormat");
		txtWO_QUANTITY2.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY2.Text = "WO_QUANTITY2";
		txtWO_QUANTITY2.Top = 1.950638f;
		txtWO_QUANTITY2.Width = 0.3996062f;
		textBox4.CanGrow = false;
		textBox4.DataField = "MODEL_SUFFIX2";
		textBox4.Height = 0.1811024f;
		textBox4.Left = 3.208184f;
		textBox4.Name = "textBox4";
		textBox4.OutputFormat = resources.GetString("textBox4.OutputFormat");
		textBox4.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		textBox4.Text = "MODEL_SUFFIX2";
		textBox4.Top = 1.958512f;
		textBox4.Width = 1.315355f;
		textBox5.CanGrow = false;
		textBox5.DataField = "WORKER_ORDER2";
		textBox5.Height = 0.1811024f;
		textBox5.Left = 4.552669f;
		textBox5.Name = "textBox5";
		textBox5.OutputFormat = resources.GetString("textBox5.OutputFormat");
		textBox5.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox5.Text = "WORKER_ORDER2";
		textBox5.Top = 1.942764f;
		textBox5.Width = 0.9145669f;
		txtWO_QUANTITY3.CanGrow = false;
		txtWO_QUANTITY3.DataField = "WO_QUANTITY3";
		txtWO_QUANTITY3.Height = 0.1811024f;
		txtWO_QUANTITY3.Left = 2.743232f;
		txtWO_QUANTITY3.Name = "txtWO_QUANTITY3";
		txtWO_QUANTITY3.OutputFormat = resources.GetString("txtWO_QUANTITY3.OutputFormat");
		txtWO_QUANTITY3.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY3.Text = "WO_QUANTITY3";
		txtWO_QUANTITY3.Top = 2.206543f;
		txtWO_QUANTITY3.Width = 0.3996062f;
		textBox9.CanGrow = false;
		textBox9.DataField = "MODEL_SUFFIX3";
		textBox9.Height = 0.1811024f;
		textBox9.Left = 3.208184f;
		textBox9.Name = "textBox9";
		textBox9.OutputFormat = resources.GetString("textBox9.OutputFormat");
		textBox9.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		textBox9.Text = "MODEL_SUFFIX3";
		textBox9.Top = 2.21048f;
		textBox9.Width = 1.315355f;
		textBox10.CanGrow = false;
		textBox10.DataField = "WORKER_ORDER3";
		textBox10.Height = 0.1811024f;
		textBox10.Left = 4.552669f;
		textBox10.Name = "textBox10";
		textBox10.OutputFormat = resources.GetString("textBox10.OutputFormat");
		textBox10.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox10.Text = "WORKER_ORDER3";
		textBox10.Top = 2.202606f;
		textBox10.Width = 0.9145669f;
		txtWO_QUANTITY4.CanGrow = false;
		txtWO_QUANTITY4.DataField = "WO_QUANTITY4";
		txtWO_QUANTITY4.Height = 0.1811024f;
		txtWO_QUANTITY4.Left = 2.743232f;
		txtWO_QUANTITY4.Name = "txtWO_QUANTITY4";
		txtWO_QUANTITY4.OutputFormat = resources.GetString("txtWO_QUANTITY4.OutputFormat");
		txtWO_QUANTITY4.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY4.Text = "WO_QUANTITY3";
		txtWO_QUANTITY4.Top = 2.466386f;
		txtWO_QUANTITY4.Width = 0.3996062f;
		textBox14.CanGrow = false;
		textBox14.DataField = "MODEL_SUFFIX4";
		textBox14.Height = 0.1811024f;
		textBox14.Left = 3.208184f;
		textBox14.Name = "textBox14";
		textBox14.OutputFormat = resources.GetString("textBox14.OutputFormat");
		textBox14.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		textBox14.Text = "MODEL_SUFFIX4";
		textBox14.Top = 2.470323f;
		textBox14.Width = 1.315355f;
		textBox15.CanGrow = false;
		textBox15.DataField = "WORKER_ORDER4";
		textBox15.Height = 0.1811024f;
		textBox15.Left = 4.552669f;
		textBox15.Name = "textBox15";
		textBox15.OutputFormat = resources.GetString("textBox15.OutputFormat");
		textBox15.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox15.Text = "WORKER_ORDER4";
		textBox15.Top = 2.454575f;
		textBox15.Width = 0.9145669f;
		txtWO_QUANTITY5.CanGrow = false;
		txtWO_QUANTITY5.DataField = "WO_QUANTITY5";
		txtWO_QUANTITY5.Height = 0.1811026f;
		txtWO_QUANTITY5.Left = 2.743232f;
		txtWO_QUANTITY5.Name = "txtWO_QUANTITY5";
		txtWO_QUANTITY5.OutputFormat = resources.GetString("txtWO_QUANTITY5.OutputFormat");
		txtWO_QUANTITY5.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtWO_QUANTITY5.Text = "WO_QUANTITY3";
		txtWO_QUANTITY5.Top = 2.753f;
		txtWO_QUANTITY5.Width = 0.3996062f;
		textBox19.CanGrow = false;
		textBox19.DataField = "MODEL_SUFFIX5";
		textBox19.Height = 0.1811024f;
		textBox19.Left = 3.208184f;
		textBox19.Name = "textBox19";
		textBox19.OutputFormat = resources.GetString("textBox19.OutputFormat");
		textBox19.Style = "background-color: Transparent; font-size: 8pt; font-weight: bold; text-align: center; vertical-align: middle; ddo-char-set: 1";
		textBox19.Text = "MODEL_SUFFIX5";
		textBox19.Top = 2.745126f;
		textBox19.Width = 1.315355f;
		textBox20.CanGrow = false;
		textBox20.DataField = "WORKER_ORDER5";
		textBox20.Height = 0.1811024f;
		textBox20.Left = 4.552669f;
		textBox20.Name = "textBox20";
		textBox20.OutputFormat = resources.GetString("textBox20.OutputFormat");
		textBox20.Style = "background-color: Transparent; font-size: 9.75pt; font-weight: bold; text-align: center; vertical-align: middle";
		textBox20.Text = "WORKER_ORDER5";
		textBox20.Top = 2.737252f;
		textBox20.Width = 0.9145669f;
		barcode.BackColor = Color.White;
		barcode.DataField = "QRCODE_VALUE";
		barcode.Font = new Font("Courier New", 8f);
		barcode.Height = 0.9319445f;
		barcode.Left = 4.557f;
		barcode.Name = "barcode";
		barcode.QuietZoneBottom = 0f;
		barcode.QuietZoneLeft = 0f;
		barcode.QuietZoneRight = 0f;
		barcode.QuietZoneTop = 0f;
		barcode.Style = BarCodeStyle.QRCode;
		barcode.Text = "barcod";
		barcode.Top = 0.403f;
		barcode.Width = 0.9326389f;
		txtLOCATOR_GROUP.DataField = "LOCATOR_GROUP";
		txtLOCATOR_GROUP.Height = 0.2468504f;
		txtLOCATOR_GROUP.Left = 0.2759097f;
		txtLOCATOR_GROUP.Name = "txtLOCATOR_GROUP";
		txtLOCATOR_GROUP.Style = "background-color: Transparent; font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtLOCATOR_GROUP.Text = "LOCATOR_GROUP";
		txtLOCATOR_GROUP.Top = 2.656543f;
		txtLOCATOR_GROUP.Width = 1.064173f;
		txtLINE.DataField = "LINE";
		txtLINE.Height = 0.2468504f;
		txtLINE.Left = 0.2798393f;
		txtLINE.Name = "txtLINE";
		txtLINE.Style = "background-color: Transparent; font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtLINE.Text = "LINE";
		txtLINE.Top = 0.5608742f;
		txtLINE.Width = 1.001575f;
		txtPART_NO.DataField = "PART_NO";
		txtPART_NO.Height = 0.2570866f;
		txtPART_NO.Left = 1.373153f;
		txtPART_NO.Name = "txtPART_NO";
		txtPART_NO.Style = "background-color: Transparent; font-size: 12pt; font-weight: bold; text-align: center; vertical-align: middle";
		txtPART_NO.Text = "PART_NO";
		txtPART_NO.Top = 1.576228f;
		txtPART_NO.Width = 1.246457f;
		line1.Height = 0f;
		line1.Left = 0.2333902f;
		line1.LineWeight = 1.5f;
		line1.Name = "line1";
		line1.Top = 0.8770158f;
		line1.Width = 4.293687f;
		line1.X1 = 0.2333902f;
		line1.X2 = 4.527078f;
		line1.Y1 = 0.8770158f;
		line1.Y2 = 0.8770158f;
		shape1.Height = 3.181102f;
		shape1.Left = 0.2089804f;
		shape1.Name = "shape1";
		shape1.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		shape1.Top = 0.3415828f;
		shape1.Width = 5.339764f;
		shape2.Height = 3.120867f;
		shape2.Left = 0.2310279f;
		shape2.Name = "shape2";
		shape2.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		shape2.Top = 0.3648112f;
		shape2.Width = 5.290158f;
		line2.Height = 2.384186E-07f;
		line2.Left = 0.2333902f;
		line2.LineWeight = 1.5f;
		line2.Name = "line2";
		line2.Top = 1.364811f;
		line2.Width = 5.294475f;
		line2.X1 = 0.2333902f;
		line2.X2 = 5.527865f;
		line2.Y1 = 1.364811f;
		line2.Y2 = 1.364811f;
		line3.Height = 4.768372E-07f;
		line3.Left = 0.2337835f;
		line3.LineWeight = 1.5f;
		line3.Name = "line3";
		line3.Top = 1.920716f;
		line3.Width = 5.294089f;
		line3.X1 = 0.2337835f;
		line3.X2 = 5.527873f;
		line3.Y1 = 1.920716f;
		line3.Y2 = 1.920717f;
		line4.Height = 0f;
		line4.Left = 0.2314213f;
		line4.LineWeight = 1.5f;
		line4.Name = "line4";
		line4.Top = 2.433315f;
		line4.Width = 5.297625f;
		line4.X1 = 0.2314213f;
		line4.X2 = 5.529046f;
		line4.Y1 = 2.433315f;
		line4.Y2 = 2.433315f;
		line5.Height = 4.768372E-07f;
		line5.Left = 0.2333902f;
		line5.LineWeight = 1.5f;
		line5.Name = "line5";
		line5.Top = 2.983708f;
		line5.Width = 5.294475f;
		line5.X1 = 0.2333902f;
		line5.X2 = 5.527865f;
		line5.Y1 = 2.983708f;
		line5.Y2 = 2.983709f;
		line6.Height = 2.615354f;
		line6.Left = 4.524716f;
		line6.LineWeight = 1.5f;
		line6.Name = "line6";
		line6.Top = 0.3652049f;
		line6.Width = 0f;
		line6.X1 = 4.524716f;
		line6.X2 = 4.524716f;
		line6.Y1 = 0.3652049f;
		line6.Y2 = 2.980559f;
		line8.Height = 1.613779f;
		line8.Left = 2.707799f;
		line8.LineWeight = 1.5f;
		line8.Name = "line8";
		line8.Top = 1.365992f;
		line8.Width = 0f;
		line8.X1 = 2.707799f;
		line8.X2 = 2.707799f;
		line8.Y1 = 1.365992f;
		line8.Y2 = 2.979772f;
		line9.Height = 1.613386f;
		line9.Left = 3.185746f;
		line9.LineWeight = 1.5f;
		line9.Name = "line9";
		line9.Top = 1.366386f;
		line9.Width = 0f;
		line9.X1 = 3.185746f;
		line9.X2 = 3.185746f;
		line9.Y1 = 1.366386f;
		line9.Y2 = 2.979772f;
		line10.Height = 2.61811f;
		line10.Left = 1.333783f;
		line10.LineWeight = 1.5f;
		line10.Name = "line10";
		line10.Top = 0.3648112f;
		line10.Width = 0f;
		line10.X1 = 1.333783f;
		line10.X2 = 1.333783f;
		line10.Y1 = 0.3648112f;
		line10.Y2 = 2.982921f;
		line11.Height = 0f;
		line11.Left = 2.711343f;
		line11.LineWeight = 1.5f;
		line11.Name = "line11";
		line11.Top = 2.164811f;
		line11.Width = 2.81653f;
		line11.X1 = 2.711343f;
		line11.X2 = 5.527873f;
		line11.Y1 = 2.164811f;
		line11.Y2 = 2.164811f;
		line12.Height = 4.768372E-07f;
		line12.Left = 2.713704f;
		line12.LineWeight = 1.5f;
		line12.Name = "line12";
		line12.Top = 2.697094f;
		line12.Width = 2.810232f;
		line12.X1 = 2.713704f;
		line12.X2 = 5.523936f;
		line12.Y1 = 2.697094f;
		line12.Y2 = 2.697095f;
		line13.Height = 0f;
		line13.Left = 2.715279f;
		line13.LineWeight = 1.5f;
		line13.Name = "line13";
		line13.Top = 1.641976f;
		line13.Width = 2.812594f;
		line13.X1 = 2.715279f;
		line13.X2 = 5.527873f;
		line13.Y1 = 1.641976f;
		line13.Y2 = 1.641976f;
		txtSheetID.CanGrow = false;
		txtSheetID.DataField = "SHEET_ID";
		txtSheetID.Height = 0.222047f;
		txtSheetID.Left = 3.333377f;
		txtSheetID.Name = "txtSheetID";
		txtSheetID.Style = "background-color: Transparent; font-size: 11.25pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtSheetID.Text = "SHEET_ID";
		txtSheetID.Top = 3.151819f;
		txtSheetID.Width = 2.214567f;
		txtPST.CanGrow = false;
		txtPST.DataField = "PST";
		txtPST.Height = 0.3692912f;
		txtPST.Left = 1.357798f;
		txtPST.Name = "txtPST";
		txtPST.Style = "background-color: Transparent; font-family: Arial; font-size: 10pt; font-weight: bold; text-align: center; vertical-align: top; ddo-char-set: 1";
		txtPST.Text = "2019-01-30 PM  12:12:11";
		txtPST.Top = 2.571504f;
		txtPST.Width = 1.35748f;
		Detail.ColumnCount = 2;
		Detail.ColumnDirection = ColumnDirection.AcrossDown;
		Detail.Controls.AddRange(new ARControl[56]
		{
			shape1, shape2, txtTOTAL_QUANTITY, txtCARRIER_ID, txtMADE_BY, txtDESCRIPTION, txtINSPECTION_FLAG, txtWO_QUANTITY1, txtTOTAL_SEQ_NO, txtMODEL_SUFFIX1,
			txtWORKER_ORDER1, label1, label2, label3, label4, label5, label6, label7, label8, label9,
			label10, label12, label13, label15, txtPrintedDate, label17, txtWO_QUANTITY2, textBox4, textBox5, txtWO_QUANTITY3,
			textBox9, textBox10, txtWO_QUANTITY4, textBox14, textBox15, txtWO_QUANTITY5, textBox19, textBox20, barcode, txtLOCATOR_GROUP,
			txtLINE, txtPART_NO, line1, line2, line3, line4, line5, line6, line8, line9,
			line10, line11, line12, line13, txtSheetID, txtPST
		});
		Detail.Height = 4.114616f;
		Detail.Name = "Detail";
		base.MasterReport = false;
		base.PageSettings.Margins.Bottom = 0f;
		base.PageSettings.Margins.Left = 0.09448819f;
		base.PageSettings.Margins.Right = 0.09448819f;
		base.PageSettings.Margins.Top = 0.03f;
		base.PageSettings.Orientation = PageOrientation.Landscape;
		base.PageSettings.PaperHeight = 11f;
		base.PageSettings.PaperWidth = 8.5f;
		base.PrintWidth = 11.46189f;
		base.Sections.Add(Detail);
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; color: Black; ddo-char-set: 186", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-size: 16pt; font-style: normal; font-weight: bold; ddo-char-set: 186", "Heading1", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-style: italic; font-weight: bold; ddo-char-set: 186", "Heading2", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-size: 13pt; font-style: normal; font-weight: bold; ddo-char-set: 186", "Heading3", "Normal"));
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
		((ISupportInitialize)label7).EndInit();
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
		((ISupportInitialize)this).EndInit();
	}
}
