using AutoStoper.API.Data.Database.Models;
using AutoStoper.API.Data.Repository;
using AutoStoper.Authorization.Data.Database.Models;
using AutoStoper.Authorization.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Authorization.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<User> Korisnici { get; }
        void Commit();
    }
}
