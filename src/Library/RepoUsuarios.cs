using Library;
using System;
using System.Collections.Generic;

namespace Library
{
    /// <summary>
    /// Administra la colección de objetos <see cref="Usuario"/>.
    /// Hereda de <see cref="RepositorioBase{T}"/> e implementa
    /// la interfaz específica <see cref="IRepoUsuarios"/>.
    /// </summary>
    public class RepoUsuarios : RepositorioBase<Usuario>, IRepoUsuarios
    {
        /// <summary>
        /// Crea y agrega un nuevo usuario (Operación Create).
        /// </summary>
        
        // --- CORRECCIÓN 1: Cambiar el tipo de retorno de 'void' a 'Usuario' ---
        public Usuario Agregar(string nombreUsuario, Rol rol)
        {

            var nuevoUsuario = new Usuario(nombreUsuario, rol); 
            

            base.Agregar(nuevoUsuario);
            
            return nuevoUsuario;
        }

        /// <summary>
        /// Cambia el estado de un usuario a 'Suspendido'.
        /// </summary>
        public void Suspender(int idUsuario)
        {
            var usuario = this.Buscar(idUsuario);
            if (usuario != null)
            {
                throw new InvalidOperationException($"No se encontró un usuario con ID: {idUsuario}");
            }
            usuario.Suspender();
        }

        /// <summary>
        /// Cambia el estado de un usuario a 'Activo'.
        /// </summary>
        public void Activar(int idUsuario)
        {
            var usuario = this.Buscar(idUsuario);
            if (usuario != null)
            {
                throw new InvalidOperationException($"No se encontró un usuario con ID: {idUsuario}");
            }
            usuario.Activar();
        }
    }
}