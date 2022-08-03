﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IInventoryRepository: IBaseRepository<Inventory>
    {
        public Task<IReadOnlyList<Inventory>> GetInventoryByCategory(string categoryName, string sortBy
            , int pageSize,int currentPage);
        public Task<IReadOnlyList<Inventory>> GetInventoryByBrand(string brandName, string sortBy, int pageSize, int currentPage);

        public Task<IReadOnlyList<Inventory>> GetProducts(string sortBy, int pageSize, int currentPage);
        public Task<int> GetCount(Expression<Func<Inventory, bool>> whereClause);

        public Task<IReadOnlyList<Inventory>> GetInventoryForSupplier(int supplierId);
        public Task<Inventory> GetProduct(int id);
        public IQueryable<Inventory> FiltrationByColor(IQueryable<Inventory> query, string color);
        public IQueryable<Inventory> FiltrationByBrand(IQueryable<Inventory> query, int brandId);
        public IQueryable<Inventory> FiltrationByPrice(IQueryable<Inventory> query, decimal PriceMin, decimal PriceMax);
        public Task<IReadOnlyList<Inventory>> Filtration(string categoryName, string sortBy, int pageSize, int currentPage, string color, int brandId, decimal PriceMin, decimal PriceMax);
        public Task<IReadOnlyList<Inventory>> GetInventoriesByProduct(int productId);

    }
}
