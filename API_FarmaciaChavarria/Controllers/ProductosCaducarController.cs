using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Models;
using API_FarmaciaChavarria.ModelsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_FarmaciaChavarria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosCaducarController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosCaducarController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos_Caducar
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoCaducar>>> GetProductos_Caducar()
        {
            return await _context.Productos_Caducar.ToListAsync();
        }

        // GET: api/Productos_Caducar/5
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoCaducarDTO>> GetProductoCaducar(int id)
        {
            var productoCaducar = await _context.Productos_Caducar.FindAsync(id);

            if (productoCaducar == null)
            {
                return NotFound();
            }

            var prodCad = new ProductoCaducarDTO
            {
                Id_producto = productoCaducar.Id_producto,
                Nombre = productoCaducar.Nombre,
                Fecha_vencimiento = productoCaducar.Fecha_vencimiento
            };

            return prodCad;
        }

        // PUT: api/Productos_Caducar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductoCaducar(int id, ProductoCaducarDTO productoCaducarDTO)
        {

            if (id != productoCaducarDTO.Id_producto)
            {
                return BadRequest();
            }

            var producto = await _context.Productos_Caducar.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            producto.Nombre = productoCaducarDTO.Nombre;
            producto.Fecha_vencimiento = productoCaducarDTO.Fecha_vencimiento;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoCaducarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Productos_Caducar
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ProductoCaducar>> PostProductoCaducar(ProductoCaducarDTO productoCaducarDTO)
        {
            var productoCaducar = new ProductoCaducar
            {
                Id_producto = productoCaducarDTO.Id_producto,
                Fecha_vencimiento = productoCaducarDTO.Fecha_vencimiento,
                Nombre = productoCaducarDTO.Nombre
            };

            _context.Productos_Caducar.Add(productoCaducar);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductoCaducarExists(productoCaducar.Id_producto))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetProductoCaducar", new { id = productoCaducar.Id_producto }, productoCaducar);
        }

        // DELETE: api/Productos_Caducar/5
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductoCaducar(int id)
        {
            var productoCaducar = await _context.Productos_Caducar.FindAsync(id);
            if (productoCaducar == null)
            {
                return NotFound();
            }

            _context.Productos_Caducar.Remove(productoCaducar);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoCaducarExists(int id)
        {
            return _context.Productos_Caducar.Any(e => e.Id_producto == id);
        }
    }
}