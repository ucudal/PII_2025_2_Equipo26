using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando para registrar el género y la fecha de nacimiento de un cliente.
    /// </summary>
    public class RegistrarDatosAdicionalesCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada; 

        public RegistrarDatosAdicionalesCommand(Fachada fachada)
        {
            _fachada = fachada;
        }

        // 1. Ayuda
        [Command("registrar_datos_cliente")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Faltan datos.**\n" +
                             "**Formato:** `!registrar_datos_cliente <ID> <Género> <FechaNacimiento(DD/MM/AAAA)>`\n\n" +
                             "**Géneros válidos:** Femenino, Masculino, Otro\n\n" +
                             "**Ejemplo:**\n" +
                             "`!registrar_datos_cliente 1 Femenino 15/05/1990`");
        }

        // 2. Ejecución
        [Command("registrar_datos_cliente")]
        [Summary("Registra datos adicionales (género y fecha de nacimiento) para un cliente.")]
        public async Task ExecuteAsync(
            [Summary("ID del Cliente")] int idCliente,
            [Summary("Género del Cliente")] string generoTexto,
            [Summary("Fecha de Nacimiento (DD/MM/AAAA)")] string fechaNacimientoTexto)
        {
            DateTime fechaNacimiento; // Declaramos la variable fuera del TryParse

            try
            {
                var cliente = _fachada.BuscarCliente(idCliente);
                if (cliente == null)
                {
                    await ReplyAsync("❌ **Error**: No existe el cliente con ese ID.");
                    return;
                }

                // Conversión de Tipos: Usamos la variable ya declarada.
                if (!DateTime.TryParse(fechaNacimientoTexto, out fechaNacimiento))
                {
                    await ReplyAsync("⚠️ **Fecha de Nacimiento inválida**. Asegúrate de usar el formato DD/MM/AAAA.");
                    return;
                }

                // Envío del Mensaje: Se delega la lógica de negocio a la Fachada.
                _fachada.RegistrarDatosAdicionalesCliente(idCliente, generoTexto, fechaNacimiento);

                // Respuesta de éxito
                await ReplyAsync("🎉 **Datos Adicionales Registrados**\n" +
                                 "- **Cliente:** " + cliente.Nombre + " " + cliente.Apellido + "\n" +
                                 "- **Género:** " + generoTexto + "\n" +
                                 "- **Fecha Nacimiento:** " + fechaNacimiento.ToString("dd/MM/yyyy"));
            }
            catch (ArgumentException ex)
            {
                await ReplyAsync("❌ **Error de datos**: " + ex.Message);
            }
            catch (Exception ex)
            {
                await ReplyAsync("❌ **Error interno al registrar**: " + ex.Message);
            }
        }
    }
}