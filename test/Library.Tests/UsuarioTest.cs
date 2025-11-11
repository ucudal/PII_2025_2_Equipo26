using NUnit.Framework;
using System;
using System.Linq;

namespace Library.Tests
{
    [TestFixture]
    public class UsuarioTest
    {
        private Fachada fachada;

       [SetUp]
        public void Setup()
        {
            // 1. Creamos las implementaciones reales de los repositorios
            IRepoClientes repoClientes = new RepoClientes();
            IRepoEtiquetas repoEtiquetas = new RepoEtiquetas();
            IRepoUsuarios repoUsuarios = new RepoUsuarios();
            IRepoVentas repoVentas = new RepoVentas();

            // 2. Inyectamos los repositorios en el constructor de la Fachada
            fachada = new Fachada(repoClientes, repoEtiquetas, repoUsuarios, repoVentas);
        }
        
        [Test]
        public void CrearUsuario_DeberiaAgregarUsuarioAlRepositorio()
        {
            string nombreUsuario = "testAdmin";
            string contrasena = "pass123";
            Rol rol = Rol.Administrador;

            fachada.CrearUsuario(nombreUsuario, rol);

            var usuarios = fachada.VerTodosLosUsuarios();

            Assert.AreEqual(1, usuarios.Count);

            Usuario usuarioCreado = usuarios[0];
            Assert.IsNotNull(usuarioCreado);
            Assert.AreEqual(nombreUsuario, usuarioCreado.NombreUsuario);
            Assert.AreEqual(rol, usuarioCreado.Rol);
            Assert.AreEqual(Estado.Activo, usuarioCreado.Estado);
            Assert.AreEqual(1, usuarioCreado.Id);
        }
        
        [Test]
        public void SuspenderUsuario_DeberiaCambiarEstadoASuspendido()
        {
            fachada.CrearUsuario("testUser", Rol.Vendedor);
            
            // Asumimos que es 1 porque [SetUp] limpia la BBDD
            int idUsuarioASuspender = 1; 

            fachada.SuspenderUsuario(idUsuarioASuspender);

            var usuarios = fachada.VerTodosLosUsuarios();
            Usuario usuarioSuspendido = usuarios[0];

            Assert.IsNotNull(usuarioSuspendido);
            Assert.AreEqual(idUsuarioASuspender, usuarioSuspendido.Id);
            Assert.AreEqual(Estado.Suspendido, usuarioSuspendido.Estado);
        }

        [Test]
        public void ActivarUsuario_DeberiaCambiarEstadoAActivo()
        {
            fachada.CrearUsuario("testUser", Rol.Vendedor);
            int idUsuario = 1;
            
            fachada.SuspenderUsuario(idUsuario);
            
            // Asumiendo que agregaste el método ActivarUsuario a Fachada.cs
            // fachada.ActivarUsuario(idUsuario); // Descomentar si agregas el método

            // Simulación manual si no tienes el método ActivarUsuario en Fachada
            var usuario = fachada.VerTodosLosUsuarios()[0];
            if (usuario.Id == idUsuario)
            {
                usuario.Activar(); 
            }

            var usuarios = fachada.VerTodosLosUsuarios();
            Usuario usuarioActivado = usuarios[0];

            Assert.IsNotNull(usuarioActivado);
            Assert.AreEqual(idUsuario, usuarioActivado.Id);
            Assert.AreEqual(Estado.Activo, usuarioActivado.Estado);
        }

        [Test]
        public void RegistrarVenta_DeberiaAgregarVentaAlCliente()
        {
            // 1. Arrange
            fachada.CrearCliente("Juan", "Perez", "099123456", "jp@mail.com", "M", DateTime.Now);
            
            // --- CORRECCIÓN: Obtenemos el ID REAL ---
            Cliente clienteCreado = fachada.VerTodosLosClientes()[0];
            int idClienteReal = clienteCreado.Id; 
            // --- FIN CORRECCIÓN ---

            string producto = "Laptop";
            float monto = 1500.50f;

            // 2. Act
            fachada.RegistrarVenta(idClienteReal, producto, monto); // Usamos el ID real

            // 3. Assert
            Cliente cliente = fachada.BuscarCliente(idClienteReal); // Buscamos con el ID real
            
            Assert.IsNotNull(cliente);
            Assert.AreEqual(1, cliente.Ventas.Count); // Ahora sí encontrará la venta
            
            Venta ventaRegistrada = cliente.Ventas[0];
            Assert.AreEqual(producto, ventaRegistrada.Producto);
            Assert.AreEqual(monto, ventaRegistrada.Importe);
            
            // Asumimos que el primer ID de Venta también es 1
            Assert.AreEqual(1, ventaRegistrada.Id); 
        }

        [Test]
        public void AsignarClienteVendedor_DeberiaAsociarVendedorAlCliente()
        {
            // --- ARRANGE ---
            
            // 1. Crear los objetos necesarios
            fachada.CrearCliente("Ana", "Gomez", "091987654", "ag@mail.com", "F", DateTime.Now);
            
            // (Asegúrate de que esta llamada coincide con tu firma en Fachada.cs)
            fachada.CrearUsuario("vendedorEstrella", Rol.Vendedor);

            // --- CORRECCIÓN: Obtenemos los IDs REALES ---

            // 2. Obtener el Cliente real
            Cliente clienteCreado = fachada.VerTodosLosClientes()[0];
            int idClienteReal = clienteCreado.Id;

            // 3. Obtener el Vendedor real
            Usuario vendedorCreado = fachada.VerTodosLosUsuarios().First(u => u.NombreUsuario == "vendedorEstrella");
            int idVendedorReal = vendedorCreado.Id;
            
            // --- ACT ---

            // 5. Ejecutar la lógica de negocio con los IDs reales
            fachada.AsignarClienteVendedor(idClienteReal, idVendedorReal);

            // --- ASSERT ---

            // 6. Buscar el cliente actualizado (usando el ID real)
            Cliente cliente = fachada.BuscarCliente(idClienteReal);
            
            // 7. Verificar los resultados
            Assert.IsNotNull(cliente);
            Assert.IsNotNull(cliente.VendedorAsignado);
            Assert.AreEqual(idVendedorReal, cliente.VendedorAsignado.Id);
        }
        
        [Test]
        public void EliminarUsuario_DeberiaQuitarUsuarioDelRepositorio()
        {
            fachada.CrearUsuario("userParaEliminar", Rol.Vendedor);
            int idUsuarioAEliminar = 1; // Asumimos 1 por [SetUp]

            Assert.AreEqual(1, fachada.VerTodosLosUsuarios().Count);

            fachada.EliminarUsuario(idUsuarioAEliminar);

            Assert.AreEqual(0, fachada.VerTodosLosUsuarios().Count);
            
            Usuario usuarioEliminado = fachada.BuscarUsuario(idUsuarioAEliminar);
            Assert.IsNull(usuarioEliminado);
        }

        [Test]
        public void AsignarClienteVendedor_NoDeberiaAsignarVendedorSuspendido()
        {
            // --- ARRANGE ---
            fachada.CrearCliente("Cliente", "Test", "123", "c@mail.com", "M", DateTime.Now);
            fachada.CrearUsuario("vendedorSuspendido", Rol.Vendedor);

            // --- CORRECCIÓN: Obtenemos los IDs REALES ---
            Cliente clienteCreado = fachada.VerTodosLosClientes()[0];
            int idClienteReal = clienteCreado.Id;
            
            Usuario vendedorCreado = fachada.VerTodosLosUsuarios().First(u => u.NombreUsuario == "vendedorSuspendido");
            int idVendedorReal = vendedorCreado.Id;
            // --- FIN CORRECCIÓN ---
            
            fachada.SuspenderUsuario(idVendedorReal); // Suspendemos el ID real

            // --- ACT ---
            fachada.AsignarClienteVendedor(idClienteReal, idVendedorReal); // Usamos IDs reales

            // --- ASSERT ---
            Cliente cliente = fachada.BuscarCliente(idClienteReal); // Buscamos el ID real
            
            Assert.IsNotNull(cliente);
            Assert.IsNull(cliente.VendedorAsignado); // Esto verificará tu lógica
        }

        [Test]
        public void AsignarClienteVendedor_NoDeberiaAsignarUsuarioNoVendedor()
        {
            // --- ARRANGE (CORREGIDO CON IDs REALES) ---
            fachada.CrearCliente("Cliente", "Test", "123", "c@mail.com", "M", DateTime.Now);
            // Obtenemos el ID real
            int idClienteReal = fachada.VerTodosLosClientes()[0].Id;

            fachada.CrearUsuario("adminUser", Rol.Administrador);
            // Obtenemos el ID real
            int idAdminReal = fachada.VerTodosLosUsuarios().First(u => u.NombreUsuario == "adminUser").Id;

            // --- ACT ---
            fachada.AsignarClienteVendedor(idClienteReal, idAdminReal);

            // --- ASSERT ---
            Cliente cliente = fachada.BuscarCliente(idClienteReal);
            
            Assert.IsNotNull(cliente);
            Assert.IsNull(cliente.VendedorAsignado);
        }
    }
}