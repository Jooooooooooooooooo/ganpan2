using System.Data;

namespace MCS.Common.BizRule;

public class Oracle_BIZ
{
	public static DataSet PROD_PLAN(bool bFlag)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = " Select * from EZMES.VW_SFC_TV_VISION_MTRL_PL_SUMM";
		return OracleDBConn.execSql("Oracle_BIZ.PROD_PLAN", sSQL, bFlag);
	}

	public static DataSet UI_VENDOR_LINE(CServerInfo.ServerType Server, string sGubun, bool bFlag)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = "\nSELECT DISTINCT\n       A.PCSGNAME||'-'||A.PCSGDESC AS CODEDESC \n      ,A.PCSGNAME  AS CODE\n  FROM EZMES_APP.PROCESSSEGMENT A\n WHERE A.PCSGIUSE =  '[1]'\nORDER BY PCSGNAME\n";
		sSQL = sSQL.Replace("[1]", "Y");
		return OracleDBConn.execSql("AutoScanBizRule.UI_VENDOR_LINE", sSQL, bFlag);
	}

	public static DataSet UI_VENDOR_NAME(CServerInfo.ServerType Server, string sGubun, bool bFlag)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = "\nSELECT DISTINCT\n       A.VENDOR_SITE_NAME CODEDESC\n      ,A.VENDOR_SITE_ID   CODE\n  FROM EZMES.TB_WTM_OSP_VENDOR_BAS A\nORDER BY 1, 2\n";
		return OracleDBConn.execSql("AutoScanBizRule.UI_VENDOR_NAME", sSQL, bFlag);
	}
}
