using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define la interfaz genérica para un Repositorio (Feedback #18).
    /// Abstrae las operaciones de almacenamiento.
    /// Esto permite aplicar el principio de Inversión de Dependencias (DIP),
    /// donde las clases de alto nivel (como Fachada) dependen de esta
    /// abstracción, y no de una implementación concreta.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad que almacena el repositorio,
    /// debe implementar IEntidad.</typeparam>
    public interface IRepositorio<T> where T : IEntidad
    {
        /// <summary>
        /// Agrega una entidad al repositorio.
        /// </summary>
        /// <param name="item">La entidad a agregar.</param>
        void Agregar(T item);

        /// <summary>
        /// Busca una entidad por su Id.
        /// </summary>
        /// <param name="id">El Id de la entidad a buscar.</param>
        /// <returns>La entidad encontrada, o null si no se encuentra.</returns>
        T? Buscar(int id);

        /// <summary>
        /// Elimina una entidad por su Id.
        /// </summary>
        /// <param name="id">El Id de la entidad a eliminar.</param>
        void Eliminar(int id);

        /// <summary>
        /// Obtiene una colección de solo lectura de todas las entidades.
        /// </summary>
        /// <returns>Un IReadOnlyList<T> con todas las entidades.</returns>
        IReadOnlyList<T> ObtenerTodos();
    }
}