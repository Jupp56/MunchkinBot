using HaltMalKurzNode.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MunchkinBot.Commands
{
    public class StandaloneCommands
    {
        [Command("/start", Description = "Starts the bot", Usage = "/start", ProcessOnAllNodes = false, RequiredContext = CommandAttribute.Context.Private, Standalone = true)]
        public static async Task Start(CommandContext context)
        {
            await Task.CompletedTask;
            throw new NotImplementedException();
        }
    }
}
