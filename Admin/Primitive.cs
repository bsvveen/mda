using MDA.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MDA.Admin
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
    }
}
