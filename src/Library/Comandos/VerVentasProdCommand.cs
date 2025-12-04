using Discord.Commands;
using Library;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    public class VerVentasProdCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public VerVentasProdCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        [Command("ver_ventas")]
        [Summary("Comando utilizado para ver las ventas de cierto producto o servicio en el historial de clientes")]
        public async Task ExecuteAsync(
            [Summary("Nombre del producto o servicio a buscar")]
            string producto)
        {
            try
            {
                List<Cliente> clients = _fachada.BuscarVentasProducto(producto);
                StringBuilder mensaje = new StringBuilder();
                mensaje.AppendLine("📋 **Clientes que han comprado este producto/servicio:**");
                mensaje.AppendLine("```"); 
                mensaje.AppendLine($"{"ID".PadRight(4)} | {"Nombre Completo".PadRight(25)}");
                mensaje.AppendLine(new string('-', 50)); // Línea separadora
                foreach (var client in clients)
                {
                    string nombreCompleto = $"{client.Nombre} {client.Apellido}";
                    
                    if (nombreCompleto.Length > 22) nombreCompleto = nombreCompleto.Substring(0, 22) + "...";

                    mensaje.AppendLine($"{client.Id.ToString().PadRight(4)} | {nombreCompleto.PadRight(25)}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}