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
        private readonly FachadaUnit _fachadaUnit;

        public RegisLlamadasCommand(FachadaUnit fachada)
        {
            _fachadaUnit = fachada;
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
            // Validación de campos de texto obligatorios (Tema y Tipo)
            if (string.IsNullOrWhiteSpace(tema))
            {
                await ReplyAsync("❌ **Error de Entrada**: El argumento **tema** no puede estar vacío. Por favor, especifique el asunto de la llamada.");
                return; // Detiene la ejecución del comando.
            }

            if (string.IsNullOrWhiteSpace(tipollamada))
            {
                await ReplyAsync("❌ **Error de Entrada**: El argumento **tipollamada** no puede estar vacío. Use 'entrante' o 'saliente'.");
                return; // Detiene la ejecución del comando.
            }
            
            // Validación de ID (asumiendo que debe ser positivo)
            if (idCliente <= 0)
            {
                await ReplyAsync("❌ **Error de Cliente**: El ID del cliente debe ser un número positivo válido.");
                return; // Detiene la ejecución del comando.
            }
            
            // Validación de Fecha (comprobación de valor por defecto)
            if (fecha == default(DateTime))
            {
                await ReplyAsync("❌ **Error de Fecha**: La fecha ingresada es inválida o tiene un formato incorrecto. Ejemplo: AAAA-MM-DD.");
                return; // Detiene la ejecución del comando.
            }
            
            try
            {
                // Delegación: El comando, tras pasar las validaciones de entrada,
                // delega la responsabilidad a la Fachada.
                _fachadaUnit.RegistrarLlamada(idCliente, fecha, tema, tipollamada);

                // Feedback al usuario (Interacción UI)
                await ReplyAsync($"📞 **Llamada Registrada**\n" +
                                 $"- **Cliente ID**: {idCliente}\n" +
                                 $"- **Tema**: {tema}\n" +
                                 $"- **Tipo**: {tipollamada}\n" +
                                 $"- **Fecha**: {fecha.ToShortDateString()} {fecha.ToShortTimeString()}");
            }
            catch (ArgumentException ex)
            {
                // Captura excepciones lanzadas por la Fachada (Reglas de Negocio)
                // Ej: El ID de cliente no existe, o el tipollamada no es "entrante" ni "saliente"
                await ReplyAsync($"⚠️ **Error de Negocio/Argumento**: La operación falló. Detalles: **{ex.Message}**");
            }
            catch (Exception ex)
            {
                // Manejo de excepciones genéricas para fallos inesperados del sistema
                await ReplyAsync($"❌ **Error Inesperado del Sistema**: Fallo interno al registrar la llamada. Detalles: {ex.Message}");
            }
        }
    }
}