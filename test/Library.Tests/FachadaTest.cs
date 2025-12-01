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
        /// Verifica específicamente que los datos de contacto (Teléfono y Correo),
        /// los cuales son críticos para la historia de usuario, se guarden correctamente.
        /// </summary>
        [Test]
        public void TestCrearCliente_VerificaDatosDeContacto()
        {
            // Arrange
            // Preparamos variables específicas para el contacto
            string nombre = "Maria";
            string apellido = "Lopez";
            string telefono = "098111222";          // Dato crítico para la historia
            string correo = "maria.contact@mail.com"; // Dato crítico para la historia
            string genero = "Femenino";
            DateTime fecha = new DateTime(1995, 1, 1);

            // Act
            this._fachada.CrearCliente(nombre, apellido, telefono, correo, genero, fecha);

            // Assert
            var listaClientes = this._fachada.VerTodosLosClientes();
            
            // Como el SetUp reinicia todo, el cliente estará en la posición 0
            var clienteCreado = listaClientes[0];

            // Validamos que los datos de contacto coincidan con lo enviado
            Assert.AreEqual(telefono, clienteCreado.Telefono, "El teléfono no se guardó correctamente.");
            Assert.AreEqual(correo, clienteCreado.Correo, "El correo no se guardó correctamente.");
        }
        
        /// <summary>
        /// Verifica que se pueda modificar TODA la información de un cliente (Nombre, Apellido, 
        /// Teléfono, Correo, Género y Fecha). Esto asegura el cumplimiento completo de la 
        /// historia de usuario de "mantener actualizada" la información.
        /// </summary>
        [Test]
        public void TestModificarCliente_ActualizacionTotal()
        {
            // Arrange
            // 1. Creamos el cliente original
            this._fachada.CrearCliente("Original", "Viejo", "099000111", "viejo@test.com", "Masculino", new DateTime(1980, 1, 1));
            
            // Obtenemos el cliente para sacar su ID (estará en posición 0)
            var lista = this._fachada.VerTodosLosClientes();
            var idCliente = lista[0].Id;

            // 2. Definimos los NUEVOS datos para cambiar absolutamente todo
            string nuevoNombre = "Modificado";
            string nuevoApellido = "Nuevo";
            string nuevoTelefono = "098222333";
            string nuevoCorreo = "nuevo@test.com";
            string nuevoGenero = "Femenino";
            DateTime nuevaFecha = new DateTime(1995, 5, 5);

            // Act
            // Ejecutamos la modificación pasando todos los nuevos valores
            this._fachada.ModificarCliente(idCliente, nuevoNombre, nuevoApellido, nuevoTelefono, nuevoCorreo, nuevoGenero, nuevaFecha);

            // Assert
            // Recuperamos el cliente actualizado
            var clienteActualizado = this._fachada.BuscarCliente(idCliente);

            Assert.IsNotNull(clienteActualizado, "El cliente debería seguir existiendo.");

            // Verificamos campo por campo
            Assert.AreEqual(nuevoNombre, clienteActualizado.Nombre, "El Nombre no se actualizó.");
            Assert.AreEqual(nuevoApellido, clienteActualizado.Apellido, "El Apellido no se actualizó.");
            Assert.AreEqual(nuevoTelefono, clienteActualizado.Telefono, "El Teléfono no se actualizó.");
            Assert.AreEqual(nuevoCorreo, clienteActualizado.Correo, "El Correo no se actualizó.");
            Assert.AreEqual(GeneroCliente.Femenino, clienteActualizado.Genero, "El Género no se actualizó.");
            Assert.AreEqual(nuevaFecha, clienteActualizado.FechaNacimiento, "La Fecha de Nacimiento no se actualizó.");
        }
        
        /// <summary>
        /// Verifica que la eliminación sea selectiva. 
        /// Crea dos clientes, elimina uno y asegura que el otro permanezca intacto.
        /// Esto garantiza que "mantener limpia la base de datos" no cause pérdida de datos válidos.
        /// </summary>
        [Test]
        public void TestEliminarCliente_BorradoSelectivo()
        {
            // Arrange
            // 1. Creamos dos clientes distintos
            this._fachada.CrearCliente("Cliente", "A_Borrar", "099111111", "borrar@test.com", "Masculino", DateTime.Now);
            this._fachada.CrearCliente("Cliente", "A_Quedar", "099222222", "quedar@test.com", "Femenino", DateTime.Now);

            // Obtenemos sus IDs (sin usar LINQ, por índice)
            var listaClientes = this._fachada.VerTodosLosClientes();
            var clienteBorrar = listaClientes[0];
            var clienteQuedar = listaClientes[1];

            // Act
            // Eliminamos SOLAMENTE al primero
            this._fachada.EliminarCliente(clienteBorrar.Id);

            // Assert
            // 1. Verificamos que el cliente objetivo haya desaparecido
            Assert.IsNull(this._fachada.BuscarCliente(clienteBorrar.Id), "El cliente eliminado no debería existir.");

            // 2. Verificamos que el OTRO cliente siga existiendo y con sus datos correctos
            var clienteSobreviviente = this._fachada.BuscarCliente(clienteQuedar.Id);
            Assert.IsNotNull(clienteSobreviviente, "El cliente que no se eliminó debería seguir existiendo.");
            Assert.AreEqual("A_Quedar", clienteSobreviviente.Apellido, "Los datos del cliente restante no deben alterarse.");

            // 3. Verificamos el conteo total para asegurar que la limpieza fue exacta
            Assert.AreEqual(1, this._fachada.VerTodosLosClientes().Count, "La base de datos debería tener exactamente 1 cliente restante.");
        }
        
        /// <summary>
        /// Verifica los criterios de búsqueda que faltaban en el test anterior:
        /// Apellido, Teléfono y Correo Electrónico.
        /// </summary>
        [Test]
        public void TestBuscarClientes_PorApellidoTelefonoCorreo()
        {
            // Arrange
            // Creamos un cliente con datos únicos para probar específicamente estos campos.
            string apellidoUnico = "Skywalker";
            string telefonoUnico = "099555444";
            string correoUnico = "luke@galaxy.com";
            
            this._fachada.CrearCliente("Luke", apellidoUnico, telefonoUnico, correoUnico, "Masculino", DateTime.Now);

            // Act & Assert 1: Búsqueda por APELLIDO
            // Verificamos que al buscar por apellido, el sistema lo encuentre.
            var porApellido = this._fachada.BuscarClientes(apellidoUnico);
            Assert.AreEqual(1, porApellido.Count, "Debería encontrar al cliente buscando por Apellido.");
            Assert.AreEqual(apellidoUnico, porApellido[0].Apellido);

            // Act & Assert 2: Búsqueda por TELÉFONO
            // Verificamos que al buscar por el número de teléfono, lo encuentre.
            var porTelefono = this._fachada.BuscarClientes(telefonoUnico);
            Assert.AreEqual(1, porTelefono.Count, "Debería encontrar al cliente buscando por Teléfono.");
            Assert.AreEqual(telefonoUnico, porTelefono[0].Telefono);

            // Act & Assert 3: Búsqueda por CORREO
            // Verificamos que al buscar por el email, lo encuentre.
            var porCorreo = this._fachada.BuscarClientes(correoUnico);
            Assert.AreEqual(1, porCorreo.Count, "Debería encontrar al cliente buscando por Correo.");
            Assert.AreEqual(correoUnico, porCorreo[0].Correo);
        }
        
        /// <summary>
        /// Verifica que se pueda recuperar la lista completa de clientes ("vista general de la cartera"),
        /// asegurando que todos los registros creados estén presentes y accesibles.
        /// </summary>
        [Test]
        public void TestVerTodosLosClientes_VistaGeneralDeCartera()
        {
            // Arrange
            // Llenamos la cartera con varios clientes para simular un escenario real
            this._fachada.CrearCliente("Cliente A", "Alpha", "100", "a@test.com", "Masculino", DateTime.Now);
            this._fachada.CrearCliente("Cliente B", "Beta", "200", "b@test.com", "Femenino", DateTime.Now);
            this._fachada.CrearCliente("Cliente C", "Gamma", "300", "c@test.com", "Otro", DateTime.Now);

            // Act
            var cartera = this._fachada.VerTodosLosClientes();

            // Assert
            // 1. Verificamos que la cantidad corresponda al total de clientes ingresados
            Assert.AreEqual(3, cartera.Count, "La lista debe contener exactamente todos los clientes de la cartera.");

            // 2. Verificamos que los datos correspondan a los ingresados (asumiendo orden de inserción)
            // Esto confirma que la "vista" contiene la información correcta de cada uno.
            Assert.AreEqual("Cliente A", cartera[0].Nombre);
            Assert.AreEqual("Cliente B", cartera[1].Nombre);
            Assert.AreEqual("Cliente C", cartera[2].Nombre);
        }
        
        /// <summary>
        /// Verifica que se pueda registrar una llamada asociada a un cliente,
        /// guardando correctamente la fecha, el tema tratado y el tipo de llamada.
        /// </summary>
        [Test]
        public void TestRegistrarLlamada_GuardadoCorrecto()
        {
            // Arrange
            // 1. Creamos un cliente para asociarle la llamada
            this._fachada.CrearCliente("Laura", "Martinez", "091222333", "laura@call.com", "Femenino", DateTime.Now);
            
            // Obtenemos el ID del cliente (estará en la posición 0 tras el SetUp)
            var listaClientes = this._fachada.VerTodosLosClientes();
            var cliente = listaClientes[0];

            // 2. Preparamos los datos de la llamada
            DateTime fechaLlamada = new DateTime(2023, 11, 15, 10, 30, 0);
            string temaTratado = "Consulta sobre renovación de contrato";
            string tipoLlamada = "entrante"; // "entrante", "saliente" o "recibida" según tu lógica

            // Act
            // Registramos la llamada usando la fachada
            this._fachada.RegistrarLlamada(cliente.Id, fechaLlamada, temaTratado, tipoLlamada);

            // Assert
            // 1. Recuperamos las interacciones del cliente
            var clienteActualizado = this._fachada.BuscarCliente(cliente.Id);
            var historial = clienteActualizado.Interacciones;

            // 2. Verificamos que se haya agregado la interacción
            Assert.IsTrue(historial.Count > 0, "El historial del cliente no debería estar vacío.");

            // 3. Obtenemos la última interacción agregada (sin usar LINQ)
            // Asumimos que es la última de la lista
            var ultimaInteraccion = historial[historial.Count - 1];

            // 4. Verificamos que sea del tipo correcto (Llamada)
            Assert.IsInstanceOf<Llamada>(ultimaInteraccion, "La interacción registrada debería ser de tipo Llamada.");

            // 5. Verificamos los datos específicos requeridos por la historia
            Llamada llamadaRegistrada = (Llamada)ultimaInteraccion;
            
            Assert.AreEqual(fechaLlamada, llamadaRegistrada.Fecha, "La fecha y hora de la llamada deben coincidir.");
            // CORREGIDO: Usamos .Tema en lugar de .Comentario
            Assert.AreEqual(temaTratado, llamadaRegistrada.Tema, "El tema tratado debe coincidir.");
            // CORREGIDO: Usamos .TipoLlamada en lugar de .Estado
            Assert.AreEqual(tipoLlamada, llamadaRegistrada.TipoLlamada, "El tipo de llamada (entrante/saliente) debe coincidir."); 
        }
        
        /// <summary>
        /// Verifica que se pueda registrar una reunión, validando que se persistan 
        /// correctamente la fecha, el tema y, fundamentalmente, el LUGAR de la reunión.
        /// </summary>
        [Test]
        public void TestRegistrarReunion_GuardadoCorrecto()
        {
            // Arrange
            // 1. Creamos un cliente para la reunión
            this._fachada.CrearCliente("Sofia", "Reuniones", "098000111", "sofia@meet.com", "Femenino", DateTime.Now);
            
            // Recuperamos el cliente (índice 0 tras el SetUp)
            var listaClientes = this._fachada.VerTodosLosClientes();
            var cliente = listaClientes[0];

            // 2. Preparamos los datos de la reunión
            DateTime fechaReunion = new DateTime(2024, 5, 20, 14, 0, 0);
            string tema = "Negociación de presupuesto anual";
            string lugar = "Oficinas Centrales - Sala 4";

            // Act
            this._fachada.RegistrarReunion(cliente.Id, fechaReunion, tema, lugar);

            // Assert
            // 1. Recuperamos las interacciones
            var clienteActualizado = this._fachada.BuscarCliente(cliente.Id);
            var historial = clienteActualizado.Interacciones;

            // 2. Verificamos que se haya guardado
            Assert.IsTrue(historial.Count > 0, "Debería haber al menos una interacción registrada.");

            // 3. Obtenemos la última interacción y verificamos su tipo
            var ultimaInteraccion = historial[historial.Count - 1];
            Assert.IsInstanceOf<Reunion>(ultimaInteraccion, "La interacción debería ser de tipo Reunion.");

            // 4. Casteamos y verificamos los datos
            Reunion reunionGuardada = (Reunion)ultimaInteraccion;

            Assert.AreEqual(fechaReunion, reunionGuardada.Fecha, "La fecha de la reunión no coincide.");
            Assert.AreEqual(tema, reunionGuardada.Tema, "El tema de la reunión no coincide.");
            Assert.AreEqual(lugar, reunionGuardada.Lugar, "El lugar de la reunión no se guardó correctamente.");
        }
        
        /// <summary>
        /// Verifica que se pueda registrar un mensaje (SMS/WhatsApp, etc.) asociado al cliente.
        /// Valida que se guarden la fecha, el tema, quién lo envió (Remitente) y quién lo recibió (Destinatario).
        /// </summary>
        [Test]
        public void TestRegistrarMensaje_GuardadoCorrecto()
        {
            // Arrange
            // 1. Creamos un cliente de prueba
            this._fachada.CrearCliente("Lucas", "Mensajero", "099123123", "lucas@msg.com", "Masculino", DateTime.Now);
            
            // Obtenemos el cliente (posición 0)
            var listaClientes = this._fachada.VerTodosLosClientes();
            var cliente = listaClientes[0];

            // 2. Preparamos los datos del mensaje
            DateTime fechaMensaje = new DateTime(2024, 8, 10, 9, 15, 0);
            string tema = "Consulta de stock";
            string remitente = "099123123"; // El cliente envía el mensaje
            string destinatario = "Ventas";    // Nosotros lo recibimos

            // Act
            this._fachada.RegistrarMensaje(cliente.Id, fechaMensaje, tema, remitente, destinatario);

            // Assert
            // 1. Recuperamos el cliente y su historial
            var clienteActualizado = this._fachada.BuscarCliente(cliente.Id);
            var historial = clienteActualizado.Interacciones;

            // 2. Verificamos que exista la interacción
            Assert.IsTrue(historial.Count > 0, "El historial debería tener al menos una interacción.");

            // 3. Obtenemos la última interacción y verificamos el tipo
            var ultimaInteraccion = historial[historial.Count - 1];
            Assert.IsInstanceOf<Mensaje>(ultimaInteraccion, "La interacción debería ser de tipo Mensaje.");

            // 4. Casteamos y verificamos los datos campo por campo
            Mensaje mensajeGuardado = (Mensaje)ultimaInteraccion;

            Assert.AreEqual(fechaMensaje, mensajeGuardado.Fecha, "La fecha del mensaje no coincide.");
            Assert.AreEqual(tema, mensajeGuardado.Tema, "El tema del mensaje no coincide.");
            Assert.AreEqual(remitente, mensajeGuardado.Remitente, "El remitente no se guardó correctamente.");
            Assert.AreEqual(destinatario, mensajeGuardado.Destinatario, "El destinatario no se guardó correctamente.");
        }
        
        /// <summary>
        /// Verifica que se pueda registrar un Correo Electrónico asociado al cliente.
        /// Valida la persistencia de fecha, tema, remitente, destinatario y ASUNTO.
        /// </summary>
        [Test]
        public void TestRegistrarCorreo_GuardadoCorrecto()
        {
            // Arrange
            // 1. Creamos un cliente para la prueba
            this._fachada.CrearCliente("Daniela", "Email", "098777666", "daniela@email.com", "Femenino", DateTime.Now);
            
            // Recuperamos el cliente (índice 0)
            var listaClientes = this._fachada.VerTodosLosClientes();
            var cliente = listaClientes[0];

            // 2. Preparamos los datos del correo
            DateTime fechaCorreo = new DateTime(2024, 9, 20, 15, 45, 0);
            string tema = "Envío de presupuesto";
            string remitente = "ventas@miempresa.com";
            string destinatario = "daniela@email.com";
            string asunto = "Presupuesto Solicitado #4500";

            // Act
            this._fachada.RegistrarCorreo(cliente.Id, fechaCorreo, tema, remitente, destinatario, asunto);

            // Assert
            // 1. Recuperamos el historial del cliente
            var clienteActualizado = this._fachada.BuscarCliente(cliente.Id);
            var historial = clienteActualizado.Interacciones;

            // 2. Verificamos que se haya agregado
            Assert.IsTrue(historial.Count > 0, "El historial debería tener al menos una interacción.");

            // 3. Obtenemos la última interacción y verificamos el tipo
            var ultimaInteraccion = historial[historial.Count - 1];
            Assert.IsInstanceOf<Correo>(ultimaInteraccion, "La interacción debería ser de tipo Correo.");

            // 4. Casteamos y verificamos los datos
            Correo correoGuardado = (Correo)ultimaInteraccion;

            Assert.AreEqual(fechaCorreo, correoGuardado.Fecha, "La fecha del correo no coincide.");
            Assert.AreEqual(tema, correoGuardado.Tema, "El tema general no coincide.");
            Assert.AreEqual(remitente, correoGuardado.Remitente, "El remitente no se guardó correctamente.");
            Assert.AreEqual(destinatario, correoGuardado.Destinatario, "El destinatario no se guardó correctamente.");
            Assert.AreEqual(asunto, correoGuardado.Asunto, "El asunto del correo no se guardó correctamente.");
        }
        
        /// <summary>
        /// Verifica que se pueda agregar una nota (comentario) a una interacción existente.
        /// Esto cubre la historia de "tener información adicional de mis interacciones"
        /// validando que el texto se asocie correctamente a la interacción específica.
        /// </summary>
        [Test]
        public void TestAgregarNotaAInteraccion_GuardaNotaCorrectamente()
        {
            // Arrange
            // 1. Creamos el cliente
            this._fachada.CrearCliente("Carlos", "Notas", "099123123", "carlos@nota.com", "Masculino", DateTime.Now);
            
            // Obtenemos el cliente (índice 0)
            var listaClientes = this._fachada.VerTodosLosClientes();
            var cliente = listaClientes[0];

            // 2. Registramos una interacción (ej. una Llamada) para ponerle la nota
            // Esta será la interacción en el índice 0 del historial del cliente.
            this._fachada.RegistrarLlamada(cliente.Id, DateTime.Now, "Llamada inicial", "entrante");

            // 3. Verificamos pre-condición: que la interacción no tenga nota aún
            var historialInicial = this._fachada.VerInteraccionesCliente(cliente.Id);
            var interaccionSinNota = historialInicial[0];
            Assert.IsNull(interaccionSinNota.NotaAdicional, "La interacción no debería tener nota al inicio.");

            // Act
            string contenidoNota = "El cliente mencionó que prefiere ser contactado por la tarde.";
            
            // Agregamos la nota a la interacción con índice 0
            this._fachada.AgregarNotaAInteraccion(cliente.Id, 0, contenidoNota);

            // Assert
            // Recuperamos la interacción nuevamente para verificar la persistencia
            var historialActualizado = this._fachada.VerInteraccionesCliente(cliente.Id);
            var interaccionConNota = historialActualizado[0];

            // Verificamos que el objeto Nota se haya creado
            Assert.IsNotNull(interaccionConNota.NotaAdicional, "La nota debería haber sido creada y asignada.");
            
            // Verificamos el contenido del texto
            // Asumimos que la propiedad en la clase Nota es 'Texto' según convenciones comunes.
            Assert.AreEqual(contenidoNota, interaccionConNota.NotaAdicional.Texto, "El contenido de la nota no coincide.");
        }
        
        /// <summary>
        /// Verifica que se puedan registrar datos adicionales (Género y Fecha de Nacimiento)
        /// en un cliente que fue creado inicialmente solo con información de contacto.
        /// Esto cubre la historia de usuario sobre "realizar campañas y saludos de cumpleaños".
        /// </summary>
        [Test]
        public void TestRegistrarDatosAdicionales_ParaCampañasYCumpleanos()
        {
            // Arrange
            // 1. Creamos un cliente solo con datos básicos de contacto
            this._fachada.CrearCliente("Usuario", "Campañas", "091888999", "campania@test.com", "NoEspecificado", DateTime.MinValue);
            
            // Recuperamos el cliente (índice 0)
            var listaClientes = this._fachada.VerTodosLosClientes();
            var cliente = listaClientes[0];

            // 2. Definimos los datos que queremos agregar para la campaña/cumpleaños
            string generoObjetivo = "Femenino";
            DateTime fechaCumpleanos = new DateTime(1990, 12, 25); // Navidad

            // Act
            // Ejecutamos el método específico para completar estos datos
            this._fachada.RegistrarDatosAdicionalesCliente(cliente.Id, generoObjetivo, fechaCumpleanos);

            // Assert
            // 1. Recuperamos el cliente actualizado
            var clienteActualizado = this._fachada.BuscarCliente(cliente.Id);

            // 2. Validamos que el género se haya actualizado correctamente
            // CORRECCIÓN: Usamos 'GeneroCliente' que es el nombre real de tu enum
            Assert.AreEqual(GeneroCliente.Femenino, clienteActualizado.Genero, "El género debería actualizarse para permitir la segmentación en campañas.");

            // 3. Validamos que la fecha de nacimiento se haya registrado
            Assert.AreEqual(fechaCumpleanos, clienteActualizado.FechaNacimiento, "La fecha de nacimiento debería registrarse correctamente para los saludos.");
        }
    }
}