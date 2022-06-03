
using Newtonsoft.Json.Schema;

namespace MDA.Infrastructure
{
    public enum PropertyDataType
    {
        Text, Number, DateTime, ID
    }

    static class Extensions
    {
        public static JSchemaType ToJSchemaType(this PropertyDataType datatype)
        {
            switch (datatype)
            {
                case PropertyDataType.Text: return JSchemaType.String;
                case PropertyDataType.Number: return JSchemaType.Integer;
                case PropertyDataType.DateTime: return JSchemaType.Object;
                case PropertyDataType.ID: return JSchemaType.String;
                default: throw new ArgumentOutOfRangeException(nameof(datatype));
            }
        }        
    }
}
