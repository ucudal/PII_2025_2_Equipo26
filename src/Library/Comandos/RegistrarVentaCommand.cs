using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    public class RegistrarVentaCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public RegistrarVentaCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        // 1. Ayuda
        [Command("registrar_venta")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "**Formato:** `!registrar_venta <ID> <Producto> <Monto> <Fecha(Opcional)>`\n\n" +
                             "**Ejemplos:**\n" +
                             "`!registrar_venta 1 \"Televisor\" 800` (Fecha de hoy)\n" +
                             "`!registrar_venta 1 \"Televisor\" 800 10/05/2023` (Fecha pasada)");
        }

        // 2. Ejecución
        [Command("registrar_venta")]
        [Summary("Registra una venta con fecha opcional.")]
        public async Task ExecuteAsync(
            [Summary("ID Cliente")] int idCliente, 
            [Summary("Producto")] string producto,
            [Summary("Monto")] float monto,
            [Summary("Fecha (dd/mm/aaaa)")] string fechaTexto = null) // Opcional
        {
            try
            {
                var cliente = _fachada.BuscarCliente(idCliente);
                if (cliente == null)
                {
                    await ReplyAsync($"❌ **Error**: Cliente {idCliente} no encontrado.");
                    return;
                }

                // Lógica de la Fecha
                DateTime fechaVenta = DateTime.Now; // Por defecto hoy

                if (fechaTexto != null)
                {
                    // Intentamos leer la fecha que escribió el usuario
                    try
                    {
                        fechaVenta = DateTime.Parse(fechaTexto);
                    }
                    catch
                    {
                        await ReplyAsync("⚠️ **Fecha inválida**. Usando fecha actual...");
                    }
                }

                // Llamamos a la Fachada actualizada (que ahora acepta fecha)
                _fachada.RegistrarVenta(idCliente, producto, monto, fechaVenta);

                await ReplyAsync($"💰 **Venta Registrada**\n" +
                                 $"👤 **Cliente:** {cliente.Nombre} {cliente.Apellido}\n" +
                                 $"🛍️ **Producto:** {producto}\n" +
                                 $"💵 **Monto:** ${monto}\n" +
                                 $"📅 **Fecha:** {fechaVenta.ToString("dd/MM/yyyy")}");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ **Error**: {ex.Message}");
            }
        }
    }
}