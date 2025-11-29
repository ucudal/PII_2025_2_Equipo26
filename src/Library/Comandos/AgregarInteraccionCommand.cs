using Discord.Commands;
using Library;
using System;
using System.Collections.Generic; 
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para cumplir la historia de usuario: 
    /// "Como usuario quiero agregar notas o comentarios a las interacciones..."
    /// </summary>
    public class AgregarInteraccionCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public AgregarInteraccionCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        [Command("agregar_interaccion")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "Este comando agrega una nota a la **última** interacción de ese tipo que tengas con el cliente.\n\n" +
                             "**Formato:** `!agregar_interaccion <ID> <Tipo> <Comentario>`\n" +
                             "**Tipos válidos:** Llamada, Reunion, Mensaje, Correo.\n\n" +
                             "**Ejemplo:**\n" +
                             "`!agregar_interaccion 1 Llamada El cliente pidió que lo llamen de nuevo mañana`");
        }

        [Command("agregar_interaccion")]
        [Summary("Agrega una nota a la última interacción encontrada del tipo especificado.")]
        public async Task ExecuteAsync(
            [Summary("ID del cliente")] int id, 
            [Summary("Tipo de interacción (Llamada, Reunion, etc.)")] string tipo, 
            [Summary("Nota a agregar")] [Remainder] string comentario)
        {
            try
            {
               
                var cliente = _fachada.BuscarCliente(id);
                if (cliente == null)
                {
                    await ReplyAsync($"❌ **Error**: No existe un cliente con el ID {id}.");
                    return;
                }
                
                List<Interaccion> interacciones = _fachada.VerInteraccionesCliente(id);

                if (interacciones.Count == 0)
                {
                    await ReplyAsync($"⚠️ El cliente {cliente.Nombre} no tiene ninguna interacción registrada aún.");
                    return;
                }

                
                int indiceEncontrado = -1;
                string tipoBuscado = tipo.ToLower();

                for (int i = interacciones.Count - 1; i >= 0; i--)
                {
                    
                    string tipoActual = interacciones[i].GetType().Name.ToLower();
                    
                    if (tipoActual.Contains(tipoBuscado))
                    {
                        indiceEncontrado = i;
                        break; 
                    }
                }

               
                if (indiceEncontrado == -1)
                {
                    await ReplyAsync($"⚠️ **No se encontró ninguna interacción de tipo '{tipo}'** para este cliente.");
                    return;
                }

              
                _fachada.AgregarNotaAInteraccion(id, indiceEncontrado, comentario);

          
                await ReplyAsync($"✅ **Nota Agregada Exitosamente**\n" +
                                 $"📌 **Cliente:** {cliente.Nombre} {cliente.Apellido}\n" +
                                 $"🔗 **Vinculada a:** {interacciones[indiceEncontrado].GetType().Name} (Reciente)\n" +
                                 $"📝 **Nota:** \"{comentario}\"");
            }
            catch (Exception ex)
            {
                await ReplyAsync($"❌ **Error inesperado**: {ex.Message}");
            }
        }
    }
}