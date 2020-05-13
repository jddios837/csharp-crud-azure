using System.Collections;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.EntityFrameworkCore; // Include is here
using System.Threading.Tasks;

using backend_cshar.Entities;

namespace backend_cshar.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static ConcurrentDictionary<string, Product> productsCache;
        private DataBaseDbContext db;

        public ProductRepository(DataBaseDbContext db)
        {
            this.db = db;
            if(productsCache == null)
            {
                // var users = db.Products.Include(p => p.User).ToList();
                productsCache = new ConcurrentDictionary<string, Product>(db.Products.ToDictionary(p => p.Id.ToString(), p => p));
            }
        }

        public async Task<Product> CreateAsync(Product p)
        {
            User u = db.Users.Find(p.UserId);
            // p.User = u;

            EntityEntry<Product> added = await db.Products.AddAsync(p);
            u.Products.Add(added.Entity);

            int affected = await db.SaveChangesAsync();

            if(affected == 1) {
                return productsCache.AddOrUpdate(p.Id.ToString(), p, UpdateCache);
            } else {
                return null;
            }
        }

        public Task<IEnumerable<Product>> RetrieveAllAsync()
        {
            return Task.Run<IEnumerable<Product>>(() => productsCache.Values);
        }

        public Task<Product> RetrieveAsync(string id)
        {
            return Task.Run(() => {
                Product p;
                productsCache.TryGetValue(id, out p);
                return p;
            });
        }

        public async Task<Product> UpdateAsync(string id, Product p)
        {
            db.Products.Update(p);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return UpdateCache(id, p);
            }

            return null;
        }

        public async Task<bool?> DeleteAsync(string id)
        {
            Product p = db.Products.Find(int.Parse(id));
            db.Products.Remove(p);
            int affected = await db.SaveChangesAsync();

            if (affected == 1)
            {
                return productsCache.TryRemove(id, out p);
            } else {
                return null;
            }
        }


        private Product UpdateCache(string id, Product p)
        {
            Product old;
            if (productsCache.TryGetValue(id, out old)) {
                if (productsCache.TryUpdate(id, p, old)) {
                    return p;
                }
            }
            return null;
        }
    }
}