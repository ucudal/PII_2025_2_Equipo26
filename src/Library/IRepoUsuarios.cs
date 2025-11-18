using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Define el contrato para el Repositorio de Usuarios.
    /// Hereda la funcionalidad base de IRepositorio.
    /// </summary>
    public interface IRepoUsuarios : IRepositorio<Usuario>
    {
        /// <summary>
        /// Crea y agrega un nuevo usuario.
        /// </summary>
        /// <param name="nombreUsuario">El nombre de login.</param>
        /// <param name="rol">El rol del usuario.</param>
        /// <returns>El usuario creado.</returns>
        // --- PARÁMETRO 'contrasena' ELIMINADO ---
        Usuario Agregar(string nombreUsuario, Rol rol);

        /// <summary>
        /// Suspende a un usuario.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        void Suspender(int id);

        /// <summary>
        /// Activa a un usuario.
        /// </summary>
        /// <param name="id">ID del usuario.</param>
        void Activar(int id);
        void AgregarRol(int idUsuario, Rol rol);
    }
}