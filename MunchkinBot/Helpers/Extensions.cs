using HaltMalKurzNode.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MunchkinBot.Helpers
{
    public static class Extensions
    {
        public static bool HasRequiredContext(this CommandAttribute command, Message msg)
        {
            switch (command.RequiredContext)
            {
                case CommandAttribute.Context.All:
                    return true;
                case CommandAttribute.Context.Group:
                    return msg.Chat.Type == ChatType.Group || msg.Chat.Type == ChatType.Supergroup;
                case CommandAttribute.Context.Private:
                    return msg.Chat.Type == ChatType.Private;
            }
            return false;
        }
    }
}
