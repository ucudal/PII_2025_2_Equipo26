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
        private readonly FachadaUnit _fachadaUnit;

        public RegisReunionesCommand(FachadaUnit fachada)
        {
            _fachadaUnit = fachada;
        }

        [Command("registrar_reunion")]
        [Summary("Registra la reunion con el cliente seleccionado.")]

        public async Task ExecuteAsync(
            [Summary("Id del cliente con el que se tuvo la reunion")]
            int idCliente,
            [Summary("Fecha en la que se dio la reunion")]
            DateTime fecha,
            [Summary("Tema del que se trato la reunion")]
            string tema,
            [Summary("Lugar donde se tuvo la reunion")]
            string lugar)
        {
            try
            {
                // Delegamos la lógica a la Fachada
                // La fachada se encargará de buscar al cliente y crear el objeto 'Reunion'
                _fachadaUnit.RegistrarReunion(idCliente, fecha, tema, lugar);

                // Enviamos confirmación visual al usuario (Feedback)
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