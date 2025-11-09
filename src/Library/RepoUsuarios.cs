using System.Collections.Generic;
using System.Linq;

namespace Library
{
    /// <summary>
    /// Administra la colección de objetos <see cref="Usuario"/>.
    /// Hereda la lógica común de <see cref="RepositorioBase{T}"/>.
    /// </summary>
    public class RepoUsuarios : RepositorioBase<Usuario>, IRepoUsuarios
    {
        // (Hereda _items y _nextId de RepositorioBase)
        // (Hereda Buscar, Eliminar y ObtenerTodos de RepositorioBase)

        /// <summary>
        /// Agrega un nuevo usuario a la lista (Create).
        /// </summary>
        /// <param name="nombreUsuario">Nombre de login.</param>
        /// <param name="rol">Rol del usuario.</param>
        /// <returns>El usuario recién creado.</returns>
        // --- PARÁMETRO 'contrasena' ELIMINADO ---
        public Usuario Agregar(string nombreUsuario, Rol rol)
        {
            // --- LLAMADA AL CONSTRUCTOR ACTUALIZADA ---
            var nuevoUsuario = new Usuario(this._nextId++, nombreUsuario, rol);
            this._items.Add(nuevoUsuario); // Usa _items heredado
            return nuevoUsuario;
        }

        /// <summary>
        /// Cambia el estado de un usuario a 'Suspendido'.
        /// </summary>
        /// <param name="id">El ID del usuario a suspender.</param>
        public void Suspender(int id)
        {
            var usuario = this.Buscar(id); // Usa Buscar() heredado
            if (usuario != null)
            {
                usuario.Suspender(); 
            }
        }
        
        /// <summary>
        /// Cambia el estado de un usuario a 'Activo'.
        /// </summary>
        /// <param name="id">El ID del usuario a activar.</param>
        public void Activar(int id)
        {
            var usuario = this.Buscar(id); // Usa Buscar() heredado
            if (usuario != null)
            {
                usuario.Activar();
            }
        }
    }
}