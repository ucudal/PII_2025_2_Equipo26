using Library;
using System;

/// <summary>
/// Representa una Venta que se ha completado.
/// Es un modelo de datos simple (a veces llamado POCO o DTO).
/// </summary>
public class Venta : IEntidad
{
    // --- Propiedades ---

    /// <summary>
    /// Obtiene o establece el identificador único de la venta.
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Obtiene o establece la descripción del producto o servicio vendido.
    /// </summary>
    public string Producto { get; set; }
    
    /// <summary>
    /// Obtiene o establece el monto o valor total de la venta.
    /// </summary>
    public float Importe { get; set; }
    
    /// <summary>
    /// Obtiene o establece la fecha y hora en que se registró la venta.
    /// </summary>
    public DateTime Fecha { get; set; }

    // --- Constructor ---
    
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Venta"/>.
    /// </summary>
    /// <param name="id">El ID único de la venta.</param>
    /// <param name="producto">Descripción del producto/servicio.</param>
    /// <param name="importe">Monto de la venta.</param>
    /// <param name="fecha">Fecha de la venta.</param>
    public Venta(int id, string producto, float importe, DateTime fecha)
    {
        this.Id = id;
        this.Producto = producto;
        this.Importe = importe;
        this.Fecha = fecha;
    }
}