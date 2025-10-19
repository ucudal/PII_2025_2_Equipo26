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
}