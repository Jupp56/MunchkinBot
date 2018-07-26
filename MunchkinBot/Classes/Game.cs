using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotApi;
using TelegramBotApi.Enums;
using TelegramBotApi.Types;
using TelegramBotApi.Types.Events;
using Microsoft.Win32;


namespace MunchkinBot.Classes
{
    public class Game
    {
        #region variables

        public long GroupId { get; set; }
        public string Token { get; set; }
        public List<Player> Players = new List<Player>();
        public Pile DoorPile = new Pile();
        public Pile TreasurePile = new Pile();
        public Stack DoorStack;
        public Stack TreasureStack;
        public event EventHandler<string> SendMessage;
        private static TelegramBot Bot; //do we do this from here?
        private Card ActiveDoorCard = new Card();
        private bool started;
        private string botUsername;

        #endregion

        #region constructor

        public Game(List<long> PlayerIDs, long GroupId)
        {
            Console.WriteLine("starting new game");
            this.GroupId = GroupId;
            //Players = PlayerIDs;
            foreach (long l in PlayerIDs)
            {
                Player p = new Player();
                p.Id = l;
                p.Level = 1;
                p.AttackValue = 0;
                Players.Add(p);
            }

            StartRecieving();

            Bot.OnMessage += Bot_OnMessage;

            Console.WriteLine("Players:");
            foreach (Player p in Players)
            {
                Console.WriteLine("PlayerId: {0}, Playerlevel: {1}", p.Id, p.Level);
                
            }

            DoorStack = new Stack(DoorPile, StackType.Door);
            DoorStack.SendMessage += SendMessage;
            DoorStack.Shuffle();
            TreasureStack = new Stack(TreasurePile, StackType.Treasure);
            TreasureStack.SendMessage += SendMessage;
            TreasureStack.Shuffle();
            
            SendStartingMessage();

        }

        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
        }

        private void NextPlayer(Player p)
        {
            ActiveDoorCard = DoorStack.TakeFirst();
            p.ComputeAttackValue();

            //room for special cards like 

        }

        private void SendStartingMessage()
        {
            Console.WriteLine("sending game started messages");
            foreach (Player p in Players)
            {
                Console.WriteLine("To: {0}",p.Id);
                Bot.SendTextMessageAsync(p.Id, "Hallo! Willkommen zum MunchkinBot. Du hast erfolgreich das Spiel betreten, welches soeben gestartet ist!").Wait();
            }
        }

        private bool StartRecieving()
        {
            try
            {
                var subkey = Registry.CurrentUser.CreateSubKey("MunchkinBot", true);
                if (subkey.GetValue("Token") == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\nKein gespeichertes Telegram-Bot-Token gefunden. Bitte eingeben:\n");
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Token = Console.ReadLine();
                    subkey.SetValue("Token", Token);
                }
                else
                {
                    Token = (string)subkey.GetValue("Token");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("\n<Error> Fehler beim Lesen des Tokens! Fehler: {0}", ex);
                Token = "Fehler";
            }

            Console.Write("\n{0}\n", Token);

            if (Token == "Fehler")
            {
                started = false;
            }
            else
            {
                started = true;
                try
                {
                    Bot = new TelegramBot(Token);
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
    }
        #endregion
    
}
