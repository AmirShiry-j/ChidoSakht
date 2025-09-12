using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class Address
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Navs
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
