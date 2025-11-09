using System.Collections.Generic;
namespace Library
{

    /// <summary>
    /// Implementa el patrón "Repositorio" (Repository).
    /// Su única responsabilidad (SRP) es administrar la colección en memoria
    /// de objetos <see cref="Usuario"/> (Vendedores y Administradores).
    /// </summary>
    public class RepoUsuarios
    {
        // --- Campos Privados ---

        /// <summary>
        /// La lista interna donde se guardan los objetos Usuario.
        /// </summary>
        private List<Usuario> _usuarios = new List<Usuario>();

        /// <summary>
        /// Un contador para asignar IDs únicos y automáticos.
        /// </summary>
        private int _nextId = 1;

        // --- Métodos Públicos (Operaciones CRUD) ---

        /// <summary>
        /// Agrega un nuevo usuario a la lista (Create).
        /// </summary>
        /// <param name="nombreUsuario">El nombre de login del usuario.</param>
        /// <param name="contrasena">La contraseña del usuario.</param>
        /// <param name="rol">El rol del usuario (Admin o Vendedor).</param>
        /// <returns>El <see cref="Usuario"/> recién creado (con su ID asignado).</returns>
        public Usuario Agregar(string nombreUsuario, string contrasena, RolUsuario rol)
        {
            var nuevoUsuario = new Usuario(_nextId++, nombreUsuario, contrasena, rol);
            _usuarios.Add(nuevoUsuario);
            return nuevoUsuario;
        }

        /// <summary>
        /// Busca un usuario por su ID (Read).
        /// </summary>
        /// <param name="id">El ID del usuario a buscar.</param>
        /// <returns>El <see cref="Usuario"/> encontrado, o <c>null</c> si no existe.</returns>
        public Usuario Buscar(int id)
        {
            foreach (var usuario in _usuarios)
            {
                if (usuario.Id == id)
                {
                    return usuario;
                }
            }

            return null;
        }

        /// <summary>
        /// Cambia el estado de un usuario a 'Suspendido'.
        /// Delega la acción al objeto Usuario (Principio Expert).
        /// </summary>
        /// <param name="id">El ID del usuario a suspender.</param>
        public void Suspender(int id)
        {
            var usuario = Buscar(id);

            if (usuario != null)
            {
                // Delegación de responsabilidad
                usuario.Suspender();
            }
        }

        /// <summary>
        /// Cambia el estado de un usuario a 'Activo'.
        /// Delega la acción al objeto Usuario (Principio Expert).
        /// </summary>
        /// <param name="id">El ID del usuario a activar.</param>
        public void Activar(int id)
        {
            var usuario = Buscar(id);

            if (usuario != null)
            {
                // Delegación de responsabilidad
                usuario.Activar();
            }
        }

        /// <summary>
        /// Elimina un usuario de la lista (Delete).
        /// </summary>
        /// <param name="id">El ID del usuario a eliminar.</param>
        public void Eliminar(int id)
        {
            var usuario = Buscar(id);

            if (usuario != null)
            {
                _usuarios.Remove(usuario);
            }
        }

        /// <summary>
        /// Devuelve la lista completa de todos los usuarios (Read All).
        /// </summary>
        /// <returns>Una <see cref="List{T}"/> de <see cref="Usuario"/>.</returns>
        public List<Usuario> ObtenerTodos()
        {
            return _usuarios;
        }
    }
}