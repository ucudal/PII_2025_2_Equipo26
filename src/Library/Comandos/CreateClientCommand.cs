using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir la historia de usuario: 
    /// "Como usuario quiero crear un nuevo cliente con su información básica..."
    /// </summary>
    public class CrearClienteCommand : ModuleBase<SocketCommandContext>
    {
        private readonly FachadaUnit _fachadaUnit;

        public CrearClienteCommand(FachadaUnit fachada)
        {
            _fachadaUnit = fachada;
        }

        [Command("crear_cliente")]
        [Summary("Registra un nuevo cliente solo con datos básicos.")]
        public async Task ExecuteAsync(
            [Summary("Nombre")] string nombre, 
            [Summary("Apellido")] string apellido, 
            [Summary("Teléfono")] string telefono, 
            [Summary("Correo")] string correo)
        {
            try
            {
                // Esta es la lógica principal que se intenta ejecutar.
                _fachadaUnit.CrearCliente(nombre, apellido, telefono, correo);

                await ReplyAsync($"✅ **Cliente Creado**: {nombre} {apellido} ha sido registrado exitosamente.\n*Nota: Recuerde completar los datos faltantes (género, fecha nac.) posteriormente.*");
            }
            catch (Exception ex)
            {
                // Este bloque captura cualquier error.
                await ReplyAsync($"❌ **Error**: El comando no pudo completarse. **Detalle**: {ex.Message}");
            }
        }
    }
}