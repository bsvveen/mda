
using Newtonsoft.Json.Schema;
using MDA.Infrastructure;
using MDA.Admin;
using static MDA.Infrastructure.Primitive;
using MediatR;
using System.Text.Json;

namespace MDA.User
{
    public class UserServices
    {
        private readonly Primitive _model;        

        public UserServices(Primitive model)
        {         
            _model = model;            
        }        

        public async Task<object> List(ListRequest request)
        {
            if (request.Properties.Count == 0)
            {
                Entity entity = _model.Entities.Single(e => e.Name == request.Entity);
                entity.Properties.ForEach(p => request.Properties.Add(p.Key));
            }             

            string stringResponse = await new UserSql().List(request);
            return JsonSerializer.Deserialize<object>(stringResponse);
        }

        public async Task<object> GetById(GetByIdRequest request)
        {
            if (request.Properties.Count == 0) {
                Entity entity = _model.Entities.Single(e => e.Name == request.Entity);
                entity.Properties.ForEach(p => request.Properties.Add(p.Key));
            }

            string stringResponse = await new UserSql().GetById(request);
            return JsonSerializer.Deserialize<object>(stringResponse);
        }

        public async Task<int> Submit(SubmitRequest request)
        {
            return await new UserSql().Submit(request);
        }
    }    
}