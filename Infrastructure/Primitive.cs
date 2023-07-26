using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.Json.Serialization;
using static MDA.Infrastructure.Primitive;

namespace MDA.Infrastructure
{
    public class Primitive
    {
        public Primitive()
        {
            Entities = new List<Entity>();
        }

        [JsonPropertyName("entities")]
        [Required]
        public List<Entity> Entities { get; set; }

        public class Entity
        {
            public Entity()
            {
                Properties = new List<Property>();
                Name = string.Empty;
            }

            [JsonPropertyName("name")]
            [Required]
            public string Name { get; set; }

            [JsonPropertyName("properties")]
            [Required]
            public List<Property> Properties { get; set; }
        }

        public class Property
        {
            public Property()
            {
                Key = string.Empty;
                Name = string.Empty;
            }

            [JsonPropertyName("key")]
            [Required]
            public string Key { get; set; }

            [JsonPropertyName("name")]
            [Required]
            public string Name { get; set; }

            [JsonPropertyName("type")]
            [Required]
            public PropertyDataType Type { get; set; }          

            [JsonPropertyName("notnull")]
            [Required]
            public bool NotNull { get; set; }

            [JsonPropertyName("foreignkey")]
            public ForeignKey ForeignKey { get; set; }

            [JsonPropertyName("validations")]
            public Dictionary<string, object> Validations { get; set; }
        }

        public class ForeignKey
        {           
            public ForeignKey()
            {
                Constrains = new List<Constrain>();
            }

            [JsonPropertyName("related")]
            public string Relatedentity { get; set; }

            [JsonPropertyName("lookup")]
            public string Lookup { get; set; }

            [JsonPropertyName("constrains")]
            public List<Constrain> Constrains { get; set; }
        }

        public abstract class Validation
        {
            public string configuration { get; set;  }

            public abstract string message { get; }

            public abstract bool isValid(object instance);
        }      

        public ValidationResult CheckValuesValidity(string Entity, Dictionary<string, object> Properties)
        {
            var validationResult = new ValidationResult();     
            return validationResult;
        }

        public ValidationResult CheckAuthorization(string Entity, List<string> Properties)
        {
            var validationResult = new ValidationResult();
            return validationResult;
        }
    }

    public class requiredValidation : Validation
    {
        public override string message => "Cannot be NULL";

        public override bool isValid(object instance)
        {
            return !(configuration == "true" && instance == null);
        }
    }
}
