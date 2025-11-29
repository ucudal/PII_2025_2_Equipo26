using Discord.Commands;
using Library;
using System;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    public class VerInteraccionesCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public VerInteraccionesCommand(Fachada fachada)
        {
            _fachada = fachada;
        }
        
        [Command("ver-interacciones")]
        public async Task ExecuteAyudaAsync()
        {
            await ReplyAsync("❌ **Falta el ID del cliente.**\n" +
                             "Ejemplos:\n" +
                             "`!ver-interacciones 1` (Ver todo)\n" +
                             "`!ver-interacciones 1 Llamada` (Solo llamadas)\n" +
                             "`!ver-interacciones 1 Correo 25/10/2023` (Correos desde esa fecha)");
        }


        [Command("ver-interacciones")]
        [Summary("Muestra historial filtrado delegando la lógica a la Fachada.")]
        public async Task ExecuteAsync(
            [Summary("ID del cliente")] int id,
            [Summary("Filtro tipo (opcional)")] string tipoTexto = null,
            [Summary("Filtro fecha (opcional)")] string fechaTexto = null)
        {
            try
            {

                var cliente = _fachada.BuscarCliente(id);
                if (cliente == null)
                {
                    await ReplyAsync("❌ Error: Cliente no encontrado.");
                    return;
                }
                
                List<Interaccion> resultados;

                if (tipoTexto == null)
                { 
                    resultados = _fachada.VerInteraccionesCliente(id);
                }
                else
                {

                    TipoInteraccion tipoEnum = ConvertirTextoAEnum(tipoTexto);

                    if (fechaTexto == null)
                    {
    
                        resultados = _fachada.VerInteraccionesCliente(id, tipoEnum);
                    }
                    else
                    {
                      
                        DateTime fechaFiltro;
                        try
                        {
                            fechaFiltro = DateTime.Parse(fechaTexto);
                        }
                        catch
                        {
                            await ReplyAsync("⚠️ Fecha inválida. Usa dd/mm/aaaa.");
                            return;
                        }

                        resultados = _fachada.VerInteraccionesCliente(id, tipoEnum, fechaFiltro);
                    }
                }
                
                if (resultados.Count == 0)
                {
                    await ReplyAsync("📭 No hay interacciones con esos criterios.");
                    return;
                }

                await EnviarReporte(cliente, resultados);
            }
            catch (Exception ex)
            {
                await ReplyAsync("❌ Error: " + ex.Message);
            }
        }

        private TipoInteraccion ConvertirTextoAEnum(string texto)
        {
            string t = texto.ToLower();
            if (t == "reunion")
            {
                return TipoInteraccion.Reunion;
            }

            if (t == "mensaje")
            {
                return TipoInteraccion.Mensaje;
            }

            if (t == "correo")
            {
                return TipoInteraccion.Correo;
            }

            if (t == "cotizacion")
            {
                return TipoInteraccion.Cotizacion;
            }
            
            return TipoInteraccion.Llamada;
        }

        private async Task EnviarReporte(Cliente cliente, List<Interaccion> lista)
        {
            StringBuilder reporte = new StringBuilder();
            reporte.AppendLine($"📂 **Historial: {cliente.Nombre} {cliente.Apellido}**");
            reporte.AppendLine("--------------------------------");

            foreach (var inter in lista)
            {
                string icono = "📄";
                string nombre = inter.GetType().Name;

                if (nombre == "Llamada") icono = "📞";
                if (nombre == "Reunion") icono = "📅";
                if (nombre == "Mensaje") icono = "💬";
                if (nombre == "Correo")  icono = "📧";

                reporte.AppendLine($"{icono} **{nombre}** - {inter.Fecha.ToString("dd/MM/yyyy")}");
                reporte.AppendLine($"   📝 {inter.Tema}"); 
                
                if (inter.NotaAdicional != null)
                {
                    reporte.AppendLine($"   📌 Nota: {inter.NotaAdicional.Texto}");
                }
                reporte.AppendLine("");
            }

            string mensaje = reporte.ToString();
            if (mensaje.Length > 2000) mensaje = mensaje.Substring(0, 1900) + "...";
            
            await ReplyAsync(mensaje);
        }
    }
}