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
            // La lógica de hash de contraseña se elimina ya que 
            // la contraseña no se maneja en esta versión.

            var nuevoUsuario = new Usuario(nombreUsuario, rol); 
            
            // Llama al método 'Agregar' de la clase base para asignar ID
            base.Agregar(nuevoUsuario);
            
            // --- CORRECCIÓN 2: Retornar el usuario recién creado (como pide la interfaz) ---
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
                usuario.Suspender();
            }
        }

        /// <summary>
        /// Cambia el estado de un usuario a 'Activo'.
        /// </summary>
        public void Activar(int idUsuario)
        {
            var usuario = this.Buscar(idUsuario);
            if (usuario != null)
            {
                usuario.Activar();
            }
        }
    }
}