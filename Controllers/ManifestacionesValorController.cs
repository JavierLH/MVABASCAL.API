using global::SistemaAduanero.API.Models;
using SistemaAduanero.API.Services;
using SistemaAduanero.Shared.DTOs; 
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaAduanero.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAduanero.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManifestacionesValorController : ControllerBase
    {
        private readonly AduanaDbContext _context;
        private readonly ICurrentUserService _currentUser; 

        public ManifestacionesValorController(AduanaDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        // GET: api/ManifestacionesValor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManifestacionesValor>>> GetManifestacionesValor()
        {
            // Incluimos las relaciones importantes como los pagos o conceptos
            return await _context.ManifestacionesValors
                .Include(m => m.ManifestacionConceptosValors)
                .Include(m => m.ManifestacionPagos)
                .ToListAsync();
        }

        // GET: api/ManifestacionesValor/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ManifestacionesValor>> GetManifestacionesValor(int id)
        {
            var manifestacion = await _context.ManifestacionesValors
                // 1. Traer a los hijos (COVES)
                .Include(m => m.ManifestacionCoves)
                    // 2. Traer a los nietos (INCREMENTABLES dentro de los COVEs)
                    .ThenInclude(c => c.ManifestacionConceptosValor)
                .FirstOrDefaultAsync(m => m.ManifestacionId == id);

            if (manifestacion == null) return NotFound();

            return manifestacion;
        }

        // PUT: api/ManifestacionesValor/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutManifestacion(int id, CrearManifestacionDto dto)
        {
            // 1. Buscar la manifestación existente con todas sus relaciones
            var manifestacionExistente = await _context.ManifestacionesValors
                .Include(m => m.ManifestacionCoves) // Traemos los hijos para poder borrarlos/reemplazarlos
                .FirstOrDefaultAsync(m => m.ManifestacionId == id);

            if (manifestacionExistente == null)
            {
                return NotFound($"No se encontró el expediente {id}");
            }

            // 2. Actualizar datos de la Cabecera (Abuelo)
            manifestacionExistente.NumeroPedimento = dto.ReferenciaAdmin;
            manifestacionExistente.TipoOperacion = dto.TipoOperacion;
            // Actualiza aquí otros campos si los tuvieras (ej. FechaFirma, etc.)

            // 3. ESTRATEGIA DE REEMPLAZO PARA HIJOS (COVES)
            // Borramos los COVEs viejos de la base de datos para insertar la nueva versión limpia
            // EF Core borrará en cascada los incrementables (nietos) si la relación está bien configurada
            if (manifestacionExistente.ManifestacionCoves != null)
            {
                _context.ManifestacionCoves.RemoveRange(manifestacionExistente.ManifestacionCoves);
            }

            // 4. Insertamos los COVEs nuevos (Usamos la misma lógica de mapeo que en el POST)
            manifestacionExistente.ManifestacionCoves = dto.Coves.Select(c => new ManifestacionCove
            {
                // Mapeo COVE
                NumeroCove = c.NumeroCove ?? "S/N",
                Incoterm = c.Incoterm ?? "",
                ExisteVinculacion = c.ExisteVinculacion,
                NumeroPedimento = c.NumeroPedimento ?? "",
                Patente = c.Patente,
                Aduana = c.Aduana,
                FechaPago = c.FechaPago,
                TotalPago = c.TotalPago,
                TipoPago = c.TipoPago ?? "",
                MonedaPago = c.MonedaPago ?? "",
                TipoCambioPago = c.TipoCambioPago,
                MetodoValoracion = c.MetodoValoracion ?? "",

                // Totales
                TotalPrecioPagado = c.TotalPrecioPagado,
                TotalPrecioPorPagar = c.TotalPrecioPorPagar,
                TotalIncrementables = c.TotalIncrementables,
                TotalDecrementables = c.TotalDecrementables,
                TotalValorAduana = c.TotalValorAduana,

                FechaRegistro = DateTime.Now,
                Activo = true,

                // Mapeo NIETOS (Incrementables)
                ManifestacionConceptosValor = c.Incrementables.Select(inc => new ManifestacionConceptosValor
                {
                    TipoConcepto = "INCREMENTABLE",
                    ClaveConcepto = inc.TipoIncrementable,
                    FechaErogacion = inc.FechaErogacion,
                    Importe = inc.Importe,
                    TipoMoneda = inc.TipoMoneda,
                    TipoCambio = inc.TipoCambio,
                    AcargoImportador = inc.ACargoImportador
                }).ToList()

            }).ToList();

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Actualizado correctamente" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al actualizar: {ex.Message}");
            }
        }




        [HttpPost]
        public async Task<ActionResult> PostManifestacion(CrearManifestacionDto dto)
        {
            // 1. Validar Seguridad
            if (_currentUser.ClienteId == 0) return Unauthorized("Token inválido o sesión expirada.");

            // 2. Validar Datos Mínimos
            if (string.IsNullOrEmpty(dto.ReferenciaAdmin)) return BadRequest("Se requiere una Referencia o Pedimento.");
            if (dto.Coves == null || dto.Coves.Count == 0) return BadRequest("Debe agregar al menos un COVE.");

            try
            {
                // 3. Crear el OBJETO PADRE (La Carpeta)
                var nuevaManifestacion = new ManifestacionesValor
                {
                    NumeroPedimento = dto.ReferenciaAdmin,
                    TipoOperacion = dto.TipoOperacion,
                    ClienteId = _currentUser.ClienteId,
                    UsuarioCreadorId = _currentUser.UsuarioId,
                    FechaRegistro = DateTime.Now,
                    Activo = true,
                    EstadoEnvio = "BORRADOR",

                    // COMODÍN: Para evitar error 500 por la columna vieja 'Cove' que aún pide datos
                    Cove = "",

                    // 4. Crear los HIJOS (Los COVEs)
                    ManifestacionCoves = dto.Coves.Select(c => new ManifestacionCove
                    {
                        // Identificadores del COVE
                        NumeroCove = c.NumeroCove ?? "S/N",
                        Incoterm = c.Incoterm ?? "",
                        ExisteVinculacion = c.ExisteVinculacion,

                        // Datos Pedimento Interno
                        NumeroPedimento = c.NumeroPedimento ?? "",
                        Patente = c.Patente,
                        Aduana = c.Aduana,

                        // Datos Pago
                        FechaPago = c.FechaPago,
                        TotalPago = c.TotalPago,
                        TipoPago = c.TipoPago ?? "",
                        MonedaPago = c.MonedaPago ?? "",
                        TipoCambioPago = c.TipoCambioPago,

                        // Valoración
                        MetodoValoracion = c.MetodoValoracion ?? "",

                        // Totales
                        TotalPrecioPagado = c.TotalPrecioPagado,
                        TotalPrecioPorPagar = c.TotalPrecioPorPagar,
                        TotalIncrementables = c.TotalIncrementables,
                        TotalDecrementables = c.TotalDecrementables,
                        TotalValorAduana = c.TotalValorAduana,

                        // Auditoría
                        FechaRegistro = DateTime.Now,
                        Activo = true,

                        // =========================================================================
                        // 5. ¡AQUÍ ESTÁ LO QUE FALTABA! -> Los Incrementables (NIETOS)
                        // =========================================================================
                        ManifestacionConceptosValor = c.Incrementables.Select(inc => new ManifestacionConceptosValor
                        {
                            // Mapeamos tus datos del XML a las columnas de tu tabla existente
                            TipoConcepto = "INCREMENTABLE",
                            ClaveConcepto = inc.TipoIncrementable, // Ej: INCRE.GS

                            FechaErogacion = inc.FechaErogacion,
                            Importe = inc.Importe,

                            TipoMoneda = inc.TipoMoneda,
                            TipoCambio = inc.TipoCambio,
                            AcargoImportador = inc.ACargoImportador

                            // IMPORTANTE: 'CoveId' y 'ManifestacionId' se llenan solos automáticamente
                        }).ToList()
                        // =========================================================================

                    }).ToList()
                };

                _context.ManifestacionesValors.Add(nuevaManifestacion);
                await _context.SaveChangesAsync();

                return Ok(new { id = nuevaManifestacion.ManifestacionId, mensaje = "Expediente guardado correctamente." });
            }
            catch (Exception ex)
            {
                var mensaje = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest($"Error al guardar en BD: {mensaje}");
            }
        }


        // DELETE: api/ManifestacionesValor/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManifestacionesValor(int id)
        {
            var manifestacionesValor = await _context.ManifestacionesValors.FindAsync(id);
            if (manifestacionesValor == null)
            {
                return NotFound();
            }

            // Baja Lógica en lugar de DELETE físico (Recomendado)
            manifestacionesValor.Activo = false;
            _context.Entry(manifestacionesValor).State = EntityState.Modified;

            // Si prefieres borrado real, descomenta esto:
            // _context.ManifestacionesValors.Remove(manifestacionesValor);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ManifestacionesValorExists(int id)
        {
            return _context.ManifestacionesValors.Any(e => e.ManifestacionId == id);
        }
    }
}