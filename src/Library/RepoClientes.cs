namespace Library;

using System.Collections.Generic;

public class RepoClientes
{
    private List<Cliente> _clientes = new();
    private int _nextId = 1;

    public void Agregar(Cliente cliente)
    {
        // El repo le asigna el ID
        var nuevoCliente = new Cliente(_nextId++, cliente.Nombre, cliente.Apellido, cliente.Telefono, cliente.Correo);
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