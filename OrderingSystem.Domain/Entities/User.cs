using System.Collections.Generic;

namespace OrderingSystem.Domain.Entities
{
    public class User
    {
        public int Id { get; private set; }
        public string Username { get; private set; } = "";
        public string PasswordHash { get; private set; } = "";
        public bool IsActive { get; private set; }
        public List<UserRole> Roles { get; private set; } = new();

        private User() { } // EF Core

        // Factory method
        public static User Create(string username, string passwordHash)
        {
            return new User
            {
                Username = username,
                PasswordHash = passwordHash,
                IsActive = true
            };
        }

         public void UpdateUsername(string username)
        {
            Username = username;
        }

         public void Deactivate()
        {
            IsActive = false;
        }

         public void AssignRole(int roleId)
        {
            Roles.Add(new UserRole(Id, roleId));
        }
    }
}
