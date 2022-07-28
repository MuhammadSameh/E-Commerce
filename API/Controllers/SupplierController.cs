using API.DTOs;
using API.Helpers;
using Core.Entities;
using Core.Interfaces;
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
        private readonly IBaseRepository<SupplierInfo> infoRepo;

        public SupplierController(UserManager<User> userManager, IConfiguration configuration, IBaseRepository<SupplierInfo> infoRepo)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.infoRepo = infoRepo;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<int>> Register(ReigsterDto registerDto)
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
            
            var supplierInfo = new SupplierInfo { UserId = supplier.Id };
           await infoRepo.Add(supplierInfo);
            await userManager.AddClaimsAsync(supplier, new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, supplier.Id),
                new Claim(ClaimTypes.Role, "Supplier"),
                new Claim("SupplierInfo", supplierInfo.Id.ToString())
            });
            return supplierInfo.Id;

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
            int supplierInfoId = 0;
            foreach (var claim in claims)
            {
                if(claim.Type == "SupplierInfo")
                {
                    supplierInfoId = int.Parse(claim.Value);
                }
            }
            return Ok(
                new { Token = tokenHandler.WriteToken(securityToken),SupplierInfoId= supplierInfoId ,ExpirtyDate = securityToken.ValidTo}
                );


        }

       
    }
}
