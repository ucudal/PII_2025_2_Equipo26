using System;

namespace Library
{
    /// <summary>
    /// Representa una interacción de tipo Reunión.
    /// </summary>
    public class Reunion : Interaccion
    {
        /// <summary>
        /// Obtiene o establece el lugar de la reunión.
        /// </summary>
        public string Lugar { get; set; }

        /// <summary>
        /// Obtiene el tipo de esta interacción.
        /// </summary>
        public override TipoInteraccion Tipo => TipoInteraccion.Reunion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Reunion"/>.
        /// </summary>
        public Reunion(DateTime fecha, string tema, string lugar)
            : base(fecha, tema)
        {
            this.Lugar = lugar;
        }
    }
}