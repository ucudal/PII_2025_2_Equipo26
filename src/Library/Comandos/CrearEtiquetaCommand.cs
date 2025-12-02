using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir la historia de usuario: 
    /// "Como usuario quiero poder definir etiquetas..."
    /// </summary>
    public class CrearEtiquetaCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public CrearEtiquetaCommand(Fachada fachada)
        {
            _fachada = fachada;
        }
        
        [Command("crear_etiqueta")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Falta el nombre de la etiqueta.**\n" +
                             "Este comando crea una nueva etiqueta para clasificar a tus clientes.\n\n" +
                             "**Formato:** `!crear_etiqueta <Nombre>`\n" +
                             "**Ejemplos:**\n" +
                             "`!crear_etiqueta VIP`\n" +
                             "`!crear_etiqueta Deudor`\n" +
                             "`!crear_etiqueta Potencial Compra`");
        }
        [Command("crear_etiqueta")]
        [Summary("Define una nueva etiqueta en el sistema.")]
        public async Task ExecuteAsync(
            [Summary("Nombre de la etiqueta")] [Remainder] string nombre)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                {
                    await ReplyAsync("❌ El nombre de la etiqueta no puede estar vacío.");
                    return;
                }
                
                _fachada.CrearEtiqueta(nombre);

                await ReplyAsync($"🏷️ **Etiqueta Creada Exitosamente**\n" +
                                 $"Has definido la etiqueta: **{nombre}**\n" +
                                 $"*Ahora puedes asignarla a tus clientes usando `!asignar_etiqueta`*");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ **Error**: No se pudo crear la etiqueta. {ex.Message}");
            }
        }
    }
}