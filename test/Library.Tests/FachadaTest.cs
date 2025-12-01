using NUnit.Framework;
using Library;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Library.Tests
{
    /// <summary>
    /// Pruebas unitarias para los métodos de la Fachada relacionados
    /// con la gestión de Clientes (Crear, Modificar, Eliminar, Buscar).
    /// </summary>
    [TestFixture]
    public class FachadaTest
    {
        /// <summary>
        /// La instancia de la fachada que se probará.
        /// </summary>
        private FachadaUnit _fachadaUnit;

        /// <summary>
        /// Repositorios "mock" o "en memoria" que se inyectarán
        /// en la fachada antes de cada prueba.
        /// </summary>
        private IRepoClientes repoClientes;
        private IRepoEtiquetas repoEtiquetas;
        private IRepoUsuarios repoUsuarios;
        private IRepoVentas repoVentas;

        [SetUp]
        public void SetUp()
        {
            /// <summary>
            /// Asumimos que las implementaciones concretas (RepoClientes, etc.)
            /// están definidas en el proyecto Library.
            /// </summary>
            this.repoClientes = new RepoClientes();
            this.repoEtiquetas = new RepoEtiquetas();
            this.repoUsuarios = new RepoUsuarios();
            this.repoVentas = new RepoVentas();

            /// <summary>
            /// Inyectamos las dependencias en la fachada.
            /// </summary>
            this._fachadaUnit = new FachadaUnit(this.repoClientes, this.repoEtiquetas, this.repoUsuarios, this.repoVentas);
        }

        /// <summary>
        /// Verifica que se pueda crear un cliente correctamente.
        /// NOTA: Se usa "Masculino" como string para coincidir con el enum.
        /// </summary>
        [Test]
        public void TestCrearCliente()
        {
            string nombre = "Juan";
            string apellido = "Perez";
            string genero = "Masculino"; // Usamos el nombre completo del enum.
            DateTime fechaNac = new DateTime(1990, 5, 15);

            /// <summary>
            /// Actuamos (Llamamos al método de la fachada)
            /// </summary>
            this._fachadaUnit.CrearCliente(nombre, apellido, "099123456", "juan@perez.com", genero, fechaNac);

            /// <summary>
            /// Verificamos 
            /// </summary>
            var clientes = this._fachadaUnit.VerTodosLosClientes();
            
            Assert.AreEqual(1, clientes.Count);
            Assert.AreEqual(nombre, clientes[0].Nombre);
            Assert.AreEqual(apellido, clientes[0].Apellido);
            Assert.AreEqual(fechaNac, clientes[0].FechaNacimiento);
            Assert.AreEqual(GeneroCliente.Masculino, clientes[0].Genero); // Comprobamos el tipo enum
            Assert.AreEqual(1, clientes[0].Id); 
        }

        /// <summary>
        /// Verifica que los datos de un cliente existente se puedan modificar
        /// correctamente.
        /// NOTA: Se usa "Femenino" como string para la modificación.
        /// </summary>
        [Test]
        public void TestModificarCliente()
        {
            /// <summary>
            /// Arrange: Preparamos el escenario creando un cliente.
            /// </summary>
            this._fachadaUnit.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "Masculino", DateTime.Now); // CORRECCIÓN
            var cliente = this._fachadaUnit.VerTodosLosClientes()[0];
            
            string nuevoNombre = "Juan Modificado";
            string nuevoCorreo = "nuevo@correo.com";
            string nuevoGenero = "Femenino"; // Nuevo valor
            DateTime nuevaFecha = new DateTime(1985, 10, 20); // Nuevo valor

            /// <summary>
            /// Act: Ejecutamos la lógica a probar.
            /// </summary>
            this._fachadaUnit.ModificarCliente(cliente.Id, nuevoNombre, cliente.Apellido, cliente.Telefono, 
                                          nuevoCorreo, nuevoGenero, nuevaFecha); 

            /// <summary>
            /// Assert: Verificamos que los cambios se hayan aplicado.
            /// </summary>
            var clienteModificado = this._fachadaUnit.BuscarCliente(cliente.Id);
            Assert.IsNotNull(clienteModificado);
            Assert.AreEqual(nuevoNombre, clienteModificado.Nombre);
            Assert.AreEqual(nuevoCorreo, clienteModificado.Correo);
            Assert.AreEqual(GeneroCliente.Femenino, clienteModificado.Genero); // Verificamos el enum
            Assert.AreEqual(nuevaFecha, clienteModificado.FechaNacimiento);
        }

        /// <summary>
        /// Verifica el nuevo método de RegistrarDatosAdicionalesCliente.
        /// Este test se enfoca en la nueva funcionalidad.
        /// </summary>
        [Test]
        public void TestRegistrarDatosAdicionalesCliente_ActualizaSoloDatosEspecificos()
        {
            /// <summary>
            /// Arrange: Creamos un cliente con datos iniciales (genero por defecto y fecha antigua).
            /// </summary>
            DateTime fechaInicial = new DateTime(2000, 1, 1);
            this._fachadaUnit.CrearCliente("Ana", "Gomez", "999888777", "ana@test.com", "NoEspecificado", fechaInicial); 
            var idCliente = this._fachadaUnit.VerTodosLosClientes()[0].Id;
            
            // Datos que no deberían cambiar
            string nombreOriginal = this._fachadaUnit.BuscarCliente(idCliente).Nombre;

            // Nuevos datos a registrar
            string generoNuevoTexto = "Femenino";
            DateTime fechaNueva = new DateTime(1995, 12, 25); 

            /// <summary>
            /// Act: Llamamos al método que solo actualiza los datos adicionales.
            /// </summary>
            this._fachadaUnit.RegistrarDatosAdicionalesCliente(idCliente, generoNuevoTexto, fechaNueva);

            /// <summary>
            /// Assert: Verificamos la actualización y que los otros datos sigan intactos.
            /// </summary>
            var clienteActualizado = this._fachadaUnit.BuscarCliente(idCliente);
            
            // 1. Verificamos que los datos nuevos se hayan actualizado correctamente
            Assert.AreEqual(GeneroCliente.Femenino, clienteActualizado.Genero);
            Assert.AreEqual(fechaNueva, clienteActualizado.FechaNacimiento);
            
            // 2. Verificamos que el nombre (u otro dato básico) no haya cambiado 
            Assert.AreEqual(nombreOriginal, clienteActualizado.Nombre, "El nombre no debería haber cambiado.");
        }


        /// <summary>
        /// Verifica que un cliente pueda ser eliminado del sistema
        /// usando su ID.
        /// </summary>
        [Test]
        public void TestEliminarCliente()
        {
            /// <summary>
            /// Arrange
            /// </summary>
            this._fachadaUnit.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "Masculino", DateTime.Now); // CORRECCIÓN
            Assert.AreEqual(1, this._fachadaUnit.VerTodosLosClientes().Count);
            var clienteId = this._fachadaUnit.VerTodosLosClientes()[0].Id;

            /// <summary>
            /// Act
            /// </summary>
            this._fachadaUnit.EliminarCliente(clienteId);

            /// <summary>
            /// Assert
            /// </summary>
            Assert.AreEqual(0, this._fachadaUnit.VerTodosLosClientes().Count);
            Assert.IsNull(this._fachadaUnit.BuscarCliente(clienteId));
        }

        /// <summary>
        /// Prueba la funcionalidad de búsqueda de clientes.
        /// </summary>
        [Test]
        public void TestBuscarClientes()
        {
            /// <summary>
            /// Arrange
            /// </summary>
            this._fachadaUnit.CrearCliente("Juan", "Perez", "111111", "juan@mail.com", "Masculino", DateTime.Now); // CORRECCIÓN
            this._fachadaUnit.CrearCliente("Juana", "Gonzalez", "222222", "juana@mail.com", "Femenino", DateTime.Now); // CORRECCIÓN
            this._fachadaUnit.CrearCliente("Pedro", "Gomez", "333333", "pedro@mail.com", "Masculino", DateTime.Now); // CORRECCIÓN

            /// <summary>
            /// Act
            /// </summary>
            var resultadoBusqueda = this._fachadaUnit.BuscarClientes("Juan");

            /// <summary>
            /// Assert
            /// </summary>
            Assert.AreEqual(2, resultadoBusqueda.Count);
            Assert.IsTrue(resultadoBusqueda.Any(c => c.Nombre == "Juan"));
            Assert.IsTrue(resultadoBusqueda.Any(c => c.Nombre == "Juana"));
        } 
        /// <summary>
        /// Verifica que el método ObtenerClientesInactivos funcione correctamente
        /// al identificar clientes cuya última interacción es anterior al límite de días.
        /// Este test comprueba la Delegación de la lógica de negocio desde la Fachada al Cliente.
        /// </summary>
        [Test]
     public void TestObtenerClientesInactivos()
        {
         /// Arrange: Crear clientes con diferentes fechas de interacción.
            
            // --- PREPARACIÓN DE DATOS ---
        
            // Cliente Activo (Interacción reciente, hace 5 días)
            this._fachadaUnit.CrearCliente("Activo", "Reciente", "1", "a@a.com", "Otro", DateTime.Now);
            // Asume que RegistrarLlamada ya existe y crea una Interaccion con fecha.
            this._fachadaUnit.RegistrarLlamada(1, DateTime.Now.AddDays(-5), "Reciente", "entrante"); 

            // Cliente Inactivo (Interacción antigua, hace 20 días)
            this._fachadaUnit.CrearCliente("Inactivo", "Antiguo", "2", "i@i.com", "Otro", DateTime.Now);
            this._fachadaUnit.RegistrarLlamada(2, DateTime.Now.AddDays(-20), "Antigua", "saliente");

            // Cliente Nuevo (Nunca tuvo interacción, su fecha es DateTime.MinValue)
            this._fachadaUnit.CrearCliente("Nunca", "Visto", "3", "n@n.com", "Otro", DateTime.Now);

            // Cliente de Límite (Interacción justo antes del límite de 15 días - NO DEBE APARECER)
            // Usamos AddHours(1) para asegurarnos que está JUSTO fuera del rango de inactividad.
            this._fachadaUnit.CrearCliente("Limite", "Exacto", "4", "l@l.com", "Otro", DateTime.Now);
            this._fachadaUnit.RegistrarLlamada(4, DateTime.Now.AddDays(-15).AddHours(1), "Justo", "saliente");

            // --- DEFINICIÓN DEL LÍMITE ---
            // Establecemos el límite en 15 días.
            int diasLimite = 15; 
            
            /// Act: Ejecutamos el método que queremos probar.
            var inactivos = this._fachadaUnit.ObtenerClientesInactivos(diasLimite);

            /// Assert: Verificamos los resultados.
            
            // Esperamos 2 clientes: Inactivo (20 días) y Nunca (DateTime.MinValue).
            Assert.AreEqual(2, inactivos.Count, "Solo dos clientes deben ser clasificados como inactivos.");
            
            // Verificamos que los clientes correctos estén presentes.
            Assert.IsTrue(inactivos.Any(c => c.Nombre == "Inactivo"), "El cliente con 20 días de antigüedad debe ser inactivo.");
            Assert.IsTrue(inactivos.Any(c => c.Nombre == "Nunca"), "El cliente sin interacciones debe ser inactivo.");
            
            // Verificamos que los clientes activos NO estén presentes.
            Assert.IsFalse(inactivos.Any(c => c.Nombre == "Activo"), "El cliente de 5 días no debe ser inactivo.");
            Assert.IsFalse(inactivos.Any(c => c.Nombre == "Limite"), "El cliente justo en el límite no debe ser inactivo.");
        }
    }
}