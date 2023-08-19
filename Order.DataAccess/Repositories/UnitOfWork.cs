using Order.DataAccess.DbContext;
using Order.DataAccess.Repositories.IRepositories;
using Order.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork 
    {
        private OrderDbContext _db;
        public IProductRepository Product { get; private set; }
        public IUserRepository User { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }
        public UnitOfWork(OrderDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            Product = new ProductRepository(_db);
            ProductImage = new ProductImageRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
