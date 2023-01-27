using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
        }         

        public ValidationResult CheckExistence(string Entity, List<string> Properties)
        {
            var validationResult = new ValidationResult(); 

            var entity = Entities.SingleOrDefault(e => e.Name == Entity);
            if (entity == null) {
                validationResult.Errors.Add($"Entity '{Entity}' does not exists in de model");
            } else  {
                var missing = Properties.Select(p => p).Except(entity.Properties.Select(p => p.Key));
                if (missing.Any()) {
                    missing.ToList().ForEach(i => validationResult.Errors.Add($"Property '{i}' does not exists on Entity '{Entity}' "));                
                }
            }   

            return validationResult;
        }

        public ValidationResult CheckValuesValidity(string Entity, List<string> Properties)
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
}
