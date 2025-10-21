using System;

namespace Library
{
    public class Correo : Interaccion
    {
        // Esta clase representa un correo electrónico.
        // Es un tipo específico de 'Interaccion', por lo tanto, hereda de ella.
        public string Remitente { get; private set; }
        public string Destinatario { get; private set; }
        public string Asunto { get; private set; }

        // --- Constructor ---
        // Para crear un nuevo Correo, necesitamos los datos base (fecha, tema)
        // y también los datos propios de esta clase (remitente, destinatario, asunto).
        public Correo(DateTime fecha, string tema, string remitente, string destinatario, string asunto)
            : base(fecha, tema) // <-- Llama al constructor de la clase 'Interaccion' primero.
        {
            // 'base' ya se encargó de 'fecha' y 'tema'.
            // Ahora asignamos las propiedades de Correo.
            this.Remitente = remitente;
            this.Destinatario = destinatario;
            this.Asunto = asunto;
        }
    }
}
