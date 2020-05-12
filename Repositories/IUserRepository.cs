using System.Threading.Tasks;
using backend_cshar.Entities;
using System.Collections.Generic;


namespace backend_cshar.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> RetrieveAllAsync();
        Task<User> RetrieveAsync(string id);
        Task<User> CreateAsync(User u);

    }
}