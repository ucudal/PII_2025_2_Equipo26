using NUnit.Framework;
using Library;
using System;
using System.Linq;

namespace Library.Tests
{
    /// <summary>
    /// Pruebas unitarias para los métodos de la Fachada relacionados
    /// con la gestión de Clientes (Crear, Modificar, Eliminar, Buscar).
    /// </summary>
    [TestFixture]
    public class FachadaTest
    {
        /// <summary>
        /// Instancia de la fachada (Singleton) que se utilizará en todas las pruebas.
        /// </summary>
        private Fachada fachada;

        /// <summary>
        /// Prepara el entorno para CADA prueba unitaria.
        /// Este método se ejecuta antes de cada [Test].
        /// Obtiene la instancia de la Fachada y resetea todos los repositorios
        /// para garantizar el aislamiento de la prueba (evitar que una
        /// prueba afecte el resultado de otra).
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.fachada = Fachada.Instancia;

            // Gracias a InternalsVisibleTo, podemos invocar Reset()
            RepoClientes.Instancia.Reset();
            RepoUsuarios.Instancia.Reset();
            RepoEtiquetas.Instancia.Reset();
            RepoVentas.Instancia.Reset();
        }

        /// <summary>
        /// Verifica que se pueda crear un cliente correctamente.
        /// Comprueba que el cliente se persiste en el repositorio
        /// y que sus datos (nombre, apellido, ID) son correctos.
        /// </summary>
        [Test]
        public void TestCrearCliente()
        {
            string nombre = "Juan";
            string apellido = "Perez";
            DateTime fechaNac = new DateTime(1990, 5, 15);

            this.fachada.CrearCliente(nombre, apellido, "099123456", "juan@perez.com", "M", fechaNac);

            var clientes = this.fachada.VerTodosLosClientes();
            
            Assert.AreEqual(1, clientes.Count);
            Assert.AreEqual(nombre, clientes[0].Nombre);
            Assert.AreEqual(apellido, clientes[0].Apellido);
            Assert.AreEqual(fechaNac, clientes[0].FechaNacimiento);
            Assert.AreEqual(1, clientes[0].Id); // El primer ID debe ser 1
        }

        /// <summary>
        /// Verifica que los datos de un cliente existente se puedan modificar
        /// correctamente.
        /// </summary>
        [Test]
        public void TestModificarCliente()
        {
            // Primero creamos un cliente para modificar
            this.fachada.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "M", DateTime.Now);
            var cliente = this.fachada.VerTodosLosClientes()[0];
            
            string nuevoNombre = "Juan Modificado";
            string nuevoCorreo = "nuevo@correo.com";

            // Actuamos
            this.fachada.ModificarCliente(cliente.Id, nuevoNombre, cliente.Apellido, cliente.Telefono, 
                                          nuevoCorreo, cliente.Genero, cliente.FechaNacimiento);

            // Verificamos
            var clienteModificado = this.fachada.BuscarCliente(cliente.Id);
            Assert.IsNotNull(clienteModificado);
            Assert.AreEqual(nuevoNombre, clienteModificado.Nombre);
            Assert.AreEqual(nuevoCorreo, clienteModificado.Correo);
        }

        /// <summary>
        /// Verifica que un cliente pueda ser eliminado del sistema
        /// usando su ID.
        /// </summary>
        [Test]
        public void TestEliminarCliente()
        {
            this.fachada.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "M", DateTime.Now);
            Assert.AreEqual(1, this.fachada.VerTodosLosClientes().Count);
            var clienteId = this.fachada.VerTodosLosClientes()[0].Id;

            this.fachada.EliminarCliente(clienteId);

            Assert.AreEqual(0, this.fachada.VerTodosLosClientes().Count);
            Assert.IsNull(this.fachada.BuscarCliente(clienteId));
        }

        /// <summary>
        /// Prueba la funcionalidad de búsqueda de clientes.
        /// Verifica que el método devuelva todos los clientes
        /// que coincidan parcialmente con el término de búsqueda.
        /// </summary>
        [Test]
        public void TestBuscarClientes()
        {
            this.fachada.CrearCliente("Juan", "Perez", "111111", "juan@mail.com", "M", DateTime.Now);
            this.fachada.CrearCliente("Juana", "Gonzalez", "222222", "juana@mail.com", "F", DateTime.Now);
            this.fachada.CrearCliente("Pedro", "Gomez", "333333", "pedro@mail.com", "M", DateTime.Now);

            var resultadoBusqueda = this.fachada.BuscarClientes("Juan");

            Assert.AreEqual(2, resultadoBusqueda.Count);
            Assert.IsTrue(resultadoBusqueda.Any(c => c.Nombre == "Juan"));
            Assert.IsTrue(resultadoBusqueda.Any(c => c.Nombre == "Juana"));
        }
    }
}