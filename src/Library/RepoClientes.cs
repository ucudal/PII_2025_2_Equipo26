using Library;
using System;
using System.Collections.Generic;

/// <summary>
/// Implementa el patrón "Repositorio" (Repository).
/// Su única responsabilidad (SRP) es administrar la colección en memoria
/// de objetos <see cref="Cliente"/>.
/// Abstrae la lógica de almacenamiento de datos (en este caso, una Lista).
/// </summary>
public class RepoClientes
{
    // --- Campos Privados ---

    /// <summary>
    /// La lista interna (privada) donde se guardan los objetos Cliente.
    /// </summary>
    private List<Cliente> _clientes = new List<Cliente>();
    
    /// <summary>
    /// Un contador interno para asignar IDs únicos y automáticos.
    /// </summary>
    private int _nextId = 1;

    // --- Métodos Públicos (Operaciones CRUD) ---

    /// <summary>
    /// Agrega un nuevo cliente a la lista (Operación Create).
    /// Asigna un nuevo ID automático, ignorando el ID del objeto 'cliente' recibido.
    /// </summary>
    /// <param name="cliente">Un objeto Cliente (usado como plantilla, su ID será reemplazado).</param>
    public void Agregar(Cliente cliente)
    {
        var nuevoCliente = new Cliente(
            _nextId++, // Asigna el ID actual y luego lo incrementa.
            cliente.Nombre, 
            cliente.Apellido, 
            cliente.Telefono, 
            cliente.Correo,
            cliente.Genero, 
            cliente.FechaNacimiento 
        );
        _clientes.Add(nuevoCliente);
    }

    /// <summary>
    /// Busca un cliente por su ID (Operación Read).
    /// </summary>
    /// <param name="id">El ID del cliente a buscar.</param>
    /// <returns>El objeto <see cref="Cliente"/> si se encuentra; de lo contrario, <c>null</c>.</returns>
    public Cliente Buscar(int id)
    {
        foreach (var cliente in _clientes)
        {
            if (cliente.Id == id)
            {
                return cliente;
            }
        }
        return null;
    }

    /// <summary>
    /// Actualiza los datos de un cliente existente (Operación Update).
    /// </summary>
    /// <param name="id">El ID del cliente a modificar.</param>
    /// <param name="nombre">El nuevo nombre.</param>
    /// <param name="apellido">El nuevo apellido.</param>
    /// <param name="telefono">El nuevo teléfono.</param>
    /// <param name="correo">El nuevo correo.</param>
    /// <param name="genero">El nuevo género.</param>
    /// <param name="fechaNacimiento">La nueva fecha de nacimiento.</param>
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
    
    /// <summary>
    /// Busca clientes que coincidan con un término en varios campos (Nombre, Apellido, Teléfono, Correo, Género).
    /// </summary>
    /// <param name="termino">El texto a buscar (ignora mayúsculas/minúsculas).</param>
    /// <returns>Una <see cref="List{T}"/> de <see cref="Cliente"/> que coinciden.</returns>
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
    
    /// <summary>
    /// Devuelve la lista completa de todos los clientes (Operación Read All).
    /// </summary>
    /// <returns>Una <see cref="List{T}"/> con todos los <see cref="Cliente"/>.</returns>
    public List<Cliente> ObtenerTodos()
    {
        return _clientes;
    }

    /// <summary>
    /// Elimina un cliente de la lista por su ID (Operación Delete).
    /// </summary>
    /// <param name="id">El ID del cliente a eliminar.</param>
    public void Eliminar(int id)
    {
        var cliente = Buscar(id);
        
        if (cliente != null)
        {
            _clientes.Remove(cliente);
        }
    }
}