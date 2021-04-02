using AutoMapper;
using Core.Entities;

namespace WebApi.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Producto, ProductoDto>()
                .ForMember(x => x.CategoriaNombre, x=> x.MapFrom(c => c.Categoria.Nombre))
                .ForMember(x => x.MarcaNombre, x=> x.MapFrom(c => c.Marca.Nombre));
        }
    }
}