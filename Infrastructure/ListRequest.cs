
using MDA.Admin;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace MDA.Infrastructure
{
    public class ListRequest
    {
        public ListRequest() => Properties = new List<string>();

        [Required]
        public string? Entity { get; set; }
       
        public List<string> Properties { get; set; }

        public List<Constrains>? Constrains { get; set; }      
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
