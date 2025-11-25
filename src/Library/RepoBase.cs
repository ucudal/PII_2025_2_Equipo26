using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    /// <summary>
    /// Proporciona la implementación base para todos los repositorios.
    /// Implementa el patrón Repository: gestiona la persistencia y acceso a datos de manera genérica.
    /// ...
    /// </summary>
    /// <typeparam name="T">El tipo de entidad, debe implementar IEntidad.</typeparam>
    
    // 1. Añadimos la implementación de 'IRepoBase<T>'
    public abstract class RepoBase<T> : IRepoBase<T> where T : IEntidad
    {
        // (El resto del código de esta clase que te di antes
        //  con _items, _nextId, Buscar, ObtenerTodos y Eliminar
        //  permanece EXACTAMENTE IGUAL.)
        
        /// <summary>
        /// Lista protegida de ítems, accesible por las clases derivadas.
        /// </summary>
        protected readonly List<T> _items = new List<T>();
        
        /// <summary>
        /// Contador de ID protegido, accesible por las clases derivadas.
        /// </summary>
        protected int _nextId = 1;


        /// <summary>
        /// Agrega una entidad ya creada a la lista y le asigna un ID.
        /// </summary>
        /// <param name="item">La entidad a agregar.</param>
        public void Agregar(T item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), "El item a agregar no puede ser nulo.");
            }
            
            item.Id = this._nextId; // Asigna el ID
            this._nextId++;         // Incrementa el contador para el próximo
            this._items.Add(item);  // Agrega a la lista
        }
        public T Buscar(int id)
        {
            foreach (var item in this._items)
            {
                if (item.Id == id)
                {
                    return item;
                }
            }
            return default(T); // Devuelve null (para clases)
        }

        /// <summary>
        /// Devuelve una lista de solo lectura de todas las entidades.
        /// </summary>
        public IReadOnlyList<T> ObtenerTodas()
        {
            return this._items.AsReadOnly();
        }

        /// <summary>
        /// Elimina una entidad de la lista por su ID.
        /// </summary>
        public void Eliminar(int id)
        {
            var item = this.Buscar(id);
            if (item != null)
            {
                this._items.Remove(item);
            }
        }

        /// <summary>
        /// Busca entidades que coincidan con un término de búsqueda.
        /// Utiliza la interfaz <see cref="IBuscable"/> si la entidad la implementa.
        /// </summary>
        /// <param name="termino">El término a buscar.</param>
        /// <returns>Una lista de entidades que coinciden.</returns>
        public List<T> BuscarPorTermino(string termino)
        {
            var resultados = new List<T>();

            foreach (var item in this._items)
            {
                var buscable = item as IBuscable;
                if (buscable != null && buscable.Coincide(termino))
                {
                    resultados.Add(item);
                }
            }
            return resultados;
        }
    }
}