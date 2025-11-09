using System.Collections.Generic;
using library;

namespace Library
{
    /// <summary>
    /// Repositorio de Ventas.
    /// Implementa Singleton y IRepoVentas (DIP).
    /// Hereda de Repositorio<Venta> para reutilizar la lógica (Feedback #18).
    /// </summary>
    public class RepoVentas : Repositorio<Venta>, IRepoVentas
    {
        public static RepoVentas _instancia;

        /// <summary>
        /// Constructor privado para asegurar el patrón Singleton.
        /// </summary>
        public RepoVentas() : base()
        {
        }

        /// <summary>
        /// Obtiene la instancia única del repositorio (Singleton).
        /// </summary>
        public static RepoVentas Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new RepoVentas();
                }
                return _instancia;
            }
        }
        
        // ¡Lógica heredada!
        // (Tu compañero "Nahuel" agregará el método 'CrearVenta' aquí en su commit)
    }
}