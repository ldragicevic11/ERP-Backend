using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Interface;
using System.Linq.Expressions;

namespace SmartZonePhoneShop.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
    
        protected readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbContext.Set<TEntity>().ToListAsync();
        }

        public async Task AddAsync(TEntity entity)  
        {
            _dbContext.Set<TEntity>().Add(entity);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            if (!EntityExists(entity))
            {
                throw new ArgumentException("Entitet ne postoji u bazi podataka.");
            }

            _dbContext.Set<TEntity>().Update(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            if (!EntityExists(entity))
            {
                throw new ArgumentException("Entitet ne postoji u bazi podataka.");
            }

            _dbContext.Set<TEntity>().Remove(entity);
            await SaveChangesAsync();
        }

        private async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Možete obraditi specifične greške koje se odnose na Entity Framework ovde
                throw new Exception("Greška prilikom čuvanja promena u bazi podataka.", ex);
            }
        }

        private bool EntityExists(TEntity entity)
        {
            var entry = _dbContext.Entry(entity);
            return entry.State != EntityState.Detached && entry.State != EntityState.Added;
        }
    }
}
