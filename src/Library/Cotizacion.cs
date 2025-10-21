using Library;
using System;

// Esta clase representa una cotización enviada a un cliente.
// También es un tipo de 'Interaccion', así que hereda de ella.
public class Cotizacion : Interaccion
{
    // --- Propiedades Específicas de Cotizacion ---

    // El valor monetario de lo que se está cotizando (ej: 1500.50).
    public double Monto { get; private set; }
    // Una descripción o detalle de los productos/servicios cotizados.
    public string Detalle { get; private set; }

    // --- Constructor ---
    // Esta es la "receta" para crear un nuevo objeto Cotizacion.
    public Cotizacion(string tema, double monto, string detalle)
        // Llama al constructor de la clase base (Interaccion).
        // Usa 'DateTime.Now' para asignar automáticamente la fecha
        // y hora actual al momento de crear la cotización.
        : base(DateTime.Now, tema)
    {
        // 'base' ya se encargó de 'Fecha' y 'Tema'.
        // Ahora asignamos los valores propios de la cotización.
        this.Monto = monto;
        this.Detalle = detalle;
    }
}
