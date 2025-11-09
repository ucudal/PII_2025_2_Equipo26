using System;

namespace Library
{

    /// <summary>
    /// Clase base abstracta para todos los tipos de interacción (Llamada, Reunion, Correo, etc.).
    /// Define las propiedades comunes que toda interacción debe tener.
    /// No se puede instanciar directamente (es abstracta).
    /// </summary>

    public abstract class Interaccion : IEntidad
    {
        public int Id { get; set; }
        // --- Propiedades Comunes ---

        /// <summary>
        /// Obtiene o establece la fecha y hora en que ocurrió la interacción.
        /// </summary>
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Obtiene o establece un breve resumen o tema de la interacción.
        /// </summary>
        public string Tema { get; set; }

        /// <summary>
        /// Obtiene o establece una <see cref="Nota"/> opcional para agregar más detalles.
        /// (Ejemplo de Agregación).
        /// </summary>
        public Nota NotaAdicional { get; set; }

        // --- Constructor Base ---

        /// <summary>
        /// Inicializa una nueva instancia de una clase derivada de <see cref="Interaccion"/>.
        /// </summary>
        /// <param name="fecha">La fecha y hora de la interacción.</param>
        /// <param name="tema">El tema de la interacción.</param>
        public Interaccion(DateTime fecha, string tema)
        {
            this.Fecha = fecha;
            this.Tema = tema;
        }
    }
}