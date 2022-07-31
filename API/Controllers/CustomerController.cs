using API.DTOs;
using API.Helpers;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public CustomerController(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<string>> Register(ReigsterDto registerDto)
        {
            var customer = new User
            {
                UserName = registerDto.UserName,
                Address = registerDto.Address,
                Email = registerDto.Email,
                Cart = new Cart()
            };

            var result = await userManager.CreateAsync(customer, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await userManager.AddClaimsAsync(customer, new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, customer.Id),
                new Claim(ClaimTypes.Role, "Customer"),
                new Claim("CartId", $"{customer.CartId}")
            });

            return customer.Id;

        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var customer = await userManager.FindByEmailAsync(loginDto.Email);
            if (customer == null) return BadRequest("This email is not registered");
            var isAuth = await userManager.CheckPasswordAsync(customer, loginDto.Password);
            if (!isAuth) return Unauthorized("Wrong Password");

            var claims = await userManager.GetClaimsAsync(customer);
            var key = TokenHelper.GenerateSecretKey(configuration);
            var securityToken = TokenHelper.GenerateToken(claims, DateTime.Now.AddDays(1),key);
            var tokenHandler = new JwtSecurityTokenHandler();

            int cartId = 0;
            foreach (var claim in claims)
            {
                if (claim.Type == "CartId")
                {
                    cartId = int.Parse(claim.Value);
                }
            }
            return Ok(
                new { Token = tokenHandler.WriteToken(securityToken),CartId=cartId, ExpirtyDate = securityToken.ValidTo}
                );


        }

       
    }
}
