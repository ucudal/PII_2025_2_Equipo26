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
        private Fachada _fachada;

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
            this._fachada = new Fachada(this.repoClientes, this.repoEtiquetas, this.repoUsuarios, this.repoVentas);
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
            this._fachada.CrearCliente(nombre, apellido, "099123456", "juan@perez.com", genero, fechaNac);

            /// <summary>
            /// Verificamos 
            /// </summary>
            var clientes = this._fachada.VerTodosLosClientes();
            
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
            this._fachada.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "Masculino", DateTime.Now); // CORRECCIÓN
            var cliente = this._fachada.VerTodosLosClientes()[0];
            
            string nuevoNombre = "Juan Modificado";
            string nuevoCorreo = "nuevo@correo.com";
            string nuevoGenero = "Femenino"; // Nuevo valor
            DateTime nuevaFecha = new DateTime(1985, 10, 20); // Nuevo valor

            /// <summary>
            /// Act: Ejecutamos la lógica a probar.
            /// </summary>
            this._fachada.ModificarCliente(cliente.Id, nuevoNombre, cliente.Apellido, cliente.Telefono, 
                                          nuevoCorreo, nuevoGenero, nuevaFecha); 

            /// <summary>
            /// Assert: Verificamos que los cambios se hayan aplicado.
            /// </summary>
            var clienteModificado = this._fachada.BuscarCliente(cliente.Id);
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
            this._fachada.CrearCliente("Ana", "Gomez", "999888777", "ana@test.com", "NoEspecificado", fechaInicial); 
            var idCliente = this._fachada.VerTodosLosClientes()[0].Id;
            
            // Datos que no deberían cambiar
            string nombreOriginal = this._fachada.BuscarCliente(idCliente).Nombre;

            // Nuevos datos a registrar
            string generoNuevoTexto = "Femenino";
            DateTime fechaNueva = new DateTime(1995, 12, 25); 

            /// <summary>
            /// Act: Llamamos al método que solo actualiza los datos adicionales.
            /// </summary>
            this._fachada.RegistrarDatosAdicionalesCliente(idCliente, generoNuevoTexto, fechaNueva);

            /// <summary>
            /// Assert: Verificamos la actualización y que los otros datos sigan intactos.
            /// </summary>
            var clienteActualizado = this._fachada.BuscarCliente(idCliente);
            
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
            this._fachada.CrearCliente("Juan", "Perez", "099123456", "juan@perez.com", "Masculino", DateTime.Now); // CORRECCIÓN
            Assert.AreEqual(1, this._fachada.VerTodosLosClientes().Count);
            var clienteId = this._fachada.VerTodosLosClientes()[0].Id;

            /// <summary>
            /// Act
            /// </summary>
            this._fachada.EliminarCliente(clienteId);

            /// <summary>
            /// Assert
            /// </summary>
            Assert.AreEqual(0, this._fachada.VerTodosLosClientes().Count);
            Assert.IsNull(this._fachada.BuscarCliente(clienteId));
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
            this._fachada.CrearCliente("Juan", "Perez", "111111", "juan@mail.com", "Masculino", DateTime.Now); // CORRECCIÓN
            this._fachada.CrearCliente("Juana", "Gonzalez", "222222", "juana@mail.com", "Femenino", DateTime.Now); // CORRECCIÓN
            this._fachada.CrearCliente("Pedro", "Gomez", "333333", "pedro@mail.com", "Masculino", DateTime.Now); // CORRECCIÓN

            /// <summary>
            /// Act
            /// </summary>
            var resultadoBusqueda = this._fachada.BuscarClientes("Juan");

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
            this._fachada.CrearCliente("Activo", "Reciente", "1", "a@a.com", "Otro", DateTime.Now);
            // Asume que RegistrarLlamada ya existe y crea una Interaccion con fecha.
            this._fachada.RegistrarLlamada(1, DateTime.Now.AddDays(-5), "Reciente", "entrante"); 

            // Cliente Inactivo (Interacción antigua, hace 20 días)
            this._fachada.CrearCliente("Inactivo", "Antiguo", "2", "i@i.com", "Otro", DateTime.Now);
            this._fachada.RegistrarLlamada(2, DateTime.Now.AddDays(-20), "Antigua", "saliente");

            // Cliente Nuevo (Nunca tuvo interacción, su fecha es DateTime.MinValue)
            this._fachada.CrearCliente("Nunca", "Visto", "3", "n@n.com", "Otro", DateTime.Now);

            // Cliente de Límite (Interacción justo antes del límite de 15 días - NO DEBE APARECER)
            // Usamos AddHours(1) para asegurarnos que está JUSTO fuera del rango de inactividad.
            this._fachada.CrearCliente("Limite", "Exacto", "4", "l@l.com", "Otro", DateTime.Now);
            this._fachada.RegistrarLlamada(4, DateTime.Now.AddDays(-15).AddHours(1), "Justo", "saliente");

            // --- DEFINICIÓN DEL LÍMITE ---
            // Establecemos el límite en 15 días.
            int diasLimite = 15; 
            
            /// Act: Ejecutamos el método que queremos probar.
            var inactivos = this._fachada.ObtenerClientesInactivos(diasLimite);

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
        
        /// <summary>
        /// Verifica que la lógica de AsignarClienteVendedor funcione, incluyendo la 
        /// validación de roles y estados requerida por la historia de usuario.
        /// </summary>
        
        [Test]
        public void TestAsignarClienteVendedor_ValidacionDeRolesYEstado()
        {
            // Arrange: Preparar objetos y roles 
            
            // 1. Clientes
            this._fachada.CrearCliente("Cliente", "Activo", "000", "c@c.com", "Otro", DateTime.Now);
            var clienteId = this._fachada.BuscarCliente(1).Id; // ID 1

            // 2. Usuarios del CRM
            this._fachada.CrearUsuario("VendedorActivo", Rol.Vendedor);      // ID 1
            this._fachada.CrearUsuario("NoVendedor", Rol.Administrador);     // ID 2
            this._fachada.CrearUsuario("VendedorSuspendido", Rol.Vendedor);  // ID 3
            
            // Obtenemos los IDs para la prueba.
            var vendedorActivoId = this._fachada.BuscarUsuario(1).Id;
            var noVendedorId = this._fachada.BuscarUsuario(2).Id;
            var vendedorSuspendidoId = this._fachada.BuscarUsuario(3).Id;
            
            // Suspender al usuario 3 (VendedorSuspendido)
            this._fachada.SuspenderUsuario(vendedorSuspendidoId);
            
            // Act & Assert 1: Asignación Exitosa 
            
            // Act: Asignar al Vendedor Activo
            this._fachada.AsignarClienteVendedor(clienteId, vendedorActivoId);
            
            // Assert: La asignación fue exitosa (el Cliente cambió su VendedorAsignado)
            Assert.IsNotNull(this._fachada.BuscarCliente(clienteId).VendedorAsignado);
            Assert.AreEqual(vendedorActivoId, this._fachada.BuscarCliente(clienteId).VendedorAsignado.Id);
            
            // Act & Assert 2: Falla por Rol Incorrecto (Validación del Cliente/Expert) 
            
            // Act: Intentar asignar a un usuario que es Administrador (NoVendedor)
            // Assert: Esperamos una InvalidOperationException (regla de negocio validada por Cliente)
            Assert.Throws<InvalidOperationException>(() =>
            {
                this._fachada.AsignarClienteVendedor(clienteId, noVendedorId);
            }, "Debe fallar porque el usuario no tiene el rol Vendedor.");
            
            // --- Act & Assert 3: Falla por Estado Suspendido (Validación del Cliente/Expert) ---
            
            // Act: Intentar asignar a un usuario que tiene el rol, pero está suspendido.
            // Assert: Esperamos una InvalidOperationException (regla de negocio validada por Cliente)
            Assert.Throws<InvalidOperationException>(() =>
            {
                this._fachada.AsignarClienteVendedor(clienteId, vendedorSuspendidoId);
            }, "Debe fallar porque el vendedor está suspendido.");
            
            // --- Act & Assert 4: Falla por Cliente Inexistente (Precondición de la Fachada) ---
            
            // Act: Intentar asignar un cliente con un ID que no existe (99)
            // Assert: Esperamos una KeyNotFoundException (Validación de Precondición por la Fachada)
            Assert.Throws<KeyNotFoundException>(() =>
            {
                this._fachada.AsignarClienteVendedor(99, vendedorActivoId);
            }, "Debe fallar porque el cliente no existe.");
        }
        
        /// <summary>
        /// Verifica que el método CalcularTotalVentas sume correctamente solo las ventas
        /// que caen dentro del rango de fechas especificado (inclusivo).
        /// </summary>
        [Test]
        public void TestCalcularTotalVentas_FiltraYOperaCorrectamente()
        {
            // --- Arrange: Preparar el escenario ---
            
            // 1. Crear un Cliente, ya que RegistrarVenta ahora requiere un clienteId.
            // Esto asegura que el cliente ID 1 exista.
            this._fachada.CrearCliente("Reporte", "User", "0", "r@r.com", "Otro", new DateTime(1990, 1, 1)); 
            // Obtenemos el ID del cliente creado, que será 1.
            var clienteId = this._fachada.VerTodosLosClientes()[0].Id; 
            
            // 2. Ventas con 4 parámetros: (clienteId, producto, monto, fecha)
            
            // Ventas fuera del período:
            this._fachada.RegistrarVenta(clienteId, "Venta Antigua", 1000.00f, new DateTime(2024, 1, 1)); // Fuera
            this._fachada.RegistrarVenta(clienteId, "Venta Futura", 500.00f, new DateTime(2025, 6, 1)); // Fuera
            
            // Ventas dentro del período:
            this._fachada.RegistrarVenta(clienteId, "Venta A", 200.00f, new DateTime(2025, 2, 1)); // Dentro
            this._fachada.RegistrarVenta(clienteId, "Venta B", 300.00f, new DateTime(2025, 2, 15)); // Dentro
            
            // Ventas en el Límite:
            this._fachada.RegistrarVenta(clienteId, "Venta Limite Inicio", 150.00f, new DateTime(2025, 2, 1).AddHours(1)); // Dentro
            
            // Usamos el día 28.
            this._fachada.RegistrarVenta(clienteId, "Venta Limite Fin", 50.00f, new DateTime(2025, 2, 28).AddHours(-1)); // Dentro
            
            // Definición del Período: Febrero 2025 (inclusive)
            DateTime inicio = new DateTime(2025, 2, 1);
            DateTime fin = new DateTime(2025, 2, 28); 
            
            // Calculo manual esperado: 200 + 300 + 150 + 50 = 700.00
            float totalEsperado = 700.00f;
            
            // --- Act: Ejecutar el método ---
            float totalObtenido = this._fachada.CalcularTotalVentas(inicio, fin);
            
            // --- Assert: Verificación de la suma y el filtro ---
            Assert.AreEqual(totalEsperado, totalObtenido, 0.001f, "El total de ventas debe coincidir con la suma del período filtrado.");
        }
        
        /// <summary>
        /// Verifica que el método ObtenerResumenDashboard compile correctamente
        /// la información de clientes totales, reuniones futuras e interacciones pasadas.
        /// </summary>
        [Test]
        public void TestObtenerResumenDashboard_AgregacionYFiltro()
        {
            // --- Arrange ---
            
            // 1. Clientes
            this._fachada.CrearCliente("C1", "A", "1", "c1@c.com", "Otro", DateTime.Now); // ID 1
            this._fachada.CrearCliente("C2", "B", "2", "c2@c.com", "Otro", DateTime.Now); // ID 2
            
            // 2. Interacciones
            // Interacción Reciente (PASADA)
            this._fachada.RegistrarLlamada(1, DateTime.Now.AddHours(-2), "Llamada Reciente", "entrante"); 
            // Interacción Antigua (PASADA)
            this._fachada.RegistrarLlamada(1, DateTime.Now.AddDays(-10), "Llamada Antigua", "entrante");
            
            // Reunión Próxima (FUTURA)
            this._fachada.RegistrarReunion(2, DateTime.Now.AddDays(5), "Reunion Venta", "Oficina"); 
            // Reunión Antigua (PASADA - NO DEBE APARECER)
            this._fachada.RegistrarReunion(2, DateTime.Now.AddDays(-1), "Reunion Antigua", "Oficina"); 

            // --- Act ---
            ResumenDashboard resumen = this._fachada.ObtenerResumenDashboard();

            // --- Assert ---
            
            // 1. Clientes Totales
            Assert.AreEqual(2, resumen.TotalClientes, "El conteo de clientes debe ser 2.");
            
            // 2. Reuniones Próximas (Solo 1 reunión futura)
            Assert.AreEqual(1, resumen.ReunionesProximas.Count, "Solo debe haber 1 reunión en la lista de próximas.");
            Assert.AreEqual("Reunion Venta", resumen.ReunionesProximas[0].Tema, "La reunión debe ser la futura.");
            
            // 3. Interacciones Recientes (Solo 3 interacciones pasadas)
            // C1 (Llamada Reciente) + C1 (Llamada Antigua) + C2 (Reunión Antigua) = 3 interacciones pasadas
            // La más reciente debe ser la de hace 2 horas.
            Assert.AreEqual(3, resumen.InteraccionesRecientes.Count, "Debe haber 3 interacciones pasadas.");
            Assert.AreEqual("Llamada Reciente", resumen.InteraccionesRecientes[0].Tema, "La interacción más reciente debe aparecer primera.");
        }
        
        /// <summary>
        /// Verifica específicamente que los datos de contacto (teléfono y correo),
        /// requeridos por la historia de usuario, se guarden correctamente.
        /// </summary>
        [Test]
        public void TestCrearCliente_VerificarDatosDeContacto()
        {
            // Arrange
            string nombre = "Maria";
            string apellido = "Rodriguez";
            string telefono = "099888777";
            string correo = "maria.rod@correo.com";
            string genero = "F";
            DateTime fechaNac = new DateTime(1995, 10, 20);

            // Act
            this.fachada.CrearCliente(nombre, apellido, telefono, correo, genero, fechaNac);

            // Assert
            var listaClientes = this.fachada.VerTodosLosClientes();
            
            // Obtenemos el cliente creado usando índice estándar (sin LINQ)
            // Dado que el SetUp limpia el repo, debería estar en la posición 0.
            var clienteCreado = listaClientes[0];

            Assert.AreEqual(telefono, clienteCreado.Telefono, "El teléfono debe coincidir con el ingresado.");
            Assert.AreEqual(correo, clienteCreado.Correo, "El correo debe coincidir con el ingresado.");
        }
    }
}