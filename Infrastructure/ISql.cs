using System.Data;

namespace MDA.Infrastructure
{
    public interface ISql
    {
        Task AddColumn(string TableName, string ColumnName, ColumnDataType eDataType, bool notnull);

        Task CreateTable(string TableName);

        Task DropColumn(string TableName, string ColumnName);

        Task DropTable(string TableName);

        Task<DataTable> GetColumns(string TableName);

        Task<DataTable> GetTables();
    }
}