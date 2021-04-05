using System;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Core.Specifications;
using WebApi.Dtos;
using WebApi.Errors;

namespace WebApi.Controllers
{
    public class ProductosController : BaseApiController
    {
        private readonly IGenericRepositiory<Producto> _productoRepository;
        private readonly IMapper _mapper;

        public ProductosController(IGenericRepositiory<Producto> productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }
        
        [HttpGet]
        public async Task<ActionResult<Pagination<Producto>>> GetProductosAll([FromQuery] ProductoSpecificationParams productoSpecificationParams)
        {
            // spec = debe incluir la logica de la condicion de la consulta y tambien las relaciones entre las entidades
            // en este caso seria la relacion, entre producto y marca, categoria
            var spec = new ProductoWithCategoriaAndMarcaSpecification(productoSpecificationParams);
            var productos = await _productoRepository.GetAllWithSpec(spec);
            var specCount = new ProductoForCountingSpecification(productoSpecificationParams);
            var totalProductos = await _productoRepository.CountAsync(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalProductos / productoSpecificationParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<Producto>, IReadOnlyList<ProductoDto>>(productos);

            var result = new Pagination<ProductoDto>
            {
                Count = totalProductos,
                Data = data,
                PageCount = totalPages,
                PageIndex = productoSpecificationParams.PageIndex,
                PageSize = productoSpecificationParams.PageSize
            };
            
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProductoById(int id)
        {
            // spec = debe incluir la logica de la condicion de la consulta y tambien las relaciones entre las entidades
            // en este caso seria la relacion, entre producto y marca, categoria
            var spec = new ProductoWithCategoriaAndMarcaSpecification(id);
            var producto = await _productoRepository.GetByIdWithSpec(spec);

            if (producto is null) return NotFound(new CodeErrorResponse(404, "El producto no existe."));
            
            var productoDto = _mapper.Map<Producto, ProductoDto>(producto);
            return Ok(productoDto);

        }
        
    }
}
