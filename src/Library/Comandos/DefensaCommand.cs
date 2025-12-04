using Discord.Commands;
using Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{

    public class DefensaCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

   
        public DefensaCommand(Fachada fachada)
        {
            _fachada = fachada;
        }
        
        [Command("defensa_cliente_conventas")]
        [Summary("Muestra clientes sin interacciones en el número de días especificado.")]
        public async Task ExecuteAsync(
            double montoInicio,
            double montoFin)

    {
            try
            {

     
                List<Cliente> clientesVentas = this._fachada.CalcularClientesVentasRango(montoInicio,montoFin);
                
                        StringBuilder sb = new StringBuilder();
       
                        sb.Append(String.Format(" **Clientes con compras entre ${0} y ${1}**\n\n", montoInicio, montoFin));

                        if (clientesVentas.Count == 0)
                        {
                            sb.Append("No se encontraron ventas en ese rango de precios.");
                        }
                        else
                        {
                            foreach (Cliente cliente in clientesVentas)
                            {
                                sb.Append(String.Format("- **ID {0}**: {1} {2}\n",
                                    cliente.Id, cliente.Nombre, cliente.Apellido));
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
