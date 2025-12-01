using Discord.Commands;
using Library;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir la historia de usuario: 
    /// "Como usuario quiero ver una lista de todos mis clientes..."
    /// </summary>
    public class VerClientesCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public VerClientesCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        [Command("ver_clientes")]
        [Alias("listar_clientes")]
        [Summary("Muestra una lista de todos los clientes registrados.")]
        public async Task ExecuteAsync()
        {
            try
            {
                var clientes = _fachada.VerTodosLosClientes();

                if (clientes.Count == 0)
                {
                    await ReplyAsync("📭 **La lista está vacía.** No hay clientes registrados en el sistema.");
                    return;
                }
                
                StringBuilder mensaje = new StringBuilder();
                mensaje.AppendLine("📋 **Cartera de Clientes:**");
                mensaje.AppendLine("```"); 
                mensaje.AppendLine($"{"ID".PadRight(4)} | {"Nombre Completo".PadRight(25)} | {"Teléfono".PadRight(15)}");
                mensaje.AppendLine(new string('-', 50)); // Línea separadora

                foreach (var cliente in clientes)
                {
                    string nombreCompleto = $"{cliente.Nombre} {cliente.Apellido}";
                    
                    if (nombreCompleto.Length > 22) nombreCompleto = nombreCompleto.Substring(0, 22) + "...";

                    mensaje.AppendLine($"{cliente.Id.ToString().PadRight(4)} | {nombreCompleto.PadRight(25)} | {cliente.Telefono.PadRight(15)}");
                }

                mensaje.AppendLine("```"); 
                mensaje.AppendLine($"*Total: {clientes.Count} clientes registrados.*");
                if (mensaje.Length > 2000)
                {
                    await ReplyAsync("⚠️ **Alerta**: Hay demasiados clientes para mostrar en un solo mensaje. Mostrando los primeros de la lista...");
                }

                await ReplyAsync(mensaje.ToString());
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ **Error**: No se pudo obtener la lista. {ex.Message}");
            }
        }
    }
}