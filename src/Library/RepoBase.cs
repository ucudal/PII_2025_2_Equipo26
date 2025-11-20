using System;
using System.Collections.Generic;
using System.Linq;

namespace Library
{
    /// <summary>
    /// Proporciona la implementación base para todos los repositorios.
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


        // --- CORRECCIÓN: MÉTODO 'Agregar' FALTANTE ---
        /// <summary>
        /// Agrega una entidad ya creada a la lista y le asigna un ID.
        /// Este es el método que 'RepoClientes' llamará con 'base.Agregar(nuevoCliente)'.
        /// </summary>
        /// <param name="item">La entidad a agregar (ej. un Cliente).</param>
        public void Agregar(T item)
        {
            if (item != null)
            {
                item.Id = this._nextId; // Asigna el ID
                this._nextId++;         // Incrementa el contador para el próximo
                this._items.Add(item);  // Agrega a la lista
            }
        }
        // --- FIN DE LA CORRECCIÓN ---


        /// <summary>
        /// Busca una entidad por su ID.
        /// </summary>
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
    }
}