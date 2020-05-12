using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using backend_cshar.Repositories;
using System.Collections.Generic;
using backend_cshar.Entities;

namespace backend_cshar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserRepository repo;

        public UsersController(IUserRepository repo)
        {
            this.repo = repo;
        }

        // GET: api/users
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<User>))]
        public async Task<IEnumerable<User>> GetUsers() {
            return await repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetUser))] // Name Route
        [ProducesResponseType(200, Type = typeof(User))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(string id)
        {
            User u = await repo.RetrieveAsync(id);

            if (u == null)
            {
                return NotFound();
            }

            return Ok(u);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(User))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] User u)
        {
            if (u == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User added = await repo.CreateAsync(u);

            return CreatedAtRoute(
                routeName: nameof(GetUser),
                routeValues: new { id = added.Id.ToString()},
                value: added
            );
        }

    }
}