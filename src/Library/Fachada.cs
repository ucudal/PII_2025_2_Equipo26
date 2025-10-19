namespace Library;

using System;
using System.Collections.Generic;

public class Fachada
{
    private RepoClientes _repoClientes = new();

    // --- Clientes ---
    public void CrearCliente(string nombre, string apellido, string telefono, string correo)
    {
        var clienteTemporal = new Cliente(0, nombre, apellido, telefono, correo);
        _repoClientes.Agregar(clienteTemporal);
    }

    public List<Cliente> VerTodosLosClientes()
    {
        return _repoClientes.ObtenerTodos();
    }
    public void ModificarCliente(int id, string nombre, string apellido, string telefono, string correo)
    {
        _repoClientes.Modificar(id, nombre, apellido, telefono, correo);
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

        // Me aseguro que el cliente exista
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
}