using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;
using MCS.PrintBoard.Properties;

namespace MCS.PrintBoard.PrintBoard;

public class frmLocatorGroupTree_P : frmBase
{
	private Font fBold = new Font("맑은 고딕", 9f, FontStyle.Bold);

	private Font fRegular = new Font("맑은 고딕", 9f, FontStyle.Regular);

	private TreeNode tnTemp = null;

	private string _LocatorGroupCode;

	private string _LocatorGroupName;

	private DataSet dsLocatorGroup = new DataSet();

	private IContainer components = null;

	private SplitContainer splitContainer1;

	private System.Windows.Forms.Button btn_close;

	private TreeView tvLocatorGroup;

	private TextBox txtDescription;

	private TextBox txtCode;

	private ImageList imgList;

	public string sLocatorGroupCode
	{
		get
		{
			return _LocatorGroupCode;
		}
		set
		{
			_LocatorGroupCode = value;
		}
	}

	public string sLocatorGroupName
	{
		get
		{
			return _LocatorGroupName;
		}
		set
		{
			_LocatorGroupName = value;
		}
	}

	public frmLocatorGroupTree_P()
	{
		InitializeComponent();
	}

	private void frmLocatorGroupTree_P_Load(object sender, EventArgs e)
	{
		try
		{
			GetLocationGroup();
			AddTreeInfo(dsLocatorGroup, "");
		}
		catch (Exception ex)
		{
			ShowErrMsg(ex);
		}
	}

	private void GetLocationGroup()
	{
		try
		{
			DataSet ds = new DataSet();
			string sOrgCode = "203";
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "ORG_ID", (sOrgCode == "") ? null : sOrgCode);
			BizService bizServer = new BizService();
			dsLocatorGroup = bizServer.ExecBizRule("GMCS_GET_LOCATOR_GROUP_INFO", ds, "IN_DATA", "OUT_DATA");
		}
		catch (Exception)
		{
		}
	}

	private void AddTreeInfo(DataSet dsTemp, string pParentNodeId)
	{
		tvLocatorGroup.Nodes.Clear();
		tvLocatorGroup.BeginUpdate();
		tnTemp = null;
		string sNODEID = string.Empty;
		string sName = string.Empty;
		int iType = 0;
		string filter0 = string.Format(" TYPE = '{0}' ", "0");
		DataRow[] Rows0 = dsTemp.Tables[0].Select(filter0);
		DataRow[] array = Rows0;
		foreach (DataRow dtrow2 in array)
		{
			sNODEID = dtrow2["NODEID"].ToString();
			sName = dtrow2["NAME"].ToString();
			iType = int.Parse(dtrow2["TYPE"].ToString());
			tvLocatorGroup.Nodes.Add(sNODEID, sName, iType);
		}
		string filter1 = string.Format(" TYPE <> '{0}' ", "0");
		DataRow[] Rows1 = dsTemp.Tables[0].Select(filter1);
		DataRow[] array2 = Rows1;
		foreach (DataRow dtrow in array2)
		{
			string sParentNodeId = string.Empty;
			try
			{
				sNODEID = dtrow["NODEID"].ToString();
				sName = dtrow["NAME"].ToString();
				sParentNodeId = dtrow["PARENTNODEID"].ToString();
				iType = int.Parse(dtrow["TYPE"].ToString());
				switch (iType)
				{
				case 1:
					tvLocatorGroup.Nodes[sParentNodeId].Nodes.Add(sNODEID, sName, iType);
					break;
				case 2:
					tvLocatorGroup.Nodes[sParentNodeId.Substring(0, 1)].Nodes[sParentNodeId].Nodes.Add(sNODEID, sName, iType);
					break;
				}
			}
			catch (Exception)
			{
			}
		}
		tvLocatorGroup.EndUpdate();
	}

	private void tvLocatorGroup_AfterSelect(object sender, TreeViewEventArgs e)
	{
		setControlInit();
		string sNode = e.Node.Name;
		string sNodeText = e.Node.Text;
		tnTemp = e.Node;
	}

	private void setControlInit()
	{
		try
		{
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
		}
	}

	private void tvLocatorGroup_AfterCheck(object sender, TreeViewEventArgs e)
	{
		for (int i = 0; i < e.Node.Nodes.Count; i++)
		{
			e.Node.Nodes[i].Checked = e.Node.Checked;
		}
	}

	private void btn_close_Click(object sender, EventArgs e)
	{
		GetChekedNodes(tvLocatorGroup.Nodes);
		base.DialogResult = DialogResult.OK;
		Close();
	}

	private void GetChekedNodes(TreeNodeCollection e)
	{
		string sCode = string.Empty;
		string sDescription = string.Empty;
		foreach (TreeNode tnL0 in tvLocatorGroup.Nodes)
		{
			foreach (TreeNode tnL1 in tnL0.Nodes)
			{
				foreach (TreeNode tnL2 in tnL1.Nodes)
				{
					if (tnL2.Checked && tnL2.Level == 2)
					{
						if (!string.IsNullOrEmpty(sCode))
						{
							sCode += ",";
						}
						sCode += tnL2.Name;
						if (!string.IsNullOrEmpty(sDescription))
						{
							sDescription += ",";
						}
						sDescription += tnL2.Text;
					}
				}
			}
		}
		txtCode.Text = sCode;
		txtDescription.Text = sDescription;
		sLocatorGroupCode = sCode;
		sLocatorGroupName = sDescription;
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MCS.PrintBoard.PrintBoard.frmLocatorGroupTree_P));
		this.splitContainer1 = new System.Windows.Forms.SplitContainer();
		this.tvLocatorGroup = new System.Windows.Forms.TreeView();
		this.btn_close = new System.Windows.Forms.Button();
		this.txtDescription = new System.Windows.Forms.TextBox();
		this.txtCode = new System.Windows.Forms.TextBox();
		this.imgList = new System.Windows.Forms.ImageList(this.components);
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
		this.splitContainer1.Panel1.SuspendLayout();
		this.splitContainer1.Panel2.SuspendLayout();
		this.splitContainer1.SuspendLayout();
		base.SuspendLayout();
		this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
		this.splitContainer1.Location = new System.Drawing.Point(0, 0);
		this.splitContainer1.Name = "splitContainer1";
		this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
		this.splitContainer1.Panel1.Controls.Add(this.tvLocatorGroup);
		this.splitContainer1.Panel2.Controls.Add(this.btn_close);
		this.splitContainer1.Panel2.Controls.Add(this.txtDescription);
		this.splitContainer1.Panel2.Controls.Add(this.txtCode);
		this.splitContainer1.Size = new System.Drawing.Size(296, 580);
		this.splitContainer1.SplitterDistance = 534;
		this.splitContainer1.TabIndex = 0;
		this.tvLocatorGroup.BackColor = System.Drawing.Color.White;
		this.tvLocatorGroup.CheckBoxes = true;
		this.tvLocatorGroup.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tvLocatorGroup.Location = new System.Drawing.Point(0, 0);
		this.tvLocatorGroup.Name = "tvLocatorGroup";
		this.tvLocatorGroup.Size = new System.Drawing.Size(296, 534);
		this.tvLocatorGroup.TabIndex = 0;
		this.tvLocatorGroup.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(tvLocatorGroup_AfterCheck);
		this.tvLocatorGroup.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(tvLocatorGroup_AfterSelect);
		this.btn_close.BackColor = System.Drawing.Color.Transparent;
		this.btn_close.BackgroundImage = MCS.PrintBoard.Properties.Resources.sbtnbg;
		this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.btn_close.Font = new System.Drawing.Font("Arial", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btn_close.ForeColor = System.Drawing.Color.Black;
		this.btn_close.Image = (System.Drawing.Image)resources.GetObject("btn_close.Image");
		this.btn_close.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btn_close.Location = new System.Drawing.Point(80, 8);
		this.btn_close.Name = "btn_close";
		this.btn_close.Size = new System.Drawing.Size(137, 30);
		this.btn_close.TabIndex = 0;
		this.btn_close.Text = "  Apply";
		this.btn_close.UseVisualStyleBackColor = false;
		this.btn_close.Click += new System.EventHandler(btn_close_Click);
		this.txtDescription.Location = new System.Drawing.Point(3, 18);
		this.txtDescription.Name = "txtDescription";
		this.txtDescription.Size = new System.Drawing.Size(281, 21);
		this.txtDescription.TabIndex = 2;
		this.txtDescription.Visible = false;
		this.txtCode.Location = new System.Drawing.Point(3, 5);
		this.txtCode.Name = "txtCode";
		this.txtCode.Size = new System.Drawing.Size(281, 21);
		this.txtCode.TabIndex = 1;
		this.txtCode.Visible = false;
		this.imgList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imgList.ImageStream");
		this.imgList.TransparentColor = System.Drawing.Color.Transparent;
		this.imgList.Images.SetKeyName(0, "editclear.png");
		this.imgList.Images.SetKeyName(1, "NewDocumentC.png");
		this.imgList.Images.SetKeyName(2, "new-document.png");
		this.imgList.Images.SetKeyName(3, "newN.png");
		base.AcceptButton = this.btn_close;
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 12f);
		this.AutoSize = true;
		base.ClientSize = new System.Drawing.Size(296, 580);
		base.Controls.Add(this.splitContainer1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "frmLocatorGroupTree_P";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "LocatorGroup";
		base.Load += new System.EventHandler(frmLocatorGroupTree_P_Load);
		this.splitContainer1.Panel1.ResumeLayout(false);
		this.splitContainer1.Panel2.ResumeLayout(false);
		this.splitContainer1.Panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
		this.splitContainer1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
