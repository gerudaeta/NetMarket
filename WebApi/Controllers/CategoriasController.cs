using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class CategoriasController : BaseApiController
    {
        private readonly IGenericRepositiory<Categoria> _categoriaRepository;

        public CategoriasController(IGenericRepositiory<Categoria> categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> GetCategoriasAll()
        {
            var categorias = await _categoriaRepository.GetAllAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> GetCategoriaById(int categoriaId)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(categoriaId);
            return Ok(categoria);
        }
        
    }
}