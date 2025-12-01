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
    public class RegisMensajesCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public RegisMensajesCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        [Command("registrar_mensaje")]
        [Summary("Registra el mensaje al/del cliente seleccionado.")]

        public async Task ExecuteAsync(
            [Summary("Id del cliente para el que es el mensaje")]
            int idCliente,
            [Summary("Fecha en la que se mando/recibio el mensaje")]
            DateTime fecha,
            [Summary("Tema del que se trato el mensaje")]
            string tema,
            [Summary("Remitente del mensaje")]
            string remitente,
            [Summary("Destinatario del mensaje")]
            string destinatario)
        {
            try
            {
                // Delegamos la creación del mensaje a la Fachada.
                // Esto mantiene el principio de "Experto en Información": la fachada sabe cómo
                // buscar al cliente y cómo crear la interacción interna.
                _fachada.RegistrarMensaje(idCliente, fecha, tema, remitente, destinatario);

                // Confirmación al usuario
                await ReplyAsync($"📨 **Mensaje Registrado**\n" +
                                 $"- **Cliente ID**: {idCliente}\n" +
                                 $"- **De**: {remitente}\n" +
                                 $"- **Para**: {destinatario}\n" +
                                 $"- **Tema**: {tema}\n" +
                                 $"- **Fecha**: {fecha.ToShortDateString()}");
            }
            catch (Exception ex)
            {
                // Manejo de errores (ej. ID de cliente no encontrado)
                await ReplyAsync($"❌ **Error**: {ex.Message}");
            }
        }
    }
}