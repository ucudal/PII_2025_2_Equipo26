using Discord.Commands;
using Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando creado para cumplir la historia de usuario:
    /// "Como usuario quiero ver un panel con clientes totales, interacciones recientes y reuniones próximas, para tener un resumen rápido."
    /// </summary>
    public class DashboardCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public DashboardCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        [Command("dashboard")]
        [Summary("Muestra un resumen rápido del estado de la cartera de clientes.")]
        public async Task ExecuteAsync()
        {
            try
            {
                // Envío del Mensaje: Delegación a la Fachada
                ResumenDashboard resumen = this._fachada.ObtenerResumenDashboard();

                StringBuilder sb = new StringBuilder();
                sb.Append("📊 **RESUMEN DEL CRM (DASHBOARD)** 📊\n");
                sb.Append("-------------------------------------\n");

                // 1. Clientes Totales
                sb.Append(String.Format("👥 **Clientes Totales**: {0}\n\n", resumen.TotalClientes));

                // 2. Reuniones Próximas
                sb.Append("📅 **Próximas Reuniones**:\n");
                if (resumen.ReunionesProximas.Count == 0)
                {
                    sb.Append("  *No hay reuniones programadas.*\n");
                }
                else
                {
                    // Mostramos solo las primeras 3 por brevedad
                    for (int i = 0; i < 3 && i < resumen.ReunionesProximas.Count; i++)
                    {
                        Reunion r = resumen.ReunionesProximas[i];
                        sb.Append(String.Format("  - {0} en {1} ({2})\n", 
                                                r.Tema, 
                                                r.Lugar, 
                                                r.Fecha.ToString("dd/MM HH:mm")));
                    }
                }
                
                sb.Append("\n");

                // 3. Interacciones Recientes
                sb.Append("💬 **Interacciones Recientes (Top 5)**:\n");
                if (resumen.InteraccionesRecientes.Count == 0)
                {
                    sb.Append("  *No hay interacciones recientes.*\n");
                }
                else
                {
                    foreach (Interaccion i in resumen.InteraccionesRecientes)
                    {
                        // Usamos el Tipo de Interacción para la visualización
                        sb.Append(String.Format("  - {0} sobre '{1}' ({2})\n", 
                                                i.Tipo.ToString(), 
                                                i.Tema, 
                                                i.Fecha.ToString("dd/MM HH:mm")));
                    }
                }
                
                await ReplyAsync(sb.ToString());
            }
            catch (Exception ex)
            {
                await ReplyAsync("❌ **Error al generar Dashboard**: " + ex.Message);
            }
        }
    }
}