using Library;
using System;

public class Program
{
    public static void Main()
    {
        var fachada = new Fachada();
        var printer = new CRMPrinter();

        Console.WriteLine("--- Creando Clientes ---");
        fachada.CrearCliente("Ana", "García", "099123456", "ana.garcia@email.com");
        fachada.CrearCliente("Juan", "Rodríguez", "098765432", "juan.rodriguez@email.com");
        Console.WriteLine("Se crearon 2 clientes.\n");

        Console.WriteLine("--- Listado de Clientes ---");
        var todosLosClientes = fachada.VerTodosLosClientes();
        Console.WriteLine(printer.GetTextoListaClientes(todosLosClientes));

        Console.WriteLine("--- Registrando Interacción ---");
        fachada.RegistrarLlamada(1, DateTime.Now, "Llamada de seguimiento", "Enviada");
        Console.WriteLine("Llamada registrada para el cliente con ID 1.\n");

        Console.WriteLine("--- Verificando Interacciones del Cliente 1 ---");
        var clienteAna = fachada.VerTodosLosClientes()[0];
        if (clienteAna.Interacciones.Count > 0 && clienteAna.Interacciones[0] is Llamada llamada)
        {
            Console.WriteLine($"El cliente {clienteAna.Nombre} tiene una llamada del tipo '{llamada.TipoLlamada}' sobre '{llamada.Tema}'.");
        }
    }
}
