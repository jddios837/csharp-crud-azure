using System.Threading.Tasks;
using backend_cshar.Entities;
using System.Collections.Generic;

namespace backend_cshar.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> RetrieveAllAsync();
        Task<Product> RetrieveAsync(string id);
        Task<Product> CreateAsync(Product p);
        Task<Product> UpdateAsync(string id, Product u);
        Task<bool?> DeleteAsync(string id);
    }
}