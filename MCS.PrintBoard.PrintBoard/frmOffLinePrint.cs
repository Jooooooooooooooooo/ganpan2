using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FarPoint.Win;
using FarPoint.Win.Spread;
using MCS.Common;
using MCS.Common.Controls;

namespace MCS.PrintBoard.PrintBoard;

public class frmOffLinePrint : frmBase
{
	private PrintInfo pi = new PrintInfo();

	private FarPoint.Win.Spread.SheetView svPrintContent = new FarPoint.Win.Spread.SheetView();

	private MCS.Common.SheetView svPrintBoard;

	private MCS.Common.SheetView svPrintMade;

	private TextBox[] tbary = new TextBox[10];

	private string[] CurrentItem = new string[4];

	private int iWO = -1;

	private Random ran = new Random();

	private static DataTable dtPrintMain = new DataTable();

	private static DataTable dtGroupBy = new DataTable();

	private static DataTable dtReturn = new DataTable();

	private IContainer components = null;

	private System.Windows.Forms.Timer tmDemo;

	private DataGridView dgvBuffer;

	public System.Windows.Forms.Timer tmRefresh;

	private SearchPanel searchPanel1;

	private SplitContainer splitContainerMain;

	private BackPanel backPanel2;

	private BackPanel backPanel1;

	private MCS.Common.FpSpread fpProdResult;

	private MCS.Common.FpSpread fpPrintMain;

	private System.Windows.Forms.Button btn_New;

	private PanelOnly panelOnly1;

	private Panel panel1;

	private Label label8;

	private TextBox txtOrg;

	private Label label2;

	private System.Windows.Forms.Button btn_org_change1;

	private System.Windows.Forms.Button btn_excel;

	private SplitContainer splitContainer1;

	private SplitContainer splitContainer2;

	private Panel panel3;

	private System.Windows.Forms.Button btn_Down;

	private MCS.Common.FpSpread fpPrintMade;

	private RadioButton rdoWorkOrder;

	private RadioButton rdoCarrierQty;

	private Label label1;

	private UserBox userBox1;

	private Label label4;

	private Label label5;

	private Panel panel2;

	private RadioButton rdoLabel;

	private Label label3;

	private RadioButton rdoA4;

	private UserBox userBox2;

	private System.Windows.Forms.Button btn_preview;

	private RadioButton rdoGsrm;

	private RadioButton rdoLabelA4;

	private RadioButton rdoNV;

	private Panel panel5;

	private RadioButton rdoB;

	private RadioButton rdoP;

	private UserBox userBox5;

	private RadioButton rdoZebra;

	private System.Windows.Forms.Button btn_Setting;

	public frmOffLinePrint()
	{
		InitializeComponent();
	}

	private void btn_Org_Change_Click(object sender, EventArgs e)
	{
		using frmOrgSave_P frm = new frmOrgSave_P();
		frm.sOrg = "";
		if (frm.ShowDialog() == DialogResult.OK)
		{
			if (frm.sOrg.Length == 3)
			{
				txtOrg.Text = frm.sOrg;
			}
			setrdoVisible();
		}
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		procMakeSheetColumn();
		GetOrg();
	}

	private void fpPrintMain_CellClick(object sender, CellClickEventArgs e)
	{
	}

	private void btn_preview_Click(object sender, EventArgs e)
	{
		dtGroupBy = (DataTable)fpPrintMade.ActiveSheet.DataSource;
		string sPrintType = "";
		string sOrg = txtOrg.Text.Trim();
		try
		{
			if (dtGroupBy.Rows.Count > 0)
			{
				for (int i = 0; dtGroupBy.Rows.Count > i; i++)
				{
					dtGroupBy.Rows[i]["QRCODE_VALUE"] = MakeQRData(i);
					if (rdoP.Checked)
					{
						dtGroupBy.Rows[i]["PB"] = "P";
					}
					else
					{
						dtGroupBy.Rows[i]["PB"] = "";
					}
				}
				sPrintType = (rdoA4.Checked ? "A4" : (rdoNV.Checked ? "NV" : (rdoGsrm.Checked ? "GSRM" : (rdoLabelA4.Checked ? "LabelA4" : ((!rdoZebra.Checked) ? "Label" : "Zebra")))));
				if (sOrg == "CNZ" && sPrintType == "Zebra")
				{
					ZebraLabelPrint(dtGroupBy);
					return;
				}
				frmOffLinePrintPreview frm = new frmOffLinePrintPreview(sPrintType, sOrg);
				frm.dtGroupBy = dtGroupBy;
				frm.ShowDialog();
			}
			else
			{
				MessageBox.Show("There is no Data to Print!!");
			}
		}
		catch
		{
		}
	}

	private void btn_New_Click(object sender, EventArgs e)
	{
		procMakeSheetColumn();
		dtPrintMain.Clear();
		dtGroupBy.Clear();
		fpPrintMain.ActiveSheet.Rows.Clear();
		fpPrintMade.ActiveSheet.Rows.Clear();
		fpPrintMain.ActiveSheet.Rows.Add(0, 100);
	}

	private void btn_Setting_Click(object sender, EventArgs e)
	{
		try
		{
			PrintLabel(null, bSetting: false);
			string strPortName = GetConfigValue("ASSET_PORT");
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private bool GetOrg()
	{
		bool bReturn = false;
		try
		{
			string sOrg = "";
			string path = Application.StartupPath;
			string filePath = path + "\\\\ORG.txt";
			FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
			using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
			{
				sOrg = streamReader.ReadToEnd();
				txtOrg.Text = sOrg;
				if (sOrg.Length > 2)
				{
					bReturn = true;
				}
			}
			setrdoVisible();
		}
		catch (Exception)
		{
			using frmOrgSave_P frm = new frmOrgSave_P();
			frm.sOrg = "";
			if (frm.ShowDialog() == DialogResult.OK)
			{
				txtOrg.Text = frm.sOrg;
				if (txtOrg.Text.Length > 2)
				{
					bReturn = true;
				}
			}
		}
		return bReturn;
	}

	private void ZebraLabelPrint(DataTable dtGroupBy)
	{
		try
		{
			string sAsNo = string.Empty;
			string sAsName = string.Empty;
			string sDepart = string.Empty;
			int iQty = 0;
			DataRow dr = dtGroupBy.NewRow();
			if (dtGroupBy.Rows.Count > 0)
			{
				for (int i = 0; i < dtGroupBy.Rows.Count; i++)
				{
					dr = dtGroupBy.Rows[i];
					PrintLabel(dr, bSetting: false);
				}
				iQty++;
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void procMakeSheetColumn()
	{
		try
		{
			if (fpPrintMain.Sheets.Count > 0)
			{
				fpPrintMain.Sheets.RemoveAt(0);
			}
			if (fpPrintMade.Sheets.Count > 0)
			{
				fpPrintMade.Sheets.RemoveAt(0);
			}
			MCS.Common.SheetView svPrintBoard = new MCS.Common.SheetView(fpPrintMain, "Offline PrintBoard", OperationMode.Normal, bRowHeaderVisible: true);
			MCS.Common.SheetView svPrintMade = new MCS.Common.SheetView(fpPrintMade, "Offline PrintBoard", OperationMode.Normal, bRowHeaderVisible: true);
			svPrintBoard.AddColumnText("Line", "LINE", 60, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintBoard.AddColumnText("Made By", "MADE_BY", 90, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 150);
			svPrintBoard.AddColumnText("Carrier ID", "CARRIER_ID", 90, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintBoard.AddColumnText("  Locator  Group", "LOCATOR_GROUP", 70, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintBoard.AddColumnText("Inspection", "INSPECTION_FLAG", 110, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 80);
			svPrintBoard.AddColumnText("PST", "PST", 160, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 120);
			svPrintBoard.AddColumnText("Part No", "PART_NO", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintBoard.AddColumnNumber("Qty", "WO_QUANTITY", 30, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 0, 10000.0, 0.0, bNegativeRed: true);
			svPrintBoard.AddColumnText("Description", "DESCRIPTION", 150, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 300);
			svPrintBoard.AddColumnText("Seq From", "PROD_SEQ_FROM", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintBoard.AddColumnText("   Seq    To", "PROD_SEQ_TO", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintBoard.AddColumnText("Model.Suffix", "MODEL_SUFFIX", 150, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintBoard.AddColumnText("Worker Order", "WORKER_ORDER", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintBoard.AddColumnText("Carrier Qty", "CARRIER_QTY", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("", "GROUPID", 0, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 200);
			svPrintMade.AddColumnText("Line", "LINE", 60, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Made By", "MADE_BY", 90, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 150);
			svPrintMade.AddColumnText("Carrier ID", "CARRIER_ID", 90, CellHorizontalAlignment.Center, bLocked: false, bVisible: true, 200);
			svPrintMade.AddColumnText("  Locator  Group", "LOCATOR_GROUP", 70, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Inspection", "INSPECTION_FLAG", 110, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 80);
			svPrintMade.AddColumnText("PST", "PST", 160, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 120);
			svPrintMade.AddColumnText("Part No", "PART_NO", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Description", "DESCRIPTION", 150, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 300);
			svPrintMade.AddColumnText("Total Qty", "TOTAL_QUANTITY", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Total SeqNO", "TOTAL_SEQ_NO", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("sheet ID", "SHEET_ID", 100, CellHorizontalAlignment.Left, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("", "PRINTED_DATE", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 200);
			svPrintMade.AddColumnText("", "QRCODE_VALUE", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: false, 200);
			svPrintMade.AddColumnNumber("Qty1", "WO_QUANTITY1", 40, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 0, 10000.0, 0.0, bNegativeRed: true);
			svPrintMade.AddColumnText("Seq From1", "PROD_SEQ_FROM1", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("   Seq    To1", "PROD_SEQ_TO1", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Model.Suffix1", "MODEL_SUFFIX1", 150, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Worker Order1", "WORKER_ORDER1", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnNumber("Qty2", "WO_QUANTITY2", 40, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 0, 10000.0, 0.0, bNegativeRed: true);
			svPrintMade.AddColumnText("Seq From2", "PROD_SEQ_FROM2", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("   Seq    To2", "PROD_SEQ_TO2", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Model.Suffix2", "MODEL_SUFFIX2", 150, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Worker Order2", "WORKER_ORDER2", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnNumber("Qty3", "WO_QUANTITY3", 40, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 0, 10000.0, 0.0, bNegativeRed: true);
			svPrintMade.AddColumnText("Seq From3", "PROD_SEQ_FROM3", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("   Seq    To3", "PROD_SEQ_TO3", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Model.Suffix3", "MODEL_SUFFIX3", 150, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Worker Order3", "WORKER_ORDER3", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnNumber("Qty4", "WO_QUANTITY4", 40, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 0, 10000.0, 0.0, bNegativeRed: true);
			svPrintMade.AddColumnText("Seq From4", "PROD_SEQ_FROM4", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("   Seq    To4", "PROD_SEQ_TO4", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Model.Suffix4", "MODEL_SUFFIX4", 150, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Worker Order4", "WORKER_ORDER4", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnNumber("Qty5", "WO_QUANTITY5", 40, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 0, 10000.0, 0.0, bNegativeRed: true);
			svPrintMade.AddColumnText("Seq From5", "PROD_SEQ_FROM5", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("   Seq    To5", "PROD_SEQ_TO5", 47, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Model.Suffix5", "MODEL_SUFFIX5", 150, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			svPrintMade.AddColumnText("Worker Order5", "WORKER_ORDER5", 100, CellHorizontalAlignment.Center, bLocked: true, bVisible: true, 200);
			if (!dtPrintMain.Columns.Contains("LINE"))
			{
				dtPrintMain.Columns.Add("LINE");
			}
			if (!dtPrintMain.Columns.Contains("MADE_BY"))
			{
				dtPrintMain.Columns.Add("MADE_BY");
			}
			if (!dtPrintMain.Columns.Contains("CARRIER_ID"))
			{
				dtPrintMain.Columns.Add("CARRIER_ID");
			}
			if (!dtPrintMain.Columns.Contains("LOCATOR_GROUP"))
			{
				dtPrintMain.Columns.Add("LOCATOR_GROUP");
			}
			if (!dtPrintMain.Columns.Contains("INSPECTION_FLAG"))
			{
				dtPrintMain.Columns.Add("INSPECTION_FLAG");
			}
			if (!dtPrintMain.Columns.Contains("PST"))
			{
				dtPrintMain.Columns.Add("PST");
			}
			if (!dtPrintMain.Columns.Contains("PART_NO"))
			{
				dtPrintMain.Columns.Add("PART_NO");
			}
			if (!dtPrintMain.Columns.Contains("WO_QUANTITY"))
			{
				dtPrintMain.Columns.Add("WO_QUANTITY", typeof(long));
			}
			if (!dtPrintMain.Columns.Contains("DESCRIPTION"))
			{
				dtPrintMain.Columns.Add("DESCRIPTION");
			}
			if (!dtPrintMain.Columns.Contains("PROD_SEQ_FROM"))
			{
				dtPrintMain.Columns.Add("PROD_SEQ_FROM");
			}
			if (!dtPrintMain.Columns.Contains("PROD_SEQ_TO"))
			{
				dtPrintMain.Columns.Add("PROD_SEQ_TO");
			}
			if (!dtPrintMain.Columns.Contains("MODEL_SUFFIX"))
			{
				dtPrintMain.Columns.Add("MODEL_SUFFIX");
			}
			if (!dtPrintMain.Columns.Contains("WORKER_ORDER"))
			{
				dtPrintMain.Columns.Add("WORKER_ORDER");
			}
			if (!dtPrintMain.Columns.Contains("CARRIER_QTY"))
			{
				dtPrintMain.Columns.Add("CARRIER_QTY");
			}
			if (!dtPrintMain.Columns.Contains("SEQ"))
			{
				dtPrintMain.Columns.Add("SEQ", typeof(long));
			}
			if (!dtPrintMain.Columns.Contains("GROUPID"))
			{
				dtPrintMain.Columns.Add("GROUPID");
			}
			if (!dtPrintMain.Columns.Contains("CHECKYN"))
			{
				dtPrintMain.Columns.Add("CHECKYN");
			}
			svPrintBoard.RowHeader.Visible = true;
			svPrintBoard.Rows.Default.Height = 22f;
			svPrintBoard.ColumnHeader.Rows[0].Height = 40f;
			svPrintMade.Rows.Default.Height = 22f;
			svPrintMade.ColumnHeader.Rows[0].Height = 40f;
			svPrintBoard.Rows.Add(0, 100);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool GetDataFromfpPrintMain()
	{
		string sValue = string.Empty;
		DataTable dtGroupTemp = new DataTable();
		DataTable dtFpTemp = new DataTable();
		bool bReturn = false;
		try
		{
			if (fpPrintMain.ActiveSheet.Rows.Count > 0)
			{
				for (int r = 0; fpPrintMain.ActiveSheet.Rows.Count > r; r++)
				{
					object sPART_NO = fpPrintMain.ActiveSheet.GetValue(r, "PART_NO");
					object sWO_QUANTITY = fpPrintMain.ActiveSheet.GetValue(r, "WO_QUANTITY");
					object sINSPECTION_FLAG = fpPrintMain.ActiveSheet.GetValue(r, "INSPECTION_FLAG");
					object sPROD_SEQ_FROM = fpPrintMain.ActiveSheet.GetValue(r, "PROD_SEQ_FROM");
					object sPROD_SEQ_TO = fpPrintMain.ActiveSheet.GetValue(r, "PROD_SEQ_TO");
					object sCARRIER_QTY = fpPrintMain.ActiveSheet.GetValue(r, "CARRIER_QTY");
					int iQty = -100;
					int iFrom = -100;
					int iTo = -100;
					int iCarrierQty = -100;
					if (sPART_NO == null || sWO_QUANTITY == null)
					{
						continue;
					}
					if (sINSPECTION_FLAG != null)
					{
						if (!(sINSPECTION_FLAG.ToString().ToUpper() == "INSPECTION") && !(sINSPECTION_FLAG.ToString().ToUpper() == "NON INSPECTION"))
						{
							MessageBox.Show("[Row" + (r + 1) + "] Inspection is not correct. \r\n(\"INSPECTION\" or \"NON INSPECTION\") ");
							return false;
						}
						if (sWO_QUANTITY != null)
						{
							try
							{
								iQty = int.Parse(sWO_QUANTITY.ToString());
							}
							catch (Exception)
							{
							}
						}
						if (sCARRIER_QTY != null)
						{
							try
							{
								iCarrierQty = int.Parse(sCARRIER_QTY.ToString());
							}
							catch (Exception)
							{
							}
						}
						if (iQty == -100)
						{
							MessageBox.Show("[Row" + (r + 1) + "] Qty is not correct.");
							return false;
						}
						if (iCarrierQty == -100)
						{
							MessageBox.Show("[Row [" + (r + 1) + "] Carrier Qty is not correct.");
							return false;
						}
					}
					else if (!txtOrg.Text.Contains("EN7"))
					{
						MessageBox.Show("[Row" + (r + 1) + "] Inspection is not correct. \r\n(\"INSPECTION\" or \"NON INSPECTION\") ");
						return false;
					}
				}
			}
			if (dtPrintMain.Rows.Count > 0)
			{
				dtFpTemp = dtPrintMain.Clone();
				for (int j = dtPrintMain.Rows.Count - 1; j > 0; j--)
				{
					if (dtPrintMain.Rows[j][0].ToString().Length == 0)
					{
						dtPrintMain.Rows.RemoveAt(j);
					}
				}
				if (dtPrintMain.Rows.Count > 0)
				{
					dtFpTemp = dtPrintMain.Copy();
				}
			}
			dtPrintMain.Clear();
			dtGroupBy.Clear();
			dtGroupBy.Reset();
			if (dtFpTemp.Rows.Count > 0)
			{
				fpPrintMain.ActiveSheet.DataSource = dtFpTemp;
			}
			try
			{
				for (int r2 = 0; fpPrintMain.ActiveSheet.Rows.Count > r2; r2++)
				{
					DataRow dr = dtPrintMain.NewRow();
					for (int c = 0; fpPrintMain.ActiveSheet.Columns.Count > c; c++)
					{
						if (fpPrintMain.ActiveSheet.GetValue(r2, c) != null)
						{
							sValue = fpPrintMain.ActiveSheet.GetValue(r2, c).ToString();
							if (!string.IsNullOrEmpty(sValue))
							{
								dr[c] = sValue;
							}
						}
					}
					if (dr["LINE"].ToString() != "" && dr["LOCATOR_GROUP"].ToString() != "" && dr["PART_NO"].ToString() != "" && dr["WORKER_ORDER"].ToString() != "" && dr["WO_QUANTITY"].ToString() != "" && dr["MODEL_SUFFIX"].ToString() != "")
					{
						dtPrintMain.Rows.Add(dr);
					}
				}
				fpPrintMain.ActiveSheet.DataSource = dtPrintMain;
				dtGroupTemp = MakeCarrierLoading(dtPrintMain);
				string[] sColnames = new string[1] { "GROUPID" };
				dtGroupBy = dtGroupTemp.DefaultView.ToTable(distinct: true, sColnames);
				dtGroupBy.Columns.Add("LINE");
				dtGroupBy.Columns.Add("LOCATOR_GROUP");
				dtGroupBy.Columns.Add("PART_NO");
				dtGroupBy.Columns.Add("CARRIER_ID");
				dtGroupBy.Columns.Add("SHEET_ID");
				dtGroupBy.Columns.Add("TOTAL_QUANTITY");
				dtGroupBy.Columns.Add("MADE_BY");
				dtGroupBy.Columns.Add("TOTAL_SEQ_NO");
				dtGroupBy.Columns.Add("DESCRIPTION");
				dtGroupBy.Columns.Add("INSPECTION_FLAG");
				dtGroupBy.Columns.Add("PST");
				dtGroupBy.Columns.Add("PRINTED_DATE");
				dtGroupBy.Columns.Add("QRCODE_VALUE");
				dtGroupBy.Columns.Add("WO_QUANTITY1");
				dtGroupBy.Columns.Add("PROD_SEQ_FROM1");
				dtGroupBy.Columns.Add("PROD_SEQ_TO1");
				dtGroupBy.Columns.Add("MODEL_SUFFIX1");
				dtGroupBy.Columns.Add("WORKER_ORDER1");
				dtGroupBy.Columns.Add("WO_QUANTITY2");
				dtGroupBy.Columns.Add("PROD_SEQ_FROM2");
				dtGroupBy.Columns.Add("PROD_SEQ_TO2");
				dtGroupBy.Columns.Add("MODEL_SUFFIX2");
				dtGroupBy.Columns.Add("WORKER_ORDER2");
				dtGroupBy.Columns.Add("WO_QUANTITY3");
				dtGroupBy.Columns.Add("PROD_SEQ_FROM3");
				dtGroupBy.Columns.Add("PROD_SEQ_TO3");
				dtGroupBy.Columns.Add("MODEL_SUFFIX3");
				dtGroupBy.Columns.Add("WORKER_ORDER3");
				dtGroupBy.Columns.Add("WO_QUANTITY4");
				dtGroupBy.Columns.Add("PROD_SEQ_FROM4");
				dtGroupBy.Columns.Add("PROD_SEQ_TO4");
				dtGroupBy.Columns.Add("MODEL_SUFFIX4");
				dtGroupBy.Columns.Add("WORKER_ORDER4");
				dtGroupBy.Columns.Add("WO_QUANTITY5");
				dtGroupBy.Columns.Add("PROD_SEQ_FROM5");
				dtGroupBy.Columns.Add("PROD_SEQ_TO5");
				dtGroupBy.Columns.Add("MODEL_SUFFIX5");
				dtGroupBy.Columns.Add("WORKER_ORDER5");
				dtGroupBy.Columns.Add("PB");
				for (int i = 0; dtGroupBy.Rows.Count > i; i++)
				{
					string sGROUPID = dtGroupBy.Rows[i]["GROUPID"].ToString();
					string sFilterExpression = $"GROUPID = '{sGROUPID}'";
					int iTOTAL_QUANTITY = 0;
					string sTOTAL_SEQ_NO_FROM = string.Empty;
					string sTOTAL_SEQ_NO_TO = string.Empty;
					string sTOTAL_SEQ = string.Empty;
					string sWORKER_ORDER = string.Empty;
					string sWO_QUANTITY2 = string.Empty;
					string sCARRIER_ID = string.Empty;
					string sORG = txtOrg.Text.Trim();
					sORG += "F";
					string sSheetID = sORG + DateTime.Now.ToString("yyMMddHHmmssffffff").ToString();
					Thread.Sleep(20);
					DataRow[] Rows = dtGroupTemp.Select(sFilterExpression);
					int l = 0;
					for (int k = 0; Rows.Length > k && 5 > k; k++)
					{
						string sBefore_WO = sWORKER_ORDER;
						string sBrefore_Qty = sWO_QUANTITY2;
						string sBefore_Carrier = sCARRIER_ID;
						sWORKER_ORDER = Rows[k]["WORKER_ORDER"].ToString();
						sWO_QUANTITY2 = Rows[k]["WO_QUANTITY"].ToString();
						sCARRIER_ID = Rows[k]["CARRIER_ID"].ToString();
						if (sBefore_Carrier == sCARRIER_ID && sBefore_WO == sWORKER_ORDER && rdoCarrierQty.Checked)
						{
							string WO_QUANTITY_Name = "WO_QUANTITY" + l;
							if (sTOTAL_SEQ_NO_FROM.Length + sTOTAL_SEQ_NO_TO.Length > 0)
							{
								sTOTAL_SEQ = sTOTAL_SEQ_NO_FROM + "  ~  " + sTOTAL_SEQ_NO_TO;
							}
							else
							{
								sTOTAL_SEQ = "";
							}
							iTOTAL_QUANTITY += int.Parse(sWO_QUANTITY2);
							dtGroupBy.Rows[i]["TOTAL_QUANTITY"] = iTOTAL_QUANTITY.ToString();
							dtGroupBy.Rows[i][WO_QUANTITY_Name] = iTOTAL_QUANTITY.ToString();
							continue;
						}
						string sLINE = Rows[k]["LINE"].ToString();
						string sLOCATOR_GROUP = Rows[k]["LOCATOR_GROUP"].ToString();
						string sPART_NO2 = Rows[k]["PART_NO"].ToString();
						string sMADE_BY = Rows[k]["MADE_BY"].ToString();
						string sDESCRIPTION = Rows[k]["DESCRIPTION"].ToString();
						string sINSPECTION_FLAG2 = Rows[k]["INSPECTION_FLAG"].ToString();
						string sPST = Rows[k]["PST"].ToString();
						string sPROD_SEQ_FROM2 = Rows[k]["PROD_SEQ_FROM"].ToString();
						string sPROD_SEQ_TO2 = Rows[k]["PROD_SEQ_TO"].ToString();
						string sMODEL_SUFFIX = Rows[k]["MODEL_SUFFIX"].ToString();
						if (string.IsNullOrEmpty(sWO_QUANTITY2))
						{
							sWO_QUANTITY2 = "0";
						}
						string WORKER_ORDER_Name = "WORKER_ORDER" + (l + 1);
						string WO_QUANTITY_Name2 = "WO_QUANTITY" + (l + 1);
						string PROD_SEQ_FROM_Name = "PROD_SEQ_FROM" + (l + 1);
						string PROD_SEQ_TO_Name = "PROD_SEQ_TO" + (l + 1);
						string MODEL_SUFFIX_Name = "MODEL_SUFFIX" + (l + 1);
						if (k == 0)
						{
							dtGroupBy.Rows[i]["LINE"] = sLINE;
							dtGroupBy.Rows[i]["LOCATOR_GROUP"] = sLOCATOR_GROUP;
							dtGroupBy.Rows[i]["PART_NO"] = sPART_NO2;
							dtGroupBy.Rows[i]["PST"] = sPST;
							dtGroupBy.Rows[i]["INSPECTION_FLAG"] = sINSPECTION_FLAG2;
							dtGroupBy.Rows[i]["DESCRIPTION"] = sDESCRIPTION;
							dtGroupBy.Rows[i]["MADE_BY"] = sMADE_BY;
							dtGroupBy.Rows[i]["CARRIER_ID"] = sCARRIER_ID;
							sTOTAL_SEQ_NO_FROM = sPROD_SEQ_FROM2;
						}
						sTOTAL_SEQ_NO_TO = sPROD_SEQ_TO2;
						sTOTAL_SEQ = ((sTOTAL_SEQ_NO_FROM.Length + sTOTAL_SEQ_NO_TO.Length <= 0) ? "" : (sTOTAL_SEQ_NO_FROM + "  ~  " + sTOTAL_SEQ_NO_TO));
						iTOTAL_QUANTITY += int.Parse(sWO_QUANTITY2);
						dtGroupBy.Rows[i]["SHEET_ID"] = sSheetID;
						dtGroupBy.Rows[i]["PRINTED_DATE"] = DateTime.Now.ToString("yyyy.MM.dd HH:mm");
						dtGroupBy.Rows[i]["TOTAL_QUANTITY"] = iTOTAL_QUANTITY.ToString();
						dtGroupBy.Rows[i]["TOTAL_SEQ_NO"] = sTOTAL_SEQ;
						dtGroupBy.Rows[i][WO_QUANTITY_Name2] = sWO_QUANTITY2;
						dtGroupBy.Rows[i][PROD_SEQ_FROM_Name] = sPROD_SEQ_FROM2;
						dtGroupBy.Rows[i][PROD_SEQ_TO_Name] = sPROD_SEQ_TO2;
						dtGroupBy.Rows[i][MODEL_SUFFIX_Name] = sMODEL_SUFFIX;
						dtGroupBy.Rows[i][WORKER_ORDER_Name] = sWORKER_ORDER;
						dtGroupBy.Rows[i]["QRCODE_VALUE"] = MakeQRData(i);
						l++;
					}
				}
				bReturn = dtGroupBy.Rows.Count > 0;
			}
			catch (Exception)
			{
				return false;
			}
			return bReturn;
		}
		catch (Exception)
		{
			return false;
		}
	}

	private string MakeQRData(int iRow)
	{
		StringBuilder sbData = new StringBuilder();
		string sResult = string.Empty;
		char sSTX = '\u0002';
		char sETX = '\u0003';
		try
		{
			sbData.Append(dtGroupBy.Rows[iRow]["SHEET_ID"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["LINE"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PART_NO"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["TOTAL_QUANTITY"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["CARRIER_ID"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["TOTAL_SEQ_NO"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WO_QUANTITY1"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_FROM1"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_TO1"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["MODEL_SUFFIX1"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WORKER_ORDER1"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WO_QUANTITY2"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_FROM2"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_TO2"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["MODEL_SUFFIX2"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WORKER_ORDER2"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WO_QUANTITY3"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_FROM3"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_TO3"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["MODEL_SUFFIX3"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WORKER_ORDER3"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WO_QUANTITY4"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_FROM4"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_TO4"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["MODEL_SUFFIX4"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WORKER_ORDER4"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WO_QUANTITY5"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_FROM5"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["PROD_SEQ_TO5"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["MODEL_SUFFIX5"].ToString());
			sbData.Append("¿");
			sbData.Append(dtGroupBy.Rows[iRow]["WORKER_ORDER5"].ToString());
			sResult = sbData.ToString();
			sResult = sSTX + sResult + sETX;
		}
		catch (Exception)
		{
		}
		return sResult;
	}

	private void btn_excel_Click(object sender, EventArgs e)
	{
		try
		{
			string xlFileName = "";
			xlFileName = ((!(txtOrg.Text.Trim() == "CNZ")) ? (Environment.CurrentDirectory + "\\Template_Offline_Sheet.xlsx") : (Environment.CurrentDirectory + "\\Template_Offline_Sheet_REF.xlsx"));
			ExcelAutoMation.ExecExcel(xlFileName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private DataTable MakeCarrierLoading(DataTable pdtPrintMain)
	{
		string sValue = string.Empty;
		string sCurrPartNo = string.Empty;
		string sCurrLocatorGroup = string.Empty;
		string sCurrLine = string.Empty;
		string sCurrWO = string.Empty;
		string sBeforePartNo = string.Empty;
		string sBeforeLocatorGroup = string.Empty;
		string sBeforeLine = string.Empty;
		string sBeforeWO = string.Empty;
		string sLocator = string.Empty;
		int iGroupID = 1;
		int iLeftQty = 0;
		int iQty = 0;
		int iGroupIDRowCount = 0;
		int iPackUnitqty = 0;
		int iGroupIDSumQty = 0;
		Dictionary<string, string> dicPackingQty = new Dictionary<string, string>();
		DataRow dr = pdtPrintMain.NewRow();
		dtReturn = pdtPrintMain.Clone();
		try
		{
			for (int j = 0; pdtPrintMain.Rows.Count > j; j++)
			{
				pdtPrintMain.Rows[j]["SEQ"] = j;
				pdtPrintMain.Rows[j]["CHECKYN"] = "N";
				string sPartNo = pdtPrintMain.Rows[j]["PART_NO"].ToString();
				string sPackingQty = pdtPrintMain.Rows[j]["CARRIER_QTY"].ToString();
				if (!dicPackingQty.ContainsKey(sPartNo))
				{
					dicPackingQty.Add(sPartNo, sPackingQty);
				}
			}
			for (int i = 0; pdtPrintMain.Rows.Count > i; i++)
			{
				sCurrPartNo = pdtPrintMain.Rows[i]["PART_NO"].ToString();
				sCurrLocatorGroup = pdtPrintMain.Rows[i]["LOCATOR_GROUP"].ToString();
				sCurrLine = pdtPrintMain.Rows[i]["LINE"].ToString();
				sCurrWO = pdtPrintMain.Rows[i]["WORKER_ORDER"].ToString();
				if (string.IsNullOrEmpty(sBeforePartNo))
				{
					sBeforePartNo = sCurrPartNo;
					sBeforeLocatorGroup = sCurrLocatorGroup;
					sBeforeLine = sCurrLine;
					sBeforeWO = sCurrWO;
					int.TryParse(dicPackingQty[sCurrPartNo], out iPackUnitqty);
				}
				int.TryParse(pdtPrintMain.Rows[i]["WO_QUANTITY"].ToString(), out iQty);
				int icurrQty = 0;
				iGroupIDRowCount++;
				iGroupIDSumQty += iQty;
				if (iGroupIDRowCount > 5 || sCurrPartNo != sBeforePartNo || sCurrLocatorGroup != sBeforeLocatorGroup || sCurrLine != sBeforeLine || iGroupIDSumQty > iPackUnitqty || (rdoWorkOrder.Checked && sCurrWO != sBeforeWO))
				{
					iGroupID++;
					iGroupIDRowCount = 0;
					sBeforePartNo = sCurrPartNo;
					sBeforeLocatorGroup = sCurrLocatorGroup;
					sBeforeLine = sCurrLine;
					sBeforeWO = sCurrWO;
					int.TryParse(dicPackingQty[sCurrPartNo], out iPackUnitqty);
					iGroupIDSumQty = iQty;
				}
				int iSumQty = 0;
				if (iPackUnitqty <= 0 || iQty <= 0)
				{
					continue;
				}
				for (int ipack = 0; iQty > ipack; ipack += iPackUnitqty)
				{
					iLeftQty = iQty - ipack;
					if (iLeftQty >= iPackUnitqty)
					{
						icurrQty = iPackUnitqty;
					}
					else if (iLeftQty < iPackUnitqty)
					{
						icurrQty = iLeftQty;
					}
					iSumQty += icurrQty;
					for (int k = 0; pdtPrintMain.Columns.Count > k; k++)
					{
						try
						{
							sValue = pdtPrintMain.Rows[i][k].ToString();
							if (!string.IsNullOrEmpty(sValue))
							{
								dr[k] = sValue;
							}
						}
						catch (Exception)
						{
							sValue = string.Empty;
						}
					}
					dr["WO_QUANTITY"] = icurrQty;
					dr["GROUPID"] = iGroupID;
					dtReturn.Rows.Add(dr.ItemArray);
					if (iSumQty >= iPackUnitqty)
					{
						iGroupID++;
						iSumQty = 0;
					}
					else if (rdoWorkOrder.Checked && sCurrWO != sBeforeWO)
					{
						iGroupID++;
						iSumQty = 0;
						dtReturn.Rows[dtReturn.Rows.Count - 1]["GROUPID"] = iGroupID;
					}
				}
			}
		}
		catch (Exception)
		{
			return dtReturn;
		}
		if (rdoCarrierQty.Checked)
		{
			callMakeQtyCarrierLoading(ref dtReturn);
		}
		return dtReturn;
	}

	private void callMakeQtyCarrierLoading(ref DataTable pdtPrintMain)
	{
		string sCHECKYNALL = "N";
		for (int i = 0; pdtPrintMain.Rows.Count > i; i++)
		{
			dtReturn.Rows[i]["GROUPID"] = "";
		}
		while (sCHECKYNALL == "N")
		{
			sCHECKYNALL = MakeQtyCarrierLoading(ref pdtPrintMain);
		}
	}

	private string MakeQtyCarrierLoading(ref DataTable pdtPrintMain)
	{
		string sValue = string.Empty;
		string sCurrPartNo = string.Empty;
		string sCurrLocatorGroup = string.Empty;
		string sCurrLine = string.Empty;
		string sBeforePartNo = string.Empty;
		string sBeforeLocatorGroup = string.Empty;
		string sBeforeLine = string.Empty;
		string sLocator = string.Empty;
		string sCHECKYNALL = string.Empty;
		string sCheckYN = string.Empty;
		int iGroupID = 0;
		int iLeftQty = 0;
		int iQty = 0;
		int iGroupIDRowCount = 0;
		int iPackUnitqty = 0;
		int iGroupIDSumQty = 0;
		bool bEnd = false;
		Dictionary<string, string> dicPackingQty = new Dictionary<string, string>();
		DataView myDataView = pdtPrintMain.DefaultView;
		myDataView.Sort = "CHECKYN DESC, SEQ ASC";
		dtReturn = myDataView.ToTable();
		DataRow dr = dtReturn.NewRow();
		try
		{
			for (int j = 0; dtReturn.Rows.Count > j; j++)
			{
				dtReturn.Rows[j]["SEQ"] = j;
				string sPartNo = dtReturn.Rows[j]["PART_NO"].ToString();
				string sPackingQty = dtReturn.Rows[j]["CARRIER_QTY"].ToString();
				if (!dicPackingQty.ContainsKey(sPartNo))
				{
					dicPackingQty.Add(sPartNo, sPackingQty);
				}
			}
			for (int i = 0; dtReturn.Rows.Count > i; i++)
			{
				if (bEnd)
				{
					break;
				}
				sCheckYN = dtReturn.Rows[i]["CHECKYN"].ToString();
				sCurrPartNo = dtReturn.Rows[i]["PART_NO"].ToString();
				sCurrLocatorGroup = dtReturn.Rows[i]["LOCATOR_GROUP"].ToString();
				sCurrLine = dtReturn.Rows[i]["LINE"].ToString();
				int.TryParse(dtReturn.Rows[i]["WO_QUANTITY"].ToString(), out iQty);
				int iLackQty = 0;
				if (string.IsNullOrEmpty(sBeforePartNo))
				{
					sBeforePartNo = sCurrPartNo;
					sBeforeLocatorGroup = sCurrLocatorGroup;
					sBeforeLine = sCurrLine;
					int.TryParse(dicPackingQty[sCurrPartNo], out iPackUnitqty);
				}
				iGroupIDRowCount++;
				iGroupIDSumQty += iQty;
				if (sCurrPartNo != sBeforePartNo || sCurrLocatorGroup != sBeforeLocatorGroup || sCurrLine != sBeforeLine)
				{
					iGroupID++;
					sBeforePartNo = sCurrPartNo;
					sBeforeLocatorGroup = sCurrLocatorGroup;
					sBeforeLine = sCurrLine;
					iGroupIDSumQty = iQty;
					iGroupIDRowCount = 1;
					int.TryParse(dicPackingQty[sCurrPartNo], out iPackUnitqty);
					if (sCheckYN != "Y" && i > 0)
					{
						updateGroupID(ref dtReturn, i - 1, iGroupID);
					}
				}
				if (iGroupIDSumQty == iPackUnitqty)
				{
					iGroupID++;
					sBeforePartNo = sCurrPartNo;
					sBeforeLocatorGroup = sCurrLocatorGroup;
					sBeforeLine = sCurrLine;
					iGroupIDSumQty = 0;
					iGroupIDRowCount = 0;
					int.TryParse(dicPackingQty[sCurrPartNo], out iPackUnitqty);
					if (sCheckYN != "Y")
					{
						updateGroupID(ref dtReturn, i, iGroupID);
					}
				}
				if (iGroupIDSumQty > iPackUnitqty && dtReturn.Rows.Count > i)
				{
					iLackQty = iPackUnitqty - (iGroupIDSumQty - iQty);
					int iNextrow2 = i;
					int iTempRowCount2 = iGroupIDRowCount;
					iGroupID++;
					sBeforePartNo = sCurrPartNo;
					sBeforeLocatorGroup = sCurrLocatorGroup;
					sBeforeLine = sCurrLine;
					iGroupIDSumQty = iQty;
					iGroupIDRowCount = 1;
					int.TryParse(dicPackingQty[sCurrPartNo], out iPackUnitqty);
					if (sCheckYN != "Y")
					{
						if (iLackQty != 0)
						{
							string sReturn2 = AdjustLackQty(ref dtReturn, iGroupID, iTempRowCount2, i, iLackQty);
							bEnd = true;
						}
						if (i > 0)
						{
							updateGroupID(ref dtReturn, i - 1, iGroupID);
						}
					}
				}
				if (iGroupIDRowCount == 5)
				{
					iLackQty = iPackUnitqty - iGroupIDSumQty;
					int iNextrow = i + 1;
					int iTempRowCount = iGroupIDRowCount;
					iGroupID++;
					sBeforePartNo = sCurrPartNo;
					sBeforeLocatorGroup = sCurrLocatorGroup;
					sBeforeLine = sCurrLine;
					iGroupIDSumQty = iQty;
					iGroupIDRowCount = 1;
					int.TryParse(dicPackingQty[sCurrPartNo], out iPackUnitqty);
					if (sCheckYN != "Y" && iLackQty != 0 && dtReturn.Rows.Count > i)
					{
						string sReturn = AdjustLackQty(ref dtReturn, iGroupID, iTempRowCount, iNextrow, iLackQty);
						bEnd = true;
					}
				}
				if (dtReturn.Rows.Count - 1 == i)
				{
					iGroupID++;
					sBeforePartNo = sCurrPartNo;
					sBeforeLocatorGroup = sCurrLocatorGroup;
					sBeforeLine = sCurrLine;
					int.TryParse(dicPackingQty[sCurrPartNo], out iPackUnitqty);
					if (sCheckYN != "Y" && i > 0)
					{
						updateGroupID(ref dtReturn, i, iGroupID);
					}
				}
			}
			sCHECKYNALL = "Y";
			for (int k = 0; dtReturn.Rows.Count > k; k++)
			{
				if (dtReturn.Rows.Count != 1 && dtReturn.Rows[k]["CHECKYN"].ToString() != "Y")
				{
					sCHECKYNALL = "N";
				}
			}
		}
		catch (Exception)
		{
			return sCHECKYNALL;
		}
		return sCHECKYNALL;
	}

	private void updateGroupID(ref DataTable pDataTable, int PStart, int pGroupID)
	{
		for (int i = 0; PStart >= i; i++)
		{
			string sGROUPID = pDataTable.Rows[i]["GROUPID"].ToString();
			if (string.IsNullOrEmpty(sGROUPID))
			{
				pDataTable.Rows[i]["GROUPID"] = pGroupID;
				pDataTable.Rows[i]["CHECKYN"] = "Y";
			}
		}
	}

	private string AdjustLackQty(ref DataTable dtMakeQty, int piGroupID, int piGroupIDRowCount, int piNextSeq, int piLackQty)
	{
		int iCurrQty = 0;
		int iNextQty = 0;
		string sCurrPartNo = string.Empty;
		string sNextPartNo = string.Empty;
		string sValue = string.Empty;
		string sReturn = string.Empty;
		int iPackUnitqty = 0;
		DataRow dr = dtReturn.NewRow();
		try
		{
			if (dtMakeQty.Rows.Count == piNextSeq)
			{
				dtReturn.Rows[piNextSeq - 1]["CHECKYN"] = "Y";
				sReturn = "LAST ROW";
				return sReturn;
			}
			sCurrPartNo = dtMakeQty.Rows[piNextSeq - 1]["PART_NO"].ToString();
			sNextPartNo = dtMakeQty.Rows[piNextSeq]["PART_NO"].ToString();
			if (sCurrPartNo != sNextPartNo)
			{
				dtReturn.Rows[piNextSeq - 1]["CHECKYN"] = "Y";
				sReturn = "DIFF_PART";
				return sReturn;
			}
			int.TryParse(dtMakeQty.Rows[piNextSeq - 1]["WO_QUANTITY"].ToString(), out iCurrQty);
			int.TryParse(dtMakeQty.Rows[piNextSeq]["WO_QUANTITY"].ToString(), out iNextQty);
			if (iNextQty > piLackQty)
			{
				if (piGroupIDRowCount == 5)
				{
					dtReturn.Rows[piNextSeq]["WO_QUANTITY"] = iNextQty - piLackQty;
					dtReturn.Rows[piNextSeq - 1]["WO_QUANTITY"] = iCurrQty + piLackQty;
					dtReturn.Rows[piNextSeq - 1]["CHECKYN"] = "Y";
				}
				else
				{
					for (int i = 0; dtReturn.Columns.Count > i; i++)
					{
						try
						{
							sValue = dtReturn.Rows[piNextSeq][i].ToString();
							if (!string.IsNullOrEmpty(sValue))
							{
								dr[i] = sValue;
							}
						}
						catch (Exception)
						{
							sValue = string.Empty;
						}
					}
					dtReturn.Rows[piNextSeq]["WO_QUANTITY"] = iNextQty - piLackQty;
					dr["WO_QUANTITY"] = piLackQty;
					dr["SEQ"] = piNextSeq;
					dr["CHECKYN"] = "Y";
					dr["GROUPID"] = piGroupID;
					dtReturn.Rows.Add(dr.ItemArray);
				}
			}
			else if (iNextQty == piLackQty)
			{
				if (piGroupIDRowCount == 5)
				{
					dtReturn.Rows[piNextSeq - 1]["WO_QUANTITY"] = iCurrQty + piLackQty;
					dtReturn.Rows[piNextSeq].Delete();
					dtReturn.Rows[piNextSeq - 1]["CHECKYN"] = "Y";
				}
				else
				{
					MessageBox.Show("Check Logic AdjustLackQty1");
				}
			}
			else if (iNextQty < piLackQty)
			{
				if (piGroupIDRowCount == 5)
				{
					dtReturn.Rows[piNextSeq - 1]["WO_QUANTITY"] = iCurrQty + iNextQty;
					dtReturn.Rows[piNextSeq].Delete();
					dtReturn.Rows[piNextSeq - 1]["CHECKYN"] = "N";
				}
				else
				{
					MessageBox.Show("Check Logic AdjustLackQty2");
				}
			}
		}
		catch (Exception)
		{
			return sReturn;
		}
		return sReturn;
	}

	private void btn_Down_Click(object sender, EventArgs e)
	{
		if (GetDataFromfpPrintMain())
		{
			for (int i = 0; fpPrintMain.ActiveSheet.Rows.Count - 1 > i; i++)
			{
				string iPart = fpPrintMain.ActiveSheet.GetValue(i, "PART_NO").ToString();
				string iWO = fpPrintMain.ActiveSheet.GetValue(i, "WORKER_ORDER").ToString();
				for (int j = 1; fpPrintMain.ActiveSheet.Rows.Count > j; j++)
				{
					string jPart = fpPrintMain.ActiveSheet.GetValue(j, "PART_NO").ToString();
					string jWO = fpPrintMain.ActiveSheet.GetValue(j, "WORKER_ORDER").ToString();
					if (iPart == jPart && iWO == jWO && i != j)
					{
						MessageBox.Show("[Row" + (i + 1) + " , " + (j + 1) + "] Inspection is not correct. \r\n(\"Part No\" and \"Worker Order)");
						fpPrintMade.ActiveSheet.Rows.Clear();
						return;
					}
				}
			}
			if (dtGroupBy.Rows.Count > 0)
			{
				fpPrintMade.ActiveSheet.DataSource = dtGroupBy;
				int iColumn = fpPrintMade.ActiveSheet.GetColumnIndex("CARRIER_ID");
				int iRows = fpPrintMade.ActiveSheet.Rows.Count - 1;
				int iCols = fpPrintMade.ActiveSheet.Columns.Count - 1;
				fpPrintMade.ActiveSheet.Cells[0, 0, iRows, iCols].CanFocus = false;
				fpPrintMade.ActiveSheet.Cells[0, iColumn, iRows, iColumn].CanFocus = true;
			}
			else
			{
				MessageBox.Show("There is no Data to Print!!");
			}
		}
		else
		{
			MessageBox.Show("There is no Data to Print!!");
		}
	}

	private void fpPrintMain_Resize(object sender, EventArgs e)
	{
		try
		{
			spreadColumnResize(fpPrintMain);
		}
		catch (Exception)
		{
		}
	}

	private void spreadColumnResize(MCS.Common.FpSpread fpSpread)
	{
		MCS.Common.FpSpread spread = null;
		try
		{
			spread = fpSpread;
			if (spread == null || spread.Width <= 100 || spread.ActiveSheet.Columns.Count <= 0)
			{
				return;
			}
			float spreadWidth = spread.Width;
			if (spread.ActiveSheet.RowHeader.Visible)
			{
				for (int k = 0; k < spread.ActiveSheet.RowHeader.Columns.Count; k++)
				{
					if (spread.ActiveSheet.RowHeader.Columns[k].Visible)
					{
						spreadWidth -= spread.ActiveSheet.RowHeader.Columns[k].Width;
					}
				}
			}
			if (spread.VerticalScrollBarPolicy == ScrollBarPolicy.AsNeeded || spread.VerticalScrollBarPolicy == ScrollBarPolicy.Always)
			{
				spreadWidth -= 17f;
			}
			spreadWidth *= 2f - spread.ActiveSheet.ZoomFactor;
			float totalWidth = 0f;
			for (int j = 0; j < spread.ActiveSheet.ColumnCount; j++)
			{
				if (spread.ActiveSheet.Columns[j].Visible)
				{
					totalWidth += spread.ActiveSheet.Columns[j].Width;
				}
			}
			if (!(totalWidth > 0f))
			{
				return;
			}
			int addWidth = 0;
			int lastVisibleColumnIndex = -1;
			for (int i = 0; i < spread.ActiveSheet.ColumnCount; i++)
			{
				if (spread.ActiveSheet.Columns[i].Visible)
				{
					lastVisibleColumnIndex = i;
					float width2 = spread.ActiveSheet.Columns[i].Width;
					if (width2 > 0f)
					{
						width2 = width2 / totalWidth * spreadWidth;
						spread.ActiveSheet.Columns[i].Width = (int)width2;
						addWidth += (int)width2;
					}
					else
					{
						spread.ActiveSheet.Columns[i].Width = 0f;
					}
				}
			}
			if (lastVisibleColumnIndex > -1)
			{
				int width = (int)spread.ActiveSheet.Columns[lastVisibleColumnIndex].Width + (int)spreadWidth - addWidth;
				if (width > 0)
				{
					spread.ActiveSheet.Columns[lastVisibleColumnIndex].Width = width;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void setrdoVisible()
	{
		if (txtOrg.Text.Contains("CNZ") || txtOrg.Text.Contains("CWZ"))
		{
			rdoWorkOrder.Visible = false;
			rdoP.Visible = false;
			rdoB.Visible = false;
			userBox5.Visible = false;
			rdoLabel.Enabled = true;
			rdoLabelA4.Enabled = true;
			rdoZebra.Visible = true;
			rdoA4.Checked = true;
			rdoZebra.Enabled = true;
			rdoLabelA4.Visible = false;
			rdoLabel.Visible = false;
			rdoNV.Visible = false;
			rdoGsrm.Visible = false;
		}
		else if (txtOrg.Text.Contains("EN7"))
		{
			rdoWorkOrder.Visible = false;
			rdoP.Visible = true;
			rdoB.Visible = true;
			userBox5.Visible = true;
			rdoLabel.Enabled = true;
			rdoLabelA4.Enabled = true;
			rdoLabel.Checked = true;
			rdoZebra.Visible = false;
		}
		else if (txtOrg.Text.Contains("CW4"))
		{
			rdoWorkOrder.Visible = true;
			rdoP.Visible = false;
			rdoB.Visible = false;
			userBox5.Visible = false;
			rdoLabel.Enabled = false;
			rdoNV.Enabled = true;
			rdoLabelA4.Enabled = false;
			rdoNV.Checked = true;
			rdoZebra.Visible = false;
		}
		else
		{
			rdoWorkOrder.Visible = true;
			rdoP.Visible = false;
			rdoB.Visible = false;
			userBox5.Visible = false;
			rdoLabel.Enabled = false;
			rdoLabelA4.Enabled = false;
			rdoNV.Enabled = false;
			rdoGsrm.Checked = true;
			rdoZebra.Visible = false;
		}
	}

	private void PrintLabel(DataRow dr, bool bSetting)
	{
		try
		{
			string str = GetLabelInfo(dr);
			PrintBarcode("ASSET", str, bSetting);
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private string GetLabelInfo(DataRow dr)
	{
		try
		{
			string sRet = string.Empty;
			string sBarcode = dr["SHEET_ID"].ToString();
			string sDescription = dr["DESCRIPTION"].ToString();
			string sInspetionFlag = dr["INSPECTION_FLAG"].ToString();
			string sLine = dr["LINE"].ToString();
			string sModelSuffix1 = dr["MODEL_SUFFIX1"].ToString();
			string sModelSuffix2 = dr["MODEL_SUFFIX2"].ToString();
			string sModelSuffix3 = dr["MODEL_SUFFIX3"].ToString();
			string sModelSuffix4 = dr["MODEL_SUFFIX4"].ToString();
			string sModelSuffix5 = dr["MODEL_SUFFIX5"].ToString();
			string sPartNo = dr["PART_NO"].ToString();
			string sQR1 = dr["QRCODE_VALUE"].ToString();
			string sQR2 = dr["QRCODE_VALUE"].ToString();
			string sQty1 = dr["WO_QUANTITY1"].ToString();
			string sQty2 = dr["WO_QUANTITY2"].ToString();
			string sQty3 = dr["WO_QUANTITY3"].ToString();
			string sQty4 = dr["WO_QUANTITY4"].ToString();
			string sQty5 = dr["WO_QUANTITY5"].ToString();
			string sSupplier = dr["MADE_BY"].ToString();
			string sTotalQty = dr["TOTAL_QUANTITY"].ToString();
			string sWO1 = dr["WORKER_ORDER1"].ToString();
			string sWO2 = dr["WORKER_ORDER2"].ToString();
			string sWO3 = dr["WORKER_ORDER3"].ToString();
			string sWO4 = dr["WORKER_ORDER4"].ToString();
			string sWO5 = dr["WORKER_ORDER5"].ToString();
			string sPrintedDate = "Printed Date : " + dr["PRINTED_DATE"].ToString();
			sRet = "^XA\r\n                                ^MD{DARKNESS}^PR2,2^LH{LEFT},{TOP}^FS\r\n                                ~TA000\r\n                                ~JSN\r\n                                ^LT0\r\n                                ^MNW\r\n                                ^MTT\r\n                                ^PON\r\n                                ^PMN\r\n                                ^LH0,0\r\n                                ^JMA\r\n                                ^PR6,6\r\n                                ~SD15\r\n                                ^JUS\r\n                                ^LRN\r\n                                ^CI27\r\n                                ^PA0,1,1,0\r\n                                ^MMC\r\n                                ^PW1122\r\n                                ^LL3508\r\n                                ^LS0\r\n                                ^FO53,89^GB853,3324,4,,0^FS\r\n                                ^FO56,2919^GB849,0,4^FS\r\n                                ^FO317,2034^GB586,0,4^FS\r\n                                ^FO314,1241^GB587,0,4^FS\r\n                                ^FO217,2922^GB0,489,4^FS\r\n                                ^FO378,2922^GB0,489,4^FS\r\n                                ^FO553,2922^GB0,489,4^FS\r\n                                ^FO739,2922^GB0,489,4^FS\r\n                                ^FO400,91^GB0,2829,4^FS\r\n                                ^FO520,90^GB0,2829,4^FS\r\n                                ^FO632,91^GB0,2829,4^FS\r\n                                ^FO757,758^GB0,2161,4^FS\r\n                                ^FO315,94^GB0,2829,4^FS\r\n                                ^FO314,757^GB587,0,4^FS\r\n                                ^FT85,3402^AFB,26,13^FH\\^FDLine^FS\r\n                                ^FT247,3401^AFB,26,13^FH\\^FDQty^FS\r\n                                ^FT409,3401^AFB,26,13^FH\\^FDSupplier^FS\r\n                                ^FT584,3401^AFB,26,13^FH\\^FDCNTRCode^FS\r\n                                ^FT769,3401^AFB,26,13^FH\\^FDL/No^FS\r\n                                ^FT287,2914^AFB,26,13^FB212,1,0,C^FH\\^FDDescription^FS\r\n                                ^FPH,10^FT375,2814^AFB,52,26^FB641,1,0,C^FH\\^FDModel.Suffix^FS\r\n                                ^FT377,1878^AFB,52,26^FB469,1,0,C^FH\\^FDW/O^FS\r\n                                ^FT377,1232^AFB,52,26^FB469,1,0,C^FH\\^FDQty^FS\r\n                                ^FT377,669^AFB,52,26^FB469,1,0,C^FH\\^FDPart No^FS\r\n                                ^FPH,2^FT1001,3402^ADB,36,20^FB1234,1,0,C^FH\\^FD[sPrintedDate]^FS\r\n                                ^FPH,2^FT1001,657^ADB,36,20^FB549,1,0,C^FH\\^FDPrinted by GMCS^FS\r\n                                ^BY7,3,92^FT165,2345^BCB,,Y,N,,A\r\n                                ^FN1[vBarcode]^FS\r\n                                ^FT87,358 ^BQN,2,10\r\n                                ^FH\\^FN2[vQR1] ^FS\r\n                                ^FT677,361 ^BQN,2,10\r\n                                ^FH\\^FN3[vQR2]^FS\r\n                                ^FPH,10 ^FO113,3000 ^AFB,52,26 ^FB382,1,0,C ^FH\\^FN4[vLine]^FS\r\n                                ^FO275,3000 ^AFB,52,26 ^FB372,1,0,C ^FH\\^FN5[vTotalQty]^FS\r\n                                ^FPH,10 ^FO447,2930 ^AFB,52,26 ^FB488,1,0,C ^FH\\^FN6[vSupplier]^FS\r\n                                ^FPH,10 ^FO240,1241 ^AFB,52,26 ^FB1488,1,0,L ^FH\\^FN7[vDescription]^FS\r\n                                ^FPH,10 ^FO429,2056 ^AFB,52,26 ^FB866,1,0,C ^FH\\^FN8[vModelSuffix1]^FS\r\n                                ^FO429,1271 ^AFB,52,26 ^FB739,1,0,C ^FH\\^FN9[vWO1]^FS\r\n                                ^FO429,788 ^AFB,52,26 ^FB431,1,0,C ^FH\\^FN10[vQty1]^FS\r\n                                ^FO429,117 ^AFB,52,26 ^FB622,1,0,C ^FH\\^FN11[vPartNo]^FS\r\n                                ^FPH,10 ^FO546,2056 ^AFB,52,26 ^FB866,1,0,C ^FH\\^FN12[vModelSuffix2]^FS\r\n                                ^FO663,2056 ^AFB,52,26 ^FB856,1,0,C ^FH\\^FN13[vModelSuffix3]^FS\r\n                                ^FO796,2056 ^AFB,52,26 ^FB856,1,0,C ^FH\\^FN14[vModelSuffix4]^FS\r\n                                ^FO543,1267 ^AFB,52,26 ^FB739,1,0,C ^FH\\^FN15[vWO2]^FS\r\n                                ^FO665,1263 ^AFB,52,26 ^FB739,1,0,C ^FH\\^FN16[vWO3]^FS\r\n                                ^FO795,1263 ^AFB,52,26 ^FB739,1,0,C ^FH\\^FN17[vWO4]^FS\r\n                                ^FO543,792 ^AFB,52,26 ^FB431,1,0,C ^FH\\^FN18[vQty2]^FS\r\n                                ^FO665,792 ^AFB,52,26 ^FB431,1,0,C ^FH\\^FN19[vQty3]^FS\r\n                                ^FO795,792 ^AFB,52,26 ^FB431,1,0,C ^FH\\^FN20[vQty4]^FS\r\n                                ^FO543,116 ^AFB,52,26 ^FB622,1,0,C ^FH\\^FN21[vInspectionFlag]^FS\r\n\r\n                                ^XZ\r\n                                ";
			sRet = sRet.Replace("[vBarcode]", sBarcode);
			sRet = sRet.Replace("[vDescription]", sDescription);
			sRet = sRet.Replace("[vInspetionFlag]", sInspetionFlag);
			sRet = sRet.Replace("[vLine]", sLine);
			sRet = sRet.Replace("[vModelSuffix1]", sModelSuffix1);
			sRet = sRet.Replace("[vModelSuffix2]", sModelSuffix2);
			sRet = sRet.Replace("[vModelSuffix3]", sModelSuffix3);
			sRet = sRet.Replace("[vModelSuffix4]", sModelSuffix4);
			sRet = sRet.Replace("[vModelSuffix5]", sModelSuffix5);
			sRet = sRet.Replace("[vPartNo]", sPartNo);
			sRet = sRet.Replace("[vQR1]", sQR1);
			sRet = sRet.Replace("[vQR2]", sQR2);
			sRet = sRet.Replace("[vQty1]", sQty1);
			sRet = sRet.Replace("[vQty2]", sQty2);
			sRet = sRet.Replace("[vQty3]", sQty3);
			sRet = sRet.Replace("[vQty4]", sQty4);
			sRet = sRet.Replace("[vQty5]", sQty5);
			sRet = sRet.Replace("[vSupplier]", sSupplier);
			sRet = sRet.Replace("[vTotalQty]", sTotalQty);
			sRet = sRet.Replace("[vWO1]", sWO1);
			sRet = sRet.Replace("[vWO2]", sWO2);
			sRet = sRet.Replace("[vWO3]", sWO3);
			sRet = sRet.Replace("[vWO4]", sWO4);
			sRet = sRet.Replace("[vWO5]", sWO5);
			return sRet.Replace("[sPrintedDate]", sPrintedDate);
		}
		catch (Exception ex)
		{
			throw ex;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmOffLinePrint));
		FarPoint.Win.Spread.DefaultFocusIndicatorRenderer defaultFocusIndicatorRenderer2 = new FarPoint.Win.Spread.DefaultFocusIndicatorRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer5 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer6 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer1 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer2 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer3 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		FarPoint.Win.Spread.DefaultScrollBarRenderer defaultScrollBarRenderer4 = new FarPoint.Win.Spread.DefaultScrollBarRenderer();
		this.tmDemo = new System.Windows.Forms.Timer(this.components);
		this.dgvBuffer = new System.Windows.Forms.DataGridView();
		this.tmRefresh = new System.Windows.Forms.Timer(this.components);
		this.searchPanel1 = new MCS.Common.SearchPanel();
		this.splitContainerMain = new System.Windows.Forms.SplitContainer();
		this.panelOnly1 = new MCS.Common.PanelOnly();
		this.btn_org_change1 = new System.Windows.Forms.Button();
		this.txtOrg = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.btn_excel = new System.Windows.Forms.Button();
		this.btn_New = new System.Windows.Forms.Button();
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.splitContainer2 = new System.Windows.Forms.SplitContainer();
		this.label4 = new System.Windows.Forms.Label();
		this.fpPrintMain = new MCS.Common.FpSpread();
		this.panel5 = new System.Windows.Forms.Panel();
		this.rdoB = new System.Windows.Forms.RadioButton();
		this.rdoP = new System.Windows.Forms.RadioButton();
		this.userBox5 = new MCS.Common.Controls.UserBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.rdoZebra = new System.Windows.Forms.RadioButton();
		this.rdoA4 = new System.Windows.Forms.RadioButton();
		this.rdoNV = new System.Windows.Forms.RadioButton();
		this.rdoLabelA4 = new System.Windows.Forms.RadioButton();
		this.rdoGsrm = new System.Windows.Forms.RadioButton();
		this.rdoLabel = new System.Windows.Forms.RadioButton();
		this.label3 = new System.Windows.Forms.Label();
		this.userBox2 = new MCS.Common.Controls.UserBox();
		this.btn_preview = new System.Windows.Forms.Button();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.rdoWorkOrder = new System.Windows.Forms.RadioButton();
		this.rdoCarrierQty = new System.Windows.Forms.RadioButton();
		this.userBox1 = new MCS.Common.Controls.UserBox();
		this.btn_Down = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.fpPrintMade = new MCS.Common.FpSpread();
		this.backPanel2 = new MCS.Common.BackPanel();
		this.fpProdResult = new MCS.Common.FpSpread();
		this.backPanel1 = new MCS.Common.BackPanel();
		this.btn_Setting = new System.Windows.Forms.Button();
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).BeginInit();
		this.splitContainerMain.Panel1.SuspendLayout();
		this.splitContainerMain.Panel2.SuspendLayout();
		this.splitContainerMain.SuspendLayout();
		this.panelOnly1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer2).BeginInit();
		this.splitContainer2.Panel1.SuspendLayout();
		this.splitContainer2.Panel2.SuspendLayout();
		this.splitContainer2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpPrintMain).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpPrintMade).BeginInit();
		this.backPanel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.fpProdResult).BeginInit();
		base.SuspendLayout();
		this.dgvBuffer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
		this.dgvBuffer.Location = new System.Drawing.Point(848, 12);
		this.dgvBuffer.Name = "dgvBuffer";
		this.dgvBuffer.RowTemplate.Height = 23;
		this.dgvBuffer.Size = new System.Drawing.Size(121, 45);
		this.dgvBuffer.TabIndex = 53;
		this.dgvBuffer.Visible = false;
		this.tmRefresh.Interval = 5000;
		this.searchPanel1.BackColor = System.Drawing.Color.Red;
		this.searchPanel1.BackgroundImage = (System.Drawing.Image)resources.GetObject("searchPanel1.BackgroundImage");
		this.searchPanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.searchPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.searchPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.searchPanel1.Location = new System.Drawing.Point(0, 0);
		this.searchPanel1.Name = "searchPanel1";
		this.searchPanel1.Padding = new System.Windows.Forms.Padding(8);
		this.searchPanel1.Size = new System.Drawing.Size(1000, 109);
		this.searchPanel1.TabIndex = 0;
		this.splitContainerMain.BackColor = System.Drawing.Color.DimGray;
		this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
		this.splitContainerMain.Name = "splitContainerMain";
		this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainerMain.Panel1.BackColor = System.Drawing.Color.DimGray;
		this.splitContainerMain.Panel1.Controls.Add(this.panelOnly1);
		this.splitContainerMain.Panel2.Controls.Add(this.splitContainer1);
		this.splitContainerMain.Size = new System.Drawing.Size(1251, 761);
		this.splitContainerMain.SplitterDistance = 52;
		this.splitContainerMain.SplitterWidth = 1;
		this.splitContainerMain.TabIndex = 66;
		this.panelOnly1.BackColor = System.Drawing.Color.Transparent;
		this.panelOnly1.BackgroundImage = (System.Drawing.Image)resources.GetObject("panelOnly1.BackgroundImage");
		this.panelOnly1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.panelOnly1.Controls.Add(this.btn_org_change1);
		this.panelOnly1.Controls.Add(this.txtOrg);
		this.panelOnly1.Controls.Add(this.label2);
		this.panelOnly1.Controls.Add(this.label8);
		this.panelOnly1.Controls.Add(this.panel1);
		this.panelOnly1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelOnly1.Location = new System.Drawing.Point(0, 0);
		this.panelOnly1.Name = "panelOnly1";
		this.panelOnly1.Padding = new System.Windows.Forms.Padding(8);
		this.panelOnly1.Size = new System.Drawing.Size(1251, 52);
		this.panelOnly1.TabIndex = 80;
		this.btn_org_change1.BackColor = System.Drawing.Color.DimGray;
		this.btn_org_change1.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_org_change1.ForeColor = System.Drawing.Color.White;
		this.btn_org_change1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_org_change1.Location = new System.Drawing.Point(570, 9);
		this.btn_org_change1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_org_change1.Name = "btn_org_change1";
		this.btn_org_change1.Size = new System.Drawing.Size(116, 33);
		this.btn_org_change1.TabIndex = 83;
		this.btn_org_change1.Text = "Org Change";
		this.btn_org_change1.UseVisualStyleBackColor = false;
		this.btn_org_change1.Click += new System.EventHandler(btn_Org_Change_Click);
		this.txtOrg.Font = new System.Drawing.Font("Arial", 10f);
		this.txtOrg.Location = new System.Drawing.Point(495, 14);
		this.txtOrg.Name = "txtOrg";
		this.txtOrg.ReadOnly = true;
		this.txtOrg.Size = new System.Drawing.Size(71, 23);
		this.txtOrg.TabIndex = 81;
		this.txtOrg.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.label2.BackColor = System.Drawing.Color.Transparent;
		this.label2.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.Black;
		this.label2.Location = new System.Drawing.Point(451, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(49, 22);
		this.label2.TabIndex = 80;
		this.label2.Text = "Org";
		this.label8.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Arial", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(5, 7);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(270, 38);
		this.label8.TabIndex = 79;
		this.label8.Text = "Offline Sheet Management";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel1.BackColor = System.Drawing.Color.Transparent;
		this.panel1.Controls.Add(this.btn_Setting);
		this.panel1.Controls.Add(this.btn_excel);
		this.panel1.Controls.Add(this.btn_New);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel1.Location = new System.Drawing.Point(911, 8);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(332, 36);
		this.panel1.TabIndex = 78;
		this.btn_excel.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.btn_excel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_excel.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_excel.ForeColor = System.Drawing.Color.White;
		this.btn_excel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_excel.Location = new System.Drawing.Point(123, 2);
		this.btn_excel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_excel.Name = "btn_excel";
		this.btn_excel.Size = new System.Drawing.Size(100, 33);
		this.btn_excel.TabIndex = 78;
		this.btn_excel.Text = "Templete";
		this.btn_excel.UseVisualStyleBackColor = false;
		this.btn_excel.Click += new System.EventHandler(btn_excel_Click);
		this.btn_New.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.btn_New.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_New.ForeColor = System.Drawing.Color.White;
		this.btn_New.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_New.Location = new System.Drawing.Point(23, 2);
		this.btn_New.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_New.Name = "btn_New";
		this.btn_New.Size = new System.Drawing.Size(100, 33);
		this.btn_New.TabIndex = 77;
		this.btn_New.Text = "Clear";
		this.btn_New.UseVisualStyleBackColor = false;
		this.btn_New.Click += new System.EventHandler(btn_New_Click);
		this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
		this.splitContainer1.Panel2.Controls.Add(this.label5);
		this.splitContainer1.Panel2.Controls.Add(this.fpPrintMade);
		this.splitContainer1.Size = new System.Drawing.Size(1251, 708);
		this.splitContainer1.SplitterDistance = 459;
		this.splitContainer1.SplitterWidth = 1;
		this.splitContainer1.TabIndex = 72;
		this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		this.splitContainer2.Location = new System.Drawing.Point(0, 0);
		this.splitContainer2.Name = "splitContainer2";
		this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer2.Panel1.Controls.Add(this.label4);
		this.splitContainer2.Panel1.Controls.Add(this.fpPrintMain);
		this.splitContainer2.Panel2.Controls.Add(this.panel5);
		this.splitContainer2.Panel2.Controls.Add(this.panel2);
		this.splitContainer2.Panel2.Controls.Add(this.panel3);
		this.splitContainer2.Panel2.Controls.Add(this.btn_Down);
		this.splitContainer2.Size = new System.Drawing.Size(1251, 459);
		this.splitContainer2.SplitterDistance = 429;
		this.splitContainer2.SplitterWidth = 1;
		this.splitContainer2.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Arial", 12.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(14, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(407, 19);
		this.label4.TabIndex = 1;
		this.label4.Text = "Copy and Paste from Excel or Input the Sheet data";
		this.fpPrintMain.AccessibleDescription = "";
		this.fpPrintMain.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.fpPrintMain.AutoSizeColumnWidth = true;
		this.fpPrintMain.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpPrintMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpPrintMain.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintMain.EnableSort = false;
		this.fpPrintMain.FocusRenderer = defaultFocusIndicatorRenderer2;
		this.fpPrintMain.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpPrintMain.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPrintMain.HorizontalScrollBar.Name = "";
		this.fpPrintMain.HorizontalScrollBar.Renderer = defaultScrollBarRenderer5;
		this.fpPrintMain.HorizontalScrollBar.TabIndex = 0;
		this.fpPrintMain.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPrintMain.Location = new System.Drawing.Point(5, 35);
		this.fpPrintMain.Name = "fpPrintMain";
		this.fpPrintMain.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintMain.Size = new System.Drawing.Size(1244, 391);
		this.fpPrintMain.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpPrintMain.TabIndex = 71;
		this.fpPrintMain.TextTipDelay = 1000;
		this.fpPrintMain.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpPrintMain.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPrintMain.VerticalScrollBar.Name = "";
		this.fpPrintMain.VerticalScrollBar.Renderer = defaultScrollBarRenderer6;
		this.fpPrintMain.VerticalScrollBar.TabIndex = 0;
		this.fpPrintMain.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPrintMain.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.fpPrintMain.CellClick += new FarPoint.Win.Spread.CellClickEventHandler(fpPrintMain_CellClick);
		this.fpPrintMain.Resize += new System.EventHandler(fpPrintMain_Resize);
		this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.panel5.Controls.Add(this.rdoB);
		this.panel5.Controls.Add(this.rdoP);
		this.panel5.Controls.Add(this.userBox5);
		this.panel5.Location = new System.Drawing.Point(607, 1);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(120, 36);
		this.panel5.TabIndex = 114;
		this.rdoB.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoB.AutoSize = true;
		this.rdoB.Checked = true;
		this.rdoB.Font = new System.Drawing.Font("Arial", 9f);
		this.rdoB.Location = new System.Drawing.Point(68, 7);
		this.rdoB.Name = "rdoB";
		this.rdoB.Size = new System.Drawing.Size(45, 19);
		this.rdoB.TabIndex = 122;
		this.rdoB.TabStop = true;
		this.rdoB.Tag = "Box";
		this.rdoB.Text = "Box";
		this.rdoB.UseVisualStyleBackColor = true;
		this.rdoP.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoP.AutoSize = true;
		this.rdoP.Font = new System.Drawing.Font("Arial", 9f);
		this.rdoP.Location = new System.Drawing.Point(14, 7);
		this.rdoP.Name = "rdoP";
		this.rdoP.Size = new System.Drawing.Size(43, 19);
		this.rdoP.TabIndex = 119;
		this.rdoP.Tag = "Pallet";
		this.rdoP.Text = "Pal";
		this.rdoP.UseVisualStyleBackColor = true;
		this.userBox5.BackColor = System.Drawing.Color.Transparent;
		this.userBox5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.userBox5.ForeColor = System.Drawing.SystemColors.ControlText;
		this.userBox5.Location = new System.Drawing.Point(0, 1);
		this.userBox5.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
		this.userBox5.Name = "userBox5";
		this.userBox5.Size = new System.Drawing.Size(114, 27);
		this.userBox5.TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.panel2.Controls.Add(this.rdoZebra);
		this.panel2.Controls.Add(this.rdoA4);
		this.panel2.Controls.Add(this.rdoNV);
		this.panel2.Controls.Add(this.rdoLabelA4);
		this.panel2.Controls.Add(this.rdoGsrm);
		this.panel2.Controls.Add(this.rdoLabel);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.userBox2);
		this.panel2.Controls.Add(this.btn_preview);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel2.Location = new System.Drawing.Point(729, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(522, 29);
		this.panel2.TabIndex = 0;
		this.rdoZebra.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoZebra.AutoSize = true;
		this.rdoZebra.Checked = true;
		this.rdoZebra.Enabled = false;
		this.rdoZebra.Location = new System.Drawing.Point(113, 6);
		this.rdoZebra.Name = "rdoZebra";
		this.rdoZebra.Size = new System.Drawing.Size(57, 19);
		this.rdoZebra.TabIndex = 122;
		this.rdoZebra.TabStop = true;
		this.rdoZebra.Text = "Zebra";
		this.rdoZebra.UseVisualStyleBackColor = true;
		this.rdoA4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoA4.AutoSize = true;
		this.rdoA4.Location = new System.Drawing.Point(51, 6);
		this.rdoA4.Name = "rdoA4";
		this.rdoA4.Size = new System.Drawing.Size(51, 19);
		this.rdoA4.TabIndex = 114;
		this.rdoA4.Text = "A4-P";
		this.rdoA4.UseVisualStyleBackColor = true;
		this.rdoNV.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoNV.AutoSize = true;
		this.rdoNV.Location = new System.Drawing.Point(184, 6);
		this.rdoNV.Name = "rdoNV";
		this.rdoNV.Size = new System.Drawing.Size(41, 19);
		this.rdoNV.TabIndex = 121;
		this.rdoNV.Text = "NV";
		this.rdoNV.UseVisualStyleBackColor = true;
		this.rdoLabelA4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoLabelA4.AutoSize = true;
		this.rdoLabelA4.Enabled = false;
		this.rdoLabelA4.Location = new System.Drawing.Point(319, 6);
		this.rdoLabelA4.Name = "rdoLabelA4";
		this.rdoLabelA4.Size = new System.Drawing.Size(78, 19);
		this.rdoLabelA4.TabIndex = 120;
		this.rdoLabelA4.Text = "Label(A4)";
		this.rdoLabelA4.UseVisualStyleBackColor = true;
		this.rdoGsrm.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoGsrm.AutoSize = true;
		this.rdoGsrm.Location = new System.Drawing.Point(115, 8);
		this.rdoGsrm.Name = "rdoGsrm";
		this.rdoGsrm.Size = new System.Drawing.Size(50, 19);
		this.rdoGsrm.TabIndex = 119;
		this.rdoGsrm.Text = "A4-L";
		this.rdoGsrm.UseVisualStyleBackColor = true;
		this.rdoLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdoLabel.AutoSize = true;
		this.rdoLabel.Enabled = false;
		this.rdoLabel.Location = new System.Drawing.Point(245, 6);
		this.rdoLabel.Name = "rdoLabel";
		this.rdoLabel.Size = new System.Drawing.Size(56, 19);
		this.rdoLabel.TabIndex = 118;
		this.rdoLabel.Text = "Label";
		this.rdoLabel.UseVisualStyleBackColor = true;
		this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(10, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(32, 15);
		this.label3.TabIndex = 117;
		this.label3.Text = "Type";
		this.userBox2.BackColor = System.Drawing.Color.Transparent;
		this.userBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.userBox2.ForeColor = System.Drawing.SystemColors.ControlText;
		this.userBox2.Location = new System.Drawing.Point(5, 0);
		this.userBox2.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
		this.userBox2.Name = "userBox2";
		this.userBox2.Size = new System.Drawing.Size(413, 26);
		this.userBox2.TabIndex = 116;
		this.btn_preview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btn_preview.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.btn_preview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
		this.btn_preview.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_preview.ForeColor = System.Drawing.Color.White;
		this.btn_preview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_preview.Location = new System.Drawing.Point(420, -1);
		this.btn_preview.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_preview.Name = "btn_preview";
		this.btn_preview.Size = new System.Drawing.Size(100, 28);
		this.btn_preview.TabIndex = 76;
		this.btn_preview.Text = "Print";
		this.btn_preview.UseVisualStyleBackColor = false;
		this.btn_preview.Click += new System.EventHandler(btn_preview_Click);
		this.panel3.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.rdoWorkOrder);
		this.panel3.Controls.Add(this.rdoCarrierQty);
		this.panel3.Controls.Add(this.userBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Font = new System.Drawing.Font("Arial", 9f);
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(409, 29);
		this.panel3.TabIndex = 113;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(14, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(80, 15);
		this.label1.TabIndex = 113;
		this.label1.Text = "Loading Type";
		this.rdoWorkOrder.AutoSize = true;
		this.rdoWorkOrder.Location = new System.Drawing.Point(270, 4);
		this.rdoWorkOrder.Name = "rdoWorkOrder";
		this.rdoWorkOrder.Size = new System.Drawing.Size(87, 19);
		this.rdoWorkOrder.TabIndex = 111;
		this.rdoWorkOrder.Text = "Work Order";
		this.rdoWorkOrder.UseVisualStyleBackColor = true;
		this.rdoCarrierQty.AutoSize = true;
		this.rdoCarrierQty.Checked = true;
		this.rdoCarrierQty.Location = new System.Drawing.Point(141, 4);
		this.rdoCarrierQty.Name = "rdoCarrierQty";
		this.rdoCarrierQty.Size = new System.Drawing.Size(83, 19);
		this.rdoCarrierQty.TabIndex = 110;
		this.rdoCarrierQty.TabStop = true;
		this.rdoCarrierQty.Text = "Carrier Qty";
		this.rdoCarrierQty.UseVisualStyleBackColor = true;
		this.userBox1.BackColor = System.Drawing.Color.Transparent;
		this.userBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.userBox1.Dock = System.Windows.Forms.DockStyle.Right;
		this.userBox1.ForeColor = System.Drawing.SystemColors.ControlText;
		this.userBox1.Location = new System.Drawing.Point(12, 0);
		this.userBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 10);
		this.userBox1.Name = "userBox1";
		this.userBox1.Padding = new System.Windows.Forms.Padding(0, 0, 0, 50);
		this.userBox1.Size = new System.Drawing.Size(397, 29);
		this.userBox1.TabIndex = 112;
		this.btn_Down.Anchor = System.Windows.Forms.AnchorStyles.Top;
		this.btn_Down.BackColor = System.Drawing.Color.White;
		this.btn_Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_Down.Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
		this.btn_Down.ForeColor = System.Drawing.Color.Black;
		this.btn_Down.Image = (System.Drawing.Image)resources.GetObject("btn_Down.Image");
		this.btn_Down.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Down.Location = new System.Drawing.Point(450, 0);
		this.btn_Down.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Down.Name = "btn_Down";
		this.btn_Down.Size = new System.Drawing.Size(120, 28);
		this.btn_Down.TabIndex = 109;
		this.btn_Down.Text = "        Down";
		this.btn_Down.UseVisualStyleBackColor = false;
		this.btn_Down.Click += new System.EventHandler(btn_Down_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Arial", 12.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(14, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(261, 19);
		this.label5.TabIndex = 72;
		this.label5.Text = "Check Carrier ID before Printing";
		this.fpPrintMade.AccessibleDescription = "";
		this.fpPrintMade.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
		this.fpPrintMade.AutoSizeColumnWidth = true;
		this.fpPrintMade.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpPrintMade.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpPrintMade.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintMade.EnableSort = true;
		this.fpPrintMade.FocusRenderer = defaultFocusIndicatorRenderer2;
		this.fpPrintMade.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpPrintMade.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPrintMade.HorizontalScrollBar.Name = "";
		this.fpPrintMade.HorizontalScrollBar.Renderer = defaultScrollBarRenderer1;
		this.fpPrintMade.HorizontalScrollBar.TabIndex = 0;
		this.fpPrintMade.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPrintMade.Location = new System.Drawing.Point(7, 33);
		this.fpPrintMade.Name = "fpPrintMade";
		this.fpPrintMade.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpPrintMade.Size = new System.Drawing.Size(1239, 224);
		this.fpPrintMade.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpPrintMade.TabIndex = 72;
		this.fpPrintMade.TextTipDelay = 1000;
		this.fpPrintMade.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpPrintMade.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpPrintMade.VerticalScrollBar.Name = "";
		this.fpPrintMade.VerticalScrollBar.Renderer = defaultScrollBarRenderer2;
		this.fpPrintMade.VerticalScrollBar.TabIndex = 0;
		this.fpPrintMade.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpPrintMade.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.backPanel2.BackColor = System.Drawing.Color.FromArgb(215, 214, 216);
		this.backPanel2.Controls.Add(this.fpProdResult);
		this.backPanel2.Location = new System.Drawing.Point(10, 63);
		this.backPanel2.Name = "backPanel2";
		this.backPanel2.Padding = new System.Windows.Forms.Padding(0);
		this.backPanel2.Size = new System.Drawing.Size(650, 285);
		this.backPanel2.TabIndex = 69;
		this.backPanel2.Title = "생산실적";
		this.fpProdResult.AccessibleDescription = "";
		this.fpProdResult.AutoSizeColumnWidth = true;
		this.fpProdResult.BackColor = System.Drawing.Color.FromArgb(181, 203, 231);
		this.fpProdResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.fpProdResult.ColumnSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpProdResult.Dock = System.Windows.Forms.DockStyle.Fill;
		this.fpProdResult.EnableSort = false;
		this.fpProdResult.FocusRenderer = defaultFocusIndicatorRenderer2;
		this.fpProdResult.Font = new System.Drawing.Font("맑은 고딕", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 129);
		this.fpProdResult.HorizontalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpProdResult.HorizontalScrollBar.Name = "";
		this.fpProdResult.HorizontalScrollBar.Renderer = defaultScrollBarRenderer3;
		this.fpProdResult.HorizontalScrollBar.TabIndex = 0;
		this.fpProdResult.HorizontalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpProdResult.Location = new System.Drawing.Point(7, 29);
		this.fpProdResult.Name = "fpProdResult";
		this.fpProdResult.RowSplitBoxPolicy = FarPoint.Win.Spread.SplitBoxPolicy.Never;
		this.fpProdResult.Size = new System.Drawing.Size(636, 249);
		this.fpProdResult.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Classic;
		this.fpProdResult.TabIndex = 66;
		this.fpProdResult.TextTipDelay = 1000;
		this.fpProdResult.TextTipPolicy = FarPoint.Win.Spread.TextTipPolicy.Floating;
		this.fpProdResult.VerticalScrollBar.Buttons = new FarPoint.Win.Spread.FpScrollBarButtonCollection("BackwardLineButton,ThumbTrack,ForwardLineButton");
		this.fpProdResult.VerticalScrollBar.Name = "";
		this.fpProdResult.VerticalScrollBar.Renderer = defaultScrollBarRenderer4;
		this.fpProdResult.VerticalScrollBar.TabIndex = 0;
		this.fpProdResult.VerticalScrollBarPolicy = FarPoint.Win.Spread.ScrollBarPolicy.AsNeeded;
		this.fpProdResult.VisualStyles = FarPoint.Win.VisualStyles.Off;
		this.backPanel1.BackColor = System.Drawing.Color.FromArgb(215, 214, 216);
		this.backPanel1.Location = new System.Drawing.Point(661, 63);
		this.backPanel1.Name = "backPanel1";
		this.backPanel1.Padding = new System.Windows.Forms.Padding(0);
		this.backPanel1.Size = new System.Drawing.Size(647, 285);
		this.backPanel1.TabIndex = 68;
		this.backPanel1.Title = "생산계획";
		this.btn_Setting.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.btn_Setting.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold);
		this.btn_Setting.ForeColor = System.Drawing.Color.White;
		this.btn_Setting.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_Setting.Location = new System.Drawing.Point(223, 2);
		this.btn_Setting.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btn_Setting.Name = "btn_Setting";
		this.btn_Setting.Size = new System.Drawing.Size(100, 33);
		this.btn_Setting.TabIndex = 79;
		this.btn_Setting.Text = "Setting";
		this.btn_Setting.UseVisualStyleBackColor = false;
		this.btn_Setting.Click += new System.EventHandler(btn_Setting_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
		this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
		base.ClientSize = new System.Drawing.Size(1251, 761);
		base.Controls.Add(this.splitContainerMain);
		this.Cursor = System.Windows.Forms.Cursors.Arrow;
		this.DoubleBuffered = true;
		this.Font = new System.Drawing.Font("Arial", 9f);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmOffLinePrint";
		this.Text = "MCS Offline Sheet";
		base.Title = "MCS Offline Sheet";
		base.Load += new System.EventHandler(frmMain_Load);
		((System.ComponentModel.ISupportInitialize)this.dgvBuffer).EndInit();
		this.splitContainerMain.Panel1.ResumeLayout(false);
		this.splitContainerMain.Panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.splitContainerMain).EndInit();
		this.splitContainerMain.ResumeLayout(false);
		this.panelOnly1.ResumeLayout(false);
		this.panelOnly1.PerformLayout();
		this.panel1.ResumeLayout(false);
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
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.fpPrintMade).EndInit();
		this.backPanel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.fpProdResult).EndInit();
		base.ResumeLayout(false);
	}
}
