using Discord.Commands;
using Library;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para filtrar y mostrar clientes basándose en el monto total de sus ventas.
    /// Permite al usuario buscar quiénes han comprado más o menos de cierta cantidad.
    /// </summary>
    public class ClientesVentasCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        /// <summary>
        /// Constructor que recibe la fachada mediante inyección de dependencias.
        /// </summary>
        public ClientesVentasCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        /// <summary>
        /// Muestra un mensaje de ayuda si el usuario escribe el comando sin parámetros o de una forma que esta mal.
        /// Se activa con "!clientes_ventas" sin nada mas.
        /// </summary>
        [Command("clientes_ventas")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Uso correcto:** `!clientes_ventas [mayor/menor] [monto]`\nEjemplo: `!clientes_ventas mayor 1000`");
        }

        /// <summary>
        /// Ejecuta la lógica principal del filtro.
        /// Recibe el criterio ("mayor" o "menor") y el monto como texto para procesarlo de forma segura.
        /// </summary>
        /// <param name="criterio">Define si buscamos ventas mayores o menores al monto.</param>
        /// <param name="montoTexto">El monto límite (se recibe como string para manejar puntos y comas).</param>
        [Command("clientes_ventas")]
        [Summary("Filtra clientes por total de ventas.")]
        public async Task ExecuteAsync(string criterio, string montoTexto)
        {
            try
            {
                // 1. Parseo para evitar errores de números con decimales
                float monto;
                if (!float.TryParse(montoTexto.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out monto))
                {
                    await ReplyAsync($"❌ El monto '{montoTexto}' no es un número válido.");
                    return;
                }

                // 2. Definir criterio basándonos en lo que escribió el usuario
                bool buscarMayores = criterio.ToLower().Contains("mayor") || criterio.Contains(">");
                string textoCriterio = buscarMayores ? "mayores" : "menores";

                // 3. Llamar a la Fachada para obtener la lista filtrada
                List<Cliente> clientesFiltrados = _fachada.ObtenerClientesPorMontoVentas(monto, buscarMayores);

                // 4. Verificar si obtuvimos resultados
                if (clientesFiltrados.Count == 0)
                {
                    await ReplyAsync($"📭 No se encontraron clientes con ventas {textoCriterio} a ${monto}.");
                    return;
                }

                // 5. Generar el reporte para Discord
                StringBuilder reporte = new StringBuilder();
                reporte.AppendLine($"📊 **Clientes con ventas {textoCriterio} a ${monto}:**");
                reporte.AppendLine("--------------------------------");

                foreach (var c in clientesFiltrados)
                {
                    // Calculamos el total aca para mostrarlo en el mensaje
                    float total = c.CalcularTotalVentas();
                    
                    reporte.AppendLine($"👤 **{c.Nombre} {c.Apellido}**");
                    reporte.AppendLine($"   🆔 ID: {c.Id} | 💰 Total: **${total}**");
                    reporte.AppendLine("");
                }

                string mensajeFinal = reporte.ToString();
                
                // cortamos el mensaje si pasa el límite de Discord que son 200 caracteres
                if (mensajeFinal.Length > 2000)
                {
                    mensajeFinal = mensajeFinal.Substring(0, 1900) + "\n...(cortado por límite de Discord)...";
                }

                await ReplyAsync(mensajeFinal);
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ Ocurrió un error inesperado: {ex.Message}");
            }
        }
    }
}