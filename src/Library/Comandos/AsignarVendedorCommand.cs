using Discord.Commands;
using Discord.WebSocket; 
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando creado para cumplir la historia de usuario:
    ///  "Como vendedor, quiero poder asignar un cliente a otro vendedor para distribuir el trabajo en el equipo."
    /// </summary>
   public class AsignarVendedorCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public AsignarVendedorCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        // Método auxiliar para verificar permiso
        private bool TienePermisoVendedor()
        {
            // 1. Obtenemos el nombre del usuario de Discord que ejecuta el comando
            string nombreUsuarioDiscord = Context.User.Username;
            
            // 2. Traemos todos los usuarios del CRM
            var listaUsuarios = this._fachada.VerTodosLosUsuarios();
            
            // 3. Buscamos si existe y si tiene rol Vendedor
            foreach (Usuario u in listaUsuarios)
            {
                if (u.NombreUsuario == nombreUsuarioDiscord)
                {
                    foreach (Rol r in u.Roles)
                    {
                        if (r == Rol.Vendedor)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        [Command("asignar_vendedor")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "**Formato:** `!asignar_vendedor <ID Cliente> <@Vendedor>`\n\n" +
                             "**Ejemplo:**\n" +
                             "`!asignar_vendedor 5 @JuanPerez`");
        }

        [Command("asignar_vendedor")]
        [Summary("Asigna un cliente a otro vendedor usando su mención (@).")]
        public async Task ExecuteAsync(
            [Summary("ID del Cliente")] int idCliente,
            [Summary("Usuario de Discord (@Mención)")] SocketGuildUser vendedorDiscord)
        {
            try
            {
                // --- 1. VALIDAR PERMISOS (Ejecutor) ---
                // Usamos la misma lógica que tu Admin (comparar nombre de usuario)
                if (!TienePermisoVendedor())
                {
                    await ReplyAsync("⛔ **Acceso Denegado:** Tu usuario de Discord no coincide con ningún usuario 'Vendedor' registrado en el sistema.");
                    return;
                }

                // --- 2. BUSCAR AL VENDEDOR DESTINO (@Mención) EN EL CRM ---
                string nombreVendedorDestino = vendedorDiscord.Username;
                
                var listaUsuarios = this._fachada.VerTodosLosUsuarios();
                Usuario vendedorCrm = null;

                // Buscamos manualmente por nombre
                foreach (Usuario u in listaUsuarios)
                {
                    if (u.NombreUsuario == nombreVendedorDestino)
                    {
                        vendedorCrm = u;
                        break;
                    }
                }

                if (vendedorCrm == null)
                {
                    await ReplyAsync($"❌ **Error:** El usuario mencionado (**{nombreVendedorDestino}**) no está registrado en el sistema CRM.");
                    return;
                }

                // --- 3. EJECUTAR LÓGICA DE NEGOCIO ---
                // Usamos el ID interno del usuario que encontramos
                _fachada.AsignarClienteVendedor(idCliente, vendedorCrm.Id);

                await ReplyAsync($"✅ **Asignación Exitosa:** El Cliente {idCliente} ahora está asignado a **{vendedorCrm.NombreUsuario}**.");
            }
            catch (Exception ex)
            {
                await ReplyAsync("❌ **Error:** " + ex.Message);
            }
        }
    }
}