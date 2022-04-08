// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/
using MDA.Dbo;
using Microsoft.Data.Sqlite;
using System.Data;

namespace MDA.Infrastructure
{
    public static class Sqlite
    {
        public static async Task DropTable(string TableName)
        {
            await ExecuteSql($"DROP TABLE {TableName};");
        }

        public static async Task<ActionResponse> CreateTable(string TableName)
        {
            return await ExecuteSql($"CREATE TABLE {TableName} (ID uniqueidentifier PRIMARY KEY);");
        }

        public static async Task<ActionResponse> AddColumn(string TableName, string ColumnName, ColumnDataType DataType)
        {
            return await ExecuteSql($"ALTER TABLE {TableName} ADD {ColumnName} {DataType?.Value}");            
        }

        public static async Task<ActionResponse> DropColumn(string TableName, string ColumnName)
        {
            return await ExecuteSql($"ALTER TABLE {TableName} DROP {ColumnName}");
        }

        public static async Task<ActionResponse> ExecuteSql(string sqlCommand)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./Database.db";

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                var Command = connection.CreateCommand();
                Command.CommandText = sqlCommand;
                try
                {
                    await Command.ExecuteNonQueryAsync();
                }
                catch (Exception ex)
                {
                    return new ActionResponse(false, ex.Message);
                }
            }

            return new ActionResponse(true); ;
        }

        public static async Task ExecuteTransAction(string[] sqlCommands)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "./Database.db";

            using (var connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();

                //Seed some data:
                using (var transaction = connection.BeginTransaction())
                {
                    var insertCmd = connection.CreateCommand();

                    foreach (string sqlCommand in sqlCommands)
                    {
                        insertCmd.CommandText = sqlCommand;
                        await insertCmd.ExecuteNonQueryAsync();
                    }

                    transaction.Commit();
                }
            }
        }

        public static async Task<DataTable> ExecuteDataTable(string command)
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

