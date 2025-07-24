using System.ComponentModel;
using System.Drawing;
using DDCssLib;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Controls;
using GrapeCity.ActiveReports.SectionReportModel;

namespace MCS.PrintBoard;

public class OfflinePrintBoard : SectionReport
{
	private TextBox txtTOTAL_SEQ_NO;

	private TextBox txtPST;

	private Detail Detail;

	private TextBox txtTOTAL_QUANTITY;

	private TextBox txtCARRIER_ID;

	private TextBox txtMADE_BY;

	private TextBox txtDESCRIPTION;

	private TextBox txtINSPECTION_FLAG;

	private TextBox txtWO_QUANTITY1;

	private TextBox txtPROD_SEQ_FROM1;

	private TextBox txtPROD_SEQ_TO1;

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

	private Label label11;

	private Label label12;

	private Label label13;

	private Label label15;

	private TextBox txtPrintedDate;

	private Label label17;

	private Label lblB1;

	private TextBox txtWO_QUANTITY2;

	private TextBox textBox2;

	private TextBox textBox3;

	private TextBox textBox4;

	private TextBox textBox5;

	private Label lblB2;

	private TextBox txtWO_QUANTITY3;

	private TextBox textBox7;

	private TextBox textBox8;

	private TextBox textBox9;

	private TextBox textBox10;

	private Label lblB3;

	private TextBox txtWO_QUANTITY4;

	private TextBox textBox12;

	private TextBox textBox13;

	private TextBox textBox14;

	private TextBox textBox15;

	private Label lblB4;

	private TextBox txtWO_QUANTITY5;

	private TextBox textBox17;

	private TextBox textBox18;

	private TextBox textBox19;

	private TextBox textBox20;

	private Label lblB5;

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

	private Line line7;

	private Line line8;

	private Line line9;

	private Line line10;

	private Line line11;

	private Line line12;

	private Line line13;

	private TextBox txtSheetID;

	public OfflinePrintBoard()
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
		ComponentResourceManager resources = new ComponentResourceManager(typeof(OfflinePrintBoard));
		txtTOTAL_QUANTITY = new TextBox();
		txtCARRIER_ID = new TextBox();
		txtMADE_BY = new TextBox();
		txtDESCRIPTION = new TextBox();
		txtINSPECTION_FLAG = new TextBox();
		txtWO_QUANTITY1 = new TextBox();
		txtTOTAL_SEQ_NO = new TextBox();
		txtPST = new TextBox();
		txtPROD_SEQ_FROM1 = new TextBox();
		txtPROD_SEQ_TO1 = new TextBox();
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
		label11 = new Label();
		label12 = new Label();
		label13 = new Label();
		label15 = new Label();
		txtPrintedDate = new TextBox();
		label17 = new Label();
		lblB1 = new Label();
		txtWO_QUANTITY2 = new TextBox();
		textBox2 = new TextBox();
		textBox3 = new TextBox();
		textBox4 = new TextBox();
		textBox5 = new TextBox();
		lblB2 = new Label();
		txtWO_QUANTITY3 = new TextBox();
		textBox7 = new TextBox();
		textBox8 = new TextBox();
		textBox9 = new TextBox();
		textBox10 = new TextBox();
		lblB3 = new Label();
		txtWO_QUANTITY4 = new TextBox();
		textBox12 = new TextBox();
		textBox13 = new TextBox();
		textBox14 = new TextBox();
		textBox15 = new TextBox();
		lblB4 = new Label();
		txtWO_QUANTITY5 = new TextBox();
		textBox17 = new TextBox();
		textBox18 = new TextBox();
		textBox19 = new TextBox();
		textBox20 = new TextBox();
		lblB5 = new Label();
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
		line7 = new Line();
		line8 = new Line();
		line9 = new Line();
		line10 = new Line();
		line11 = new Line();
		line12 = new Line();
		line13 = new Line();
		txtSheetID = new TextBox();
		Detail = new Detail();
		((ISupportInitialize)txtTOTAL_QUANTITY).BeginInit();
		((ISupportInitialize)txtCARRIER_ID).BeginInit();
		((ISupportInitialize)txtMADE_BY).BeginInit();
		((ISupportInitialize)txtDESCRIPTION).BeginInit();
		((ISupportInitialize)txtINSPECTION_FLAG).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY1).BeginInit();
		((ISupportInitialize)txtTOTAL_SEQ_NO).BeginInit();
		((ISupportInitialize)txtPST).BeginInit();
		((ISupportInitialize)txtPROD_SEQ_FROM1).BeginInit();
		((ISupportInitialize)txtPROD_SEQ_TO1).BeginInit();
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
		((ISupportInitialize)label11).BeginInit();
		((ISupportInitialize)label12).BeginInit();
		((ISupportInitialize)label13).BeginInit();
		((ISupportInitialize)label15).BeginInit();
		((ISupportInitialize)txtPrintedDate).BeginInit();
		((ISupportInitialize)label17).BeginInit();
		((ISupportInitialize)lblB1).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY2).BeginInit();
		((ISupportInitialize)textBox2).BeginInit();
		((ISupportInitialize)textBox3).BeginInit();
		((ISupportInitialize)textBox4).BeginInit();
		((ISupportInitialize)textBox5).BeginInit();
		((ISupportInitialize)lblB2).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY3).BeginInit();
		((ISupportInitialize)textBox7).BeginInit();
		((ISupportInitialize)textBox8).BeginInit();
		((ISupportInitialize)textBox9).BeginInit();
		((ISupportInitialize)textBox10).BeginInit();
		((ISupportInitialize)lblB3).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY4).BeginInit();
		((ISupportInitialize)textBox12).BeginInit();
		((ISupportInitialize)textBox13).BeginInit();
		((ISupportInitialize)textBox14).BeginInit();
		((ISupportInitialize)textBox15).BeginInit();
		((ISupportInitialize)lblB4).BeginInit();
		((ISupportInitialize)txtWO_QUANTITY5).BeginInit();
		((ISupportInitialize)textBox17).BeginInit();
		((ISupportInitialize)textBox18).BeginInit();
		((ISupportInitialize)textBox19).BeginInit();
		((ISupportInitialize)textBox20).BeginInit();
		((ISupportInitialize)lblB5).BeginInit();
		((ISupportInitialize)txtLOCATOR_GROUP).BeginInit();
		((ISupportInitialize)txtLINE).BeginInit();
		((ISupportInitialize)txtPART_NO).BeginInit();
		((ISupportInitialize)txtSheetID).BeginInit();
		((ISupportInitialize)this).BeginInit();
		txtTOTAL_QUANTITY.CanGrow = false;
		txtTOTAL_QUANTITY.DataField = "TOTAL_QUANTITY";
		resources.ApplyResources(txtTOTAL_QUANTITY, "txtTOTAL_QUANTITY");
		txtTOTAL_QUANTITY.Name = "txtTOTAL_QUANTITY";
		txtCARRIER_ID.DataField = "CARRIER_ID";
		resources.ApplyResources(txtCARRIER_ID, "txtCARRIER_ID");
		txtCARRIER_ID.Name = "txtCARRIER_ID";
		txtMADE_BY.CanGrow = false;
		txtMADE_BY.DataField = "MADE_BY";
		resources.ApplyResources(txtMADE_BY, "txtMADE_BY");
		txtMADE_BY.Name = "txtMADE_BY";
		txtDESCRIPTION.CanGrow = false;
		txtDESCRIPTION.DataField = "DESCRIPTION";
		resources.ApplyResources(txtDESCRIPTION, "txtDESCRIPTION");
		txtDESCRIPTION.Name = "txtDESCRIPTION";
		txtINSPECTION_FLAG.CanGrow = false;
		txtINSPECTION_FLAG.DataField = "INSPECTION_FLAG";
		resources.ApplyResources(txtINSPECTION_FLAG, "txtINSPECTION_FLAG");
		txtINSPECTION_FLAG.Name = "txtINSPECTION_FLAG";
		txtWO_QUANTITY1.CanGrow = false;
		txtWO_QUANTITY1.DataField = "WO_QUANTITY1";
		resources.ApplyResources(txtWO_QUANTITY1, "txtWO_QUANTITY1");
		txtWO_QUANTITY1.Name = "txtWO_QUANTITY1";
		txtTOTAL_SEQ_NO.DataField = "TOTAL_SEQ_NO";
		resources.ApplyResources(txtTOTAL_SEQ_NO, "txtTOTAL_SEQ_NO");
		txtTOTAL_SEQ_NO.Name = "txtTOTAL_SEQ_NO";
		txtPST.CanGrow = false;
		txtPST.DataField = "PST";
		resources.ApplyResources(txtPST, "txtPST");
		txtPST.Name = "txtPST";
		txtPROD_SEQ_FROM1.CanGrow = false;
		txtPROD_SEQ_FROM1.DataField = "PROD_SEQ_FROM1";
		resources.ApplyResources(txtPROD_SEQ_FROM1, "txtPROD_SEQ_FROM1");
		txtPROD_SEQ_FROM1.Name = "txtPROD_SEQ_FROM1";
		txtPROD_SEQ_TO1.CanGrow = false;
		txtPROD_SEQ_TO1.DataField = "PROD_SEQ_TO1";
		resources.ApplyResources(txtPROD_SEQ_TO1, "txtPROD_SEQ_TO1");
		txtPROD_SEQ_TO1.Name = "txtPROD_SEQ_TO1";
		txtMODEL_SUFFIX1.CanGrow = false;
		txtMODEL_SUFFIX1.DataField = "MODEL_SUFFIX1";
		resources.ApplyResources(txtMODEL_SUFFIX1, "txtMODEL_SUFFIX1");
		txtMODEL_SUFFIX1.Name = "txtMODEL_SUFFIX1";
		txtWORKER_ORDER1.CanGrow = false;
		txtWORKER_ORDER1.DataField = "WORKER_ORDER1";
		resources.ApplyResources(txtWORKER_ORDER1, "txtWORKER_ORDER1");
		txtWORKER_ORDER1.Name = "txtWORKER_ORDER1";
		resources.ApplyResources(label1, "label1");
		label1.Name = "label1";
		resources.ApplyResources(label2, "label2");
		label2.Name = "label2";
		resources.ApplyResources(label3, "label3");
		label3.Name = "label3";
		resources.ApplyResources(label4, "label4");
		label4.Name = "label4";
		resources.ApplyResources(label5, "label5");
		label5.Name = "label5";
		resources.ApplyResources(label6, "label6");
		label6.Name = "label6";
		resources.ApplyResources(label7, "label7");
		label7.Name = "label7";
		resources.ApplyResources(label8, "label8");
		label8.Name = "label8";
		resources.ApplyResources(label9, "label9");
		label9.Name = "label9";
		resources.ApplyResources(label10, "label10");
		label10.Name = "label10";
		resources.ApplyResources(label11, "label11");
		label11.Name = "label11";
		resources.ApplyResources(label12, "label12");
		label12.Name = "label12";
		resources.ApplyResources(label13, "label13");
		label13.Name = "label13";
		resources.ApplyResources(label15, "label15");
		label15.Name = "label15";
		txtPrintedDate.CanGrow = false;
		txtPrintedDate.DataField = "PRINTED_DATE";
		resources.ApplyResources(txtPrintedDate, "txtPrintedDate");
		txtPrintedDate.Name = "txtPrintedDate";
		resources.ApplyResources(label17, "label17");
		label17.Name = "label17";
		resources.ApplyResources(lblB1, "lblB1");
		lblB1.Name = "lblB1";
		txtWO_QUANTITY2.CanGrow = false;
		txtWO_QUANTITY2.DataField = "WO_QUANTITY2";
		resources.ApplyResources(txtWO_QUANTITY2, "txtWO_QUANTITY2");
		txtWO_QUANTITY2.Name = "txtWO_QUANTITY2";
		textBox2.CanGrow = false;
		textBox2.DataField = "PROD_SEQ_FROM2";
		resources.ApplyResources(textBox2, "textBox2");
		textBox2.Name = "textBox2";
		textBox3.CanGrow = false;
		textBox3.DataField = "PROD_SEQ_TO2";
		resources.ApplyResources(textBox3, "textBox3");
		textBox3.Name = "textBox3";
		textBox4.CanGrow = false;
		textBox4.DataField = "MODEL_SUFFIX2";
		resources.ApplyResources(textBox4, "textBox4");
		textBox4.Name = "textBox4";
		textBox5.CanGrow = false;
		textBox5.DataField = "WORKER_ORDER2";
		resources.ApplyResources(textBox5, "textBox5");
		textBox5.Name = "textBox5";
		resources.ApplyResources(lblB2, "lblB2");
		lblB2.Name = "lblB2";
		txtWO_QUANTITY3.CanGrow = false;
		txtWO_QUANTITY3.DataField = "WO_QUANTITY3";
		resources.ApplyResources(txtWO_QUANTITY3, "txtWO_QUANTITY3");
		txtWO_QUANTITY3.Name = "txtWO_QUANTITY3";
		textBox7.CanGrow = false;
		textBox7.DataField = "PROD_SEQ_FROM3";
		resources.ApplyResources(textBox7, "textBox7");
		textBox7.Name = "textBox7";
		textBox8.CanGrow = false;
		textBox8.DataField = "PROD_SEQ_TO3";
		resources.ApplyResources(textBox8, "textBox8");
		textBox8.Name = "textBox8";
		textBox9.CanGrow = false;
		textBox9.DataField = "MODEL_SUFFIX3";
		resources.ApplyResources(textBox9, "textBox9");
		textBox9.Name = "textBox9";
		textBox10.CanGrow = false;
		textBox10.DataField = "WORKER_ORDER3";
		resources.ApplyResources(textBox10, "textBox10");
		textBox10.Name = "textBox10";
		resources.ApplyResources(lblB3, "lblB3");
		lblB3.Name = "lblB3";
		txtWO_QUANTITY4.CanGrow = false;
		txtWO_QUANTITY4.DataField = "WO_QUANTITY4";
		resources.ApplyResources(txtWO_QUANTITY4, "txtWO_QUANTITY4");
		txtWO_QUANTITY4.Name = "txtWO_QUANTITY4";
		textBox12.CanGrow = false;
		textBox12.DataField = "PROD_SEQ_FROM4";
		resources.ApplyResources(textBox12, "textBox12");
		textBox12.Name = "textBox12";
		textBox13.CanGrow = false;
		textBox13.DataField = "PROD_SEQ_TO4";
		resources.ApplyResources(textBox13, "textBox13");
		textBox13.Name = "textBox13";
		textBox14.CanGrow = false;
		textBox14.DataField = "MODEL_SUFFIX4";
		resources.ApplyResources(textBox14, "textBox14");
		textBox14.Name = "textBox14";
		textBox15.CanGrow = false;
		textBox15.DataField = "WORKER_ORDER4";
		resources.ApplyResources(textBox15, "textBox15");
		textBox15.Name = "textBox15";
		resources.ApplyResources(lblB4, "lblB4");
		lblB4.Name = "lblB4";
		txtWO_QUANTITY5.CanGrow = false;
		txtWO_QUANTITY5.DataField = "WO_QUANTITY5";
		resources.ApplyResources(txtWO_QUANTITY5, "txtWO_QUANTITY5");
		txtWO_QUANTITY5.Name = "txtWO_QUANTITY5";
		textBox17.CanGrow = false;
		textBox17.DataField = "PROD_SEQ_FROM5";
		resources.ApplyResources(textBox17, "textBox17");
		textBox17.Name = "textBox17";
		textBox18.CanGrow = false;
		textBox18.DataField = "PROD_SEQ_TO5";
		resources.ApplyResources(textBox18, "textBox18");
		textBox18.Name = "textBox18";
		textBox19.CanGrow = false;
		textBox19.DataField = "MODEL_SUFFIX5";
		resources.ApplyResources(textBox19, "textBox19");
		textBox19.Name = "textBox19";
		textBox20.CanGrow = false;
		textBox20.DataField = "WORKER_ORDER5";
		resources.ApplyResources(textBox20, "textBox20");
		textBox20.Name = "textBox20";
		resources.ApplyResources(lblB5, "lblB5");
		lblB5.Name = "lblB5";
		barcode.BackColor = Color.White;
		barcode.DataField = "QRCODE_VALUE";
		barcode.Font = new Font("Courier New", 8f);
		resources.ApplyResources(barcode, "barcode");
		barcode.Name = "barcode";
		barcode.QuietZoneBottom = 0f;
		barcode.QuietZoneLeft = 0f;
		barcode.QuietZoneRight = 0f;
		barcode.QuietZoneTop = 0f;
		barcode.Style = BarCodeStyle.QRCode;
		txtLOCATOR_GROUP.DataField = "LOCATOR_GROUP";
		resources.ApplyResources(txtLOCATOR_GROUP, "txtLOCATOR_GROUP");
		txtLOCATOR_GROUP.Name = "txtLOCATOR_GROUP";
		txtLINE.DataField = "LINE";
		resources.ApplyResources(txtLINE, "txtLINE");
		txtLINE.Name = "txtLINE";
		txtPART_NO.DataField = "PART_NO";
		resources.ApplyResources(txtPART_NO, "txtPART_NO");
		txtPART_NO.Name = "txtPART_NO";
		resources.ApplyResources(line1, "line1");
		line1.LineWeight = 1.5f;
		line1.Name = "line1";
		line1.X1 = 0.3255906f;
		line1.X2 = 6.63189f;
		line1.Y1 = 0.4775591f;
		line1.Y2 = 0.4775591f;
		resources.ApplyResources(shape1, "shape1");
		shape1.Name = "shape1";
		shape1.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		resources.ApplyResources(shape2, "shape2");
		shape2.Name = "shape2";
		shape2.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		resources.ApplyResources(line2, "line2");
		line2.LineWeight = 1.5f;
		line2.Name = "line2";
		line2.X1 = 0.3255906f;
		line2.X2 = 7.507874f;
		line2.Y1 = 0.8925198f;
		line2.Y2 = 0.8925198f;
		resources.ApplyResources(line3, "line3");
		line3.LineWeight = 1.5f;
		line3.Name = "line3";
		line3.X1 = 0.3255906f;
		line3.X2 = 7.507874f;
		line3.Y1 = 1.358662f;
		line3.Y2 = 1.358662f;
		resources.ApplyResources(line4, "line4");
		line4.LineWeight = 1.5f;
		line4.Name = "line4";
		line4.X1 = 0.3255906f;
		line4.X2 = 7.507874f;
		line4.Y1 = 1.79252f;
		line4.Y2 = 1.79252f;
		resources.ApplyResources(line5, "line5");
		line5.LineWeight = 1.5f;
		line5.Name = "line5";
		line5.X1 = 0.3255906f;
		line5.X2 = 7.507874f;
		line5.Y1 = 2.224804f;
		line5.Y2 = 2.224804f;
		resources.ApplyResources(line6, "line6");
		line6.LineWeight = 1.5f;
		line6.Name = "line6";
		line6.X1 = 6.63189f;
		line6.X2 = 6.63189f;
		line6.Y1 = 0.0468504f;
		line6.Y2 = 2.224803f;
		resources.ApplyResources(line7, "line7");
		line7.LineWeight = 1.5f;
		line7.Name = "line7";
		line7.X1 = 4.984252f;
		line7.X2 = 4.984252f;
		line7.Y1 = 0.8964568f;
		line7.Y2 = 2.225591f;
		resources.ApplyResources(line8, "line8");
		line8.LineWeight = 1.5f;
		line8.Name = "line8";
		line8.X1 = 3.605906f;
		line8.X2 = 3.605906f;
		line8.Y1 = 0.8984253f;
		line8.Y2 = 2.227559f;
		resources.ApplyResources(line9, "line9");
		line9.LineWeight = 1.5f;
		line9.Name = "line9";
		line9.X1 = 4.075984f;
		line9.X2 = 4.075984f;
		line9.Y1 = 0.8964568f;
		line9.Y2 = 2.225591f;
		resources.ApplyResources(line10, "line10");
		line10.LineWeight = 1.5f;
		line10.Name = "line10";
		line10.X1 = 1.886614f;
		line10.X2 = 1.886614f;
		line10.Y1 = 0.0468504f;
		line10.Y2 = 2.227559f;
		resources.ApplyResources(line11, "line11");
		line11.LineWeight = 1.5f;
		line11.Name = "line11";
		line11.X1 = 3.605906f;
		line11.X2 = 7.507874f;
		line11.Y1 = 1.575984f;
		line11.Y2 = 1.572047f;
		resources.ApplyResources(line12, "line12");
		line12.LineWeight = 1.5f;
		line12.Name = "line12";
		line12.X1 = 3.605906f;
		line12.X2 = 7.507874f;
		line12.Y1 = 2.012992f;
		line12.Y2 = 2.009056f;
		resources.ApplyResources(line13, "line13");
		line13.LineWeight = 1.5f;
		line13.Name = "line13";
		line13.X1 = 3.607087f;
		line13.X2 = 7.507874f;
		line13.Y1 = 1.127165f;
		line13.Y2 = 1.123229f;
		txtSheetID.CanGrow = false;
		txtSheetID.DataField = "SHEET_ID";
		resources.ApplyResources(txtSheetID, "txtSheetID");
		txtSheetID.Name = "txtSheetID";
		Detail.Controls.AddRange(new ARControl[73]
		{
			shape1, shape2, txtTOTAL_QUANTITY, txtCARRIER_ID, txtMADE_BY, txtDESCRIPTION, txtINSPECTION_FLAG, txtWO_QUANTITY1, txtTOTAL_SEQ_NO, txtPST,
			txtPROD_SEQ_FROM1, txtPROD_SEQ_TO1, txtMODEL_SUFFIX1, txtWORKER_ORDER1, label1, label2, label3, label4, label5, label6,
			label7, label8, label9, label10, label11, label12, label13, label15, txtPrintedDate, label17,
			lblB1, txtWO_QUANTITY2, textBox2, textBox3, textBox4, textBox5, lblB2, txtWO_QUANTITY3, textBox7, textBox8,
			textBox9, textBox10, lblB3, txtWO_QUANTITY4, textBox12, textBox13, textBox14, textBox15, lblB4, txtWO_QUANTITY5,
			textBox17, textBox18, textBox19, textBox20, lblB5, barcode, txtLOCATOR_GROUP, txtLINE, txtPART_NO, line1,
			line2, line3, line4, line5, line6, line7, line8, line9, line10, line11,
			line12, line13, txtSheetID
		});
		Detail.Height = 2.796013f;
		Detail.KeepTogether = true;
		Detail.Name = "Detail";
		base.MasterReport = false;
		base.MaxPages = 100L;
		base.PageSettings.Margins.Bottom = 0.1181102f;
		base.PageSettings.Margins.Left = 0.2125984f;
		base.PageSettings.Margins.Right = 0.2125984f;
		base.PageSettings.Margins.Top = 0.2125984f;
		base.PageSettings.PaperHeight = 11f;
		base.PageSettings.PaperWidth = 8.5f;
		base.PrintWidth = 7.836889f;
		base.Script = resources.GetString("$this.Script");
		base.Sections.Add(Detail);
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; color: Black; ddo-char-set: 186", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-size: 16pt; font-style: normal; font-weight: bold", "Heading1", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-style: italic; font-weight: bold", "Heading2", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-size: 13pt; font-style: normal; font-weight: bold", "Heading3", "Normal"));
		((ISupportInitialize)txtTOTAL_QUANTITY).EndInit();
		((ISupportInitialize)txtCARRIER_ID).EndInit();
		((ISupportInitialize)txtMADE_BY).EndInit();
		((ISupportInitialize)txtDESCRIPTION).EndInit();
		((ISupportInitialize)txtINSPECTION_FLAG).EndInit();
		((ISupportInitialize)txtWO_QUANTITY1).EndInit();
		((ISupportInitialize)txtTOTAL_SEQ_NO).EndInit();
		((ISupportInitialize)txtPST).EndInit();
		((ISupportInitialize)txtPROD_SEQ_FROM1).EndInit();
		((ISupportInitialize)txtPROD_SEQ_TO1).EndInit();
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
		((ISupportInitialize)label11).EndInit();
		((ISupportInitialize)label12).EndInit();
		((ISupportInitialize)label13).EndInit();
		((ISupportInitialize)label15).EndInit();
		((ISupportInitialize)txtPrintedDate).EndInit();
		((ISupportInitialize)label17).EndInit();
		((ISupportInitialize)lblB1).EndInit();
		((ISupportInitialize)txtWO_QUANTITY2).EndInit();
		((ISupportInitialize)textBox2).EndInit();
		((ISupportInitialize)textBox3).EndInit();
		((ISupportInitialize)textBox4).EndInit();
		((ISupportInitialize)textBox5).EndInit();
		((ISupportInitialize)lblB2).EndInit();
		((ISupportInitialize)txtWO_QUANTITY3).EndInit();
		((ISupportInitialize)textBox7).EndInit();
		((ISupportInitialize)textBox8).EndInit();
		((ISupportInitialize)textBox9).EndInit();
		((ISupportInitialize)textBox10).EndInit();
		((ISupportInitialize)lblB3).EndInit();
		((ISupportInitialize)txtWO_QUANTITY4).EndInit();
		((ISupportInitialize)textBox12).EndInit();
		((ISupportInitialize)textBox13).EndInit();
		((ISupportInitialize)textBox14).EndInit();
		((ISupportInitialize)textBox15).EndInit();
		((ISupportInitialize)lblB4).EndInit();
		((ISupportInitialize)txtWO_QUANTITY5).EndInit();
		((ISupportInitialize)textBox17).EndInit();
		((ISupportInitialize)textBox18).EndInit();
		((ISupportInitialize)textBox19).EndInit();
		((ISupportInitialize)textBox20).EndInit();
		((ISupportInitialize)lblB5).EndInit();
		((ISupportInitialize)txtLOCATOR_GROUP).EndInit();
		((ISupportInitialize)txtLINE).EndInit();
		((ISupportInitialize)txtPART_NO).EndInit();
		((ISupportInitialize)txtSheetID).EndInit();
		((ISupportInitialize)this).EndInit();
	}
}
