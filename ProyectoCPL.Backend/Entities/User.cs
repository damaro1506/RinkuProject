using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cover.Backend.Entities
{
    public class User
    {
        public User()
        {
            Role = new Role();
            CustomerType = new CustomerType();
        }
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }
        public Role Role { get; set; }
        public bool Active { get; set; }
        public bool IsVisible { get; set; }
        public CustomerType CustomerType { get; set; }

    }
}
