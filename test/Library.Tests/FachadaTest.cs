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
        /// La instancia de la fachada que se probará.
        /// </summary>
        private Fachada fachada;

        /// <summary>
        /// Repositorios "mock" o "en memoria" que se inyectarán
        /// en la fachada antes de cada prueba.
        /// </summary>
        private IRepoClientes repoClientes;
        private IRepoEtiquetas repoEtiquetas;
        private IRepoUsuarios repoUsuarios;
        private IRepoVentas repoVentas;

        /// <summary>
        /// Prepara el entorno para CADA prueba unitaria.
        /// Este método se ejecuta antes de cada [Test].
        /// 
        /// 1. Crea instancias "frescas" de los repositorios concretos.
        /// 2. Crea una "nueva" instancia de Fachada, inyectando
        ///    dichos repositorios en su constructor (Inyección de Dependencias).
        /// 
        /// Esto garantiza que cada prueba se ejecute en un estado aislado
        /// y limpio, sin interferencia de pruebas anteriores.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            /// <summary>
            /// Asumimos que las implementaciones concretas (RepoClientes, etc.)
            /// tienen constructores públicos y ya no son Singletons,
            /// para coincidir con el diseño DI de la Fachada.
            /// </summary>
            this.repoClientes = new RepoClientes();
            this.repoEtiquetas = new RepoEtiquetas();
            this.repoUsuarios = new RepoUsuarios();
            this.repoVentas = new RepoVentas();

            /// <summary>
            /// Inyectamos las dependencias en la fachada.
            /// </summary>
            this.fachada = new Fachada(this.repoClientes, this.repoEtiquetas, this.repoUsuarios, this.repoVentas);
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

            /// <summary>
            /// Actuamos (Llamamos al método de la fachada)
            /// </summary>
            this.fachada.CrearCliente(nombre, apellido, "099123456", "juan@perez.com", "M", fechaNac);

            /// <summary>
            /// Verificamos (Consultamos a la fachada, que a su vez
            /// consultará al repositorio que le inyectamos).
            /// </summary>
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
            /// <summary>
            /// Arrange: Preparamos el escenario creando un cliente.
            /// </summary>
            this.fachada.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "M", DateTime.Now);
            var cliente = this.fachada.VerTodosLosClientes()[0];
            
            string nuevoNombre = "Juan Modificado";
            string nuevoCorreo = "nuevo@correo.com";

            /// <summary>
            /// Act: Ejecutamos la lógica a probar.
            /// </summary>
            this.fachada.ModificarCliente(cliente.Id, nuevoNombre, cliente.Apellido, cliente.Telefono, 
                                          nuevoCorreo, cliente.Genero, cliente.FechaNacimiento);

            /// <summary>
            /// Assert: Verificamos que los cambios se hayan aplicado.
            /// </summary>
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
            /// <summary>
            /// Arrange
            /// </summary>
            this.fachada.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "M", DateTime.Now);
            Assert.AreEqual(1, this.fachada.VerTodosLosClientes().Count);
            var clienteId = this.fachada.VerTodosLosClientes()[0].Id;

            /// <summary>
            /// Act
            /// </summary>
            this.fachada.EliminarCliente(clienteId);

            /// <summary>
            /// Assert
            /// </summary>
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
            /// <summary>
            /// Arrange
            /// </summary>
            this.fachada.CrearCliente("Juan", "Perez", "111111", "juan@mail.com", "M", DateTime.Now);
            this.fachada.CrearCliente("Juana", "Gonzalez", "222222", "juana@mail.com", "F", DateTime.Now);
            this.fachada.CrearCliente("Pedro", "Gomez", "333333", "pedro@mail.com", "M", DateTime.Now);

            /// <summary>
            /// Act
            /// </summary>
            var resultadoBusqueda = this.fachada.BuscarClientes("Juan");

            /// <summary>
            /// Assert
            /// </summary>
            Assert.AreEqual(2, resultadoBusqueda.Count);
            Assert.IsTrue(resultadoBusqueda.Any(c => c.Nombre == "Juan"));
            Assert.IsTrue(resultadoBusqueda.Any(c => c.Nombre == "Juana"));
        }
        
        /// <summary>
        /// Verifica específicamente que los datos de contacto (teléfono y correo),
        /// requeridos por la historia de usuario, se guarden correctamente.
        /// </summary>
        [Test]
        public void TestCrearCliente_VerificarDatosDeContacto()
        {
            // Arrange
            string nombre = "Maria";
            string apellido = "Rodriguez";
            string telefono = "099888777";
            string correo = "maria.rod@correo.com";
            string genero = "F";
            DateTime fechaNac = new DateTime(1995, 10, 20);

            // Act
            this.fachada.CrearCliente(nombre, apellido, telefono, correo, genero, fechaNac);

            // Assert
            var listaClientes = this.fachada.VerTodosLosClientes();
            
            // Obtenemos el cliente creado usando índice estándar (sin LINQ)
            // Dado que el SetUp limpia el repo, debería estar en la posición 0.
            var clienteCreado = listaClientes[0];

            Assert.AreEqual(telefono, clienteCreado.Telefono, "El teléfono debe coincidir con el ingresado.");
            Assert.AreEqual(correo, clienteCreado.Correo, "El correo debe coincidir con el ingresado.");
        }
    }
}