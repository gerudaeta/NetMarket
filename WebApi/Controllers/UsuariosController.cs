using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos;
using WebApi.Errors;
using WebApi.Extensions;

namespace WebApi.Controllers
{
    public class UsuariosController : BaseApiController
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public UsuariosController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
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
                Token = _tokenService.CreateToken(user),
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
                Token = _tokenService.CreateToken(user),
                Nombre = user.Nombre,
                Apellido = user.Apellido
            };
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UsuarioDto>> GetUsuario()
        {
            var usuario = await _userManager.BuscarUsuarioAsync(HttpContext.User);

            return new UsuarioDto
            {
                Email = usuario.Email,
                Username = usuario.UserName,
                Token = _tokenService.CreateToken(usuario),
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido
            };
        }

        [HttpGet("EmailValido")]
        public async Task<ActionResult<bool>> ValidarEmail([FromQuery] string email)
        {
            var usuario = await _userManager.FindByEmailAsync(email);

            if (usuario is null) return false;
            return true;
        }

        [Authorize]
        [HttpGet("direccion")]
        private async Task<ActionResult<DireccionDto>> ObtenerDireccion()
        {
            var usuario = await _userManager.BuscarUsuarioConDireccionAsync(HttpContext.User);
            var direccionDto = _mapper.Map<Direccion, DireccionDto>(usuario.Direccion);
            return direccionDto;
        }

        [Authorize]
        [HttpPut("direccion")]
        private async Task<ActionResult<DireccionDto>> ActualizarDireccion(DireccionDto direccionDto)
        {
            var usuario = await _userManager.BuscarUsuarioAsync(HttpContext.User);

            var direccion = _mapper.Map<DireccionDto, Direccion>(direccionDto);
            usuario.Direccion = direccion;
            
            var resultado = await _userManager.UpdateAsync(usuario);

            if (!resultado.Succeeded) return BadRequest("No se pudo actualizar la direccion del usuario");
            
            var direccionDtoUpdate = _mapper.Map<Direccion, DireccionDto>(usuario.Direccion);
            return Ok(direccionDtoUpdate);
        }
    }
}