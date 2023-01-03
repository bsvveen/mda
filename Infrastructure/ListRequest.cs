
using MDA.Admin;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class ListRequest
    {
        [Required]
        public string? Entity { get; set; }
       
        public List<string>? Properties { get; set; }

        public List<Constrains>? Constrains { get; set; }

        public bool IsValid
        {
            get
            {
                var model = new AdminServices().Model;
                var entity = model.Entities.SingleOrDefault(tbl => tbl.Name == Entity);

                if (entity != null)
                {
                    var AllPropertiesExists = (Properties != null) && Properties.All(regProp => entity.Properties.Any(entProp => entProp.Name.Equals(regProp)));
                    return AllPropertiesExists;
                }

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
