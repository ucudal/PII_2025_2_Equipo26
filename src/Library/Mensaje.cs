using System;

namespace Library
{
    // Esta clase representa un mensaje de texto o chat (ej: SMS, WhatsApp).
    // Es otro tipo específico de 'Interaccion', por lo tanto, hereda de ella.
    public class Mensaje : Interaccion
    {
        // --- Propiedades Específicas de Mensaje ---

        // Quién envía el mensaje (ej: número de teléfono o ID de usuario).
        public string Remitente { get; private set; }
        
        // Quién recibe el mensaje.
        public string Destinatario { get; private set; }

        // --- Constructor ---
        // Para crear un nuevo Mensaje, pide los datos base (fecha, tema)
        // y los datos propios de esta clase.
        public Mensaje(DateTime fecha, string tema, string remitente, string destinatario)
            : base(fecha, tema) // <-- Llama al constructor de la clase 'Interaccion'.
        {
            // 'base' ya se encargó de 'fecha' y 'tema'.
            // Asignamos las propiedades de Mensaje.
            this.Remitente = remitente;
            this.Destinatario = destinatario;
        }
    }
}
