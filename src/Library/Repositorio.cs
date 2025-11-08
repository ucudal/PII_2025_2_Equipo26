using System.Collections.Generic;
// CORRECCIÓN: Se elimina 'using System.Linq;' porque no se usará FirstOrDefault.

namespace Library
{
    /// <summary>
    /// Implementación base genérica de IRepositorio<T>.
    /// Centraliza toda la lógica común de manejo de listas (Feedback #18).
    /// </summary>
    /// <typeparam name="T">El tipo de entidad a almacenar.</typeparam>
    //
    // CORRECCIÓN: Se añade 'class' para que T pueda ser 'null'.
    //
    public abstract class Repositorio<T> : IRepositorio<T> where T : class, IEntidad
    {
        /// <summary>
        /// Lista protegida que almacena los elementos.
        /// </summary>
        protected readonly List<T> elementos = new List<T>();
        
        private int nextId = 1;

        /// <inheritdoc />
        public virtual void Agregar(T item)
        {
            if (item.Id == 0)
            {
                item.Id = this.nextId;
                this.nextId++;
            }
            this.elementos.Add(item);
        }

        /// <inheritdoc />
        /// <remarks>
        //
        // CORRECCIÓN: Se elimina LINQ (FirstOrDefault) y la
        // expresión lambda (=>) para no usar "sistema de flechas".
        // También se quita el '?' de 'T?'.
        //
        /// </remarks>
        public virtual T Buscar(int id)
        {
            // Bucle manual en lugar de FirstOrDefault
            foreach (T item in this.elementos)
            {
                if (item.Id == id)
                {
                    return item; // Se encontró el elemento
                }
            }
            
            // Si el bucle termina, no se encontró nada
            return null; // Esto es válido porque T es 'class'
        }

        /// <inheritdoc />
        public virtual void Eliminar(int id)
        {
            // CORRECCIÓN: Se quita el '?' de 'T? item'.
            T item = this.Buscar(id);
            if (item != null)
            {
                this.elementos.Remove(item);
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// CORRECCIÓN DE ENCAPSULAMIENTO (Feedback #10):
        /// Devuelve una vista de solo lectura de la lista.
        /// </remarks>
        public virtual IReadOnlyList<T> ObtenerTodos()
        {
            // AsReadOnly() NO es LINQ, es un método de List<T>,
            // así que esto está permitido.
            return this.elementos.AsReadOnly();
        }
    }
}