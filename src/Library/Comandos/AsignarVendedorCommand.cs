using Discord.Commands;
using Discord.WebSocket; // Necesario para acceder a Context.User como SocketGuildUser
using Library;
using System;
using System.Collections.Generic;
using System.Linq; // Necesario para usar .Any() en la colección de roles de Discord
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para asignar un cliente a un vendedor diferente.
    /// Valida que el usuario que ejecuta el comando tenga el rol de Discord "Vendedor".
    /// </summary>
    public class AsignarVendedorCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;
        private const string ROL_REQUERIDO = "Vendedor"; // Mapeo del rol de Discord

        public AsignarVendedorCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        // 1. Ayuda
        [Command("asignar_vendedor")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "**Formato:** `!asignar_vendedor <ID Cliente> <ID Vendedor>`\n\n" +
                             "**Ejemplo:**\n" +
                             "`!asignar_vendedor 5 2` (Asigna Cliente 5 a Vendedor 2)");
        }

        // 2. Ejecución
        [Command("asignar_vendedor")]
        [Summary("Asigna un cliente a otro vendedor. Requiere el rol de Discord 'Vendedor'.")]
        public async Task ExecuteAsync(
            [Summary("ID del Cliente")] int idCliente,
            [Summary("ID del Nuevo Vendedor")] int idNuevoVendedor)
        {
            try
            {
                // --- Validación de Permisos (Rol de Discord) ---
                
                // Obtenemos el usuario como SocketGuildUser para acceder a sus roles.
                var usuarioDiscord = Context.User as SocketGuildUser;
                
                if (usuarioDiscord == null)
                {
                    await ReplyAsync("❌ **Error de Contexto**: Este comando debe ejecutarse dentro de un servidor (guild).");
                    return;
                }

                // C# 6 compatible: Chequeamos si el usuario tiene el rol requerido.
                bool tieneRolVendedor = false;
                
                // NOTE: Para usar .Any() de LINQ aquí, tu proyecto debe tener 'using System.Linq'.
                // Si no puedes usar LINQ, se debe usar un bucle foreach. Usaremos el bucle para máxima compatibilidad C# 6.
                
                foreach (var rol in usuarioDiscord.Roles)
                {
                    // Comparamos el nombre del rol de Discord con el rol requerido.
                    if (rol.Name.Equals(ROL_REQUERIDO, StringComparison.OrdinalIgnoreCase))
                    {
                        tieneRolVendedor = true;
                        break;
                    }
                }
                
                if (!tieneRolVendedor)
                {
                    await ReplyAsync(String.Format("❌ **Acceso Denegado**: Solo los usuarios con el rol de Discord '{0}' pueden asignar clientes.", ROL_REQUERIDO));
                    return;
                }

                // --- Ejecución de la Lógica de Negocio (Delegación) ---
                
                // La Fachada realizará la búsqueda, validación de estado/rol del nuevo vendedor y la asignación.
                this._fachada.AsignarClienteVendedor(idCliente, idNuevoVendedor);

                // Respuesta de éxito
                await ReplyAsync(String.Format("✅ **Asignación Exitosa**: El Cliente ID {0} ha sido reasignado al Vendedor ID {1}.", idCliente, idNuevoVendedor));
            }
            catch (KeyNotFoundException ex)
            {
                // Captura si el Cliente o el Vendedor no existen.
                await ReplyAsync("❌ **Error de Búsqueda**: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Captura si el Nuevo Vendedor no tiene el rol de CRM Vendedor o está Suspendido.
                await ReplyAsync("❌ **Error de Negocio**: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Captura cualquier otro error.
                await ReplyAsync("❌ **Error Desconocido**: Ha ocurrido un error al procesar la solicitud. Detalle: " + ex.Message);
            }
        }
    }
}