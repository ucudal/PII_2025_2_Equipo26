namespace Library
{
    /// <summary>
    /// Define la interfaz base para todas las entidades
    /// que pueden ser almacenadas en un Repositorio.
    /// Exige que toda entidad tenga una propiedad 'Id'.
    /// </summary>
    public interface IEntidad
    {
        /// <summary>
        /// Obtiene o establece el Identificador único de la entidad.
        /// </summary>
        
        // --- CORRECCIÓN ---
        // Se agrega el 'set;' para permitir que
        // el RepoBase pueda asignar el ID.
        int Id { get; set; }
    }
}