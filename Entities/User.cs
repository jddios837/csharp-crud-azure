using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace backend_cshar.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}