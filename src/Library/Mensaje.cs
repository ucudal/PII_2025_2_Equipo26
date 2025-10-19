using System;

namespace Library
{
    public class Mensaje : Interaccion
    {
        public string Remitente { get; private set; }
        public string Destinatario { get; private set; }

        public Mensaje(DateTime fecha, string tema, string remitente, string destinatario)
            : base(fecha, tema)
        {
            this.Remitente = remitente;
            this.Destinatario = destinatario;
        }
    }
}