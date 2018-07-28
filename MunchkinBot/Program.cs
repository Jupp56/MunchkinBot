using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using TelegramBotApi;
using TelegramBotApi.Enums;
using TelegramBotApi.Types;
using TelegramBotApi.Types.Exceptions;
using TelegramBotApi.Types.Game;
using System.Data.Entity;
using Microsoft.Win32;
using TelegramBotApi.Types.Events;

namespace MunchkinBot
{
    class Lobbyplayer
    {
        //for sorting players in the lobby into the respective groups they posted their /join in
        public long GroupID { get; set; }
        public long PlayerID { get; set; }
        public string PlayerName { get; set; }
    }

    class Program
    {
        #region variables and constants
        private const string dbFileName = "cardDb.sqlc";
        private static TelegramBot Bot;
        private static MunchkinDB db;// = new MunchkinDB(@"C:\Users\Nick\Documents\Visual Studio 2015\Projects\MunchkinBot\MunchkinBot\MunchkinDB.edmx.sql");
        private static string token;
        private static string botUsername = "";
        private static Dictionary<string, Action<Message>> commands = new Dictionary<string, Action<Message>>();
        private static List<Lobbyplayer> playerIds = new List<Lobbyplayer>();
        
        
        #endregion

        #region command dictionary

        #endregion

        #region MAIN
        static void Main(string[] args)
        {
            
            
            Console.WriteLine("Munchkin Bot v. (alpha) 0.0.1 \n@Author: Olfi01 und SAvB\n\nNur zur privaten Verwendung! Munchkin: (c) Steve Jackson Games 2001 und Pegasus Spiele 2003 für die deutsche Übersetzung.\nAlle Rechte bleiben bei den entsprechenden Eigentümern\n");
            Console.WriteLine("<Bot startet...>\n");

            bool started = StartBot();

            if (started == false)
            {
                Console.WriteLine("Es gab ein Problem mit dem Start des Bots. Entweder kann keine Verbindung zum Telegram-Server hergestellt werden, oder das Token ist falsch, oder...\nDetailiertere Fehlerbeschreibung oben.");
                Console.ReadLine();
                Environment.Exit(1);
            }

            InitCommands();
            Bot.OnMessage += Bot_OnMessage;
            

            Console.WriteLine("<Bot gestartet!>\n");

            Console.ReadKey();
            StopBot();
            
            
        }
        #endregion

        #region messagehandler
        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
            {

                #region command
                //bug: does not recognize commands at all
                if (e.Message.Entities.Any(x => x.Type == MessageEntityType.BotCommand && x.Offset == 0 && x.Value != null))
                {
                    //int entityIndex = e.Message.Entities.Find(x => x.Type == MessageEntityType.BotCommand && x.Offset == 0).Value;
                    
                    string command = e.Message.Entities.FirstOrDefault(x => x.Type == MessageEntityType.BotCommand && x.Offset == 0).Value;
                    
                    if (command.EndsWith($"@{botUsername}")) command = command.Remove(command.LastIndexOf($"@{botUsername}"));
                    if (commands.ContainsKey(command)) commands[command].Invoke(e.Message);
                    Console.Write("command: {0}", command);
                    
                    
                }
                #endregion

                #region extra commands
                if (e.Message.Text=="someone there?")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Bot up and running... " + e.Message.Chat.FirstName + " " + e.Message.Chat.LastName);
                }
                //to be deleted, only testing
                if (e.Message.Text == "/startgame")
                {
                    if (e.Message.Chat.Type == ChatType.Group)
                    {
                        //Console.WriteLine("starting new game - lobby");

                        List<long> gameplayerIds = new List<long>();

                        foreach (Lobbyplayer lp in playerIds)
                        {
                            if (lp.GroupID == e.Message.Chat.Id) gameplayerIds.Add(lp.PlayerID);
                        }
                        
                        Classes.Game g = new Classes.Game(gameplayerIds, e.Message.Chat.Id);
                    }

                }
                if (e.Message.Text == "myID") Console.Write(e.Message.Chat.Id);

                if (e.Message.Text == "/join")
                {
                    if (e.Message.Chat.Type == ChatType.Group)
                    {
                        //sorts players by the groups they posted /join in so they are recognized correctly
                        playerIds.RemoveAll(x => x.PlayerID == e.Message.From.Id);
                        playerIds.Add(new Lobbyplayer() { PlayerID = e.Message.From.Id, GroupID = e.Message.Chat.Id, PlayerName = e.Message.From.FullName});
                        Console.WriteLine("Player joined, ID: {0}, groupid: {1}, Name: {2}", e.Message.From.Id, e.Message.Chat.Id, e.Message.From.FullName);
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

        #region StartandStop

        public static bool StartBot()
        {
            bool started = false;

            db = new MunchkinDB(dbFileName);
            //TODO: Eine weitere Datenbank für Spiele, Nutzer und Gruppen anlegen im AppData\Roaming\MunchkinBot Ordner
            //Hier die Startüberprüfungen (Datenbank???)(Token überprüfen...)
            //Token überprüft die lib selbst, no need to worry. Evtl. Bot.GetMe() für Funktionalität.
            try
            {
                var subkey = Registry.CurrentUser.CreateSubKey("MunchkinBot", true);
                if (subkey.GetValue("Token") == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nKein gespeichertes Telegram-Bot-Token gefunden. Bitte eingeben:\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    token = Console.ReadLine();
                    subkey.SetValue("Token", token);
                }
                else
                {
                    token = (string)subkey.GetValue("Token");
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n<Error> Fehler beim Lesen des Tokens! Fehler: {0}", ex);
                token = "Fehler";
            }

            Console.Write("\nToken des Bots: {0}\n", token);

            if (token == "Fehler")
            {
                started = false;
            }
            else
            {
                started = true;
                try
                {
                    Bot = new TelegramBot(token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Etwas stimmt nicht mit deinem Token! Fehler: {0}", ex);
                    var subkey = Registry.CurrentUser.CreateSubKey("MunchkinBot", true);
                    subkey.DeleteValue("Token");
                    started = false;
                }
            }

            botUsername = Bot.GetMeAsync().Result.Username;
            Bot.StartReceiving();

            //Console.WriteLine(started.ToString());
            return started;
        }

        static void StopBot()
        {
            //Cleanup-stuff

            if(allcurrentchats != null)
            {
                foreach (string s in allcurrentchats)
                {
                    Bot.SendTextMessageAsync(s, "Bot stopped"); //not implemented yet
                }
            }

            
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
            commands.Add("/start", StartCommand);
        }

        private static void StartCommand(Message msg)
        {
            throw new NotImplementedException();
        }
        #endregion

        public static List<string> allcurrentchats = new List<string>(); //nowhere implemented yet

    }

}