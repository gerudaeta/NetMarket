using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<Usuario> BuscarUsuarioConDireccionAsync(this UserManager<Usuario> input, ClaimsPrincipal usrPrincipal)
        {
            var email = usrPrincipal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var usuario = await input.Users.Include(x => x.Direccion).SingleOrDefaultAsync(x => x.Email == email);
            
            return usuario;
        }
        
        public static async Task<Usuario> BuscarUsuarioAsync(this UserManager<Usuario> input, ClaimsPrincipal usrPrincipal)
        {
            var email = usrPrincipal?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var usuario = await input.Users.SingleOrDefaultAsync(x => x.Email == email);
            
            return usuario;
        }
    }
}