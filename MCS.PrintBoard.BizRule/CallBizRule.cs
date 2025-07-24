using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using LGCNS.ezMES.HTML5.Common;
using MCS.Common;

namespace MCS.PrintBoard.BizRule;

public class CallBizRule
{
	public DataSet CallBizMCSAGVStatus(string st)
	{
		try
		{
			DataSet dsInput = new DataSet();
			string strMCSCallTime = DateTime.Now.ToString("yyyyMMddHHmmss");
			return new DataSet();
		}
		catch (Exception)
		{
			return null;
		}
	}

	public async void CallBizOrder(Dictionary<string, string> args)
	{
		DataSet dsInput = new DataSet();
		Stopwatch sw = new Stopwatch();
		try
		{
			string sID = string.Empty;
			string sTXN_DATE = DateTime.Now.ToString("yyyyMMddHHmmss");
			string sORDER_ID = null;
			string sFROM_LOCATOR = null;
			string sFROM_GUBUN = null;
			string sTO_LOCATOR = null;
			string sTO_GUBUN = null;
			string sTARGET_ID = null;
			string sGROUP_ID = null;
			string sAGV_ID = null;
			string sSTATUS = string.Empty;
			_ = string.Empty;
			_ = string.Empty;
			args.TryGetValue("ID", out sID);
			args.TryGetValue("ORDER_ID", out sORDER_ID);
			args.TryGetValue("FROM_LOCATOR", out sFROM_LOCATOR);
			args.TryGetValue("FROM_GUBUN", out sFROM_GUBUN);
			args.TryGetValue("TO_LOCATOR", out sTO_LOCATOR);
			args.TryGetValue("TO_GUBUN", out sTO_GUBUN);
			args.TryGetValue("TARGET_ID", out sTARGET_ID);
			args.TryGetValue("GROUP_ID", out sGROUP_ID);
			args.TryGetValue("AGV_ID", out sAGV_ID);
			switch (sID)
			{
			case "ORDER_CANCEL":
				sSTATUS = "D";
				break;
			case "ORDER_ASSIGN":
				sSTATUS = "F";
				break;
			case "ORDER_START":
				sSTATUS = "S";
				break;
			case "ORDER_COMPLETE":
				sSTATUS = "C";
				break;
			}
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_ORDER_ID", sORDER_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_STATUS", sSTATUS);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_FROM_LOCATOR", sFROM_LOCATOR);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_FROM_GUBUN", sFROM_GUBUN);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_TO_LOCATOR", sTO_LOCATOR);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_TO_GUBUN", sTO_GUBUN);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_TARGET_ID", sTARGET_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_GROUP_ID", sGROUP_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_AGV_ID", sAGV_ID);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_TXNDATE", sTXN_DATE);
			LGCNS.ezMES.HTML5.Common.Common.MakeDataTable(ref dsInput, "IN_DATA", "I_USER_ID", "MCSServer");
			BizService bizServer = new BizService();
			Log.SaveLog("RECEIVE", "BIZ : GMCS_RUN_ORDER_STATUS_TXN Start ");
			sw.Start();
			DataSet dsResult = bizServer.ExecBizRule("GMCS_RUN_ORDER_STATUS_TXN", dsInput, "IN_DATA", "OUT_DATA");
			sw.Stop();
			string sO_RTN_CODE = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_CODE"].ToString();
			string sO_RTN_MSG = dsResult.Tables["OUT_DATA"].Rows[0]["O_RTN_MSG"].ToString();
			Log.SaveLog("RECEIVE", "BIZ : GMCS_RUN_ORDER_STATUS_TXN End [Elapsed Time : " + sw.Elapsed.ToString() + "] " + sO_RTN_CODE + " : " + sO_RTN_MSG);
		}
		catch (Exception)
		{
			sw.Stop();
			Log.SaveLog("RECEIVE", "BIZ Call Error : GMCS_RUN_ORDER_STATUS_TXN  Error");
		}
	}

	public async void CallBiz44050(Dictionary<string, string> args)
	{
		try
		{
			new DataSet();
			DateTime.Now.ToString("yyyyMMddHHmmss");
			string sCEID = string.Empty;
			args.TryGetValue("CEID", out sCEID);
			new DataSet();
		}
		catch (Exception)
		{
		}
	}
}
