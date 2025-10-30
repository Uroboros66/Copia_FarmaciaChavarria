using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_FarmaciaChavarria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetalleFacturasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DetalleFacturasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/DetalleFacturas
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetalleFactura>>> GetDetalleFactura()
        {
            return await _context.Detalle_Facturas.ToListAsync();
        }

        // GET: api/DetalleFacturas/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<DetalleFactura>> GetDetalleFactura(int id)
        {
            var detalleFactura = await _context.Detalle_Facturas.FindAsync(id);

            if (detalleFactura == null)
            {
                return NotFound();
            }

            return detalleFactura;
        }

        // PUT: api/DetalleFacturas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDetalleFactura(int id, DetalleFactura detalleFactura)
        {
            if (id != detalleFactura.Id_detalle)
            {
                return BadRequest();
            }

            if (detalleFactura.Cantidad <= 0)
            {
                return BadRequest("El campo cantidad no puede ser menor o igual que 0");
            }

            if (detalleFactura.Precio_unitario <= 0)
            {
                return BadRequest("El campo precio unitario no puede ser menor o igual que 0");
            }

            _context.Entry(detalleFactura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DetalleFacturaExists(id))
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

        // POST: api/DetalleFacturas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<DetalleFactura>> PostDetalleFactura(DetalleFactura detalleFactura)
        {
            if (detalleFactura.Cantidad <= 0)
            {
                return BadRequest("El campo cantidad no puede ser menor o igual que 0");
            }

            if (detalleFactura.Precio_unitario <= 0)
            {
                return BadRequest("El campo precio unitario no puede ser menor o igual que 0");
            }

            // Buscar el producto
            var producto = await _context.Productos.FindAsync(detalleFactura.Id_producto);

            if (producto == null)
                return NotFound("Producto no encontrado");

            if (producto.Stock < detalleFactura.Cantidad)
                return BadRequest("No hay suficiente stock disponible");

            // Restar la cantidad al stock del producto
            producto.Stock -= detalleFactura.Cantidad;

            _context.Detalle_Facturas.Add(detalleFactura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDetalleFactura", new { id = detalleFactura.Id_detalle }, detalleFactura);
        }

        // DELETE: api/DetalleFacturas/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDetalleFactura(int id)
        {
            var detalleFactura = await _context.Detalle_Facturas.FindAsync(id);
            if (detalleFactura == null)
            {
                return NotFound();
            }

            _context.Detalle_Facturas.Remove(detalleFactura);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DetalleFacturaExists(int id)
        {
            return _context.Detalle_Facturas.Any(e => e.Id_detalle == id);
        }
    }
}