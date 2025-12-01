using NUnit.Framework;
using Library;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestFixture]
    public class FachadaUnitTests
    {
        private Fachada _fachada;
        private IRepoClientes _repoClientes;
        private IRepoEtiquetas _repoEtiquetas;
        private IRepoUsuarios _repoUsuarios;
        private IRepoVentas _repoVentas;

        [SetUp]
        public void Setup()
        {
            _repoClientes = new RepoClientes();
            _repoEtiquetas = new RepoEtiquetas();
            _repoUsuarios = new RepoUsuarios();
            _repoVentas = new RepoVentas();
            _fachada = new Fachada(_repoClientes, _repoEtiquetas, _repoUsuarios, _repoVentas);
        }

        [Test]
        public void TestCrearCliente_DeberiaAgregarClienteAlRepositorio()
        {
            _fachada.CrearCliente("Juan", "Perez", "123", "juan@mail.com", "Masculino", DateTime.Now);
            var clientes = _fachada.VerTodosLosClientes();
            
            Assert.AreEqual(1, clientes.Count);
            Assert.AreEqual("Juan", clientes[0].Nombre);
        }

        [Test]
        public void TestRegistrarInteraccion_DeberiaAgregarInteraccionAlCliente()
        {
            _fachada.CrearCliente("Juan", "Perez", "123", "juan@mail.com", "M", DateTime.Now);
            var cliente = _fachada.VerTodosLosClientes()[0];
            
            _fachada.RegistrarLlamada(cliente.Id, DateTime.Now, "Consulta", "Entrante");
            
            var interacciones = _fachada.VerInteraccionesCliente(cliente.Id);
            
            Assert.AreEqual(1, interacciones.Count);
            Assert.IsInstanceOf<Llamada>(interacciones[0]);
        }

        [Test]
        public void TestObtenerClientesSinRespuesta_DeberiaRetornarClientesCorrectos()
        {
            _fachada.CrearCliente("Juan", "Perez", "123", "juan@mail.com", "M", DateTime.Now);
            var cliente = _fachada.VerTodosLosClientes()[0];
            
            // Registramos una llamada "Recibida" (sin respuesta)
            _fachada.RegistrarLlamada(cliente.Id, DateTime.Now, "Perdida", "Recibida");
            
            var sinRespuesta = _fachada.ObtenerClientesSinRespuesta();
            
            Assert.AreEqual(1, sinRespuesta.Count);
            Assert.AreEqual(cliente.Id, sinRespuesta[0].Id);
        }

        [Test]
        public void TestObtenerClientesSinRespuesta_NoDeberiaRetornarClientesConLlamadasSalientes()
        {
            _fachada.CrearCliente("Ana", "Gomez", "456", "ana@mail.com", "F", DateTime.Now);
            var cliente = _fachada.VerTodosLosClientes()[0];
            
            // Registramos una llamada "Saliente" (atendida/realizada)
            _fachada.RegistrarLlamada(cliente.Id, DateTime.Now, "Venta", "Saliente");
            
            var sinRespuesta = _fachada.ObtenerClientesSinRespuesta();
            
            Assert.AreEqual(0, sinRespuesta.Count);
        }

        [Test]
        public void TestAgregarEtiquetaCliente_DeberiaAgregarEtiqueta()
        {
            _fachada.CrearCliente("Juan", "Perez", "123", "juan@mail.com", "Masculino", DateTime.Now);
            _fachada.CrearEtiqueta("VIP");
            
            var cliente = _fachada.VerTodosLosClientes()[0];
            var etiqueta = _fachada.VerTodasLasEtiquetas()[0];
            
            _fachada.AgregarEtiquetaCliente(cliente.Id, etiqueta.Id);
            
            Assert.AreEqual(1, cliente.Etiquetas.Count);
            Assert.AreEqual("VIP", cliente.Etiquetas[0].Nombre);
        }
    }
}