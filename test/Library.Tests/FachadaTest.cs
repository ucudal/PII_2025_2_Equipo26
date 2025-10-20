namespace Library.Tests;
using Library;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Tests
{
    [TestClass]
    public class FachadaTests
    {
        [TestMethod]
        public void CrearCliente_AumentaLaCantidadDeClientesEnUno()
        {
            // Arrange
            var fachada = new Fachada();
            int cantidadInicial = fachada.VerTodosLosClientes().Count;

            // Act
            fachada.CrearCliente("Carlos", "Perez", "123", "cp@test.com");
            int cantidadFinal = fachada.VerTodosLosClientes().Count;

            // Assert
            Assert.AreEqual(cantidadInicial + 1, cantidadFinal);
        }

        [TestMethod]
        public void EliminarCliente_DisminuyeLaCantidadDeClientes()
        {
            // Arrange
            var fachada = new Fachada();
            fachada.CrearCliente("Cliente a borrar", "Test", "456", "b@test.com");
            var idParaBorrar = fachada.VerTodosLosClientes()[0].Id;
            int cantidadInicial = fachada.VerTodosLosClientes().Count;

            // Act
            fachada.EliminarCliente(idParaBorrar);
            int cantidadFinal = fachada.VerTodosLosClientes().Count;

            // Assert
            Assert.AreEqual(cantidadInicial - 1, cantidadFinal);
        }
        
        [TestMethod]
        public void RegistrarLlamada_AgregaInteraccionAlClienteCorrecto()
        {
            // Arrange
            var fachada = new Fachada();
            fachada.CrearCliente("Cliente con llamada", "Test", "789", "c@test.com");
            var cliente = fachada.VerTodosLosClientes()[0];
            
            // Act
            fachada.RegistrarLlamada(cliente.Id, System.DateTime.Now, "Tema de prueba", "Entrante");
            
            // Assert
            Assert.AreEqual(1, cliente.Interacciones.Count);
            Assert.IsInstanceOfType(cliente.Interacciones[0], typeof(Llamada));
        }
    }
}