using System.Data;

namespace MCS.Common.BizRule;

public class MsSql_BIZ
{
	public static DataSet UI_FACTORY_BASIC(bool bFlag)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = "\nSELECT CP.PLANT AS FACTORY_CODE, CP.ENG_NAME AS DESCRIPTION\nFROM   [LGE.TV.MA_PACKING].[dbo].[COM_PLANT] CP\nORDER BY CP.SORT_SEQ";
		return MsSqlDBConn.execSql("CommonBizRule.UI_FACTORY_BASIC", sSQL, bFlag);
	}

	public static DataSet IMAGE_EXIST(string sPartNo)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = "\n                            SELECT  [PARTNO]\n                                          ,[IMAGE_PATH1]\n                                          ,[IMAGE_PATH2]\n                                          ,[IMAGE_PATH3]\n                                          ,[IMAGE_PATH4]\n                                          ,[CREATE_DATE]\n                            FROM    [LGE.TV.VISION].[dbo].[IMAGE_BY_PARTNO]\n                            WHERE [PARTNO] =  '[1]'     ";
		sSQL = sSQL.Replace("[1]", sPartNo);
		return MsSqlDBConn.execSql("MaVLIBizRule.IMAGE_EXIST", sSQL, progressFlag: false);
	}

	public static DataSet ITEM_PARTNO_EXIST(string sModel, string sItemType)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = "\n                            SELECT  MODEL_SUFFIX, \n                                          ITEM_TYPE, \n                                          PARTNO, \n                                          USERNAME, \n                                          LINE,\n                                          CREATE_DATE\n                            FROM    [LGE.TV.VISION].[dbo].[PARTNO_LIST_BY_MODEL]\n                            WHERE [MODEL_SUFFIX] =  '[1]'    \n                                 AND [ITEM_TYPE] = '[2]'";
		sSQL = sSQL.Replace("[1]", sModel);
		sSQL = sSQL.Replace("[2]", sItemType);
		return MsSqlDBConn.execSql("MaVLIBizRule.ITEM_PARTNO_EXIST", sSQL, progressFlag: false);
	}

	public static DataSet PARTNO_LIST_BY_MODEL(string sModel)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = "\n                            SELECT  MODEL_SUFFIX, \n                                          ITEM_TYPE, \n                                          PARTNO, \n                                          USERNAME, \n                                          LINE,\n                                          CREATE_DATE\n                            FROM    [LGE.TV.VISION].[dbo].[PARTNO_LIST_BY_MODEL]\n                            WHERE [MODEL_SUFFIX] LIKE  '[1]'    ";
		sSQL = sSQL.Replace("[1]", sModel + "%");
		return MsSqlDBConn.execSql("MaVLIBizRule.PARTNO_LIST_BY_MODEL", sSQL, progressFlag: false);
	}

	public static DataSet MODEL_IMAGE(string sModel)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = "\n             SELECT A.ITEM_TYPE\n                          ,A.PARTNO\n                          ,B.IMAGE_PATH1 \n\t                      ,B.IMAGE_PATH2\n\t                      ,B.IMAGE_PATH3 \n\t                      ,B.IMAGE_PATH4\n              FROM [LGE.TV.VISION].[dbo].[PARTNO_LIST_BY_MODEL] A, \n                          [LGE.TV.VISION].[dbo].[IMAGE_BY_PARTNO]  B\n             WHERE A.PARTNO = B.PARTNO\n                  AND A.MODEL_SUFFIX =  '[1]'     ";
		sSQL = sSQL.Replace("[1]", sModel);
		return MsSqlDBConn.execSql("MaVLIBizRule.MODEL_IMAGE", sSQL, progressFlag: false);
	}

	public static DataSet PARTNO_EXIST(string sModel)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsQuery = null;
		sSQL = "\n                            SELECT [MODEL_SUFFIX]\n                                          ,[ITEM_TYPE]\n                                          ,[PARTNO]\n                                          ,[USERNAME]\n                                          ,[LINE]\n                                          ,[CREATE_DATE]\n                            FROM [LGE.TV.VISION].[dbo].[PARTNO_LIST_BY_MODEL]\n                            WHERE [MODEL_SUFFIX] =  '[1]'     ";
		sSQL = sSQL.Replace("[1]", sModel);
		return MsSqlDBConn.execSql("MaVLIBizRule.PARTNO_EXIST", sSQL, progressFlag: false);
	}

	public static int UPDATE_IMAGE_DATA(string sPartNo, string path1, string path2, string path3, string path4)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsReturn = new DataSet();
		sSQL = "\nUPDATE[LGE.TV.VISION].[dbo].[IMAGE_BY_PARTNO]\n  SET IMAGE_PATH1     = '[2]',\n         IMAGE_PATH2     = '[3]',\n         IMAGE_PATH3     = '[4]',\n         IMAGE_PATH4     = '[5]',\n        CREATE_DATE       = GETDATE()\n WHERE   PARTNO   = '[1]'";
		sSQL = sSQL.Replace("[1]", sPartNo);
		sSQL = sSQL.Replace("[2]", path1);
		sSQL = sSQL.Replace("[3]", path2);
		sSQL = sSQL.Replace("[4]", path3);
		sSQL = sSQL.Replace("[5]", path4);
		return MsSqlDBConn.execNonDBSql(CServerInfo.ServerType.Prod, "UPDATE_IMAGE_DATA", sSQL, progressFlag: false);
	}

	public static int INSERT_PARTNO_LIST(string sModel, string sItemType, string sPartNo, string sUserName, string sLine)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsReturn = new DataSet();
		sSQL = "\nINSERT INTO [LGE.TV.VISION].[dbo].[PARTNO_LIST_BY_MODEL]\n ([MODEL_SUFFIX], [ITEM_TYPE], [PARTNO]   ,[USERNAME]  ,[LINE]   ,[CREATE_DATE])\n VALUES ( \n        '[1]',        \n        '[2]',\n        '[3]',\n        '[4]',\n        '[5]',\n        GETDATE()  )";
		sSQL = sSQL.Replace("[1]", sModel);
		sSQL = sSQL.Replace("[2]", sItemType);
		sSQL = sSQL.Replace("[3]", sPartNo);
		sSQL = sSQL.Replace("[4]", sUserName);
		sSQL = sSQL.Replace("[5]", sLine);
		return MsSqlDBConn.execNonDBSql(CServerInfo.ServerType.Prod, "INSERT_PARTNO_LIST", sSQL, progressFlag: false);
	}

	public static int DELETE_PARTNO_LIST(string sModel, string sItemType)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsReturn = new DataSet();
		sSQL = "\n        DELETE [LGE.TV.VISION].[dbo].[PARTNO_LIST_BY_MODEL]\n        WHERE MODEL_SUFFIX   = '[1]'\n             AND ITEM_TYPE =   '[2]'  ";
		sSQL = sSQL.Replace("[1]", sModel);
		sSQL = sSQL.Replace("[2]", sItemType);
		return MsSqlDBConn.execNonDBSql(CServerInfo.ServerType.Prod, "DELETE_PARTNO_LIST", sSQL, progressFlag: false);
	}

	public static int INSERT_IMAGE_DATA(string sPartNo, string path1, string path2, string path3, string path4)
	{
		string sSQL = string.Empty;
		string sConn = string.Empty;
		DataSet dsReturn = new DataSet();
		sSQL = "\nINSERT INTO [LGE.TV.VISION].[dbo].[IMAGE_BY_PARTNO]\n ([PARTNO]    ,[IMAGE_PATH1]   ,[IMAGE_PATH2]   ,[IMAGE_PATH3]   ,[IMAGE_PATH4]   ,[CREATE_DATE])\n VALUES ( \n        '[1]',        \n        '[2]',\n        '[3]',\n        '[4]',\n        '[5]',\n        GETDATE()  )";
		sSQL = sSQL.Replace("[1]", sPartNo);
		sSQL = sSQL.Replace("[2]", path1);
		sSQL = sSQL.Replace("[3]", path2);
		sSQL = sSQL.Replace("[4]", path3);
		sSQL = sSQL.Replace("[5]", path4);
		return MsSqlDBConn.execNonDBSql(CServerInfo.ServerType.Prod, "INSERT_IMAGE_DATA", sSQL, progressFlag: false);
	}
}
