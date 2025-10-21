using Library;
using System;

// Esta clase representa una llamada telefónica.
// Como es un tipo de interacción, hereda de la clase base 'Interaccion'.
public class Llamada : Interaccion
{
    // --- Propiedad Específica de Llamada ---

    // Indica si la llamada fue "Entrante", "Saliente" o "Perdida".
    public string TipoLlamada { get; set; }

    // Constructor
    // Para crear una nueva 'Llamada', necesitamos los datos base (fecha, tema)
    // y también el dato específico de esta clase (tipoLlamada).
    public Llamada(DateTime fecha, string tema, string tipoLlamada)
        : base(fecha, tema)// <-- Llama al constructor de 'Interaccion' primero.
    {
        // 'base' se encargó de asignar 'fecha' y 'tema'.
        // Ahora asignamos la propiedad propia de 'Llamada'.
        TipoLlamada = tipoLlamada;
    }
}
