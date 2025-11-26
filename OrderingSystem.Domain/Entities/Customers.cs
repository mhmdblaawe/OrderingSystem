using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Domain.DbModels
{

    public class Customers
    {
        [Key]
        public int Id { get; private set; }
        [Required]
        public string Name { get; private set; } = string.Empty;
        [Required]
        public string Email { get; private set; } = string.Empty;
        [Required]
        public string Phone { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsActive { get; private set; }
   

        private Customers() { }

        public Customers(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
        public static Customers CreateFromDb(int id, string name, string email, string phone,
            DateTime createdAt, bool isDeleted,bool isActive)
        {
            return new Customers
            {
                Id = id,
                Name = name,
                Email = email,
                Phone = phone,
                CreatedAt = createdAt,
                IsDeleted = isDeleted,
                IsActive=isActive
            };
        }
        public static Customers Create(string name, string email, string phone)
        {
            return new Customers
            {
                Name = name,
                Email = email,
                Phone = phone,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                IsActive = true
            };
        }
        public void Update(string name, string email, string phone)
        {
            Name = name;
            Email = email;
            Phone = phone;
        }
        public void MarkDeleted()
        {
            IsDeleted = true;
            IsActive = false;
         }
    }

}
