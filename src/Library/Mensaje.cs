using System;

namespace Library
{
    /// <summary>
    /// Representa una interacción de tipo Mensaje (SMS, Chat).
    /// </summary>
    public class Mensaje : Interaccion
    {
        /// <summary>
        /// Obtiene o establece el remitente.
        /// </summary>
        public string Remitente { get; set; }

        /// <summary>
        /// Obtiene o establece el destinatario.
        /// </summary>
        public string Destinatario { get; set; }

        /// <summary>
        /// Obtiene el tipo de esta interacción.
        /// </summary>
        public override TipoInteraccion Tipo => TipoInteraccion.Mensaje;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Mensaje"/>.
        /// </summary>
        public Mensaje(DateTime fecha, string tema, string remitente, string destinatario)
            : base(fecha, tema)
        {
            this.Remitente = remitente;
            this.Destinatario = destinatario;
        }
    }
}