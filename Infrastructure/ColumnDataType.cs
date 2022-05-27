
using Newtonsoft.Json.Schema;
using System.Data;
using System.Text.Json;

namespace MDA.Infrastructure
{
    public enum ColumnDataType
    {
        Text, Number, DateTime, ID
    }

    static class Extensions
    {
        public static JSchemaType ToJSchemaType(this ColumnDataType datatype)
        {
            switch (datatype)
            {
                case ColumnDataType.Text: return JSchemaType.String;
                case ColumnDataType.Number: return JSchemaType.Integer;
                case ColumnDataType.DateTime: return JSchemaType.Object;
                case ColumnDataType.ID: return JSchemaType.String;
                default: throw new ArgumentOutOfRangeException(nameof(datatype));
            }
        }        
    }
}
