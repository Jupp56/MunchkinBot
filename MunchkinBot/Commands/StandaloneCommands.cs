using HaltMalKurzNode.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;

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

        [Command("/stickerid", Description = "Sends back the sticker ID", Usage = "/stickerid", RequiredContext = CommandAttribute.Context.Private, RequiresGlobalAdmin = true, Standalone = true)]
        public static async Task StickerId(CommandContext context)
        {
            if (context.Message.Type != MessageType.Sticker) return;
            await context.Bot.SendTextMessageAsync(context.Message.Chat.Id, context.Message.Sticker.FileId);
        }
    }
}
