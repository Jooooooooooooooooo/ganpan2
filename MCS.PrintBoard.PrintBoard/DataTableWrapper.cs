using System.Collections;
using System.Data;
using PresentationControls;

namespace MCS.PrintBoard.PrintBoard;

public class DataTableWrapper : ListSelectionWrapper<DataRow>
{
	public DataTableWrapper(DataTable dataTable, string usePropertyAsDisplayName)
		: base((IEnumerable)dataTable.Rows, showCounts: false, usePropertyAsDisplayName)
	{
	}
}
