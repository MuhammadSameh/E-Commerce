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
        public int? CartId { get; set; }
        public Cart Cart { get; set; }
    }
}
/*

{
        
        "cartId": null,
        "cart": null,



            ---

        "id": "",

        "userName": "",
        "email": "",
        "passwordHash": ,
        "address": "",
        "phoneNumberConfirmed": ,
        "twoFactorEnabled": ,
        "lockoutEnabled": ,
        "accessFailedCount": 

            ---

        "normalizedUserName": ,
        "normalizedEmail": ,
        "emailConfirmed": ,
        "securityStamp": ,
        "concurrencyStamp": ,
        "phoneNumber": ,
        "phoneNumberConfirmed": ,
        "twoFactorEnabled": ,
        "lockoutEnd": ,
        "lockoutEnabled": ,
        "lockoutEnd": ,
        "accessFailedCount": 
    } 



 */