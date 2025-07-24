using System.ComponentModel;
using System.Drawing;
using System.Resources;
using DDCssLib;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Controls;
using GrapeCity.ActiveReports.SectionReportModel;

namespace MCS.PrintBoard;

public class QtyChangePrint : SectionReport
{
	private Detail detail;

	private Shape shape2;

	private Label label11;

	private Line line1;

	private Line line2;

	private Line line4;

	private Line line5;

	private Line line9;

	private Line line15;

	private Line line3;

	private Line line7;

	private Line line16;

	private Line line17;

	private Label label16;

	private Label label17;

	private Label label18;

	private Label label1;

	private Label label2;

	private Label label3;

	private Label label4;

	private Label label5;

	private Label label6;

	private Label label7;

	private Label label13;

	private Label label14;

	private Label label15;

	private Barcode barcode;

	private Shape shape1;

	private TextBox txtSheetID;

	private Line line19;

	private Label label12;

	private TextBox txtOrg;

	private TextBox txtLine;

	private TextBox textPartNo;

	private TextBox txtDescription;

	private TextBox txtDelivery;

	private TextBox txtWorkOrder1;

	private TextBox txtPlanQty1;

	private TextBox txtPlanDate1;

	private TextBox txtWorkOrder2;

	private TextBox txtPlanQty2;

	private TextBox txtPlanDate2;

	private TextBox txtWorkOrder3;

	private TextBox txtPlanQty3;

	private TextBox txtPlanDate3;

	private TextBox txtWorkOrder4;

	private TextBox txtPlanQty4;

	private TextBox txtPlanDate4;

	private TextBox txtSpec1;

	private TextBox txtSpec2;

	private TextBox txtSpec3;

	private TextBox textBox1;

	private Line line8;

	private TextBox textBox2;

	private Line line11;

	private Line line13;

	private Line line12;

	private Line line14;

	private Label label8;

	public QtyChangePrint()
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
		ResourceManager resources = new ResourceManager(typeof(QtyChangePrint));
		detail = new Detail();
		shape1 = new Shape();
		shape2 = new Shape();
		label11 = new Label();
		line1 = new Line();
		line2 = new Line();
		line4 = new Line();
		line5 = new Line();
		line9 = new Line();
		line15 = new Line();
		line3 = new Line();
		line7 = new Line();
		line16 = new Line();
		line17 = new Line();
		label16 = new Label();
		label17 = new Label();
		label18 = new Label();
		label1 = new Label();
		label2 = new Label();
		label3 = new Label();
		label4 = new Label();
		label5 = new Label();
		label6 = new Label();
		label7 = new Label();
		label13 = new Label();
		label14 = new Label();
		label15 = new Label();
		barcode = new Barcode();
		txtSheetID = new TextBox();
		line19 = new Line();
		label12 = new Label();
		txtOrg = new TextBox();
		txtLine = new TextBox();
		txtDelivery = new TextBox();
		txtWorkOrder1 = new TextBox();
		txtPlanQty1 = new TextBox();
		txtPlanDate1 = new TextBox();
		txtWorkOrder2 = new TextBox();
		txtPlanQty2 = new TextBox();
		txtPlanDate2 = new TextBox();
		txtWorkOrder3 = new TextBox();
		txtPlanQty3 = new TextBox();
		txtPlanDate3 = new TextBox();
		txtWorkOrder4 = new TextBox();
		txtPlanQty4 = new TextBox();
		txtPlanDate4 = new TextBox();
		txtSpec1 = new TextBox();
		txtSpec2 = new TextBox();
		txtSpec3 = new TextBox();
		textBox1 = new TextBox();
		line8 = new Line();
		textBox2 = new TextBox();
		line11 = new Line();
		line12 = new Line();
		line13 = new Line();
		line14 = new Line();
		txtDescription = new TextBox();
		textPartNo = new TextBox();
		label8 = new Label();
		((ISupportInitialize)label11).BeginInit();
		((ISupportInitialize)label16).BeginInit();
		((ISupportInitialize)label17).BeginInit();
		((ISupportInitialize)label18).BeginInit();
		((ISupportInitialize)label1).BeginInit();
		((ISupportInitialize)label2).BeginInit();
		((ISupportInitialize)label3).BeginInit();
		((ISupportInitialize)label4).BeginInit();
		((ISupportInitialize)label5).BeginInit();
		((ISupportInitialize)label6).BeginInit();
		((ISupportInitialize)label7).BeginInit();
		((ISupportInitialize)label13).BeginInit();
		((ISupportInitialize)label14).BeginInit();
		((ISupportInitialize)label15).BeginInit();
		((ISupportInitialize)txtSheetID).BeginInit();
		((ISupportInitialize)label12).BeginInit();
		((ISupportInitialize)txtOrg).BeginInit();
		((ISupportInitialize)txtLine).BeginInit();
		((ISupportInitialize)txtDelivery).BeginInit();
		((ISupportInitialize)txtWorkOrder1).BeginInit();
		((ISupportInitialize)txtPlanQty1).BeginInit();
		((ISupportInitialize)txtPlanDate1).BeginInit();
		((ISupportInitialize)txtWorkOrder2).BeginInit();
		((ISupportInitialize)txtPlanQty2).BeginInit();
		((ISupportInitialize)txtPlanDate2).BeginInit();
		((ISupportInitialize)txtWorkOrder3).BeginInit();
		((ISupportInitialize)txtPlanQty3).BeginInit();
		((ISupportInitialize)txtPlanDate3).BeginInit();
		((ISupportInitialize)txtWorkOrder4).BeginInit();
		((ISupportInitialize)txtPlanQty4).BeginInit();
		((ISupportInitialize)txtPlanDate4).BeginInit();
		((ISupportInitialize)txtSpec1).BeginInit();
		((ISupportInitialize)txtSpec2).BeginInit();
		((ISupportInitialize)txtSpec3).BeginInit();
		((ISupportInitialize)textBox1).BeginInit();
		((ISupportInitialize)textBox2).BeginInit();
		((ISupportInitialize)txtDescription).BeginInit();
		((ISupportInitialize)textPartNo).BeginInit();
		((ISupportInitialize)label8).BeginInit();
		((ISupportInitialize)this).BeginInit();
		detail.Controls.AddRange(new ARControl[58]
		{
			shape1, shape2, label11, line1, line2, line4, line5, line9, line15, line3,
			line7, line16, line17, label16, label17, label18, label1, label2, label3, label4,
			label5, label6, label7, label13, label14, label15, barcode, txtSheetID, line19, label12,
			txtOrg, txtLine, txtDelivery, txtWorkOrder1, txtPlanQty1, txtPlanDate1, txtWorkOrder2, txtPlanQty2, txtPlanDate2, txtWorkOrder3,
			txtPlanQty3, txtPlanDate3, txtWorkOrder4, txtPlanQty4, txtPlanDate4, txtSpec1, txtSpec2, txtSpec3, textBox1, line8,
			textBox2, line11, line12, line13, line14, txtDescription, textPartNo, label8
		});
		detail.Height = 5.676585f;
		detail.Name = "detail";
		shape1.Height = 0.9379997f;
		shape1.Left = 0.27f;
		shape1.Name = "shape1";
		shape1.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		shape1.Top = 4.034f;
		shape1.Width = 3.599f;
		shape2.Height = 2.937f;
		shape2.Left = 0.27f;
		shape2.Name = "shape2";
		shape2.RoundingRadius = new CornersRadius(10f, null, null, null, null);
		shape2.Top = 1.099f;
		shape2.Width = 3.599f;
		label11.Height = 0.3645833f;
		label11.HyperLink = null;
		label11.Left = 1.128f;
		label11.Name = "label11";
		label11.Style = "font-size: 16pt";
		label11.Text = "WIP Delivery Sheet";
		label11.Top = 0.345f;
		label11.Width = 2.136f;
		line1.Height = 0f;
		line1.Left = 0.27f;
		line1.LineWeight = 1f;
		line1.Name = "line1";
		line1.Top = 1.34f;
		line1.Width = 3.599f;
		line1.X1 = 0.27f;
		line1.X2 = 3.869f;
		line1.Y1 = 1.34f;
		line1.Y2 = 1.34f;
		line2.Height = 0f;
		line2.Left = 0.27f;
		line2.LineWeight = 1f;
		line2.Name = "line2";
		line2.Top = 1.573926f;
		line2.Width = 3.599f;
		line2.X1 = 0.27f;
		line2.X2 = 3.869f;
		line2.Y1 = 1.573926f;
		line2.Y2 = 1.573926f;
		line4.Height = 0f;
		line4.Left = 0.27f;
		line4.LineWeight = 1f;
		line4.Name = "line4";
		line4.Top = 2.041777f;
		line4.Width = 3.599f;
		line4.X1 = 0.27f;
		line4.X2 = 3.869f;
		line4.Y1 = 2.041777f;
		line4.Y2 = 2.041777f;
		line5.Height = 0f;
		line5.Left = 0.27f;
		line5.LineWeight = 1f;
		line5.Name = "line5";
		line5.Top = 2.439f;
		line5.Width = 3.599f;
		line5.X1 = 0.27f;
		line5.X2 = 3.869f;
		line5.Y1 = 2.439f;
		line5.Y2 = 2.439f;
		line9.Height = 0f;
		line9.Left = 0.27f;
		line9.LineWeight = 1f;
		line9.Name = "line9";
		line9.Top = 3.334f;
		line9.Width = 3.592f;
		line9.X1 = 0.27f;
		line9.X2 = 3.862f;
		line9.Y1 = 3.334f;
		line9.Y2 = 3.334f;
		line15.Height = 0f;
		line15.Left = 0.27f;
		line15.LineWeight = 1f;
		line15.Name = "line15";
		line15.Top = 4.267966f;
		line15.Width = 3.599f;
		line15.X1 = 0.27f;
		line15.X2 = 3.869f;
		line15.Y1 = 4.267966f;
		line15.Y2 = 4.267966f;
		line3.Height = 1.34f;
		line3.Left = 1.013f;
		line3.LineWeight = 1f;
		line3.Name = "line3";
		line3.Top = 1.099f;
		line3.Width = 0f;
		line3.X1 = 1.013f;
		line3.X2 = 1.013f;
		line3.Y1 = 1.099f;
		line3.Y2 = 2.439f;
		line7.Height = 0.8950002f;
		line7.Left = 2.741f;
		line7.LineWeight = 1f;
		line7.Name = "line7";
		line7.Top = 2.439f;
		line7.Width = 0f;
		line7.X1 = 2.741f;
		line7.X2 = 2.741f;
		line7.Y1 = 2.439f;
		line7.Y2 = 3.334f;
		line16.Height = 0.9380083f;
		line16.Left = 1.531f;
		line16.LineWeight = 1f;
		line16.Name = "line16";
		line16.Top = 4.034f;
		line16.Width = 0f;
		line16.X1 = 1.531f;
		line16.X2 = 1.531f;
		line16.Y1 = 4.034f;
		line16.Y2 = 4.972008f;
		line17.Height = 0.9380093f;
		line17.Left = 2.740999f;
		line17.LineWeight = 1f;
		line17.Name = "line17";
		line17.Top = 4.034f;
		line17.Width = 9.536743E-07f;
		line17.X1 = 2.741f;
		line17.X2 = 2.740999f;
		line17.Y1 = 4.034f;
		line17.Y2 = 4.972009f;
		label16.Height = 0.202362f;
		label16.HyperLink = null;
		label16.Left = 0.27f;
		label16.Name = "label16";
		label16.Style = "font-family: Arial; font-size: 9pt; font-weight: normal; vertical-align: middle";
		label16.Text = "Issued Date :";
		label16.Top = 5.060009f;
		label16.Width = 0.807f;
		label17.DataField = "PRINTED_DATE";
		label17.Height = 0.202362f;
		label17.HyperLink = null;
		label17.Left = 1.128f;
		label17.Name = "label17";
		label17.Style = "font-family: Arial; font-size: 9pt; font-weight: normal; vertical-align: middle";
		label17.Text = "PRINTED_DATE";
		label17.Top = 5.060009f;
		label17.Width = 1.014961f;
		label18.Height = 0.202362f;
		label18.HyperLink = null;
		label18.Left = 2.854f;
		label18.Name = "label18";
		label18.Style = "font-family: Arial; font-size: 9pt; font-weight: normal; vertical-align: middle";
		label18.Text = "Printed by MCS";
		label18.Top = 5.060009f;
		label18.Width = 1.014961f;
		label1.Height = 0.1500002f;
		label1.HyperLink = null;
		label1.Left = 0.302f;
		label1.Name = "label1";
		label1.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label1.Text = "Org";
		label1.Top = 1.147f;
		label1.Width = 0.661f;
		label2.Height = 0.1500002f;
		label2.HyperLink = null;
		label2.Left = 0.302f;
		label2.Name = "label2";
		label2.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label2.Text = "Line / Prod.";
		label2.Top = 1.391f;
		label2.Width = 0.661f;
		label3.Height = 0.1500002f;
		label3.HyperLink = null;
		label3.Left = 0.302f;
		label3.Name = "label3";
		label3.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label3.Text = "Part No";
		label3.Top = 1.734f;
		label3.Width = 0.661f;
		label4.Height = 0.1399998f;
		label4.HyperLink = null;
		label4.Left = 0.446f;
		label4.Name = "label4";
		label4.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label4.Text = "Work Order";
		label4.Top = 2.49f;
		label4.Width = 0.867f;
		label5.Height = 0.1399998f;
		label5.HyperLink = null;
		label5.Left = 1.84f;
		label5.Name = "label5";
		label5.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label5.Text = "Plan Qty";
		label5.Top = 2.49f;
		label5.Width = 0.6000002f;
		label6.Height = 0.1399998f;
		label6.HyperLink = null;
		label6.Left = 2.909f;
		label6.Name = "label6";
		label6.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label6.Text = "Plan Date";
		label6.Top = 2.49f;
		label6.Width = 0.807f;
		label7.Height = 0.1500002f;
		label7.HyperLink = null;
		label7.Left = 0.302f;
		label7.Name = "label7";
		label7.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label7.Text = "Delivery";
		label7.Top = 2.169f;
		label7.Width = 0.661f;
		label13.Height = 0.202362f;
		label13.HyperLink = null;
		label13.Left = 0.486f;
		label13.Name = "label13";
		label13.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label13.Text = "Sender";
		label13.Top = 4.056f;
		label13.Width = 0.807f;
		label14.Height = 0.202362f;
		label14.HyperLink = null;
		label14.Left = 1.703f;
		label14.Name = "label14";
		label14.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label14.Text = "OQA";
		label14.Top = 4.056f;
		label14.Width = 0.807f;
		label15.Height = 0.202362f;
		label15.HyperLink = null;
		label15.Left = 2.877f;
		label15.Name = "label15";
		label15.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label15.Text = "PQA";
		label15.Top = 4.056f;
		label15.Width = 0.807f;
		barcode.BackColor = Color.FromArgb(255, 255, 255);
		barcode.DataField = "QRCODE_VALUE";
		barcode.Font = new Font("Courier New", 8f);
		barcode.Height = 0.6881945f;
		barcode.Left = 0.306f;
		barcode.Name = "barcode";
		barcode.QuietZoneBottom = 0f;
		barcode.QuietZoneLeft = 0f;
		barcode.QuietZoneRight = 0f;
		barcode.QuietZoneTop = 0f;
		barcode.Style = BarCodeStyle.QRCode;
		barcode.Text = "barcod";
		barcode.Top = 0.345f;
		barcode.Width = 0.6881945f;
		txtSheetID.CanGrow = false;
		txtSheetID.DataField = "SHEET_ID";
		txtSheetID.Height = 0.222047f;
		txtSheetID.Left = 1.064f;
		txtSheetID.Name = "txtSheetID";
		txtSheetID.Style = "background-color: Transparent; font-size: 10pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtSheetID.Text = "SHEET_ID";
		txtSheetID.Top = 0.846f;
		txtSheetID.Width = 2.808f;
		line19.Height = 0f;
		line19.Left = 0.27f;
		line19.LineWeight = 1f;
		line19.Name = "line19";
		line19.Top = 2.672297f;
		line19.Width = 3.619f;
		line19.X1 = 0.27f;
		line19.X2 = 3.889f;
		line19.Y1 = 2.672297f;
		line19.Y2 = 2.672297f;
		label12.Height = 0.15f;
		label12.HyperLink = null;
		label12.Left = 0.302f;
		label12.Name = "label12";
		label12.Style = "font-family: Arial; font-size: 8pt; font-weight: normal; text-align: center; vertical-align: middle";
		label12.Text = "Apply Model";
		label12.Top = 3.496f;
		label12.Width = 0.661f;
		txtOrg.CanGrow = false;
		txtOrg.DataField = "ORG_ID";
		txtOrg.Height = 0.16f;
		txtOrg.Left = 1.098f;
		txtOrg.Name = "txtOrg";
		txtOrg.Style = "background-color: Transparent; font-size: 9pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtOrg.Text = "ORG_ID";
		txtOrg.Top = 1.138f;
		txtOrg.Width = 1.394f;
		txtLine.CanGrow = false;
		txtLine.DataField = "LINE_CODE";
		txtLine.Height = 0.1590001f;
		txtLine.Left = 1.098f;
		txtLine.Name = "txtLine";
		txtLine.Style = "background-color: Transparent; font-size: 9pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtLine.Text = "LINE_CODE";
		txtLine.Top = 1.382f;
		txtLine.Width = 2.235001f;
		txtDelivery.CanGrow = false;
		txtDelivery.DataField = "BOXQTY";
		txtDelivery.Height = 0.222047f;
		txtDelivery.Left = 1.098f;
		txtDelivery.Name = "txtDelivery";
		txtDelivery.Style = "background-color: Transparent; font-size: 20.25pt; font-weight: bold; text-align: left; vertical-align: middle; ddo-char-set: 0";
		txtDelivery.Text = "BOXQTY";
		txtDelivery.Top = 2.132f;
		txtDelivery.Width = 2.235001f;
		txtWorkOrder1.CanGrow = false;
		txtWorkOrder1.DataField = "WORK_ORDER1";
		txtWorkOrder1.Height = 0.1599998f;
		txtWorkOrder1.Left = 0.295f;
		txtWorkOrder1.Name = "txtWorkOrder1";
		txtWorkOrder1.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtWorkOrder1.Text = "WORK_ORDER1";
		txtWorkOrder1.Top = 2.692f;
		txtWorkOrder1.Width = 1.216f;
		txtPlanQty1.CanGrow = false;
		txtPlanQty1.DataField = "PLAN_QTY1";
		txtPlanQty1.Height = 0.1599998f;
		txtPlanQty1.Left = 1.795f;
		txtPlanQty1.Name = "txtPlanQty1";
		txtPlanQty1.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtPlanQty1.Text = "PLAN_QTY1";
		txtPlanQty1.Top = 2.692f;
		txtPlanQty1.Width = 0.712f;
		txtPlanDate1.CanGrow = false;
		txtPlanDate1.DataField = "PLAN_DATE1";
		txtPlanDate1.Height = 0.1599998f;
		txtPlanDate1.Left = 2.909f;
		txtPlanDate1.Name = "txtPlanDate1";
		txtPlanDate1.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtPlanDate1.Text = "PLAN_DATE1";
		txtPlanDate1.Top = 2.692f;
		txtPlanDate1.Width = 0.827f;
		txtWorkOrder2.CanGrow = false;
		txtWorkOrder2.DataField = "WORK_ORDER2";
		txtWorkOrder2.Height = 0.1599998f;
		txtWorkOrder2.Left = 0.295f;
		txtWorkOrder2.Name = "txtWorkOrder2";
		txtWorkOrder2.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtWorkOrder2.Text = "WORK_ORDER2";
		txtWorkOrder2.Top = 2.852f;
		txtWorkOrder2.Width = 1.216f;
		txtPlanQty2.CanGrow = false;
		txtPlanQty2.DataField = "PLAN_QTY2";
		txtPlanQty2.Height = 0.1599998f;
		txtPlanQty2.Left = 1.795f;
		txtPlanQty2.Name = "txtPlanQty2";
		txtPlanQty2.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtPlanQty2.Text = "PLAN_QTY2";
		txtPlanQty2.Top = 2.852f;
		txtPlanQty2.Width = 0.712f;
		txtPlanDate2.CanGrow = false;
		txtPlanDate2.DataField = "PLAN_DATE2";
		txtPlanDate2.Height = 0.1599998f;
		txtPlanDate2.Left = 2.909f;
		txtPlanDate2.Name = "txtPlanDate2";
		txtPlanDate2.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtPlanDate2.Text = "PLAN_DATE2";
		txtPlanDate2.Top = 2.852f;
		txtPlanDate2.Width = 0.827f;
		txtWorkOrder3.CanGrow = false;
		txtWorkOrder3.DataField = "WORK_ORDER3";
		txtWorkOrder3.Height = 0.1599998f;
		txtWorkOrder3.Left = 0.295f;
		txtWorkOrder3.Name = "txtWorkOrder3";
		txtWorkOrder3.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtWorkOrder3.Text = "WORK_ORDER3";
		txtWorkOrder3.Top = 3.002f;
		txtWorkOrder3.Width = 1.216f;
		txtPlanQty3.CanGrow = false;
		txtPlanQty3.DataField = "PLAN_QTY3";
		txtPlanQty3.Height = 0.1599998f;
		txtPlanQty3.Left = 1.795f;
		txtPlanQty3.Name = "txtPlanQty3";
		txtPlanQty3.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtPlanQty3.Text = "PLAN_QTY3";
		txtPlanQty3.Top = 3.002f;
		txtPlanQty3.Width = 0.712f;
		txtPlanDate3.CanGrow = false;
		txtPlanDate3.DataField = "PLAN_DATE3";
		txtPlanDate3.Height = 0.1599998f;
		txtPlanDate3.Left = 2.909f;
		txtPlanDate3.Name = "txtPlanDate3";
		txtPlanDate3.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtPlanDate3.Text = "PLAN_DATE3";
		txtPlanDate3.Top = 3.002f;
		txtPlanDate3.Width = 0.827f;
		txtWorkOrder4.CanGrow = false;
		txtWorkOrder4.DataField = "WORK_ORDER4";
		txtWorkOrder4.Height = 0.1599998f;
		txtWorkOrder4.Left = 0.295f;
		txtWorkOrder4.Name = "txtWorkOrder4";
		txtWorkOrder4.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtWorkOrder4.Text = "WORK_ORDER4";
		txtWorkOrder4.Top = 3.162f;
		txtWorkOrder4.Width = 1.216f;
		txtPlanQty4.CanGrow = false;
		txtPlanQty4.DataField = "PLAN_QTY4";
		txtPlanQty4.Height = 0.1599998f;
		txtPlanQty4.Left = 1.795f;
		txtPlanQty4.Name = "txtPlanQty4";
		txtPlanQty4.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtPlanQty4.Text = "PLAN_QTY4";
		txtPlanQty4.Top = 3.162f;
		txtPlanQty4.Width = 0.712f;
		txtPlanDate4.CanGrow = false;
		txtPlanDate4.DataField = "PLAN_DATE4";
		txtPlanDate4.Height = 0.1599998f;
		txtPlanDate4.Left = 2.909f;
		txtPlanDate4.Name = "txtPlanDate4";
		txtPlanDate4.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtPlanDate4.Text = "PLAN_DATE4";
		txtPlanDate4.Top = 3.162001f;
		txtPlanDate4.Width = 0.827f;
		txtSpec1.CanGrow = false;
		txtSpec1.DataField = "PART_NO_1";
		txtSpec1.Height = 0.1599998f;
		txtSpec1.Left = 0.286f;
		txtSpec1.Name = "txtSpec1";
		txtSpec1.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtSpec1.Text = "PART_NO_1";
		txtSpec1.Top = 3.836f;
		txtSpec1.Width = 0.7050002f;
		txtSpec2.CanGrow = false;
		txtSpec2.DataField = "PART_NO_2";
		txtSpec2.Height = 0.1599998f;
		txtSpec2.Left = 1.017f;
		txtSpec2.Name = "txtSpec2";
		txtSpec2.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: center; vertical-align: middle";
		txtSpec2.Text = "PART_NO_2";
		txtSpec2.Top = 3.836f;
		txtSpec2.Width = 0.7050002f;
		txtSpec3.CanGrow = false;
		txtSpec3.DataField = "PART_NO_3";
		txtSpec3.Height = 0.1599998f;
		txtSpec3.Left = 1.765f;
		txtSpec3.Name = "txtSpec3";
		txtSpec3.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtSpec3.Text = "PART_NO_3";
		txtSpec3.Top = 3.836f;
		txtSpec3.Width = 2.067f;
		textBox1.CanGrow = false;
		textBox1.DataField = "PROD_DATE";
		textBox1.Height = 0.16f;
		textBox1.Left = 2.762f;
		textBox1.Name = "textBox1";
		textBox1.Style = "background-color: Transparent; font-size: 9pt; font-weight: normal; text-align: center; vertical-align: middle";
		textBox1.Text = "PROD_DATE";
		textBox1.Top = 1.138f;
		textBox1.Width = 1.1f;
		line8.Height = 0.8950002f;
		line8.Left = 1.531f;
		line8.LineWeight = 1f;
		line8.Name = "line8";
		line8.Top = 2.439f;
		line8.Width = 0f;
		line8.X1 = 1.531f;
		line8.X2 = 1.531f;
		line8.Y1 = 2.439f;
		line8.Y2 = 3.334f;
		textBox2.CanGrow = false;
		textBox2.DataField = "APPLY_MODEL";
		textBox2.Height = 0.358f;
		textBox2.Left = 1.098f;
		textBox2.Name = "textBox2";
		textBox2.Style = "background-color: Transparent; font-size: 7pt; font-weight: normal; text-align: left; vertical-align: middle";
		textBox2.Text = "APPLY_MODEL";
		textBox2.Top = 3.366f;
		textBox2.Width = 2.746f;
		line11.Height = 0.7019978f;
		line11.Left = 1.013f;
		line11.LineWeight = 1f;
		line11.Name = "line11";
		line11.Top = 3.334f;
		line11.Width = 0f;
		line11.X1 = 1.013f;
		line11.X2 = 1.013f;
		line11.Y1 = 3.334f;
		line11.Y2 = 4.035998f;
		line12.Height = 0.2339981f;
		line12.Left = 1.752f;
		line12.LineWeight = 1f;
		line12.Name = "line12";
		line12.Top = 3.802f;
		line12.Width = 0f;
		line12.X1 = 1.752f;
		line12.X2 = 1.752f;
		line12.Y1 = 3.802f;
		line12.Y2 = 4.035998f;
		line13.Height = 0.2410001f;
		line13.Left = 2.741f;
		line13.LineWeight = 1f;
		line13.Name = "line13";
		line13.Top = 1.099f;
		line13.Width = 0f;
		line13.X1 = 2.741f;
		line13.X2 = 2.741f;
		line13.Y1 = 1.099f;
		line13.Y2 = 1.34f;
		line14.Height = 0f;
		line14.Left = 0.27f;
		line14.LineWeight = 1f;
		line14.Name = "line14";
		line14.Top = 3.802f;
		line14.Width = 3.599f;
		line14.X1 = 0.27f;
		line14.X2 = 3.869f;
		line14.Y1 = 3.802f;
		line14.Y2 = 3.802f;
		txtDescription.CanGrow = false;
		txtDescription.DataField = "PART_NAME";
		txtDescription.Height = 0.1700001f;
		txtDescription.Left = 1.098f;
		txtDescription.Name = "txtDescription";
		txtDescription.Style = "background-color: Transparent; font-size: 11pt; font-weight: normal; text-align: left; vertical-align: middle";
		txtDescription.Text = "PART_NAME";
		txtDescription.Top = 1.846f;
		txtDescription.Width = 2.235001f;
		textPartNo.CanGrow = false;
		textPartNo.DataField = "PART_NO";
		textPartNo.Height = 0.222047f;
		textPartNo.Left = 1.098f;
		textPartNo.Name = "textPartNo";
		textPartNo.Style = "background-color: Transparent; font-size: 20pt; font-weight: bold; text-align: left; vertical-align: middle";
		textPartNo.Text = "PART_NO";
		textPartNo.Top = 1.634f;
		textPartNo.Width = 2.235001f;
		label8.DataField = "PB";
		label8.Height = 0.6881945f;
		label8.HyperLink = null;
		label8.Left = 3.264f;
		label8.Name = "label8";
		label8.Style = "font-family: Arial; font-size: 50pt; font-weight: bold; text-align: center; vertical-align: bottom; ddo-char-set: 1";
		label8.Text = "P";
		label8.Top = 0.335f;
		label8.Width = 0.6881945f;
		base.MasterReport = false;
		base.PageSettings.Margins.Bottom = 0f;
		base.PageSettings.Margins.Left = 0f;
		base.PageSettings.Margins.Right = 0f;
		base.PageSettings.Margins.Top = 0f;
		base.PageSettings.PaperHeight = 11f;
		base.PageSettings.PaperWidth = 8.5f;
		base.PrintWidth = 4.104f;
		base.Sections.Add(detail);
		base.StyleSheet.Add(new StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; color: Black", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: italic", "Heading2", "Normal"));
		base.StyleSheet.Add(new StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"));
		((ISupportInitialize)label11).EndInit();
		((ISupportInitialize)label16).EndInit();
		((ISupportInitialize)label17).EndInit();
		((ISupportInitialize)label18).EndInit();
		((ISupportInitialize)label1).EndInit();
		((ISupportInitialize)label2).EndInit();
		((ISupportInitialize)label3).EndInit();
		((ISupportInitialize)label4).EndInit();
		((ISupportInitialize)label5).EndInit();
		((ISupportInitialize)label6).EndInit();
		((ISupportInitialize)label7).EndInit();
		((ISupportInitialize)label13).EndInit();
		((ISupportInitialize)label14).EndInit();
		((ISupportInitialize)label15).EndInit();
		((ISupportInitialize)txtSheetID).EndInit();
		((ISupportInitialize)label12).EndInit();
		((ISupportInitialize)txtOrg).EndInit();
		((ISupportInitialize)txtLine).EndInit();
		((ISupportInitialize)txtDelivery).EndInit();
		((ISupportInitialize)txtWorkOrder1).EndInit();
		((ISupportInitialize)txtPlanQty1).EndInit();
		((ISupportInitialize)txtPlanDate1).EndInit();
		((ISupportInitialize)txtWorkOrder2).EndInit();
		((ISupportInitialize)txtPlanQty2).EndInit();
		((ISupportInitialize)txtPlanDate2).EndInit();
		((ISupportInitialize)txtWorkOrder3).EndInit();
		((ISupportInitialize)txtPlanQty3).EndInit();
		((ISupportInitialize)txtPlanDate3).EndInit();
		((ISupportInitialize)txtWorkOrder4).EndInit();
		((ISupportInitialize)txtPlanQty4).EndInit();
		((ISupportInitialize)txtPlanDate4).EndInit();
		((ISupportInitialize)txtSpec1).EndInit();
		((ISupportInitialize)txtSpec2).EndInit();
		((ISupportInitialize)txtSpec3).EndInit();
		((ISupportInitialize)textBox1).EndInit();
		((ISupportInitialize)textBox2).EndInit();
		((ISupportInitialize)txtDescription).EndInit();
		((ISupportInitialize)textPartNo).EndInit();
		((ISupportInitialize)label8).EndInit();
		((ISupportInitialize)this).EndInit();
	}
}
