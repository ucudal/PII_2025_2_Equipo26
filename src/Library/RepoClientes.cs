using System.Collections.Generic;
// No necesitas System.Linq

namespace Library
{
    /// <summary>
    /// Repositorio de Clientes.
    /// Implementa Singleton y IRepoClientes (DIP).
    /// Hereda de Repositorio<Cliente> para reutilizar la lógica (Feedback #18).
    /// </summary>
    public class RepoClientes : Repositorio<Cliente>, IRepoClientes
    {
        private static RepoClientes _instancia;

        /// <summary>
        /// Constructor privado para asegurar el patrón Singleton.
        /// </summary>
        private RepoClientes() : base()
        {
        }

        /// <summary>
        /// Obtiene la instancia única del repositorio (Singleton).
        /// </summary>
        public static RepoClientes Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new RepoClientes();
                }
                return _instancia;
            }
        }

        // ¡YA NO NECESITAS LOS MÉTODOS Agregar, Buscar, Eliminar, ObtenerTodos!
        // Se heredan de Repositorio<Cliente>.
        
        // (Tu compañero "Nahuel" agregará el método 'CrearCliente' aquí en su commit)
    }
}