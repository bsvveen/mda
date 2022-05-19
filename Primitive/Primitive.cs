using MDA.Infrastructure;
using System.Text.Json.Serialization;

namespace MDA.Primitive
{
    public class Primitive
    {
        public Primitive()
        {
            Tables = new List<Table>();
        }

        [JsonPropertyName("Tables")]
        public List<Table> Tables { get; set; }

        public class Table
        {
            public Table()
            {
                Columns = new List<Column>();
            }

            [JsonPropertyName("Name")]
            public string Name { get; set; }

            [JsonPropertyName("Columns")]
            public List<Column> Columns { get; set; }
        }

        public class Column
        {
            [JsonPropertyName("Name")]
            public string Name { get; set; }

            [JsonPropertyName("Type")]
            public ColumnDataType Type { get; set; }

            [JsonPropertyName("NotNull")]
            public bool NotNull { get; set; }
        }
    }
}
