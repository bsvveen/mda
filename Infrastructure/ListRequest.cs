
using MDA.Admin;
using MediatR;
using System.ComponentModel.DataAnnotations;

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
        public string? Entity { get; set; }
       
        public List<string>? Properties { get; set; }

        public List<Constrains>? Constrains { get; set; }

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

                var missing = entity.Properties.Select(p => p.Key).Except(Properties.Select(p => p));
                if (!missing.Any())
                    return true;

                missing.ToList().ForEach(i => Errors.Add($"Property {i} does not exists on Entity {Entity} "));
                return false;
            }
        }
    }

    public class Constrains
    {
        public ConstrainOperator AndOr { get; set; }

        public string? Property { get; set; }

        public PropertyOperator? Operator { get; set; }

        public object? Value { get; set; }
    }

    public enum PropertyOperator 
    {
        EqualTo, NotEquealTo
    }

    public enum ConstrainOperator
    {
        And, Or
    }
}
