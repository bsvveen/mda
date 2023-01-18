
using MDA.Admin;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class SubmitRequest
    {
        public SubmitRequest()
        {
            Properties = new Dictionary<string, string>();
            Errors = new List<string>();
        }

        [Required]
        public string? Entity { get; set; }

        public Guid? Id { get; set; }

        public Dictionary<string,string>? Properties { get; set; }

        public List<string>? Errors { get; set; }

        public bool IsValid
        {
            get
            {
                var model = new AdminServices().Model;
                var entity = model.Entities.SingleOrDefault(tbl => tbl.Name == Entity);

                if (entity == null)
                {
                    Errors.Add($"Entity {Entity} does not exists in de model");
                    return false;
                }

                var missing = entity.Properties.Select(p => p.Key).Except(Properties.Select(p => p.Key));
                if (!missing.Any())
                    return true;

                missing.ToList().ForEach(i => Errors.Add($"Property {i} does not exists on Entity {Entity} "));
                return false;
            }
        }       
    }  
}
