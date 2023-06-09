
using MDA.Admin;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using static MDA.Infrastructure.Primitive;

namespace MDA.Infrastructure
{
    public class ListRequest
    {
        public ListRequest()
        {
            Properties = new List<string>();
            Errors = new List<string>();
        }

        [Required]
        public string? EntityName { get; set; }
       
        public List<string> Properties { get; set; }

        public List<Constrain>? Constrains { get; set; }

        public List<string>? Errors { get; set; }

        public bool IsValid()
        {
            var model = new ModelServices().Model;
            var entityModel = model.Entities.SingleOrDefault(tbl => tbl.Name == EntityName);

            if (entityModel == null)
            {
                Errors.Add($"Entity '{EntityName}' does not exists in de model");
                return false;
            }           

            var notInModel = Properties.Select(p => p).Except(entityModel.Properties.Select(p => p.Key));
            if (notInModel.Any())
                notInModel.ToList().ForEach(i => Errors.Add($"Property '{i}' does not exists on Entity '{EntityName}' "));

            if (Properties.Any(p => p == "Id"))
                Errors.Add($"The ListRequest properties collection should not contain an Id");

            return !Errors.Any();

        }
    }  
}
