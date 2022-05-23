using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System.Data;
using System.Reflection;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using MDA.Primitive.Database;
using MDA.Primitive;
using MDA.Infrastructure;

namespace MDA.Model
{
    public class ModelHandler
    {
        private readonly PrimitiveHandler ph;     
       
        public ModelHandler(ISql sql)
        {
            ph = new PrimitiveHandler(sql);
        }

        //JSchema schema = new JSchema
        //{
        //    Type = JSchemaType.Object,
        //    Properties =
        //    {
        //        { "name", new JSchema { Type = JSchemaType.String } },
        //        {
        //            "hobbies", new JSchema
        //            {
        //                Type = JSchemaType.Array,
        //                Items = { new JSchema { Type = JSchemaType.String } }
        //            }
        //        }t
        //    }
        //};

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
    }    
}