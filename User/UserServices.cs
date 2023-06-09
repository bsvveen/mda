
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
        private readonly IUserSql _userSql;

        public UserServices(Primitive model, IUserSql userSql)
        {         
            _model = model;
            _userSql = userSql;
        }        

        public async Task<object> List(ListRequest request)
        {
            if (request.Properties.Count == 0)
            {
                Entity entity = _model.Entities.Single(e => e.Name == request.EntityName);
                entity.Properties.ForEach(p => request.Properties.Add(p.Key));
            }             

            string stringResponse = await _userSql.List(request);
            return JsonSerializer.Deserialize<object>(stringResponse);
        }

        public async Task<object> GetById(GetByIdRequest request)
        {
            if (request.Properties.Count == 0) {
                Entity entity = _model.Entities.Single(e => e.Name == request.EntityName);
                entity.Properties.ForEach(p => request.Properties.Add(p.Key));
            }

            string stringResponse = await _userSql.GetById(request);
            return JsonSerializer.Deserialize<object>(stringResponse);
        }

        public async Task Submit(SubmitRequest request)
        {
            await _userSql.Submit(request);
        }
    }    
}