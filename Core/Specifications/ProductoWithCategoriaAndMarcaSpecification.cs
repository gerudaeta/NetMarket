using Core.Entities;

namespace Core.Specifications
{
    public class ProductoWithCategoriaAndMarcaSpecification : BaseSpecification<Producto>
    {
        public ProductoWithCategoriaAndMarcaSpecification(ProductoSpecificationParams productoSpecificationParams)
            : base(x => 
                (string.IsNullOrEmpty(productoSpecificationParams.Search) || x.Nombre.Contains(productoSpecificationParams.Search)) &&
                (!productoSpecificationParams.Marca.HasValue || x.MarcaId == productoSpecificationParams.Marca) && 
                (!productoSpecificationParams.Categoria.HasValue || x.CategoriaId == productoSpecificationParams.Categoria)
                )
        {
            AddInclude(p => p.Categoria);
            AddInclude(p => p.Marca);
            
            ApplyPaging(productoSpecificationParams.PageSize * (productoSpecificationParams.PageIndex - 1), productoSpecificationParams.PageSize);

            if (!string.IsNullOrEmpty(productoSpecificationParams.Sort))
            {
                switch (productoSpecificationParams.Sort)
                {
                    case "nombreAsc": 
                        AddOrderBy(p => p.Nombre);
                        break;
                    case "nombreDesc": 
                        AddOrderByDescending(p => p.Nombre);
                        break;
                    case "precioAsc": 
                        AddOrderBy(p => p.Precio);
                        break;
                    case "precioDesc":
                        AddOrderByDescending(p => p.Precio);
                        break;
                    case "descripcionAsc": 
                        AddOrderBy(p => p.Descripcion);
                        break;
                    case "descripcionDesc":
                        AddOrderByDescending(p => p.Descripcion);
                        break;
                    default:
                        AddOrderBy(p => p.Nombre);
                        break;
                }
            }
        }

        public ProductoWithCategoriaAndMarcaSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.Categoria);
            AddInclude(p => p.Marca);
        }
    }
}