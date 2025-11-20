using System.Collections.Generic;
using System.Linq;

namespace Library
{
    /// <summary>
    /// Implementa el patrón "Repositorio" (Repository).
    /// Administra la colección de objetos <see cref="Etiqueta"/>.
    /// Hereda la lógica común de <see cref="RepoBase{T}"/>.
    /// </summary>
    public class RepoEtiquetas : RepoBase<Etiqueta>, IRepoEtiquetas
    {
        // --- Campos Privados ---
        // 'private List<Etiqueta> _etiquetas' HA SIDO ELIMINADO
        // 'private int _nextId' HA SIDO ELIMINADO
        // (Ambos son heredados como 'protected _items' y 'protected _nextId')

        // --- Métodos Públicos (Operaciones CRUD) ---

        /// <summary>
        /// Crea una nueva etiqueta con un nombre y la agrega a la lista (Create).
        /// Implementa el patrón Creator: RepoEtiquetas tiene la información para crear instancias de Etiqueta.
        /// </summary>
        /// <param name="nombre">El nombre de la nueva etiqueta.</param>
        public void Crear(string nombre)
        {
            // '++' después de '_nextId' usa el valor actual y LUEGO lo incrementa.
            var nuevaEtiqueta = new Etiqueta(this._nextId++, nombre);
            
            this._items.Add(nuevaEtiqueta); // Usa la lista _items heredada
        }

        // --- 'ObtenerTodas()' HA SIDO ELIMINADO (Heredado) ---
        // --- 'Buscar(int id)' HA SIDO ELIMINADO (Heredado) ---
        // --- 'Eliminar(int id)' HA SIDO ELIMINADO (Heredado) ---
    }
}