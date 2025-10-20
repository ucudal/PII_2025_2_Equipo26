using NUnit.Framework;
using Library;
using System;
using System.Linq;

namespace Library.Tests
{
    [TestFixture]
    public class InteraccionEtiquetaReporteTests
    {
        private Fachada fachada;

        [SetUp]
        public void Setup()
        {
            fachada = new Fachada();
        }

        [Test]
        public void RegistrarMultiplesInteracciones_PruebaPolimorfismo()
        {
            fachada.CrearCliente("Cliente Poli", "Test", "123", "poly@test.com", "Otro", DateTime.Now);
            var cliente = fachada.VerTodosLosClientes().First();

            fachada.RegistrarLlamada(cliente.Id, DateTime.Now.AddDays(-1), "Llamada prueba", "Saliente");
            fachada.RegistrarReunion(cliente.Id, DateTime.Now.AddDays(1), "Reunion prueba", "Oficina");
            fachada.RegistrarCorreo(cliente.Id, DateTime.Now, "Correo prueba", "cli@mail.com", "ven@mail.com", "Consulta"); //
            fachada.RegistrarCotizacion(cliente.Id, "Cotizacion prueba", 1500.0, "Detalle"); //
            
            var interacciones = fachada.VerInteraccionesDeCliente(cliente.Id);

            Assert.AreEqual(4, interacciones.Count);
            Assert.IsTrue(interacciones.Any(i => i is Llamada)); //
            Assert.IsTrue(interacciones.Any(i => i is Reunion)); //
            Assert.IsTrue(interacciones.Any(i => i is Correo)); //
            Assert.IsTrue(interacciones.Any(i => i is Cotizacion)); //
        }

        [Test]
        public void CrearYAgregarEtiquetaACliente_FuncionaCorrectamente()
        {
            fachada.CrearCliente("Cliente Tag", "Test", "456", "tag@test.com", "Otro", DateTime.Now);
            var cliente = fachada.VerTodosLosClientes().First();
            
            fachada.CrearEtiqueta("VIP"); //
            var etiqueta = fachada.VerTodasLasEtiquetas().First(e => e.Nombre == "VIP"); 

            fachada.AgregarEtiquetaACliente(cliente.Id, etiqueta.Id); //
            var clienteActualizado = fachada.BuscarCliente(cliente.Id);

            Assert.AreEqual(1, clienteActualizado.Etiquetas.Count);
            Assert.AreEqual("VIP", clienteActualizado.Etiquetas.First().Nombre); //
        }

        [Test]
        public void ObtenerResumenDashboard_CalculaDatosCorrectamente()
        {
            fachada.CrearCliente("Cliente A", "Test", "111", "a@test.com", "Otro", DateTime.Now);
            fachada.CrearCliente("Cliente B", "Test", "222", "b@test.com", "Otro", DateTime.Now);
            var clienteA = fachada.VerTodosLosClientes().First();
            var clienteB = fachada.VerTodosLosClientes().Last();

            var llamadaReciente = DateTime.Now.AddMinutes(-10);
            var llamadaMenosReciente = DateTime.Now.AddMinutes(-30);
            var reunionCercana = DateTime.Now.AddDays(1);
            var reunionLejana = DateTime.Now.AddDays(3);

            fachada.RegistrarLlamada(clienteA.Id, llamadaReciente, "Llamada Mas Reciente (A)", "Entrante");
            fachada.RegistrarLlamada(clienteB.Id, llamadaMenosReciente, "Llamada Menos Reciente (B)", "Saliente");
            fachada.RegistrarReunion(clienteB.Id, reunionCercana, "Reunion Mas Cercana (B)", "Oficina");
            fachada.RegistrarReunion(clienteA.Id, reunionLejana, "Reunion Mas Lejana (A)", "Virtual");

            var resumen = fachada.ObtenerResumenDashboard(); //

            Assert.AreEqual(2, resumen.TotalClientes); //

            Assert.AreEqual(4, resumen.InteraccionesRecientes.Count); //
            Assert.AreEqual("Reunion Mas Lejana (A)", resumen.InteraccionesRecientes[0].Tema);
            Assert.AreEqual("Reunion Mas Cercana (B)", resumen.InteraccionesRecientes[1].Tema);
            Assert.AreEqual("Llamada Mas Reciente (A)", resumen.InteraccionesRecientes[2].Tema);
            Assert.AreEqual("Llamada Menos Reciente (B)", resumen.InteraccionesRecientes[3].Tema);

            Assert.AreEqual(2, resumen.ReunionesProximas.Count); //
            Assert.AreEqual("Reunion Mas Cercana (B)", resumen.ReunionesProximas[0].Tema);
            Assert.AreEqual("Reunion Mas Lejana (A)", resumen.ReunionesProximas[1].Tema);
        }
    }
}