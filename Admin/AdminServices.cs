
using System.Data;
using MDA.Infrastructure;

namespace MDA.Admin
{
    public class AdminServices
    {    
        public async Task SyncWithDatabase()
        {
            var db = new AdminSql();
            
            Primitive dbPrimitive = await db.GetModelFromDb();
            Primitive storedPrimitive = new ModelServices().Model;

            // Drop tables from database NOT in Primitive
            dbPrimitive.Entities.Where(dbt => !storedPrimitive.Entities.Exists(mt => mt.Name == dbt.Name)).ToList().ForEach(async dbt =>
               await db.DropTable(dbt.Name));

            // Update Tables in Primitives AND in Database
            storedPrimitive.Entities.Where(mt => dbPrimitive.Entities.Exists(dbt => dbt.Name == mt.Name)).ToList().ForEach(mt =>
            {
                var dbt =  dbPrimitive.Entities.Single(dbt => dbt.Name == mt.Name);

                // Drop columns (except ID) from database NOT in Primitive
                dbt.Properties.Where(dbc => !mt.Properties.Exists(c => c.Name == dbc.Name) && dbc.Name != "ID").ToList().ForEach(async dbc =>
                   await db.DropColumn(dbt.Name, dbc.Name));

                // Add columns to database in Primitive but NOT in database
                mt.Properties.Where(c => !dbt.Properties.Exists(dbc => dbc.Name == c.Name)).ToList().ForEach(async c =>
                   await db.AddColumn(dbt.Name, c.Name, c.Type, c.NotNull));
            });

            // Add tables to database in Primitive but NOT in database
            storedPrimitive.Entities.Where(t => !dbPrimitive.Entities.Exists(dbt => dbt.Name == t.Name)).ToList().ForEach(async t => {
                await db.CreateTable(t.Name);

                // Add columns from Primitive to database 
                t.Properties.ToList().ForEach(async c =>
                   await db.AddColumn(t.Name, c.Name, c.Type, c.NotNull));

                // Add Foreighn Keys
                t.Properties.ToList().Where(p => p.Type == PropertyDataType.foreignkey).ToList().ForEach(async c =>
                  await db.AddForeignKey(t.Name, c.Name, c.ForeignKey.Relatedentity));
            });
        }       
    }    
}