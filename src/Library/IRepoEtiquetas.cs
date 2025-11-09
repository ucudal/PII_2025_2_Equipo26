using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define el contrato para el Repositorio de Etiquetas.
    /// Hereda la funcionalidad base de IRepositorio.
    /// </summary>
    
    //  hereda de IRepositorio<Etiqueta>
    public interface IRepoEtiquetas : IRepositorio<Etiqueta>
    {
        /// <summary>
        /// Crea una nueva etiqueta.
        /// </summary>
        void Crear(string nombre);

    }
}