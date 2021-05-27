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
        public IRepository<Voznja> Voznje { get; set; }
        public IRepository<VoznjaUser> VoznjaUser { get; set; }
        public UnitOfWork(AutoStoperDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<Voznja> VoznjeRepo
        {
            get
            {
                return Voznje ??
                    (Voznje = new BaseRepository<Voznja>(_dbContext));
            }
        }

        public IRepository<VoznjaUser> VoznjaUserRepo
        {
            get
            {
                return VoznjaUser ??
                    (VoznjaUser = new BaseRepository<VoznjaUser>(_dbContext));
            }
        }



        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
