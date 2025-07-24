using System;
using System.Data;
using System.Windows.Forms;
using LGCNS.ezMES.HTML5.Common;

namespace MCS.PrintBoard.PrintBoard;

internal class CommonBiz
{
	public string callGmcsSetError(string pRtnMsg)
	{
		string sReturnMsg = "";
		try
		{
			DataSet ds = new DataSet();
			BizService bizServer = new BizService();
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref ds, "IN_DATA", "I_RTN_MSG", pRtnMsg);
			DataSet dsResult = bizServer.ExecBizRule("GMCS_SET_ERROR_LOG_HIST", ds, "IN_DATA", "OUT_DATA");
			if (dsResult.Tables["OUT_DATA"].Rows.Count > 0)
			{
				sReturnMsg = dsResult.Tables["OUT_DATA"].Rows[0]["I_ERROR_MSG"].ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
			sReturnMsg = "";
		}
		return sReturnMsg;
	}
}
