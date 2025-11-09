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
        
        // ¡Lógica heredada!
        // (Tu compañero "Nahuel" agregará el método 'CrearEtiqueta' aquí en su commit)
    }
}