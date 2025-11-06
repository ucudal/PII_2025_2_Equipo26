using Library;
using System.Collections.Generic;

/// <summary>
/// Clase auxiliar (Helper/Service) con la responsabilidad única
/// de formatear la información de los clientes para ser mostrada.
/// Sigue el principio SRP, ya que la clase Cliente no sabe cómo imprimirse.
/// </summary>
public class CRMPrinter
{
    /// <summary>
    /// Genera una representación de texto de una sola línea para un cliente.
    /// </summary>
    /// <param name="cliente">El objeto Cliente a formatear.</param>
    /// <returns>Un string con los datos básicos del cliente (ID, Nombre, Apellido, Email).</returns>
    public string GetTextoCliente(Cliente cliente)
    {
        return $"ID: {cliente.Id}, Nombre: {cliente.Nombre} {cliente.Apellido}, Email: {cliente.Correo}";
    }

    /// <summary>
    /// Genera una representación de texto multilinea para una lista de clientes.
    /// </summary>
    /// <param name="clientes">La lista de objetos Cliente a formatear.</param>
    /// <returns>Un string único con cada cliente en una nueva línea.</returns>
    public string GetTextoListaClientes(List<Cliente> clientes)
    {
        // 1. Empezamos con un string vacío
        string resultado = "";

        foreach (var cliente in clientes)
        {
            // 2. Le "sumamos" el texto de cada cliente y un salto de línea
            resultado += GetTextoCliente(cliente) + "\n";
        }
        
        // 3. Devolvemos el string completo
        return resultado;
    }
}