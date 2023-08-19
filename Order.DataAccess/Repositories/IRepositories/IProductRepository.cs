using Order.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccess.Repositories.IRepositories
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product);
        void Save();

        Product GetProductWithImages(int productId);
    }
}
