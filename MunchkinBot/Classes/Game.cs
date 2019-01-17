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
    public class PlayersandModifiers
    {
        public Player p;
        public int Modifier;
    }

    public class Game
    {
        #region variables

        public static long GroupId { get; set; }
        public static string Token { get; set; }
        public static List<Player> Players = new List<Player>();
        public static Pile DoorPile = new Pile(); //ablegestapel    
        public static Pile TreasurePile = new Pile(); //ablegestapel
        public static Stack DoorStack; //nazistapel
        public static Stack TreasureStack; //nazistapel
        public event EventHandler<string> SendMessage;
        private static TelegramBot Bot; //do we do this from here?
        private static Card ActiveDoorCard = new Card();
        private static bool started;
        private static string botUsername;
        private static Random r = new Random();
        
        private static Player ActivePlayer;
        public static GameState state = GameState.NextPlayer;
        private static List<Player> OtherPlayersFighting;
        private static List<Card> ActiveMonsters = new List<Card>();

        
        public enum GameState
        {
            NextPlayer,
            DoorEntered,
            Fight1,
            AfterFight1,
            Fight2

        }
        

        #endregion

        #region constructor
        //List<long> PlayerIDs, long GroupID

        public Game(List<long> PlayerIDs, long GroupID)
        {
            Console.WriteLine("gamestarted");
            //List<long> PlayerIDs = new List<long>();
            //long GroupID = 0;
            Console.WriteLine("starting new game");
            GroupId = GroupID;
            //Players = PlayerIDs;
            foreach (long l in PlayerIDs)

            {
                Player p = new Player
                {
                    Id = l,
                    Level = 1,
                    AttackValue = 0
                };

                Players.Add(p);
            }

            ActivePlayer = Players[0];

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
            DoorStack.SendMessage += SendToAll;
            TreasureStack = new Stack(TreasurePile, StackType.Treasure);
            TreasureStack.SendMessage += SendMessage;
            TreasureStack.Shuffle();           
            TreasureStack.SendMessage += SendToAll;

            SendStartingMessage();
            SendToAll(ActivePlayer.Name + " fängt an!");

            loop:

            string command = Console.ReadLine();
            if (command != "/stop")
            {
                switch (command)
                {
                    case "/showplayers":
                        foreach (Player p in Players)
                        {
                            p.ComputeAttackValue();
                            Console.WriteLine("ID: {0}\nName: {1}\nLevel: {2}\nAttackValue: {3}",p.Id,p.Name,p.Level,p.AttackValue);
                            Console.WriteLine(Players[AnyPlayersIndex(p.Id)].Level);
                        }
                        goto loop;
                        
                }

                goto loop;
            }
            
        }

        #endregion
        
        #region Message Handler
        //Message Handler
        
        private static void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            //check if its the right group
            if (e.Message.Chat.Id == GroupId)
            {

                List<string> arguments = new List<string>(e.Message.Text.Split(' '));
                string command = arguments[0];
                arguments.RemoveAt(0);
                Player messagesender = GetPlayerObjectbyId(e.Message.From.Id);

                #region NextPlayer

                //for every command that should only work in NextPlayer gamestate
                if (state == GameState.NextPlayer)
                {

                    switch (command)                     
                    {
                        case "/Türeintreten":
                        
                            if (e.Message.From.Id == ActivePlayer.Id)
                            {
                                state = GameState.DoorEntered;
                                Nextplayer();
                            }
                            else
                            {
                                Bot.SendMessageAsync(e.Message.From.Id, "Du bist nicht an der Reihe!");
                            }
                            break;
                    }

                }
                #endregion

                #region Fight

                //for every command in fight game states
                if (state == GameState.Fight1 || state == GameState.Fight2)
                {

                    switch (command)
                    {
                        case "/joinfight":

                            OtherPlayersFighting.RemoveAll(x => x == GetPlayerObjectbyId(e.Message.From.Id));
                            OtherPlayersFighting.Add(GetPlayerObjectbyId(e.Message.From.Id));
                            break;

                        case "/fight":

                            if (e.Message.From.Id == ActivePlayer.Id)
                            {
                                Fight();
                            }
                            break;

                        case "/Monstereinwerfen":

                            foreach (string m in arguments)
                            {
                                if (messagesender.Hand.Find(x => x.Name == m) != null)
                                {
                                    //needs testing

                                    Card c = messagesender.Hand.Find(x => x.Name == m);
                                    if (c.Type == Card.CardType.Monster)
                                    {
                                        Players[AnyPlayersIndex(messagesender.Id)].Hand.Remove(c);
                                        ActiveMonsters.Add(c);
                                    }
                                    else
                                    {
                                        SendToOne(messagesender, "Diese Karte kannst du nicht spielen");
                                    }
                                }
                            }
                            
                            break;

                    }

                }

                #endregion
            }

        }

        #endregion

        #region Game Progress

        //Calling the next player to kick a door
        private static void Nextplayer()
        {
            state = GameState.NextPlayer;
            ActivePlayer = Players[NextPlayersIndex()];
            
            ActiveDoorCard = DoorStack.TakeFirst();
            SendToAll(ActivePlayer.Name + " ist an der Reihe! Er tritt eine Tür ein! Es erwartet ihn: " + ActiveDoorCard.Name);
            //here:Send image of said card

            if (ActiveDoorCard.Type == Card.CardType.Monster) //we NEED some clarification about that types! 0 = monster seems good enough, doesnt it?
            {
                //prepareforfight
                SendToAll("Du musst jetzt kämpfen, " + ActivePlayer.Name + "! Du kannst dir Hilfe holen. Andere Spieler können mit /joinfight dem Kampf beitreten, und helfen. Aber sie können auch Böse sein: sie können dem Kampf ein Monster hinzufügen oder Ereigniskarten auf Spieler wirken! Dazu die Karte mit /playcard <Kartenname> ausspielen.");
            }

            if (ActiveDoorCard.Type == Card.CardType.Clothing || ActiveDoorCard.Type == Card.CardType.Weapon || ActiveDoorCard.Type == Card.CardType.Companion) Players[ThisPlayersIndex()].Hand.Add(ActiveDoorCard);
            
            if (ActiveDoorCard.Type == Card.CardType.Curse)
            {
                //whatever happens then... To be implemented
            }
            
            //not here: ActivePlayer.ComputeAttackValue();
        }


        #endregion

        #region Mechanics

        //called by players message when he is ready figuring out his attack with the other players
        //to implement: curses
        private static void Fight()
        {
            int combinedAttackValue = 0;

            int MonsterValue = 0;

            bool haswon = false;

            ActiveMonsters.Add(ActiveDoorCard);

            ActivePlayer.ComputeAttackValue();

            combinedAttackValue += ActivePlayer.AttackValue;

            foreach (Player pl in OtherPlayersFighting)
            {
                pl.ComputeAttackValue();
                combinedAttackValue += pl.AttackValue;
            }

            if (combinedAttackValue >= MonsterValue)
            {
                haswon = true;
            }
            else
            {
                haswon = false;
            }

            //below this in this method only: reset variables to standard values
            if (state == GameState.Fight2 && haswon == true) state = GameState.NextPlayer;
            if (state == GameState.Fight1 && haswon == true) state = GameState.Fight2;


            List<PlayersandModifiers> playersandModifiers = new List<PlayersandModifiers>();

            if (haswon == false)
            {
                //Insert schlimme Dinge actions here of every player participating
            }
            if (haswon == true)
            {
               //Insert levelups here
               //Insert Treasure Cards here
            }

            foreach (PlayersandModifiers playerwithmodifier in playersandModifiers)
            {
                Players[AnyPlayersIndex(playerwithmodifier.p.Id)].Level += playerwithmodifier.Modifier;
            }

            foreach(Card c in ActiveMonsters)
            {
                DoorPile.Add(c);
            }
            playersandModifiers = null;
            ActiveDoorCard = null;
            OtherPlayersFighting = null;
            ActiveMonsters = null;

            
            
        }

        //checks if someone has won
        private static bool Winningcondition()
        {
            foreach (Player p in Players)
            {
                if (p.Level >= 10) return true;
            }
            return false;
        }

        private static void Calculateloosing()
        {

        }

        #endregion

        #region Message sending methods
        

        private static void SendToAll(string message)
        {           
            Bot.SendMessageAsync(GroupId, message);

        }

        static void SendToAll(object sender, string s)
        {
            SendToAll(s);
        }

        private static void SendToOne(Player p, string message)
        {
            Bot.SendMessageAsync(p.Id, message);
        }

        private static void SendToGroup(List<Player> pl, string message)
        {
            foreach(Player p in pl)
            {
                Bot.SendMessageAsync(p.Id, message);
            }
        }
        
        private void SendStartingMessage()
        {
            Console.WriteLine("sending game started messages");
            foreach (Player p in Players)
            {
                Console.WriteLine("To: {0}",p.Id);
                Bot.SendMessageAsync(p.Id, "Hallo! Willkommen zum MunchkinBot. Du hast erfolgreich das Spiel betreten, welches soeben gestartet ist!");
            }
        }

        #endregion

        #region little helpers

        //helps Nextplayer() calculate the next players ID
        private static int NextPlayersIndex()
        {
            int index = Players.FindIndex(x => x.Id == ActivePlayer.Id) + 1;
            if (index > Players.Count - 1) index = 0;
            return index;

        }

        //returns active players index in players list 
        private static int ThisPlayersIndex()
        {
            int index = Players.FindIndex(x => x.Id == ActivePlayer.Id);
            return index;
        }

        private static int AnyPlayersIndex(long Id)
        {
            int index = Players.FindIndex(x => x.Id == Id);
            return index;
        }
        private static Player GetPlayerObjectbyId(long id)
        {
            Player p = Players[AnyPlayersIndex(id)];
            return p;
        }

        #endregion

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


}
