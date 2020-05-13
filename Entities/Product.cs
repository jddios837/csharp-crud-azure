using System;
using System.ComponentModel.DataAnnotations;

namespace backend_cshar.Entities
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}