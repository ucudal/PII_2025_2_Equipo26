using System;

namespace Library
{
    /// <summary>
    /// Representa una interacción de tipo Llamada Telefónica.
    /// </summary>
    public class Llamada : Interaccion
    {
        /// <summary>
        /// Obtiene o establece el tipo de llamada (ej: "Entrante", "Saliente").
        /// </summary>
        public string TipoLlamada { get; set; }

        /// <summary>
        /// (NUEVO) Obtiene el tipo de esta interacción.
        /// </summary>
        public override TipoInteraccion Tipo
        {
            get { return TipoInteraccion.Llamada; }
        }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Llamada"/>.
        /// </summary>
        public Llamada(DateTime fecha, string tema, string tipoLlamada)
            : base(fecha, tema) // Llama al constructor de la clase base.
        {
            this.TipoLlamada = tipoLlamada;
        }

        public override bool EsSinRespuesta()
        {
            return this.TipoLlamada == "Recibida";
        }
    }
}