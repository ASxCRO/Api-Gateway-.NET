using AutoStoper.Authorization.Data.Database.Models;
using AutoStoper.Authorization.Data.Repository;

namespace AutoStoper.Authorization.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<User> Korisnici { get; }
        void Commit();
    }
}
