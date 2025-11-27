using Discord.Commands;
using Library;
using System;
using System.Threading.Tasks;

namespace Ucu.Poo.DiscordBot.Commands
{
    /// <summary>
    /// Comando creado para cumplir con historia de usuario:
    /// "Como usuario quiero registrar llamadas enviadas o recibidas de clientes..."
    /// </summary>
    public class RegisLlamadasCommand : ModuleBase<SocketCommandContext>
    {
        private readonly Fachada _fachada;

        public RegisLlamadasCommand(Fachada fachada)
        {
            _fachada = fachada;
        }
        
        [Command("registrar-llamada")]
        [Summary("Registra la llamada con el cliente seleccionada.")]
        
        public async Task ExecuteAsync (
            [Summary("cliente con el que se efectuo la llamada")] Cliente cliente,
            
        
        
        )
        
    }
}