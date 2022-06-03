// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;
using Microsoft.Data.Sqlite;
using System.Data;

namespace MDA.User
{
    public class UserSql
    {
        public async Task<string> List(GetRequest request)
        {            
            var propsql = "'ID', quote(ID), " + string.Join(',', request.Properties.Select(x => $"'{x}', {x}"));
            var sql = $"SELECT json_group_array(json_object({propsql})) AS json_result FROM(SELECT * FROM {request.Entity});";

            return await ExecuteReader(sql);
        }

        private async Task<string> ExecuteReader(string sqlCommand)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "../Model/Database.db";

            using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            var Command = connection.CreateCommand();
            Command.CommandText = sqlCommand;

            var reader = await Command.ExecuteReaderAsync();
            reader.Read();
            return reader["json_result"].ToString();
        }
    }
}

