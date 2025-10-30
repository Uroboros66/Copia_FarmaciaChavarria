using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Models;
using API_FarmaciaChavarria.Models.PaginationModels;
using API_FarmaciaChavarria.ModelsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace API_FarmaciaChavarria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categorias
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaPageResult>>> GetCategoria([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {


            var query = _context.Categorias.AsQueryable();

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var categorias = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new CategoriaPageResult
            {
                Categorias = categorias,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // GET: api/Categorias/5
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> GetCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);

            if (categoria == null)
            {
                return NotFound();
            }

            var categ = new CategoriaDTO
            {
                Id_categoria = categoria.Id_categoria,
                Nombre = categoria.Nombre
            };

            return categ;
        }

        // GET: api/Categorias/nombre/jarabe
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<CategoriaPageResult>> GetCategoriaByNombre(string nombre, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var query = from c in _context.Categorias
                        where c.Nombre.ToLower().Contains(nombre.ToLower())
                        select new Categoria
                        {
                            Id_categoria = c.Id_categoria,
                            Nombre = c.Nombre
                        };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var categorias = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new CategoriaPageResult
            {
                Categorias = categorias,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // PUT: api/Categorias/5
        [EnableRateLimiting("globalLimiter")]
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategoria(int id, CategoriaDTO categoria)
        {
            if (id != categoria.Id_categoria)
            {
                return BadRequest();
            }

            if (categoria.Nombre == "")
            {
                return BadRequest("El campo nombre de categoría no puede estar vacío");
            }

            var categ = new Categoria
            {
                Id_categoria = categoria.Id_categoria,
                Nombre = categoria.Nombre
            };

            _context.Entry(categ).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(id))
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

        // POST: api/Categorias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Categoria>> PostCategoria(CategoriaDTO categoria)
        {
            if (categoria.Nombre == "")
            {
                return BadRequest("El campo nombre de categoría no puede estar vacío");
            }

            var categ = new Categoria
            {
                Id_categoria = categoria.Id_categoria,
                Nombre = categoria.Nombre
            };

            _context.Categorias.Add(categ);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategoria", new { id = categoria.Id_categoria }, categoria);
        }

        // DELETE: api/Categorias/5
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categorias.Any(e => e.Id_categoria == id);
        }
    }
}