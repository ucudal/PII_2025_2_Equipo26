using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Repositorio de Etiquetas.
    /// Implementa Singleton y IRepoEtiquetas (DIP).
    /// Hereda de Repositorio<Etiqueta> para reutilizar la lógica (Feedback #18).
    /// </summary>
    public class RepoEtiquetas : Repositorio<Etiqueta>, IRepoEtiquetas
    {
        public static RepoEtiquetas _instancia;

        /// <summary>
        /// Constructor privado para asegurar el patrón Singleton.
        /// </summary>
        public RepoEtiquetas() : base()
        {
        }

        /// <summary>
        /// Obtiene la instancia única del repositorio (Singleton).
        /// </summary>
        public static RepoEtiquetas Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new RepoEtiquetas();
                }
                return _instancia;
            }
        }

        /// <summary>
        /// Devuelve la lista completa de todas las etiquetas (Read All).
        /// </summary>
        /// <returns>Una <see cref="List{T}"/> de <see cref="Etiqueta"/>.</returns>
        public List<Etiqueta> ObtenerTodas()
        {
            return _etiquetas;
        }

        /// <summary>
        /// Crea una nueva etiqueta con un nombre y la agrega a la lista (Create).
        /// </summary>
        /// <param name="nombre">El nombre de la nueva etiqueta.</param>
        public void Crear(string nombre)
        {
            // '++' después de '_nextId' usa el valor actual y LUEGO lo incrementa.
            var nuevaEtiqueta = new Etiqueta(_nextId++, nombre);

            _etiquetas.Add(nuevaEtiqueta);
        }
    }
}