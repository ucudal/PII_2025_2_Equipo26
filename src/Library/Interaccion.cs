using Library;
using System;

// Esta es una clase "abstracta". Sirve como plantilla base.
// No se puede crear un objeto "Interaccion" directamente.
// Define las cosas que TODAS las interacciones (Llamadas, Correos, etc.) deben tener.
public abstract class Interaccion
{
    // --- Propiedades Comunes ---

    // Guarda la fecha y hora en que ocurrió la interacción.
    public DateTime Fecha { get; set; }
    // Un breve resumen o tema de qué se trató (ej: "Consulta de precios").
    public string Tema { get; set; }
    // Un objeto 'Nota' opcional para agregar más detalles o un recordatorio.
    public Nota NotaAdicional { get; set; }

    // --- Constructor Base ---
    // Este constructor debe ser llamado por todas las clases que hereden de Interaccion
    // (como Llamada, Reunion, Correo)
    public Interaccion(DateTime fecha, string tema)
    {
        // 'this.Fecha' es la propiedad (arriba), 'fecha' es el valor que recibimos.
        this.Fecha = fecha;
        this.Tema = tema;
    }
}
