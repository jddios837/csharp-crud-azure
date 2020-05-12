using System.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

using backend_cshar.Entities;

namespace backend_cshar.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static ConcurrentDictionary<string, User> usersCache;

        private DataBaseDbContext db;

        public UserRepository(DataBaseDbContext db)
        {
            this.db = db;

            if (usersCache == null)
            {
                usersCache = new ConcurrentDictionary<string, User>(db.Users.ToDictionary(c => c.Id.ToString()));
            }
        }

        public async Task<User> CreateAsync(User u)
        {
            EntityEntry<User> added = await db.Users.AddAsync(u);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return usersCache.AddOrUpdate(u.Id.ToString(), u, UpdateCache);
            }
            else
            {
                return null;
            }

        }

        public Task<IEnumerable<User>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<User>>(() => usersCache.Values);
        }
        
        private User UpdateCache(string id, User u)
        {
            User old;
            if (usersCache.TryGetValue(id, out old))
            {
                if (usersCache.TryUpdate(id, u, old))
                {
                    return u;
                }
            }
            return null;
        }
        
    }
}