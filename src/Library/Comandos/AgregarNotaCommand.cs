using Discord.Commands;
using Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir la historia: 
    /// "Como usuario quiero agregar notas o comentarios a las llamadas, reuniones..."
    /// </summary>
    public class AgregarNotaCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public AgregarNotaCommand(Fachada fachada)
        {
            _fachada = fachada;
        }
        
        [Command("agregar-nota")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "Este comando agrega una nota a la **última** interacción de ese tipo que tengas con el cliente.\n\n" +
                             "**Formato:** `!agregar-nota <ID_Cliente> <Tipo> <Nota>`\n" +
                             "**Tipos válidos:** Llamada, Reunion, Mensaje, Correo.\n\n" +
                             "**Ejemplo:**\n" +
                             "`!agregar-nota 1 Llamada El cliente pidió que lo llamen de nuevo mañana`\n" +
                             "*(Esto agregará la nota a la última llamada registrada con el cliente 1)*");
        }
        
        [Command("agregar-nota")]
        [Summary("Agrega una nota a la última interacción encontrada del tipo especificado.")]
        public async Task ExecuteAsync(
            [Summary("ID del cliente")] int id, 
            [Summary("Tipo de interacción (Llamada, Reunion, etc.)")] string tipo, 
            [Summary("Nota a agregar")] [Remainder] string nota)
        {
            try
            {
                // A. Validar Cliente
                var cliente = _fachada.BuscarCliente(id);
                if (cliente == null)
                {
                    await ReplyAsync($"❌ **Error**: No existe un cliente con el ID {id}.");
                    return;
                }

                // B. Obtener interacciones para buscar la correcta
                List<Interaccion> interacciones = _fachada.VerInteraccionesCliente(id);

                if (interacciones.Count == 0)
                {
                    await ReplyAsync($"⚠️ El cliente {cliente.Nombre} no tiene ninguna interacción registrada para agregarle notas.");
                    return;
                }
                
                int indiceEncontrado = -1;
                string tipoBuscado = tipo.ToLower();

                for (int i = interacciones.Count - 1; i >= 0; i--)
                {

                    string tipoActual = interacciones[i].GetType().Name.ToLower();

                    if (tipoActual == tipoBuscado)
                    {
                        indiceEncontrado = i;
                        break; 
                    }
                }


                if (indiceEncontrado == -1)
                {
                    await ReplyAsync($"⚠️ **No se encontró ninguna '{tipo}'** previa para este cliente. Registra una primero.");
                    return;
                }
                
                _fachada.AgregarNotaAInteraccion(id, indiceEncontrado, nota);
                
                await ReplyAsync($"✅ **Nota Agregada**\n" +
                                 $"📌 **Cliente:** {cliente.Nombre} {cliente.Apellido}\n" +
                                 $"🔗 **Vinculada a:** {interacciones[indiceEncontrado].GetType().Name} (del {interacciones[indiceEncontrado].Fecha.ToShortDateString()})\n" +
                                 $"📝 **Contenido:** \"{nota}\"");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ **Error inesperado**: {ex.Message}");
            }
        }
    }
}