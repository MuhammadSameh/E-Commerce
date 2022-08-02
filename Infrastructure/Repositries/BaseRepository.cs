using Core.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


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
