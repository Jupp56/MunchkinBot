using MunchkinBotControl.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HaltMalKurzNode.Commands
{
    public class CommandContext
    {
        public TelegramBotClient Bot { get; }
        public Message Message { get; }
        public string[] Args { get; }
        public MunchkinContext DB { get; }

        public CommandContext(TelegramBotClient bot, Message message, MunchkinContext db)
        {
            Bot = bot;
            Message = message;
            DB = db;
            var split = message.Text.Split(' ');
            Args = new string[split.Length - 1];
            for (int i = 1; i < split.Length; i++) Args[i - 1] = split[i];
        }
    }
}
