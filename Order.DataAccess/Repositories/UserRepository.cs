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
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly OrderDbContext _db;

        public UserRepository(OrderDbContext db) : base(db)
        {
            _db = db;
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(User user)
        {
            _db.Users.Update(user);
        }

        public User Get(Expression<Func<User, bool>> filter)
        {
            return DbSet.FirstOrDefault(filter);
        }
    }
}
