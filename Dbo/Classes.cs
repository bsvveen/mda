
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MDA.Dbo
{   
    [JsonConverter(typeof(EnumConverter<ColumnDataType>))]
    public class ColumnDataType
    {       
        public string? Value { get; private set; }        

        private ColumnDataType(string value)
        {
            Value = value;            
        }
        
        public static ColumnDataType CHAR_255 = new ColumnDataType("CHAR(255)");        
        public static ColumnDataType INT_255 = new ColumnDataType("INT(255)");     
        public static ColumnDataType DateTime = new ColumnDataType("DateTime");
    }

    [JsonConverter(typeof(EnumConverter<ColumnConstrains>))]
    public class ColumnConstrains
    {
        public string? Value { get; private set; } = string.Empty;

        private ColumnConstrains(string value)
        {
            Value = value;
        }              

        public static ColumnConstrains NULL = new ColumnConstrains("NULL");
        public static ColumnConstrains NOT_NULL = new ColumnConstrains("NOT NULL");
        public static ColumnConstrains UNIQUE = new ColumnConstrains("UNIQUE");
        public static ColumnConstrains FOREIGN_KEY = new ColumnConstrains("FOREIGN KEY");
    }    

    public class EnumConverter<T> : JsonConverter<T>
    {
        public override T? Read(ref Utf8JsonReader reader,Type typeToConvert,JsonSerializerOptions options)
        { 
            var val = reader.GetString();

            if (val == null || val == string.Empty)
                return default(T);

            Type Type = typeof(T);
            FieldInfo FieldInfo = Type.GetField(val);

            if (FieldInfo == null)
                throw new ArgumentException($"'{val}' is not a valid {Type}");

            return (T)FieldInfo.GetValue(null);
        }       

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
