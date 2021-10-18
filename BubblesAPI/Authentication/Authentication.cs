using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BubblesAPI.Authentication
{
    public class Authentication : IAuthentication
    {
        public string GetToken(string id, string key, string issuer)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("id", id)
            };
            var token = new JwtSecurityToken(issuer, issuer,
                claims, expires: DateTime.Now.AddMinutes(240), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}