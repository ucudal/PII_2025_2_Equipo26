using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando creado para cumplir con historia de usuario:
    /// "Como usuario quiero registrar mensajes enviados a o recibidos de los clientes..."
    /// </summary>
    public class RegisCorreosCommand : ModuleBase<SocketCommandContext>
    {
        private readonly FachadaUnit _fachadaUnit;

        public RegisCorreosCommand(FachadaUnit fachada)
        {
            _fachadaUnit = fachada;
        }

        [Command("registrar_correo")]
        [Summary("Registra el correo al/del cliente seleccionado.")]

        public async Task ExecuteAsync(
            [Summary("Id del cliente al que se mando/del que se recibio el correo")]
            int idCliente,
            [Summary("Fecha en la que se mando/recibio el correo")]
            DateTime fecha,
            [Summary("Tema del que se trato el correo")]
            string tema,
            [Summary("Remitente del correo")]
            string remitente,
            [Summary("Destinatario del correo")]
            string destinatario,
            [Summary("Asunto del correo")]
            string asunto)
        {
            try
            {
                // Invocamos a la Fachada pasando todos los argumentos necesarios.
                // Notar que el orden de los parámetros debe coincidir exactamente con el método en Fachada.cs
                _fachadaUnit.RegistrarCorreo(idCliente, fecha, tema, remitente, destinatario, asunto);

                // Enviamos el feedback visual al canal de Discord
                await ReplyAsync($"📧 **Correo Registrado Exitosamente**\n" +
                                 $"- **Cliente ID**: {idCliente}\n" +
                                 $"- **De**: {remitente}\n" +
                                 $"- **Para**: {destinatario}\n" +
                                 $"- **Asunto**: {asunto}\n" +
                                 $"- **Tema**: {tema}\n" +
                                 $"- **Fecha**: {fecha}");
            }
            catch (Exception ex)
            {
                // Manejo de errores (Cliente no encontrado, fallo en BD, etc.)
                await ReplyAsync($"❌ **Error al registrar el correo**: {ex.Message}");
            }
        }
    }
}