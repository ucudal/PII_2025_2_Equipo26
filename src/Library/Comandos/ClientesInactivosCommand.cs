using Discord.Commands;
using Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando creado para cumplir con la historia de usuario:
    /// "Cómo usuario quiero saber los clientes que hace cierto tiempo que no tengo ninguna interacción con ellos..."
    /// </summary>
    public class ClientesInactivosCommand : ModuleBase<SocketCommandContext>
    {
        private readonly FachadaUnit _fachadaUnit;

        // Inyección de Dependencia para la Fachada
        public ClientesInactivosCommand(FachadaUnit fachada)
        {
            _fachadaUnit = fachada;
        }

        // 1. Ayuda
        [Command("clientes_inactivos")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "**Formato:** `!clientes_inactivos <días>`\n\n" +
                             "**Ejemplo:**\n" +
                             "`!clientes_inactivos 30` (Muestra clientes sin interacción en los últimos 30 días)");
        }

        // 2. Ejecución
        [Command("clientes_inactivos")]
        [Summary("Muestra clientes sin interacciones en el número de días especificado.")]
        public async Task ExecuteAsync(
            [Summary("Días sin interacción")] int dias)
        {
            try
            {
                if (dias <= 0)
                {
                    await ReplyAsync("⚠️ **Error**: El número de días debe ser positivo.");
                    return;
                }

                // Envío del Mensaje: Delegación a la Fachada
                List<Cliente> clientesInactivos = this._fachadaUnit.ObtenerClientesInactivos(dias);
                
                // Usamos StringBuilder para construir el mensaje eficientemente
                StringBuilder sb = new StringBuilder();
                sb.Append(String.Format("📋 **Clientes Inactivos (Últimos {0} días)**\n\n", dias));

                if (clientesInactivos.Count == 0)
                {
                    sb.Append("✅ ¡Todos los clientes han tenido interacción reciente!");
                }
                else
                {
                    foreach (Cliente cliente in clientesInactivos)
                    {
                        // Se delega a cada objeto Cliente para obtener su última fecha
                        DateTime ultimaInteraccion = cliente.ObtenerFechaUltimaInteraccion();
                        
                        // Formateamos la fecha para la respuesta
                        string fechaStr = ultimaInteraccion == DateTime.MinValue ? "Nunca" : ultimaInteraccion.ToString("dd/mm/yyyy");
                        
                        sb.Append(String.Format("- **ID {0}**: {1} {2} (Última: {3})\n", 
                                                cliente.Id, cliente.Nombre, cliente.Apellido, fechaStr));
                    }
                }
                
                await ReplyAsync(sb.ToString());
            }
            catch (Exception ex)
            {
                await ReplyAsync("❌ **Error**: " + ex.Message);
            }
        }
    }
}