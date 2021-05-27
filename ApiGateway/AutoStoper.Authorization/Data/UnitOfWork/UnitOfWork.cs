using AutoStoper.Authorization.Data.Database;
using AutoStoper.Authorization.Data.Database.Models;
using AutoStoper.Authorization.Data.Repository;
using AutoStoper.Authorization.Data.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.Authorization.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private AuthDbContext _dbContext;
        public IRepository<User> Korisnici { get; set; }
        public UnitOfWork(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<User> KorisniciRepo
        {
            get
            {
                return Korisnici ??
                    (Korisnici = new BaseRepository<User>(_dbContext));
            }
        }


        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
