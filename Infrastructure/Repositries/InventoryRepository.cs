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
using Microsoft.EntityFrameworkCore.Query;

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

        public async Task<Inventory> GetProduct(int id)
        {
            return await context.Inventories
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand).Where(i => i.InventoryId == id).FirstOrDefaultAsync();




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

        public async Task<IReadOnlyList<Inventory>> GetInventoriesByProduct(int productId)
        {
            var query = context.Inventories
                .Where(b => b.Product.ProductId==productId)
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand).ToListAsync();

            return await query;

        }

        public async Task<IReadOnlyList<Inventory>> Filtration(string categoryName, string sortBy, int pageSize, int currentPage, string color, int brandId, decimal PriceMin, decimal PriceMax)
        {
            var query= context.Inventories
                .Where(b => b.Product.Category.Name == categoryName)
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand);
            
            if (!String.IsNullOrEmpty(color)) {query= (IIncludableQueryable<Inventory, Brand>)FiltrationByColor(query, color); }
            if (!(Decimal.ToDouble(PriceMin)==0)|| !(Decimal.ToDouble(PriceMax) == 0)) { query = (IIncludableQueryable<Inventory, Brand>)FiltrationByPrice(query, PriceMin, PriceMax); }
            if (!(brandId==0)) { query = (IIncludableQueryable<Inventory, Brand>)FiltrationByBrand(query, brandId); }


            var sortedQuery = AddSort(query, sortBy);
            return await AddPagination(sortedQuery, pageSize, currentPage).ToListAsync();
        }

        public IQueryable<Inventory> FiltrationByColor(IQueryable<Inventory> query, string color)
        {
            var outQuery = query
                .Where(b => b.Color==color)
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand);
            return outQuery;
        }

        public IQueryable<Inventory> FiltrationByBrand(IQueryable<Inventory> query, int brandId)
        {
            var outQuery = query
                .Where(b => b.Product.BrandId==brandId)
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand);
            return outQuery;
        }

        public IQueryable<Inventory> FiltrationByPrice(IQueryable<Inventory> query, decimal PriceMin, decimal PriceMax)
        {
            var outQuery = query
                .Where(b => b.Price >PriceMin&&b.Price<PriceMax)
                .Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand);
            return outQuery;
        }

        public async Task<IReadOnlyList<Inventory>> GetInvenntoriesByNameAndSupplier(string name, int supplierId)
        {
            var invens = await context.Inventories.Include(c => c.Product)
                .Include(b => b.Product.Category)
                .Include(b => b.Medias)
                .Include(m => m.Product.Brand)
                .Where(i => i.Product.SupplierInfoId == supplierId && i.Product.Name.Contains(name))
                .ToListAsync();
            return invens;
        }
    }
}
