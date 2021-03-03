using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace HomeHelper.Repositories.Base
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetAsync(string id);
        Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null
            );
        Task<T> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null
            );

        Task AddAsync(T entity);
        Task RemoveAsync(string id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
