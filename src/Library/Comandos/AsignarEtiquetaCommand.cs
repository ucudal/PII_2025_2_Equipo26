using Discord.Commands;
using Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir la historia de usuario: 
    /// "Como usuario quiero poder agregar una etiqueta a un cliente..."
    /// </summary>
    public class AsignarEtiquetaCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public AsignarEtiquetaCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        // 1. Ayuda: Se activa si pones solo !asignar_etiqueta
        [Command("asignar_etiqueta")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "Este comando asigna una etiqueta existente a un cliente.\n\n" +
                             "**Formato:** `!asignar_etiqueta <ID_Cliente> <NombreEtiqueta>`\n" +
                             "**Ejemplo:**\n" +
                             "`!asignar_etiqueta 1 VIP`\n" +
                             "`!asignar_etiqueta 5 Deudor`");
        }

        // 2. Ejecución: Busca el ID de la etiqueta por su nombre y la asigna
        [Command("asignar_etiqueta")]
        [Summary("Asigna una etiqueta a un cliente usando el nombre de la etiqueta.")]
        public async Task ExecuteAsync(
            [Summary("ID del cliente")] int idCliente, 
            [Summary("Nombre de la etiqueta")] [Remainder] string nombreEtiqueta)
        {
            try
            {
                // A. Validar que el cliente existe
                var cliente = _fachada.BuscarCliente(idCliente);
                if (cliente == null)
                {
                    await ReplyAsync($"❌ **Error**: No existe el cliente con ID {idCliente}.");
                    return;
                }

                // B. Buscar el ID de la etiqueta basándonos en el nombre que escribió el usuario.
                // Usamos VerTodasLasEtiquetas() que ya está en la Fachada.
                var listaEtiquetas = _fachada.VerTodasLasEtiquetas();
                int idEtiquetaEncontrada = -1;
                
                // Recorremos manualmente (sin LINQ)
                foreach (var etiqueta in listaEtiquetas)
                {
                    // Comparamos nombres ignorando mayúsculas/minúsculas
                    if (etiqueta.Nombre.ToLower() == nombreEtiqueta.ToLower())
                    {
                        idEtiquetaEncontrada = etiqueta.Id;
                        break;
                    }
                }

                if (idEtiquetaEncontrada == -1)
                {
                    await ReplyAsync($"⚠️ **Etiqueta no encontrada**: No existe la etiqueta '{nombreEtiqueta}'.\n" +
                                     $"Usa `!crear-etiqueta {nombreEtiqueta}` primero si deseas crearla.");
                    return;
                }

                // C. Asignar la etiqueta usando el método de la Fachada que pide IDs
                _fachada.AgregarEtiquetaCliente(idCliente, idEtiquetaEncontrada);

                await ReplyAsync($"✅ **Etiqueta Asignada**\n" +
                                 $"🏷️ **Etiqueta:** {nombreEtiqueta}\n" +
                                 $"👤 **Cliente:** {cliente.Nombre} {cliente.Apellido}");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ **Error**: {ex.Message}");
            }
        }
    }
}