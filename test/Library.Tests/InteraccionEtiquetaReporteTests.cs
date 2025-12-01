using NUnit.Framework;
using Library;
using System;
using System.Linq; // Necesario para .First(), .Any(), .Count()

namespace Library.Tests
{
    /// <summary>
    /// Pruebas de escenario que validan la lógica de negocio y la
    /// coordinación entre múltiples clases (Clientes, Usuarios, Interacciones, Etiquetas).
    /// </summary>
    [TestFixture]
    public class InteraccionEtiquetaReporteTests
    {
        /// <summary>
        /// Instancia de la fachada que se probará.
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
        
        /// <summary>
        /// IDs de prueba para Cliente y Vendedor
        /// </summary>
        private int clienteId;
        private int vendedorId;

        /// <summary>
        /// Prepara el entorno para CADA prueba unitaria.
        /// Crea instancias "frescas" de los repositorios y las
        /// inyecta en una nueva instancia de Fachada (Inyección de Dependencias).
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            this.repoClientes = new RepoClientes();
            this.repoEtiquetas = new RepoEtiquetas();
            this.repoUsuarios = new RepoUsuarios();
            this.repoVentas = new RepoVentas();
            this._fachada = new Fachada(this.repoClientes, this.repoEtiquetas, this.repoUsuarios, this.repoVentas);

            /// <summary>
            /// Creamos data base para los tests de este archivo
            /// </summary>
            this._fachada.CrearCliente("Cliente", "Prueba", "123456", "cliente@test.com", "F", DateTime.Now);
            this.clienteId = this._fachada.VerTodosLosClientes()[0].Id;

            /// <summary>
            /// CORREGIDO: Llamada a CrearUsuario con 2 argumentos (string, Rol)
            /// </summary>
            this._fachada.CrearUsuario("Vendedor", Rol.Vendedor);
            this.vendedorId = this._fachada.VerTodosLosUsuarios()[0].Id;
        }

        // --- Tests de Asignación (Coordinación) ---

        /// <summary>
        /// Prueba el escenario de coordinación donde la Fachada asigna
        /// un Vendedor (Usuario) a un Cliente.
        /// </summary>
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
            Assert.AreEqual("Vendedor", clienteActualizado.VendedorAsignado.NombreUsuario);
        }

        /// <summary>
        /// Prueba la regla de negocio que impide asignar un Usuario
        /// con Rol 'Administrador' como vendedor de un cliente.
        /// </summary>

        // --- Tests de Interacciones (Polimorfismo) ---

        /// <summary>
        /// Verifica el uso de Polimorfismo.
        /// Prueba que se puedan registrar diferentes tipos de Interaccion
        /// (Llamada, Reunion, Correo) en la misma lista de un Cliente.
        /// </summary>
        [Test]
        public void TestRegistrarInteraccionesPolimorficas()
        {
            DateTime fechaLlamada = new DateTime(2025, 1, 1);
            DateTime fechaReunion = new DateTime(2025, 1, 2);
            DateTime fechaCorreo = new DateTime(2025, 1, 3);
            
            this._fachada.RegistrarLlamada(this.clienteId, fechaLlamada, "Consulta", "Entrante");
            this._fachada.RegistrarReunion(this.clienteId, fechaReunion, "Demo", "Oficina");
            this._fachada.RegistrarCorreo(this.clienteId, fechaCorreo, "Seguimiento", "vendedor@mail.com", "cliente@mail.com", "RE: Demo");

            /// <summary>
            /// CORREGIDO: Nombre de método 'VerInteraccionesCliente'
            /// </summary>
            var interacciones = this._fachada.VerInteraccionesCliente(this.clienteId);
            Assert.AreEqual(3, interacciones.Count);

            /// <summary>
            /// CORREGIDO: Verifica usando la propiedad 'Tipo' (enum)
            /// Esto soluciona la 'Ambiguedad' del Assert.IsTrue
            /// </summary>
            Assert.AreEqual(1, interacciones.Count(i => i.Tipo == TipoInteraccion.Llamada));
            Assert.AreEqual(1, interacciones.Count(i => i.Tipo == TipoInteraccion.Reunion));
            Assert.AreEqual(1, interacciones.Count(i => i.Tipo == TipoInteraccion.Correo));

            Assert.AreEqual("Consulta", interacciones.First(i => i.Tipo == TipoInteraccion.Llamada).Tema);
            Assert.AreEqual("Demo", interacciones.First(i => i.Tipo == TipoInteraccion.Reunion).Tema);
        }

        /// <summary>
        /// Comprueba el filtrado de interacciones por su tipo específico,
        /// usando el método sobrecargado que recibe un 'TipoInteraccion'.
        /// </summary>
        [Test]
        public void TestFiltrarInteraccionesPorTipo()
        {
            this._fachada.RegistrarLlamada(this.clienteId, DateTime.Now, "Consulta 1", "Entrante");
            this._fachada.RegistrarLlamada(this.clienteId, DateTime.Now, "Consulta 2", "Saliente");
            this._fachada.RegistrarReunion(this.clienteId, DateTime.Now, "Demo", "Oficina");

            /// <summary>
            /// CORREGIDO: Nombre de método y uso del enum TipoInteraccion
            /// </summary>
            var llamadas = this._fachada.VerInteraccionesCliente(this.clienteId, TipoInteraccion.Llamada);
            var reuniones = this._fachada.VerInteraccionesCliente(this.clienteId, TipoInteraccion.Reunion);
            var correos = this._fachada.VerInteraccionesCliente(this.clienteId, TipoInteraccion.Correo);

            Assert.AreEqual(2, llamadas.Count);
            Assert.AreEqual(1, reuniones.Count);
            Assert.AreEqual(0, correos.Count);
        }
        
        // --- Tests de Etiquetas (Agregación) ---

        /// <summary>
        /// Prueba la relación de agregación entre Cliente y Etiqueta.
        /// Verifica que se puedan crear etiquetas globales, asignarlas
        /// a un cliente y quitarlas.
        /// </summary>
        [Test]
        public void TestAsignarYQuitarEtiquetaACliente()
        {
            this._fachada.CrearEtiqueta("VIP");
            this._fachada.CrearEtiqueta("Competencia");
            
            var etiquetaVip = this._fachada.VerTodasLasEtiquetas().First(e => e.Nombre == "VIP");
            var etiquetaComp = this._fachada.VerTodasLasEtiquetas().First(e => e.Nombre == "Competencia");

            /// <summary>
            /// CORREGIDO: Nombre de método 'AgregarEtiquetaCliente'
            /// </summary>
            this._fachada.AgregarEtiquetaCliente(this.clienteId, etiquetaVip.Id);
            this._fachada.AgregarEtiquetaCliente(this.clienteId, etiquetaComp.Id);

            var cliente = this._fachada.BuscarCliente(this.clienteId);
            Assert.AreEqual(2, cliente.Etiquetas.Count);
            Assert.IsTrue(cliente.Etiquetas.Contains(etiquetaVip));

            /// <summary>
            /// CORREGIDO: Nombre de método 'QuitarEtiquetaCliente'
            /// </summary>
            this._fachada.QuitarEtiquetaCliente(this.clienteId, etiquetaVip.Id);

            var clienteActualizado = this._fachada.BuscarCliente(this.clienteId);
            Assert.AreEqual(1, clienteActualizado.Etiquetas.Count);
            Assert.IsFalse(clienteActualizado.Etiquetas.Contains(etiquetaVip));
            Assert.IsTrue(clienteActualizado.Etiquetas.Contains(etiquetaComp));
        }

        // --- Tests de Ventas y Reportes ---

        /// <summary>
        /// Prueba el registro de ventas generales (no asignadas a cliente)
        /// y la lógica del reporte 'CalcularTotalVentas' por rango de fechas.
        /// </summary>
        [Test]
        public void TestRegistrarVentaGeneralYReporte()
        {
            // 1. ARRANGE
            // Definimos las fechas
            DateTime fecha1 = new DateTime(2025, 10, 5);
            DateTime fecha2 = new DateTime(2025, 10, 15);
            DateTime fecha3 = new DateTime(2025, 10, 25); // Esta venta queda fuera del rango

            // 2. ACT
            // CORRECCIÓN: Ahora le pasamos 'this.clienteId' (creado en el SetUp)
            // para cumplir con la nueva firma del método RegistrarVenta(int, string, float, DateTime).
            this._fachada.RegistrarVenta(this.clienteId, "Producto A", 100, fecha1);
            this._fachada.RegistrarVenta(this.clienteId, "Producto B", 50, fecha2);
            this._fachada.RegistrarVenta(this.clienteId, "Producto C", 200, fecha3); 

            // 3. ASSERT
            // Definimos el rango del reporte
            DateTime inicioReporte = new DateTime(2025, 10, 1);
            DateTime finReporte = new DateTime(2025, 10, 20);
    
            // Calculamos el total
            float total = this._fachada.CalcularTotalVentas(inicioReporte, finReporte);
    
            // Verificamos que sume solo las ventas dentro del rango (100 + 50 = 150)
            Assert.AreEqual(150, total); 
        }

        /// <summary>
        /// Verifica que una venta pueda ser registrada y
        /// agregada al historial de un cliente específico.
        /// </summary>
        [Test]
        public void TestRegistrarVentaAsignadaACliente()
        {
            // 1. ARRANGE
            var cliente = this._fachada.BuscarCliente(this.clienteId);
            Assert.AreEqual(0, cliente.Ventas.Count); // Pre-condición

            DateTime fechaVenta = DateTime.Now; // Definimos la fecha

            // 2. ACT
            // Agregamos la fecha como 4to parámetro en ambas llamadas
            this._fachada.RegistrarVenta(this.clienteId, "Servicio Premium", 1500, fechaVenta);
            this._fachada.RegistrarVenta(this.clienteId, "Soporte", 500, fechaVenta);

            // 3. ASSERT
            var clienteActualizado = this._fachada.BuscarCliente(this.clienteId);
    
            Assert.AreEqual(2, clienteActualizado.Ventas.Count);
    
            // Verificamos datos de la primera venta
            Assert.AreEqual(1500, clienteActualizado.Ventas[0].Importe);
            Assert.AreEqual(fechaVenta, clienteActualizado.Ventas[0].Fecha); // Verificamos fecha
    
            // Verificamos datos de la segunda venta
            Assert.AreEqual("Soporte", clienteActualizado.Ventas[1].Producto);
        }
        /// <summary>
        /// Prueba la lógica del reporte de clientes inactivos.
        /// Un cliente es inactivo si no tiene interacciones o si su
        /// última interacción fue anterior a la fecha límite.
        /// </summary>
        [Test]
        public void TestObtenerClientesInactivos()
        {
            // 1. Cliente nuevo (creado en SetUp), sin interacciones. ID = this.clienteId
            
            // 2. Cliente activo (interacción reciente)
            this._fachada.CrearCliente("Activo", "User", "777", "activo@mail.com", "M", DateTime.Now);
            int clienteActivoId = this._fachada.VerTodosLosClientes().First(c => c.Nombre == "Activo").Id;
            this._fachada.RegistrarLlamada(clienteActivoId, DateTime.Now.AddDays(-10), "Consulta", "Entrante");

            // 3. Cliente inactivo (interacción antigua)
            this._fachada.CrearCliente("Inactivo", "User", "888", "inactivo@mail.com", "F", DateTime.Now);
            int clienteInactivoId = this._fachada.VerTodosLosClientes().First(c => c.Nombre == "Inactivo").Id;
            this._fachada.RegistrarLlamada(clienteInactivoId, DateTime.Now.AddDays(-40), "Consulta vieja", "Entrante");

            // Buscamos clientes sin interacción en los últimos 30 días
            var inactivos = this._fachada.ObtenerClientesInactivos(30);

            Assert.AreEqual(2, inactivos.Count);
            // Debe encontrar al cliente nuevo (ID = this.clienteId) y al "Inactivo"
            Assert.IsTrue(inactivos.Any(c => c.Id == this.clienteId));
            Assert.IsTrue(inactivos.Any(c => c.Nombre == "Inactivo"));
            Assert.IsFalse(inactivos.Any(c => c.Nombre == "Activo"));
        }
        
        /// <summary>
        /// Prueba la correcta composición del objeto ResumenDashboard.
        /// Verifica el total de clientes, el orden de interacciones
        /// recientes (más nuevas primero) y el orden de reuniones
        /// próximas (más cercanas primero).
        /// </summary>
        [Test]
        public void TestObtenerResumenDashboard()
        {
            this._fachada.RegistrarLlamada(this.clienteId, DateTime.Now.AddDays(1), "Llamada futura", "Saliente"); // No debe aparecer en recientes
            this._fachada.RegistrarReunion(this.clienteId, DateTime.Now.AddDays(5), "Reunion Proxima 1", "Zoom");
            this._fachada.RegistrarReunion(this.clienteId, DateTime.Now.AddDays(2), "Reunion Proxima 2 (Mas cercana)", "Oficina");
            this._fachada.RegistrarLlamada(this.clienteId, DateTime.Now.AddDays(-1), "Llamada Reciente 1", "Entrante");
            this._fachada.RegistrarCorreo(this.clienteId, DateTime.Now.AddDays(-2), "Correo Reciente 2", "a", "b", "c");

            var dashboard = this._fachada.ObtenerResumenDashboard();

            // Solo el cliente del SetUp
            Assert.AreEqual(1, dashboard.TotalClientes);
            
            // Solo las 2 del pasado, en orden descendente (más nueva primero)
            Assert.AreEqual(2, dashboard.InteraccionesRecientes.Count);
            Assert.AreEqual("Llamada Reciente 1", dashboard.InteraccionesRecientes[0].Tema);
            Assert.AreEqual("Correo Reciente 2", dashboard.InteraccionesRecientes[1].Tema);

            // Las 2 del futuro, en orden ascendente (más cercana primero)
            Assert.AreEqual(2, dashboard.ReunionesProximas.Count);
            Assert.AreEqual("Reunion Proxima 2 (Mas cercana)", dashboard.ReunionesProximas[0].Tema);
            Assert.AreEqual("Reunion Proxima 1", dashboard.ReunionesProximas[1].Tema);
        }
    }
}