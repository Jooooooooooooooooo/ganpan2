using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Reflection;
using System.Windows.Forms;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;
using MCS.Common.BizRule;

namespace MCS.PrintBoard.PrintBoard;

public class frmMain : frmBase
{
	private string _bcdStr;

	private bool bIsConnected = false;

	private string _APServerIP = string.Empty;

	private string sLast2IP = string.Empty;

	private bool _Update = false;

	private bool _Transit = false;

	private string _menuGroup = "";

	private IContainer components = null;

	private MenuStrip menuStrip1;

	private StatusStrip statusStrip1;

	private ToolStripStatusLabel tsslMsg;

	private ToolStripStatusLabel tsslUserName;

	private ToolStripStatusLabel tsslVersion;

	private ToolStripStatusLabel tsslTime;

	private Timer tmStatTimer;

	private Timer tmServerAlarmChk;

	private Label lblProcess;

	private SerialPort comScaner;

	private FTPClient ftpClient1;

	private ToolStripStatusLabel tssTransit;

	private ToolStripStatusLabel tssForm;

	private SerialPort comScanerFix;

	private ToolStripStatusLabel toolStripStatusLabel1;

	private PictureBox picMesStatus;

	private ToolStripMenuItem deliverySheetMgtToolStripMenuItem;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripMenuItem onLineSheetPrintNewToolStripMenuItem;

	private ToolStripMenuItem onLineSheetMgtToolStripMenuItem;

	private ToolStripMenuItem offlineSheetMgtToolStripMenuItem;

	private ToolStripMenuItem decantMgtToolStripMenuItem;

	private ToolStripMenuItem makeCartToolStripMenuItem;

	private ToolStripMenuItem configToolStripMenuItem;

	private ToolStripMenuItem settingToolStripMenuItem;

	private ToolStripMenuItem monitoringToolStripMenuItem;

	private ToolStripMenuItem sensingMonitoringToolStripMenuItem;

	private ToolStripMenuItem outsideIssueListToolStripMenuItem;

	private ToolStripMenuItem qtyChangePrintToolStripMenuItem;

	public bool BisConnected
	{
		get
		{
			return bIsConnected;
		}
		set
		{
			bIsConnected = value;
		}
	}

	public string APServerIP => _APServerIP;

	public string StatusMessage
	{
		set
		{
			SetStatusMessage(value);
		}
	}

	public frmMain()
	{
		try
		{
			InitializeComponent();
			string strIP = Net.GetIpAddress();
			strIP = strIP.Substring(0, strIP.LastIndexOf('.'));
			tsslUserName.Text = Config.UserName;
			Assembly assembly = Assembly.GetExecutingAssembly();
			DateTime dt = File.GetLastWriteTime(assembly.Location);
			tsslVersion.Text = "[최종수정일자: " + dt.ToString() + "]";
			tsslVersion.ForeColor = Color.Blue;
			tmServerAlarmChk.Enabled = true;
			try
			{
				comScaner.PortName = GetConfigValue("SCANER_PORT");
				comScaner.Open();
			}
			catch
			{
			}
			try
			{
				comScanerFix.PortName = GetConfigValue("FIX_SCANER_PORT");
				comScanerFix.Open();
			}
			catch
			{
			}
			bIsConnected = true;
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			bIsConnected = false;
		}
	}

	private void Menu_Click(object sender, EventArgs e)
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			DataRow dr = (DataRow)((ToolStripMenuItem)sender).Tag;
			string formName = dr["FORMID"].ToString();
			string MenuID = dr["MENUID"].ToString();
			string MenuPlant = dr["SET_PLANT"].ToString();
			int formPrivilege = int.Parse(dr["EDITYN"].ToString());
			Config.GsformName = formName;
			Config.GsMenuID = MenuID;
			Config.GsMenuPlant = MenuPlant;
			int pos = formName.LastIndexOf('.');
			string assmName = Application.StartupPath;
			if (assmName.Substring(assmName.Length - 1, 1) != "\\")
			{
				assmName += "\\";
			}
			assmName = assmName + formName.Substring(0, pos) + ".dll";
			formName = formName.Substring(pos + 1);
			DataSet dtTemp = CommonBizRule.GetPrMenu(base.CurrentShop, dr["MENUID_PR"].ToString(), progressFlag: false);
			string sPrMenu = dtTemp.Tables[0].Rows[0][0].ToString();
			Form[] mdiChildren = base.MdiChildren;
			foreach (Form frm in mdiChildren)
			{
				if (frm.Name == formName)
				{
					if (frm.WindowState == FormWindowState.Minimized)
					{
						frm.WindowState = FormWindowState.Maximized;
					}
					lblProcess.Text = ((ToolStripMenuItem)sender).Text;
					frm.BringToFront();
					return;
				}
			}
			if (!File.Exists(assmName))
			{
				throw new Exception(assmName + " 파일이 존재하지 않습니다.");
			}
			Assembly assm = Assembly.LoadFrom(assmName);
			Type[] types = assm.GetTypes();
			foreach (Type type in types)
			{
				if (type.Name == formName)
				{
					object[] arg = new object[1] { base.CurrentShop };
					frmBase form1 = (frmBase)assm.CreateInstance(type.FullName, ignoreCase: false, BindingFlags.ExactBinding, null, arg, null, null);
					if (form1 == null)
					{
						throw new Exception(formName + "화면을 실행할 수 없습니다.");
					}
					MAINBizRule.InsertLoginHistory(base.CurrentShop, MenuID, progressFlag: false);
					form1.MdiParent = this;
					form1.FormPrivilege = formPrivilege;
					form1.WindowState = FormWindowState.Maximized;
					form1.Text = ((ToolStripMenuItem)sender).Text;
					form1.HelpId = MenuID;
					form1.Show();
					return;
				}
			}
			throw new Exception(formName + "화면은 프로젝트에 포함되어 있지않습니다.");
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
		Cursor.Current = Cursors.Default;
	}

	private void Menu2_Click(object sender, EventArgs e)
	{
		try
		{
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void tmStatTimer_Tick(object sender, EventArgs e)
	{
		try
		{
			if (bIsConnected)
			{
				tmStatTimer.Enabled = true;
			}
			else
			{
				tmStatTimer.Enabled = false;
			}
			tsslTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			tssTransit.Text = "";
			if (_Transit)
			{
				tssTransit.Text = "알림";
				tssTransit.ForeColor = ((tssTransit.ForeColor == Color.Red) ? Color.Black : Color.Red);
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
			tmStatTimer.Enabled = false;
		}
	}

	private void tmServerAlarmChk_Tick(object sender, EventArgs e)
	{
		try
		{
			tmServerAlarmChk.Enabled = false;
			_Transit = false;
			tmServerAlarmChk.Enabled = true;
		}
		catch
		{
			tmServerAlarmChk.Enabled = true;
		}
	}

	private void frmMain_MdiChildActivate(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveMdiChild == null)
			{
				lblProcess.Visible = false;
				lblProcess.Text = "";
				tssForm.Text = "";
				return;
			}
			if (base.CurrentShop == CServerInfo.ServerType.Dev)
			{
				lblProcess.Text = "[DEV] " + base.ActiveMdiChild.Text;
			}
			else
			{
				lblProcess.Text = base.ActiveMdiChild.Text;
			}
			tssForm.Text = base.ActiveMdiChild.Name;
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void frmMain_Resize(object sender, EventArgs e)
	{
		try
		{
			if (base.WindowState != FormWindowState.Minimized)
			{
				picMesStatus.Location = new Point(lblProcess.Width - picMesStatus.Width - 3, picMesStatus.Top);
			}
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			comScaner.Close();
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void comScaner_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		try
		{
			string str = comScaner.ReadExisting();
			switch (base.ActiveMdiChild.Name)
			{
			case "UI_CS_1308":
			case "UI_CS_1312":
			case "UI_CS_1407":
			case "UI_CS_1330":
			case "UI_CS_1353":
				_bcdStr += str;
				if (_bcdStr.EndsWith("\r\n"))
				{
					_bcdStr = "";
				}
				break;
			default:
				SendKeys.SendWait(str.ToUpper());
				break;
			}
		}
		catch (Exception ex)
		{
			_bcdStr = "";
			ShowErrMsg(ex);
		}
	}

	private void comScanerFix_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		try
		{
		}
		catch (Exception ex)
		{
			_bcdStr = "";
			ShowErrMsg(ex);
		}
	}

	private void frmMain_Load(object sender, EventArgs e)
	{
		frmBase.gstBarMain = statusStrip1;
		McsServerInfo.ServerInfo();
		offlineSheetMgtToolStripMenuItem.Visible = false;
		onLineSheetMgtToolStripMenuItem.Visible = false;
		onLineSheetPrintNewToolStripMenuItem.Visible = false;
		decantMgtToolStripMenuItem.Visible = false;
		outsideIssueListToolStripMenuItem.Visible = false;
		monitoringToolStripMenuItem.Visible = false;
		qtyChangePrintToolStripMenuItem.Visible = false;
		configToolStripMenuItem.Visible = false;
		if (McsServerInfo.xOFFLINE_VISIBLE == "YES")
		{
			offlineSheetMgtToolStripMenuItem.Visible = true;
		}
		if (McsServerInfo.xONLINE_VISIBLE == "YES")
		{
			onLineSheetMgtToolStripMenuItem.Visible = true;
			onLineSheetPrintNewToolStripMenuItem.Visible = true;
		}
		if (McsServerInfo.xDECANT_VISIBLE == "YES")
		{
			decantMgtToolStripMenuItem.Visible = true;
		}
		if (McsServerInfo.xOUTSIDE_VISIBLE == "YES")
		{
			outsideIssueListToolStripMenuItem.Visible = true;
		}
		if (McsServerInfo.xMONITORING_VISIBLE == "YES")
		{
			monitoringToolStripMenuItem.Visible = true;
		}
		if (McsServerInfo.xQTYCHANGE_VISIBLE == "YES")
		{
			qtyChangePrintToolStripMenuItem.Visible = true;
		}
		if (McsServerInfo.xCONFIG_VISIBLE == "YES")
		{
			configToolStripMenuItem.Visible = true;
		}
	}

	public void SetStatusMessage(string Message)
	{
		tsslMsg.Text = Message;
	}

	private void offlineSheetMgtToolStripMenuItem_Click(object sender, EventArgs e)
	{
		foreach (Form frm in Application.OpenForms)
		{
			if (frm.GetType() == typeof(frmOffLinePrint))
			{
				frm.Activate();
				frm.BringToFront();
				frm.WindowState = FormWindowState.Maximized;
				frm.MdiParent = this;
				return;
			}
		}
		frmOffLinePrint form = new frmOffLinePrint();
		form.WindowState = FormWindowState.Maximized;
		form.MdiParent = this;
		form.Show();
	}

	private void onLineSheetPrintNewToolStripMenuItem_Click(object sender, EventArgs e)
	{
		foreach (Form frm in Application.OpenForms)
		{
			if (frm.GetType() == typeof(frmOnLinePrintNew))
			{
				frm.Activate();
				frm.BringToFront();
				frm.WindowState = FormWindowState.Maximized;
				frm.MdiParent = this;
				return;
			}
		}
		frmOnLinePrintNew form = new frmOnLinePrintNew();
		form.WindowState = FormWindowState.Maximized;
		form.MdiParent = this;
		form.Show();
	}

	private void onLineSheetMgtToolStripMenuItem_Click(object sender, EventArgs e)
	{
		foreach (Form frm in Application.OpenForms)
		{
			if (frm.GetType() == typeof(frmOnLinePrint))
			{
				frm.Activate();
				frm.BringToFront();
				frm.WindowState = FormWindowState.Maximized;
				frm.MdiParent = this;
				return;
			}
		}
		frmOnLinePrint form = new frmOnLinePrint();
		form.WindowState = FormWindowState.Maximized;
		form.MdiParent = this;
		form.Show();
	}

	private void makeCartToolStripMenuItem_Click(object sender, EventArgs e)
	{
		foreach (Form frm in Application.OpenForms)
		{
			if (frm.GetType() == typeof(frmDecantMakeCart))
			{
				frm.Activate();
				frm.BringToFront();
				frm.WindowState = FormWindowState.Maximized;
				frm.MdiParent = this;
				return;
			}
		}
		frmDecantMakeCart form = new frmDecantMakeCart();
		form.WindowState = FormWindowState.Maximized;
		form.MdiParent = this;
		form.Show();
	}

	private void multiToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void carrierStatusToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void settingToolStripMenuItem_Click(object sender, EventArgs e)
	{
		frmConfig popup = null;
		try
		{
			popup = new frmConfig();
			popup.StartPosition = FormStartPosition.CenterScreen;
			popup.ShowDialog();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void monitoringToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void sensingMonitoringToolStripMenuItem_Click(object sender, EventArgs e)
	{
	}

	private void outsideIssueListToolStripMenuItem_Click(object sender, EventArgs e)
	{
		foreach (Form frm in Application.OpenForms)
		{
			if (frm.GetType() == typeof(frmOutsideIssueList))
			{
				frm.Activate();
				frm.BringToFront();
				frm.WindowState = FormWindowState.Maximized;
				frm.MdiParent = this;
				return;
			}
		}
		frmOutsideIssueList form = new frmOutsideIssueList();
		form.WindowState = FormWindowState.Maximized;
		form.MdiParent = this;
		form.Show();
	}

	private void qtyChangePrintToolStripMenuItem_Click(object sender, EventArgs e)
	{
		foreach (Form frm in Application.OpenForms)
		{
			if (frm.GetType() == typeof(frmQtyChangePrint))
			{
				frm.Activate();
				frm.BringToFront();
				frm.WindowState = FormWindowState.Maximized;
				frm.MdiParent = this;
				return;
			}
		}
		frmQtyChangePrint form = new frmQtyChangePrint();
		form.WindowState = FormWindowState.Maximized;
		form.MdiParent = this;
		form.Show();
	}

	private ToolStripMenuItem MakeMenuItem(DataRow row)
	{
		try
		{
			ToolStripMenuItem menu = new ToolStripMenuItem(row["MENUNAME"].ToString());
			if (row["FORMID"].ToString() != "*")
			{
				menu.Click += Menu_Click;
				menu.Tag = row;
			}
			menu.Name = row["MENUID"].ToString();
			return menu;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private void InitializeMenuItem(ToolStripMenuItem root, DataTable dt)
	{
		try
		{
			foreach (DataRow row in dt.Rows)
			{
				if (row["MENUID_PR"].ToString() == root.Name)
				{
					ToolStripMenuItem menu = MakeMenuItem(row);
					root.DropDownItems.Add(menu);
					InitializeMenuItem(menu, dt);
				}
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	private bool ChkDownloadFile(string strFileDir)
	{
		try
		{
			string strDataSet = "INDATA(FILE_NAME:STRING,FILE_DIR:STRING)";
			using (DataSet ds = MakeDataSet(strDataSet))
			{
				DataRow dr = ds.Tables["INDATA"].NewRow();
				dr["FILE_NAME"] = null;
				dr["FILE_DIR"] = ((strFileDir == "") ? null : strFileDir);
				ds.Tables["INDATA"].Rows.Add(dr);
				using DataSet ret = ExecuteService("R_UPDATEFILE", "INDATA", "OUTDATA", ds, progressFlag: false);
				foreach (DataRow rr in ret.Tables["OUTDATA"].Rows)
				{
					string strFileName1 = rr["FILE_NAME"].ToString();
					string strFileDate1 = rr["FILE_DATE"].ToString();
					string strFileDir2 = rr["FILE_DIR"].ToString();
					if (strFileDir2 == "SolarMES")
					{
						strFileDir2 = ".";
					}
					string strFileDir3 = Application.StartupPath;
					if (strFileDir3.Substring(strFileDir3.Length - 1) != "\\")
					{
						strFileDir3 += "\\";
					}
					if (strFileDir2 != ".")
					{
						strFileDir3 = strFileDir3 + strFileDir3 + "\\";
					}
					string strFileName2 = strFileDir3 + strFileName1;
					if (!File.Exists(strFileName2))
					{
						return true;
					}
					string strFileDate2 = File.GetLastWriteTime(strFileName2).ToString("yyyyMMddHHmmss");
					if (strFileDate1.CompareTo(strFileDate2) > 0)
					{
						return true;
					}
				}
			}
			return false;
		}
		catch
		{
			return false;
		}
	}

	private void ChkDownloadFileSolarMES()
	{
		try
		{
			if (!ChkDownloadFile("SolarMES"))
			{
				return;
			}
			string strDataSet = "INDATA(FILE_NAME:STRING)";
			using DataSet ds = MakeDataSet(strDataSet);
			DataRow dr = ds.Tables["INDATA"].NewRow();
			dr["FILE_NAME"] = "SolarMES.exe";
			ds.Tables["INDATA"].Rows.Add(dr);
			using DataSet ret = ExecuteService("R_UPDATEFILE_NAME", "INDATA", "OUTDATA", ds, progressFlag: false);
			string strFileName = Application.StartupPath;
			if (strFileName.Substring(strFileName.Length - 1) != "\\")
			{
				strFileName += "\\";
			}
			strFileName += "SolarMES.exe";
			File.WriteAllBytes(strFileName, (byte[])ret.Tables["OUTDATA"].Rows[0]["FILE_DESC"]);
		}
		catch
		{
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmMain));
		this.statusStrip1 = new System.Windows.Forms.StatusStrip();
		this.tsslMsg = new System.Windows.Forms.ToolStripStatusLabel();
		this.tssForm = new System.Windows.Forms.ToolStripStatusLabel();
		this.tsslUserName = new System.Windows.Forms.ToolStripStatusLabel();
		this.tssTransit = new System.Windows.Forms.ToolStripStatusLabel();
		this.tsslVersion = new System.Windows.Forms.ToolStripStatusLabel();
		this.tsslTime = new System.Windows.Forms.ToolStripStatusLabel();
		this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
		this.tmStatTimer = new System.Windows.Forms.Timer(this.components);
		this.tmServerAlarmChk = new System.Windows.Forms.Timer(this.components);
		this.lblProcess = new System.Windows.Forms.Label();
		this.menuStrip1 = new System.Windows.Forms.MenuStrip();
		this.deliverySheetMgtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.offlineSheetMgtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.onLineSheetPrintNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.onLineSheetMgtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.decantMgtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.makeCartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.outsideIssueListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.qtyChangePrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.monitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.sensingMonitoringToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.comScaner = new System.IO.Ports.SerialPort(this.components);
		this.ftpClient1 = new MCS.Common.FTPClient(this.components);
		this.comScanerFix = new System.IO.Ports.SerialPort(this.components);
		this.picMesStatus = new System.Windows.Forms.PictureBox();
		this.statusStrip1.SuspendLayout();
		this.menuStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picMesStatus).BeginInit();
		base.SuspendLayout();
		resources.ApplyResources(this.statusStrip1, "statusStrip1");
		this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.tsslMsg, this.tssForm, this.tsslUserName, this.tssTransit, this.tsslVersion, this.tsslTime, this.toolStripStatusLabel1 });
		this.statusStrip1.Name = "statusStrip1";
		this.tsslMsg.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
		this.tsslMsg.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
		this.tsslMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		resources.ApplyResources(this.tsslMsg, "tsslMsg");
		this.tsslMsg.Name = "tsslMsg";
		this.tsslMsg.Spring = true;
		resources.ApplyResources(this.tssForm, "tssForm");
		this.tssForm.Name = "tssForm";
		this.tsslUserName.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
		this.tsslUserName.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
		this.tsslUserName.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		resources.ApplyResources(this.tsslUserName, "tsslUserName");
		this.tsslUserName.Name = "tsslUserName";
		resources.ApplyResources(this.tssTransit, "tssTransit");
		this.tssTransit.Name = "tssTransit";
		this.tsslVersion.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
		this.tsslVersion.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
		this.tsslVersion.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		resources.ApplyResources(this.tsslVersion, "tsslVersion");
		this.tsslVersion.Name = "tsslVersion";
		this.tsslTime.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
		this.tsslTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		resources.ApplyResources(this.tsslTime, "tsslTime");
		this.tsslTime.Name = "tsslTime";
		this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
		resources.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
		this.tmStatTimer.Enabled = true;
		this.tmStatTimer.Interval = 1000;
		this.tmStatTimer.Tick += new System.EventHandler(tmStatTimer_Tick);
		this.tmServerAlarmChk.Enabled = true;
		this.tmServerAlarmChk.Interval = 60000;
		this.tmServerAlarmChk.Tick += new System.EventHandler(tmServerAlarmChk_Tick);
		this.lblProcess.BackColor = System.Drawing.Color.Transparent;
		resources.ApplyResources(this.lblProcess, "lblProcess");
		this.lblProcess.ForeColor = System.Drawing.Color.Ivory;
		this.lblProcess.Name = "lblProcess";
		resources.ApplyResources(this.menuStrip1, "menuStrip1");
		this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.deliverySheetMgtToolStripMenuItem, this.decantMgtToolStripMenuItem, this.outsideIssueListToolStripMenuItem, this.qtyChangePrintToolStripMenuItem, this.monitoringToolStripMenuItem, this.configToolStripMenuItem });
		this.menuStrip1.Name = "menuStrip1";
		this.deliverySheetMgtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.offlineSheetMgtToolStripMenuItem, this.toolStripSeparator1, this.onLineSheetPrintNewToolStripMenuItem, this.onLineSheetMgtToolStripMenuItem });
		resources.ApplyResources(this.deliverySheetMgtToolStripMenuItem, "deliverySheetMgtToolStripMenuItem");
		this.deliverySheetMgtToolStripMenuItem.Name = "deliverySheetMgtToolStripMenuItem";
		resources.ApplyResources(this.offlineSheetMgtToolStripMenuItem, "offlineSheetMgtToolStripMenuItem");
		this.offlineSheetMgtToolStripMenuItem.Name = "offlineSheetMgtToolStripMenuItem";
		this.offlineSheetMgtToolStripMenuItem.Click += new System.EventHandler(offlineSheetMgtToolStripMenuItem_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
		resources.ApplyResources(this.onLineSheetPrintNewToolStripMenuItem, "onLineSheetPrintNewToolStripMenuItem");
		this.onLineSheetPrintNewToolStripMenuItem.Name = "onLineSheetPrintNewToolStripMenuItem";
		this.onLineSheetPrintNewToolStripMenuItem.Click += new System.EventHandler(onLineSheetPrintNewToolStripMenuItem_Click);
		resources.ApplyResources(this.onLineSheetMgtToolStripMenuItem, "onLineSheetMgtToolStripMenuItem");
		this.onLineSheetMgtToolStripMenuItem.Name = "onLineSheetMgtToolStripMenuItem";
		this.onLineSheetMgtToolStripMenuItem.Click += new System.EventHandler(onLineSheetMgtToolStripMenuItem_Click);
		this.decantMgtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.makeCartToolStripMenuItem });
		resources.ApplyResources(this.decantMgtToolStripMenuItem, "decantMgtToolStripMenuItem");
		this.decantMgtToolStripMenuItem.Name = "decantMgtToolStripMenuItem";
		this.makeCartToolStripMenuItem.Name = "makeCartToolStripMenuItem";
		resources.ApplyResources(this.makeCartToolStripMenuItem, "makeCartToolStripMenuItem");
		this.makeCartToolStripMenuItem.Click += new System.EventHandler(makeCartToolStripMenuItem_Click);
		resources.ApplyResources(this.outsideIssueListToolStripMenuItem, "outsideIssueListToolStripMenuItem");
		this.outsideIssueListToolStripMenuItem.Name = "outsideIssueListToolStripMenuItem";
		this.outsideIssueListToolStripMenuItem.Click += new System.EventHandler(outsideIssueListToolStripMenuItem_Click);
		resources.ApplyResources(this.qtyChangePrintToolStripMenuItem, "qtyChangePrintToolStripMenuItem");
		this.qtyChangePrintToolStripMenuItem.Name = "qtyChangePrintToolStripMenuItem";
		this.qtyChangePrintToolStripMenuItem.Click += new System.EventHandler(qtyChangePrintToolStripMenuItem_Click);
		this.monitoringToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.sensingMonitoringToolStripMenuItem });
		resources.ApplyResources(this.monitoringToolStripMenuItem, "monitoringToolStripMenuItem");
		this.monitoringToolStripMenuItem.Name = "monitoringToolStripMenuItem";
		this.monitoringToolStripMenuItem.Click += new System.EventHandler(monitoringToolStripMenuItem_Click);
		this.sensingMonitoringToolStripMenuItem.Name = "sensingMonitoringToolStripMenuItem";
		resources.ApplyResources(this.sensingMonitoringToolStripMenuItem, "sensingMonitoringToolStripMenuItem");
		this.sensingMonitoringToolStripMenuItem.Click += new System.EventHandler(sensingMonitoringToolStripMenuItem_Click);
		this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.settingToolStripMenuItem });
		resources.ApplyResources(this.configToolStripMenuItem, "configToolStripMenuItem");
		this.configToolStripMenuItem.Name = "configToolStripMenuItem";
		this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
		resources.ApplyResources(this.settingToolStripMenuItem, "settingToolStripMenuItem");
		this.settingToolStripMenuItem.Click += new System.EventHandler(settingToolStripMenuItem_Click);
		this.comScaner.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(comScaner_DataReceived);
		this.ftpClient1.Password = "";
		this.ftpClient1.Port = 21;
		this.ftpClient1.Server = "";
		this.ftpClient1.Timeout = 30;
		this.ftpClient1.User = "";
		this.comScanerFix.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(comScanerFix_DataReceived);
		resources.ApplyResources(this.picMesStatus, "picMesStatus");
		this.picMesStatus.Name = "picMesStatus";
		this.picMesStatus.TabStop = false;
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
		resources.ApplyResources(this, "$this");
		base.Controls.Add(this.lblProcess);
		base.Controls.Add(this.menuStrip1);
		base.Controls.Add(this.statusStrip1);
		base.Controls.Add(this.picMesStatus);
		base.IsMdiContainer = true;
		base.MainMenuStrip = this.menuStrip1;
		base.Name = "frmMain";
		base.Tag = "Ver 0.1";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmMain_FormClosing);
		base.Load += new System.EventHandler(frmMain_Load);
		base.MdiChildActivate += new System.EventHandler(frmMain_MdiChildActivate);
		base.Resize += new System.EventHandler(frmMain_Resize);
		this.statusStrip1.ResumeLayout(false);
		this.statusStrip1.PerformLayout();
		this.menuStrip1.ResumeLayout(false);
		this.menuStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.picMesStatus).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
