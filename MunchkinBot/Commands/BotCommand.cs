using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaltMalKurzNode.Commands
{
    public class BotCommand
    {
        public CommandAttribute Command { get; }
        public Func<CommandContext, Task> Action { get; }

        public BotCommand(CommandAttribute command, Func<CommandContext, Task> actionAsync)
        {
            Command = command;
            Action = actionAsync;
        }
    }
}
