using MDA.Infrastructure;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using System.Text.Json.Serialization;

namespace MDA.Admin
{
    public class Primitive
    {
        public Primitive()
        {
            Entities = new List<Entity>();
        }

        public static JSchema AsJSchema
        {
            get
            {
                JSchemaGenerator generator = new JSchemaGenerator();
                return generator.Generate(typeof(Primitive));
            }
        }

        [JsonPropertyName("Entity")]
        public List<Entity> Entities { get; set; }

        public class Entity
        {
            public Entity()
            {
                Properties = new List<Property>();
            }

            [JsonPropertyName("Name")]
            public string Name { get; set; }

            [JsonPropertyName("Properties")]
            public List<Property> Properties { get; set; }
        }

        public class Property
        {
            [JsonPropertyName("Name")]
            public string Name { get; set; }

            [JsonPropertyName("Type")]
            public PropertyDataType Type { get; set; }

            [JsonPropertyName("NotNull")]
            public bool NotNull { get; set; }
        }
    }
}
