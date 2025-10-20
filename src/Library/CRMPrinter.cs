using Library;

using System.Collections.Generic;

public class CRMPrinter
{
    public string GetTextoCliente(Cliente cliente)
    {
        return $"ID: {cliente.Id}, Nombre: {cliente.Nombre} {cliente.Apellido}, Email: {cliente.Correo}";
    }

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