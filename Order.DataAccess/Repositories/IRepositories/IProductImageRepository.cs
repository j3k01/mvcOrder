using Order.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccess.Repositories.IRepositories
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        void Update(ProductImage ProductImage);
       // void DeleteImageById(int imageId);
        void Save();
    }
}
