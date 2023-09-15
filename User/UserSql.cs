// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using static MDA.Infrastructure.Primitive;

namespace MDA.User
{
    public class UserSql
    {
        private readonly string _connectionstring;
        private readonly Primitive _model;

        public UserSql(Primitive model)
        {
            _connectionstring = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Database")["connectionstring"]; ;
            _model = model;
        }

        public async Task<string> List(ListRequest request)
        {
            var select_statement = "Id As Id, " + string.Join(',', request.Properties.Select(p => $"{p} As '{p}'"));            

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

            // error when response becomes too long, end of string...
            var sql = $"SELECT {select_statement} FROM {request.EntityName} {query_where} FOR JSON AUTO;";

            return await ExecuteReader(sql);
        }

        public async Task<string> GetById(GetByIdRequest request)
        {
            var sql = $"SELECT * FROM {request.EntityName} ";

            Entity entity = _model.Entities.Single(e => e.Name == request.EntityName);
            List<Property> FKs = entity.Properties.Where(p => p.ForeignKey != null).Select(p => p).ToList();
            if (FKs.Any()) {
                sql += "LEFT JOIN ";
                FKs.ForEach(p =>
                {
                    sql += $"{p.ForeignKey.Relatedentity} ON {request.EntityName}.{p.Key} = {p.ForeignKey.Relatedentity}.Id";
                });
            };     

            sql += $" WHERE {request.EntityName}.Id = '{request.Id}' FOR JSON AUTO, WITHOUT_ARRAY_WRAPPER;";
             
            return await ExecuteReader(sql);
        }       

        public async Task Update(UpdateRequest request)
        {
            var keyscollection = string.Join(',', request.Properties.Select(x => $"{x.Key} = '{x.Value}'"));

            var sql = $"UPDATE {request.EntityName} SET {keyscollection} WHERE Id = '{request.Id}';";
            await ExecuteNonQuery(sql);
        }

        public async Task<Guid> Create(CreateRequest request)
        {
            var keyscollection = string.Join(',', request.Properties.Select(x => $"{x.Key}"));
            var valuescollection = string.Join(',', request.Properties.Select(x => $"'{x.Value}'"));

            var ID = Guid.NewGuid();

            var sql = $"INSERT INTO {request.EntityName} (Id, {keyscollection}) VALUES('{ID}', {valuescollection});";
            await ExecuteNonQuery(sql);

            return ID;
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

                var retValue = new StringBuilder();               
                if (!reader.HasRows)
                {
                    retValue.Append("[]");
                }
                else
                {
                    while (reader.Read())
                    {
                        retValue.Append(reader.GetValue(0).ToString());
                    }
                }

                await reader.CloseAsync();     

                return retValue.ToString();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Error executing: '{sqlCommand}, Message:{sqlEx.Message},{sqlEx.StackTrace}");
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

