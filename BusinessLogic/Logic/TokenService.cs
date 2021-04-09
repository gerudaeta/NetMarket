using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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

        public TokenService(SymmetricSecurityKey symetricSecurityKey, IConfiguration configuration)
        {
            _symetricSecurityKey = symetricSecurityKey;
            _configuration = configuration;
        }

        public string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nombre),
                new Claim(JwtRegisteredClaimNames.FamilyName, usuario.Apellido),
                new Claim("username", usuario.UserName)
            };

            var credenciales = new SigningCredentials(_symetricSecurityKey, SecurityAlgorithms.HmacSha512);

            var tokenConfiguracion = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(10),
                SigningCredentials = credenciales,
                Issuer = _configuration["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfiguracion);
            return tokenHandler.WriteToken(token);
        }
    }
}