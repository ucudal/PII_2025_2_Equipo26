using NUnit.Framework;
using Library;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestFixture]
    public class InteraccionEtiquetaReporteTests
    {
        private Fachada _fachada;
        private IRepoClientes repoClientes;
        private IRepoEtiquetas repoEtiquetas;
        private IRepoUsuarios repoUsuarios;
        private IRepoVentas repoVentas;
        
        private int clienteId;
        private int vendedorId;

        [SetUp]
        public void SetUp()
        {
            this.repoClientes = new RepoClientes();
            this.repoEtiquetas = new RepoEtiquetas();
            this.repoUsuarios = new RepoUsuarios();
            this.repoVentas = new RepoVentas();
            this._fachada = new Fachada(this.repoClientes, this.repoEtiquetas, this.repoUsuarios, this.repoVentas);

            // Setup inicial
            this._fachada.CrearCliente("Cliente", "Prueba", "123456", "cliente@test.com", "F", DateTime.Now);
            var clientes = this._fachada.VerTodosLosClientes();
            this.clienteId = clientes[0].Id;

            this._fachada.CrearUsuario("Vendedor", Rol.Vendedor);
            var usuarios = this._fachada.VerTodosLosUsuarios();
            this.vendedorId = usuarios[0].Id;
        }

        [Test]
        public void TestAsignarClienteVendedor()
        {
            var cliente = this._fachada.BuscarCliente(this.clienteId);
            Assert.IsNull(cliente.VendedorAsignado);

            this._fachada.AsignarClienteVendedor(this.clienteId, this.vendedorId);

            var clienteActualizado = this._fachada.BuscarCliente(this.clienteId);
            var vendedor = this._fachada.BuscarUsuario(this.vendedorId);

            Assert.IsNotNull(clienteActualizado.VendedorAsignado);
            Assert.AreEqual(vendedor.Id, clienteActualizado.VendedorAsignado.Id);
        }

        [Test]
        public void TestRegistrarInteraccionesPolimorficas()
        {
            DateTime fechaLlamada = new DateTime(2025, 1, 1);
            DateTime fechaReunion = new DateTime(2025, 1, 2);
            DateTime fechaCorreo = new DateTime(2025, 1, 3);
            
            this._fachada.RegistrarLlamada(this.clienteId, fechaLlamada, "Consulta", "Entrante");
            this._fachada.RegistrarReunion(this.clienteId, fechaReunion, "Demo", "Oficina");
            this._fachada.RegistrarCorreo(this.clienteId, fechaCorreo, "Seguimiento", "v@m.com", "c@m.com", "Asunto");

            var interacciones = this._fachada.VerInteraccionesCliente(this.clienteId);
            Assert.AreEqual(3, interacciones.Count);

            int countLlamadas = 0;
            int countReuniones = 0;
            int countCorreos = 0;

            foreach (var i in interacciones)
            {
                if (i.Tipo == TipoInteraccion.Llamada) countLlamadas++;
                else if (i.Tipo == TipoInteraccion.Reunion) countReuniones++;
                else if (i.Tipo == TipoInteraccion.Correo) countCorreos++;
            }

            Assert.AreEqual(1, countLlamadas);
            Assert.AreEqual(1, countReuniones);
            Assert.AreEqual(1, countCorreos);
        }

        [Test]
        public void TestFiltrarInteraccionesPorTipo()
        {
            this._fachada.RegistrarLlamada(this.clienteId, DateTime.Now, "1", "Entrante");
            this._fachada.RegistrarLlamada(this.clienteId, DateTime.Now, "2", "Saliente");
            this._fachada.RegistrarReunion(this.clienteId, DateTime.Now, "3", "Oficina");

            var llamadas = this._fachada.VerInteraccionesCliente(this.clienteId, TipoInteraccion.Llamada);
            var reuniones = this._fachada.VerInteraccionesCliente(this.clienteId, TipoInteraccion.Reunion);
            var correos = this._fachada.VerInteraccionesCliente(this.clienteId, TipoInteraccion.Correo);

            Assert.AreEqual(2, llamadas.Count);
            Assert.AreEqual(1, reuniones.Count);
            Assert.AreEqual(0, correos.Count);
        }
        
        [Test]
        public void TestAsignarYQuitarEtiquetaACliente()
        {
            // Arrange
            this._fachada.CrearEtiqueta("VIP");
            this._fachada.CrearEtiqueta("Competencia");
            
            Etiqueta etiquetaVip = null;
            Etiqueta etiquetaComp = null;
            
            foreach (var e in this._fachada.VerTodasLasEtiquetas())
            {
                if (e.Nombre == "VIP") etiquetaVip = e;
                if (e.Nombre == "Competencia") etiquetaComp = e;
            }

            Assert.IsNotNull(etiquetaVip);
            Assert.IsNotNull(etiquetaComp);

            // Act - Asignar
            this._fachada.AgregarEtiquetaCliente(this.clienteId, etiquetaVip.Id);
            this._fachada.AgregarEtiquetaCliente(this.clienteId, etiquetaComp.Id);

            var cliente = this._fachada.BuscarCliente(this.clienteId);
            Assert.AreEqual(2, cliente.Etiquetas.Count);

            // --- CORRECCIÓN: MANUAL CONTAINS ---
            bool tieneVip = false;
            foreach (var e in cliente.Etiquetas)
            {
                if (e.Id == etiquetaVip.Id) { tieneVip = true; break; }
            }
            Assert.IsTrue(tieneVip, "Debería tener la etiqueta VIP");

            // Act - Quitar
            this._fachada.QuitarEtiquetaCliente(this.clienteId, etiquetaVip.Id);

            // Assert - Eliminación
            var clienteActualizado = this._fachada.BuscarCliente(this.clienteId);
            Assert.AreEqual(1, clienteActualizado.Etiquetas.Count);

            // --- CORRECCIÓN: MANUAL CONTAINS ---
            bool tieneVipDespues = false;
            bool tieneCompDespues = false;
            foreach (var e in clienteActualizado.Etiquetas)
            {
                if (e.Id == etiquetaVip.Id) tieneVipDespues = true;
                if (e.Id == etiquetaComp.Id) tieneCompDespues = true;
            }

            Assert.IsFalse(tieneVipDespues, "Ya no debería tener VIP");
            Assert.IsTrue(tieneCompDespues, "Debería mantener Competencia");
        }

        [Test]
        public void TestRegistrarVentaGeneralYReporte()
        {
            DateTime fecha1 = new DateTime(2025, 10, 5);
            DateTime fecha2 = new DateTime(2025, 10, 15);
            DateTime fecha3 = new DateTime(2025, 10, 25); 

            this._fachada.RegistrarVenta(this.clienteId, "A", 100, fecha1);
            this._fachada.RegistrarVenta(this.clienteId, "B", 50, fecha2);
            this._fachada.RegistrarVenta(this.clienteId, "C", 200, fecha3); 

            DateTime inicio = new DateTime(2025, 10, 1);
            DateTime fin = new DateTime(2025, 10, 20);
    
            float total = this._fachada.CalcularTotalVentas(inicio, fin);
            Assert.AreEqual(150, total); 
        }

        [Test]
        public void TestRegistrarVentaAsignadaACliente()
        {
            var cliente = this._fachada.BuscarCliente(this.clienteId);
            Assert.AreEqual(0, cliente.Ventas.Count);
            
            this._fachada.RegistrarVenta(this.clienteId, "P1", 1500, DateTime.Now);
            
            var clienteActualizado = this._fachada.BuscarCliente(this.clienteId);
            Assert.AreEqual(1, clienteActualizado.Ventas.Count);
            Assert.AreEqual(1500, clienteActualizado.Ventas[0].Importe);
        }

        [Test]
        public void TestObtenerClientesInactivos()
        {
            this._fachada.CrearCliente("Activo", "User", "1", "a", "M", DateTime.Now);
            int idActivo = 0;
            foreach(var c in this._fachada.VerTodosLosClientes()) { if(c.Nombre == "Activo") idActivo = c.Id; }
            this._fachada.RegistrarLlamada(idActivo, DateTime.Now.AddDays(-10), "Ok", "Entrante");

            this._fachada.CrearCliente("Inactivo", "User", "2", "b", "F", DateTime.Now);
            int idInactivo = 0;
            foreach (var c in this._fachada.VerTodosLosClientes()) { if (c.Nombre == "Inactivo") idInactivo = c.Id; }
            this._fachada.RegistrarLlamada(idInactivo, DateTime.Now.AddDays(-40), "Old", "Entrante");

            var inactivos = this._fachada.ObtenerClientesInactivos(30);

            Assert.AreEqual(2, inactivos.Count); // El del setup (sin interacciones) + el Inactivo
            
            bool estaInactivo = false;
            bool estaOriginal = false;
            foreach(var c in inactivos)
            {
                if (c.Nombre == "Inactivo") estaInactivo = true;
                if (c.Id == this.clienteId) estaOriginal = true;
            }
            Assert.IsTrue(estaInactivo);
            Assert.IsTrue(estaOriginal);
        }
        
        [Test]
        public void TestObtenerResumenDashboard()
        {
            this._fachada.RegistrarLlamada(this.clienteId, DateTime.Now.AddDays(-1), "Call", "Entrante");
            this._fachada.RegistrarReunion(this.clienteId, DateTime.Now.AddDays(5), "Meet", "Zoom");

            var dashboard = this._fachada.ObtenerResumenDashboard();

            Assert.AreEqual(1, dashboard.TotalClientes);
            Assert.AreEqual(1, dashboard.InteraccionesRecientes.Count);
            Assert.AreEqual(1, dashboard.ReunionesProximas.Count);
        }
    }
}