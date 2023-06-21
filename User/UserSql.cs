// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MDA.User
{
    public class UserSql
    {
        private readonly string _connectionstring;

        public UserSql()
        {
            _connectionstring = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Database")["connectionstring"]; ;
        }

        public async Task<string> List(ListRequest request)
        {
            var select_statement = "'Id' As Id, " + string.Join(',', request.Properties.Select(x => $"'{x}' AS {x}"));            

            var query_where = "";
            if (request.Constrains != null && request.Constrains.Count != 0)
            {
                query_where = " WHERE ";
                for (int index = 0; index < request.Constrains.Count; index++)
                {
                    var con = request.Constrains.ElementAt(index);
                    query_where += (index != 0) ? TranslateCO(con.AndOr) : "" + con.Property + TranslatePO(con.Operator) + "'" + con.Value + "'";
                }
            }

            var sql = $"SELECT {select_statement} FROM {request.EntityName} {query_where} FOR JSON AUTO;";

            return await ExecuteReader(sql);
        }

        public async Task<string> GetById(GetByIdRequest request)
        {
            var select_statement = "'Id', " + string.Join(',', request.Properties.Select(x => $"'{x}'"));

            var sql = $"SELECT {select_statement} FROM {request.EntityName} WHERE ID = '{request.Id}') FOR JSON AUTO;";

            return await ExecuteReader(sql);
        }

        public async Task Submit(SubmitRequest request)
        {
            if (request.Properties == null || request.Properties.Count == 0)
                throw new ArgumentException("SubmitRequest does not contain a properties collection.");

            if (request.Id == null)
                await Insert(request);

            await Update(request);
        }

        private async Task Update(SubmitRequest request)
        {
            var keyscollection = string.Join(',', request.Properties.Select(x => $"'{x.Key}' = '{x.Value}'"));

            var sql = $"UPDATE {request.EntityName} SET {keyscollection} WHERE 'ID' = '{request.Id}';";
            await ExecuteNonQuery(sql);
        }

        private async Task Insert(SubmitRequest request)
        {
            var keyscollection = string.Join(',', request.Properties.Select(x => $"'{x.Key}'"));
            var valuescollection = string.Join(',', request.Properties.Select(x => $"'{x.Value}'"));

            var ID = Guid.NewGuid();

            var sql = $"INSERT INTO {request.EntityName} ('ID', {keyscollection}) VALUES('{ID}', {valuescollection});";
            await ExecuteNonQuery(sql);
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
            using var connection = new SqlConnection(_connectionstring);
            using var command = new SqlCommand(sqlCommand, connection);

            try
            {
                await connection.OpenAsync();
                using var reader = await command.ExecuteReaderAsync();
                reader.Read();

                var retValue = reader["json_result"].ToString();
               
                await reader.CloseAsync();               

                if (retValue == null)
                    throw new Exception($"{sqlCommand} did not return any result from the database");

                return retValue;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing: '{sqlCommand}, Message:{ex.Message},{ex.StackTrace}");
            }
            finally 
            {
                if (connection.State != ConnectionState.Closed)
                    await connection.CloseAsync();                
            }
        }       

        private async Task ExecuteNonQuery(string sqlCommand)
        {
            using var connection = new SqlConnection(_connectionstring);
            using var command = new SqlCommand(sqlCommand, connection);

            try
            {
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();               
            }
            catch (Exception ex)
            {
                throw new Exception($"Error executing: '{sqlCommand}, Message:{ex.Message},{ex.StackTrace}");
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    await connection.CloseAsync();
            }
        }
    }
}

