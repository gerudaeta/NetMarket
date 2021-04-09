using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
    public class UsuariosController : BaseApiController
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;

        public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UsuarioDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var resultado = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!resultado.Succeeded)
            {
                return Unauthorized(new CodeErrorResponse(401));
            }

            var userDto = new UsuarioDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token = "Token",
                Nombre = user.Nombre,
                Apellido = user.Apellido
            };

            return userDto;
        }

        [HttpPost("registrar")]
        public async Task<ActionResult<UsuarioDto>> RegistrarUsuario([FromBody] NuevoUsuarioDto registrarUsuarioDto)
        {
            var user = new Usuario
            {
                Email = registrarUsuarioDto.Email,
                UserName = registrarUsuarioDto.Username,
                Nombre = registrarUsuarioDto.Nombre,
                Apellido = registrarUsuarioDto.Apellido
            };

            var resultado = await _userManager.CreateAsync(user, registrarUsuarioDto.Password);

            if (!resultado.Succeeded)
            {
                return BadRequest(new CodeErrorResponse(400));
            }

            return new UsuarioDto
            {
                Email = user.Email,
                Username = user.UserName,
                Token = "Token",
                Nombre = user.Nombre,
                Apellido = user.Apellido
            };
        }
    }
}