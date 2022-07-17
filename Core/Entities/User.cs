using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class User: IdentityUser
    {
        public string Address { get; set; }
        public int CartId { get; set; }
        public Cart cart { get; set; }
    }
}
