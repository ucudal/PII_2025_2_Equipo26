using Library;
using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Implementa el patrón "Fachada" (Facade) para ocultar la complejidad del sistema
    /// y el patrón "Controlador" (Controller) (GRASP) para coordinar las operaciones.
    /// </summary>
    public class Fachada
    {
        // --- Repositorios Internos (Inyectados) ---
        
        private readonly IRepoClientes _repoClientes;
        private readonly IRepoEtiquetas _repoEtiquetas;
        private readonly IRepoUsuarios _repoUsuarios;
        private readonly IRepoVentas _repoVentas;

        private int _proximoIdVenta = 1;

        // --- Constructor (DIP) ---
        
        public Fachada(IRepoClientes repoClientes, IRepoEtiquetas repoEtiquetas,
                       IRepoUsuarios repoUsuarios, IRepoVentas repoVentas)
        {
            this._repoClientes = repoClientes;
            this._repoEtiquetas = repoEtiquetas;
            this._repoUsuarios = repoUsuarios;
            this._repoVentas = repoVentas;
        }

        
        // --- Métodos de Clientes ---

        public void CrearCliente(string nombre, string apellido, string telefono, string correo, 
                               string genero, DateTime fechaNacimiento)
        {
            this._repoClientes.Agregar(nombre, apellido, telefono, correo, 
                                     genero, fechaNacimiento);
        }
        public void CrearCliente(string nombre, string apellido, string telefono, string correo)
        {
            string generoPorDefecto = "NoEspecificado";
            DateTime fechaPorDefecto = DateTime.MinValue;
            
            this.CrearCliente(nombre, apellido, telefono, correo, generoPorDefecto, fechaPorDefecto);
        }
        
        public IReadOnlyList<Cliente> VerTodosLosClientes()
        {
            return this._repoClientes.ObtenerTodas();
        }
        
        public void ModificarCliente(int id, string nombre, string apellido, string telefono, 
                                   string correo, string genero, DateTime fechaNacimiento)
        {
            this._repoClientes.Modificar(id, nombre, apellido, telefono, correo, genero, fechaNacimiento);
        }

        /// <summary>
        /// Registra el género y la fecha de nacimiento de un cliente existente.
        /// Implementa Composición/Delegación: La Fachada delega la lógica de 
        /// persistencia al Repositorio de Clientes.
        /// </summary>
        /// <param name="idCliente">El ID del cliente a modificar.</param>
        /// <param name="generoTexto">El género del cliente (como string).</param>
        /// <param name="fechaNacimiento">La fecha de nacimiento (como DateTime).</param>
        public void RegistrarDatosAdicionalesCliente(int idCliente, string generoTexto, DateTime fechaNacimiento)
        {
            // La Fachada coordina la operación, delegando la acción de persistencia.
            // Si el método lanza una excepción (por ID inválido o Género inválido),
            // la Fachada simplemente la propaga al Comando de Discord (la capa superior).
            
            this._repoClientes.ActualizarDatosAdicionales(
                idCliente, 
                generoTexto, 
                fechaNacimiento
            );
        }
        
        public List<Cliente> BuscarClientes(string termino)
        {
            return this._repoClientes.BuscarPorTermino(termino);
        }

        public void EliminarCliente(int id)
        {
            this._repoClientes.Eliminar(id);
        }

        // --- Métodos de Coordinación ---
        
        
        /// <summary>
        /// Asigna un cliente existente a un nuevo vendedor (Usuario).
        /// Coordina la búsqueda de ambos objetos y delega la asignación al Cliente.
        /// </summary>
        /// <param name="idCliente">El ID del cliente a reasignar.</param>
        /// <param name="idNuevoVendedor">El ID del usuario que será el nuevo vendedor.</param>
        /// <exception cref="KeyNotFoundException">Se lanza si el cliente o el nuevo vendedor no existen.</exception>
        /// <exception cref="InvalidOperationException">Se lanza si el nuevo vendedor no tiene el rol Vendedor o está Suspendido (validación realizada por el objeto Cliente).</exception>
        public void AsignarClienteVendedor(int idCliente, int idNuevoVendedor)
        {
            Cliente cliente = this._repoClientes.Buscar(idCliente);
            Usuario nuevoVendedor = this._repoUsuarios.Buscar(idNuevoVendedor);

            // Precondición 1: El Cliente debe existir.
            if (cliente == null)
            {
                throw new KeyNotFoundException(String.Format("No se encontró el cliente con ID {0}.", idCliente));
            }

            // Precondición 2: El Nuevo Vendedor debe existir.
            if (nuevoVendedor == null)
            {
                throw new KeyNotFoundException(String.Format("No se encontró el usuario vendedor con ID {0}.", idNuevoVendedor));
            }

            // DELEGACIÓN: El Cliente (Expert) valida las reglas de negocio (rol y estado) y realiza la asignación.
            cliente.AsignarVendedor(nuevoVendedor);
        }

        // --- Métodos de Interacciones (Polimorfismo) ---

        public void RegistrarLlamada(int idCliente, DateTime fecha, string tema, string tipoLlamada)
        {
            // VALIDACIÓN DE PRECONDICIÓN (NEGOCIO): Cliente debe existir.
            var cliente = this._repoClientes.Buscar(idCliente);
    
            if (cliente == null)
            {
                throw new ArgumentException($"No se encontró un cliente registrado con el ID: {idCliente}. No se puede registrar la llamada.", nameof(idCliente));
            }
    
            // VALIDACIÓN DE PRECONDICIÓN (NEGOCIO): Tipo de llamada debe ser válido.
            string tipoNormalizado = tipoLlamada.Trim().ToLowerInvariant();
    
            // CORRECCIÓN: Ahora incluimos 'recibida'
            if (tipoNormalizado != "entrante" && tipoNormalizado != "saliente" && tipoNormalizado != "recibida")
            {
                // Actualizamos el mensaje de error para reflejar los tipos permitidos.
                throw new ArgumentException($"El tipo de llamada '{tipoLlamada}' es inválido. Los tipos de llamada permitidos son 'entrante', 'saliente' o 'recibida'.", nameof(tipoLlamada));
            }

            // DELEGACIÓN: Si todas las Precondiciones pasan.
            cliente.AgregarInteraccion(new Llamada(fecha, tema, tipoLlamada));
        }

        public void RegistrarReunion(int idCliente, DateTime fecha, string tema, string lugar)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                cliente.AgregarInteraccion(new Reunion(fecha, tema, lugar));
            }
        }

        public void RegistrarMensaje(int idCliente, DateTime fecha, string tema, string remitente, string destinatario)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                cliente.AgregarInteraccion(new Mensaje(fecha, tema, remitente, destinatario));
            }
        }

        public void RegistrarCorreo(int idCliente, DateTime fecha, string tema, string remitente, string destinatario, string asunto)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                cliente.AgregarInteraccion(new Correo(fecha, tema, remitente, destinatario, asunto));
            }
        }

        // --- MÉTODOS RENOMBRADOS ---
        public List<Interaccion> VerInteraccionesCliente(int idCliente)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                // Convertimos IReadOnlyList a List para mantener la firma, 
                // aunque sería mejor devolver IReadOnlyList.
                return new List<Interaccion>(cliente.Interacciones);
            }
            return new List<Interaccion>(); 
        }

        public List<Interaccion> VerInteraccionesCliente(int idCliente, TipoInteraccion tipo)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente == null)
            {
                return new List<Interaccion>();
            }

            var interaccionesFiltradas = new List<Interaccion>();
            foreach (var inter in cliente.Interacciones)
            {
                if (inter.Tipo == tipo)
                {
                    interaccionesFiltradas.Add(inter);
                }
            }
            
            return interaccionesFiltradas;
        }

        public List<Interaccion> VerInteraccionesCliente(int idCliente, TipoInteraccion tipo, DateTime fechaDesde)
        {
            var interaccionesFiltradasPorTipo = this.VerInteraccionesCliente(idCliente, tipo);
            
            var resultadoFinal = new List<Interaccion>();
            foreach (var inter in interaccionesFiltradasPorTipo)
            {
                if (inter.Fecha >= fechaDesde)
                {
                    resultadoFinal.Add(inter);
                }
            }

            return resultadoFinal;
        }

        public void AgregarNotaAInteraccion(int idCliente, int indiceInteraccion, string textoNota)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                if (indiceInteraccion >= 0 && indiceInteraccion < cliente.Interacciones.Count)
                {
                    cliente.Interacciones[indiceInteraccion].NotaAdicional = new Nota(textoNota);
                }
            }
        }

        // --- Métodos de Etiquetas ---

        public void CrearEtiqueta(string nombre)
        {
            this._repoEtiquetas.Crear(nombre);
        }

        public IReadOnlyList<Etiqueta> VerTodasLasEtiquetas()
        {
            return this._repoEtiquetas.ObtenerTodas();
        }

        public void EliminarEtiqueta(int idEtiqueta)
        {
            this._repoEtiquetas.Eliminar(idEtiqueta);
        }

        // --- MÉTODOS RENOMBRADOS ---
        public void AgregarEtiquetaCliente(int idCliente, int idEtiqueta)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            var etiqueta = this._repoEtiquetas.Buscar(idEtiqueta);

            if (cliente != null && etiqueta != null)
            {
                cliente.AgregarEtiqueta(etiqueta);
            }
        }

        public void QuitarEtiquetaCliente(int idCliente, int idEtiqueta)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            var etiqueta = this._repoEtiquetas.Buscar(idEtiqueta);

            if (cliente != null && etiqueta != null)
            {
                cliente.QuitarEtiqueta(etiqueta);
            }
        }

        // --- Métodos de Usuarios ---
        
        /// <summary>
        /// Crea un nuevo usuario (Vendedor o Administrador) en el sistema.
        /// </summary>
        /// <param name="nombreUsuario">Nombre de login.</param>
        /// <param name="rol">Rol del usuario.</param>
        // --- PARÁMETRO 'contrasena' ELIMINADO ---
        public void CrearUsuario(string nombreUsuario, Rol rol)
        {
            // --- LLAMADA AL REPO ACTUALIZADA ---
            this._repoUsuarios.Agregar(nombreUsuario, rol);
        }

        public void SuspenderUsuario(int idUsuario)
        {
            this._repoUsuarios.Suspender(idUsuario);
        }
        
        public void ActivarUsuario(int idUsuario)
        {
            this._repoUsuarios.Activar(idUsuario);
        }
        
        public void EliminarUsuario(int idUsuario)
        {
            this._repoUsuarios.Eliminar(idUsuario);
        }
        
        public Usuario BuscarUsuario(int idUsuario)
        {
            return this._repoUsuarios.Buscar(idUsuario);
        }
        public void AgregarRolUsuario(int idUsuario, Rol nuevoRol)
        {
            
            this._repoUsuarios.AgregarRol(idUsuario, nuevoRol);
        }
        
        public IReadOnlyList<Usuario> VerTodosLosUsuarios()
        {
            return this._repoUsuarios.ObtenerTodas();
        }

        // --- Métodos de Ventas y Reportes ---
        public void RegistrarVenta(int clienteId, string producto, float monto, DateTime fecha)
        {
            Cliente cliente = this._repoClientes.Buscar(clienteId);
    
            if (cliente != null)
            {
                Venta nuevaVenta = new Venta(this._proximoIdVenta++, producto, monto, fecha);
                cliente.AgregarInteraccion(nuevaVenta); 
                cliente.AgregarVenta(nuevaVenta);
                this._repoVentas.Agregar(producto, monto, fecha);
            }
        }


        

        public float CalcularTotalVentas(DateTime fechaInicio, DateTime fechaFin)
        {
            var todasLasVentas = this._repoVentas.ObtenerTodas();
            float total = 0;

            foreach (var venta in todasLasVentas)
            {
                if (venta.Fecha >= fechaInicio && venta.Fecha <= fechaFin)
                {
                    total += venta.Importe;
                }
            }
            return total;
        }

        public List<Cliente> BuscarVentasProducto(string producto)
        {
            var todoslosclientes = _repoClientes.ObtenerTodas();
            List<Cliente> clients = new List<Cliente>();
            foreach (var cliente in todoslosclientes)
            {
                foreach (var venta in cliente.Ventas)
                {
                    if (venta.Producto.ToLower() == producto.ToLower())
                    {
                        clients.Add(cliente);
                    }
                }
            }
            return clients;
         }

        public void RegistrarCotizacion(int clienteId, string tema, double monto, DateTime fecha)
        {
            Cliente cliente = this._repoClientes.Buscar(clienteId);
            if (cliente != null)
            {
                Cotizacion nuevaCoti = new Cotizacion(fecha, tema, monto, tema);
                cliente.AgregarInteraccion(nuevaCoti);
            }
        }

        /// <summary>
        /// Obtiene una lista de clientes que no han tenido ninguna interacción
        /// en el número de días especificado.
        /// La Fachada delega la lógica de encontrar la última fecha de interacción
        /// al objeto Cliente (Patrón Expert).
        /// </summary>
        /// <param name="diasSinInteraccion">Número de días límite para considerar inactivo al cliente.</param>
        /// <returns>Lista de clientes inactivos.</returns>
        public List<Cliente> ObtenerClientesInactivos(int diasSinInteraccion)
        {
            List<Cliente> clientesInactivos = new List<Cliente>();
            
            // Calcula la fecha límite (e.g., hoy menos 30 días).
            DateTime fechaLimite = DateTime.Now.AddDays(-diasSinInteraccion);

            foreach (var cliente in this._repoClientes.ObtenerTodas())
            {
                // 1. DELEGACIÓN AL EXPERT: El Cliente calcula la fecha de su última interacción.
                DateTime fechaUltimaInteraccion = cliente.ObtenerFechaUltimaInteraccion(); 

                // 2. REGLA DE NEGOCIO: El cliente se considera inactivo si su última interacción
                //    es anterior (<) a la fecha límite.
                //    Esto incluye clientes que tienen fecha de interacción DateTime.MinValue ("Nunca").
                if (fechaUltimaInteraccion < fechaLimite)
                {
                    clientesInactivos.Add(cliente);
                }
            }
            return clientesInactivos;
        }

        public Cliente BuscarCliente(int clienteId)
        {
            return this._repoClientes.Buscar(clienteId);
        }

        public List<Cliente> ObtenerClientesSinRespuesta()
        {
            List<Cliente> clientesSinRespuesta = new List<Cliente>();

            foreach (var cliente in this._repoClientes.ObtenerTodas())
            {
                if (cliente.Interacciones.Count == 0) { continue; }

                Interaccion ultimaInteraccion = null;
                DateTime fechaMasReciente = DateTime.MinValue;

                foreach (var interaccion in cliente.Interacciones)
                {
                    if (interaccion.Fecha > fechaMasReciente)
                    {
                        fechaMasReciente = interaccion.Fecha;
                        ultimaInteraccion = interaccion;
                    }
                }
                
                // Usamos polimorfismo en lugar de 'is' y 'as' si es posible, 
                // pero como no podemos modificar Interaccion fácilmente sin verla,
                // mantendremos esto por ahora pero lo marcaremos para refactorizar si Interaccion lo permite.
                // EL USUARIO PIDIO EVITAR PREGUNTAR POR EL TIPO.
                // Vamos a asumir que podemos agregar una propiedad virtual a Interaccion.
                
                if (ultimaInteraccion != null && ultimaInteraccion.EsSinRespuesta())
                {
                    clientesSinRespuesta.Add(cliente);
                }
            }
            return clientesSinRespuesta;
        }
        
        // --- Dashboard ---
        
        /// <summary>
        /// Obtiene un resumen de datos para el Dashboard.
        /// </summary>
        public ResumenDashboard ObtenerResumenDashboard()
        {
            var todosLosClientes = this._repoClientes.ObtenerTodas();
            int totalClientes = todosLosClientes.Count;

            List<Interaccion> todasLasInteracciones = new List<Interaccion>();
            foreach (Cliente cliente in todosLosClientes)
            {
                todasLasInteracciones.AddRange(cliente.Interacciones);
            }

            DateTime ahora = DateTime.Now;

            // --- REFACTORIZADO SIN LINQ ---

            // 1. Interacciones Recientes (PASADAS)
            List<Interaccion> interaccionesRecientes = new List<Interaccion>();
            foreach (var i in todasLasInteracciones)
            {
                if (i.Fecha <= ahora)
                {
                    interaccionesRecientes.Add(i);
                }
            }
            // Ordenar descendente manualmente (burbuja simple o Sort)
            interaccionesRecientes.Sort((a, b) => b.Fecha.CompareTo(a.Fecha));
            
            // Tomar 5
            List<Interaccion> top5Recientes = new List<Interaccion>();
            for (int i = 0; i < 5 && i < interaccionesRecientes.Count; i++)
            {
                top5Recientes.Add(interaccionesRecientes[i]);
            }

            // 2. Reuniones Próximas (FUTURAS)
            List<Reunion> reunionesProximas = new List<Reunion>();
            foreach (var i in todasLasInteracciones)
            {
                if (i is Reunion && i.Fecha > ahora)
                {
                    reunionesProximas.Add((Reunion)i);
                }
            }
            // Ordenar ascendente
            reunionesProximas.Sort((a, b) => a.Fecha.CompareTo(b.Fecha));

            var resumen = new ResumenDashboard
            {
                TotalClientes = totalClientes,
                InteraccionesRecientes = top5Recientes,
                ReunionesProximas = reunionesProximas
            };

            return resumen;
        }
    }
}