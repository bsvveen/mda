// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;
using Microsoft.Data.Sqlite;
using System.Data;

namespace MDA.User
{
    public class UserSql
    {
        public async Task<string> List(ListRequest request)
        {
            if (request.Properties == null || request.Properties.Count == 0)
                throw new ArgumentException("ListRequest does not contain a properties collection.");
            
            var json_object_props = "'ID', ID, " + string.Join(',', request.Properties.Select(x => $"'{x}', {x}"));
            var query_props = "ID, " + string.Join(',', request.Properties.Select(x => $"{x}"));

            var query_where = "";
            if (request.Constrains != null)
            {
                query_where = " WHERE ";
                for (int index = 0; index < request.Constrains.Count; index++)
                {
                    var con = request.Constrains.ElementAt(index);
                    query_where += (index != 0) ? TranslateCO(con.AndOr) : "" + con.Property + TranslatePO(con.Operator) + "'" + con.Value + "'";
                }
            }

            var sql = $"SELECT json_group_array(json_object({json_object_props})) AS json_result FROM(SELECT {query_props} FROM {request.Entity} {query_where});";
            return await ExecuteReader(sql);
        }

        public async Task<string> GetById(GetByIdRequest request)
        {  
            var json_object_props = "'ID', ID, " + string.Join(',', request.Properties.Select(x => $"'{x}', {x}"));
            var query_props = "ID, " + string.Join(',', request.Properties.Select(x => $"{x}"));

            var sql = $"SELECT json_group_array(json_object({json_object_props})) AS json_result FROM(SELECT {query_props} FROM {request.Entity} WHERE ID = '{request.Id})';";
            return await ExecuteReader(sql);
        }        

        public async Task<int> Submit(SubmitRequest request)
        {
            if (request.Properties == null || request.Properties.Count == 0)
                throw new ArgumentException("SubmitRequest does not contain a properties collection.");

            if (request.Id == null)
                return await Insert(request);

            return await Update(request);
        }

        private async Task<int> Update(SubmitRequest request)
        {
            var keyscollection = string.Join(',', request.Properties.Select(x => $"'{x.Key}' = '{x.Value}'"));           

            var sql = $"UPDATE {request.Entity} SET {keyscollection} WHERE 'ID' = '{request.Id}';";
            return await ExecuteNonQuery(sql);
        }

        private async Task<int> Insert(SubmitRequest request)
        {           
            var keyscollection = string.Join(',', request.Properties.Select(x => $"'{x.Key}'"));
            var valuescollection = string.Join(',', request.Properties.Select(x => $"'{x.Value}'"));

            var ID = Guid.NewGuid();

            var sql = $"INSERT INTO {request.Entity} ('ID', {keyscollection}) VALUES('{ID}', {valuescollection});";
            return await ExecuteNonQuery(sql);
        }

        private string TranslatePO(PropertyOperator? @operator)
        {
            switch (@operator)
            {
                case PropertyOperator.NotEquealTo:
                    return " <> ";
                default: return " = ";
            }
        }       

        private string TranslateCO(ConstrainOperator @operator)
        {
            switch (@operator)
            {
                case ConstrainOperator.Or:
                    return " OR ";
                default: return " AND ";
            }
        }

        private async Task<string> ExecuteReader(string sqlCommand)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "Model/Database.db";

            using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            var Command = connection.CreateCommand();
            Command.CommandText = sqlCommand;

            var reader = await Command.ExecuteReaderAsync();
            reader.Read();

            var retValue = reader["json_result"].ToString();
            reader.Close();

            connection.Close();

            if (retValue == null)
                throw new Exception($"{sqlCommand} did not return any result from the database");

            return retValue;
        }

        private async Task<int> ExecuteNonQuery(string sqlCommand)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = "Model/Database.db";

            using var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();

            var Command = connection.CreateCommand();
            Command.CommandText = sqlCommand;

            int updated  = await Command.ExecuteNonQueryAsync();     

            connection.Close();

            if (updated != 1)
                throw new Exception($"{sqlCommand} did not return any result from the database");

            return updated;
        }
    }
}

