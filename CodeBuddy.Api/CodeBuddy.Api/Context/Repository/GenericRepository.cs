using CodeBuddy.Api.Helpers;
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

        public async Task<T> Get<T>(int id) where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Get<T>(int id
            , Expression<Func<T, bool>> predicate1
            , Expression<Func<T, bool>> predicate2) where T : class
        {
            return await _context.Set<T>().Where(predicate1).FirstOrDefaultAsync(predicate2);
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

        public async Task<PagedList<T>> GetAll<T>(Expression<Func<T, object>> includes, 
            UserParams userParams, 
            Expression<Func<T, bool>> predicate = null) where T : class
        {
            var result = _context.Set<T>().Include(includes).AsQueryable().Where(predicate);

            return await PagedList<T>.CreateAsync(result, userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<T> GetConnection<T>(int userId, int recipientId, Expression<Func<T, bool>> predicate) where T : class
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);

            //u => u.FollowerId == userId && u.FollowingId == recipientId
        }

        public async Task<IEnumerable<int>> GetUserFollower(int id, bool followers)
        {
            var user = await _context.Users
                .Include(x => x.Followers)
                .Include(x => x.Followings)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (followers)
            {
                return user.Followers.Where(u => u.FollowingId == id)
                    .Select(i => i.FollowerId);
            }

            else
            {
                return user.Followings.Where(u => u.FollowerId == id)
                    .Select(i => i.FollowingId);
            }
        }
    }
}
