using Library;

using System;

// Esta clase representa una Venta que se ha completado.
// Es un modelo de datos simple (a veces llamado POCO o DTO).
public class Venta
{
    // --- Propiedades ---

    // Identificador único de la venta.
    public int Id { get; set; }
    
    // Descripción del producto o servicio vendido.
    public string Producto { get; set; }
    
    // El monto o valor total de la venta (usa 'float' para decimales).
    public float Importe { get; set; }
    
    // La fecha y hora en que se registró la venta.
    public DateTime Fecha { get; set; }

    // --- Constructor ---
    // Es la "receta" para crear un nuevo objeto Venta.
    // Pide todos los datos necesarios para registrarla.
    public Venta(int id, string producto, float importe, DateTime fecha)
    {
        // Asigna los valores recibidos a las propiedades de la clase.
        this.Id = id;
        this.Producto = producto;
        this.Importe = importe;
        this.Fecha = fecha;
    }
}
