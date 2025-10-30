using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Models;
using API_FarmaciaChavarria.Models.PaginationModels;
using API_FarmaciaChavarria.ModelsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API_FarmaciaChavarria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaboratoriosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LaboratoriosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Laboratorios
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<LaboratorioPagedResult>> GetLaboratorio([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            var query = _context.Laboratorios.AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var laboratorios = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new LaboratorioPagedResult
            {
                Laboratorios = laboratorios,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // GET: api/Laboratorios/5
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<LaboratorioDTO>> GetLaboratorio(int id)
        {
            var laboratorio = await _context.Laboratorios.FindAsync(id);

            if (laboratorio == null)
            {
                return NotFound();
            }

            var laboratorioDTO = new LaboratorioDTO
            {
                Id_laboratorio = laboratorio.Id_laboratorio,
                Nombre = laboratorio.Nombre
            };

            return laboratorioDTO;
        }

        // GET: api/Laboratorios/nombre/galo
        [Authorize]
        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<LaboratorioPagedResult>> GetLaboratorioByNombre(string nombre, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 2)
        {
            var query = from c in _context.Laboratorios
                        where c.Nombre.ToLower().Contains(nombre.ToLower())
                        select new Laboratorio
                        {
                            Id_laboratorio = c.Id_laboratorio,
                            Nombre = c.Nombre
                        };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var laboratorios = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new LaboratorioPagedResult
            {
                Laboratorios = laboratorios,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // PUT: api/Laboratorios/5

        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLaboratorio(int id, LaboratorioDTO laboratorioDTO)
        {

            if (laboratorioDTO.Nombre == "")
            {
                return BadRequest("El campo nombre de laboratorio no puede estar vacío");
            }

            var laboratorio = new Laboratorio
            {
                Id_laboratorio = laboratorioDTO.Id_laboratorio,
                Nombre = laboratorioDTO.Nombre
            };


            if (id != laboratorio.Id_laboratorio)
            {
                return BadRequest();
            }

            _context.Entry(laboratorio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LaboratorioExists(id))
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

        // POST: api/Laboratorios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Laboratorio>> PostLaboratorio(LaboratorioDTO LaboratorioDTO)
        {
            if (LaboratorioDTO.Nombre == "")
            {
                return BadRequest("El campo nombre de laboratorio no puede estar vacío");
            }

            var laboratorio = new Laboratorio
            {
                Id_laboratorio = LaboratorioDTO.Id_laboratorio,
                Nombre = LaboratorioDTO.Nombre
            };

            _context.Laboratorios.Add(laboratorio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLaboratorio", new { id = laboratorio.Id_laboratorio }, laboratorio);
        }

        // DELETE: api/Laboratorios/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLaboratorio(int id)
        {
            var laboratorio = await _context.Laboratorios.FindAsync(id);
            if (laboratorio == null)
            {
                return NotFound();
            }

            _context.Laboratorios.Remove(laboratorio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LaboratorioExists(int id)
        {
            return _context.Laboratorios.Any(e => e.Id_laboratorio == id);
        }
    }
}