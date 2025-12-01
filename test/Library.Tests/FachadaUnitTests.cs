using NUnit.Framework;
using Library;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    public class FachadaTests
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
        public void CrearCliente_ShouldAddClienteToRepo()
        {
            _fachada.CrearCliente("Juan", "Perez", "123", "juan@mail.com", "Masculino", DateTime.Now);
            var clientes = _fachada.VerTodosLosClientes();
            Assert.That(clientes.Count, Is.EqualTo(1));
            Assert.That(clientes[0].Nombre, Is.EqualTo("Juan"));
        }

        [Test]
        public void RegistrarInteraccion_ShouldAddInteraccionToCliente()
        {
            _fachada.CrearCliente("Juan", "Perez", "123", "juan@mail.com", "M", DateTime.Now);
            var cliente = _fachada.VerTodosLosClientes()[0];
            
            _fachada.RegistrarLlamada(cliente.Id, DateTime.Now, "Consulta", "Entrante");
            
            var interacciones = _fachada.VerInteraccionesCliente(cliente.Id);
            Assert.That(interacciones.Count, Is.EqualTo(1));
            Assert.That(interacciones[0], Is.InstanceOf<Llamada>());
        }

        [Test]
        public void ObtenerClientesSinRespuesta_ShouldReturnCorrectClients()
        {
            _fachada.CrearCliente("Juan", "Perez", "123", "juan@mail.com", "M", DateTime.Now);
            var cliente = _fachada.VerTodosLosClientes()[0];
            
            // Registramos una llamada "Recibida" (sin respuesta)
            _fachada.RegistrarLlamada(cliente.Id, DateTime.Now, "Perdida", "Recibida");
            
            var sinRespuesta = _fachada.ObtenerClientesSinRespuesta();
            Assert.That(sinRespuesta.Count, Is.EqualTo(1));
            Assert.That(sinRespuesta[0].Id, Is.EqualTo(cliente.Id));
        }

        [Test]
        public void ObtenerClientesSinRespuesta_ShouldNotReturnClientsWithOutgoingCalls()
        {
            _fachada.CrearCliente("Ana", "Gomez", "456", "ana@mail.com", "F", DateTime.Now);
            var cliente = _fachada.VerTodosLosClientes()[0];
            
            // Registramos una llamada "Saliente" (atendida/realizada)
            _fachada.RegistrarLlamada(cliente.Id, DateTime.Now, "Venta", "Saliente");
            
            var sinRespuesta = _fachada.ObtenerClientesSinRespuesta();
            Assert.That(sinRespuesta.Count, Is.EqualTo(0));
        }

        [Test]
        public void AgregarEtiquetaCliente_ShouldAddTag()
        {
            _fachada.CrearCliente("Juan", "Perez", "123", "juan@mail.com", "Masculino", DateTime.Now);
            _fachada.CrearEtiqueta("VIP");
            
            var cliente = _fachada.VerTodosLosClientes()[0];
            var etiqueta = _fachada.VerTodasLasEtiquetas()[0];
            
            _fachada.AgregarEtiquetaCliente(cliente.Id, etiqueta.Id);
            
            Assert.That(cliente.Etiquetas.Count, Is.EqualTo(1));
            Assert.That(cliente.Etiquetas[0].Nombre, Is.EqualTo("VIP"));
        }
    }
}
