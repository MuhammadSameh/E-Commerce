using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EcommereceContextSeed
    {
        public static async Task SeedAsync(EcommerceContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Brands.Any())
                {
                    
                    var brandsData = File.ReadAllText("../Infrastructure/Data/MockData/Brands.json");
                    var brands = JsonSerializer.Deserialize<List<Brand>>(brandsData);
                    foreach (var item in brands)
                    {
                        context.Brands.Add(item);
                    }
                    context.Database.OpenConnection();
                    try
                    {
                        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Brands ON");
                        await context.SaveChangesAsync();
                        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Brands OFF");
                    }
                    finally
                    {
                        context.Database.CloseConnection();
                        
                    }
                   
                }

                if (!context.Categories.Any())
                {
                    var categoriesData = File.ReadAllText("../Infrastructure/Data/MockData/Categories.json");
                    var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
                    foreach (var item in categories)
                    {
                        context.Categories.Add(item);
                    }
                    context.Database.OpenConnection();
                    try
                    {
                        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Categories ON");
                        await context.SaveChangesAsync();
                        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Categories OFF");
                    }
                    finally
                    {
                        context.Database.CloseConnection();
                    }
                    
                }

                //if (!context.Products.Any())
                //{
                //    var productsData = File.ReadAllText("../Infrastructure/Data/MockData/ProductList.json");
                //    var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                //    foreach (var item in products)
                //    {
                //        context.Products.Add(item);
                //    }

                //    context.Database.OpenConnection();
                //    try
                //    {
                //        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Products ON");
                //        await context.SaveChangesAsync();
                //        await context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Products OFF");
                //    }
                //    finally
                //    {
                //        context.Database.CloseConnection();
                //    }
                    
                //}

                //if (!context.Inventories.Any())
                //{
                //    var inventoryData = File.ReadAllText("../Infrastructure/Data/MockData/Inventory.json");
                //    var inventory = JsonSerializer.Deserialize<List<Inventory>>(inventoryData);
                //    foreach (var item in inventory)
                //    {
                //        context.Inventories.Add(item);
                //    }
         
                //        await context.SaveChangesAsync();
                    
                //}

            }
            catch (Exception ex)
            {

                var logger = loggerFactory.CreateLogger<EcommereceContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}
