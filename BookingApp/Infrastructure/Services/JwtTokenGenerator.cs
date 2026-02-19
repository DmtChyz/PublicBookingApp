using Application.DTO;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserTokenDataDto userData)
        {
            if (userData == null) return null;

            var claimsList = GetClaim(userData);
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("JwtSettings:Secret").Value));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenObject = new JwtSecurityToken(

                issuer: _configuration.GetSection("JwtSettings:Issuer").Value,
                audience: _configuration.GetSection("JwtSettings:Audience").Value,
                claims: claimsList,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(15)
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenObject); ;

        }


        protected static List<Claim> GetClaim(UserTokenDataDto userData)
        {
            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Name,userData.Username),
                new Claim(JwtRegisteredClaimNames.Sub,userData.Id),
            };
            foreach(var role in userData.Role)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }
    }
}
