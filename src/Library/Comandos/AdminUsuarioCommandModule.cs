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
            if (Context.User.Id == Context.Guild.OwnerId)
            {
                return true;
            }

            string nombreUsuarioDiscord = Context.User.Username;
            var listaUsuarios = this._fachada.VerTodosLosUsuarios();
            
            foreach (Usuario u in listaUsuarios)
            {
                if (u.NombreUsuario == nombreUsuarioDiscord)
                {
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
        [Summary("Registra un usuario. Ej: !admin crear_usuario @Facu Vendedor")]
        public async Task CrearUsuarioAsync(string mencion, string rolTexto1, string rolTexto2 = null)
        {
            if (!TienePermiso())
            {
                await ReplyAsync("⛔ **Acceso Denegado:** Solo el Dueño o Admins registrados pueden hacer esto.");
                return;
            }

            try
            {
                // --- PASO 1: OBTENER USUARIO DESDE LA MENCIÓN ---
                ulong idUsuarioDiscord;
                if (!MentionUtils.TryParseUser(mencion, out idUsuarioDiscord))
                {
                    await ReplyAsync($"❌ **Error:** No se reconoció el usuario '{mencion}'. Asegúrate de mencionarlo correctamente.");
                    return;
                }

                // CORRECCIÓN CS1061: Casteamos Context.Guild a (IGuild) para acceder a GetUserAsync
                var usuarioDiscord = await ((IGuild)Context.Guild).GetUserAsync(idUsuarioDiscord, CacheMode.AllowDownload);
                
                if (usuarioDiscord == null)
                {
                    await ReplyAsync("❌ **Error:** No se pudo encontrar al usuario en el servidor (quizás no está en caché).");
                    return;
                }

                // --- PASO 2: PARSEAR ROLES ---
                Rol rol1 = (Rol)Enum.Parse(typeof(Rol), rolTexto1, true);

                Rol rol2 = default(Rol);
                bool tieneSegundoRol = false;

                if (!string.IsNullOrEmpty(rolTexto2))
                {
                    tieneSegundoRol = true;
                    rol2 = (Rol)Enum.Parse(typeof(Rol), rolTexto2, true);
                }

                // --- PASO 3: GUARDAR EN EL SISTEMA ---
                string nombreParaGuardar = usuarioDiscord.Username;
                
                // Verificar si existe y limpiar
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

                // Crear base
                this._fachada.CrearUsuario(nombreParaGuardar, rol1);
                
                // Agregar segundo rol
                if (tieneSegundoRol)
                {
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
                        this._fachada.AgregarRolUsuario(nuevoUsuario.Id, rol2);
                    }
                }

                // Respuesta final
                string msgRoles = "";
                if (tieneSegundoRol)
                {
                    msgRoles = rol1 + " y " + rol2;
                }
                else
                {
                    msgRoles = rol1.ToString();
                }

                await ReplyAsync($"✅ **Éxito:** Usuario **{nombreParaGuardar}** creado con roles: **{msgRoles}**.");
            }
            catch (ArgumentException)
            {
                await ReplyAsync($"❌ **Error:** Rol no válido. Usa 'Administrador' o 'Vendedor'.");
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
                foreach (Rol r in usuario.Roles)
                {
                    if (r == Rol.Administrador) { iconos += "🛡️ "; }
                    if (r == Rol.Vendedor) { iconos += "💼 "; }
                }

                string estado = "";
                if (usuario.Estado == Estado.Suspendido) { estado = " (SUSPENDIDO)"; }

                builder.AppendLine($"`ID {usuario.Id}` | {iconos} **{usuario.NombreUsuario}** {estado}");
            }

            await ReplyAsync(builder.ToString());
        }

        // --- COMANDOS DE MANTENIMIENTO ---
        
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