using HaltMalKurzNode.Commands;
using Microsoft.Win32;
using MunchkinBot.Commands;
using MunchkinBot.Helpers;
using MunchkinBotControl;
using MunchkinBotControl.DB;
using MunchkinBotControl.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using ZetaIpc.Runtime.Server;

namespace MunchkinBot
{
    class Program
    {
        #region variables and constants
        private static TelegramBotClient Bot;
        private static string botUsername = "";
        private static List<BotCommand> commands = new List<BotCommand>();
        private static IpcServer _server;
        private const string version = "v0.0.1";
        private static ManualResetEvent stopEvent = new ManualResetEvent(false);
        private static bool stopping;
        private static readonly MunchkinContext db = new MunchkinContext();
        #endregion

        #region MAIN
        static void Main(string[] args)
        {
            if (args.Length != 3)
            {
                Console.WriteLine("Irgendwie hast du einen Riesenfehler beim Starten des Bots gemacht. Breche ab...");
                Environment.Exit(-1);
            }

            Console.WriteLine("Munchkin Bot v. (alpha) 0.0.1 \n@Author: Olfi01 und SAvB\n\nNur zur privaten Verwendung! Munchkin: (c) Steve Jackson Games 2001 und Pegasus Spiele 2003 für die deutsche Übersetzung.\nAlle Rechte bleiben bei den entsprechenden Eigentümern\n");
            Console.WriteLine("<Bot startet...>\n");

            int port = int.Parse(args[0]);
            string eventHandleName = args[1];
            string botToken = args[2];
            var eventWaitHandle = new EventWaitHandle(false, EventResetMode.ManualReset, eventHandleName);

            _server = new IpcServer();
            _server.ReceivedRequest += ServerReceivedRequest;
            _server.Start(port);
            try
            {
                Bot = new TelegramBotClient(botToken);
            }
            catch
            {
                Console.WriteLine("Dein Token scheint nicht zu stimmen.");
                Environment.Exit(-1);
            }
            try
            {
                botUsername = Bot.GetMeAsync().Result.Username;
            }
            catch
            {
                Console.WriteLine("Konnte keine Verbindung zu den Telegram-Servern herstellen.");
                Environment.Exit(-1);
            }
            eventWaitHandle.Set();

            InitCommands();

            Console.WriteLine("<Bot gestartet!>\n");

            stopEvent.WaitOne();
            StopBot();
        }
        #endregion

        #region messagehandler
        #region Received request from control
        private static void ServerReceivedRequest(object sender, ReceivedRequestEventArgs e)
        {
            if (e.Handled) return;
            IpcMessage requestMessage = JsonConvert.DeserializeObject<IpcMessage>(e.Request);
            switch (requestMessage.Type)
            {
                case IpcMessage.TcpMessageType.Command:
                    #region Handle IPC Commands
                    if (requestMessage.IsGetVersionMessage())
                    {
                        e.Response = version;
                        e.Handled = true;
                        return;
                    }
                    if (requestMessage.IsStopMessage())
                    {
                        stopEvent.Set();
                        e.Handled = true;
                        return;
                    }
                    #endregion
                    break;
                case IpcMessage.TcpMessageType.Update:
                    HandleUpdate(requestMessage.Update);
                    e.Handled = true;
                    break;
            }
        }
        #endregion

        public static void HandleUpdate(Update update)
        {
            if (update.Type == UpdateType.Message)
            {
                OnMessage(update.Message);
            }
        }

        private static void OnMessage(Message msg)
        {
            // TODO: hand over messages to respective games if necessary
            if (msg.Type == MessageType.Text)
            {

                #region command
                //bug: does not recognize commands at all
                if (msg.Entities.Any(x => x.Type == MessageEntityType.BotCommand && x.Offset == 0))
                {
                    string command = null;
                    for (int i = 0; i < msg.Entities.Length; i++)
                    {
                        if (msg.Entities[i].Type == MessageEntityType.BotCommand && msg.Entities[i].Offset == 0)
                        {
                            command = msg.EntityValues.ElementAt(i);
                            break;
                        }
                    }

                    List<BotCommand> commandsToExecute;
                    // checks for all the prerequisites
                    commandsToExecute = commands.FindAll(x =>
                        (x.Command.Trigger == command || x.Command.Trigger + "@" + botUsername == command)
                        && (x.Command.ProcessOnAllNodes || !stopping)
                        && x.Command.HasRequiredContext(msg)
                        && (msg.Text.Trim().Length > command.Length ^ x.Command.Standalone));
                    // create the context for executing the commands
                    CommandContext context = new CommandContext(Bot, msg, db);
                    // go through all commands found
                    foreach (var cmd in commandsToExecute)
                    {
                        // if the chat is a group and the command requires admin, check whether the person issueing the command is admin (or global admin, hehehe)
                        if (msg.Chat.IsGroup() && cmd.Command.RequiresAdmin)
                        {
                            var chatMember = Bot.GetChatMemberAsync(msg.Chat.Id, msg.From.Id).Result;
                            if (chatMember.Status != ChatMemberStatus.Administrator && chatMember.Status != ChatMemberStatus.Creator && !msg.From.IsGlobalAdmin(db)) continue;
                        }
                        // if global admin is required, check whether the person issueing the command is global admin
                        if (cmd.Command.RequiresGlobalAdmin && !msg.From.IsGlobalAdmin(db)) continue;
                        // either execute the command synchronously or asynchronously, depending on configuration
                        if (cmd.Command.ExecuteAsync)
                            cmd.Action.Invoke(context);
                        else
                        {
                            try
                            {
                                cmd.Action.Invoke(context).Wait();
                            }
                            catch (Exception ex)
                            {
                                Bot.SendTextMessageAsync(267376056, ex.ToString());
                                // maybe make dedicated logging channel?
                            }
                        }
                    }
                }
                #endregion

                #region no command



                #endregion

            }
            else
            {
                //Bot.SendTextMessageAsync(e.Message.Chat.Id, "Es werden keine anderen Nachrichten als Textnachrichten unterstützt!");
            }
        }

        #endregion

        #region Stop

        static void StopBot()
        {
            //Cleanup-stuff
            stopping = true;
            // TODO: wait for all games to stop

            Console.WriteLine("<Bot hält an...>");
            Thread.Sleep(1000);
            Bot.StopReceiving();
            Console.WriteLine("<Bot beendet>");
            Environment.Exit(0);

        }

        #endregion

        #region Commands
        private static void InitCommands()
        {
            Type[] classesToSearch = { typeof(StandaloneCommands) };

            foreach (Type t in classesToSearch)
            {
                foreach (var method in t.GetMethods())
                {
                    if (method.ReturnParameter?.ParameterType != typeof(Task)) continue;
                    var parameters = method.GetParameters();
                    if (parameters.Length != 1 || parameters[0].ParameterType != typeof(CommandContext)) continue;
                    var commandAttributes = method.GetCustomAttributes().OfType<CommandAttribute>();
                    if (commandAttributes.Count() < 1) continue;
                    Func<CommandContext, Task> action = (Func<CommandContext, Task>)method.CreateDelegate(typeof(Func<CommandContext, Task>));
                    commands.Add(new BotCommand(commandAttributes.First(), action));
                }
            }
        }
        #endregion

    }

}