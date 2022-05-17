// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using Microsoft.Data.Sqlite;
using System.Data;

namespace MDA.Infrastructure
{
    public class Sqlite : ISql
    {
        public async Task<DataTable> GetTables()
        {
            return await ExecuteDataTable("SELECT * FROM sqlite_master where type='table';"); 
        }        

        public async Task<DataTable> GetColumns(string TableName)
        {
            return await ExecuteDataTable($"PRAGMA table_info('{TableName}');"); 
        }       

        public async Task DropTable(string TableName)
        {
            await ExecuteSql($"DROP TABLE {TableName};");
        }

        public async Task CreateTable(string TableName)
        {
            await ExecuteSql($"CREATE TABLE {TableName} (ID uniqueidentifier PRIMARY KEY);");
        }

        public async Task AddColumn(string TableName, string ColumnName, ColumnDataType eDataType, bool notnull)
        {
            string? datatype;
            switch (eDataType)
            {
                case ColumnDataType.DateTime:
                    datatype = "DATETIME";
                    break;
                case ColumnDataType.INT_255:
                    datatype = "INT(255)";
                    break;
                case ColumnDataType.UNIQUEIDENTIFIER:
                    datatype = "uniqueidentifier";
                    break;
                default:
                    datatype = "CHAR(255)";
                    break;
            }

            await ExecuteSql($"ALTER TABLE {TableName} ADD {ColumnName} {datatype}");
        }

        public async Task DropColumn(string TableName, string ColumnName)
        {
            await ExecuteSql($"ALTER TABLE {TableName} DROP {ColumnName}");
        }

        private static async Task ExecuteSql(string sqlCommand)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./Database.db";

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var Command = connection.CreateCommand();
                Command.CommandText = sqlCommand;
                await Command.ExecuteNonQueryAsync();               
            }           
        }        

        private static async Task<DataTable> ExecuteDataTable(string command)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./Database.db";

            var dataTable = new DataTable();

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var Command = connection.CreateCommand();
                Command.CommandText = command;

                var dataReader = await Command.ExecuteReaderAsync();

                dataTable.Load(dataReader);
            }

            return dataTable;
        }
    }        
}

