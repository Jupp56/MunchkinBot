using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotApi;
using TelegramBotApi.Enums;
using TelegramBotApi.Types;
using TelegramBotApi.Types.Events;


namespace MunchkinBot.Classes
{
    public class Game
    {
        #region variables

        public long GroupId { get; set; }
        public List<Player> Players = new List<Player>();
        public Pile DoorPile = new Pile();
        public Pile TreasurePile = new Pile();
        public Stack DoorStack;
        public Stack TreasureStack;
        public event EventHandler<string> SendMessage;
        private static TelegramBot Bot; //do we do this from here?
        private Card ActiveDoorCard = new Card();

        #endregion

        #region constructor

        public Game(List<Player> PlayerIDs)
        {
            
            DoorStack = new Stack(DoorPile, StackType.Door);
            DoorStack.SendMessage += SendMessage;
            DoorStack.Shuffle();
            TreasureStack = new Stack(TreasurePile, StackType.Treasure);
            TreasureStack.SendMessage += SendMessage;
            TreasureStack.Shuffle();
            Players = PlayerIDs;
            SendStartingMessage();

        }

        private void NextPlayer(Player p)
        {
            ActiveDoorCard = DoorStack.TakeFirst();
            p.ComputeAttackValue();

            //room for special cards like 

        }

        private void SendStartingMessage()
        {
            foreach (Player p in Players) //does it work like this?
            {
                Bot.SendTextMessageAsync(p.Id.ToString(), "Hallo! Willkommen zum MunchkinBot. Du hast erfolgreich das Spiel betreten!"); //text is subject to change
            }
        }

        #endregion
    }
}
