using Microsoft.EntityFrameworkCore;
using Order.DataAccess.DbContext;
using Order.DataAccess.Repositories.IRepositories;
using Order.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccess.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly OrderDbContext _db;

        public ProductRepository(OrderDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public Product GetProductWithImages(int productId)
        {
            return _db.Products
                .Include(p => p.ProductImages)
                .FirstOrDefault(p => p.Id == productId);
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Products.FirstOrDefault(s => s.Id == product.Id);
            if(objFromDb != null)
            {
                objFromDb.Name = product.Name;
                objFromDb.NetPrice = product.NetPrice;
                objFromDb.GrossPrice = product.GrossPrice;
                objFromDb.VATRate = product.VATRate;
                objFromDb.ActiveState = product.ActiveState;
                objFromDb.ProductImages = product.ProductImages;
                objFromDb.Symbol = product.Symbol; 
                objFromDb.ShortDescription = product.ShortDescription;
                objFromDb.LongDescription = product.LongDescription;
                objFromDb.MainImageId = product.MainImageId;
            }
            _db.SaveChanges();
        }
    }
}
