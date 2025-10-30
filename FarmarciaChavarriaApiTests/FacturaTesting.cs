using API_FarmaciaChavarria.Context;
using API_FarmaciaChavarria.Controllers;
using API_FarmaciaChavarria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmarciaChavarriaApiTests
{
    public class FacturaTesting
    {
        private static AppDbContext GetDbContextConDatosPrueba()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);

            // Datos de prueba
            context.Facturas.AddRange(
                new Factura { Id_factura = 1, Id_usuario = 1, Fecha_venta = DateTime.Today, Total = 1000 },
                new Factura { Id_factura = 2, Id_usuario = 2, Fecha_venta = DateTime.Today, Total = 2000 },
                new Factura { Id_factura = 3, Id_usuario = 2, Fecha_venta = DateTime.Today, Total = 3000 }
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
        public async Task GetFacturas_DeberiaRetornarUnaListaDeFacturas()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new FacturasController(context);

            // Act
            var result = await controller.GetFacturas();

            // Assert
            var facturas = Assert.IsType<List<Factura>>(result.Value);
            Assert.NotEmpty(facturas);
        }

        [Fact]
        public async Task GetFacturas_DeberiaRetornarListaVaciaCuandoNoHayFacturas()
        {
            // Arrange
            var context = GetDbContextSinDatos(); // DB sin datos
            var controller = new FacturasController(context);

            // Act
            var result = await controller.GetFacturas();

            // Assert
            var facturas = Assert.IsType<List<Factura>>(result.Value);

            Assert.Empty(facturas);
        }

        [Fact]
        public async Task GetFactura_DeberiaRetornarUnaFacturaFiltradoPorId()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new FacturasController(context);

            // Act
            var result = await controller.GetFactura(1);

            // Assert
            var factura = Assert.IsType<Factura>(result.Value);
            Assert.Equal(1, factura.Id_factura);

        }

        [Fact]
        public async Task GetFactura_DeberiaRetornarUnNotFound()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new FacturasController(context);

            // Act
            var result = await controller.GetFactura(10);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task PostFactura_DeberiaCrearFacturaYRetornarCreatedAtAction()
        {
            // Arrange
            var context = GetDbContextSinDatos(); // DB vacía
            var controller = new FacturasController(context);
            var nuevaFactura = new Factura { Id_factura = 10, Id_usuario = 1, Fecha_venta = DateTime.Today, Total = 1000 };

            // Act
            var result = await controller.PostFactura(nuevaFactura);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var facturaDevuelta = Assert.IsType<Factura>(createdResult.Value);
            Assert.Equal(nuevaFactura.Id_factura, facturaDevuelta.Id_factura);

            var facturaEnDb = await context.Facturas.FindAsync(facturaDevuelta.Id_factura);
            Assert.NotNull(facturaEnDb);
        }

        [Fact]
        public async Task PutFactura_DeberiaActualizarFacturaYRetornarNoContent()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();

            // Desanclar manualmente la entidad que se insertó en GetDbContextConDatosPrueba
            var local = context.Facturas.Local.FirstOrDefault(c => c.Id_factura == 1);
            if (local != null)
                context.Entry(local).State = EntityState.Detached;

            var controller = new FacturasController(context);
            var facturaActualizada = new Factura { Id_factura = 1, Id_usuario = 1, Fecha_venta = DateTime.Today, Total = 1000 };

            // Act
            var result = await controller.PutFactura(1, facturaActualizada);

            // Assert
            Assert.IsType<NoContentResult>(result);

            var facturaEnDb = await context.Facturas.FindAsync(1);
            Assert.NotNull(facturaEnDb);
        }

        [Fact]
        public async Task PutFactura_DeberiaRetornarBadRequestSiIdNoCoincide()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();

            var local = context.Facturas.Local.FirstOrDefault(c => c.Id_factura == 1);

            var controller = new FacturasController(context);
            var facturaConOtroId = new Factura { Id_factura = 99, Id_usuario = 1, Fecha_venta = DateTime.Today, Total = 1000 };

            // Act
            var result = await controller.PutFactura(1, facturaConOtroId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task DeleteFactura_DeberiaEliminarFacturaYRetornarNoContent()
        {
            // Arrange
            var context = GetDbContextConDatosPrueba();
            var controller = new FacturasController(context);

            // Act
            var result = await controller.DeleteFactura(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Null(await context.Facturas.FindAsync(1));
        }

        [Fact]
        public async Task DeleteFacturas_DeberiaRetornarNotFoundSiNoExiste()
        {
            // Arrange
            var context = GetDbContextSinDatos();
            var controller = new FacturasController(context);

            // Act
            var result = await controller.DeleteFactura(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
