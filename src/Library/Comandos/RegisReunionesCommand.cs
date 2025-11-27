using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando creado para cumplir con historia de usuario:
    /// "Como usuario quiero registrar reuniones con los clientes..."
    /// </summary>
    public class RegisReunionesCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public RegisReunionesCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        [Command("registrar-reunion")]
        [Summary("Registra la reunion con el cliente seleccionado.")]

        public async Task ExecuteAsync(
            [Summary("Id del cliente con el que se tuvo la reunion")]
            int idCliente,
            [Summary("Tema del que se trato la reunion")]
            string tema,
            [Summary("Lugar donde se tuvo la reunion")]
            string lugar)
        {
            try
            {
                // 1. Definimos la fecha actual automáticamente
                DateTime fecha = DateTime.Now;

                // 2. Delegamos la lógica a la Fachada
                // La fachada se encargará de buscar al cliente y crear el objeto 'Reunion'
                _fachada.RegistrarReunion(idCliente, fecha, tema, lugar);

                // 3. Enviamos confirmación visual al usuario (Feedback)
                await ReplyAsync($"📅 **Reunión Registrada Exitosamente**\n" +
                                 $"- **Cliente ID**: {idCliente}\n" +
                                 $"- **Tema**: {tema}\n" +
                                 $"- **Lugar**: {lugar}\n" +
                                 $"- **Fecha**: {fecha.ToShortDateString()}");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones (ej. si el cliente no existe)
                await ReplyAsync($"❌ **Ocurrió un error**: {ex.Message}");
            }
        }
    }
}