namespace Library
{
    /// <summary>
    /// Define la interfaz para entidades que pueden ser buscadas por un término de texto.
    /// </summary>
    public interface IBuscable
    {
        /// <summary>
        /// Determina si la entidad coincide con el término de búsqueda dado.
        /// </summary>
        /// <param name="termino">El término a buscar.</param>
        /// <returns>True si coincide, False en caso contrario.</returns>
        bool Coincide(string termino);
    }
}
