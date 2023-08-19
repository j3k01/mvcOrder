using Order.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.DataAccess.Repositories.IRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User user);
        void Save();
    }
}
