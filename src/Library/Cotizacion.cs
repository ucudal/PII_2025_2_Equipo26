using Library;
using System;

/// <summary>
/// Representa una cotización enviada a un cliente.
/// Hereda de <see cref="Interaccion"/>, demostrando el principio de Herencia.
/// </summary>
public class Cotizacion : Interaccion
{
    // --- Propiedades Específicas de Cotizacion ---

    /// <summary>
    /// Obtiene el valor monetario de lo que se está cotizando.
    /// </summary>
    public double Monto { get; private set; }
    
    /// <summary>
    /// Obtiene una descripción o detalle de los productos/servicios cotizados.
    /// </summary>
    public string Detalle { get; private set; }

    // --- Constructor ---
    
    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Cotizacion"/>.
    /// Asigna automáticamente la fecha y hora actual.
    /// </summary>
    /// <param name="tema">El tema o título de la cotización (ej: "Cotización CRM Licencia Plus").</param>
    /// <param name="monto">El valor monetario de la cotización.</param>
    /// <param name="detalle">Una descripción de los ítems cotizados.</param>
    public Cotizacion(string tema, double monto, string detalle)
        // Llama al constructor de la clase base (Interaccion).
        // Usa 'DateTime.Now' para asignar automáticamente la fecha
        // y hora actual al momento de crear la cotización.
        : base(DateTime.Now, tema)
    {
        this.Monto = monto;
        this.Detalle = detalle;
    }
}