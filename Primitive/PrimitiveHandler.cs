using MDA.Dbo;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System.Data;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using static MDA.Dbo.Primitive;

namespace MDA.Infrastructure
{
    public class PrimitiveHandler
    {
        public ISql Sql { get; }       

        public PrimitiveHandler(ISql sql)
        {
            Sql = sql ?? throw new ArgumentNullException(nameof(sql));
        }

        private static Primitive primitive;

        private readonly string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Model\Primitive.Json");

        public Primitive Primitive
        {
            get {
                if (primitive == null) {
                    primitive = GetFromFile();
                }
                
                return primitive; 
            }            
        }   

        public JSchema AsJSchema
        {
            get
            {
                JSchemaGenerator generator = new JSchemaGenerator();
                return generator.Generate(typeof(Primitive));
            }
        }        

        public async Task<Primitive> UpdatePrimitive(Primitive newPrimitive)
        {
            JSchema schema = this.AsJSchema;
            var primitiveJSON = JsonSerializer.Serialize(newPrimitive);
            JObject newModelAsJson = JObject.Parse(primitiveJSON);

            IList<string> errors;
            bool valid = newModelAsJson.IsValid(schema, out errors);

            if (valid)
            {
                SaveToFile(newPrimitive);
                await SyncWithDatabase();
                primitive = newPrimitive;
            } else
            {
                throw new Exception(string.Join(",", errors));
            }

            return primitive;
        }      
        

        private Primitive GetFromFile()
        {            
            using FileStream openStream = File.OpenRead(path);
            Primitive? primitive = JsonSerializer.Deserialize<Primitive>(openStream);
            return primitive;
        }

        private async void SaveToFile(Primitive newPrimitive)
        {
            using FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, newPrimitive);
            createStream.Dispose();            
        }

        private async Task SyncWithDatabase()
        {
            Primitive dbPrimitive = await GetPrimitiveFromDatabase();

            // Drop tables from database NOT in Primitive
            dbPrimitive.Tables.Where(dbt => !Primitive.Tables.Exists(mt => mt.Name == dbt.Name)).ToList().ForEach(async dbt =>
               await Sql.DropTable(dbt.Name)); 

            // Update Tables in Primitives AND in Database
            Primitive.Tables.Where(mt => dbPrimitive.Tables.Exists(dbt => dbt.Name == mt.Name)).ToList().ForEach(async mt =>
            {
                var dbt = dbPrimitive.Tables.Single(dbt => dbt.Name == mt.Name);

                // Drop columns (except ID) from database NOT in Primitive
                dbt.Columns.Where(dbc => !mt.Columns.Exists(c => c.Name == dbc.Name)).ToList().ForEach(async dbc =>
                   await Sql.DropColumn(dbt.Name, dbc.Name));

                // Add columns to database in Primitive but NOT in database
                mt.Columns.Where(c => !dbt.Columns.Exists(dbc => dbc.Name == c.Name)).ToList().ForEach(async c =>
                   await Sql.AddColumn(dbt.Name, c.Name, c.Type, c.NotNull));
            });

            // Add tables to database in Primitive but NOT in database
            Primitive.Tables.Where(t => !dbPrimitive.Tables.Exists(dbt => dbt.Name == t.Name)).ToList().ForEach(async t => {
                await Sql.CreateTable(t.Name);

                // Add columns from Primitive to database 
                t.Columns.ToList().ForEach(async c =>
                   await Sql.AddColumn(t.Name, c.Name, c.Type, c.NotNull));
            });
        }

        private async Task<Primitive> GetPrimitiveFromDatabase()
        {
            Primitive DbPrimitive = new Primitive();

            DbPrimitive.Tables = new List<Table>();

            var tables = await Sql.GetTables();
            foreach (DataRow dr in tables.Rows)
            {
                Table table = new Table();
                table.Name = (string)dr["Name"];
                var columns = await Sql.GetColumns(table.Name);

                foreach (DataRow dr2 in columns.Rows)
                {
                    var column = new Column();                   
                    column.Name = (string)dr2["name"];
                    var columndatatype = dr2["type"];
                    switch (columndatatype)
                    {
                        case "CHAR(255)":
                            column.Type = ColumnDataType.CHAR_255;
                            break;
                        case "DateTime":
                            column.Type = ColumnDataType.DateTime;
                            break;
                        case "INT(255)":
                            column.Type = ColumnDataType.INT_255;
                            break;
                        case "UNIQUEIDENTIFIER":
                            column.Type = ColumnDataType.UNIQUEIDENTIFIER;
                            break;
                    }
                    column.NotNull = (bool)dr2["notnull"];
                    
                    table.Columns.Add(column);                    
                }
                DbPrimitive.Tables.Add(table);
            }

            return DbPrimitive;
        }

       

    }    
}