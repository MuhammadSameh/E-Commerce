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
    public class SupplierController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration configuration;

        public SupplierController(UserManager<User> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<string>> Register(ReigsterDto registerDto)
        {
            var supplier = new User
            {
                UserName = registerDto.UserName,
                Address = registerDto.Address,
                Email = registerDto.Email,
                Cart = new Cart()
            };

            var result = await userManager.CreateAsync(supplier, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            await userManager.AddClaimsAsync(supplier, new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, supplier.Id),
                new Claim(ClaimTypes.Role, "Supplier")
            });

            return supplier.Id;

        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult> Login(LoginDto loginDto)
        {
            var supplier = await userManager.FindByEmailAsync(loginDto.Email);
            if (supplier == null) return BadRequest("This email is not registered");
            var isAuth = await userManager.CheckPasswordAsync(supplier, loginDto.Password);
            if (!isAuth) return Unauthorized("Wrong Password");

            var claims = await userManager.GetClaimsAsync(supplier);
            var key = TokenHelper.GenerateSecretKey(configuration);
            var securityToken = TokenHelper.GenerateToken(claims, DateTime.Now.AddDays(1),key);
            var tokenHandler = new JwtSecurityTokenHandler();
            return Ok(
                new { Token = tokenHandler.WriteToken(securityToken), ExpirtyDate = securityToken.ValidTo}
                );


        }

       
    }
}
