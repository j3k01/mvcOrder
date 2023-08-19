using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccess.Repositories.IRepositories
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IProductRepository Product { get; }
        IProductImageRepository ProductImage { get; }

        void Save();
    }
}
