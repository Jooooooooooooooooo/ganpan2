using System;
using System.Collections.Generic;
using System.Data;

namespace MCS.PrintBoard.BizRule;

public class BizData : EventArgs
{
	private string _EqptID = string.Empty;

	private string _EqptName = string.Empty;

	private string _ProcID = string.Empty;

	private List<string> _ProcIDList = new List<string>();

	private string _LineID = string.Empty;

	private string _SrcType = string.Empty;

	private string _UserID = string.Empty;

	private string _BizName = string.Empty;

	private DataSet _BizDataSet = new DataSet();

	private string _Version = string.Empty;

	private string _Judge = string.Empty;

	private string _PcsgID = string.Empty;

	private string _FileID = string.Empty;

	private string _FileTRFResult = string.Empty;

	private string _ErrorMSG = string.Empty;

	public string EqptID
	{
		get
		{
			return _EqptID;
		}
		set
		{
			_EqptID = value;
		}
	}

	public string EqptName
	{
		get
		{
			return _EqptName;
		}
		set
		{
			_EqptName = value;
		}
	}

	public string ProcID
	{
		get
		{
			return _ProcID;
		}
		set
		{
			_ProcID = value;
		}
	}

	public List<string> ProcIDList
	{
		get
		{
			return _ProcIDList;
		}
		set
		{
			_ProcIDList = value;
		}
	}

	public string LineID
	{
		get
		{
			return _LineID;
		}
		set
		{
			_LineID = value;
		}
	}

	public string SrcType
	{
		get
		{
			return _SrcType;
		}
		set
		{
			_SrcType = value;
		}
	}

	public string UserID
	{
		get
		{
			return _UserID;
		}
		set
		{
			_UserID = value;
		}
	}

	public string BizName
	{
		get
		{
			return _BizName;
		}
		set
		{
			_BizName = value;
		}
	}

	public DataSet BizDataSet
	{
		get
		{
			return _BizDataSet;
		}
		set
		{
			_BizDataSet = value;
		}
	}

	public string Version
	{
		get
		{
			return _Version;
		}
		set
		{
			_Version = value;
		}
	}

	public string Judge
	{
		get
		{
			return _Judge;
		}
		set
		{
			_Judge = value;
		}
	}

	public string PcsgID
	{
		get
		{
			return _PcsgID;
		}
		set
		{
			_PcsgID = value;
		}
	}

	public string FileID
	{
		get
		{
			return _FileID;
		}
		set
		{
			_FileID = value;
		}
	}

	public string FileTRFResult
	{
		get
		{
			return _FileTRFResult;
		}
		set
		{
			_FileTRFResult = value;
		}
	}

	public string ErrorMSG
	{
		get
		{
			return _ErrorMSG;
		}
		set
		{
			_ErrorMSG = value;
		}
	}
}
