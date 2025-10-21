using Library;

using System;
using System.Collections.Generic;

// Esta clase es un "Repositorio" (Patrón Repository).
// Su responsabilidad es manejar la lista de todas las ventas generales del sistema
// (aquellas que quizás no están atadas a un cliente específico).
public class RepoVentas
{
    // --- Campos Privados ---

    // La lista interna (privada) donde se guardan los objetos 'Venta'.
    private List<Venta> _ventas = new List<Venta>();
    
    // Un contador para asignar IDs únicos y automáticos a las nuevas ventas.
    private int _nextId = 1;

    // --- Métodos Públicos ---

    // Agrega una nueva venta a la lista (Operación Create).
    public Venta Agregar(string producto, float importe, DateTime fecha)
    {
        // 1. Crea el nuevo objeto Venta, usando el contador de ID.
        //    '_nextId++' usa el valor actual y luego lo incrementa.
        var nuevaVenta = new Venta(_nextId++, producto, importe, fecha);
        
        // 2. Agrega la venta a la lista interna.
        _ventas.Add(nuevaVenta);
        
        // 3. Devuelve la venta recién creada (por si se necesita el ID).
        return nuevaVenta;
    }

    // Devuelve la lista completa de todas las ventas (Operación Read All).
    public List<Venta> ObtenerTodas()
    {
        // Devuelve la referencia a la lista interna.
        return _ventas;
    }
}
