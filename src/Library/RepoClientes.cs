using Library;
using System;
using System.Collections.Generic;

// Esta clase es un "Repositorio" (Patrón Repository).
// Su única responsabilidad es guardar y administrar la lista de Clientes.
// Funciona como la "base de datos" en memoria para los clientes.
public class RepoClientes
{
    // --- Campos Privados ---

    // La lista real donde se guardan los objetos Cliente.
    // Es 'private' para que nadie fuera de esta clase pueda modificarla directamente.
    private List<Cliente> _clientes = new List<Cliente>();
    
    // Un contador interno para asignar IDs únicos y automáticos.
    private int _nextId = 1;

    // --- Métodos Públicos (Operaciones CRUD) ---

    // Agrega un nuevo cliente a la lista (Create).
    public void Agregar(Cliente cliente)
    {
        // Crea una *nueva* instancia de Cliente.
        // Esto asegura que el ID sea asignado por el Repositorio
        // usando el contador '_nextId'.
        var nuevoCliente = new Cliente(
            _nextId++, // Asigna el ID actual y luego lo incrementa para el próximo.
            cliente.Nombre, 
            cliente.Apellido, 
            cliente.Telefono, 
            cliente.Correo,
            cliente.Genero, 
            cliente.FechaNacimiento 
        );
        // Añade el cliente recién creado a la lista interna.
        _clientes.Add(nuevoCliente);
    }

    // Busca un cliente por su ID (Read).
    public Cliente Buscar(int id)
    {
        // Recorre la lista de clientes uno por uno.
        foreach (var cliente in _clientes)
        {
            // Si el ID del cliente actual es igual al que buscamos...
            if (cliente.Id == id)
            {
                // ...lo devuelve.
                return cliente;
            }
        }
        // Si termina de recorrer la lista y no lo encontró, devuelve null.
        return null;
    }

    // Actualiza los datos de un cliente existente (Update).
    public void Modificar(int id, string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
    {
        // 1. Usa el método Buscar para encontrar al cliente.
        var cliente = Buscar(id);
        
        // 2. Si el cliente existe (no es null)...
        if (cliente != null)
        {
            // 3. ...actualiza todas sus propiedades con los nuevos valores.
            cliente.Nombre = nombre;
            cliente.Apellido = apellido;
            cliente.Telefono = telefono;
            cliente.Correo = correo;
            cliente.Genero = genero; 
            cliente.FechaNacimiento = fechaNacimiento; 
        }
    }
    
    // Busca clientes que coincidan con un 'termino' en varios de sus campos.
    public List<Cliente> BuscarPorTermino(string termino)
    {
        // Crea una lista nueva para guardar los resultados.
        var resultados = new List<Cliente>();
        // Pasa el término a minúsculas para que la búsqueda no distinga mayúsculas.
        var busqueda = termino.ToLower();

        // Recorre todos los clientes en la lista.
        foreach (var cliente in _clientes)
        {
            // Compara el término de búsqueda (en minúsculas) con varias propiedades.
            // 'Contains' chequea si el texto está *dentro* del campo.
            if (cliente.Nombre.ToLower().Contains(busqueda) ||
                cliente.Apellido.ToLower().Contains(busqueda) ||
                cliente.Telefono.Contains(busqueda) || // Teléfono no se pasa a minúsculas
                cliente.Correo.ToLower().Contains(busqueda) ||
                cliente.Genero.ToLower().Contains(busqueda)) 
            {
                // Si coincide en cualquier campo, lo agrega a la lista de resultados.
                resultados.Add(cliente);
            }
        }
        // Devuelve la lista de clientes que coincidieron.
        return resultados;
    }
    
    // Devuelve la lista completa de todos los clientes (Read All).
    public List<Cliente> ObtenerTodos()
    {
        // Devuelve la referencia a la lista interna.
        return _clientes;
    }

    // Elimina un cliente de la lista por su ID (Delete).
    public void Eliminar(int id)
    {
        // 1. Busca al cliente que se quiere borrar.
        var cliente = Buscar(id);
        
        // 2. Si se encontró...
        if (cliente != null)
        {
            // 3. ...se lo quita de la lista.
            _clientes.Remove(cliente);
        }
    }
}
