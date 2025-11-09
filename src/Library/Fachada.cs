using Library;
using System;
using System.Collections.Generic;
using System.Linq; 

namespace Library
{
    /// <summary>
    /// Implementa el patrón "Fachada" (Facade).
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
            if (nuevoVendedor.Rol != Rol.Vendedor) { return; } 
            if (nuevoVendedor.Estado == Estado.Suspendido) { return; } 

            cliente.AsignarVendedor(nuevoVendedor);
        }

        // --- Métodos de Interacciones (Polimorfismo) ---

        public void RegistrarLlamada(int idCliente, DateTime fecha, string tema, string tipoLlamada)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                cliente.Interacciones.Add(new Llamada(fecha, tema, tipoLlamada));
            }
        }

        public void RegistrarReunion(int idCliente, DateTime fecha, string tema, string lugar)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                cliente.Interacciones.Add(new Reunion(fecha, tema, lugar));
            }
        }

        public void RegistrarMensaje(int idCliente, DateTime fecha, string tema, string remitente, string destinatario)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                cliente.Interacciones.Add(new Mensaje(fecha, tema, remitente, destinatario));
            }
        }

        public void RegistrarCorreo(int idCliente, DateTime fecha, string tema, string remitente, string destinatario, string asunto)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                cliente.Interacciones.Add(new Correo(fecha, tema, remitente, destinatario, asunto));
            }
        }

        // --- MÉTODOS RENOMBRADOS ---
        public List<Interaccion> VerInteraccionesCliente(int idCliente)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            if (cliente != null)
            {
                return cliente.Interacciones;
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

            var interaccionesFiltradas = cliente.Interacciones
                                                .Where(inter => inter.Tipo == tipo)
                                                .ToList();
            
            return interaccionesFiltradas;
        }

        public List<Interaccion> VerInteraccionesCliente(int idCliente, TipoInteraccion tipo, DateTime fechaDesde)
        {
            var interaccionesFiltradasPorTipo = this.VerInteraccionesCliente(idCliente, tipo);
            
            var resultadoFinal = interaccionesFiltradasPorTipo
                                    .Where(inter => inter.Fecha >= fechaDesde)
                                    .ToList();

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
                if (!cliente.Etiquetas.Contains(etiqueta))
                {
                    cliente.Etiquetas.Add(etiqueta);
                }
            }
        }

        public void QuitarEtiquetaCliente(int idCliente, int idEtiqueta)
        {
            var cliente = this._repoClientes.Buscar(idCliente);
            var etiqueta = this._repoEtiquetas.Buscar(idEtiqueta);

            if (cliente != null && etiqueta != null)
            {
                cliente.Etiquetas.Remove(etiqueta);
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
                clienteEncontrado.Ventas.Add(nuevaVenta); 
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
                clienteEncontrado.Interacciones.Add(nuevaCotizacion); 
            }
        }

        public List<Cliente> ObtenerClientesInactivos(int diasSinInteraccion)
        {
            List<Cliente> clientesInactivos = new List<Cliente>();
            DateTime fechaLimite = DateTime.Now.AddDays(-diasSinInteraccion);

            foreach (var cliente in this._repoClientes.ObtenerTodas())
            {
                if (cliente.Interacciones.Count == 0)
                {
                    clientesInactivos.Add(cliente);
                    continue; 
                }

                DateTime fechaMasReciente = DateTime.MinValue;
                foreach (var interaccion in cliente.Interacciones)
                {
                    if (interaccion.Fecha > fechaMasReciente)
                    {
                        fechaMasReciente = interaccion.Fecha;
                    }
                }

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
                
                if (ultimaInteraccion != null && ultimaInteraccion is Llamada)
                {
                    Llamada ultimaLlamada = ultimaInteraccion as Llamada;
                    if (ultimaLlamada != null && ultimaLlamada.TipoLlamada == "Recibida")
                    {
                        clientesSinRespuesta.Add(cliente);
                    }
                }
            }
            return clientesSinRespuesta;
        }
        
        // --- Dashboard ---
        
        public ResumenDashboard ObtenerResumenDashboard()
        {
            var todosLosClientes = this._repoClientes.ObtenerTodas();
            int totalClientes = todosLosClientes.Count;

            List<Interaccion> todasLasInteracciones = new List<Interaccion>();
            foreach (Cliente cliente in todosLosClientes)
            {
                todasLasInteracciones.AddRange(cliente.Interacciones);
            }

            todasLasInteracciones.Sort((i1, i2) => i2.Fecha.CompareTo(i1.Fecha));
            
            List<Interaccion> interaccionesRecientes = todasLasInteracciones.Take(5).ToList();

            List<Reunion> reunionesProximas = new List<Reunion>();
            DateTime ahora = DateTime.Now;

            foreach (Interaccion interaccion in todasLasInteracciones)
            {
                if (interaccion is Reunion) 
                {
                    if (interaccion.Fecha > ahora) 
                    {
                        reunionesProximas.Add((Reunion)interaccion);
                    }
                }
            }

            reunionesProximas.Sort((r1, r2) => r1.Fecha.CompareTo(r2.Fecha));

            var resumen = new ResumenDashboard
            {
                TotalClientes = totalClientes,
                InteraccionesRecientes = interaccionesRecientes,
                ReunionesProximas = reunionesProximas
            };

            return resumen;
        }
    }
}