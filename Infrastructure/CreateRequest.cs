
using MDA.Admin;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Xml;
using static MDA.Infrastructure.Primitive;

namespace MDA.Infrastructure
{
    public class CreateRequest
    {
        public CreateRequest()
        {
            Properties = new Dictionary<string, object>();
            Errors = new List<string>();
        }

        [Required]
        public string? EntityName { get; set; }      

        public Dictionary<string,object>? Properties { get; set; }

        public List<string>? Errors { get; set; }        

        public ValidationResult Validate()
        {
            var retValue = new ValidationResult();
            
            var model = new ModelServices().Model;
            var entityModel = model.Entities.SingleOrDefault(tbl => tbl.Name == EntityName);

            if (entityModel == null)            
                retValue.Errors.Add($"Entity '{EntityName}' does not exists in de model");               

            var notInModel = Properties.Select(p => p.Key).Except(entityModel.Properties.Select(p => p.Key));
            if (notInModel.Any())
                notInModel.ToList().ForEach(i => retValue.Errors.Add($"Property '{i}' does not exists on Entity '{EntityName}' "));

            if (Properties.Any(p => p.Key == "Id"))
                retValue.Errors.Add($"The submit request properties collection should not contain an Id");

            foreach(var prop in Properties)
            {
                var validations = entityModel.Properties.Single(p => p.Key == prop.Key).Validations;

                if (validations != null)
                {
                    foreach (var validation in validations)
                    {
                        Type validationType = Type.GetType($"MDA.Infrastructure.{validation.Key}Validation");
                        if (validationType == null)                        
                            throw new Exception($"MDA.Infrastructure.{validation.Key}Validation\" was not found.");

                        object validationInstance = Activator.CreateInstance(validationType, null);
                        string validationMessage = (string)validationType.GetProperty("message").GetValue(validationInstance, null);
                        bool isValid = (bool)validationType.GetMethod("isValid").Invoke(validationInstance, new object[] { prop.Value });

                        if (!isValid)
                            retValue.ValidationErrors.AddModelError(prop.Key, validationMessage);
                    }
                }                
            }

            return retValue;           
        }
    }  
}
