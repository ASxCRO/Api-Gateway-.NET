using ApiGateway.Core.User;
using AutoStoper.Authorization.Data.Repository;

namespace AutoStoper.Authorization.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<User> Korisnici { get; }
        void Commit();
    }
}
