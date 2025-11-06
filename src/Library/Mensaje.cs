using System;

namespace Library
{
    /// <summary>
    /// Representa una interacción de tipo Mensaje (SMS, Chat, etc.).
    /// Hereda de <see cref="Interaccion"/> (Herencia).
    /// </summary>
    public class Mensaje : Interaccion
    {
        // --- Propiedades Específicas de Mensaje ---

        /// <summary>
        /// Obtiene quién envía el mensaje (ej: número de teléfono o ID).
        /// </summary>
        public string Remitente { get; private set; }
        
        /// <summary>
        /// Obtiene quién recibe el mensaje.
        /// </summary>
        public string Destinatario { get; private set; }

        // --- Constructor ---
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Mensaje"/>.
        /// </summary>
        /// <param name="fecha">La fecha y hora del mensaje.</param>
        /// <param name="tema">El tema o resumen del mensaje.</param>
        /// <param name="remitente">El remitente del mensaje.</param>
        /// <param name="destinatario">El destinatario del mensaje.</param>
        public Mensaje(DateTime fecha, string tema, string remitente, string destinatario)
            : base(fecha, tema) // Llama al constructor de la clase base (Interaccion).
        {
            this.Remitente = remitente;
            this.Destinatario = destinatario;
        }
    }
}