using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogic.Logic
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _symetricSecurityKey;
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _symetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:Key"]));
        }

        public string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, usuario.Email),
                new(JwtRegisteredClaimNames.Name, usuario.Nombre),
                new(JwtRegisteredClaimNames.FamilyName, usuario.Apellido),
                new("username", usuario.UserName),
                new("id", usuario.Id)
            };

            var credencials = new SigningCredentials(_symetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenConfiguracion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = credencials,
                Issuer = _configuration["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfiguracion);
            
            return tokenHandler.WriteToken(token);
        }
    }
}