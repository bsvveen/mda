// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;

namespace MDA.Admin
{
    public interface IAdminSql
    {
        Task AddColumn(string TableName, string ColumnName, PropertyDataType eDataType, bool notnull);
        Task CreateTable(string TableName);
        Task DropColumn(string TableName, string ColumnName);
        Task DropTable(string TableName);
        Task<Primitive> GetModelFromDb();
    }
}