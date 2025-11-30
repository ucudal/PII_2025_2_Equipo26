namespace Library
{
    /// <summary>
    /// Define los tipos de interacciones posibles en el sistema.
    /// </summary>
    public enum TipoInteraccion
    {
        /// <summary>
        /// Interacción de tipo Llamada.
        /// </summary>
        Llamada,

        /// <summary>
        /// Interacción de tipo Reunión.
        /// </summary>
        Reunion,

        /// <summary>
        /// Interacción de tipo Correo electrónico.
        /// </summary>
        Correo,

        /// <summary>
        /// Interacción de tipo Mensaje (SMS, Chat).
        /// </summary>
        Mensaje,
        
        /// <summary>
        /// Interacción de tipo Cotización.
        /// </summary>
        Cotizacion,
        Venta
    }
}