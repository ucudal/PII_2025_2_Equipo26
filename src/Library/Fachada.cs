using Library;
using System;
using System.Collections.Generic;
using System.Linq; 

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
        
        public IReadOnlyList<Cliente> VerTodosLosClientes()
        {
            return this._repoClientes.ObtenerTodas();
        }
        
        public void ModificarCliente(int id, string nombre, string apellido, string telefono, 
                                   string correo, string genero, DateTime fechaNacimiento)
        {
            this._repoClientes.Modificar(id, nombre, apellido, telefono, correo, genero, fechaNacimiento);
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

        public void AsignarClienteVendedor(int idCliente, int idNuevoVendedor)
        {
            Cliente cliente = this._repoClientes.Buscar(idCliente);
            Usuario nuevoVendedor = this._repoUsuarios.Buscar(idNuevoVendedor);
            
            if (cliente == null || nuevoVendedor == null) { return; }
    
            cliente.AsignarVendedor(nuevoVendedor);
        }

        // --- Métodos de Interacciones (Polimorfismo) ---

        public void RegistrarLlamada(int idCliente, DateTime fecha, string tema, string tipoLlamada)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                cliente.AgregarInteraccion(new Llamada(fecha, tema, tipoLlamada));
            }
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
        
        public void RegistrarVenta(string producto, float importe, DateTime fecha)
        {
            this._repoVentas.Agregar(producto, importe, fecha);
        }

        public void RegistrarVenta(int clienteId, string producto, float monto)
        {
            Cliente clienteEncontrado = this._repoClientes.Buscar(clienteId);
            if (clienteEncontrado != null)
            {
                Venta nuevaVenta = new Venta(this._proximoIdVenta++, producto, monto, DateTime.Now);
                clienteEncontrado.AgregarVenta(nuevaVenta); 
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

        public void RegistrarCotizacion(int clienteId, string tema, double monto, string detalle)
        {
            Cliente clienteEncontrado = this._repoClientes.Buscar(clienteId); 
            if (clienteEncontrado != null)
            {
                Cotizacion nuevaCotizacion = new Cotizacion(tema, monto, detalle);
                clienteEncontrado.AgregarInteraccion(nuevaCotizacion); 
            }
        }

        public List<Cliente> ObtenerClientesInactivos(int diasSinInteraccion)
        {
            List<Cliente> clientesInactivos = new List<Cliente>();
            // Calcula la fecha límite (ej. si hoy es 11/Nov y dias=30, fechaLimite es 12/Oct)
            DateTime fechaLimite = DateTime.Now.AddDays(-diasSinInteraccion);

            foreach (var cliente in this._repoClientes.ObtenerTodas())
            {
                // 1. Si el cliente no tiene interacciones, es inactivo.
                if (cliente.Interacciones.Count == 0)
                {
                    clientesInactivos.Add(cliente);
                    continue; // Pasa al siguiente cliente
                }

                // 2. Encontrar la interacción más reciente del cliente
                DateTime fechaMasReciente = DateTime.MinValue;
                foreach (var interaccion in cliente.Interacciones)
                {
                    if (interaccion.Fecha > fechaMasReciente)
                    {
                        fechaMasReciente = interaccion.Fecha;
                    }
                }

                // --- ESTA ES LA LÓGICA CLAVE ---
                // 3. El cliente es inactivo si su última interacción (fechaMasReciente)
                //    es ANTERIOR (<) a la fechaLímite.
                if (fechaMasReciente < fechaLimite)
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