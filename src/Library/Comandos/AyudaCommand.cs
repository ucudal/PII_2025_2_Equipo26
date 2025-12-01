using Discord.Commands;
using System.Text;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para mostrar la lista de ayuda automáticamente.
    /// </summary>
    public class AyudaCommand : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        
        public AyudaCommand(CommandService service)
        {
            _service = service;
        }

        [Command("ayuda")]
        [Alias("comandos", "help")]
        [Summary("Muestra esta lista de comandos disponibles.")]
        public async Task ExecuteAsync()
        {
            StringBuilder mensaje = new StringBuilder();
            mensaje.AppendLine("🤖 **Lista de Comandos Disponibles**");
            mensaje.AppendLine("Aquí tienes todo lo que puedo hacer por ti:");
            mensaje.AppendLine("```"); 
            foreach (var module in _service.Modules)
            {
                foreach (var command in module.Commands)
                {
                    string nombre = command.Name;
                    
                    string descripcion = command.Summary;
                    
                    if (descripcion == null)
                    {
                        descripcion = "Sin descripción";
                    }
                    mensaje.AppendLine($"!{nombre.PadRight(20)} - {descripcion}");
                }
            }

            mensaje.AppendLine("```");
            mensaje.AppendLine("💡 *Tip: Escribe los comandos tal cual aparecen aquí.*");

            await ReplyAsync(mensaje.ToString());
        }
    }
}