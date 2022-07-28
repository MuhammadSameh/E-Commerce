﻿using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositries
{
    public class InventoryRepository : BaseRepository<Inventory>, IInventoryRepository
    {
        private readonly EcommerceContext context;

        public InventoryRepository(EcommerceContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IReadOnlyList<Inventory>> GetInventoryByBrand(string brandName, string sortBy,
            int pageSize, int currentPage)
        {
            var query = context.Inventories.Where(i => i.Product.Brand.Name == brandName).Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand);
            var sortedQuery = AddSort(query,sortBy);

            return await AddPagination(sortedQuery, pageSize, currentPage).ToListAsync();

        }

        public async Task<IReadOnlyList<Inventory>> GetInventoryByCategory(string categoryName, string sortBy
            , int pageSize, int currentPage)
        {
            var query = context.Inventories
                .Where(i => i.Product.Category.Name == categoryName).Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand);

            var sortedQuery =  AddSort(query,sortBy);
            return await AddPagination(sortedQuery, pageSize, currentPage).ToListAsync();

        }


        public async Task<IReadOnlyList<Inventory>> GetProducts(string sortBy, int pageSize, int currentPage)
        {
            var query = context.Inventories
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand);

            var sortedQuery = AddSort(query, sortBy);
            return await AddPagination(sortedQuery, pageSize, currentPage).ToListAsync();


        }

        private IQueryable<Inventory> AddSort(IQueryable<Inventory> query, string sortBy)
        {
            switch (sortBy)
            {
                case "priceAsc":
                    {
                       query = query
                  .OrderBy(inv => inv.Price);

                    }
                    break;

                case "priceDesc":
                    {
                        query =query.OrderByDescending(inv => inv.Price);  
                    }
                    break;

                default:
                    {
                        query = query.OrderByDescending(inv => inv.CreatedDate);
                    }
                    break;
            }
            return query;
        }

        private IQueryable<Inventory> AddPagination(IQueryable<Inventory> query,  int pageSize, int currentPage)
        {
            query = query.Skip((currentPage-1)*pageSize).Take(pageSize);
            return query;
        }

        public async Task<int> GetCount(Expression<Func<Inventory, bool>> whereClause)
        {
            int count = await context.Inventories.Where(whereClause).CountAsync();
            return count;
        }

        public async Task<IReadOnlyList<Inventory>> GetInventoryForSupplier(int supplierId)
        {
            return await context.Inventories.Include(i => i.Medias).Include(i => i.Product).ThenInclude(p => p.Category).
                Include(i => i.Product.Brand).Where(i => i.Product.SupplierInfo.Id == supplierId).ToListAsync();
        }
    }
}
