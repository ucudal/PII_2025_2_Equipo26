namespace Library;

using System;
using System.Collections.Generic;

public class Fachada
{
    private RepoClientes _repoClientes = new();
    private RepoEtiquetas _repoEtiquetas = new();
    private int _proximoIdVenta = 1;

    // --- Clientes ---
    public void CrearCliente(string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
    {
        var clienteTemporal = new Cliente(0, nombre, apellido, telefono, correo, genero, fechaNacimiento);
        _repoClientes.Agregar(clienteTemporal);
    }

    public List<Cliente> VerTodosLosClientes()
    {
        return _repoClientes.ObtenerTodos();
    }
    public void ModificarCliente(int id, string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
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

    // --- Interacciones ---
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


    // --- Consultas ---




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

    // NUEVA SECCIÓN: Gestión de Etiquetas
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
    

    public void RegistrarVenta(int clienteId, string producto, float monto)
    {
        Cliente clienteEncontrado = _repoClientes.Buscar(clienteId);

        if (clienteEncontrado != null)
        {
            Venta nuevaVenta = new Venta(_proximoIdVenta++, producto, monto, DateTime.Now);
            clienteEncontrado.Ventas.Add(nuevaVenta);
        
            Console.WriteLine($"Venta de '{producto}' por ${monto} registrada para el cliente: {clienteEncontrado.Nombre}");
        }
        else
        {
            Console.WriteLine($"Error: No se encontró un cliente con el ID {clienteId}. No se pudo registrar la venta.");
        }
    }
    
    public void RegistrarCotizacion(int clienteId, string tema, double monto, string detalle)
    {
        Cliente clienteEncontrado = _repoClientes.Buscar(clienteId); 

        if (clienteEncontrado != null)
        {
            Cotizacion nuevaCotizacion = new Cotizacion(tema, monto, detalle);
            clienteEncontrado.Interacciones.Add(nuevaCotizacion); 
        
            Console.WriteLine($"Cotización sobre '{tema}' registrada para el cliente: {clienteEncontrado.Nombre}");
        }
        else
        {
            Console.WriteLine($"Error: No se encontró un cliente con el ID {clienteId}. No se pudo registrar la cotización.");
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
}