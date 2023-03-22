using Core.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace Infrastructure.Repositries
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly EcommerceContext context;

        public BaseRepository(EcommerceContext context)
        {
            this.context = context;
        }

        public async Task Add(T obj)
        {
            await context.Set<T>().AddAsync(obj);
            await context.SaveChangesAsync();
        }

        public void Delete(T obj)
        {

           context.Set<T>().Remove(obj);
            context.SaveChanges();
        }

        public async Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression = null,
                                             Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
                                             Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                             Func<IQueryable<T>, int, int, IQueryable<T>> pagination = null,
                                             int pageSize = 20,
                                             int currentPage = 1)
        {
            try
            {
                IQueryable<T> query = context.Set<T>().AsNoTracking();

                if (expression != null)
                    query = query.Where(expression);

                if (include != null)
                    query = include(query);

                if (orderBy != null)
                    orderBy(query);

                if (pagination != null)
                   return await pagination(query, pageSize, currentPage).ToListAsync();

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't Retrieve Data:{ex.Message}");
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task Save()
        {
             await context.SaveChangesAsync();
        }

        public async Task Update(T obj)
        {
            context.Entry(obj).State = EntityState.Modified;
            await context.SaveChangesAsync();
        }
    }
}
