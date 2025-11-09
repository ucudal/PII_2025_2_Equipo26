namespace Library
{
    /// <summary>
    /// Define la propiedad básica que debe tener
    /// cualquier entidad persistible en un repositorio.
    /// </summary>
    public interface IEntidad
    {
        /// <summary>
        /// Obtiene el Identificador único de la entidad.
        /// </summary>
        int Id { get; }
    }
}