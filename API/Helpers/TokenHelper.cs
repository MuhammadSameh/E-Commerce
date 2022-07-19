using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Helpers
{
    public static class TokenHelper
    {
        public static SymmetricSecurityKey GenerateSecretKey(IConfiguration configuration)
        {
            var secretKey = configuration.GetValue<string>("SecretKey");
            var keyInBytes = Encoding.ASCII.GetBytes(secretKey);
            var key = new SymmetricSecurityKey(keyInBytes);
            return key;
        }

        public static JwtSecurityToken GenerateToken(IList<Claim> claims, DateTime expirtyDate, SymmetricSecurityKey key)
        {
            var signingCredintials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiryDate = expirtyDate;
            var securityToken = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredintials,
                expires: expiryDate
                );
            return securityToken;
        }
    }
}
