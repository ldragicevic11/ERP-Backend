using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;
using System.Collections.Generic;

namespace SmartZonePhoneShop.Repository
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string model)
        {
            return await _dbContext.Set<Product>().Where(p => p.Model == model).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            return await _dbContext.Set<Product>().Where(p => p.Name == name).ToListAsync();
        }
    }
}
