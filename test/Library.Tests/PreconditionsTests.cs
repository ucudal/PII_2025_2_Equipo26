using NUnit.Framework;
using Library;
using System;

namespace Library.Tests
{
    [TestFixture]
    public class PreconditionsTests
    {
        [Test]
        public void RepoClientes_Agregar_DeberiaLanzarExcepcion_CuandoNombreEsNulo()
        {
            // Arrange
            var repo = new RepoClientes();
            bool excepcionLanzada = false;

            // Act
            try
            {
                repo.Agregar(null, "Apellido", "123", "mail", "M", DateTime.Now);
            }
            catch (ArgumentException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }

        [Test]
        public void RepoClientes_Agregar_DeberiaLanzarExcepcion_CuandoNombreEsVacio()
        {
            // Arrange
            var repo = new RepoClientes();
            bool excepcionLanzada = false;

            // Act
            try
            {
                repo.Agregar("", "Apellido", "123", "mail", "M", DateTime.Now);
            }
            catch (ArgumentException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }

        [Test]
        public void RepoClientes_Modificar_DeberiaLanzarExcepcion_CuandoNombreEsNulo()
        {
            // Arrange
            var repo = new RepoClientes();
            bool excepcionLanzada = false;

            // Act
            try
            {
                repo.Modificar(1, null, "Apellido", "123", "mail", "M", DateTime.Now);
            }
            catch (ArgumentException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }

        [Test]
        public void Usuario_Constructor_DeberiaLanzarExcepcion_CuandoNombreEsNulo()
        {
            // Arrange
            bool excepcionLanzada = false;

            // Act
            try
            {
                new Usuario(null, Rol.Vendedor);
            }
            catch (ArgumentException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }

        [Test]
        public void Cliente_Constructor_DeberiaLanzarExcepcion_CuandoNombreEsNulo()
        {
            // Arrange
            bool excepcionLanzada = false;

            // Act
            try
            {
                new Cliente(null, "Apellido", "123", "mail", "M", DateTime.Now);
            }
            catch (ArgumentException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }

        [Test]
        public void Cliente_AgregarVenta_DeberiaLanzarExcepcion_CuandoVentaEsNula()
        {
            // Arrange
            var cliente = new Cliente("Nombre", "Apellido", "123", "mail", "M", DateTime.Now);
            bool excepcionLanzada = false;

            // Act
            try
            {
                cliente.AgregarVenta(null);
            }
            catch (ArgumentNullException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }

        [Test]
        public void Venta_Constructor_DeberiaLanzarExcepcion_CuandoProductoEsNulo()
        {
            // Arrange
            bool excepcionLanzada = false;

            // Act
            try
            {
                new Venta(1, null, 100, DateTime.Now);
            }
            catch (ArgumentException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }

        [Test]
        public void Venta_Constructor_DeberiaLanzarExcepcion_CuandoImporteEsNegativo()
        {
            // Arrange
            bool excepcionLanzada = false;

            // Act
            try
            {
                new Venta(1, "Producto", -10, DateTime.Now);
            }
            catch (ArgumentException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }
        
        [Test]
        public void RepoBase_Agregar_DeberiaLanzarExcepcion_CuandoItemEsNulo()
        {
            // Arrange
            var repoDummy = new RepoDummy();
            bool excepcionLanzada = false;

            // Act
            try
            {
                repoDummy.Agregar(null);
            }
            catch (ArgumentNullException)
            {
                excepcionLanzada = true;
            }

            // Assert
            Assert.IsTrue(excepcionLanzada);
        }

        private class DummyEntity : IEntidad
        {
            public int Id { get; set; }
        }

        private class RepoDummy : RepoBase<DummyEntity>
        {
            // Exponemos el método para probarlo
            public new void Agregar(DummyEntity item)
            {
                base.Agregar(item);
            }
        }
    }
}
