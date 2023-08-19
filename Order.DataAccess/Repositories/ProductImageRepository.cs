using Order.DataAccess.DbContext;
using Order.DataAccess.Repositories.IRepositories;
using Order.Model.Models;
using Microsoft.AspNetCore.Hosting;



namespace Order.DataAccess.Repositories
{
    public class ProductImageRepository : Repository<ProductImage>, IProductImageRepository
    {
        private readonly OrderDbContext _db;
        public ProductImageRepository(OrderDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(ProductImage productImage)
        {
            _db.ProductImages.Update(productImage);
        }
    }
}
