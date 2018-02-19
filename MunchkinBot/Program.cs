using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using Telegram.Bot;
using System.Data.Entity;
using Microsoft.Win32;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;

namespace MunchkinBot
{
    class Program
    {
        #region variables
        private const string dbFileName = "cardDb.sqlc";
        private static TelegramBotClient Bot; // = new TelegramBotClient("523450690:AAHuwdKhWHIZwwndxQ1bWcktpL9kJqnEGR8"); //tokenabfrage noch nicht implementiert

        private static MunchkinDB db;// = new MunchkinDB(@"C:\Users\Nick\Documents\Visual Studio 2015\Projects\MunchkinBot\MunchkinBot\MunchkinDB.edmx.sql");
        private static string token;
        private static string botUsername = "";
        private static Dictionary<string, Action<Message>> commands = new Dictionary<string, Action<Message>>();
        #endregion
        #region MAIN
        static void Main(string[] args)
        {
            Console.WriteLine(@"C:\users\" + Environment.UserName + @"\test");
            Console.WriteLine("<Bot startet...>\n");
            Console.WriteLine("Munchkin Bot v. (alpha) 0.0.1 \n@Author: Olfi01 und SAvB\n\nNur zur privaten Verwendung! Munchkin: (c) Steve Jackson Games 2001 und Pegasus Spiele 2003 für die deutsche Übersetzung.\nAlle Rechte bleiben bei den entsprechenden Eigentümern\n");

            bool started = StartBot();

            if (started == false)
            {
                Console.WriteLine("Es gab ein Problem mit dem Start des Bots. Entweder kann keine Verbindung zum Telegram-Server hergestellt werden, oder das Token ist falsch, oder...\nDetailiertere Fehlerbeschreibung oben.");
                Console.ReadLine();
                Environment.Exit(1);
            }

            InitCommands();
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnMessageEdited += Bot_OnMessage;

            Console.WriteLine("<Bot gestartet!>\n");

            

            Console.ReadKey();
            StopBot();
            
            
        }
        #endregion

        #region messagehandler
        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
            {

                #region commands
                if (e.Message.Entities.Any(x => x.Type == MessageEntityType.BotCommand && x.Offset == 0))
                {
                    int entityIndex = e.Message.Entities.FindIndex(x => x.Type == MessageEntityType.BotCommand && x.Offset == 0);
                    string command = e.Message.EntityValues[entityIndex];
                    if (command.EndsWith($"@{botUsername}")) command = command.Remove(command.LastIndexOf($"@{botUsername}"));
                    if (commands.ContainsKey(command)) commands[command].Invoke(e.Message);
                }
                #endregion

                #region extra commands
                if (e.Message.Text=="someone there?")
                {
                    Bot.SendTextMessageAsync(e.Message.Chat.Id, "Bot up and running... " + e.Message.Chat.FirstName + " " + e.Message.Chat.LastName);
                }
                #endregion

                #region no command
                
                //eingehende Nachricht an die anderen Spieler weiterleiten? Oder kommt der Bot in eine Gruppe?

                #endregion

            }
            else
            {
                Bot.SendTextMessageAsync(e.Message.Chat.Id, "Es werden keine anderen Nachrichten als Textnachrichten unterstützt!");
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
                /*if (!File.Exists(Path.Combine(Environment.SpecialFolder.DesktopDirectory + "/testformunchkin/token.conf")))
                {
                    Directory.CreateDirectory(Path.Combine(Environment.SpecialFolder.DesktopDirectory + "/testformunchkin"));
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nKein gespeichertes Telegram-Bot-Token gefunden. Bitte eingeben:\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    token = Console.ReadLine();
                    File.WriteAllText(Environment.SpecialFolder.DesktopDirectory + "/testformunchkin/token.conf", token);
                }

                else
                {
                    token = File.ReadAllText(Path.Combine(Environment.SpecialFolder.DesktopDirectory + "/testformunchkin/token.conf"));                                     
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n<Error> Fehler beim Lesen des Tokens! Fehler: {0}", ex);
                token = "Fehler";
            }

            Console.Write("\n{0}\n",token);

            if (token == "Fehler")
            {
                started = false;
            }
            else
            {
                started = true;
                try
                {
                    Bot = new TelegramBotClient(token);
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

            Console.WriteLine(started.ToString());
            return started;
        }

        static void StopBot()
        {
            //Cleanup-Zeug... falls da was sein sollte          
            Console.WriteLine("<Bot hält an...");
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
    }

}