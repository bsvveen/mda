// https://www.developersoapbox.com/connecting-to-a-sqlite-database-using-net-core/

using MDA.Infrastructure;

namespace MDA.User
{
    public interface IUserSql
    {
        Task<string> GetById(GetByIdRequest request);
        Task<string> List(ListRequest request);
        Task Submit(SubmitRequest request);
    }
}