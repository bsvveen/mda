using System.ComponentModel;
using System.Data;
using System.Text.Json;

namespace MDA.Infrastructure
{
    public static class Extentions
    {
        public static string ToJSON(this DataTable table)
        {           
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return JsonSerializer.Serialize(parentRow);
        }       
    }
}
