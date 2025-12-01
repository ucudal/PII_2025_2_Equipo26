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
        private readonly FachadaUnit _fachadaUnit;

        public ModificarClienteCommand(FachadaUnit fachada)
        {
            _fachadaUnit = fachada;
        }

        [Command("modificar_cliente")]
        [Summary("Actualiza toda la información de un cliente existente dado su ID.")]
        public async Task ExecuteAsync(
            [Summary("ID del cliente a modificar")] int id,
            [Summary("Nuevo Nombre")] string nombre, 
            [Summary("Nuevo Apellido")] string apellido, 
            [Summary("Nuevo Teléfono")] string telefono, 
            [Summary("Nuevo Correo")] string correo,
            [Summary("Nuevo Género")] string genero,
            [Summary("Nueva Fecha de nacimiento (yyyy-mm-dd)")] DateTime fechaNacimiento)
        {
            try
            {
                var clienteExistente = _fachadaUnit.BuscarCliente(id);
                if (clienteExistente == null)
                {
                    await ReplyAsync($"❌ **Error**: No se encontró ningún cliente con el ID {id}.");
                    return;
                }
                
                _fachadaUnit.ModificarCliente(id, nombre, apellido, telefono, correo, genero, fechaNacimiento);

                await ReplyAsync($"✅ **Datos Actualizados**: El cliente #{id} ({nombre} {apellido}) ha sido modificado correctamente.");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ **Error**: No se pudo modificar el cliente. {ex.Message}");
            }
        }
    }
}