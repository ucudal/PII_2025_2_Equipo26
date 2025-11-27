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

        [Command("registrar-mensaje")]
        [Summary("Registra el mensaje al/del cliente seleccionado.")]

        public async Task ExecuteAsync(
            [Summary("Id del cliente para el que es el mensaje")]
            int idCliente,
            [Summary("Tema del que se trato el mensaje")]
            string tema,
            [Summary("Remitente del mensaje")]
            string remitente,
            [Summary("Destinatario del mensaje")]
            string destinatario)
        {
            try
            {
                // 1. Asignamos la fecha y hora actual al mensaje
                DateTime fecha = DateTime.Now;

                // 2. Delegamos la creación del mensaje a la Fachada.
                // Esto mantiene el principio de "Experto en Información": la fachada sabe cómo
                // buscar al cliente y cómo crear la interacción interna.
                _fachada.RegistrarMensaje(idCliente, fecha, tema, remitente, destinatario);

                // 3. Confirmación al usuario
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