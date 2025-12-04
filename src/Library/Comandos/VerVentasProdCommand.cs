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
        [Summary("Muestra los clientes que han comprado un producto o servicio específico.")]
        public async Task ExecuteAsync([Summary("Nombre del producto")] string producto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(producto))
                {
                    await ReplyAsync("❌ Por favor, indica un nombre de producto. Ejemplo: `!ver_ventas Laptop`");
                    return;
                }

                
                List<Cliente> clients = _fachada.BuscarVentasProducto(producto);

                
                if (clients.Count == 0)
                {
                    await ReplyAsync($"⚠️ No se encontraron clientes que hayan comprado productos que contengan: **'{producto}'**.");
                    return;
                }
                
                StringBuilder mensaje = new StringBuilder();
                mensaje.AppendLine($"📋 **Clientes que han comprado '{producto}':**");
                mensaje.AppendLine("```"); 
                mensaje.AppendLine($"{"ID".PadRight(4)} | {"Nombre Completo".PadRight(25)}");
                mensaje.AppendLine(new string('-', 35)); 

                foreach (var client in clients)
                {
                    string nombreCompleto = $"{client.Nombre} {client.Apellido}";
                    
                    if (nombreCompleto.Length > 22) 
                        nombreCompleto = nombreCompleto.Substring(0, 19) + "...";

                    mensaje.AppendLine($"{client.Id.ToString().PadRight(4)} | {nombreCompleto.PadRight(25)}");
                }
                
                mensaje.AppendLine("```"); 
                
                await ReplyAsync(mensaje.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ReplyAsync("❌ Ocurrió un error al buscar las ventas.");
            }
        }
    }
}