// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;
using Microsoft.Data.Sqlite;
using System.Data;

namespace MDA.Primitive.Database
{ 
    public class Sqlite : ISql
    { 
        public async Task<Primitive> GetPrimitive()
        {
            Primitive Primitive = new Primitive();

            Primitive.Tables = new List<Primitive.Table>();

            var tables = await ExecuteDataTable("SELECT * FROM sqlite_master where type='table';");
            foreach (DataRow dr in tables.Rows)
            {
                Primitive.Table table = new Primitive.Table();
                table.Name = (string)dr["Name"];
                var columns = await ExecuteDataTable($"PRAGMA table_info('{table.Name}');");

                foreach (DataRow dr2 in columns.Rows)
                {
                    var column = new Primitive.Column();
                    column.Name = (string)dr2["name"];
                    var columndatatype = dr2["type"];
                    switch (columndatatype)
                    {
                        case "CHAR(255)":
                            column.Type = ColumnDataType.Text;
                            break;
                        case "DateTime":
                            column.Type = ColumnDataType.DateTime;
                            break;
                        case "INT(255)":
                            column.Type = ColumnDataType.Number;
                            break;
                        case "UNIQUEIDENTIFIER":
                            column.Type = ColumnDataType.ID;
                            break;
                    }
                    column.NotNull = (long)dr2["notnull"] != 0;

                    table.Columns.Add(column);
                }
                Primitive.Tables.Add(table);
            }

            return Primitive;
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
                case ColumnDataType.Text:
                    datatype = "INT(255)";
                    break;
                case ColumnDataType.ID:
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

