using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Controllers;
using API_FarmaciaChavarria.Models;
using API_FarmaciaChavarria.ModelsDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmarciaChavarriaApiTests
{
    public class ProductoCaducarTesting
    {
        private static AppDbContext GetDbContextConDatosPrueba()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            // Datos de prueba
            context.Productos_Caducar.AddRange(
                new ProductoCaducar { Id_producto = 1, Fecha_vencimiento = new DateOnly(2026, 12, 31), Nombre = "Acetaminofen" },
                new ProductoCaducar { Id_producto = 2, Fecha_vencimiento = new DateOnly(2026, 12, 31), Nombre = "Ibuprofeno" },
                new ProductoCaducar { Id_producto = 3, Fecha_vencimiento = new DateOnly(2026, 12, 31), Nombre = "Loratadina" }
            );

            context.SaveChanges();

            return context;
        }

        private static AppDbContext GetDbContextSinDatos()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // Para que cada test tenga su propia DB
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetProductos_Caducar_DeberiaRetornarUnaListaDeProductos()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProductosCaducarController(context);

            // Act
            var result = await controller.GetProductos_Caducar();

            // Assert
            var productos = Assert.IsType<List<ProductoCaducar>>(result.Value);
            Assert.NotEmpty(productos);
        }

        [Fact]
        public async Task GetProductos_Caducar_DeberiaRetornarListaVaciaCuandoNoHayProductos()
        {
            // Arrange
            var context = GetDbContextSinDatos(); // DB sin datos
            var controller = new ProductosCaducarController(context);

            // Act
            var result = await controller.GetProductos_Caducar();

            // Assert
            var productos = Assert.IsType<List<ProductoCaducar>>(result.Value);

            Assert.Empty(productos);
        }

        [Fact]
        public async Task GetProductoCaducar_DeberiaRetornarUnProductoFiltradoPorId()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProductosCaducarController(context);

            // Act
            var result = await controller.GetProductoCaducar(1);

            // Assert
            var producto = Assert.IsType<ProductoCaducarDTO>(result.Value);
            Assert.Equal(1, producto.Id_producto);

        }

        [Fact]
        public async Task GetProductoCaducar_DeberiaRetornarUnNotFound()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProductosCaducarController(context);

            // Act
            var result = await controller.GetProductoCaducar(10);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }

        [Fact]
        public async Task PostProductoCaducar_DeberiaCrearProductoYRetornarCreatedAtAction()
        {
            // Arrange
            var context = GetDbContextSinDatos(); // DB vacía
            var controller = new ProductosCaducarController(context);
            var nuevoProductoCaducar = new ProductoCaducarDTO { Id_producto = 1, Fecha_vencimiento = new DateOnly(2026, 12, 31), Nombre = "Vitaflenaco" };

            // Act
            var result = await controller.PostProductoCaducar(nuevoProductoCaducar);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var productoCaducarDevuelto = Assert.IsType<ProductoCaducar>(createdResult.Value);
            Assert.Equal(nuevoProductoCaducar.Id_producto, productoCaducarDevuelto.Id_producto);

            var productoCaducarEnDb = await context.Productos_Caducar.FindAsync(productoCaducarDevuelto.Id_producto);
            Assert.NotNull(productoCaducarEnDb);
        }

        [Fact]
        public async Task PutProductoCaducar_DeberiaActualizarProductoYRetornarNoContent()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();

            // Desanclar manualmente la entidad que se insertó en GetDbContextConDatosPrueba
            var local = context.Productos_Caducar.Local.FirstOrDefault(c => c.Id_producto == 1);
            if (local != null)
                context.Entry(local).State = EntityState.Detached;

            var controller = new ProductosCaducarController(context);
            var productoCaducarActualizado = new ProductoCaducarDTO { Id_producto = 1, Fecha_vencimiento = new DateOnly(2026, 12, 31), Nombre = "Vitaflenaco" };

            // Act
            var result = await controller.PutProductoCaducar(1, productoCaducarActualizado);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var productoCaducarEnDb = await context.Productos_Caducar.FindAsync(1);
            Assert.NotNull(productoCaducarEnDb);
        }

        [Fact]
        public async Task PutProductoCaducar_DeberiaRetornarBadRequestSiIdNoCoincide()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();

            var local = context.Productos_Caducar.Local.FirstOrDefault(c => c.Id_producto == 1);

            var controller = new ProductosCaducarController(context);
            var productoCaducarConOtroId = new ProductoCaducarDTO { Id_producto = 10, Fecha_vencimiento = new DateOnly(2026, 12, 31), Nombre = "Vitaflenaco" };

            // Act
            var result = await controller.PutProductoCaducar(1, productoCaducarConOtroId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutProductoCaducar_DeberiaRetornarNotFoundSiProductoNoExiste()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba(); // Solo tiene productos con id 1, 2 y 3

            var controller = new ProductosCaducarController(context);
            var productoCaducarInexistente = new ProductoCaducarDTO
            {
                Id_producto = 999, // Este id no existe en la base de datos
                Fecha_vencimiento = new DateOnly(2026, 12, 31),
                Nombre = "Producto Fantasma"
            };

            // Act
            var result = await controller.PutProductoCaducar(999, productoCaducarInexistente);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task DeleteProductoCaducar_DeberiaEliminarProductoCaducarYRetornarNoContent()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProductosCaducarController(context);

            // Act
            var result = await controller.DeleteProductoCaducar(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Productos_Caducar.FindAsync(1));
        }

        [Fact]
        public async Task DeleteProductoCaducar_DeberiaRetornarNotFoundSiNoExiste()
        {
            // Arrange
            var context = GetDbContextSinDatos();
            var controller = new ProductosCaducarController(context);

            // Act
            var result = await controller.DeleteProductoCaducar(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
