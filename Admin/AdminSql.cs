// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;
using Microsoft.Data.Sqlite;
using System.Data;

namespace MDA.Admin
{
    public class AdminSql 
    {
        public async Task<Primitive> GetModel()
        {
            Primitive Model = new Primitive();

            Model.Entities = new List<Primitive.Entity>();

            var dbTables = await ExecuteDataTable("SELECT * FROM sqlite_master where type='table';");
            foreach (DataRow dr in dbTables.Rows)
            {
                Primitive.Entity entity = new Primitive.Entity();
                entity.Name = (string)dr["Name"];

                var dbColumns = await ExecuteDataTable($"PRAGMA table_info('{entity.Name}');");
                foreach (DataRow dr2 in dbColumns.Rows)
                {
                    var property = new Primitive.Property();
                    property.Name = (string)dr2["name"];
                    switch (dr2["type"])
                    {
                        case "CHAR(255)":
                            property.Type = PropertyDataType.Text;
                            break;
                        case "DateTime":
                            property.Type = PropertyDataType.DateTime;
                            break;
                        case "INT(255)":
                            property.Type = PropertyDataType.Number;
                            break;
                        case "UNIQUEIDENTIFIER":
                            property.Type = PropertyDataType.ID;
                            break;
                    }
                    property.NotNull = (long)dr2["notnull"] != 0;

                    entity.Properties.Add(property);
                }

                Model.Entities.Add(entity);
            }

            return Model;
        }

        public async Task DropTable(string TableName)
        {
            await ExecuteSql($"DROP TABLE {TableName};");
        }

        public async Task CreateTable(string TableName)
        {
            await ExecuteSql($"CREATE TABLE {TableName} (ID uniqueidentifier PRIMARY KEY);");
        }

        public async Task AddColumn(string TableName, string ColumnName, PropertyDataType eDataType, bool notnull)
        {
            string? datatype;
            switch (eDataType)
            {
                case PropertyDataType.DateTime:
                    datatype = "DATETIME";
                    break;
                case PropertyDataType.Text:
                    datatype = "INT(255)";
                    break;
                case PropertyDataType.ID:
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
            connectionStringBuilder.DataSource = "./Model/Database.db";

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
            connectionStringBuilder.DataSource = "./Model/Database.db";

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

