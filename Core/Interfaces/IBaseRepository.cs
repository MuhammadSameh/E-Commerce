using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
     public interface IBaseRepository<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task Add(T obj);
        Task Save();
        Task Update(T obj);
        void Delete(T obj);
        public Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression = null,
                                                     Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                                     Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                                     Func<IQueryable<T>, IQueryable<T>> pagination= null,
                                                     int pageSize = 20,
                                                     int currentPage = 1);
    }
}
