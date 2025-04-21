using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CRMSystem.WebAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CRMSystem.WebAPI.Auth
{
    public class JwtProvider(IOptions<JwtOptions> options)
        : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;
    
        public string GenerateToken(User user)
        {
            Claim[] claims =
            [
                new("userId", user.Id.ToString()),
                new("username", user.Username),
                new("role", user.RoleId.ToString())
            ];
        
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);
        
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                issuer: _options.Issuer,
                audience: _options.Audience,
                expires: DateTime.UtcNow.AddHours(_options.ExpireHours));
        
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        
            return tokenString;
        }
    }
}