using System;

namespace Library
{
    /// <summary>
    /// Representa una interacción de tipo Cotización.
    /// </summary>
    public class Cotizacion : Interaccion
    {
        /// <summary>
        /// Obtiene o establece el monto cotizado.
        /// </summary>
        public double Monto { get; set; }

        /// <summary>
        /// Obtiene o establece el detalle de la cotización.
        /// </summary>
        public string Detalle { get; set; }

        /// <summary>
        /// Obtiene el tipo de esta interacción.
        /// </summary>
        public override TipoInteraccion Tipo => TipoInteraccion.Cotizacion;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Cotizacion"/>.
        /// Asigna la fecha actual por defecto.
        /// </summary>
        public Cotizacion(string tema, double monto, string detalle)
            : base(DateTime.Now, tema) // Asume la fecha de creación.
        {
            this.Monto = monto;
            this.Detalle = detalle;
        }
    }
}