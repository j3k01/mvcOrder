using Order.DataAccess.Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Order.DataAccess.Repositories.IRepositories;
using Order.DataAccess.DbContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Order.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly OrderDbContext _dbContext;
        internal DbSet<T> DbSet { get; set; }
        public Repository(OrderDbContext dbContext) 
        {
            _dbContext = dbContext;
            this.DbSet = dbContext.Set<T>();
        }
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        //public T Get(Expression<Func<T, bool>> filter)
        //{
        //    IQueryable<T> query = DbSet;
        //    query = query.Where(filter);
        //    return query.FirstOrDefault();
        //}

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return query.ToList();
        }

        public T Get(Expression<Func<T, bool>> filter, string includeProperties)
        {
            IQueryable<T> query = DbSet;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                var includeProps = includeProperties.Split(',');
                foreach (var includeProp in includeProps)
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            query = query.Where(filter);
            return query.FirstOrDefault();
        }
    }
}
