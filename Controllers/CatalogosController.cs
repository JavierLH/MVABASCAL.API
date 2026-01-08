using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaAduanero.API.Models;
using SistemaAduanero.API.Models.Catalogos;

namespace SistemaAduanero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogosController : ControllerBase
    {
        private readonly AduanaDbContext _context;

        public CatalogosController(AduanaDbContext context)
        {
            _context = context;
        }

        // GET: api/Catalogos/incoterms
        [HttpGet("incoterms")]
        public async Task<ActionResult<IEnumerable<CatIncoterm>>> GetIncoterms()
        {
            return await _context.CatIncoterms
                                 .Where(c => c.Activo)
                                 .OrderBy(c => c.Clave) // Opcional: ordenar por clave
                                 .ToListAsync();
        }

        // GET: api/Catalogos/metodos-valoracion
        [HttpGet("metodos-valoracion")]
        public async Task<ActionResult<IEnumerable<CatMetodoValoracion>>> GetMetodosValoracion()
        {
            return await _context.CatMetodosValoracion
                                 .Where(c => c.Activo)
                                 .OrderBy(c => c.Clave)
                                 .ToListAsync();
        }

        // GET: api/Catalogos/formas-pago
        [HttpGet("formas-pago")]
        public async Task<ActionResult<IEnumerable<CatFormaPago>>> GetFormasPago()
        {
            return await _context.CatFormasPago
                                 .Where(c => c.Activo)
                                 .ToListAsync();
        }

        // GET: api/Catalogos/incrementables
        [HttpGet("incrementables")]
        public async Task<ActionResult<IEnumerable<CatIncrementable>>> GetIncrementables()
        {
            return await _context.CatIncrementables
                                 .Where(c => c.Activo)
                                 .ToListAsync();
        }

        // GET: api/Catalogos/decrementables
        [HttpGet("decrementables")]
        public async Task<ActionResult<IEnumerable<CatDecrementable>>> GetDecrementables()
        {
            return await _context.CatDecrementables
                                 .Where(c => c.Activo)
                                 .ToListAsync();
        }

        // GET: api/Catalogos/tipos-figura
        [HttpGet("tipos-figura")]
        public async Task<ActionResult<IEnumerable<CatTipoFigura>>> GetTiposFigura()
        {
            return await _context.CatTipoFigura
                                 .Where(c => c.Activo)
                                 .ToListAsync();
        }
    }
}