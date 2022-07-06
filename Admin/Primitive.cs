using MDA.Infrastructure;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MDA.Admin
{
    public class Primitive
    {  
        public static JSchema AsJSchema
        {
            get
            {
                JSchemaGenerator generator = new JSchemaGenerator();
                return generator.Generate(typeof(Primitive));
            }
        }

        [JsonPropertyName("Entities")]
        [Required]
        public List<Entity> Entities { get; set; }

        public class Entity
        {
            public Entity()
            {
                Properties = new List<Property>();
            }

            [JsonPropertyName("Name")]          
            [Required]
            public string Name { get; set; }

            [JsonPropertyName("Properties")]
            [Required]
            public List<Property> Properties { get; set; }
        }

        public class Property
        {
            [JsonPropertyName("Name")]
            [Required]
            public string Name { get; set; }

            [JsonPropertyName("Type")]
            [Required]
            public PropertyDataType Type { get; set; }

            [JsonPropertyName("NotNull")]
            [Required]
            public bool NotNull { get; set; }
        }
    }
}
