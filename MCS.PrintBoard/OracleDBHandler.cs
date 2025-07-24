using System.Data;

namespace MCS.PrintBoard;

internal class OracleDBHandler
{
	public DataTable QuerySelect(string strQuery)
	{
		return null;
	}

	public bool QueryInsert(string strinQuery)
	{
		return true;
	}

	public bool BufferDelete(string firstfieldName, string firstValue, string SecFieldName, string secValue)
	{
		return true;
	}
}
