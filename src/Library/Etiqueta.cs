namespace Library
{

/// <summary>
/// Representa una Etiqueta o "Tag" (ej: "VIP", "Nuevo", "Inactivo").
/// Sirve para clasificar o agrupar objetos <see cref="Cliente"/>.
/// Implementa el patrón Expert: conoce su nombre e identificación.
/// </summary>
    public class Etiqueta : IEntidad, IBuscable
    {
        // --- Propiedades ---

        /// <summary>
        /// Obtiene el identificador numérico único para la etiqueta.
        /// </summary>
        /// <remarks>
        /// 'private set' significa que su valor solo se puede asignar en el constructor.
        /// </remarks>
        public int Id { get; set; }

        /// <summary>
        /// Obtiene o establece el texto visible de la etiqueta (ej: "Cliente Frecuente").
        /// </summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Etiqueta"/>.
        /// </summary>
        /// <param name="id">El ID único para la etiqueta.</param>
        /// <param name="nombre">El nombre de la etiqueta.</param>
        public Etiqueta(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
        }

        /// <summary>
        /// Verifica si la etiqueta coincide con un término de búsqueda.
        /// Busca en el Nombre.
        /// </summary>
        /// <param name="termino">El término a buscar.</param>
        /// <returns>True si coincide, False en caso contrario.</returns>
        public bool Coincide(string termino)
        {
            if (termino == null || termino == "")
            {
                return false;
            }

            if (this.Nombre != null && this.Nombre != "" && this.Nombre.ToLower().Contains(termino.ToLower()))
            {
                return true;
            }

            return false;
        }
    }
}