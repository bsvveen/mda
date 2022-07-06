
using System.ComponentModel.DataAnnotations;
using static MDA.Admin.Primitive;

namespace MDA.Infrastructure
{
    public class ListRequest
    {
        [Required]
        public string? Entity { get; set; }
       
        public List<Property>? Properties { get; set; }

        public Filter? Filter { get; set; }

    }

    public class Filter
    {
        public string? Property { get; set; }

        public EOperator Operator { get; set; }

        public object? Value { get; set; }
    }

    public enum EOperator 
    {
        EqualTo, NotEquealTo
    }

    public class ViewRequest
    {
        [Required]
        public string? Query { get; set; }

        [Required]
        public string[] Properties { get; set; }
    }
}
