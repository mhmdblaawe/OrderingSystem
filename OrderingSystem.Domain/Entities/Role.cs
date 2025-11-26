using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Domain.Entities
{
    public class Role
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = "";


        private Role() { }

        public static Role Create(string name)
        {
            return new Role { Name = name };
        }
    }
}
