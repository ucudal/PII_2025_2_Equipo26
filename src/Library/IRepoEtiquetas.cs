using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define el contrato para el Repositorio de Etiquetas.
    /// Hereda la funcionalidad base de IRepoBase.
    /// </summary>
    
    //  hereda de IRepoBase<Etiqueta>
    public interface IRepoEtiquetas : IRepoBase<Etiqueta>
    {
        /// <summary>
        /// Crea una nueva etiqueta.
        /// </summary>
        void Crear(string nombre);

    }
}