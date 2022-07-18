using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
     public interface IBaseRepository<T>
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Add(T obj);
        void Update(T obj);
        void Delete(T obj);
    }
}
