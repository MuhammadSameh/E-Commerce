using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class BaseService<T> : IBaseService<T>
    {
        private readonly IBaseRepository<T> _repository;

        public BaseService(IBaseRepository<T> repository)
        {
            _repository = repository;
        }

        public Task Add(T obj)
        {
            return _repository.Add(obj);
        }

        public void Delete(T obj)
        {
             _repository.Delete(obj);
        }

        public Task<List<T>> FindByCondition(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, Func<IQueryable<T>, int, int, IQueryable<T>> pagination = null, int pageSize = 20, int currentPage = 1)
        {
            return _repository.FindByCondition(expression, include, orderBy, pagination, pageSize);
        }

        public Task<IReadOnlyList<T>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<T> GetByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task Save()
        {
            return _repository.Save();
        }

        public Task Update(T obj)
        {
            return _repository.Update(obj);
        }
    }
}
