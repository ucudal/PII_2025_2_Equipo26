using System;

namespace Library
{
    /// <summary>
    /// Representa una interacción de tipo Correo Electrónico.
    /// </summary>
    public class Correo : Interaccion
    {
        /// <summary>
        /// Obtiene el remitente del correo.
        /// </summary>
        public string Remitente { get; private set; }
        
        /// <summary>
        /// Obtiene el destinatario del correo.
        /// </summary>
        public string Destinatario { get; private set; }
        
        /// <summary>
        /// Obtiene el asunto del correo.
        /// </summary>
        public string Asunto { get; private set; }

        /// <summary>
        /// (NUEVO) Obtiene el tipo de esta interacción.
        /// </summary>
        public override TipoInteraccion Tipo => TipoInteraccion.Correo;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Correo"/>.
        /// </summary>
        public Correo(DateTime fecha, string tema, string remitente, string destinatario, string asunto)
            : base(fecha, tema) // Llama al constructor de la clase base.
        {
            this.Remitente = remitente;
            this.Destinatario = destinatario;
            this.Asunto = asunto;
        }
    }
}