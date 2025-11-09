namespace Library
{
    /// <summary>
    /// Interfaz que define una entidad con un Identificador único.
    /// Se utiliza para la restricción 'where T : IEntidad' en el Repositorio Genérico.
    /// </summary>
    public interface IEntidad
    {
        /// <summary>
        /// Obtiene o establece el Identificador único de la entidad.
        /// </summary>
        int Id { get; set; }
    }
}