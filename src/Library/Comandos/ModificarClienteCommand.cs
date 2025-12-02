using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir la historia de usuario: 
    /// "Como usuario quiero modificar la información de un cliente existente..."
    /// </summary>
   public class ModificarClienteCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public ModificarClienteCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        // 1. Comando de Ayuda
        [Command("modificar_cliente")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Uso Incorrecto.**\n" +
                             "**Formato:** `!modificar_cliente <ID> <Campo> <Valor>`\n" +
                             "**Ejemplo:** `!modificar_cliente 1 Telefono 099123456`");
        }

        // 2. Ejecución
        [Command("modificar_cliente")]
        [Summary("Modifica un dato del cliente.")]
        public async Task ExecuteAsync(
            [Summary("ID")] int id, 
            [Summary("Campo")] string campo, 
            [Summary("Valor")] [Remainder] string valor)
        {
            try
            {
                // Llamamos a la fachada
                _fachada.ModificarCliente(id, campo, valor);

                // Buscamos al cliente solo para mostrar confirmación bonita
                Cliente cliente = _fachada.BuscarCliente(id);

                await ReplyAsync("✅ **Cliente Actualizado**\n" +
                                 "Cliente: " + cliente.Nombre + " " + cliente.Apellido + "\n" +
                                 "Campo modificado: " + campo + "\n" +
                                 "Nuevo valor: " + valor);
            }
            catch (Exception ex)
            {
                await ReplyAsync("❌ **Error**: " + ex.Message);
            }
         }
    }
}