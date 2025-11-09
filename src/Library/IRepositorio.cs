using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define el contrato genérico para un repositorio de solo lectura
    /// y búsqueda/eliminación básica.
    /// </summary>
    /// <typeparam name="T">El tipo de entidad, debe implementar IEntidad.</typeparam>
    public interface IRepositorio<T> where T : IEntidad
    {
        /// <summary>
        /// Busca una entidad por su ID.
        /// </summary>
        /// <param name="id">El ID a buscar.</param>
        /// <returns>La entidad encontrada, o <c>null</c>.</returns>
        T Buscar(int id);

        /// <summary>
        /// Devuelve una lista de solo lectura de todas las entidades.
        /// </summary>
        /// <returns>Un <see cref="IReadOnlyList{T}"/>.</returns>
        IReadOnlyList<T> ObtenerTodas();

        /// <summary>
        /// Elimina una entidad de la lista por su ID.
        /// </summary>
        /// <param name="id">El ID de la entidad a eliminar.</param>
        void Eliminar(int id);
    }
}