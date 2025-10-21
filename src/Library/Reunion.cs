using Library;

using System;

// Esta clase representa una reunión presencial o virtual.
// Es un tipo específico de 'Interaccion', por lo tanto, hereda de ella.
public class Reunion : Interaccion
{
    // --- Propiedad Específica de Reunion ---

    // Dónde se llevó a cabo la reunión (ej: "Oficina Cliente", "Zoom", "Café Central").
    public string Lugar { get; set; }

    // --- Constructor ---
    // Esta es la "receta" para crear un nuevo objeto Reunion.
    public Reunion(DateTime fecha, string tema, string lugar)
        // Llama al constructor de la clase base (Interaccion)
        // para que guarde la fecha y el tema.
        : base(fecha, tema)
    {
        // 'base' ya se encargó de 'fecha' y 'tema'.
        // Ahora asignamos la propiedad propia de 'Reunion'.
        Lugar = lugar;
    }
}
