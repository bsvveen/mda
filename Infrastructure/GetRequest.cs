
using static MDA.Admin.Primitive;

namespace MDA.Infrastructure
{
    public class GetRequest
    {
        public string Entity { get; set; }
        public List<Property> Properties { get; set; }
        public Filter Filter { get; set; }

    }

    public class Filter
    {
        public string Property { get; set; }

        public eOperator Operator { get; set; }

        public object Value { get; set; }
    }
    public enum eOperator 
    {
        EqualTo, NotEquealTo
    }
}
