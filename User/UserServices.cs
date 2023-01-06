
using Newtonsoft.Json.Schema;
using MDA.Infrastructure;
using MDA.Admin;
using static MDA.Infrastructure.Primitive;
using MediatR;

namespace MDA.User
{
    public class UserServices
    {
        private readonly Primitive model;
        private readonly Entity entity;

        public UserServices(string Entity)
        {         
            model = new AdminServices().Model;
            entity = model.Entities.Single(tbl => tbl.Name == Entity);
        }        

        public async Task<string> List(ListRequest request)
        {
            if (request.Properties.Count == 0)                            
                entity.Properties.ForEach(p => request.Properties.Add(p.Key));            

            return await new UserSql().List(request);     
        }

        public async Task<string> GetById(GetByIdRequest request)
        {
            if (request.Properties ==  null || request.Properties.Count == 0)
                entity.Properties.ForEach(p => request.Properties.Add(p.Key));

            return await new UserSql().GetById(request);
        }

        public async Task<int> Submit(SubmitRequest request)
        {
            return await new UserSql().Submit(request);
        }
    }    
}