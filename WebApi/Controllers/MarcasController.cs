using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class MarcasController : BaseApiController
    {
        private readonly IGenericRepositiory<Marca> _marcaRepository;

        public MarcasController(IGenericRepositiory<Marca> marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Marca>>> GetMarcasAll()
        {
            var marcas = await _marcaRepository.GetAllAsync();
            return Ok(marcas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Marca>> GetMarcaById(int marcaId)
        {
            var marca = await _marcaRepository.GetByIdAsync(marcaId);
            return Ok(marca);
        }
    }
}