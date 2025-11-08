using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define la interfaz genérica para un Repositorio (Feedback #18).
    /// </summary>
    /// <typeparam name="T">El tipo de entidad que almacena el repositorio.</typeparam>
    //
    // CORRECCIÓN: Se añade 'class' para permitir que 'T' sea 'null'
    // sin usar la característica 'nullable'.
    //
    public interface IRepositorio<T> where T : class, IEntidad
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
        //
        // CORRECCIÓN: Se quita el '?' de 'T?'.
        // Como 'T' es una 'class', ya puede ser 'null' sin el '?'.
        //
        T Buscar(int id);

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