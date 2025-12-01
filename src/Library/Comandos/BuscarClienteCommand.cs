using Discord.Commands;
using Library;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir la historia de usuario: 
    /// "Como usuario quiero buscar clientes por nombre, apellido, teléfono o correo..."
    /// </summary>
    public class BuscarClienteCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public BuscarClienteCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        [Command("buscar_cliente")]
        [Summary("Busca clientes por nombre, apellido, teléfono o correo.")]
        public async Task ExecuteAsync(
            [Summary("Término de búsqueda (use comillas si hay espacios)")] string termino)
        {
            try
            {
                var resultados = _fachada.BuscarClientes(termino);

                if (resultados.Count == 0)
                {
                    await ReplyAsync($"🔍 No se encontraron clientes que coincidan con: **{termino}**");
                    return;
                }
                
                StringBuilder respuesta = new StringBuilder();
                respuesta.AppendLine($"✅ **Resultados encontrados ({resultados.Count}):**");
                respuesta.AppendLine("------------------------------------------------");

                foreach (var cliente in resultados)
                {
                    respuesta.AppendLine($"🆔 **ID:** {cliente.Id}");
                    respuesta.AppendLine($"👤 **Nombre:** {cliente.Nombre} {cliente.Apellido}");
                    respuesta.AppendLine($"📞 **Tel:** {cliente.Telefono} | 📧 **Email:** {cliente.Correo}");
                    respuesta.AppendLine("------------------------------------------------");
                }

                await ReplyAsync(respuesta.ToString());
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ **Error**: Ocurrió un problema al buscar. {ex.Message}");
            }
        }
    }
}