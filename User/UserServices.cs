
using Newtonsoft.Json.Schema;
using MDA.Infrastructure;
using MDA.Admin;

namespace MDA.User
{
    public class UserServices
    {
        private readonly Primitive model;   

        public UserServices()
        {         
            model = new AdminServices().Model;
        }

        public Primitive Model
        {
            get
            {  
                return model;
            }
        }

        public JSchema ModelJSchema
        {
            get
            {
                JSchema RootjSchema = new JSchema();
                RootjSchema.Type = JSchemaType.Object;
                
                model.Entities.ToList().ForEach(entity =>
                {
                    JSchema jSchema = new JSchema();
                    jSchema.Type = JSchemaType.Object;

                    entity.Properties.ToList().ForEach(prop =>
                    {                       
                        jSchema.Properties.Add(prop.Name, new JSchema { Type = prop.Type.ToJSchemaType() });                   
                    });

                    RootjSchema.Properties.Add(entity.Name, jSchema);
                });

                return RootjSchema;
            }
        }

        public bool validateRequest(ListRequest request)
        {  
            var entity = model.Entities.SingleOrDefault(tbl => tbl.Name == request.Entity);

            if (entity != null)  {
                var AllPropertiesExists = (request.Properties != null) && request.Properties.All(regProp => entity.Properties.Any(entProp => entProp.Name.Equals(regProp.Name)));
                var FilterPropertiesExists = (request.Filter != null) && entity.Properties.Exists(entProp => entProp.Name.Equals(request.Filter.Property));
            
                return AllPropertiesExists && FilterPropertiesExists;
            
            } else { return false;  }                
        }

        public async Task<string> List(ListRequest request)
        {
            return await new UserSql().List(request);           
        }       
    }    
}