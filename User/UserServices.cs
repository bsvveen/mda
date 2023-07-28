
using MDA.Infrastructure;
using static MDA.Infrastructure.Primitive;
using System.Text.Json;

namespace MDA.User
{
    public class UserServices
    {
        private readonly Primitive _model;
        UserSql _db;

        public UserServices(Primitive model)
        {         
            _model = model;
            _db = new UserSql();
        }        

        public async Task<object> List(ListRequest request)
        {
            if (request.Properties.Count == 0)
            {
                Entity entity = _model.Entities.Single(e => e.Name == request.EntityName);
                entity.Properties.ForEach(p => request.Properties.Add(p.Key));
            }             

            string stringResponse = await _db.List(request);

            if (stringResponse == null || stringResponse == string.Empty) { stringResponse = "[]"; }

            return JsonSerializer.Deserialize<object>(stringResponse);
        }

        public async Task<object> GetById(GetByIdRequest request)
        {
            if (request.Properties.Count == 0) {
                Entity entity = _model.Entities.Single(e => e.Name == request.EntityName);
                entity.Properties.ForEach(p => request.Properties.Add(p.Key));
            }

            string stringResponse = await _db.GetById(request);

            if (stringResponse == null || stringResponse == string.Empty) { stringResponse = "{}"; }

            return JsonSerializer.Deserialize<object>(stringResponse);
        }

        public async Task<object> Create(CreateRequest request)
        {
            Guid newID = await _db.Create(request);
            if (newID == Guid.Empty)
                throw new Exception($"Error creating new {request.EntityName}");

            return await GetById(new GetByIdRequest { EntityName = request.EntityName, Id = newID });
        }

        public async Task Update(UpdateRequest request)
        {
            await _db.Update(request);
        }
    }    
}