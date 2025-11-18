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
            Rol rol = Rol.Administrador;

            fachada.CrearUsuario(nombreUsuario, rol);

            var usuarios = fachada.VerTodosLosUsuarios();

            Assert.AreEqual(1, usuarios.Count);

            Usuario usuarioCreado = usuarios[0];


            Assert.IsNotNull(usuarioCreado);
            Assert.AreEqual(nombreUsuario, usuarioCreado.NombreUsuario);
            Assert.AreEqual(Estado.Activo, usuarioCreado.Estado);
            Assert.AreEqual(1, usuarioCreado.Id);
            Assert.AreEqual(1, usuarioCreado.Roles.Count,
                "El usuario debe tener exactamente 1 rol asignado al crearse.");
            Assert.IsTrue(usuarioCreado.Roles.Contains(rol),
                "El rol asignado en la lista no coincide con el rol esperado.");
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
            // ARRANGE: Crear usuario
            Rol rolEsperado = Rol.Vendedor;
            fachada.CrearUsuario("testUser", rolEsperado);

            // OBTENER EL ID DEL USUARIO RECIÉN CREADO para garantizar que es el correcto.
            // Usamos VerTodosLosUsuarios() para obtener el objeto Usuario y su ID real.
            Usuario usuarioExistente = fachada.VerTodosLosUsuarios()[0];
            int idUsuario = usuarioExistente.Id; // Usamos el ID que asignó el repositorio

            // ACT: Suspender
            fachada.SuspenderUsuario(idUsuario);

            // VERIFICAR ESTADO SUSPENDIDO (Precondición)
            Usuario usuarioSuspendido = fachada.VerTodosLosUsuarios()[0];
            Assert.AreEqual(Estado.Suspendido, usuarioSuspendido.Estado,
                "Fallo: El usuario no se suspendió correctamente.");

            // ACT: Activar
            // Usamos el método de la Fachada (Descomentar si agregaste el método)
            fachada.ActivarUsuario(idUsuario);

            // ASSERT: Verificar estado Activo y Roles
            var usuarios = fachada.VerTodosLosUsuarios();
            Usuario usuarioActivado = usuarios[0];

            // Chequeos de consistencia
            Assert.IsNotNull(usuarioActivado);
            Assert.AreEqual(idUsuario, usuarioActivado.Id);

            // Chequeo del estado final
            Assert.AreEqual(Estado.Activo, usuarioActivado.Estado,
                "El estado del usuario no es Activo después de la activación.");

            // Chequeo de Roles (Multi-Rol)
            Assert.AreEqual(1, usuarioActivado.Roles.Count,
                "El usuario debe tener un único rol después de la creación.");
            Assert.IsTrue(usuarioActivado.Roles.Contains(rolEsperado), "El rol Vendedor debe persistir en la lista.");
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
            
            int idClienteReal = fachada.VerTodosLosClientes()[0].Id;

            Usuario vendedorCreado = null;
            foreach(Usuario u in fachada.VerTodosLosUsuarios())
            {
                if (u.NombreUsuario == "vendedorSuspendido")
                {
                    vendedorCreado = u;
                    break;
                }
            }
            int idVendedorReal = vendedorCreado.Id;
    
            fachada.SuspenderUsuario(idVendedorReal); // Estado = Suspendido

            // --- ACT & ASSERT ---
            // El test ahora espera que la nueva precondición lance una InvalidOperationException
            Assert.Throws<InvalidOperationException>(() =>
            {
                fachada.AsignarClienteVendedor(idClienteReal, idVendedorReal); 
            });
    
            // Verificamos que no se haya asignado nada
            Cliente cliente = fachada.BuscarCliente(idClienteReal);
            Assert.IsNull(cliente.VendedorAsignado);
        }
        [Test]
        public void AsignarClienteVendedor_DeberiaLanzarExcepcionSiUsuarioNoEsVendedor()
        {
            // --- ARRANGE ---
            fachada.CrearCliente("Cliente", "Test", "123", "c@mail.com", "M", DateTime.Now);
            int idClienteReal = fachada.VerTodosLosClientes()[0].Id;

            // Crear un usuario que NO es vendedor
            fachada.CrearUsuario("adminUser", Rol.Administrador);

            // Obtener el ID real sin LINQ
            int idAdminReal = 0;
            foreach (Usuario u in fachada.VerTodosLosUsuarios())
            {
                if (u.NombreUsuario == "adminUser")
                {
                    idAdminReal = u.Id;
                    break;
                }
            }

            // --- ACT & ASSERT (La clave) ---
            // Envolvemos la llamada en Assert.Throws<InvalidOperationException>
            // Esto verifica que el CÓDIGO FALLA de forma controlada.
            Assert.Throws<InvalidOperationException>(() =>
            {
                fachada.AsignarClienteVendedor(idClienteReal, idAdminReal);
            });

            // --- VERIFICACIÓN DE ESTADO ---
            // OPCIONAL: Verificamos que el estado del cliente no cambió (sigue sin vendedor)
            Cliente cliente = fachada.BuscarCliente(idClienteReal);
            Assert.IsNull(cliente.VendedorAsignado, "El vendedor no debió ser asignado.");
        }
    }
}