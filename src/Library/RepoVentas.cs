using Library;
using System;
using System.Collections.Generic;

/// <summary>
/// Implementa el patrón "Repositorio" (Repository).
/// Su responsabilidad es manejar la lista de todas las ventas generales del sistema
/// (aquellas que no están atadas a un cliente específico).
/// </summary>
public class RepoVentas
{
    // --- Campos Privados ---

    /// <summary>
    /// La lista interna (privada) donde se guardan los objetos 'Venta'.
    /// </summary>
    private List<Venta> _ventas = new List<Venta>();
    
    /// <summary>
    /// Un contador para asignar IDs únicos y automáticos a las nuevas ventas.
    /// </summary>
    private int _nextId = 1;

    // --- Métodos Públicos ---

    /// <summary>
    /// Agrega una nueva venta a la lista (Operación Create).
    /// </summary>
    /// <param name="producto">Descripción del producto o servicio vendido.</param>
    /// <param name="importe">El monto o valor total de la venta.</param>
    /// <param name="fecha">La fecha y hora en que se registró la venta.</param>
    /// <returns>La <see cref="Venta"/> recién creada (con su ID asignado).</returns>
    public Venta Agregar(string producto, float importe, DateTime fecha)
    {
        var nuevaVenta = new Venta(_nextId++, producto, importe, fecha);
        _ventas.Add(nuevaVenta);
        return nuevaVenta;
    }

    /// <summary>
    /// Devuelve la lista completa de todas las ventas (Operación Read All).
    /// </summary>
    /// <returns>Una <see cref="List{T}"/> de <see cref="Venta"/>.</returns>
    public List<Venta> ObtenerTodas()
    {
        return _ventas;
    }
}