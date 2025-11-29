using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando creado para cumplir con historia de usuario:
    /// "Como usuario quiero registrar llamadas enviadas o recibidas de clientes..."
    /// </summary>
    public class RegisLlamadasCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public RegisLlamadasCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        [Command("registrar_llamada")]
        [Summary("Registra la llamada con el cliente seleccionado.")]

        public async Task ExecuteAsync(
            [Summary("Id del cliente con el que se tuvo la llamada")]
            int idCliente,
            [Summary("Fecha en la que se dio la llamada")]
            DateTime fecha,
            [Summary("Tema del que se trato la llamada")]
            string tema,
            [Summary("Tipo de la llamada (entrante o saliente)")]
            string tipollamada)
        {
            try
            {
                // Delegación: El comando no contiene lógica de negocio, solo orquesta la petición a la Fachada.
                _fachada.RegistrarLlamada(idCliente, fecha, tema, tipollamada);

                // Feedback al usuario (Interacción UI)
                await ReplyAsync($"📞 **Llamada Registrada**\n" +
                                 $"- **Cliente ID**: {idCliente}\n" +
                                 $"- **Tema**: {tema}\n" +
                                 $"- **Tipo**: {tipollamada}\n" +
                                 $"- **Fecha**: {fecha}");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones para evitar que el bot colapse y dar feedback del error.
                await ReplyAsync($"❌ **Error al registrar la llamada**: {ex.Message}");
            }
        }
    }
}