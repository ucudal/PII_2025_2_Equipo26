using NUnit.Framework;
using Library;
using System;
using System.Collections.Generic;

namespace Library.Tests
{
    [TestFixture]
    public class FachadaTest
    {
        private Fachada fachada;

        [SetUp]
        public void Setup()
        {
            fachada = new Fachada(); 
        }

        [Test]
        public void Test_CrearYVerCliente()
        {
            // 1. Arrange (Organizar)
            string nombre = "Juan";
            string apellido = "Perez";
            string telefono = "099123456";
            string correo = "juan@perez.com";
            DateTime fechaNac = new DateTime(1990, 1, 1);

            // 2. Act (Actuar)
            fachada.CrearCliente(nombre, apellido, telefono, correo, "Masculino", fechaNac);
            List<Cliente> clientes = fachada.VerTodosLosClientes();

            // 3. Assert (Verificar)
            Assert.AreEqual(1, clientes.Count); // Verificamos que se agregó 1 cliente
            Assert.AreEqual(nombre, clientes[0].Nombre); // Verificamos los datos
            Assert.AreEqual(apellido, clientes[0].Apellido);
            Assert.AreEqual(telefono, clientes[0].Telefono);
        }

        [Test]
        public void Test_ModificarCliente()
        {
            // 1. Arrange (Organizar)
            // Primero, creamos un cliente para poder modificarlo
            fachada.CrearCliente("Ana", "Gomez", "091111111", "ana@gomez.com", "Femenino", new DateTime(1985, 5, 5));
            Cliente clienteCreado = fachada.VerTodosLosClientes()[0];
            int idCliente = clienteCreado.Id;

            // Nuevos datos para la modificación
            string nuevoTelefono = "092222222";
            string nuevoCorreo = "ana.actualizada@gomez.com";

            // 2. Act (Actuar)
            fachada.ModificarCliente(idCliente, "Ana", "Gomez", nuevoTelefono, nuevoCorreo, "Femenino", new DateTime(1985, 5, 5));

            // 3. Assert (Verificar)
            Cliente clienteModificado = fachada.VerTodosLosClientes()[0]; // Sigue habiendo un solo cliente
            
            Assert.AreEqual(idCliente, clienteModificado.Id); // El ID no debe cambiar
            Assert.AreEqual("Ana", clienteModificado.Nombre); // El nombre se mantuvo
            Assert.AreEqual(nuevoTelefono, clienteModificado.Telefono); // El teléfono se actualizó
            Assert.AreEqual(nuevoCorreo, clienteModificado.Correo); // El correo se actualizó
        }

        [Test]
        public void Test_EliminarCliente()
        {
            // 1. Arrange (Organizar)
            // Creamos un cliente para luego eliminarlo
            fachada.CrearCliente("Carlos", "Ruiz", "093333333", "carlos@ruiz.com", "Masculino", new DateTime(1970, 10, 10));
            Cliente clienteCreado = fachada.VerTodosLosClientes()[0];
            int idCliente = clienteCreado.Id;
            
            // Verificación previa: hay 1 cliente
            Assert.AreEqual(1, fachada.VerTodosLosClientes().Count); 

            // 2. Act (Actuar)
            fachada.EliminarCliente(idCliente);

            // 3. Assert (Verificar)
            // La lista de clientes ahora debería estar vacía
            List<Cliente> clientes = fachada.VerTodosLosClientes();
            Assert.AreEqual(0, clientes.Count); 
        }

        [Test]
        public void Test_BuscarClientesPorNombre()
        {
            // 1. Arrange (Organizar)
            // Creamos varios clientes
            fachada.CrearCliente("Maria", "Rodriguez", "094444444", "maria1@mail.com", "Femenino", new DateTime(1995, 2, 2));
            fachada.CrearCliente("Maria", "Lopez", "095555555", "maria2@mail.com", "Femenino", new DateTime(1992, 3, 3));
            fachada.CrearCliente("Pedro", "Gonzalez", "096666666", "pedro@mail.com", "Masculino", new DateTime(1990, 4, 4));

            // 2. Act (Actuar)
            // Buscamos clientes que contengan "Maria" en su nombre (ignora mayúsculas/minúsculas)
            List<Cliente> resultadoBusqueda = fachada.BuscarClientes("maria");

            // 3. Assert (Verificar)
            Assert.AreEqual(2, resultadoBusqueda.Count); // Debe encontrar a las dos Marias
            Assert.AreEqual("Rodriguez", resultadoBusqueda[0].Apellido);
            Assert.AreEqual("Lopez", resultadoBusqueda[1].Apellido);
        }

        [Test]
        public void Test_BuscarClientesPorCorreo()
        {
            // 1. Arrange (Organizar)
            fachada.CrearCliente("Maria", "Rodriguez", "094444444", "maria1@mail.com", "Femenino", new DateTime(1995, 2, 2));
            fachada.CrearCliente("Pedro", "Gonzalez", "096666666", "pedro@mail.com", "Masculino", new DateTime(1990, 4, 4));

            // 2. Act (Actuar)
            // Buscamos por correo electrónico
            List<Cliente> resultadoBusqueda = fachada.BuscarClientes("pedro@mail.com");

            // 3. Assert (Verificar)
            Assert.AreEqual(1, resultadoBusqueda.Count); // Debe encontrar solo a Pedro
            Assert.AreEqual("Pedro", resultadoBusqueda[0].Nombre);
        }

        [Test]
        public void Test_BuscarClientesSinResultados()
        {
            // 1. Arrange (Organizar)
            fachada.CrearCliente("Maria", "Rodriguez", "094444444", "maria1@mail.com", "Femenino", new DateTime(1995, 2, 2));

            // 2. Act (Actuar)
            List<Cliente> resultadoBusqueda = fachada.BuscarClientes("Inexistente");

            // 3. Assert (Verificar)
            Assert.AreEqual(0, resultadoBusqueda.Count); // No debe encontrar nada
        }
    }
}