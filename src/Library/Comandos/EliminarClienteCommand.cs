    using Discord.Commands;
    using Library;
    using System;
    using System.Threading.Tasks;

    namespace Ucu.Poo.DiscordBot.Commands
    {
        /// <summary>
        /// Comando para cumplir la historia de usuario: 
        /// "Como usuario quiero eliminar un cliente..."
        /// </summary>
        public class EliminarClienteCommand : ModuleBase<SocketCommandContext>
        {
            private readonly Fachada _fachada;

            public EliminarClienteCommand(Fachada fachada)
            {
                _fachada = fachada;
            }

            [Command("eliminar-cliente")]
            [Summary("Elimina un cliente del sistema permanentemente dado su ID.")]
            public async Task ExecuteAsync(
                [Summary("ID del cliente a eliminar")] int id)
            {
                try
                {
                    // 1. Buscamos el cliente primero para poder mostrar su nombre en el mensaje de confirmación
                    // y verificar que exista antes de intentar borrarlo.
                    var cliente = _fachada.BuscarCliente(id);

                    if (cliente == null)
                    {
                        await ReplyAsync($"❌ **Error**: No existe un cliente con el ID {id}.");
                        return;
                    }

                    // 2. Eliminamos
                    _fachada.EliminarCliente(id);

                    await ReplyAsync(
                        $"🗑️ **Cliente Eliminado**: Se ha eliminado a {cliente.Nombre} {cliente.Apellido} (ID: {id}) de la base de datos.");
                }
                catch (Exception ex)
                {
                    await ReplyAsync($"❌ **Error**: No se pudo eliminar el cliente. {ex.Message}");
                }
            }
        }
    }