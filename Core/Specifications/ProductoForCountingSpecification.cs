using Core.Entities;

namespace Core.Specifications
{
    public class ProductoForCountingSpecification : BaseSpecification<Producto>
    {
        public ProductoForCountingSpecification(ProductoSpecificationParams productoSpecificationParams)
            : base(x => 
                (string.IsNullOrEmpty(productoSpecificationParams.Search) || x.Nombre.Contains(productoSpecificationParams.Search)) &&
                (!productoSpecificationParams.Marca.HasValue || x.MarcaId == productoSpecificationParams.Marca) && 
                (!productoSpecificationParams.Categoria.HasValue || x.CategoriaId == productoSpecificationParams.Categoria)
            )
        {
            
        }
    }
}