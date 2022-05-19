
using MDA.Infrastructure;

namespace MDA.Primitive.Database
{
    public interface ISql
    {
        Task AddColumn(string TableName, string ColumnName, ColumnDataType eDataType, bool notnull);

        Task CreateTable(string TableName);

        Task DropColumn(string TableName, string ColumnName);

        Task DropTable(string TableName);

        Task<Primitive> GetPrimitive();       
    }

}