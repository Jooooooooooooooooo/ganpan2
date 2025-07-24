using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MCS.PrintBoard.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
internal class Resources
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				ResourceManager temp = new ResourceManager("MCS.PrintBoard.Properties.Resources", typeof(Resources).Assembly);
				resourceMan = temp;
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	internal static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	internal static Bitmap background
	{
		get
		{
			object obj = ResourceManager.GetObject("background", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap bbtnbg
	{
		get
		{
			object obj = ResourceManager.GetObject("bbtnbg", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap bbtnbg2
	{
		get
		{
			object obj = ResourceManager.GetObject("bbtnbg2", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap btnbg
	{
		get
		{
			object obj = ResourceManager.GetObject("btnbg", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap ledHigh
	{
		get
		{
			object obj = ResourceManager.GetObject("ledHigh", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap ledLow
	{
		get
		{
			object obj = ResourceManager.GetObject("ledLow", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static string LengthMS => ResourceManager.GetString("LengthMS", resourceCulture);

	internal static string LengthPN => ResourceManager.GetString("LengthPN", resourceCulture);

	internal static string LengthWO => ResourceManager.GetString("LengthWO", resourceCulture);

	internal static Bitmap light_green
	{
		get
		{
			object obj = ResourceManager.GetObject("light_green", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap light_green2
	{
		get
		{
			object obj = ResourceManager.GetObject("light_green2", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap light_red
	{
		get
		{
			object obj = ResourceManager.GetObject("light_red", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap light_red2
	{
		get
		{
			object obj = ResourceManager.GetObject("light_red2", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static string MDB_BOM_CreateTable => ResourceManager.GetString("MDB_BOM_CreateTable", resourceCulture);

	internal static string MDB_BOM_FileName => ResourceManager.GetString("MDB_BOM_FileName", resourceCulture);

	internal static string MDB_BOM_Select => ResourceManager.GetString("MDB_BOM_Select", resourceCulture);

	internal static string MDB_BOM_TableName => ResourceManager.GetString("MDB_BOM_TableName", resourceCulture);

	internal static string MDB_Buffer_CreateTable => ResourceManager.GetString("MDB_Buffer_CreateTable", resourceCulture);

	internal static string MDB_Buffer_FileName => ResourceManager.GetString("MDB_Buffer_FileName", resourceCulture);

	internal static string MDB_Buffer_Select => ResourceManager.GetString("MDB_Buffer_Select", resourceCulture);

	internal static string MDB_Buffer_TableName => ResourceManager.GetString("MDB_Buffer_TableName", resourceCulture);

	internal static string MDB_ConnectionString => ResourceManager.GetString("MDB_ConnectionString", resourceCulture);

	internal static string MDB_DefaultTable_Create => ResourceManager.GetString("MDB_DefaultTable_Create", resourceCulture);

	internal static string MDB_Model_CreateTable => ResourceManager.GetString("MDB_Model_CreateTable", resourceCulture);

	internal static string MDB_Model_TableName => ResourceManager.GetString("MDB_Model_TableName", resourceCulture);

	internal static string MDB_MTRL_FileName => ResourceManager.GetString("MDB_MTRL_FileName", resourceCulture);

	internal static string MDB_MTRL_LOTInfo_Insert => ResourceManager.GetString("MDB_MTRL_LOTInfo_Insert", resourceCulture);

	internal static string MDB_MTRL_LOTInfo_Select_With_PN => ResourceManager.GetString("MDB_MTRL_LOTInfo_Select_With_PN", resourceCulture);

	internal static string MDB_MTRL_LOTInfo_TableName => ResourceManager.GetString("MDB_MTRL_LOTInfo_TableName", resourceCulture);

	internal static string MDB_MTRL_LOTInfo_Update => ResourceManager.GetString("MDB_MTRL_LOTInfo_Update", resourceCulture);

	internal static string MDB_MTRL_LOTInfo_Update_PartNO => ResourceManager.GetString("MDB_MTRL_LOTInfo_Update_PartNO", resourceCulture);

	internal static string MDB_MTRL_LOTInfoTable_Create => ResourceManager.GetString("MDB_MTRL_LOTInfoTable_Create", resourceCulture);

	internal static string MDB_MTRL_Model_TableName => ResourceManager.GetString("MDB_MTRL_Model_TableName", resourceCulture);

	internal static string MDB_MTRL_ModelSuffixTable_Create => ResourceManager.GetString("MDB_MTRL_ModelSuffixTable_Create", resourceCulture);

	internal static string MDB_MTRL_ModelSuffixTable_Delete => ResourceManager.GetString("MDB_MTRL_ModelSuffixTable_Delete", resourceCulture);

	internal static string MDB_MTRL_ModelSuffixTable_Insert => ResourceManager.GetString("MDB_MTRL_ModelSuffixTable_Insert", resourceCulture);

	internal static string MDB_MTRL_ModelSuffixTable_Select => ResourceManager.GetString("MDB_MTRL_ModelSuffixTable_Select", resourceCulture);

	internal static string MDB_MTRL_ModelSuffixTable_Select_With_ModelName => ResourceManager.GetString("MDB_MTRL_ModelSuffixTable_Select_With_ModelName", resourceCulture);

	internal static string MDB_MTRL_ModelSuffixTable_Update_PartNO => ResourceManager.GetString("MDB_MTRL_ModelSuffixTable_Update_PartNO", resourceCulture);

	internal static string MDB_PARTDB_FILE_NAME => ResourceManager.GetString("MDB_PARTDB_FILE_NAME", resourceCulture);

	internal static string MDB_PARTInfo_Create => ResourceManager.GetString("MDB_PARTInfo_Create", resourceCulture);

	internal static string MDB_PartInfo_Insert => ResourceManager.GetString("MDB_PartInfo_Insert", resourceCulture);

	internal static string MDB_PartInfo_Select_With_FileName => ResourceManager.GetString("MDB_PartInfo_Select_With_FileName", resourceCulture);

	internal static string MDB_PartInfo_Select_With_PartID => ResourceManager.GetString("MDB_PartInfo_Select_With_PartID", resourceCulture);

	internal static string MDB_PartInfo_TableName => ResourceManager.GetString("MDB_PartInfo_TableName", resourceCulture);

	internal static string MDB_PartInfo_Update_With_FileName => ResourceManager.GetString("MDB_PartInfo_Update_With_FileName", resourceCulture);

	internal static string MDB_Plan_CreateTable => ResourceManager.GetString("MDB_Plan_CreateTable", resourceCulture);

	internal static string MDB_Plan_FileName => ResourceManager.GetString("MDB_Plan_FileName", resourceCulture);

	internal static string MDB_Plan_Select => ResourceManager.GetString("MDB_Plan_Select", resourceCulture);

	internal static string MDB_Plan_TableName => ResourceManager.GetString("MDB_Plan_TableName", resourceCulture);

	internal static string MDB_Result_FileName => ResourceManager.GetString("MDB_Result_FileName", resourceCulture);

	internal static string MDB_ResultDB_Insert => ResourceManager.GetString("MDB_ResultDB_Insert", resourceCulture);

	internal static string Oracle_BOM_Select => ResourceManager.GetString("Oracle_BOM_Select", resourceCulture);

	internal static string Oracle_BOM_Select2 => ResourceManager.GetString("Oracle_BOM_Select2", resourceCulture);

	internal static string Oracle_Buffer_Delete => ResourceManager.GetString("Oracle_Buffer_Delete", resourceCulture);

	internal static string Oracle_Buffer_Delete2 => ResourceManager.GetString("Oracle_Buffer_Delete2", resourceCulture);

	internal static string Oracle_Buffer_Select => ResourceManager.GetString("Oracle_Buffer_Select", resourceCulture);

	internal static string Oracle_ConnectGMES => ResourceManager.GetString("Oracle_ConnectGMES", resourceCulture);

	internal static string Oracle_ConnectionString => ResourceManager.GetString("Oracle_ConnectionString", resourceCulture);

	internal static string Oracle_Plan_Select => ResourceManager.GetString("Oracle_Plan_Select", resourceCulture);

	internal static Bitmap PNG_MENU_BTN
	{
		get
		{
			object obj = ResourceManager.GetObject("PNG_MENU_BTN", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static Bitmap sbtnbg
	{
		get
		{
			object obj = ResourceManager.GetObject("sbtnbg", resourceCulture);
			return (Bitmap)obj;
		}
	}

	internal static string ServerIP => ResourceManager.GetString("ServerIP", resourceCulture);

	internal static string ServerPort => ResourceManager.GetString("ServerPort", resourceCulture);

	internal static string Unknown => ResourceManager.GetString("Unknown", resourceCulture);

	internal Resources()
	{
	}
}
