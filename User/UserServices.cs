
using Newtonsoft.Json.Schema;
using MDA.Infrastructure;
using MDA.Admin;

namespace MDA.User
{
    public class UserServices
    {
        private readonly AdminServices ph;     
       
        public UserServices()
        {
            ph = new AdminServices();
        }       

        public JSchema ModelJSchema
        {
            get
            {
                JSchema RootjSchema = new JSchema();
                RootjSchema.Type = JSchemaType.Object;

                var model = ph.Model;
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
            var model = ph.Model;

            var entity = model.Entities.SingleOrDefault(tbl => tbl.Name == request.Entity);

            if (entity != null)
            {
                var AllPropertiesExists = request.Properties.All(a => entity.Properties.Any(b => b.Name.Equals(a)));
                var FilterPropertiesExists = entity.Properties.Exists(prop => prop.Name.Equals(request.Filter.Property));

                return AllPropertiesExists && FilterPropertiesExists;

            } else { return false;  }    
        }

        public async Task<string> List(ListRequest request)
        {
            return await new UserSql().List(request);           
        }       
    }    
}