using Library;
using System;

/// <summary>
/// Representa una interacción de tipo Llamada Telefónica.
/// Hereda de <see cref="Interaccion"/> (Herencia).
/// </summary>
public class Llamada : Interaccion
{
    // --- Propiedad Específica de Llamada ---

    /// <summary>
    /// Obtiene o establece el tipo de llamada (ej: "Entrante", "Saliente", "Perdida").
    /// </summary>
    public string TipoLlamada { get; set; }

    /// <summary>
    /// Inicializa una nueva instancia de la clase <see cref="Llamada"/>.
    /// </summary>
    /// <param name="fecha">La fecha y hora de la llamada.</param>
    /// <param name="tema">El tema o resumen de la llamada.</param>
    /// <param name="tipoLlamada">El tipo de llamada.</param>
    public Llamada(DateTime fecha, string tema, string tipoLlamada)
        : base(fecha, tema) // Llama al constructor de la clase base (Interaccion).
    {
        TipoLlamada = tipoLlamada;
    }
}