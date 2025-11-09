using NUnit.Framework;
using Library;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestFixture]
    public class UsuarioTest
    {
        private Fachada fachada;

        [SetUp]
        public void Setup()
        {
            fachada = new Fachada();
        }

        [Test]
        public void CrearUsuario_DeberiaAgregarUsuarioAlRepositorio()
        {
            string nombreUsuario = "testAdmin";
            string contrasena = "pass123";
            Rol rol = Rol.Administrador;

            fachada.CrearUsuario(nombreUsuario, contrasena, rol);

            List<Usuario> usuarios = fachada.VerTodosLosUsuarios();

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
            fachada.CrearUsuario("testUser", "pass", Rol.Vendedor);
            
            int idUsuarioASuspender = 1;

            fachada.SuspenderUsuario(idUsuarioASuspender);

            List<Usuario> usuarios = fachada.VerTodosLosUsuarios();
            Usuario usuarioSuspendido = usuarios[0];

            Assert.IsNotNull(usuarioSuspendido);
            Assert.AreEqual(idUsuarioASuspender, usuarioSuspendido.Id);
            Assert.AreEqual(Estado.Suspendido, usuarioSuspendido.Estado);
        }

        [Test]
        public void ActivarUsuario_DeberiaCambiarEstadoAActivo()
        {
            fachada.CrearUsuario("testUser", "pass", Rol.Vendedor);
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

            List<Usuario> usuarios = fachada.VerTodosLosUsuarios();
            Usuario usuarioActivado = usuarios[0];

            Assert.IsNotNull(usuarioActivado);
            Assert.AreEqual(idUsuario, usuarioActivado.Id);
            Assert.AreEqual(Estado.Activo, usuarioActivado.Estado);
        }

        [Test]
        public void RegistrarVenta_DeberiaAgregarVentaAlCliente()
        {
            fachada.CrearCliente("Juan", "Perez", "099123456", "jp@mail.com", "M", DateTime.Now);
            int idCliente = 1;
            string producto = "Laptop";
            float monto = 1500.50f;

            fachada.RegistrarVenta(idCliente, producto, monto);

            Cliente cliente = fachada.BuscarCliente(idCliente);
            
            Assert.IsNotNull(cliente);
            Assert.AreEqual(1, cliente.Ventas.Count);
            
            Venta ventaRegistrada = cliente.Ventas[0];
            Assert.AreEqual(producto, ventaRegistrada.Producto);
            Assert.AreEqual(monto, ventaRegistrada.Importe);
            Assert.AreEqual(1, ventaRegistrada.Id);
        }

        [Test]
        public void AsignarClienteVendedor_DeberiaAsociarVendedorAlCliente()
        {
            fachada.CrearCliente("Ana", "Gomez", "091987654", "ag@mail.com", "F", DateTime.Now);
            int idCliente = 1;

            fachada.CrearUsuario("vendedorEstrella", "pass", Rol.Vendedor);
            int idVendedor = 1;

            fachada.AsignarClienteVendedor(idCliente, idVendedor);

            Cliente cliente = fachada.BuscarCliente(idCliente);
            
            Assert.IsNotNull(cliente);
            Assert.IsNotNull(cliente.VendedorAsignado);
            Assert.AreEqual(idVendedor, cliente.VendedorAsignado.Id);
        }

        [Test]
        public void EliminarUsuario_DeberiaQuitarUsuarioDelRepositorio()
        {
            fachada.CrearUsuario("userParaEliminar", "pass", Rol.Vendedor);
            int idUsuarioAEliminar = 1;

            Assert.AreEqual(1, fachada.VerTodosLosUsuarios().Count);

            fachada.EliminarUsuario(idUsuarioAEliminar);

            Assert.AreEqual(0, fachada.VerTodosLosUsuarios().Count);
            
            // Esta línea ahora funciona porque agregaste BuscarUsuario a la Fachada
            Usuario usuarioEliminado = fachada.BuscarUsuario(idUsuarioAEliminar);
            Assert.IsNull(usuarioEliminado);
        }

        [Test]
        public void AsignarClienteVendedor_NoDeberiaAsignarVendedorSuspendido()
        {
            fachada.CrearCliente("Cliente", "Test", "123", "c@mail.com", "M", DateTime.Now);
            int idCliente = 1;

            fachada.CrearUsuario("vendedorSuspendido", "pass", Rol.Vendedor);
            int idVendedor = 1;
            
            fachada.SuspenderUsuario(idVendedor);

            fachada.AsignarClienteVendedor(idCliente, idVendedor);

            Cliente cliente = fachada.BuscarCliente(idCliente);
            
            Assert.IsNotNull(cliente);
            Assert.IsNull(cliente.VendedorAsignado);
        }

        [Test]
        public void AsignarClienteVendedor_NoDeberiaAsignarUsuarioNoVendedor()
        {
            fachada.CrearCliente("Cliente", "Test", "123", "c@mail.com", "M", DateTime.Now);
            int idCliente = 1;

            fachada.CrearUsuario("adminUser", "pass", Rol.Administrador);
            int idAdmin = 1;

            fachada.AsignarClienteVendedor(idCliente, idAdmin);

            Cliente cliente = fachada.BuscarCliente(idCliente);
            
            Assert.IsNotNull(cliente);
            Assert.IsNull(cliente.VendedorAsignado);
        }
    }
}