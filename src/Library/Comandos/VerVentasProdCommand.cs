using Discord.Commands;
using Library;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando creado para cumplir con la historia de usuario:
    /// "Como usuario quiero ver los clientes con ventas de cierto producto o servicio"
    /// </summary>
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
                // Verifica que el comando no se pase sin argumentos
                if (string.IsNullOrWhiteSpace(producto))
                {
                    await ReplyAsync("❌ Por favor, indica un nombre de producto. Ejemplo: `!ver_ventas Laptop`");
                    return;
                }

                //Llama al metodo para buscar las ventas por producto
                List<Cliente> clients = _fachada.BuscarVentasProducto(producto);

                //Excepcion por si no hay clientes que hayan comprado tal producto/servicio
                if (clients.Count == 0)
                {
                    await ReplyAsync($"⚠️ No se encontraron clientes que hayan comprado el producto/servicio: **'{producto}'**.");
                    return;
                }
                
                StringBuilder mensaje = new StringBuilder();
                mensaje.AppendLine($"📋 **Clientes que han comprado '{producto}':**");
                mensaje.AppendLine("```"); 
                mensaje.AppendLine($"{"ID".PadRight(4)} | {"Nombre Completo".PadRight(25)}");
                mensaje.AppendLine(new string('-', 35)); 
                //Con la lista brindada por el metodo, se toma el nombre e id de cada cliente
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
            //Excepcion para errores
            catch (Exception e)
            {
                Console.WriteLine(e);
                await ReplyAsync("❌ Ocurrió un error al buscar las ventas.");
            }
        }
    }
}