using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir con la historia de usuario:
    /// "Como usuario quiero saber el total de ventas de un periodo dado, para analizar en rendimiento de mi negocio."
    /// </summary>
    public class TotalVentasCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public TotalVentasCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        // 1. Ayuda
        [Command("total_ventas")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "**Formato:** `!total_ventas <Fecha Inicio> <Fecha Fin>`\n\n" +
                             "**Formato de Fecha:** DD/MM/AAAA\n\n" +
                             "**Ejemplo:**\n" +
                             "`!total_ventas 01/01/2025 31/03/2025` (Ventas del primer trimestre)");
        }

        // 2. Ejecución
        [Command("total_ventas")]
        [Summary("Calcula el total de ventas para un periodo dado.")]
        public async Task ExecuteAsync(
            [Summary("Fecha de inicio (DD/MM/AAAA)")] string fechaInicioTexto,
            [Summary("Fecha de fin (DD/MM/AAAA)")] string fechaFinTexto)
        {
            // Declaración de variables externas para TryParse 
            DateTime fechaInicio;
            DateTime fechaFin;

            try
            {
                // Validación 1: Parseo de Fecha Inicio
                if (!DateTime.TryParse(fechaInicioTexto, out fechaInicio))
                {
                    await ReplyAsync("⚠️ **Error**: La fecha de inicio es inválida. Use el formato DD/MM/AAAA.");
                    return;
                }

                // Validación 2: Parseo de Fecha Fin
                if (!DateTime.TryParse(fechaFinTexto, out fechaFin))
                {
                    await ReplyAsync("⚠️ **Error**: La fecha de fin es inválida. Use el formato DD/MM/AAAA.");
                    return;
                }
                
                // Validación 3: Lógica de negocio - la fecha de inicio no puede ser posterior a la de fin.
                if (fechaInicio > fechaFin)
                {
                    await ReplyAsync("⚠️ **Error**: La fecha de inicio no puede ser posterior a la fecha de fin.");
                    return;
                }

                // Envío del Mensaje: Delegación a la Fachada
                float total = this._fachada.CalcularTotalVentas(fechaInicio, fechaFin);

                // Respuesta 
                await ReplyAsync(String.Format("📈 **Reporte de Ventas**\n" +
                                               "- **Período**: {0} al {1}\n" +
                                               "- **Total Ventas**: ${2:N2}", 
                                               fechaInicio.ToString("dd/MM/yyyy"),
                                               fechaFin.ToString("dd/MM/yyyy"),
                                               total));
            }
            catch (Exception ex)
            {
                await ReplyAsync("❌ **Error Desconocido**: " + ex.Message);
            }
        }
    }
}