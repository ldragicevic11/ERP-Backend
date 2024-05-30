using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Interface
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(string model);
        Task<IEnumerable<Product>> GetByNameAsync(string name);
    }
}
