using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Library;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Ucu.Poo.DiscordBot.Commands
{
    [Group("admin")]
    public class AdminUsuarioCommandModule : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public AdminUsuarioCommandModule(Fachada fachada)
        {
            this._fachada = fachada;
        }

        private bool TienePermiso()
        {
            // 1. Dueño del servidor
            if (Context.User.Id == Context.Guild.OwnerId)
            {
                return true;
            }

            // 2. Buscamos en la lista interna (Soporte para lista de Roles)
            string nombreUsuarioDiscord = Context.User.Username;
            var listaUsuarios = this._fachada.VerTodosLosUsuarios();
            
            foreach (Usuario u in listaUsuarios)
            {
                if (u.NombreUsuario == nombreUsuarioDiscord)
                {
                    // Recorremos la lista de roles del usuario
                    foreach (Rol r in u.Roles)
                    {
                        if (r == Rol.Administrador)
                        {
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        [Command("crear_usuario")]
        [Summary("Registra un usuario detectando sus roles de Discord automáticamente.")]
        // NOTA: Ya no pedimos el string 'rolComoString', lo detectamos solo.
        public async Task CrearUsuarioAsync(SocketGuildUser usuarioDiscord)
        {
            if (!TienePermiso())
            {
                await ReplyAsync("⛔ **Acceso Denegado:** Solo el Dueño o Admins registrados pueden hacer esto.");
                return;
            }

            // --- 1. DETECTAR ROLES DE DISCORD ---
            List<Rol> rolesDetectados = new List<Rol>();

            foreach (var rolDiscord in usuarioDiscord.Roles)
            {
                // Asegúrate que los roles en Discord se llamen exactamente así
                if (rolDiscord.Name == "Administrador") rolesDetectados.Add(Rol.Administrador);
                if (rolDiscord.Name == "Vendedor") rolesDetectados.Add(Rol.Vendedor);
            }

            if (rolesDetectados.Count == 0)
            {
                await ReplyAsync($"❌ El usuario {usuarioDiscord.Mention} no tiene roles válidos en Discord (Administrador/Vendedor).");
                return;
            }

            try
            {
                string nombreParaGuardar = usuarioDiscord.Username;
                
                // --- 2. VERIFICAR SI YA EXISTE Y BORRAR ---
                var listaUsuarios = this._fachada.VerTodosLosUsuarios();
                Usuario usuarioExistente = null;

                foreach (Usuario u in listaUsuarios)
                {
                    if (u.NombreUsuario == nombreParaGuardar)
                    {
                        usuarioExistente = u;
                        break; 
                    }
                }
                
                if (usuarioExistente != null)
                {
                    this._fachada.EliminarUsuario(usuarioExistente.Id);
                }

                // --- 3. CREAR CON EL PRIMER ROL ---
                // Usamos el primer rol detectado para crear la base del usuario
                this._fachada.CrearUsuario(nombreParaGuardar, rolesDetectados[0]);
                
                // --- 4. AGREGAR ROLES ADICIONALES (Si tiene más de uno) ---
                if (rolesDetectados.Count > 1)
                {
                    // Buscamos el usuario que acabamos de crear para obtener su ID
                    var usuariosActualizados = this._fachada.VerTodosLosUsuarios();
                    Usuario nuevoUsuario = null;
                    
                    foreach(Usuario u in usuariosActualizados)
                    {
                        if (u.NombreUsuario == nombreParaGuardar)
                        {
                            nuevoUsuario = u;
                            break;
                        }
                    }

                    if (nuevoUsuario != null)
                    {
                        // Agregamos el resto de roles (empezando desde el índice 1)
                        for (int i = 1; i < rolesDetectados.Count; i++)
                        {
                            this._fachada.AgregarRolUsuario(nuevoUsuario.Id, rolesDetectados[i]);
                        }
                    }
                }

                // Feedback al usuario
                string rolesTexto = string.Join(", ", rolesDetectados);
                await ReplyAsync($"✅ **Éxito:** Usuario **{nombreParaGuardar}** vinculado con roles: **{rolesTexto}**.");
            }
            catch (Exception e)
            {
                await ReplyAsync($"❌ Error inesperado: {e.Message}");
            }
        }

        [Command("listar_usuarios")]
        public async Task ListarUsuariosAsync()
        {
            if (!TienePermiso())
            {
                await ReplyAsync("⛔ Acceso denegado.");
                return;
            }

            var usuarios = this._fachada.VerTodosLosUsuarios();

            if (usuarios.Count == 0)
            {
                await ReplyAsync("📭 No hay usuarios registrados.");
                return;
            }

            var builder = new StringBuilder();
            builder.AppendLine("**📋 Usuarios del Sistema:**");
            
            foreach (var usuario in usuarios)
            {
                string iconos = "";
                // Recorremos los roles para poner los íconos correctos
                foreach (Rol r in usuario.Roles)
                {
                    if (r == Rol.Administrador) iconos += "🛡️ ";
                    if (r == Rol.Vendedor) iconos += "💼 ";
                }

                string estado = "";
                if (usuario.Estado == Estado.Suspendido) estado = " (SUSPENDIDO)";

                builder.AppendLine($"`ID {usuario.Id}` | {iconos} **{usuario.NombreUsuario}** {estado}");
            }

            await ReplyAsync(builder.ToString());
        }

        // --- COMANDOS DE MANTENIMIENTO (SIN CAMBIOS) ---
        
        [Command("suspender_usuario")]
        public async Task SuspenderUsuarioAsync(int idUsuario)
        {
             if (!TienePermiso()) { await ReplyAsync("⛔ Sin permiso."); return; }
             try {
                this._fachada.SuspenderUsuario(idUsuario);
                await ReplyAsync($"✅ Usuario {idUsuario} suspendido.");
             } catch(Exception e) { await ReplyAsync(e.Message); }
        }

        [Command("activar_usuario")]
        public async Task ActivarUsuarioAsync(int idUsuario)
        {
             if (!TienePermiso()) { await ReplyAsync("⛔ Sin permiso."); return; }
             try {
                this._fachada.ActivarUsuario(idUsuario);
                await ReplyAsync($"✅ Usuario {idUsuario} activado.");
             } catch(Exception e) { await ReplyAsync(e.Message); }
        }

        [Command("eliminar_usuario")]
        public async Task EliminarUsuarioAsync(int idUsuario)
        {
             if (!TienePermiso()) { await ReplyAsync("⛔ Sin permiso."); return; }
             try {
                this._fachada.EliminarUsuario(idUsuario);
                await ReplyAsync($"🗑️ Usuario {idUsuario} eliminado.");
             } catch(Exception e) { await ReplyAsync(e.Message); }
        }
    }
}