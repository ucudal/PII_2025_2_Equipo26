using System;
// using Ucu.Poo.DiscordBot.Domain;
using Ucu.Poo.DiscordBot.Services;

namespace Ucu.Poo.DiscordBot.Program
{
    /// <summary>
    /// Un programa que implementa un bot de Discord.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada al programa.
        /// </summary>
        private static void Main(string [] args)
        {
            DemoBot();
        }

        private static void DemoBot()
        {
            BotLoader.LoadAsync().GetAwaiter().GetResult();
        }
    }
}
