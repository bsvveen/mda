
using System.Data;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MDA.Admin
{
    public class AdminServices
    {
        private static Primitive? model;

        private readonly string path = Path.Combine(Environment.CurrentDirectory, @"Model\Model.Json");           

        public Primitive Model
        {
            get {
                model ??= GetModelFromFile();                
                return model; 
            }            
        } 

        public async Task<Primitive> UpdateModel(Primitive newModel)
        {
            SaveModelToFile(newModel);
            await SyncWithDatabase();
            model = newModel;           

            return model;
        }      
        

        private Primitive GetModelFromFile()
        {
            using FileStream openStream = File.OpenRead(path);

            JsonSerializerOptions options = new() { Converters = { new JsonStringEnumConverter() }};           
            Primitive? model = JsonSerializer.Deserialize<Primitive>(openStream, options);           

            if (model == null || model.Entities == null)
                throw new NullReferenceException("Model could not be loaded from file");
                    
            return model;
        }

        private void SaveModelToFile(Primitive newModel)
        {
            using FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, newModel);
            createStream.Dispose();
        }

        public async Task SyncWithDatabase()
        { 
            var Sql = new AdminSql();
            
            Primitive dbPrimitive = await Sql.GetModelFromDb();

            // Drop tables from database NOT in Primitive
            dbPrimitive.Entities.Where(dbt => !Model.Entities.Exists(mt => mt.Name == dbt.Name)).ToList().ForEach(async dbt =>
               await Sql.DropTable(dbt.Name));

            // Update Tables in Primitives AND in Database
            Model.Entities.Where(mt => dbPrimitive.Entities.Exists(dbt => dbt.Name == mt.Name)).ToList().ForEach(mt =>
            {
                var dbt =  dbPrimitive.Entities.Single(dbt => dbt.Name == mt.Name);

                // Drop columns (except ID) from database NOT in Primitive
                dbt.Properties.Where(dbc => !mt.Properties.Exists(c => c.Name == dbc.Name) && dbc.Name != "ID").ToList().ForEach(async dbc =>
                   await Sql.DropColumn(dbt.Name, dbc.Name));

                // Add columns to database in Primitive but NOT in database
                mt.Properties.Where(c => !dbt.Properties.Exists(dbc => dbc.Name == c.Name)).ToList().ForEach(async c =>
                   await Sql.AddColumn(dbt.Name, c.Name, c.Type, c.NotNull));
            });

            // Add tables to database in Primitive but NOT in database
            Model.Entities.Where(t => !dbPrimitive.Entities.Exists(dbt => dbt.Name == t.Name)).ToList().ForEach(async t => {
                await Sql.CreateTable(t.Name);

                // Add columns from Primitive to database 
                t.Properties.ToList().ForEach(async c =>
                   await Sql.AddColumn(t.Name, c.Name, c.Type, c.NotNull));
            });
        }       
    }    
}