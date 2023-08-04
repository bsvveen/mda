
using MDA.Admin;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using static MDA.Infrastructure.ErrorHandlerMiddleware;

namespace MDA.Infrastructure
{
    public class CreateRequest
    {
        public CreateRequest()
        {
            Properties = new Dictionary<string, object>();
        }

        [Required]
        public string? EntityName { get; set; }      

        public Dictionary<string,object>? Properties { get; set; }         

        public void Validate()
        {            
            var model = new ModelServices().Model;
            var entityModel = model.Entities.SingleOrDefault(tbl => tbl.Name == EntityName);

            if (entityModel == null)            
                throw new ModelException($"Entity '{EntityName}' does not exists in de model");               

            var notInModel = Properties.Select(p => p.Key).Except(entityModel.Properties.Select(p => p.Key));
            if (notInModel.Any())
                throw new ModelException($"Entity '{string.Join(",", notInModel)}' does not exists on Entity '{EntityName}'");

            if (Properties.Any(p => p.Key == "Id"))
                throw new ModelException("The submit request properties collection should not contain an Id");

            var ValidationErrors = new ModelStateDictionary();           

            foreach (var modelProp in entityModel.Properties)
            {
                var validations = modelProp.Validations;

                if (validations != null)
                {
                    foreach (var validation in validations)
                    {
                        Type? validationType = Type.GetType($"MDA.Infrastructure.{validation.Key}Validation");
                        if (validationType == null)                        
                            throw new Exception($"MDA.Infrastructure.{validation.Key}Validation\" was not found.");

                        object? validationInstance = Activator.CreateInstance(validationType, new object[] { validation.Value.ToString() });
                        string? validationMessage = (string)validationType.GetProperty("Message").GetValue(validationInstance, null);
                        string? requestProp = (Properties.ContainsKey(modelProp.Key)) ? Properties[modelProp.Key].ToString() : null;

                        bool isValid = false;

                        try
                        {
                            isValid = (bool)validationType.GetMethod("isValid").Invoke(validationInstance, new object[] { requestProp });
                        } catch (Exception ex)
                        {
                            throw new Exception($"Error executing validation of type {validation.Key} --> " + ex.Message + ex.StackTrace);
                        }
                        
                        

                        if (!isValid)
                            ValidationErrors.AddModelError(modelProp.Key, validationMessage??"error");                       
                    }
                }                
            }

            if (!ValidationErrors.IsValid)
                throw new ModelValidationException(ValidationErrors);
        }
    }  
}
