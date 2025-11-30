using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    public class RegistrarCotizacionCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public RegistrarCotizacionCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        // 1. Ayuda
        [Command("registrar_cotizacion")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "**Formato:** `!registrar_cotizacion <ID> <Monto> <Tema> <Fecha(Opcional)>`\n\n" +
                             "**Ejemplo 1 (Hoy):**\n" +
                             "`!registrar_cotizacion 1 1500 \"Desarrollo Web\"`\n\n" +
                             "**Ejemplo 2 (Fecha manual):**\n" +
                             "`!registrar_cotizacion 1 1500 \"Desarrollo Web\" 25/10/2023`");
        }

        // 2. Ejecución
        [Command("registrar_cotizacion")]
        [Summary("Registra una cotización con fecha personalizada u hoy.")]
        public async Task ExecuteAsync(
            [Summary("ID del Cliente")] int idCliente,
            [Summary("Importe")] double monto,
            [Summary("Tema")] string tema,
            [Summary("Fecha (opcional)")] string fechaTexto = null)
        {
            try
            {
                var cliente = _fachada.BuscarCliente(idCliente);
                if (cliente == null)
                {
                    await ReplyAsync("❌ **Error**: No existe el cliente con ese ID.");
                    return;
                }

                // Lógica de Fecha manual sin lambdas
                DateTime fechaFinal = DateTime.Now;

                // Si el usuario escribió el 4to parámetro
                if (fechaTexto != null)
                {
                    try
                    {
                        fechaFinal = DateTime.Parse(fechaTexto);
                    }
                    catch
                    {
                        await ReplyAsync("⚠️ **Fecha inválida**. Se usará la fecha de hoy.");
                    }
                }

                // Llamamos al método de la Fachada que acepta fecha
                _fachada.RegistrarCotizacion(idCliente, tema, monto, fechaFinal);

                await ReplyAsync("📋 **Cotización Registrada**\n" +
                                 "- **Cliente:** " + cliente.Nombre + " " + cliente.Apellido + "\n" +
                                 "- **Tema:** " + tema + "\n" +
                                 "- **Monto:** $" + monto + "\n" +
                                 "- **Fecha:** " + fechaFinal.ToString("dd/MM/yyyy"));
            }
            catch (Exception ex)
            {
                await ReplyAsync("❌ **Error**: " + ex.Message);
            }
        }
    }
}