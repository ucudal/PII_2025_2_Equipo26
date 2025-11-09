using Library;
using System;
using System.Collections.Generic;

/// <summary>
/// Implementa el patrón "Repositorio" (Repository).
/// Su responsabilidad es manejar la lista de todos los clientes del sistema.
/// </summary>
public class RepoClientes : Repositorio<Cliente>
{
    public Cliente CrearCliente(string nombre, string apellido, string telefono, string correo, string genero, DateTime fechaNacimiento)
    {
        var nuevoCliente = new Cliente(nombre, apellido, telefono, correo, genero, fechaNacimiento);
        this.Agregar(nuevoCliente);
        return nuevoCliente;
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
    /// <summary>
    /// Busca clientes que coincidan con un término en varios campos (Nombre, Apellido, Teléfono, Correo, Género).
    /// </summary>
    /// <param name="termino">El texto a buscar (ignora mayúsculas/minúsculas).</param>
    /// <returns>Una <see cref="List{T}"/> de <see cref="Cliente"/> que coinciden.</returns>
    public List<Cliente> BuscarPorTermino(string termino)
    {
        var resultados = new List<Cliente>();
        var busqueda = termino.ToLower();

        // src/Library/RepoClientes.cs
        foreach (var cliente in elementos)
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
}