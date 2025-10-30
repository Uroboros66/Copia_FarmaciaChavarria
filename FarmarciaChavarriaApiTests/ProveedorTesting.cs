using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Controllers;
using API_FarmaciaChavarria.Models;
using API_FarmaciaChavarria.Models.PaginationModels;
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
    public class ProveedorTesting
    {
        private static AppDbContext GetDbContextConDatosPrueba()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            // Datos de prueba
            context.Proveedores.AddRange(
                new Proveedor { Id_proveedor = 1, Nombre = "JomWest", Telefono = "99228799", Direccion = "Parque" },
                new Proveedor { Id_proveedor = 2, Nombre = "Horell IsReal", Telefono = "22334455", Direccion = "Colina Sur" },
                new Proveedor { Id_proveedor = 3, Nombre = "Yared", Telefono = "77889966", Direccion = "Cotran Norte" }
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
        public async Task GetProveedores_DeberiaRetornarUnaListaDeProveedores()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProveedorsController(context);

            // Act
            var result = await controller.GetProveedores();

            // Assert
            var proveedores = Assert.IsType<List<Proveedor>>(result.Value);
            Assert.NotEmpty(proveedores);
        }

        [Fact]
        public async Task GetProveedores_DeberiaRetornarListaVaciaCuandoNoHayProveedores()
        {
            // Arrange
            var context = GetDbContextSinDatos(); // DB sin datos
            var controller = new ProveedorsController(context);

            // Act
            var result = await controller.GetProveedores();

            // Assert
            var proveedores = Assert.IsType<List<Proveedor>>(result.Value);

            Assert.Empty(proveedores);
        }

        [Fact]
        public async Task GetProveedor_DeberiaRetornarUnProveedorFiltradoPorId()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProveedorsController(context);

            // Act
            var result = await controller.GetProveedor(1);

            // Assert
            var proveedor = Assert.IsType<ProveedorDTO>(result.Value);
            Assert.Equal(1, proveedor.Id_proveedor);

        }

        [Fact]
        public async Task GetProveedor_DeberiaRetornarUnNotFound()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProveedorsController(context);

            // Act
            var result = await controller.GetProveedor(10);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }

        [Fact]
        public async Task PostProveedor_DeberiaCrearProveedorYRetornarCreatedAtAction()
        {
            // Arrange
            var context = GetDbContextSinDatos(); // DB vacía
            var controller = new ProveedorsController(context);
            var nuevoProveedor = new ProveedorDTO { Id_proveedor = 10, Nombre = "Katerina", Telefono = "99880055", Direccion = "La Cuesta" };

            // Act
            var result = await controller.PostProveedor(nuevoProveedor);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var proveedorDevuelto = Assert.IsType<Proveedor>(createdResult.Value);
            Assert.Equal(nuevoProveedor.Id_proveedor, proveedorDevuelto.Id_proveedor);

            var proveedorEnDb = await context.Proveedores.FindAsync(nuevoProveedor.Id_proveedor);
            Assert.NotNull(proveedorEnDb);
            Assert.Equal("Katerina", proveedorEnDb.Nombre);
        }

        [Fact]
        public async Task PutProveedor_DeberiaActualizarProveedorYRetornarNoContent()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();

            // Desanclar manualmente la entidad que se insertó en GetDbContextConDatosPrueba
            var local = context.Proveedores.Local.FirstOrDefault(c => c.Id_proveedor == 1);
            if (local != null)
                context.Entry(local).State = EntityState.Detached;

            var controller = new ProveedorsController(context);
            var proveedorActualizado = new ProveedorDTO { Id_proveedor = 1, Nombre = "YuanGarcia", Telefono = "11335577", Direccion = "Por hay" };

            // Act
            var result = await controller.PutProveedor(1, proveedorActualizado);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var proveedorEnDb = await context.Proveedores.FindAsync(1);
            Assert.Equal("YuanGarcia", proveedorEnDb.Nombre);
        }

        [Fact]
        public async Task PutProveedor_DeberiaRetornarBadRequestSiIdNoCoincide()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProveedorsController(context);
            var proveedorConOtroId = new ProveedorDTO { Id_proveedor = 99, Nombre = "Yuanes" };

            // Act
            var result = await controller.PutProveedor(1, proveedorConOtroId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteProveedor_DeberiaEliminarProveedorYRetornarNoContent()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new ProveedorsController(context);

            // Act
            var result = await controller.DeleteProveedor(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Proveedores.FindAsync(1));
        }

        [Fact]
        public async Task DeleteProveedor_DeberiaRetornarNotFoundSiNoExiste()
        {
            // Arrange
            var context = GetDbContextSinDatos();
            var controller = new ProveedorsController(context);

            // Act
            var result = await controller.DeleteProveedor(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
