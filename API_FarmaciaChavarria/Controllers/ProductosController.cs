using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Models;
using API_FarmaciaChavarria.Models.PaginationModels;
using API_FarmaciaChavarria.ModelsDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace API_FarmaciaChavarria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Productos
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ProductoPagedResult>> GetProductos([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var query = from p in _context.Productos
                        join c in _context.Categorias on p.Id_categoria equals c.Id_categoria
                        join l in _context.Laboratorios on p.Id_laboratorio equals l.Id_laboratorio
                        select new ProductoDetailedDTO
                        {
                            IdProducto = p.Id_producto,
                            Nombre = p.Nombre,
                            Id_categoria = p.Id_categoria,
                            Id_laboratorio = p.Id_laboratorio,
                            CategoriaNombre = c.Nombre,
                            LaboratorioNombre = l.Nombre,
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Stock_Minimo = p.Stock_minimo,
                            Efectos_secundarios = p.Efectos_secundarios,
                            Como_usar = p.Como_usar,
                            FechaVencimiento = p.Fecha_vencimiento
                        };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new ProductoPagedResult
            {
                Productos = productos,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // GET: api/Productos
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("medicamentos-escasos")]
        public async Task<ActionResult<ProductoPagedResult>> GetProductosStockEscaso([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var query = from p in _context.Productos

                        join c in _context.Categorias on p.Id_categoria equals c.Id_categoria
                        join l in _context.Laboratorios on p.Id_laboratorio equals l.Id_laboratorio
                        where p.Stock_minimo >= p.Stock
                        select new ProductoDetailedDTO
                        {
                            IdProducto = p.Id_producto,
                            Nombre = p.Nombre,
                            Id_categoria = p.Id_categoria,
                            Id_laboratorio = p.Id_laboratorio,
                            CategoriaNombre = c.Nombre,
                            LaboratorioNombre = l.Nombre,
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Stock_Minimo = p.Stock_minimo,
                            Efectos_secundarios = p.Efectos_secundarios,
                            Como_usar = p.Como_usar,
                            FechaVencimiento = p.Fecha_vencimiento
                        };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new ProductoPagedResult
            {
                Productos = productos,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }


        // GET: api/Productos/5
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDetailedDTO>> GetProducto(int id)
        {
            var producto = await (from p in _context.Productos
                                  join c in _context.Categorias on p.Id_categoria equals c.Id_categoria
                                  join l in _context.Laboratorios on p.Id_laboratorio equals l.Id_laboratorio
                                  where p.Id_producto == id
                                  select new ProductoDetailedDTO
                                  {
                                      IdProducto = p.Id_producto,
                                      Nombre = p.Nombre,
                                      Id_categoria = p.Id_categoria,
                                      Id_laboratorio = p.Id_laboratorio,
                                      CategoriaNombre = c.Nombre,
                                      LaboratorioNombre = l.Nombre,
                                      Precio = p.Precio,
                                      Stock = p.Stock,
                                      Stock_Minimo = p.Stock_minimo,
                                      Efectos_secundarios = p.Efectos_secundarios,
                                      Como_usar = p.Como_usar,
                                      FechaVencimiento = p.Fecha_vencimiento
                                  }).FirstOrDefaultAsync();

            if (producto == null)
            {
                return NotFound();
            }

            return producto;
        }

        // GET: api/Productos/nombre/ibuprofeno
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("nombre/{nombre}")]
        public async Task<ActionResult<ProductoPagedResult>> GetProductoByName(string nombre, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var query = from p in _context.Productos
                        join c in _context.Categorias on p.Id_categoria equals c.Id_categoria
                        join l in _context.Laboratorios on p.Id_laboratorio equals l.Id_laboratorio
                        where p.Nombre.ToLower().Contains(nombre.ToLower())
                        select new ProductoDetailedDTO
                        {
                            IdProducto = p.Id_producto,
                            Nombre = p.Nombre,
                            Id_categoria = p.Id_categoria,
                            Id_laboratorio = p.Id_laboratorio,
                            CategoriaNombre = c.Nombre,
                            LaboratorioNombre = l.Nombre,
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Stock_Minimo = p.Stock_minimo,
                            Efectos_secundarios = p.Efectos_secundarios,
                            Como_usar = p.Como_usar,
                            FechaVencimiento = p.Fecha_vencimiento
                        };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new ProductoPagedResult
            {
                Productos = productos,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // GET: api/Productos/categoria/1
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("categoria/{id}")]
        public async Task<ActionResult<ProductoPagedResult>> GetProductoByCategory(int id, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {

            var query = from p in _context.Productos
                        join c in _context.Categorias on p.Id_categoria equals c.Id_categoria
                        join l in _context.Laboratorios on p.Id_laboratorio equals l.Id_laboratorio
                        where p.Id_categoria == id
                        select new ProductoDetailedDTO
                        {
                            IdProducto = p.Id_producto,
                            Nombre = p.Nombre,
                            Id_categoria = p.Id_categoria,
                            Id_laboratorio = p.Id_laboratorio,
                            CategoriaNombre = c.Nombre,
                            LaboratorioNombre = l.Nombre,
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Stock_Minimo = p.Stock_minimo,
                            Efectos_secundarios = p.Efectos_secundarios,
                            Como_usar = p.Como_usar,
                            FechaVencimiento = p.Fecha_vencimiento
                        };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new ProductoPagedResult
            {
                Productos = productos,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // GET: api/Productos/categoría/1/nombre/ibuprofeno
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("categoria/{id}/nombre/{nombre}")]
        public async Task<ActionResult<ProductoPagedResult>> GetProductoByNameAndByCategory(int id, string nombre, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {

            var query = from p in _context.Productos
                        join c in _context.Categorias on p.Id_categoria equals c.Id_categoria
                        join l in _context.Laboratorios on p.Id_laboratorio equals l.Id_laboratorio
                        where p.Id_categoria == id && p.Nombre.ToLower().Contains(nombre.ToLower())
                        select new ProductoDetailedDTO
                        {
                            IdProducto = p.Id_producto,
                            Nombre = p.Nombre,
                            Id_categoria = p.Id_categoria,
                            Id_laboratorio = p.Id_laboratorio,
                            CategoriaNombre = c.Nombre,
                            LaboratorioNombre = l.Nombre,
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Stock_Minimo = p.Stock_minimo,
                            Efectos_secundarios = p.Efectos_secundarios,
                            Como_usar = p.Como_usar,
                            FechaVencimiento = p.Fecha_vencimiento
                        };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new ProductoPagedResult
            {
                Productos = productos,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpGet("productosPorCadudar")]
        public async Task<ActionResult<ProductoPagedResult>> GetProductosPorCadudar([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 8)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Now);
            var dentroDeTresMeses = hoy.AddMonths(3);

            var query = from p in _context.Productos
                        join c in _context.Categorias on p.Id_categoria equals c.Id_categoria
                        join l in _context.Laboratorios on p.Id_laboratorio equals l.Id_laboratorio
                        where p.Fecha_vencimiento >= hoy && p.Fecha_vencimiento <= dentroDeTresMeses
                        orderby p.Fecha_vencimiento ascending
                        select new ProductoDetailedDTO
                        {
                            IdProducto = p.Id_producto,
                            Nombre = p.Nombre,
                            Id_categoria = p.Id_categoria,
                            Id_laboratorio = p.Id_laboratorio,
                            CategoriaNombre = c.Nombre,
                            LaboratorioNombre = l.Nombre,
                            Precio = p.Precio,
                            Stock = p.Stock,
                            Stock_Minimo = p.Stock_minimo,
                            Efectos_secundarios = p.Efectos_secundarios,
                            Como_usar = p.Como_usar,
                            FechaVencimiento = p.Fecha_vencimiento
                        };

            var totalItems = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var productos = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var result = new ProductoPagedResult
            {
                Productos = productos,
                TotalItems = totalItems,
                TotalPages = totalPages,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // PUT: api/Productos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableRateLimiting("globalLimiter")]
        [Authorize(Roles = "Administrador")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, ProductoDTO productoDTO)
        {

            if (productoDTO.Nombre == "")
            {
                return BadRequest("El campo nombre de producto no puede estar vacío");
            }

            if (productoDTO.Stock < 0)
            {
                return BadRequest("El campo stock no puede ser menor que 0");
            }

            if (productoDTO.Stock_minimo <= 0)
            {
                return BadRequest("El campo stock mínimo no puede ser menor o igual que 0");
            }

            if (productoDTO.Precio <= 0)
            {
                return BadRequest("El campo precio no puede ser menor o igual que 0");
            }

            if (productoDTO.Efectos_secundarios.Length > 500)
            {
                return UnprocessableEntity("El campo efectos secundarios no debe superar los 500 caracteres");
            }

            if (productoDTO.Como_usar.Length > 500)
            {
                return UnprocessableEntity("El campo 'como usar' no debe superar los 500 caracteres");
            }

            var producto = new Producto
            {
                Id_producto = productoDTO.Id_producto,
                Nombre = productoDTO.Nombre,
                Id_categoria = productoDTO.Id_categoria,
                Id_laboratorio = productoDTO.Id_laboratorio,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
                Stock_minimo = productoDTO.Stock_minimo,
                Efectos_secundarios = productoDTO.Efectos_secundarios,
                Como_usar = productoDTO.Como_usar,
                Fecha_vencimiento = productoDTO.Fecha_vencimiento
            };

            if (id != producto.Id_producto)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
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

        // POST: api/Productos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Producto>> PostProducto(ProductoDTO productoDTO)
        {
            if (productoDTO.Nombre == "")
            {
                return BadRequest("El campo nombre de producto no puede estar vacío");
            }

            if (productoDTO.Stock < 0)
            {
                return BadRequest("El campo stock no puede ser menor que 0");
            }

            if (productoDTO.Stock_minimo <= 0)
            {
                return BadRequest("El campo stock mínimo no puede ser menor o igual que 0");
            }

            if (productoDTO.Precio <= 0)
            {
                return BadRequest("El campo precio no puede ser menor o igual que 0");
            }

            if (productoDTO.Efectos_secundarios.Length > 500)
            {
                return BadRequest("El campo efectos secundarios no debe superar los 500 caracteres");
            }

            if (productoDTO.Como_usar.Length > 500)
            {
                return UnprocessableEntity("El campo 'como usar' no debe superar los 500 caracteres");
            }

            if (productoDTO.Fecha_vencimiento < DateOnly.FromDateTime(DateTime.Today))
            {
                return UnprocessableEntity("La fecha de vencimiento no puede ser menor que la fecha actual");
            }

            var producto = new Producto
            {
                Nombre = productoDTO.Nombre,
                Id_categoria = productoDTO.Id_categoria,
                Id_laboratorio = productoDTO.Id_laboratorio,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
                Stock_minimo = productoDTO.Stock_minimo,
                Fecha_vencimiento = productoDTO.Fecha_vencimiento,
                Efectos_secundarios = productoDTO.Efectos_secundarios,
                Como_usar = productoDTO.Como_usar,
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProducto", new { id = producto.Id_producto }, producto);
        }

        // DELETE: api/Productos/5
        [EnableRateLimiting("globalLimiter")]
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.Id_producto == id);
        }
    }
}