// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;
using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic;
using System.Data;
using System.Security.Cryptography.Xml;

namespace MDA.Admin
{
    public class AdminSql
    {
        private readonly string _connectionstring;

        public AdminSql()
        {
            _connectionstring = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Database")["connectionstring"]; ;
        }       

        private Primitive.Property Reader2Property(SqlDataReader DataReader)
        {
            var property = new Primitive.Property();

            property.Name = (string)DataReader["COLUMN_NAME"];
            switch (DataReader["DATA_TYPE"])
            {
                case "CHAR(255)":
                    property.Type = PropertyDataType.text;
                    break;
                case "DateTime":
                    property.Type = PropertyDataType.datetime;
                    break;
                case "INTEGER":
                    property.Type = PropertyDataType.number;
                    break;
                case "UNIQUEIDENTIFIER":
                    property.Type = PropertyDataType.id;
                    break;
            }
            property.NotNull = (long)DataReader["notnull"] != 0;

            return property;
        }

        public async Task<Primitive> GetModelFromDb()
        {
            Primitive Model = new Primitive();
            Model.Entities = new List<Primitive.Entity>();            

            var tableNames = await ExecuteReader<string>("SELECT table_name FROM INFORMATION_SCHEMA.TABLES;", (DbReader) => DbReader[0].ToString());
            foreach (string tableName in tableNames)
            {
                Primitive.Entity entity = new Primitive.Entity();
                entity.Name = tableName;

                var sql = $"SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}';";
                var properties = await ExecuteReader(sql, Reader2Property);
                entity.Properties = properties;

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
            await ExecuteSql($"CREATE TABLE {TableName} (Id uniqueidentifier PRIMARY KEY);");
        }

        public async Task AddColumn(string TableName, string ColumnName, PropertyDataType eDataType, bool notnull)
        {
            string? datatype;
            switch (eDataType)
            {
                case PropertyDataType.datetime:
                    datatype = "DATETIME";
                    break;
                case PropertyDataType.number:
                    datatype = "INTEGER";
                    break;
                case PropertyDataType.id:
                    datatype = "uniqueidentifier";
                    break;
                case PropertyDataType.foreignkey:
                    datatype = "uniqueidentifier";
                    break;
                default:
                    datatype = "CHAR(255)";
                    break;
            }

            var not_null = (notnull) ? "NOT NULL" : "";
            await ExecuteSql($"ALTER TABLE {TableName} ADD {ColumnName} {datatype} {not_null}");
        }

        public async Task AddForeignKey(string TableName, string ColumnName, string RelatedTable)
        {
            string sql = $"ALTER TABLE {TableName} ADD CONSTRAINT FK_{TableName}_{RelatedTable} FOREIGN KEY({ColumnName}) REFERENCES {RelatedTable}(Id) ON DELETE CASCADE ON UPDATE CASCADE";
                      
            await ExecuteSql(sql);
        }
        

        public async Task DropColumn(string TableName, string ColumnName)
        {
            await ExecuteSql($"ALTER TABLE {TableName} DROP {ColumnName}");
        }

        private async Task ExecuteSql(string sqlCommand)
        {
            using var connection = new SqlConnection(_connectionstring);
            using var command = new SqlCommand(sqlCommand, connection);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();               
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)                
                    await connection.CloseAsync();                
            }
        }

        private async Task<List<T>> ExecuteReader<T>(string sqlCommand, Func<SqlDataReader, T> map) 
        {
            using var connection = new SqlConnection(_connectionstring);
            using var command = new SqlCommand(sqlCommand, connection);

            try
            {
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                List<T> items = new List<T>();
                if (reader.HasRows)
                {
                    items = new List<T>();
                    while (await reader.ReadAsync())
                    {                        
                        T item = map(reader);
                        items.Add(item);
                    }
                }
                await reader.CloseAsync();                
                return items;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)                
                    await connection.CloseAsync();               
            }
        }
    }
}

