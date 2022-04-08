using MDA.Dbo;
using System.Data;
using System.Reflection;
using System.Text.Json;
using static MDA.Dbo.Model;

namespace MDA.Infrastructure
{
    public class ModelHandler
    {
        private static Model model;

        public Model Model
        {
            get {
                if (model == null) {
                    model = GetFromFile();
                }
                
                return model; 
            }
            set {
                OnModelChange();
                model = value;               
            }
        }        

        private async void OnModelChange()
        {
            SaveToFile();
            await SyncWithDatabase();
        }

        private readonly string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"Model\Model.Json");

        private Model GetFromFile()
        {            
            using FileStream openStream = File.OpenRead(path);
            Model? model = JsonSerializer.Deserialize<Model>(openStream);
            return model;
        }

        private async void SaveToFile()
        {
            using FileStream createStream = File.Create(path);
            JsonSerializer.Serialize(createStream, model);
            createStream.Dispose();            
        }

        private async Task SyncWithDatabase()
        {
            Model dbModel = await GetModelFromDatabase();

            // Drop tables from database NOT in model
            dbModel.Tables.Where(dbt => !Model.Tables.Exists(t => t.Name == dbt.Name)).ToList().ForEach(async dbt =>
               await Sqlite.DropTable(dbt.Name));          

            // Add tables to database in model but NOT in database
            Model.Tables.Where(t => !dbModel.Tables.Exists(dbt => dbt.Name == t.Name)).ToList().ForEach(async t =>
               await Sqlite.CreateTable(t.Name));

            Model.Tables.ToList().ForEach(async t =>
            {
                var dbt = dbModel.Tables.Single(dbt => dbt.Name == t.Name);

                // Drop columns from database NOT in model
                dbt.Columns.Where(dbc => !t.Columns.Exists(c => c.Name == dbc.Name)).ToList().ForEach(async dbc =>
                   await Sqlite.DropColumn(dbt.Name, dbc.Name));

                // Add columns to database in model but NOT in database
                t.Columns.Where(c => !dbt.Columns.Exists(dbc => dbc.Name == c.Name)).ToList().ForEach(async c =>
                   await Sqlite.AddColumn(dbt.Name, c.Name, c.Type));
            });
        }

        private static async Task<Model> GetModelFromDatabase()
        {
            Model DbModel = new Model();

            DbModel.Tables = new List<Model.Table>();

            var tables = await Sqlite.ExecuteDataTable("SELECT * FROM sqlite_master where type='table';");
            foreach (DataRow dr in tables.Rows)
            {
                Table table = new Table();
                table.Name = (string)dr["Name"];
                var columns = await Sqlite.ExecuteDataTable($"PRAGMA table_info('{table.Name}');");
                foreach (DataRow dr2 in columns.Rows)
                {
                    var column = new Column();
                    column.Cid = (int)dr2["cid"];
                    column.Name = (string)dr2["cid"];
                    column.Type = (ColumnDataType)dr2["type"];
                    column.Constrain = (string)dr2["constrain"];
                }
                DbModel.Tables.Add(table);
            }

            return DbModel;
        }
    }
}