using System;

namespace Library
{
    /// <summary>
    /// Representa una interacción de tipo Correo Electrónico.
    /// Hereda de <see cref="Interaccion"/>, demostrando el principio de Herencia.
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
        /// Inicializa una nueva instancia de la clase <see cref="Correo"/>.
        /// </summary>
        /// <param name="fecha">La fecha y hora en que se envió/recibió el correo.</param>
        /// <param name="tema">Un resumen o tema del correo.</param>
        /// <param name="remitente">La dirección de correo del remitente.</param>
        /// <param name="destinatario">La dirección de correo del destinatario.</param>
        /// <param name="asunto">El asunto del correo.</param>
        public Correo(DateTime fecha, string tema, string remitente, string destinatario, string asunto)
            : base(fecha, tema) // Llama al constructor de la clase base (Interaccion).
        {
            this.Remitente = remitente;
            this.Destinatario = destinatario;
            this.Asunto = asunto;
        }
    }
}