using Library;

using System;
using System.Collections.Generic;

public class Fachada
{
    private RepoClientes _repoClientes = new RepoClientes();
    private RepoEtiquetas _repoEtiquetas = new RepoEtiquetas();
    private RepoUsuarios _repoUsuarios = new RepoUsuarios();
    private RepoVentas _repoVentas = new RepoVentas();
    private int _proximoIdVenta = 1;
    
    public void CrearCliente(string nombre, string apellido, string telefono, string correo, 
                           string genero, DateTime fechaNacimiento)
    {
        var clienteTemporal = new Cliente(0, nombre, apellido, telefono, correo, 
                                        genero, fechaNacimiento);
        _repoClientes.Agregar(clienteTemporal);
    }

    public List<Cliente> VerTodosLosClientes()
    {
        return _repoClientes.ObtenerTodos();
    }
    public void ModificarCliente(int id, string nombre, string apellido, string telefono, 
                               string correo, string genero, DateTime fechaNacimiento)
    {
        _repoClientes.Modificar(id, nombre, apellido, telefono, correo, genero, fechaNacimiento);
    }

    public List<Cliente> BuscarClientes(string termino)
    {
        return _repoClientes.BuscarPorTermino(termino);
    }

    public void EliminarCliente(int id)
    {
        _repoClientes.Eliminar(id);
    }
    
    public void AsignarClienteVendedor(int idCliente, int idNuevoVendedor)
    {
        Cliente cliente = _repoClientes.Buscar(idCliente);
        Usuario nuevoVendedor = _repoUsuarios.Buscar(idNuevoVendedor);

        if (cliente == null || nuevoVendedor == null)
        {
            return; 
        }
        if (nuevoVendedor.Rol != RolUsuario.Vendedor)
        {
            return;
        }
        if (nuevoVendedor.Estado == EstadoUsuario.Suspendido)
        {
            return;
        }
        
        cliente.AsignarVendedor(nuevoVendedor);
    }


    public void RegistrarLlamada(int idCliente, DateTime fecha, string tema, string tipoLlamada)
    {
        var cliente = _repoClientes.Buscar(idCliente);

        if (cliente != null)
        {
            var nuevaLlamada = new Llamada(fecha, tema, tipoLlamada);
            cliente.Interacciones.Add(nuevaLlamada);
        }
    }

    public void RegistrarReunion(int idCliente, DateTime fecha, string tema, string lugar)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevaReunion = new Reunion(fecha, tema, lugar);
            cliente.Interacciones.Add(nuevaReunion);
        }
    }

    public void RegistrarMensaje(int idCliente, DateTime fecha, string tema, string remitente, string destinatario)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevoMensaje = new Mensaje(fecha, tema, remitente, destinatario);
            cliente.Interacciones.Add(nuevoMensaje);
        }
    }

    public void RegistrarCorreo(int idCliente, DateTime fecha, string tema, string remitente, string destinatario, string asunto)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            var nuevoCorreo = new Correo(fecha, tema, remitente, destinatario, asunto);
            cliente.Interacciones.Add(nuevoCorreo);
        }
    }
    
    public List<Interaccion> VerInteraccionesDeCliente(int idCliente)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente != null)
        {
            return cliente.Interacciones;
        }
        return new List<Interaccion>();
    }

    public List<Interaccion> VerInteraccionesDeCliente(int idCliente, string tipoDeInteraccion)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        if (cliente == null)
        {
            return new List<Interaccion>();
        }

        var interaccionesFiltradas = new List<Interaccion>();
        string tipoBuscado = tipoDeInteraccion.ToLower();

        foreach (var interaccion in cliente.Interacciones)
        {
            bool coincide = false;
            if (tipoBuscado == "llamada" && interaccion is Llamada)
            {
                coincide = true;
            }
            else if (tipoBuscado == "reunion" && interaccion is Reunion)
            {
                coincide = true;
            }
            else if (tipoBuscado == "mensaje" && interaccion is Mensaje)
            {
                coincide = true;
            }
            else if (tipoBuscado == "correo" && interaccion is Correo)
            {
                coincide = true;
            }

            if (coincide)
            {
                interaccionesFiltradas.Add(interaccion);
            }
        }

        return interaccionesFiltradas;
    }

    public List<Interaccion> VerInteraccionesDeCliente(int idCliente, string tipoDeInteraccion, DateTime fechaDesde)
    {
        var interaccionesFiltradasPorTipo = VerInteraccionesDeCliente(idCliente, tipoDeInteraccion);
        var resultadoFinal = new List<Interaccion>();

        foreach (var interaccion in interaccionesFiltradasPorTipo)
        {
            if (interaccion.Fecha >= fechaDesde)
            {
                resultadoFinal.Add(interaccion);
            }
        }

        return resultadoFinal;
    }

    public void AgregarNotaAInteraccion(int idCliente, int indiceInteraccion, string textoNota)
    {
        var cliente = _repoClientes.Buscar(idCliente);

        if (cliente != null)
        {
            if (indiceInteraccion >= 0 && indiceInteraccion < cliente.Interacciones.Count)
            {
                var interaccionSeleccionada = cliente.Interacciones[indiceInteraccion];
                interaccionSeleccionada.NotaAdicional = new Nota(textoNota);
            }
        }
    }
    
    public void CrearEtiqueta(string nombre)
    {
        _repoEtiquetas.Crear(nombre);
    }

    public List<Etiqueta> VerTodasLasEtiquetas()
    {
        return _repoEtiquetas.ObtenerTodas();
    }

    public void EliminarEtiqueta(int idEtiqueta)
    {
        _repoEtiquetas.Eliminar(idEtiqueta);
    }

    public void AgregarEtiquetaACliente(int idCliente, int idEtiqueta)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        var etiqueta = _repoEtiquetas.Buscar(idEtiqueta);

        if (cliente != null && etiqueta != null)
        {
            if (!cliente.Etiquetas.Contains(etiqueta))
            {
                cliente.Etiquetas.Add(etiqueta);
            }
        }
    }
    
    public void QuitarEtiquetaDeCliente(int idCliente, int idEtiqueta)
    {
        var cliente = _repoClientes.Buscar(idCliente);
        var etiqueta = _repoEtiquetas.Buscar(idEtiqueta);

        if (cliente != null && etiqueta != null)
        {
            cliente.Etiquetas.Remove(etiqueta);
        }
    }
    
    public void CrearUsuario(string nombreUsuario, string contrasena, RolUsuario rol)
    {
        _repoUsuarios.Agregar(nombreUsuario, contrasena, rol);
    }

    public void SuspenderUsuario(int idUsuario)
    {
        _repoUsuarios.Suspender(idUsuario);
    }
    public void ActivarUsuario(int idUsuario)
    {
        _repoUsuarios.Activar(idUsuario);
    }
    public void EliminarUsuario(int idUsuario)
    {
        _repoUsuarios.Eliminar(idUsuario);
    }
    public Usuario BuscarUsuario(int idUsuario)
    {
        return _repoUsuarios.Buscar(idUsuario);
    }
    public List<Usuario> VerTodosLosUsuarios()
    {
        return _repoUsuarios.ObtenerTodos();
    }
    
    public void RegistrarVenta(string producto, float importe, DateTime fecha)
    {
        _repoVentas.Agregar(producto, importe, fecha);
    }
    
    public void RegistrarVenta(int clienteId, string producto, float monto)
    {
        Cliente clienteEncontrado = _repoClientes.Buscar(clienteId);

        if (clienteEncontrado != null)
        {
            Venta nuevaVenta = new Venta(_proximoIdVenta++, producto, monto, DateTime.Now);
            clienteEncontrado.Ventas.Add(nuevaVenta); 
            
        }
    }
    
    public float CalcularTotalVentas(DateTime fechaInicio, DateTime fechaFin)
    {
        var todasLasVentas = _repoVentas.ObtenerTodas();
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
        Cliente clienteEncontrado = _repoClientes.Buscar(clienteId); 

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

        foreach (var cliente in _repoClientes.ObtenerTodos())
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
        return _repoClientes.Buscar(clienteId);
    }
    
    public List<Cliente> ObtenerClientesSinRespuesta()
    {
        List<Cliente> clientesSinRespuesta = new List<Cliente>();

        foreach (var cliente in _repoClientes.ObtenerTodos())
        {
            if (cliente.Interacciones.Count == 0)
            {
                continue;
            }

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

            if (ultimaInteraccion is Llamada)
            {
                Llamada ultimaLlamada = ultimaInteraccion as Llamada;
                if (ultimaLlamada.TipoLlamada == "Recibida")
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
        var todosLosClientes = _repoClientes.ObtenerTodos();
        
        int totalClientes = todosLosClientes.Count;

        List<Interaccion> todasLasInteracciones = new List<Interaccion>();
        foreach (Cliente cliente in todosLosClientes)
        {
            foreach (Interaccion interaccion in cliente.Interacciones)
            {
                todasLasInteracciones.Add(interaccion);
            }
        }

        todasLasInteracciones.Sort((i1, i2) => i2.Fecha.CompareTo(i1.Fecha));

        List<Interaccion> interaccionesRecientes = new List<Interaccion>();
        int contadorMaximo = 5;
        
        for (int i = 0; i < todasLasInteracciones.Count; i++)
        {
            if (i >= contadorMaximo)
            {
                break; 
            }
            interaccionesRecientes.Add(todasLasInteracciones[i]);
        }

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