using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System.Data;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using MDA.Primitive.Database;
using MDA.Primitive;
using MDA.Infrastructure;
using System.Linq;
using MDA.Model.Database;

namespace MDA.Model
{
    public class ModelHandler
    {
        private readonly PrimitiveHandler ph;     
       
        public ModelHandler(ISql sql)
        {
            ph = new PrimitiveHandler(sql);
        }       

        public JSchema AsJSchema
        {
            get
            {
                JSchema RootjSchema = new JSchema();
                RootjSchema.Type = JSchemaType.Object;

                var model = ph.Primitive;
                model.Tables.ToList().ForEach(tbl =>
                {
                    JSchema jSchema = new JSchema();
                    jSchema.Type = JSchemaType.Object;

                    tbl.Columns.ToList().ForEach(col =>
                    {                       
                        jSchema.Properties.Add(col.Name, new JSchema { Type = col.Type.ToJSchemaType() });                   
                    });

                    RootjSchema.Properties.Add(tbl.Name, jSchema);
                });

                return RootjSchema;
            }
        }

        public bool validateRequest(GetRequest request)
        {
            var model = ph.Primitive;

            var entity = model.Tables.SingleOrDefault(tbl => tbl.Name == request.Entity);

            if (entity != null)
            {
                var AllPropertiesExists = request.Properties.All(a => entity.Columns.Any(b => b.Name.Equals(a)));
                var FilterPropertiesExists = entity.Columns.Exists(prop => prop.Name.Equals(request.Filter.Property));

                return AllPropertiesExists && FilterPropertiesExists;

            } else { return false;  }    
        }

        public async Task<string> Get(GetRequest request)
        {
            var requestIsValid = validateRequest(request);

            if (requestIsValid)
            {
                return await new SqliteModel().List(request);
            }

            throw new Exception();
        }
    }    
}