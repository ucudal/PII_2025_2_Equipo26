using System;

namespace Library
{
    public class Correo : Interaccion
    {
        public string Remitente { get; private set; }
        public string Destinatario { get; private set; }
        public string Asunto { get; private set; }

        public Correo(DateTime fecha, string tema, string remitente, string destinatario, string asunto)
            : base(fecha, tema)
        {
            this.Remitente = remitente;
            this.Destinatario = destinatario;
            this.Asunto = asunto;
        }
    }
}