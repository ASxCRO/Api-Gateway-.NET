using AutoStoper.API.Data.Database;
using AutoStoper.API.Data.Database.Models;
using AutoStoper.API.Data.Repository;
using AutoStoper.API.Data.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoStoper.API.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private AutoStoperDbContext _dbContext;
        public BaseRepository<Voznja> VoznjeRepo { get; set; }
        public BaseRepository<VoznjaUser> VoznjeUserRepo { get; set; }
        public BaseRepository<Adresa> AdreseRepo { get; set; }

        public UnitOfWork(AutoStoperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<Voznja> Voznje
        {
            get
            {
                return VoznjeRepo ??
                    (VoznjeRepo = new BaseRepository<Voznja>(_dbContext));
            }
        }

        public IRepository<VoznjaUser> VoznjeUser
        {
            get
            {
                return VoznjeUserRepo ??
                    (VoznjeUserRepo = new BaseRepository<VoznjaUser>(_dbContext));
            }
        }

        public IRepository<Adresa> Adrese
        {
            get
            {
                return AdreseRepo ??
                    (AdreseRepo = new BaseRepository<Adresa>(_dbContext));
            }
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
