using Library;
using System;
using System.Collections.Generic;

public class RepoClientes
{
    private List<Cliente> _clientes = new List<Cliente>();
    private int _nextId = 1;

    public void Agregar(Cliente cliente)
    {
 
        var nuevoCliente = new Cliente(
            _nextId++, 
            cliente.Nombre, 
            cliente.Apellido, 
            cliente.Telefono, 
            cliente.Correo,
            cliente.Genero, 
            cliente.FechaNacimiento 
        );
        _clientes.Add(nuevoCliente);
    }


    public Cliente Buscar(int id)
    {
        // Busco el cliente en la lista
        foreach (var cliente in _clientes)
        {
            if (cliente.Id == id)
            {
                return cliente;
            }
        }
        // Si no lo encuentro, devuelvo null
        return null;
    }


    public void Modificar(int id, string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
    {
        var cliente = Buscar(id);
        if (cliente != null)
        {
            cliente.Nombre = nombre;
            cliente.Apellido = apellido;
            cliente.Telefono = telefono;
            cliente.Correo = correo;
            cliente.Genero = genero; 
            cliente.FechaNacimiento = fechaNacimiento; 
        }
    }
    
   public List<Cliente> BuscarPorTermino(string termino)
    {
        var resultados = new List<Cliente>();
        var busqueda = termino.ToLower();

        foreach (var cliente in _clientes)
        {
            if (cliente.Nombre.ToLower().Contains(busqueda) ||
                cliente.Apellido.ToLower().Contains(busqueda) ||
                cliente.Telefono.Contains(busqueda) ||
                cliente.Correo.ToLower().Contains(busqueda) ||
                cliente.Genero.ToLower().Contains(busqueda)) 
            {
                resultados.Add(cliente);
            }
        }
        return resultados;
    }
    public List<Cliente> ObtenerTodos()
    {
        return _clientes;
    }

    public void Eliminar(int id)
    {
        var cliente = Buscar(id);
        if (cliente != null)
        {
            _clientes.Remove(cliente);
        }
    }
}