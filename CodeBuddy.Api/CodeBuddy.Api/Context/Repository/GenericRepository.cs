using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace CodeBuddy.Api.Context.Repository
{
    public class GenericRepository : IGenericRepository
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            this._context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<T> Get<T>(int id
            , Expression<Func<T, object>> includes
            , Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().Include(includes).SingleOrDefaultAsync(predicate);
        }

        //public Task<IEnumerable<T>> GetAll<T>(T entity) where T : class
        public async Task<IEnumerable<T>> GetAll<T>(Expression<Func<T, object>> includes) where T : class
        {
            return await _context.Set<T>().Include(includes).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
