using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLogic.Data
{
    public class SeguridadDbContextData
    {
        private const string passwordUsuario = "german27=";
            
        public static async Task SeedUserAsync(UserManager<Usuario> userManager)
        {
            if (!userManager.Users.Any())
            {
                var usuario = new Usuario
                {
                    Nombre = "German",
                    Apellido = "Udaeta",
                    UserName = "gerudaeta",
                    Direccion = new Direccion
                    {
                        Calle = "Calle 27",
                        Ciudad = "Cordoba",
                        CodigoPostal = "5000",
                        Departamento = "Cordoba"
                    }
                };

                await userManager.CreateAsync(usuario, passwordUsuario);
            }
        }
    }
}