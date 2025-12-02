using NUnit.Framework;
using System;
using Library;

namespace Library.Tests
{
    [TestFixture]
    public class UsuarioTest
    {
        private Fachada _fachada;

        [SetUp]
        public void Setup()
        {
            // 1. Creamos las implementaciones reales de los repositorios
            IRepoClientes repoClientes = new RepoClientes();
            IRepoEtiquetas repoEtiquetas = new RepoEtiquetas();
            IRepoUsuarios repoUsuarios = new RepoUsuarios();
            IRepoVentas repoVentas = new RepoVentas();

            // 2. Inyectamos los repositorios en el constructor de la Fachada
            _fachada = new Fachada(repoClientes, repoEtiquetas, repoUsuarios, repoVentas);
        }

        [Test]
        public void CrearUsuario_DeberiaAgregarUsuarioAlRepositorio()
        {
            // Arrange
            string nombreUsuario = "testAdmin";
            Rol rol = Rol.Administrador;

            // Act
            _fachada.CrearUsuario(nombreUsuario, rol);

            // Assert
            var usuarios = _fachada.VerTodosLosUsuarios();

            Assert.AreEqual(1, usuarios.Count);

            Usuario usuarioCreado = usuarios[0];

            Assert.IsNotNull(usuarioCreado);
            Assert.AreEqual(nombreUsuario, usuarioCreado.NombreUsuario);
            Assert.AreEqual(Estado.Activo, usuarioCreado.Estado);
            Assert.AreEqual(1, usuarioCreado.Id);
            Assert.AreEqual(1, usuarioCreado.Roles.Count, "El usuario debe tener exactamente 1 rol asignado al crearse.");
            
            // Reemplazo de Contains con foreach
            bool tieneRol = false;
            foreach (var r in usuarioCreado.Roles)
            {
                if (r == rol)
                {
                    tieneRol = true;
                    break;
                }
            }
            Assert.IsTrue(tieneRol, "El rol asignado en la lista no coincide con el rol esperado.");
        }

        [Test]
        public void SuspenderUsuario_DeberiaCambiarEstadoASuspendido()
        {
            // Arrange
            _fachada.CrearUsuario("testUser", Rol.Vendedor);
            
            // Obtenemos el ID real sin LINQ
            var usuarios = _fachada.VerTodosLosUsuarios();
            int idUsuarioASuspender = usuarios[0].Id;

            // Act
            _fachada.SuspenderUsuario(idUsuarioASuspender);

            // Assert
            var usuariosDespues = _fachada.VerTodosLosUsuarios();
            Usuario usuarioSuspendido = usuariosDespues[0];

            Assert.IsNotNull(usuarioSuspendido);
            Assert.AreEqual(idUsuarioASuspender, usuarioSuspendido.Id);
            Assert.AreEqual(Estado.Suspendido, usuarioSuspendido.Estado);
        }

        [Test]
        public void ActivarUsuario_DeberiaCambiarEstadoAActivo()
        {
            // ARRANGE: Crear usuario
            Rol rolEsperado = Rol.Vendedor;
            _fachada.CrearUsuario("testUser", rolEsperado);

            // OBTENER EL ID DEL USUARIO RECIÉN CREADO
            var listaUsuarios = _fachada.VerTodosLosUsuarios();
            Usuario usuarioExistente = listaUsuarios[0];
            int idUsuario = usuarioExistente.Id;

            // ACT 1: Suspender (Precondición)
            _fachada.SuspenderUsuario(idUsuario);

            // VERIFICAR ESTADO SUSPENDIDO
            Usuario usuarioSuspendido = _fachada.VerTodosLosUsuarios()[0];
            Assert.AreEqual(Estado.Suspendido, usuarioSuspendido.Estado, "Fallo: El usuario no se suspendió correctamente.");

            // ACT 2: Activar
            _fachada.ActivarUsuario(idUsuario);

            // ASSERT: Verificar estado Activo y Roles
            var usuariosFinal = _fachada.VerTodosLosUsuarios();
            Usuario usuarioActivado = usuariosFinal[0];

            // Chequeos de consistencia
            Assert.IsNotNull(usuarioActivado);
            Assert.AreEqual(idUsuario, usuarioActivado.Id);

            // Chequeo del estado final
            Assert.AreEqual(Estado.Activo, usuarioActivado.Estado, "El estado del usuario no es Activo después de la activación.");

            // Chequeo de Roles (Multi-Rol) sin LINQ
            Assert.AreEqual(1, usuarioActivado.Roles.Count, "El usuario debe tener un único rol después de la creación.");
            
            bool rolPersiste = false;
            foreach (var r in usuarioActivado.Roles)
            {
                if (r == rolEsperado) { rolPersiste = true; break; }
            }
            Assert.IsTrue(rolPersiste, "El rol Vendedor debe persistir en la lista.");
        }

        [Test]
        public void RegistrarVenta_DeberiaAgregarVentaAlCliente()
        {
            // 1. ARRANGE (Preparar datos)
            _fachada.CrearCliente("Juan", "Perez", "099123456", "jp@mail.com", "M", DateTime.Now);
    
            // Obtenemos ID real
            var clientes = _fachada.VerTodosLosClientes();
            Cliente clienteCreado = clientes[0];
            int idClienteReal = clienteCreado.Id;

            string producto = "Laptop";
            float monto = 1500.50f;
            DateTime fechaVenta = DateTime.Now;

            // 2. ACT (Ejecutar la acción)
            _fachada.RegistrarVenta(idClienteReal, producto, monto, fechaVenta); 

            // 3. ASSERT (Verificar resultados)
            Cliente cliente = _fachada.BuscarCliente(idClienteReal);

            Assert.IsNotNull(cliente);
            Assert.AreEqual(1, cliente.Ventas.Count);

            Venta ventaRegistrada = cliente.Ventas[0];
    
            Assert.AreEqual(producto, ventaRegistrada.Producto);
            Assert.AreEqual(monto, ventaRegistrada.Importe);
            Assert.AreEqual(fechaVenta, ventaRegistrada.Fecha);
        }

        [Test]
        public void AsignarClienteVendedor_DeberiaAsociarVendedorAlCliente()
        {
            // --- ARRANGE ---
            // 1. Crear los objetos necesarios
            _fachada.CrearCliente("Ana", "Gomez", "091987654", "ag@mail.com", "F", DateTime.Now);
            _fachada.CrearUsuario("vendedorEstrella", Rol.Vendedor);

            // 2. Obtener el Cliente real
            var clientes = _fachada.VerTodosLosClientes();
            Cliente clienteCreado = clientes[0];
            int idClienteReal = clienteCreado.Id;

            // 3. Obtener el Vendedor real SIN LINQ
            int idVendedorReal = -1;
            var usuarios = _fachada.VerTodosLosUsuarios();
            foreach (var u in usuarios)
            {
                if (u.NombreUsuario == "vendedorEstrella")
                {
                    idVendedorReal = u.Id;
                    break;
                }
            }
            
            Assert.AreNotEqual(-1, idVendedorReal, "El vendedor debería existir.");

            // --- ACT ---
            _fachada.AsignarClienteVendedor(idClienteReal, idVendedorReal);

            // --- ASSERT ---
            Cliente cliente = _fachada.BuscarCliente(idClienteReal);

            Assert.IsNotNull(cliente);
            Assert.IsNotNull(cliente.VendedorAsignado);
            Assert.AreEqual(idVendedorReal, cliente.VendedorAsignado.Id);
        }

        [Test]
        public void EliminarUsuario_DeberiaQuitarUsuarioDelRepositorio()
        {
            // Arrange
            _fachada.CrearUsuario("userParaEliminar", Rol.Vendedor);
            
            var usuarios = _fachada.VerTodosLosUsuarios();
            int idUsuarioAEliminar = usuarios[0].Id;

            Assert.AreEqual(1, usuarios.Count);

            // Act
            _fachada.EliminarUsuario(idUsuarioAEliminar);

            // Assert
            var usuariosFinal = _fachada.VerTodosLosUsuarios();
            Assert.AreEqual(0, usuariosFinal.Count);

            Usuario usuarioEliminado = _fachada.BuscarUsuario(idUsuarioAEliminar);
            Assert.IsNull(usuarioEliminado);
        }

        [Test]
        public void AsignarClienteVendedor_NoDeberiaAsignarVendedorSuspendido()
        {
            // --- ARRANGE ---
            _fachada.CrearCliente("Cliente", "Test", "123", "c@mail.com", "M", DateTime.Now);
            _fachada.CrearUsuario("vendedorSuspendido", Rol.Vendedor);
            
            var clientes = _fachada.VerTodosLosClientes();
            int idClienteReal = clientes[0].Id;

            // Buscar ID vendedor sin LINQ
            int idVendedorReal = -1;
            foreach(Usuario u in _fachada.VerTodosLosUsuarios())
            {
                if (u.NombreUsuario == "vendedorSuspendido")
                {
                    idVendedorReal = u.Id;
                    break;
                }
            }
    
            _fachada.SuspenderUsuario(idVendedorReal); // Estado = Suspendido

            // --- ACT & ASSERT ---
            Assert.Throws<InvalidOperationException>(() =>
            {
                _fachada.AsignarClienteVendedor(idClienteReal, idVendedorReal); 
            });
    
            Cliente cliente = _fachada.BuscarCliente(idClienteReal);
            Assert.IsNull(cliente.VendedorAsignado);
        }

        [Test]
        public void AsignarClienteVendedor_DeberiaLanzarExcepcionSiUsuarioNoEsVendedor()
        {
            // --- ARRANGE ---
            _fachada.CrearCliente("Cliente", "Test", "123", "c@mail.com", "M", DateTime.Now);
            var clientes = _fachada.VerTodosLosClientes();
            int idClienteReal = clientes[0].Id;

            // Crear un usuario que NO es vendedor
            _fachada.CrearUsuario("adminUser", Rol.Administrador);

            // Obtener el ID real sin LINQ
            int idAdminReal = 0;
            foreach (Usuario u in _fachada.VerTodosLosUsuarios())
            {
                if (u.NombreUsuario == "adminUser")
                {
                    idAdminReal = u.Id;
                    break;
                }
            }

            // --- ACT & ASSERT ---
            Assert.Throws<InvalidOperationException>(() =>
            {
                _fachada.AsignarClienteVendedor(idClienteReal, idAdminReal);
            });

            // --- VERIFICACIÓN DE ESTADO ---
            Cliente cliente = _fachada.BuscarCliente(idClienteReal);
            Assert.IsNull(cliente.VendedorAsignado, "El vendedor no debió ser asignado.");
        }
    }
}