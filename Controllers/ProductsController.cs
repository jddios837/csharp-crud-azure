using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend_cshar.Repositories;
using System.Collections.Generic;
using backend_cshar.Entities;

namespace backend_cshar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductRepository repo;

        public ProductsController(IProductRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/products
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public async Task<IEnumerable<Product>> GetProducts() {
            return await repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetProduct))] // Name Route
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProduct(string id)
        {
            Product p = await repo.RetrieveAsync(id);

            if (p == null)
            {
                return NotFound();
            }

            return Ok(p);
        }

        // POST: api/products
        // BODY: Customer (JSON, XML)
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] Product p)
        {
            if (p == null)
            {
                return BadRequest();
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product added = await repo.CreateAsync(p);

            // made a redirect inside method
            return CreatedAtRoute(
                routeName: nameof(GetProduct),
                routeValues: new { id = added.Id.ToString()},
                value: added
            );
        }

        // PUT: api/products/[id]
        // BODY: Product (JSON, XML)
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(string id, [FromBody] Product p)
        {
            if (p == null || p.Id.ToString() != id) {
                return BadRequest();
            }

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var existing = await repo.RetrieveAsync(id);

            if (existing == null) {
                return NotFound();
            }

            await repo.UpdateAsync(id, p);

            return new NoContentResult();
        }

        // DELETE: api/products/[id]
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(string id)
        {
            var existing = await repo.RetrieveAsync(id);

            if (existing == null) {
                return NotFound();
            }

            bool? deleted = await repo.DeleteAsync(id);

            if (deleted.HasValue && deleted.Value) {
                return new NoContentResult();
            } else {
                return BadRequest($"Customer {id} was found but failed to delete.");
            }
        }
 

    }
}