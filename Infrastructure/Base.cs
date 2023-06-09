using MDA.Admin;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MDA.Infrastructure
{
    public class ApplicationInstance
    {
        public ApplicationInstance()
        {
            ModelServices modelservice = new();
            Model = modelservice.Model;
        }

        public Primitive Model { get; } = new Primitive();
    }

    public class ValidationResult
    {
        public ValidationResult()
        {
            Errors = new List<string>();
            ValidationErrors = new ModelStateDictionary();
        }

        public List<string> Errors { get; set; }

        public ModelStateDictionary ValidationErrors { get; set; }

        public bool IsValid { get { return Errors.Count == 0; } }
    }

    public class Constrain
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
