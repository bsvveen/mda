using MDA.Admin;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace MDA.Infrastructure
{
    public class ApplicationInstance
    {
        public ApplicationInstance()
        {
            AdminServices adminservice = new();
            Model = adminservice.Model;
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
}
