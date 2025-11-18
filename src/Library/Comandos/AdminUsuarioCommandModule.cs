using Discord.Commands;
using System;
using System.Threading.Tasks;
using Library; 

namespace Ucu.Poo.DiscordBot.Commands 
{
    /// <summary>
    /// Módulo de comandos para la administración de usuarios (Administradores).
    /// Este módulo demuestra:
    /// 1. Inyección de Dependencias (recibe la Fachada).
    /// 2. Parseo de argumentos (automático para 'int', manual para 'Enum').
    /// 3. Delegación de lógica de negocio a la Fachada.
    /// </summary>
    [Group("admin")]
    public class AdminUsuarioCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        /// <summary>
        /// El constructor recibe la instancia de la Fachada (DIP).
        /// Esta instancia es gestionada por el ServiceProvider del bot.
        /// </summary>
        public AdminUsuarioCommandModule(Fachada fachada)
        {
            this._fachada = fachada;
        }
        /// <summary>
        /// Comando para crear un nuevo usuario.
        /// Uso: !admin crear_usuario <email> <rol>
        /// Ej: !admin crear_usuario admin@mail.com Administrador
        /// </summary>
        [Command("crear_usuario")]
        [Summary("Crea un nuevo usuario (Vendedor o Administrador).")]
        public async Task CrearUsuarioAsync(string nombreUsuario, string rolComoString)
        {
            Rol rolEnum;
            if (!Enum.TryParse(rolComoString, true, out rolEnum))
            {
                // El tipo de Rol no era válido
                await ReplyAsync($"Error: Rol '{rolComoString}' no reconocido. Use 'Vendedor' o 'Administrador'.");
                return;
            }

            try
            {
  // El comando DELEGA la lógica a la Fachada.
                this._fachada.CrearUsuario(nombreUsuario, rolEnum);
                
                await ReplyAsync($"✅ Usuario '{nombreUsuario}' creado exitosamente con el rol '{rolEnum}'.");
            }
            catch (Exception e)
            {
                // Manejo de errores
                await ReplyAsync($"❌ Error al crear el usuario: {e.Message}");
            }
        }

        /// <summary>
        /// Comando para suspender un usuario por su ID.
        /// Uso: !admin suspender_usuario <id>
        /// Ej: !admin suspender_usuario 123
        /// </summary>
        [Command("suspender_usuario")]
        [Summary("Suspende un usuario existente por su ID.")]
        public async Task SuspenderUsuarioAsync(int idUsuario)
        {

            try
            {
                this._fachada.SuspenderUsuario(idUsuario);
                await ReplyAsync($"✅ Usuario con ID '{idUsuario}' ha sido suspendido.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"❌ Error al suspender: {e.Message} (¿Quizás el ID no existe?)");
            }
        }
        
        /// <summary>
        /// Comando para activar un usuario por su ID.
        /// Uso: !admin activar_usuario <id>
        /// Ej: !admin activar_usuario 123
        /// </summary>
        [Command("activar_usuario")]
        [Summary("Activa un usuario suspendido por su ID.")]
        public async Task ActivarUsuarioAsync(int idUsuario)
        {
            try
            {
                this._fachada.ActivarUsuario(idUsuario);
                await ReplyAsync($"✅ Usuario con ID '{idUsuario}' ha sido activado.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"❌ Error al activar: {e.Message} (¿Quizás el ID no existe?)");
            }
        }

        /// <summary>
        /// Comando para eliminar un usuario por su ID.
        /// Uso: !admin eliminar_usuario <id>
        /// Ej: !admin eliminar_usuario 123
        /// </summary>
        [Command("eliminar_usuario")]
        [Summary("Elimina permanentemente un usuario por su ID.")]
        public async Task EliminarUsuarioAsync(int idUsuario)
        {
            try
            {
                this._fachada.EliminarUsuario(idUsuario);
                await ReplyAsync($"✅ Usuario con ID '{idUsuario}' ha sido eliminado.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"❌ Error al eliminar: {e.Message} (¿Quizás el ID no existe?)");
            }
        }
    }
}