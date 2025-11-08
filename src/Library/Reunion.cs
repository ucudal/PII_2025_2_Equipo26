using System;

namespace Library
{

    /// <summary>
    /// Representa una reunión presencial o virtual.
    /// Es un tipo específico de <see cref="Interaccion"/> (Herencia).
    /// </summary>
    public class Reunion : Interaccion
    {
        // --- Propiedad Específica de Reunion ---

        /// <summary>
        /// Obtiene o establece dónde se llevó a cabo la reunión (ej: "Oficina Cliente", "Zoom").
        /// </summary>
        public string Lugar { get; set; }

        // --- Constructor ---

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Reunion"/>.
        /// </summary>
        /// <param name="fecha">La fecha y hora de la reunión.</param>
        /// <param name="tema">El tema o resumen de la reunión.</param>
        /// <param name="lugar">El lugar donde se realizó la reunión.</param>
        public Reunion(DateTime fecha, string tema, string lugar)
            // Llama al constructor de la clase base (Interaccion)
            : base(fecha, tema)
        {
            Lugar = lugar;
        }
    }
}